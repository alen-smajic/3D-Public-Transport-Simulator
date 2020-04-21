using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Hier wird die Eingabe des Nutzers von dem Stations-Dropdown verarbeitet. Die entsprechenden
// Strecken werden eingefärbt und zwischengespeichert. Es wird die Sortierung der Linie veranlasst
// um die Züge bzw. Bahnen einzufügen.
public class TranSportWayMarker : MonoBehaviour
{
    public static List<int> SelectedWays; // Hier werden aktuelle Veränderungen an den Wegen zwischengespeichert.
    static List<int> SelectedStations; // Hier werden aktuelle Veränderungen an den Stationen zwischengespeichert.
    public static Material PreviousMaterial; // Hier wird das vorherige Material eines Weges zwischengespeichert.

    public static List<Vector3> StationOrder; // Wird später genutzt um den Weg in die richtige Richtung einzuordnen.
    
    static bool SelectionStarted = false; // Wird ausschließlich von der Funktion SelectPublicTransportLine() genutzt um keine Endlosschleife zu bilden.

    private void Start()
    {
        SelectedWays = new List<int>();
        SelectedStations = new List<int>();

        StationOrder = new List<Vector3>();
    }

    // Diese Funktion wird aufgerufen sobald der Nutzer eine Option aus dem Stations-Dropdown auswählt. 
    // Dabei werden die vorherigen Veränderungen rückgängig gemacht, und dann falls eine Bus- oder Zuglinie
    // ausgewählt wurde, diese entsprechend eingefärbt und die Stationen angepasst. Am Ende wird das Objekt 
    // WayOrderManager aufgerufen um den Weg richtig zu sortieren für die Generierung der Busse und Züge.
    public static void SelectPublicTransportLine(string selection)
    {
        // Verhindert dass bei der Setzung der ausgewählten Option bei den anderen Stationen die Funktion unedlich oft
        // aufgerufen wird.
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
                allWayObjects[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false); // Alle Stationen werden geschlossen.
                allWayObjects[i].transform.GetChild(0).GetChild(0).GetChild(6).gameObject.SetActive(true); // Der Schließbutton der Stations-UI wird aktiviert.
            }
            else if (allWayObjects[i].name == "WayOrderManager(Clone)" || allWayObjects[i].name == "Bus(Clone)" || allWayObjects[i].name == "Tram(Clone)" || allWayObjects[i].name == "Subway(Clone)" || allWayObjects[i].name == "Train(Clone)")
            {
                allWayObjects[i].Destroy(); //Alle Züge bzw. Busse welche aktuell fahren, werden entfernt.
            }
        }

        if (selection == "Global View") // Falls der Nutzer die erste Option "Global view" aus dem Dropdown ausgewählt hat.
        {
            SelectionStarted = true;
            for (int j = 0; j < SelectedWays.Count; j++) // Der aktuell eingeblendete Pfad wird rückgängig gemacht.
            {
                int index = SelectedWays[j];
                allWayObjects[index].transform.localPosition = new Vector3(allWayObjects[index].transform.localPosition.x, 0, allWayObjects[index].transform.localPosition.z);
                allWayObjects[index].gameObject.GetComponent<Renderer>().material = PreviousMaterial;
            }
            for (int k = 0; k < SelectedStations.Count; k++) // Die aktuell offenen Stationen mit ausgewählter Option werden geschlossen und auf Global view gesetzt.
            {
                int index = SelectedStations[k];
                GameObject StationUI = allWayObjects[index].transform.GetChild(0).GetChild(0).gameObject;
                StationUI.transform.GetChild(2).gameObject.GetComponent<Dropdown>().value = 0;
            }
            SelectionStarted = false;
        }
        else // Falls der Nutzer eine Zug- Buslinie aus dem Dropdown ausgewählt hat.
        {
            SelectionStarted = true;

            // Als erstes wird der Ausgangszustand wiederhergestellt.
            for (int j = 0; j < SelectedWays.Count; j++) // Der aktuell eingeblendete Pfad wird rückgängig gemacht.
            {
                int index = SelectedWays[j];
                allWayObjects[index].transform.localPosition = new Vector3(allWayObjects[index].transform.localPosition.x, 0, allWayObjects[index].transform.localPosition.z);
                allWayObjects[index].gameObject.GetComponent<Renderer>().material = PreviousMaterial;
            }
            for (int k = 0; k < SelectedStations.Count; k++) // Die aktuell offenen Stationen mit ausgewählter Option werden geschlossen und auf Global view gesetzt.
            {
                int index = SelectedStations[k];
                GameObject StationUI = allWayObjects[index].transform.GetChild(0).GetChild(0).gameObject;
                StationUI.transform.GetChild(2).gameObject.GetComponent<Dropdown>().value = 0;
            }

            SelectedWays.Clear();
            SelectedStations.Clear();
            StationOrder.Clear();
            PreviousMaterial = null;

            // Ab hier wir die ausgewählte Bus- Zuglinie eingeblendet, entsprechend angepast und zwischengespeichert.
            for (int i = 0; i < allWayObjects.Count; i++)
            {
                if (allWayObjects[i].name.StartsWith("New Game Object"))
                {
                    var gameObjectText = allWayObjects[i].GetComponent<Text>();
                    if (gameObjectText.text.Contains(selection)) // Anhand des Begriffs wird nach allen WayObjekten gesucht welche diesen Begriff in der Text-Komponente enthalten.
                    {
                        SelectedWays.Add(i);
                        PreviousMaterial = allWayObjects[i].GetComponent<Renderer>().material;

                        allWayObjects[i].transform.position += new Vector3(0, 0.1f, 0); // Weg wird leicht angehoben um nicht durch andere Wege überdeckt zu sein.
                        allWayObjects[i].GetComponent<Renderer>().material = MapBuilder.selected_way; // Neues Material wird dem Weg zugewiesen.
                    }
                }
                else if (allWayObjects[i].name == "Station(Clone)")
                {
                    GameObject DropDown = allWayObjects[i].transform.GetChild(0).GetChild(0).GetChild(2).gameObject;
                    for (int j = 0; j < DropDown.GetComponent<Dropdown>().options.Count; j++)
                    {
                        if (DropDown.GetComponent<Dropdown>().options[j].text == selection) // Es wird über alle Stationen iteriert welche den Liniennamen als Option im Dropdown besitzen.
                        {
                            SelectedStations.Add(i);

                            GameObject StationUI = allWayObjects[i].transform.GetChild(0).GetChild(0).gameObject; 
                            StationUI.SetActive(true);
                            StationUI.transform.GetChild(6).gameObject.SetActive(false); // Schließbutton der Stations-UI wird deaktiviert.
                            StationUI.transform.GetChild(2).gameObject.GetComponent<Dropdown>().value = j; // Dropdown Option wird gesetzt.
                        }
                    }
                }
            }

            SelectionStarted = false;
            // Dieser Abschnitt generiert eine Liste welche die Stationskoordinaten enthält und dies in einer richtigen Reihenfolge geordnet ist.
            // Wird spätert genutzt um den Weg in die richtige Richtung einzuordnen bei der Zug- bzw. Busgenerierung.
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

            // Das Gameobjekt "WayOrderManager" wird ins Leben gerufen. Von dort aus wird mit Hilfe des Skripts "SortWay" die Generierung
            // und Fahrt der Busse und Züge gestuert.
            GameObject WayOrderer = Instantiate(Resources.Load("WayOrderManager")) as GameObject;
        }
    }
}
