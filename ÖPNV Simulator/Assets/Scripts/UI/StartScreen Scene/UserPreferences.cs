using UnityEngine;
using UnityEngine.UI;

// Diese Klasse steuert die durch den Nutzer ausgewählten Optionen. Diese
// Informationen werden genutzt um eine optimiert Ausführung des Programms
// zu gewährleisten und nur die ausgewählten Objekte anzuzeigen.
public class UserPreferences : MonoBehaviour
{
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

    // Falls diese Option gewählt wird, werden dem Nutzer weitere
    // Optionen zur verfügung gestellt, welche in einem Zusammenhang
    // stehen.
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

    // Falls diese Option gewählt wird, werden dem Nutzer weitere
    // Optionen zur verfügung gestellt, welche in einem Zusammenhang
    // stehen.
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
