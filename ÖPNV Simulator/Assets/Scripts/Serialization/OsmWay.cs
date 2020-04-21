using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// Diese Klasse nutzt die Way Datensätze aus der XML Datei und erzeugt
// die jeweiligen Objekte daraus. Anhand dieser Objekte werden die
// Bus-Straßen und Schienen erzeugt.
class OsmWay : BaseOsm
{
    public ulong ID { get; private set; }

    public List<ulong> NodeIDs { get; private set; }

    public bool IsRailway { get; private set; }

    public bool PublicTransportRailway { get; set; }

    public bool IsStreet { get; private set; }

    public bool PublicTransportStreet { get; set; }

    public List<string> TransportTypes { get; set; }

    public List<string> TransportLines { get; set; }

    public List<Vector3> UnityCoordinates { get; set; }

    public OsmWay(XmlNode node)
    {
        NodeIDs = new List<ulong>();
        TransportTypes = new List<string>();
        TransportLines = new List<string>();
        UnityCoordinates = new List<Vector3>();

        ID = GetAttribute<ulong>("id", node.Attributes);

        Tagger(node);

        if ((UserPreferences.PublicTransportRailways && IsRailway == true) || (UserPreferences.PublicTransportStreets && IsStreet == true))
        {
            NodeIDsCreator(node);
        }
    }

    // Durchsucht die "Tags" der Way Datensätze um festzustellen ob es
    // sich um eine Schiene oder eine Straße behandelt.
    void Tagger(XmlNode node)
    {
        XmlNodeList tags = node.SelectNodes("tag");
        foreach (XmlNode t in tags)
        {
            string key = GetAttribute<string>("k", t.Attributes);
            if (key == "railway")
            {
                IsRailway = true;
            }
            else if (key == "highway")
            {
                string value = GetAttribute<string>("v", t.Attributes);
                List<string> AllowedTags = new List<string> { "motorway", "trunk", "primary", "secondary", "tertiary", "unclassified", "residential", "motorway_link", "trunk_link", "primary_link", "secondary_link", "tertiary_link", "road", "living_street", "service" };
                if (AllowedTags.Contains(value))
                {
                    IsStreet = true;
                }
            }
        }
    }

    // Füllt die NodeIDs Liste mit den ganzen IDs welche Bestandteil des 
    // Ways sind.
    void NodeIDsCreator(XmlNode node)
    {
        XmlNodeList nds = node.SelectNodes("nd");
        foreach (XmlNode n in nds)
        {
            ulong refNo = GetAttribute<ulong>("ref", n.Attributes);
            NodeIDs.Add(refNo);
        }
    }
}

