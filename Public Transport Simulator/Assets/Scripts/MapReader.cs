using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.SceneManagement;

// Dieses Skript extrahiert die verschiedenen Datensätze aus der XML
// Datei und erzeugt die jeweiligen Objekte anhand der Klassen aus 
// dem Serialization Ordner. Die Objekte werden dann in Wörterbüchern/Listen
// gespeichert um auf diese Datenstrukturen aus anderen Skripten heraus 
// zuzugreifen. Sobald die Verarbeitung der Daten erfolgt ist wird die statische
// Variable IsReady auf True gesetzt und das Skript "MapBuilder" beginnt mit der 
// Generierung von Objekten.
class MapReader : MonoBehaviour
{
    public static OsmBounds bounds;  

    public static Dictionary<ulong, OsmNode> nodes;  

    public static Dictionary<ulong, OsmWay> ways;  

    public static List<OsmRelation> relations; 

    public static bool IsReady { get; private set; }  

    void Start()
    {
        nodes = new Dictionary<ulong, OsmNode>();
        ways = new Dictionary<ulong, OsmWay>();
        relations = new List<OsmRelation>();

        // Falls der Nutzer eine txt. Datei eingibt die nicht in XML Format
        // vorliegt, wird dies abgefangen und eine Fehlermeldung wird
        // produziert die anhand dieser Variable aktiviert wird.
        FileLoader.simulator_loaded = true;  

        XmlDocument doc = new XmlDocument();
        try
        {
            // Hier wird die XML-Datei geladen und die verschiedenen Datensatztypen
            // werden extrahiert anhand der jeweiligen Funktion.
            doc.Load(FileLoader.ResourceFilePath);
            SetBounds(doc.SelectSingleNode("/osm/bounds"));
            if(UserPreferences.PublicTransportStreets || UserPreferences.PublicTransportRailways || UserPreferences.Stations)
            {
                GetNodes(doc.SelectNodes("/osm/node"));
            }
            if(UserPreferences.PublicTransportStreets || UserPreferences.PublicTransportRailways)
            {
                GetWays(doc.SelectNodes("/osm/way"));
            }
            if(UserPreferences.PublicTransportStreets || UserPreferences.PublicTransportRailways || UserPreferences.Stations)
            {
                GetRelations(doc.SelectNodes("/osm/relation"));
            }
        }
        // Im Falle eines Fehlers wird der Nutzer zurück ins Hauptmenü verwiesen
        // und eine Fehlermeldung erscheint, da FileLoader.simulator_loaded auf
        // True gesetzt wurde.
        catch
        {
            SceneManager.LoadScene(0); 
        }

        // Durch diese Variable wird den anderen Skripten mitgeteilt dass die
        // Datenstrukturen verarbeitet wurden und bereit sind. Ab hier übernimmt
        // "MapBuilder".
        IsReady = true;  
    }

    // Extrahiert den Bounds Datensatz aus der XML Datei und erzeugt das entsprechende
    // Objekt.
    void SetBounds(XmlNode xmlNode)
    {
        bounds = new OsmBounds(xmlNode);
    }

    // Extrahiert die Node Datensätze aus der XML Datei und erzeugt die Objekte.
    // Anschließend werden diese in einem Wörterbuch gespeichert. Falls nur 3D-
    // Gebäude und/oder Straßen gewünscht, wird diese Funktion übersprungen.
    void GetNodes(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode n in xmlNodeList)
        {
            OsmNode node = new OsmNode(n);
            nodes[node.ID] = node;
        }
    }

    // Extrahiert die Way Datensätze aus der XML Datei und erzeugt die Objekte.
    // Anschließend werden diese nur dann im Wörtebuch gespeichert wenn es sich
    // um Schienen oder Straßen handelt und dies gewünscht ist.
    // Falls nur 3D-Gebäude und/oder Stationen und/oder Straßen gewünscht, wird 
    // diese Funktion übersprungen.
    void GetWays(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode node in xmlNodeList)
        { 
            OsmWay way = new OsmWay(node);

            if (way.IsRailway == true)
            {
                if (UserPreferences.PublicTransportRailways)
                {
                    ways[way.ID] = way;
                }
            }
            else if (way.IsStreet == true)
            {
                if (UserPreferences.PublicTransportStreets)
                {
                    ways[way.ID] = way;
                }
            }
        }
    }

    // Extrahiert die Relation Datensätze aus der XML Datei und erzeugt die Objekte.
    // Falls die Relation eine Route darstellt wird diese in einer Liste gespeichert.
    // Des Weiteren wird der Relationsname und der Transporttyp auf die Nodes innerhalb
    // der NodeIDs Liste übertragen, falls Stationen gewünscht sind. Falls nur 3D-Gebäude
    // und/oder Straßen gewünscht, wird diese Funktion übersprungen.
    void GetRelations(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode node in xmlNodeList)
        {
            OsmRelation relation = new OsmRelation(node);

            if(relation.Route == true)
            {
                relations.Add(relation);

                if (UserPreferences.PublicTransportRailways || UserPreferences.PublicTransportStreets)
                {
                    TagPublicTransportWays(relation);
                }

                if (UserPreferences.Stations)
                {
                    foreach (ulong NodeID in relation.StoppingNodeIDs)
                    {
                        try
                        {
                            nodes[NodeID].TransportLines.Add(relation.Name);
                            relation.StationNames.Add(nodes[NodeID].StationName);
                        }
                        catch (KeyNotFoundException)
                        {
                            continue;
                        }
                    }
                }
            }
        }
    }

    // Falls ÖPNV-Straßen und/oder ÖPNV-Schienen gewünscht sind, werden hier
    // noch die letzten Realtionsdaten ausgewertet und in die jeweiligen
    // Ways aufgenommen.
    void TagPublicTransportWays(OsmRelation r)
    {
        foreach (ulong WayID in r.WayIDs)
        {
            try
            {
                switch (r.TransportType)
                {
                    case "subway":
                        ways[WayID].TransportLines.Add(r.Name);
                        ways[WayID].PublicTransportRailway = true;
                        ways[WayID].TransportTypes.Add("subway");
                        break;
                    case "tram":
                        ways[WayID].TransportLines.Add(r.Name);
                        ways[WayID].PublicTransportRailway = true;
                        ways[WayID].TransportTypes.Add("tram");
                        break;
                    case "train":
                        ways[WayID].TransportLines.Add(r.Name);
                        ways[WayID].PublicTransportRailway = true;
                        ways[WayID].TransportTypes.Add("train");
                        break;
                    case "railway":
                        ways[WayID].TransportLines.Add(r.Name);
                        ways[WayID].PublicTransportRailway = true;
                        ways[WayID].TransportTypes.Add("railway");
                        break;
                    case "light_rail":
                        ways[WayID].TransportLines.Add(r.Name);
                        ways[WayID].PublicTransportRailway = true;
                        ways[WayID].TransportTypes.Add("light_rail");
                        break;
                    case "bus":
                        ways[WayID].TransportLines.Add(r.Name);
                        ways[WayID].PublicTransportStreet = true;
                        ways[WayID].TransportTypes.Add("bus");
                        break;
                }
            }
            catch (KeyNotFoundException)
            {
                continue;
            }
        }
    }
}
