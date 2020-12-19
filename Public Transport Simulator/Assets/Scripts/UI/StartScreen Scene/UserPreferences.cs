using UnityEngine;
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
/// This class is used to store the user specified options. It is used to specify
/// which type of information should be extracted and stored from the data source.
/// It also handles the available checkbox options in the options menu.
/// </summary>

public class UserPreferences : MonoBehaviour
{
    // Reference to the checkboxes from the UI
    public GameObject subways_toggle;
    public GameObject trams_toggle;
    public GameObject trains_toggle;
    public GameObject railways_toggle;
    public GameObject light_rails_toggle;
    public GameObject busses_toggle;

    public static bool PublicTransportRailways = true;

    public static bool PublicTransportStreets = true;

    public static bool AllStreets = true;

    public static bool Stations = true;

    public static bool Buildings = true;

    public static bool Subways = true;

    public static bool Trams = true;

    public static bool Trains = true;

    public static bool Railways = true;

    public static bool LightRails = true;

    public static bool Busses = true;

    /// <summary>
    /// Further checkboxes will be activated or deactivated depending on the 
    /// value of this checkbox.
    /// </summary>
    /// <param name="newValue">The value parameter of the checkbox</param>
    public void PublicTransportRailways_f(bool newValue)
    {
        PublicTransportRailways = newValue;

        if (Stations == false && PublicTransportRailways == false)
        {
            subways_toggle.GetComponent<Toggle>().isOn = false;
            subways_toggle.SetActive(false);
            Subways = false;
            trams_toggle.GetComponent<Toggle>().isOn = false;
            trams_toggle.SetActive(false);
            Trams = false;
            trains_toggle.GetComponent<Toggle>().isOn = false;
            trains_toggle.SetActive(false);
            Trains = false;
            railways_toggle.GetComponent<Toggle>().isOn = false;
            railways_toggle.SetActive(false);
            Railways = false;
            light_rails_toggle.GetComponent<Toggle>().isOn = false;
            light_rails_toggle.SetActive(false);
            LightRails = false;
        }
        else if (PublicTransportRailways == true)
        {
            subways_toggle.SetActive(true);
            trams_toggle.SetActive(true);
            trains_toggle.SetActive(true);
            railways_toggle.SetActive(true);
            light_rails_toggle.SetActive(true);
        }
    }

    /// <summary>
    /// Further checkboxes will be activated or deactivated depending on the 
    /// value of this checkbox.
    /// </summary>
    /// <param name="newValue">The value parameter of the checkbox</param>
    public void Stations_f(bool newValue)
    {
        Stations = newValue;

        if(Stations == false)
        {
            busses_toggle.GetComponent<Toggle>().isOn = false;
            busses_toggle.SetActive(false);
            Busses = false;
        }
        else if(Stations == true)
        {
            busses_toggle.SetActive(true);

        }

        if (Stations == false && PublicTransportRailways == false)
        {
            subways_toggle.GetComponent<Toggle>().isOn = false;
            subways_toggle.SetActive(false);
            Subways = false;
            trams_toggle.GetComponent<Toggle>().isOn = false;
            trams_toggle.SetActive(false);
            Trams = false;
            trains_toggle.GetComponent<Toggle>().isOn = false;
            trains_toggle.SetActive(false);
            Trains = false;
            railways_toggle.GetComponent<Toggle>().isOn = false;
            railways_toggle.SetActive(false);
            Railways = false;
            light_rails_toggle.GetComponent<Toggle>().isOn = false;
            light_rails_toggle.SetActive(false);
            LightRails = false;
        }
        else if (Stations == true)
        {
            subways_toggle.SetActive(true);
            trams_toggle.SetActive(true);
            trains_toggle.SetActive(true);
            railways_toggle.SetActive(true);
            light_rails_toggle.SetActive(true);
        }
    }

    public void PublicTransportStreets_f(bool newValue)
    {
        PublicTransportStreets = newValue;
    }

    public void AllStreets_f(bool newValue)
    {
        AllStreets = newValue;
    }

    public void Building_f(bool newValue)
    {
        Buildings = newValue;
    }

    public void Subways_f(bool newValue)
    {
        Subways = newValue;
    }

    public void Trams_f(bool newValue)
    {
        Trams = newValue;
    }

    public void Trains_f(bool newValue)
    {
        Trains = newValue;
    }

    public void Railways_f(bool newValue)
    {
        Railways = newValue;
    }

    public void LightRails_f(bool newValue)
    {
        LightRails = newValue;
    }

    public void Busses_f(bool newValue)
    {
        Busses = newValue;
    }
}
