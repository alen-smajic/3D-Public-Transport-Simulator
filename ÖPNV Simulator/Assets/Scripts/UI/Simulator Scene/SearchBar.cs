using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Diese Klasse steuert die Ausführung der Suchfunktion innerhalb der Simulation.
public class SearchBar : MonoBehaviour
{
    public GameObject TextBar;
    public GameObject DropDown;

    public List<string> FoundStations; // Hier werden die Suchergebnisse hinterlegt.
    public List<string> AllStation; // Hier befindet sich die Auswahl aller Stationen.

    // Als aller erstes wird eine Liste mit allen möglichen Stationsnamen hinterlegt.
    void Start()
    {
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

    // Hier wird über die Menge der verfügbaren Stationen iterriert und nach dem 
    // Suchbegriff gesucht. Die gefundenen Stationen werden dann in der Liste Foundstations
    // hinterlegt und als Dropdown ausgegeben.
    public void SearchForStation(string searchInput)
    {
        if(searchInput == "")
        {
            return;
        }

        DropDown.SetActive(true);
        FoundStations.Clear();
        FoundStations.Add("Suchergebnisse:");
        
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
