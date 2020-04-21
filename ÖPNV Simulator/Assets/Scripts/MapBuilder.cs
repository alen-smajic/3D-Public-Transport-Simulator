using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Dieses Skript erzeugt anhand der Datenstrukturen aus dem Skript MapReader
// alle Schienen und Stationen. Die Erzeugung der 3D-Gebäude und Straßen wird
// ebenfalls veranlasst. Der Fortschritt wird anhand eines Ladefenster eingeblendet.
class MapBuilder : MonoBehaviour
{
    public GameObject StationPrefab; // Stationsobjekt.
    public GameObject StationUIPrefab; // Stations-UI.
    public GameObject buildings; // Mapbox 3D-Gebäude.
    public GameObject roads; // Mapbox Straßen.
    public GameObject InGameLoadingScreen; // Zeigt den Ladeprozentsatz an.
    public GameObject InGameLoadingMessage; // Zeigt die Ladebenachrichtigung an.
    public GameObject InGameLoadingWindow; // Zeigt das komplette Ladefenster an.

    // Hier sind die verschiedenen Farben für die Straßen und Schienen 
    // hinterlegt. Werden bei der Generierung zugewiesen.
    public Material bus_streets;
    public Material public_transport_railways;
    public Material subways;
    public Material trams;
    public Material trains;
    public Material railway;
    public Material light_rails;
    public static Material selected_way;

    Material inUse; // Aktuell zugewiesenes Material.

    // Nachdem die Stationen generiert wurden, können die Straßen und Schienen
    // generiert werden da es hier Informationsabhängigkeiten gibt.
    bool StationsCreated = false;

    // Dieser Abschnitt wird für die Ladeprozentberechnung genutzt.
    float processed_items = 0f;
    int percentageAmount = 0;

    IEnumerator Start()
    {
        if (!UserPreferences.Stations)
        { 
            StationsCreated = true;
        }
        else
        {
            StationBuilder(); // Stationen werden erzeugt.
        }

        while (!MapReader.IsReady || !StationsCreated)
        {
            yield return null;
        }

        if (UserPreferences.Buildings)
        {
            // Falls 3D-Gebäude gewünscht, werden Sie hier erzeugt.
            GameObject MapboxBuildings = Instantiate(buildings);
            MapboxBuildings.transform.localScale = new Vector3(1.223f, 1.22264f, 1.219f);
            MapboxBuildings.transform.localPosition = new Vector3(0, -0.1f, 0);
        }
        if (UserPreferences.AllStreets)
        {
            // Falls Straßen gewünscht, werden Sie hier erzeugt.
            GameObject MapboxRoads = Instantiate(roads);
            MapboxRoads.transform.localScale = new Vector3(1.223f, 1.22264f, 1.219f);
            MapboxRoads.transform.localPosition = new Vector3(0, -0.1f, 0);
        }
        if(UserPreferences.PublicTransportRailways || UserPreferences.PublicTransportStreets)
        {
            StartCoroutine(WayBuilder()); // Schienen und Busstraßen werden erzeugt.
        }
    }


    // Hier wird über alle Relationen iteriert, um die Stationen zu erstellen.
    void StationBuilder()
    {
        for(int i=0; i<MapReader.relations.Count; i++)
        {
            switch (MapReader.relations[i].TransportType)
            {
                case "subway":
                    if (UserPreferences.Subways == true)
                    {
                        CreateStations(MapReader.relations[i]);
                    }
                    break;
                case "tram":
                    if (UserPreferences.Trams == true)
                    {
                        CreateStations(MapReader.relations[i]);
                    }
                    break;
                case "train":
                    if (UserPreferences.Trains == true)
                    {
                        CreateStations(MapReader.relations[i]);
                    }
                    break;
                case "railway":
                    if (UserPreferences.Railways == true)
                    {
                        CreateStations(MapReader.relations[i]);
                    }
                    break;
                case "light_rail":
                    if (UserPreferences.LightRails == true)
                    {
                        CreateStations(MapReader.relations[i]);
                    }
                    break;
                case "bus":
                    if (UserPreferences.Busses == true)
                    {
                        CreateStations(MapReader.relations[i]);
                    }
                    break;
            }
        }
        StationsCreated = true;
    }

    // Falls Stationen gewünscht sind, wird diese Funktion aufgerufen. Anhand
    // der Relation wird dann ein Stationsobjekt in der Unity Welt erstellt
    // und ein UI mit der jeweiligen Station gekoppelt welches Informationen
    // über die Station enthält.
    void CreateStations(OsmRelation r)
    {       
        foreach (ulong NodeID in r.StoppingNodeIDs)
        {
            GameObject station_object;
            GameObject stationUI_object;

            try
            {
                // Falls anhand eines Nodes eine Station bereits angelegt wurde,
                // wird dort nicht nochmal eine neue Station angelegt, sondern
                // die bestehende wird mit den neuen Informationen angereichert.
                // Somit vermeidet man den Fall, an einem Punkt mehrfach überlappende
                // Stationen zu haben.
                if (MapReader.nodes[NodeID].StationCreated == true)
                {
                    List<GameObject> allObjects = new List<GameObject>();
                    Scene scene = SceneManager.GetActiveScene();
                    scene.GetRootGameObjects(allObjects);

                    // Es wird über alle Unity Objekte iterriert um die bereits
                    // angelegte Station zu finden.
                    for (int i = 5; i < allObjects.Count; i++)
                    {
                        if(allObjects[i].transform.position == MapReader.nodes[NodeID] - MapReader.bounds.Centre)
                        {
                            bool doubleFound = false;

                            // Hier wird überprüft ob die Informationen die hinzugefügt werden soll
                            // evt. schon in der Station entahlten sind (ist auf einen Bug zurückzuführen
                            // wo viele gleiche Informationen gespeichert wurden). Ursache ist derzeit nicht
                            // bekannt. Dies ist ein Workaround.
                            GameObject Dropdown = allObjects[i].transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject;
                            var dropOptions = Dropdown.GetComponent<Dropdown>();
                            for(int j = 0; j < dropOptions.options.Count; j++)
                            {
                                for(int k = 0; k < MapReader.nodes[NodeID].TransportLines.Count; k++)
                                {
                                    if(dropOptions.options[j].text == MapReader.nodes[NodeID].TransportLines[k])
                                    {
                                        doubleFound = true;
                                        continue;
                                    }
                                }
                            }
                            if (!doubleFound)  // Falls Infos nicht vorliegen, werden sie hier eingetragen.
                            {
                                dropOptions.AddOptions(MapReader.nodes[NodeID].TransportLines);
                                if (r.TransportType == "bus")
                                {
                                    // Aktiviert das Bussymbol in der UI.
                                    allObjects[i].transform.GetChild(0).GetChild(0).transform.GetChild(4).gameObject.SetActive(true);
                                }
                                else
                                {
                                    // Aktiviert das Zugsymbol in der UI.
                                    allObjects[i].transform.GetChild(0).GetChild(0).transform.GetChild(5).gameObject.SetActive(true);
                                }
                                continue;
                            }
                        }
                    }
                    continue;
                }

                station_object = Instantiate(StationPrefab) as GameObject;
                OsmNode new_station = MapReader.nodes[NodeID];
                Vector3 new_station_position = new_station - MapReader.bounds.Centre;
                station_object.transform.position = new_station_position;

                // Wird gesetzt damit auf dieser Position nicht nochmals eine Station 
                // platziert wird.
                MapReader.nodes[NodeID].StationCreated = true;


                stationUI_object = Instantiate(StationUIPrefab) as GameObject;
                stationUI_object.transform.SetParent(station_object.transform.GetChild(0));
                stationUI_object.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
                stationUI_object.transform.localPosition = new Vector3(-15, 0, 0);

                GameObject station_text = stationUI_object.transform.GetChild(1).gameObject;
                station_text.transform.localPosition = new Vector3(0, 7, 0);
                Text OnScreenText = station_text.GetComponent<Text>();
                OnScreenText.text = MapReader.nodes[NodeID].StationName;

                GameObject TransportLineDropdown = stationUI_object.transform.GetChild(2).gameObject;
                var dropDownOptions = TransportLineDropdown.GetComponent<Dropdown>();
                dropDownOptions.AddOptions(MapReader.nodes[NodeID].TransportLines);

                stationUI_object.SetActive(false);

                if(r.TransportType == "bus")
                {
                    // Aktiviert das Bussymbol der UI.
                    stationUI_object.transform.GetChild(4).gameObject.SetActive(true);
                }
                else
                {
                    // Aktiviert das Zugsymbol der UI.
                    stationUI_object.transform.GetChild(5).gameObject.SetActive(true);
                }
            }
            catch (KeyNotFoundException)
            {
                continue;
            }
        }
    }

    // Diese Funktion dient der Generierung, der Straßen/Schienen.
    // Ein Ladefenster ist implementiert welches den aktuellen Stand der 
    // Durchläufe wiederspiegelt.
    IEnumerator WayBuilder()
    {
        float TargetCount = MapReader.ways.Count;
        Text PercentageDisplayer = InGameLoadingScreen.GetComponent<Text>();

        InGameLoadingWindow.SetActive(true);

        foreach (KeyValuePair<ulong, OsmWay> w in MapReader.ways)
        {
            // Hier ist ein einfacher Ladeprozentsatz implementiert, welcher
            // anhand der aktuellen Durchlaufzahl berechnet wurde.
            processed_items += 1;
            float loadingPercentage = processed_items / TargetCount * 100;

            if(loadingPercentage < 99) 
            {
                if(loadingPercentage > percentageAmount)
                {
                    percentageAmount += 1;
                    PercentageDisplayer.text = percentageAmount.ToString() + "%";
                }
            }
            else
            {
                InGameLoadingScreen.SetActive(false);
                InGameLoadingMessage.SetActive(false);
                InGameLoadingWindow.SetActive(false);
            }

            // Ab hier werden die Straßen/Schienen generiert.
            if (w.Value.PublicTransportStreet)
            {
                if (UserPreferences.PublicTransportStreets)
                {
                    inUse = bus_streets;
                }
                else
                {
                    inUse = null;
                    continue;
                }
            }
            else if (w.Value.PublicTransportRailway)
            {
                if (UserPreferences.PublicTransportRailways)
                {
                    if(!UserPreferences.Subways && !UserPreferences.Trams && !UserPreferences.Trains && !UserPreferences.Railways && !UserPreferences.LightRails)
                    {
                        inUse = public_transport_railways;
                    }
                    else if (w.Value.TransportTypes.Contains("subway"))
                    {
                        if (UserPreferences.Subways)
                        {
                            inUse = subways;
                        }
                        else
                        {
                            inUse = null;
                            continue;
                        }
                    }
                    else if (w.Value.TransportTypes.Contains("tram"))
                    {
                        if (UserPreferences.Trams)
                        {
                            inUse = trams;
                        }
                        else
                        {
                            inUse = null;
                            continue;
                        }
                    }
                    else if (w.Value.TransportTypes.Contains("train"))
                    {
                        if (UserPreferences.Trains)
                        {
                            inUse = trains;
                        }
                        else
                        {
                            inUse = null;
                            continue;
                        }
                    }
                    else if (w.Value.TransportTypes.Contains("railway"))
                    {
                        if (UserPreferences.Railways)
                        {
                            inUse = railway;
                        }
                        else
                        {
                            inUse = null;
                            continue;
                        }
                    }
                    else if (w.Value.TransportTypes.Contains("light_rail"))
                    {
                        if (UserPreferences.LightRails)
                        {
                            inUse = light_rails;
                        }
                        else
                        {
                            inUse = null;
                            continue;
                        }
                    }
                    else
                    {
                        inUse = null;
                        continue;
                    }
                    
                }
            }
            else
            {
                inUse = null;
                continue;
            }
                                             
            GameObject go = new GameObject();
            var waytext = go.AddComponent<Text>();
            foreach (string tramline in w.Value.TransportLines)
            {
                waytext.text += tramline + ", ";
            }
            Vector3 localOrigin = GetCentre(w.Value);
            go.transform.position = localOrigin - MapReader.bounds.Centre;

            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();

            mr.material = inUse;

            // Hier werden die Vektoren, Normalen und Indices gespeichert.
            List<Vector3> vectors = new List<Vector3>();  
            List<Vector3> normals = new List<Vector3>();
            List<int> indicies = new List<int>();

            for (int i = 1; i < w.Value.NodeIDs.Count; i++)
            {
                OsmNode p1 = MapReader.nodes[w.Value.NodeIDs[i - 1]];
                OsmNode p2 = MapReader.nodes[w.Value.NodeIDs[i]];

                Vector3 s1 = p1 - localOrigin;  
                Vector3 s2 = p2 - localOrigin;

                Vector3 diff = (s2 - s1).normalized;

                // Hier wird die Breite eines Weges auf einen Meter gesetzt.
                var cross = Vector3.Cross(diff, Vector3.up) * 1.0f; 

                Vector3 v1 = s1 + cross;
                Vector3 v2 = s1 - cross;
                Vector3 v3 = s2 + cross;
                Vector3 v4 = s2 - cross;

                vectors.Add(v1);  
                vectors.Add(v2);
                vectors.Add(v3);
                vectors.Add(v4);

                normals.Add(Vector3.up);  
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);

                int idx1, idx2, idx3, idx4;
                idx4 = vectors.Count - 1;
                idx3 = vectors.Count - 2;
                idx2 = vectors.Count - 3;
                idx1 = vectors.Count - 4;

                // Die Indizes müssen im Uhrzeigersinn und gegen den Uhrzeigersinn gehen. Somit werden 2 Dreiecke erstellt 
                // welche von jeder Seite aus betrachtet nicht durchsichtig sind. Erstes Dreieck v1, v3, v2.
                indicies.Add(idx1);
                indicies.Add(idx3);
                indicies.Add(idx2);

                // Zweites Dreieck v3, v4, v2.
                indicies.Add(idx3);
                indicies.Add(idx4);
                indicies.Add(idx2);
            }
            go.name += w.Value.ID;
            mf.mesh.vertices = vectors.ToArray();
            mf.mesh.normals = normals.ToArray();
            mf.mesh.triangles = indicies.ToArray();

            yield return null;

            // Am Ende wird jeder Weg anhand seiner Unity Koordinaten festgehalten. Somit können diese später für
            // die durchfahrenden Züge und Busse genutzt werden.
            for(int i = 0; i < w.Value.NodeIDs.Count; i++)
            {
                OsmNode p1 = MapReader.nodes[w.Value.NodeIDs[i]];
                w.Value.UnityCoordinates.Add(p1 - MapReader.bounds.Centre);
            }
        }
    }

    // Diese Funktion gibt den zentralen Punkt eines Objekts zurück (eines Ways). 
    // Wird als Referenzpunkt genutzt für das Platzieren der Objekte.
    protected Vector3 GetCentre(OsmWay way)  
    {
        Vector3 total = Vector3.zero;

        foreach (var id in way.NodeIDs)
        {
            total += MapReader.nodes[id];  
        }
        return total / way.NodeIDs.Count;
    }  
}