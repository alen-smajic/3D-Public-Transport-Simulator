using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

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
/// After a train/bus line has been selected, the WayOrderManager Gameobject will be instantiated, which contains this script. This scripts takes the 
/// way nodes of the train/bus line and sorts these in such a way that the vehicle can move from node to node using the coordinates list. Lastly it 
/// instantiates the vehicle objeccts.
/// </summary>
public class SortWay : MonoBehaviour
{
    // first dimension = Way objects, second dimension = node coordinates of the way objects.
    public List<List<Vector3>> allWayCoordinates; 

    // first dimension = paths (ways that are coonnected together), second dimension = way objects which are sorted with respect to the paths, third dimension = node coordinates of the way objects.
    public static List<List<List<Vector3>>> SortedPathAndWays; 
                                                     
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

        // This for loop returns a list "allWayCoordinates" which contains more lists. Every list of these are way objects
        // which contain the node coordinates
        for (int i = 0; i < TranSportWayMarker.SelectedWays.Count; i++)
        {
            string ObjectName = allObjects[TranSportWayMarker.SelectedWays[i]].name;
            string ObjectID = ObjectName.Substring(15); // The ID of the way is being determined in order to search for it in the MapReader class.

            List<Vector3> WayNodes = new List<Vector3>();
            for (int j = 0; j < MapReader.ways[Convert.ToUInt64(ObjectID)].UnityCoordinates.Count; j++)
            {
                WayNodes.Add(MapReader.ways[Convert.ToUInt64(ObjectID)].UnityCoordinates[j]);
            }
            allWayCoordinates.Add(WayNodes); 
            // allWayCoordinates contains lists which represent the way objects. Every single one of these lists contains the node coordinates as values.
        }

        // Next we create a new list (SortedPathAndWays). This list merges the ways that belong to a common path.
        // E.g. if a bus line ends on one edge of the map and continues at another edge of the map, the corresponding ways will be stored in different lists.
        List<List<Vector3>> localList = new List<List<Vector3>>();
        localList.Add(allWayCoordinates[0]);
        SortedPathAndWays.Add(localList);
        allWayCoordinates.RemoveAt(0);

        // Is being called to sort the SortedPathAndWays with  respect to allWayCoordinates.
        SortList(allWayCoordinates, 0);

        // Here we transform the three-dimensional SortedPathAndWays list into a two-dimensional list. We do this by concatenating the inner lists
        // because these are already sorted.
        List<Vector3> temporaryList = new List<Vector3>();
        for(int i = 0; i < SortedPathAndWays.Count; i++) 
        {
            temporaryList = SortedPathAndWays[i].SelectMany(x => x).ToList();
            SplittedinPaths.Add(temporaryList);
        }

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

        // Append paths which dont contain any station at the end.
        for(int i = 0; i < SplittedinPaths.Count; i++)
        {
            if (!PathsInRightOrder.Contains(SplittedinPaths[i]))
            {
                PathsInRightOrder.Add(SplittedinPaths[i]);
            }
        }

        // Switch the direction of the values within the paths using the stations.
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


    // This function sorts the way objects in the correct order. 
    void SortList(List<List<Vector3>> WaysToBeSorted, int index)
    {
        if(WaysToBeSorted.Count <= 0)
        {
            return;
        }

        List<List<Vector3>> LeftItems = WaysToBeSorted; 
        bool found_something = false;

        for(int i = 0; i < WaysToBeSorted.Count; i++)
        {
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

    /// <summary>
    /// This function can identify which vehicle is needed using the stored color value of the way. Using
    /// this information it can call the VehicleSpawner Couroutine which instantiates the correct vehicle.
    /// </summary>
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
    /// <summary>
    /// This couroutine controlls the spawning of the transport vehicles. 
    /// </summary>
    /// <param name="Vehicle">name of the transport vehicle</param>
    /// <returns></returns>
    public IEnumerator VehicleSpawner(string Vehicle)
    {
        bool flag = true;

        while (flag)
        {
            if(Vehicle == "Bus")
            {
                GameObject Bus = Instantiate(Resources.Load(Vehicle)) as GameObject; // Instantiate bus vehicle.
            }
            else if(wagons == 0 && Vehicle != "Bus") 
            {
                wagons = 1;
                GameObject Tram = Instantiate(Resources.Load(Vehicle)) as GameObject; // Instantiate leading wagon of train vehicle.
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
                GameObject Wagon = Instantiate(Resources.Load(Vehicle + " Wagon")) as GameObject; // Instantiate other wagons of train vehicle.
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
                yield return new WaitForSeconds(20f); // After 3 wagons have been instantiated wait 20 second and instantiate the next ones.
                wagons = 0;
            }
        }
    }
}
