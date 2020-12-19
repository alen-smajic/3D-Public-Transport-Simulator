using UnityEngine;
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
/// This class controlls the UI within the simulation scene.
/// </summary>
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

    /// <summary>
    /// Hides the complete UI.
    /// </summary>
    public void HideUI()
    {
        MenuWindow.SetActive(false);
        ShowMenuButton.SetActive(true);
        MenuHidden = true;
    }

    /// <summary>
    /// Show the complete UI.
    /// </summary>
    public void ShowUI()
    {
        ShowMenuButton.SetActive(false);
        MenuWindow.SetActive(true);
        MenuHidden = false;
    }

    /// <summary>
    /// Switch to light mode.
    /// </summary>
    public void LightMode()
    {
        DarkModeButton.SetActive(true);
        LightModeButton.SetActive(false);
        RenderSettings.skybox = LightSkyBox;
        Sun.SetActive(true);
        DarkModeOn = false;
    }

    /// <summary>
    /// Switch to dark mode.
    /// </summary>
    public void DarkMode()
    {
        LightModeButton.SetActive(true);
        DarkModeButton.SetActive(false);
        RenderSettings.skybox = DarkSkyBox;
        Sun.SetActive(false);
        DarkModeOn = true;
    }

    /// <summary>
    /// Show the object legend (UI-Element).
    /// </summary>
    public void ShowLegend()
    {
        ShowLegendButton.gameObject.SetActive(false);
        HideLegendButton.SetActive(true);
        LegendBar.SetActive(true);
    }

    /// <summary>
    /// Hide the object legend (UI-Element).
    /// </summary>
    public void HideLegend()
    {
        HideLegendButton.gameObject.SetActive(false);
        ShowLegendButton.SetActive(true);
        LegendBar.SetActive(false);
    }

    /// <summary>
    /// Show 3D building in the scene.
    /// </summary>
    public void ShowBuildings()
    {
        ShowBuildingsButton.SetActive(false);
        HideBuildingsButton.SetActive(true);
        Buildings.SetActive(true);
    }

    /// <summary>
    /// Hide 3D buildings from the scene.
    /// </summary>
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

    /// <summary>
    /// Open the User Manuals.
    /// </summary>
    public void OpenInstructions()
    {
        InstructionsWindows1.SetActive(true);
        MenuWindow.SetActive(false);
        InstructionsOn = true;
    }

    /// <summary>
    /// Next page button of the User Manuals.
    /// </summary>
    public void NexInstructionPage1()
    {
        InstructionsWindows2.SetActive(true);
        InstructionsWindows1.SetActive(false);
    }

    /// <summary>
    /// Next page button of the User Manuals.
    /// </summary>
    public void NextInstructionPage2()
    {
        InstructionsWIndows3.SetActive(true);
        InstructionsWindows2.SetActive(false);
    }

    /// <summary>
    /// Return button of the User Manuals.
    /// </summary>
    public void ReturnInstructionPage1()
    {
        InstructionsWindows1.SetActive(true);
        InstructionsWindows2.SetActive(false);
    }

    /// <summary>
    /// Return button of the user manuals.
    /// </summary>
    public void ReturnInstructionPage2()
    {
        InstructionsWindows2.SetActive(true);
        InstructionsWIndows3.SetActive(false);
    }

    /// <summary>
    /// Controlls the "OK" button which closes the UI-Element.
    /// </summary>
    public void OKButton()
    {
        NoBuildingsMessage.SetActive(false);
        InstructionsWindows1.SetActive(false);
        InstructionsWindows2.SetActive(false);
        InstructionsWIndows3.SetActive(false);
        MenuWindow.SetActive(true);
        InstructionsOn = false;
    }

    /// <summary>
    /// Controlls the "Leave the simulation" button which returns 
    /// the user to the Main Menu. Resets all options.
    /// </summary>
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
