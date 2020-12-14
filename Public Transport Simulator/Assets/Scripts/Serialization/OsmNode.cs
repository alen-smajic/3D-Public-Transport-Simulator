using System.Xml;
using UnityEngine;
using System.Collections.Generic;

// Diese Klasse nutzt die Node Datensätze aus der XML Datei und erzeugt
// die jeweiligen Objekte daraus. Anhand dieser Objekte werden die Ways
// (Straßen und Schienen) zusammengesetzt aber auch die Stationspunkte 
// definiert.
class OsmNode : BaseOsm
{
    public ulong ID { get; private set; }

    public float Latitude { get; private set; }

    public float Longitude { get; private set; }

    public float X { get; private set; }

    public float Y { get; private set; }

    public string StationName { get; private set; }

    public bool StationCreated { get; set; } 

    public List<string> TransportLines { get; set; }

    public static implicit operator Vector3(OsmNode node)
    {
        return new Vector3(node.X, 0, node.Y);
    }

    public OsmNode(XmlNode node)
    {
        TransportLines = new List<string>();

        ID = GetAttribute<ulong>("id", node.Attributes);

        Latitude = GetFloat("lat", node.Attributes);
        Longitude = GetFloat("lon", node.Attributes);

        // Die Flächen- und Breitenkoordinaten werden durch die Mercator-Projection in Unity
        // Koordinaten umgewandelt (also Longitude und Latitude einer Kugel werden auf X und 
        // Y einer Fläche abgebildet).
        X = (float)MercatorProjection.lonToX(Longitude);
        Y = (float)MercatorProjection.latToY(Latitude);

        findName(node);
    }

    // Jedem Node wird ein Name zugewiesen falls dieser im Datensatz vorhanden. Wird für die 
    // Benamung der Stationen benötigt.
    public void findName(XmlNode node)
    {
        XmlNodeList tags = node.SelectNodes("tag");
        foreach (XmlNode t in tags)
        {
            string key = GetAttribute<string>("k", t.Attributes);
            if (key == "name")
            {
                StationName = GetAttribute<string>("v", t.Attributes);
            }
        }
    }
}
