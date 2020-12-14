using UnityEngine;
using UnityEngine.SceneManagement;

// Diese Klasse steuert die Ausführung der Benutzeroberfläche innerhalb
// der Simulation.
public class IngameMenu : MonoBehaviour
{
    public GameObject Buildings;
    public Material DarkSkyBox;
    public Material LightSkyBox;
    public GameObject Sun;

    public GameObject ShowBuildingsButton;
    public GameObject HideBuildingsButton;
    public GameObject ShowLegendButton;
    public GameObject HideLegendButton;
    public GameObject LightModeButton;
    public GameObject DarkModeButton;
    public GameObject ShowMenuButton;

    public GameObject MenuWindow;
    public GameObject LegendBar;
    public GameObject NoBuildingsMessage;
    public GameObject InstructionsWindows1;
    public GameObject InstructionsWindows2;
    public GameObject InstructionsWIndows3;

    public static bool InstructionsOn = true;
    public static bool MenuHidden = false;
    public static bool DarkModeOn = false;

    // Blendet das komplette Menü aus.
    public void HideUI()
    {
        MenuWindow.SetActive(false);
        ShowMenuButton.SetActive(true);
        MenuHidden = true;
    }

    // Blendet das Menü wieder ein.
    public void ShowUI()
    {
        ShowMenuButton.SetActive(false);
        MenuWindow.SetActive(true);
        MenuHidden = false;
    }

    // Ändert den Simulationshimmel in die Tagesansicht.
    public void LightMode()
    {
        DarkModeButton.SetActive(true);
        LightModeButton.SetActive(false);
        RenderSettings.skybox = LightSkyBox;
        Sun.SetActive(true);
        DarkModeOn = false;
    }

    // Ändert den Simulationshimmel in die Nachtansicht.
    public void DarkMode()
    {
        LightModeButton.SetActive(true);
        DarkModeButton.SetActive(false);
        RenderSettings.skybox = DarkSkyBox;
        Sun.SetActive(false);
        DarkModeOn = true;
    }

    // Blendet die Legendenleiste ein.
    public void ShowLegend()
    {
        ShowLegendButton.gameObject.SetActive(false);
        HideLegendButton.SetActive(true);
        LegendBar.SetActive(true);
    }

    // Blendet die Legendenleiste aus.
    public void HideLegend()
    {
        HideLegendButton.gameObject.SetActive(false);
        ShowLegendButton.SetActive(true);
        LegendBar.SetActive(false);
    }

    // Blender die Gebäude ein.
    public void ShowBuildings()
    {
        ShowBuildingsButton.SetActive(false);
        HideBuildingsButton.SetActive(true);
        Buildings.SetActive(true);
    }

    // Blendet die Gebäude aus.
    public void HideBuildings()
    {
        if (UserPreferences.Buildings)
        {
            HideBuildingsButton.SetActive(false);
            ShowBuildingsButton.SetActive(true);
            Buildings = GameObject.Find("3D-Buildings(Clone)");
            Buildings.SetActive(false);
        }
        else
        {
            NoBuildingsMessage.SetActive(true);
            MenuWindow.SetActive(false);
            InstructionsOn = true;
        }
    }

    // Öffnet ein Fenster mit den Steuerungshinweisen.
    public void OpenInstructions()
    {
        InstructionsWindows1.SetActive(true);
        MenuWindow.SetActive(false);
        InstructionsOn = true;
    }

    // Öffnet die nächste Instruktionsseite.
    public void NexInstructionPage1()
    {
        InstructionsWindows2.SetActive(true);
        InstructionsWindows1.SetActive(false);
    }

    // Öffnet die nächste Instruktionsseite.
    public void NextInstructionPage2()
    {
        InstructionsWIndows3.SetActive(true);
        InstructionsWindows2.SetActive(false);
    }

    // Öffnet die vorherige Instruktionsseite.
    public void ReturnInstructionPage1()
    {
        InstructionsWindows1.SetActive(true);
        InstructionsWindows2.SetActive(false);
    }

    // Öffnet die vorherige Instruktionsseite.
    public void ReturnInstructionPage2()
    {
        InstructionsWindows2.SetActive(true);
        InstructionsWIndows3.SetActive(false);
    }

    // Schließt das aktuelle Fenster.
    public void OKButton()
    {
        NoBuildingsMessage.SetActive(false);
        InstructionsWindows1.SetActive(false);
        InstructionsWindows2.SetActive(false);
        InstructionsWIndows3.SetActive(false);
        MenuWindow.SetActive(true);
        InstructionsOn = false;
    }

    // Bricht die Simulation ab. Stellt die Optionen wieder 
    // auf die Standardeinstellungen.
    public void ReturnToMenu()
    {
        FileLoader.simulator_loaded = false;
        SceneManager.LoadScene(0);
        UserPreferences.PublicTransportRailways = true;
        UserPreferences.PublicTransportStreets = true;
        UserPreferences.AllStreets = true;
        UserPreferences.Stations = true;
        UserPreferences.Buildings = true;
        UserPreferences.Subways = true;
        UserPreferences.Trams = true;
        UserPreferences.Trains = true;
        UserPreferences.Railways = true;
        UserPreferences.LightRails = true;
    }
}
