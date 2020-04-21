using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

// Nachdem eine Zug- Buslinie ausgewählt wurde wird das Objekt WayOrderManager ins Leben gerufen, welches diese Klasse als Skript enthält. Ziel dieser
// Klasse ist es eine Liste aller Node Koordinaten zurückzugeben damit diese für die Züge/Busse als Fahrpunkte genutzt werden können. Dazu müssen anhand
// der SelectedWays Liste aus "TranSportWayMarker" die entsprechenden Way Objekte ermittelt werden und ihre Zusammenstellung aus Node Koordinaten. 
// Wenn alle Koordinaten ermittelt wurden, müssen diese als nächstes geordnet werden um korrekt befahren zu werden. Am Ende werden hier die Züge bzw. Busse
// instanziert und der entsprechende Weg wird übergeben.
public class SortWay : MonoBehaviour
{
    public List<List<Vector3>> allWayCoordinates; // Äußere Liste = Way Objekte, innere Liste = Node Koordinaten aus welchen der Way zusammengesetzt ist.
    public static List<List<List<Vector3>>> SortedPathAndWays; // Äußere Liste = Pfade (keine Zusammenhängenden Ways), mittlere Liste = Way Objekte welche anahnd 
                                                                // anhand des Zusammenhangs sortiert sind, innere Liste = Node Koordinaten aus welchen der Way zusammengesetzt ist.
    public static List<List<Vector3>> SplittedinPaths;
    public static List<List<Vector3>> PathsInRightOrder;

    public static List<Vector3> MoveToTarget;
    public static List<Vector3> PathLastNode;

    private void Start()
    {
        allWayCoordinates = new List<List<Vector3>>();
        SortedPathAndWays = new List<List<List<Vector3>>>();
        SplittedinPaths = new List<List<Vector3>>();
        PathsInRightOrder = new List<List<Vector3>>();

        MoveToTarget = new List<Vector3>();
        PathLastNode = new List<Vector3>();

        List<GameObject> allObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(allObjects);

        // Nach dieser for Schleife wird eine Liste "allWayCoordinates" zurückgegeben welche mehrere Liste enthält. Jede dieser Listen
        // sind Way Objekte welche wiederum die Node Koordinaten als Einträge enthält.
        for (int i = 0; i < TranSportWayMarker.SelectedWays.Count; i++)
        {
            string ObjectName = allObjects[TranSportWayMarker.SelectedWays[i]].name;
            string ObjectID = ObjectName.Substring(15); // ID des Ways wird ermittelt um im MapReader Skript zu suchen.

            List<Vector3> WayNodes = new List<Vector3>(); // Lokale Liste zur Zwischenspeicherung
            for (int j = 0; j < MapReader.ways[Convert.ToUInt64(ObjectID)].UnityCoordinates.Count; j++)
            {
                WayNodes.Add(MapReader.ways[Convert.ToUInt64(ObjectID)].UnityCoordinates[j]);
            }
            allWayCoordinates.Add(WayNodes); // Nach jedem Durchgang wird ein Way in Form einer Liste der Node-Koordinaten hier abgelegt.
            // Am Ende ist in allWayCoordinates eine Menge von Listen, wobei jede Liste einen Way darstellt und mit Nodekoordinaten befüllt ist.
        }

        // Als nächstes wird eine neue List (SortedPathAndWays) erzeugt. Diese hat das Ziel zusammenhängende Ways in einer Liste abzulegen.
        // Falls sich z.B. eine Buslinie am Rand befindet und dort endet, jedoch dann an einem anderem Rand wieder anfängt werden hier die entsprechenden
        // Ways in einer neuen Liste festgehalten. 
        // Um dies umzusetzen wird als erstes der erste Way in die Liste eingefügt und als Referenz für den Zusammenhang genutzt.
        List<List<Vector3>> localList = new List<List<Vector3>>();
        localList.Add(allWayCoordinates[0]);
        SortedPathAndWays.Add(localList);
        allWayCoordinates.RemoveAt(0);

        // Wird aufgerufen um anhand der Liste allWayCoordinates die Liste SortedPathAndWays zu sortieren und mit Koordinaten zu füllen.
        SortList(allWayCoordinates, 0);

        // In diesem Abschnitt wird aus der dreidimensionalen SortedPathAndWays Liste eine zweidimensionale Liste. Dabei werden die inneren Listen
        // miteinander "verschmolzen" da diese schon korrekt sortiert und zusammenhängend sind. Am Ende gibt es nur noch eine Differenzierung 
        // und dies ist auf den Pfad zurückzuführen.
        List<Vector3> temporaryList = new List<Vector3>();
        for(int i = 0; i < SortedPathAndWays.Count; i++) 
        {
            temporaryList = SortedPathAndWays[i].SelectMany(x => x).ToList();
            SplittedinPaths.Add(temporaryList);
        }

        // Sortiert Paths in der Reihenfolge anhand der stationen
        for(int i = 0; i < TranSportWayMarker.StationOrder.Count; i++)
        {
            for(int j = 0; j < SplittedinPaths.Count; j++)
            {
                for(int k = 0; k < SplittedinPaths[j].Count; k++)
                {
                    if(TranSportWayMarker.StationOrder[i] == SplittedinPaths[j][k])
                    {
                        if (PathsInRightOrder.Contains(SplittedinPaths[j]))
                        {
                            break;
                        }
                        else
                        {
                            PathsInRightOrder.Add(SplittedinPaths[j]);
                        }
                    }
                }
            }
        }

        // fügt pahts welche keine station haben am Ende hinzu
        for(int i = 0; i < SplittedinPaths.Count; i++)
        {
            if (!PathsInRightOrder.Contains(SplittedinPaths[i]))
            {
                PathsInRightOrder.Add(SplittedinPaths[i]);
            }
        }

        //Ändert die Richtung der Objekte innerhalb der Paths anhand der Stationen
        int firstIndex = -1;
        int secondIndex = -1;
        for(int i = 0; i < TranSportWayMarker.StationOrder.Count; i++)
        {
            for(int k = 0; k < PathsInRightOrder.Count; k++)
            {
                for(int j = 0; j < PathsInRightOrder[k].Count; j++)
                {
                    if(TranSportWayMarker.StationOrder[i] == PathsInRightOrder[k][j])
                    {
                        if(firstIndex == -1)
                        {
                            firstIndex = j;
                            break;
                        }
                        else
                        {
                            secondIndex = j;
                            break;
                        }
                    }
                }
                if(firstIndex != -1 && secondIndex != -1)
                {
                    if(firstIndex > secondIndex)
                    {
                        PathsInRightOrder[k].Reverse();
                        break;
                    }
                }
            }
        }

        for(int i = 0; i < PathsInRightOrder.Count; i++)
        {
            for(int j = 0; j < SortWay.PathsInRightOrder[i].Count; j++)
            {
                MoveToTarget.Add(SortWay.PathsInRightOrder[i][j]);
            }
        }

        if(SortWay.PathsInRightOrder.Count > 1)
        {
            for(int i = 0; i < PathsInRightOrder.Count; i++)
            {
                PathLastNode.Add(PathsInRightOrder[i][PathsInRightOrder[i].Count - 1]);
            }
        }

        IdentifyVehicle();
    }

    // Diese Funktion hat das Sortieren der Ways als Ziel, da diese nicht in der korrekten Reihenfolge 
    // in der Liste abgelegt sind. Dabei wurde vor dem Funktionsaufruf ein Element aus der Quellliste
    // allWayCoordinates in die Liste "SortedPathAndWays" abgelegt und wird als Referenz für den Zusammenhang
    // genutzt.
    void SortList(List<List<Vector3>> WaysToBeSorted, int index)
    {
        if(WaysToBeSorted.Count <= 0)
        {
            return;
        }

        List<List<Vector3>> LeftItems = WaysToBeSorted; // Die Liste LeftItems wird genutzt um die noch nicht eingeordneten Elemente zu verfolgen.
        bool found_something = false;

        for(int i = 0; i < WaysToBeSorted.Count; i++)
        {
            // Falls das erste Element der Liste WaysToBeSorted gleich dem letztem Element der SortedPathAndWays Liste ist, wird deie erste Liste aus WaysToBeSorted entnommen
            // und am Ende von SortedPathAndWays angehängt.
            if(WaysToBeSorted[i][0] == SortedPathAndWays[index][SortedPathAndWays[index].Count - 1][SortedPathAndWays[index][SortedPathAndWays[index].Count - 1].Count - 1])
            {
                SortedPathAndWays[index].Add(WaysToBeSorted[i]);
                LeftItems.RemoveAt(i);
                found_something = true;
            }
            else if (SortedPathAndWays[index][0][0] == WaysToBeSorted[i][WaysToBeSorted[i].Count - 1])
            {
               SortedPathAndWays[index].Insert(0, WaysToBeSorted[i]);
                LeftItems.RemoveAt(i);
                found_something = true;
            }
            else if(WaysToBeSorted[i][0] == SortedPathAndWays[index][0][0])
            {
                WaysToBeSorted[i].Reverse();
                SortedPathAndWays[index].Insert(0, WaysToBeSorted[i]);
                LeftItems.RemoveAt(i);
                found_something = true;
            }
            else if (WaysToBeSorted[i][WaysToBeSorted[i].Count - 1] == SortedPathAndWays[index][SortedPathAndWays[index].Count - 1][SortedPathAndWays[index][SortedPathAndWays[index].Count - 1].Count - 1])
            {
                WaysToBeSorted[i].Reverse();
                SortedPathAndWays[index].Add(WaysToBeSorted[i]);
                LeftItems.RemoveAt(i);
                found_something = true;
            }
            else
            {
                continue;
            }
        }

        if(found_something == false)
        {
            List<List<Vector3>> localList = new List<List<Vector3>>();
            localList.Add(WaysToBeSorted[0]);
            SortedPathAndWays.Add(localList);
            LeftItems.RemoveAt(0);
            SortList(LeftItems, index + 1);
        }
        else
        {
            SortList(LeftItems, index);
        }
    }

    // Diese Funktion erkennt anhand der zwischengespeicherten Farbe (welche in TransportWayMarker genutzt wird
    // für die Wiederherstellung des Weges in die korrekte Farbe) um welchen Transporttypen es sich handelt.
    // Anhand dieser Information wird die VehicleSpawner Coroutine aufgerufen um das Spawnen der Fahrzeuge
    // zu veranlassen.
    void IdentifyVehicle()
    {
        switch (ColorUtility.ToHtmlStringRGB(TranSportWayMarker.PreviousMaterial.color))
        {
            case "1400FF":
                StartCoroutine(VehicleSpawner("Subway"));
                break;
            case "FF0000":
                print("bus");
                StartCoroutine(VehicleSpawner("Bus"));
                break;
            case "00C5FF":
                StartCoroutine(VehicleSpawner("Tram"));
                break;
            case "FFFC00":
                StartCoroutine(VehicleSpawner("Train"));
                break;
            case "08FF00":
                StartCoroutine(VehicleSpawner("Tram"));
                break;
            case "FF7800":
                StartCoroutine(VehicleSpawner("Tram"));
                break;
        }
    }

    int wagons = 0;
    // In dieser Coroutine wird das Spawnen der Fahrzeuge gesteuert. Es wird der Name des Fahrzeugtypen
    // als Argument übergeben. Bei Bussen, wird ein Busfahrzeug erzeugt und dann jeweils 6 Sekunden gewartet
    // bis der nächste erzeugt wird. Bei Schienenfahrzeugen wird als erstes das Zugfahrzeug erzeugt und dann jeweils
    // in einem 0.1 Sekundenabstand zwei Wagonfahrzeuge. Nachdem wird ebenfalls 6 Sekunden gewartet um dies wieder
    // fortzusätzen.
    public IEnumerator VehicleSpawner(string Vehicle)
    {
        bool flag = true;

        while (flag)
        {
            if(Vehicle == "Bus")
            {
                GameObject Bus = Instantiate(Resources.Load(Vehicle)) as GameObject; // Busfahrzeug wird erzeugt.
            }
            else if(wagons == 0 && Vehicle != "Bus") 
            {
                wagons = 1;
                GameObject Tram = Instantiate(Resources.Load(Vehicle)) as GameObject; // Der erste unabhängige Zug wird erzeugt.
                if (Vehicle == "Train")
                {
                    yield return new WaitForSeconds(0.8f);
                }
                else
                {
                    yield return new WaitForSeconds(0.6f);
                }
            }
            if (wagons < 4 && Vehicle != "Bus") 
            {
                wagons += 1;
                GameObject Wagon = Instantiate(Resources.Load(Vehicle + " Wagon")) as GameObject; // Hier werden die Wagons erzeugt.
                if (Vehicle == "Train")
                {
                    yield return new WaitForSeconds(0.8f);
                }
                else
                {
                    yield return new WaitForSeconds(0.6f);
                }
            }
            else 
            {
                yield return new WaitForSeconds(20f); // Wenn 3 Schienenfahrzeuge hintereinander erzeugt wurden, wird 6 Sekunden lang gewartet.
                wagons = 0;
            }
        }
    }
}
