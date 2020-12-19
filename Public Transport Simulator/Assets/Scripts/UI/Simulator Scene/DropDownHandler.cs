using UnityEngine;
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
/// This class controlls the UI of the Station-Objects inside the simulation.
/// </summary>
public class DropDownHandler : MonoBehaviour
{
    public GameObject DropDownText;
    public GameObject SearchResultsDropdown;
    public GameObject SearchInput;
    public GameObject Camera;

    bool SearchingStarted = false;

    /// <summary>
    /// Upon selecting a public transport line from the dropdown menu of a station, this function
    /// is called which calls the function for way marking and vehicle instantiation.
    /// </summary>
    public void React()
    {
        TranSportWayMarker.SelectPublicTransportLine(DropDownText.GetComponent<Text>().text);
    }

    /// <summary>
    /// Called when the user clicks on the close button of a Station UI. Closes the Station UI.
    /// </summary>
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// This function is called when the user selects a station from the search bar dropdown. It activates the 
    /// Station UIs of all specified stations and teleports the user to their target locations.
    /// </summary>
    public void SelectFoundStations()
    {
        if (SearchingStarted)
        {
            return;
        }
        SearchingStarted = true;

        SearchResultsDropdown.SetActive(false);

        List<GameObject> allWayObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(allWayObjects);

        Vector3 StationPos = new Vector3(0, 0, 0);

        for (int i = 0; i < allWayObjects.Count; i++)
        {
            if (allWayObjects[i].name == "Station(Clone)")
            {
                if (allWayObjects[i].transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text == DropDownText.GetComponent<Text>().text)
                {
                    if(allWayObjects[i].transform.GetChild(0).GetChild(0).gameObject.activeInHierarchy != true)
                    {
                        allWayObjects[i].transform.GetChild(0).GetChild(0).GetChild(6).gameObject.SetActive(true);
                    }
                    allWayObjects[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    StationPos = allWayObjects[i].transform.localPosition;
                } 
            }
        }

        Camera.transform.localPosition = StationPos + new Vector3(0, 600, -600);
        Camera.transform.eulerAngles = new Vector3(40f, 0f, 0f);

        SearchResultsDropdown.GetComponent<Dropdown>().value = 0;
        SearchInput.transform.GetComponentInParent<InputField>().text = "";

        SearchingStarted = false;
    }
}
    