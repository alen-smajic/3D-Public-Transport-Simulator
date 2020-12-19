using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
    Copyright (c) 2020 Alen Smajic

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

/// <summary>
/// This script generates all railroads, station and roads using the information
/// from the MapReader script. The generation oof the 3D buildings is also here
/// instantiated. The progress is shown by a loading window with percentage.
/// </summary>
class MapBuilder : MonoBehaviour
{
    public GameObject StationPrefab; // Station objects.
    public GameObject StationUIPrefab; // Station-UI.
    public GameObject buildings; // Mapbox 3D buildings.
    public GameObject roads; // Mapbox map.
    public GameObject InGameLoadingScreen; // Shows the loading percentage.
    public GameObject InGameLoadingMessage; // Shows loading message.
    public GameObject InGameLoadingWindow; // Show loading windows.

    // Here are the various public transport colors stored which are used
    // upon generating the roads and railroads.
    public Material bus_streets;
    public Material public_transport_railways;
    public Material subways;
    public Material trams;
    public Material trains;
    public Material railway;
    public Material light_rails;
    public static Material selected_way;

    Material inUse; 

    bool StationsCreated = false;

    // This is used for the loading percentage window.
    float processed_items = 0f;
    int percentageAmount = 0;

    /// <summary>
    /// This function waits for the MapReader script to process all the information.
    /// As soon as this is done, the MapReader script will trigger this function to
    /// start the process of scene building.
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        if (!UserPreferences.Stations)
        { 
            StationsCreated = true;
        }
        else
        {
            StationBuilder(); // Stations are being built.
        }

        while (!MapReader.IsReady || !StationsCreated)
        {
            yield return null;
        }

        if (UserPreferences.Buildings)
        {
            // 3D buildings are being instantiated.
            GameObject MapboxBuildings = Instantiate(buildings);
            MapboxBuildings.transform.localScale = new Vector3(1.223f, 1.22264f, 1.219f);
            MapboxBuildings.transform.localPosition = new Vector3(0, -0.1f, 0);
        }
        if (UserPreferences.AllStreets)
        {
            // The map is being instantiated.
            GameObject MapboxRoads = Instantiate(roads);
            MapboxRoads.transform.localScale = new Vector3(1.223f, 1.22264f, 1.219f);
            MapboxRoads.transform.localPosition = new Vector3(0, -0.1f, 0);
        }
        if(UserPreferences.PublicTransportRailways || UserPreferences.PublicTransportStreets)
        {
            StartCoroutine(WayBuilder()); // Roads and railroads are being instantiated.
        }
    }

    /// <summary>
    /// We iterate over all relation instances to generate the station objects.
    /// </summary>
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

    /// <summary>
    /// We iterate over the relation instances to generate the Station objects and
    /// the corresponding station UIs.
    /// </summary>
    /// <param name="r">relation instance</param>
    void CreateStations(OsmRelation r)
    {       
        foreach (ulong NodeID in r.StoppingNodeIDs)
        {
            GameObject station_object;
            GameObject stationUI_object;

            try
            {
                // If a station has already been created using a node position, a new station will not
                // be created there but the existing one will be augmented with the new information. This 
                // avoids the case of multiple overlapping station object at one point.
                if (MapReader.nodes[NodeID].StationCreated == true)
                {
                    List<GameObject> allObjects = new List<GameObject>();
                    Scene scene = SceneManager.GetActiveScene();
                    scene.GetRootGameObjects(allObjects);

                    // We iterate over all Unity objects to find the station that has
                    // already been generated at the certain point.
                    for (int i = 5; i < allObjects.Count; i++)
                    {
                        if(allObjects[i].transform.position == MapReader.nodes[NodeID] - MapReader.bounds.Centre)
                        {
                            bool doubleFound = false;

                            // Here we check if the new information that is being added to the station object,
                            // has not already been stored inside the station object. This operation can be traced
                            // back to a bug which I encountered during development, where the same information
                            // was stored multiple times. The reason for this is still unclear, this is a workaround.
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
                            if (!doubleFound)  // If no information is found, we add the new information here.
                            {
                                dropOptions.AddOptions(MapReader.nodes[NodeID].TransportLines);
                                if (r.TransportType == "bus")
                                {
                                    // Activates the bus symbol in the UI.
                                    allObjects[i].transform.GetChild(0).GetChild(0).transform.GetChild(4).gameObject.SetActive(true);
                                }
                                else
                                {
                                    // Activates the train symbol on the UI.
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

                // Is being set so that on this position, no new stations are being generated.
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
                    // Activates the bus symbol on the UI.
                    stationUI_object.transform.GetChild(4).gameObject.SetActive(true);
                }
                else
                {
                    // Activates the train symbol on the UI.
                    stationUI_object.transform.GetChild(5).gameObject.SetActive(true);
                }
            }
            catch (KeyNotFoundException)
            {
                continue;
            }
        }
    }

    /// <summary>
    /// This function generates the road and railrooads. It also shows a loading
    /// screen with the progress percentage.
    /// </summary>
    /// <returns></returns>
    IEnumerator WayBuilder()
    {
        float TargetCount = MapReader.ways.Count;
        Text PercentageDisplayer = InGameLoadingScreen.GetComponent<Text>();

        InGameLoadingWindow.SetActive(true);

        foreach (KeyValuePair<ulong, OsmWay> w in MapReader.ways)
        {
            // A simple loading progress logic, which returns the amount
            // of processed items divided by the total number of items.
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

            // Here we start the process of road/rail road instantiation.
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

            // Here we store the vectors, normales and indexes.
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

                // The width of road and railroads is set to 1 meter.
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

                indicies.Add(idx1);
                indicies.Add(idx3);
                indicies.Add(idx2);

                indicies.Add(idx3);
                indicies.Add(idx4);
                indicies.Add(idx2);
            }
            go.name += w.Value.ID;
            mf.mesh.vertices = vectors.ToArray();
            mf.mesh.normals = normals.ToArray();
            mf.mesh.triangles = indicies.ToArray();

            yield return null;

            // Lastly we store the Unity coordinates of every generated way. This is then used later on
            // when we want to move the transport vehicles across the ways.
            for(int i = 0; i < w.Value.NodeIDs.Count; i++)
            {
                OsmNode p1 = MapReader.nodes[w.Value.NodeIDs[i]];
                w.Value.UnityCoordinates.Add(p1 - MapReader.bounds.Centre);
            }
        }
    }

    /// <summary>
    /// Returns the center point of an object. This information is being used as the reference
    /// for placing the object inside the Unity world.
    /// </summary>
    /// <param name="way">way instance</param>
    /// <returns></returns>
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