using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Diese Klasse steuert die Ausführung der Dropdown-Anwendungen jeder Station.
// Steuert die komplette Stations-UI.
public class DropDownHandler : MonoBehaviour
{
    public GameObject DropDownText;

    public GameObject SearchResultsDropdown;
    public GameObject SearchInput;
    public GameObject Camera;

    bool SearchingStarted = false;

    // Falls der Nutzer eine Option aus dem Dropdown auswählt wird die Funktion
    // SelectPublicTransportLine aufgerufen um die jeweilige Linie zu markieren.
    public void React()
    {
        TranSportWayMarker.SelectPublicTransportLine(DropDownText.GetComponent<Text>().text);
    }

	// Falls der Nutzer den Schließen-Button drückt, wird die Stations-UI geschlossen.
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

    // Hier wird die Suche nach den Stationen, welche aus dem Such-Dropdown ausgewählt wurde, ausgeführt.
    // Dabei wird der Suchbegriff benutzt und über alle verfügbaren Stationen iterriert. Am Ende werden diese
    // Stationen aktiviert und die Kamera entsprechend ausgerichtet.
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
    