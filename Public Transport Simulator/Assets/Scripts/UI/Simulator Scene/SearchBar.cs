using System.Collections.Generic;
using UnityEngine;
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
/// Used for the search functionality within the simulated scene (controlls the search bar).
/// </summary>
public class SearchBar : MonoBehaviour
{
    public GameObject TextBar;
    public GameObject DropDown;

    public List<string> FoundStations; 
    public List<string> AllStation; 

    /// <summary>
    /// Stores all generated station names in a list.
    /// </summary>
    void Start()
    {
        if(LanguageSettings.Language == "English")
        {
            transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Search for stations..";
        }
        else
        {
            transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Suche nach Stationen..";
        }

        FoundStations = new List<string>();
        AllStation = new List<string>();

        List<GameObject> allObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(allObjects);

        for(int i = 0; i < allObjects.Count; i++)
        {
            if(allObjects[i].name == "Station(Clone)")
            {
                if (AllStation.Contains(allObjects[i].transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text))
                {
                    continue;
                }
                AllStation.Add(allObjects[i].transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text);
            }
        }
    }

    /// <summary>
    /// Using the user input, this function searches for all stations which include the user input. These
    /// are then returned in a list as "Search results".
    /// </summary>
    /// <param name="searchInput">User input from the search bar</param>
    public void SearchForStation(string searchInput)
    {
        if(searchInput == "")
        {
            return;
        }

        DropDown.SetActive(true);
        FoundStations.Clear();
        if(LanguageSettings.Language == "English")
        {
            FoundStations.Add("Search results:");
        }
        else
        {
            FoundStations.Add("Suchergebnisse:");
        }
        
        for(int i = 0; i < AllStation.Count; i++)
        {
            if (AllStation[i].ToLower().Contains(searchInput.ToLower()))
            {
                FoundStations.Add(AllStation[i]);
            }
        }

        DropDown.GetComponent<Dropdown>().options.Clear();
        DropDown.GetComponent<Dropdown>().AddOptions(FoundStations);
    }
}
