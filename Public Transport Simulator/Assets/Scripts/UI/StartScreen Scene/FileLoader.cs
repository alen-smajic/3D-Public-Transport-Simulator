using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Diese Klasse steuert die Ausführung des Hauptmenüs innerhalb des StartScreens.
// Dies ist zugleich der erste Ausgangspunkt des Programms.
public class FileLoader : MonoBehaviour
{
    public GameObject StartScreen;
    public GameObject MainMenu;
    public GameObject SimulationSource;
    public GameObject OptionsScreen;
    public GameObject AboutScreen;
    public GameObject CreditsScreen;
    public GameObject QuitScreen;
    public GameObject LoadingScreen;
    public GameObject FilePath;
    public GameObject Instructions1;
    public GameObject Instructions2;
    public GameObject Instructions3;
    public GameObject ErrorMessage;
    public GameObject ErrorMessage_2;
    public GameObject ErrorMessageOptions;
    public GameObject ErrorMessageOptions2;


    // Hier wird der Pfad zur XML Datei gespeichert. Auf diese Variable
    // greift das Skript MapReader zu um die Daten zu laden.
    public static string ResourceFilePath;

    // Prüft ob der Nutzer eine Eingabe getätigt hat um die Simulation zu starten.
    bool user_input = false;

    // Hilfsvariable für das Abfangen von Fehlern, welche erst im MapReader Skript erkannt werden.
    public static bool simulator_loaded = false;
    
    private void Update()
    {
        // Hier wird geprüft ob der Nutzer eine korrekte Eingabe gemacht hat.
        // Falls dies stimmt wird das Ladefenster geladen.
        if (user_input == true)
        {
            StartScreen.SetActive(false);
            LoadingScreen.SetActive(true);
            user_input = false;
            if (File.Exists(ResourceFilePath) && ResourceFilePath.EndsWith(".txt"))
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                LoadingScreen.SetActive(false);
                StartScreen.SetActive(true);
                // Falls die eingegebene Datei keine .txt Datei ist
                // wird hier eine Fehlermeldung ausgegeben und der Nutzer
                // zurück ins Hauptmenü gebracht.
                ErrorMessage.SetActive(true);
            }
        }

        if (simulator_loaded)
        {
            // Falls die eingegebene Datei eine .txt Datei ist aber
            // nicht in XML Format vorliegt, wird dies durch das
            // MapReader Skript abgefangen und hier die entsprechende
            // Fehlermeldung ausgegeben.
            ErrorMessage_2.SetActive(true);
            simulator_loaded = false;
        }
    }

    // Wird aufgerufen falls der Nutzer den Simulation starten-
    // Button im Hauptmenü drückt.
    public void StartSimulation()
    {
        MainMenu.SetActive(false);
        SimulationSource.SetActive(true);
        ErrorMessage_2.SetActive(false);
    }

    // Wird aufgerufen falls der Nutzer den Eingabe-Button
    // im Ausführmenü drückt. Dadurch wird die Ausführung
    // des Programms in die nexte Etape gebracht.
    public void ConfirmPath()
    {
        ResourceFilePath = FilePath.GetComponent<Text>().text;
        if (ResourceFilePath != "")
        {
            user_input = true;
        }
    }

    // Wird aufgerufen falls der Nutzer den Hilfe-Button im
    // Simulation starten Fenster drückt.
    public void Help()
    {
        SimulationSource.SetActive(false);
        Instructions1.SetActive(true);
        ErrorMessage.SetActive(false);
    }

    // Wird aufgerufen wenn der Nutzer aus dem Hilfefenster 
    // zurück in das Simulation starten Fenster zurückkehren will.
    public void ReturnToSimulationStart()
    {
        Instructions1.SetActive(false);
        Instructions2.SetActive(false);
        Instructions3.SetActive(false);
        SimulationSource.SetActive(true);
    }

    // Wird aufgerufen wenn der Nutzer im Hilfefenster die
    // nächste Seite sehen will.
    public void NextPage1()
    {
        Instructions1.SetActive(false);
        Instructions2.SetActive(true);
    }

    // Wird aufgerufen wenn der Nutzer im Hilfefenster die 
    // vorherige Seite sehen will.
    public void ReturnPage()
    {
        Instructions1.SetActive(true);
        Instructions2.SetActive(false);
    }

    // Wird aufgerufen wenn der Nutzer im Hilfefenster 
    // die nächste Seite sehen will.
    public void NexPage2()
    {
        Instructions2.SetActive(false);
        Instructions3.SetActive(true);
    }

    // Wird aufgerufen wenn der Nutzer im Hilfefenster
    // die vorherige Seite sehen will.
    public void Return2()
    {
        Instructions3.SetActive(false);
        Instructions2.SetActive(true);
    }

    // Wird aufgerufen falls der Nutzer den Optionen - Button
    // im Hauptmenü drückt.
    public void Options()
    {
        MainMenu.SetActive(false);
        OptionsScreen.SetActive(true);
        ErrorMessage_2.SetActive(false);
    }

    // Wird aufgerufen wenn der Nutzer das Optionen Fenster
    // verlassen will. Dabei wird der Fall abgefangen das der
    // Nutzer keine Option festgelegt hat oder Stationen ohne
	// Stationstyp auswählt.
    public void ReturnfromOptions()
    {
        if (!UserPreferences.Buildings && !UserPreferences.Stations && !UserPreferences.AllStreets && !UserPreferences.PublicTransportStreets && !UserPreferences.PublicTransportRailways)
        {
            ErrorMessageOptions2.SetActive(false);
            ErrorMessageOptions.SetActive(true);
        }
        else if(UserPreferences.Stations && !UserPreferences.Subways && !UserPreferences.Trams && !UserPreferences.Trains && !UserPreferences.Railways && !UserPreferences.LightRails && !UserPreferences.Busses)
        {
            ErrorMessageOptions.SetActive(false);
            ErrorMessageOptions2.SetActive(true);
        }
        else
        {
            ErrorMessageOptions.SetActive(false);
            ErrorMessageOptions2.SetActive(false);
            OptionsScreen.SetActive(false);
            MainMenu.SetActive(true);
        }
    }

    // Wird aufgerufen falls der Nutzer den About - Button
    // im Hauptmenü drückt.
    public void About()
    {
        MainMenu.SetActive(false);
        AboutScreen.SetActive(true);
        ErrorMessage_2.SetActive(false);
    }

    // Wird aufgerufen falls der Nutzer den Credits - Button
    // im Hauptmenü drückt.
    public void Credits()
    {
        MainMenu.SetActive(false);
        CreditsScreen.SetActive(true);
        ErrorMessage_2.SetActive(false);
    }

    // Wird aufgerufen falls der Nutzer den Verlassen - Button
    // im Hauptmenü drückt.
    public void Close()
    {
        MainMenu.SetActive(false);
        QuitScreen.SetActive(true);
        ErrorMessage_2.SetActive(false);
    }

    // Wird aufgerufen falls der Nutzer den Ja - Button im
    // Ausgangsmenü drückt.
    public void Quit_yes()
    {
        Application.Quit();
    }

    // Wird aufgerufen falls der Nutzer den Nein - Button im
    // Ausgangsmenü drückt.
    public void Quit_no()
    {
        QuitScreen.SetActive(false);
        MainMenu.SetActive(true);
    }

    // Wird aufgerufen falls der Nutzer den Zurück - Button
    // drückt um ins Hauptmenü zurückzukehren.
    public void Return()
    {
        CreditsScreen.SetActive(false);
        AboutScreen.SetActive(false);
        OptionsScreen.SetActive(false);
        SimulationSource.SetActive(false);
        MainMenu.SetActive(true);
        ErrorMessage.SetActive(false);
    }
}
