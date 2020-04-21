using System.Collections.Generic;
using System.Xml;

// Diese Klasse nutzt die Relation Datensätze aus der XML Datei und erzeugt
// die jeweiligen Objekte daraus. Anhand dieser Objekte werden die einzelnen
// Ways kategorisiert und es wird definiert welche Punkte Stationen darstellen
// und welche Ways für den ÖPNV genutzt werden.
class OsmRelation : BaseOsm
{
    public bool Route { get; private set; }

    public string TransportType { get; private set; }

    public string Name { get; private set; }

    public List<ulong> StoppingNodeIDs { get; private set; }

    public List<ulong> WayIDs { get; private set; }

    public List<string> StationNames { get; set; }

    public OsmRelation(XmlNode node)
    {
        StoppingNodeIDs = new List<ulong>();
        WayIDs = new List<ulong>();
        StationNames = new List<string>();

        IsPublicRoute(node);
    }

    // Durchsucht die "Tags" der Relation Datensätze um festzustellen ob diese
    // Relation eine Bus oder Zug-Route ist. Falls es sich um eine Zug-Route handelt 
    // wird der Zugtyp erfragt. Des Weiteren werden dann auch der Name der Relation
    // und die ganzen Members der Relation aufgegriffen.
    void IsPublicRoute(XmlNode node)
    {
        XmlNodeList tags = node.SelectNodes("tag");
        foreach (XmlNode t in tags)
        {
            string key = GetAttribute<string>("k", t.Attributes);
            if (key == "route")
            {
                string value = GetAttribute<string>("v", t.Attributes);
                switch (value)
                {
                    case "subway":
                        TransportType = "subway";
                        Route = true;
                        NameFinder(node);
                        RelationMembers(node);
                        break;
                    case "tram":
                        TransportType = "tram";
                        Route = true;
                        NameFinder(node);
                        RelationMembers(node);
                        break;
                    case "train":
                        TransportType = "train";
                        Route = true;
                        NameFinder(node);
                        RelationMembers(node);
                        break;
                    case "railway":
                        TransportType = "railway";
                        Route = true;
                        NameFinder(node);
                        RelationMembers(node);
                        break;
                    case "light_rail":
                        TransportType = "light_rail";
                        Route = true;
                        NameFinder(node);
                        RelationMembers(node);
                        break;
                    case "bus":
                        TransportType = "bus";
                        Route = true;
                        NameFinder(node);
                        RelationMembers(node);
                        break;
                }
            }
        }
    }

    // Greift auf den Namen des Relation-Datensatzes zu.
    void NameFinder(XmlNode node)
    {
        XmlNodeList tags = node.SelectNodes("tag");
        foreach(XmlNode t in tags)
        {
            string key = GetAttribute<string>("k", t.Attributes);
            if (key == "name")
            {
                Name = GetAttribute<string>("v", t.Attributes);
            }
        }
    }

    // Speichert die IDs der Ways welche Bestandteil der Relation sind.
    // Speichert die IDs der Nodes welche die Haltepositionen der Route
    // darstellen.
    void RelationMembers(XmlNode node)
    {
        XmlNodeList members = node.SelectNodes("member");
        foreach (XmlNode n in members)
        {
            string type = GetAttribute<string>("type", n.Attributes);
            if (type == "node")
            {
                string role = GetAttribute<string>("role", n.Attributes);
                if (role == "stop")
                {
                    ulong refNo = GetAttribute<ulong>("ref", n.Attributes);
                    StoppingNodeIDs.Add(refNo);
                }
            }
            else if (type == "way")
            {
                string role = GetAttribute<string>("role", n.Attributes);
                if (role != "platform")
                {
                    ulong refNo = GetAttribute<ulong>("ref", n.Attributes);
                    WayIDs.Add(refNo);
                }
            }
        }
    }
}