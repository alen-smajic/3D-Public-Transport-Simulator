using UnityEngine;
using System.Globalization;
using System.Xml;

// Diese Klasse nimmt den Grenzmarkierungsdatensatz der XML Datei auf und berechnet das Zentrum 
// der Karte. Anhand dieses Zentrums werden dann die manuell generierten Objekte später zentriert.
// Der zweite Teil generiert anhand der Dimensionen einen Referenzpunkt für die von Mapbox generierten
// Objekte. Diese Objekte werden anhand dieser Daten zentriert und ihre Dimension wird hier festgelegt.
class OsmBounds : BaseOsm
{
    public float MinLat { get; private set; }

    public float MaxLat { get; private set; }

    public float MinLon { get; private set; }

    public float MaxLon { get; private set; }
	
    public Vector3 Centre { get; private set; } // Referenz für die manuell generierten Objekte.

    public string BuildingCentre { get; private set; } // Referenzpunkt für die Objekte generiert durch Mapbox.

    public static int LenghtFactor { get; private set; } // Dimensionen der Objekte generiert durch Mapbox.

    public static int WidthFactor { get; private set; } // Dimensionen der Objekte generiert durch Mapbox.

    public OsmBounds(XmlNode node)
    {
        MinLat = GetFloat("minlat", node.Attributes);
        MaxLat = GetFloat("maxlat", node.Attributes);
        MinLon = GetFloat("minlon", node.Attributes);
        MaxLon = GetFloat("maxlon", node.Attributes);

        // Die Flächen- und Breitenkoordinaten werden durch die Mercator-Projection in Unity
        // Koordinaten umgewandelt (also Longitude und Latitude einer Kugel werden auf X und 
        // Y einer Fläche abgebildet).
        float x = (float)((MercatorProjection.lonToX(MaxLon) + MercatorProjection.lonToX(MinLon)) / 2);
        float y = (float)((MercatorProjection.latToY(MaxLat) + MercatorProjection.latToY(MinLat)) / 2);

        // Dies wird bei der Generierung von Busstraßen, Schienen und Stationen als Referenz genutzt
		// (manuell generierte Objekte). Dient zur Zentrierung.
        Centre = new Vector3(x, 0, y);

        string x_2 = ((MaxLat + MinLat) / 2).ToString(CultureInfo.InvariantCulture);
        string y_2 = ((MaxLon + MinLon) / 2).ToString(CultureInfo.InvariantCulture);

        // Dies wird bei der Generierung der 3D Gebäude und Straßen (durch Mapbox) als Referenzpunkt genutzt.
		// Dient zur Zentrierung.
        BuildingCentre = x_2 + ", " + y_2;

        // Dies wird bei der Generierung der 3D Gebäude und Straßen (durch Mapbox) für die Festlegung 
		// der Dimensionen genutzt. Dient zur Dimensionssetzung.
        SetMapboxFactor(MaxLon, MinLon, MaxLat, MinLat);
    }

	// Diese Funktion nimmt als Parameter die Dimensionen des ausgewählten Fenster (aus OpenStreetMap) und
	// nutzt diese um die Dimensionen der Mapbox Objekte anzupassen. Diese Werte werden dann der Mapbox-SDK übergeben
	// da diese mit zwei Größenfaktoren arbeitet.
    void SetMapboxFactor(float MaxLon, float MinLon, float MaxLat, float MinLat)
    {
        float lenght = MaxLon - MinLon;
        float width = MaxLat - MinLat;

        int lenghtValue = Mathf.FloorToInt(lenght / 0.0237f);
        int widthValue = Mathf.FloorToInt(width / 0.017f) + 1;

        if(lenghtValue == 0)
        {
            lenghtValue = 1;
        }
        if(widthValue == 0)
        {
            widthValue = 1;
        }

        LenghtFactor = lenghtValue + 1;
        WidthFactor = widthValue + 1;
    }
}

