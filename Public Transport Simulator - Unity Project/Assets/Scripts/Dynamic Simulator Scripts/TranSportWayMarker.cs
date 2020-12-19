using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
/// Here we process the user input from the station dropdown. The way object of the selected public
/// transport line will be marked and stored. Furthermore, the SortWay script is being triggered
/// to sort out the way information for the transport vehicle to move across.
/// </summary>
public class TranSportWayMarker : MonoBehaviour
{
    public static List<int> SelectedWays; // Here we store the current changes to the way objects.
    static List<int> SelectedStations; // Here we store the current changes to the station objects.
    public static Material PreviousMaterial; // Here we store the previous object color.

    public static List<Vector3> StationOrder; 
    
    static bool SelectionStarted = false; 

    private void Start()
    {
        SelectedWays = new List<int>();
        SelectedStations = new List<int>();

        StationOrder = new List<Vector3>();
    }

    /// <summary>
    /// This function is called as soon as the user selects an option from the station dropdown. This undoes the
    /// previous changes, and marks the currently selected public transport line.
    /// </summary>
    /// <param name="selection">The dropdown selection of the user from the station UI</param>
    public static void SelectPublicTransportLine(string selection)
    {
        if (SelectionStarted == true || selection == "")
        {
            return;
        }

        List<GameObject> allWayObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(allWayObjects);

        for (int i = 0; i < allWayObjects.Count; i++)
        {
            if (allWayObjects[i].name == "Station(Clone)")
            {
                allWayObjects[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false); // Close all station UIs.
                allWayObjects[i].transform.GetChild(0).GetChild(0).GetChild(6).gameObject.SetActive(true); // Deactivate the close button on the station UI.
            }
            else if (allWayObjects[i].name == "WayOrderManager(Clone)" || allWayObjects[i].name == "Bus(Clone)" || allWayObjects[i].name == "Tram(Clone)" || allWayObjects[i].name == "Subway(Clone)" || allWayObjects[i].name == "Train(Clone)")
            {
                allWayObjects[i].Destroy(); // Remove all current transport vehicles.
            }
        }

        if (selection == "Global View") 
        {
            SelectionStarted = true;
            for (int j = 0; j < SelectedWays.Count; j++) // The current markings are being reverted.
            {
                int index = SelectedWays[j];
                allWayObjects[index].transform.localPosition = new Vector3(allWayObjects[index].transform.localPosition.x, 0, allWayObjects[index].transform.localPosition.z);
                allWayObjects[index].gameObject.GetComponent<Renderer>().material = PreviousMaterial;
            }
            for (int k = 0; k < SelectedStations.Count; k++) // The station UIs are being closed and the drop down selection is being reverted to global view
            {
                int index = SelectedStations[k];
                GameObject StationUI = allWayObjects[index].transform.GetChild(0).GetChild(0).gameObject;
                StationUI.transform.GetChild(2).gameObject.GetComponent<Dropdown>().value = 0;
            }
            SelectionStarted = false;
        }
        else // If the user selected a public transport line
        {
            SelectionStarted = true;

            for (int j = 0; j < SelectedWays.Count; j++) 
            {
                int index = SelectedWays[j];
                allWayObjects[index].transform.localPosition = new Vector3(allWayObjects[index].transform.localPosition.x, 0, allWayObjects[index].transform.localPosition.z);
                allWayObjects[index].gameObject.GetComponent<Renderer>().material = PreviousMaterial;
            }
            for (int k = 0; k < SelectedStations.Count; k++) 
            {
                int index = SelectedStations[k];
                GameObject StationUI = allWayObjects[index].transform.GetChild(0).GetChild(0).gameObject;
                StationUI.transform.GetChild(2).gameObject.GetComponent<Dropdown>().value = 0;
            }

            SelectedWays.Clear();
            SelectedStations.Clear();
            StationOrder.Clear();
            PreviousMaterial = null;

            // Here we mark the chosen public transport line and store its original information for later revert.
            for (int i = 0; i < allWayObjects.Count; i++)
            {
                if (allWayObjects[i].name.StartsWith("New Game Object"))
                {
                    var gameObjectText = allWayObjects[i].GetComponent<Text>();
                    if (gameObjectText.text.Contains(selection)) // Using the string selection, we search for all Way objects which include this string.
                    {
                        SelectedWays.Add(i);
                        PreviousMaterial = allWayObjects[i].GetComponent<Renderer>().material;

                        allWayObjects[i].transform.position += new Vector3(0, 0.1f, 0); // Increase the height of the object so it doesnt intersect with other objects.
                        allWayObjects[i].GetComponent<Renderer>().material = MapBuilder.selected_way; // Apply the new material to the way object.
                    }
                }
                else if (allWayObjects[i].name == "Station(Clone)")
                {
                    GameObject DropDown = allWayObjects[i].transform.GetChild(0).GetChild(0).GetChild(2).gameObject;
                    for (int j = 0; j < DropDown.GetComponent<Dropdown>().options.Count; j++)
                    {
                        if (DropDown.GetComponent<Dropdown>().options[j].text == selection) 
                        {
                            SelectedStations.Add(i);

                            GameObject StationUI = allWayObjects[i].transform.GetChild(0).GetChild(0).gameObject; 
                            StationUI.SetActive(true);
                            StationUI.transform.GetChild(6).gameObject.SetActive(false); 
                            StationUI.transform.GetChild(2).gameObject.GetComponent<Dropdown>().value = j; 
                        }
                    }
                }
            }

            SelectionStarted = false;

            // This part generates a list of station coordinates which are sorted in the coorrect order.
            // Later is this used for the transport vehicle movement.
            for (int i = 0; i < MapReader.relations.Count; i++)
            {
                if (MapReader.relations[i].Name == selection)
                {
                    for (int j = 0; j < MapReader.relations[i].StoppingNodeIDs.Count; j++)
                    {
                        try
                        {
                            StationOrder.Add(MapReader.nodes[MapReader.relations[i].StoppingNodeIDs[j]] - MapReader.bounds.Centre);
                        }
                        catch (KeyNotFoundException)
                        {
                            continue;
                        }
                    }
                }
            }

            // The Gameobject "WayOrderManager" is being instantiated. This object triggers the script "SortWay" which controlls the 
            // flow of the public transport vehicles.
            GameObject WayOrderer = Instantiate(Resources.Load("WayOrderManager")) as GameObject;
        }
    }
}
