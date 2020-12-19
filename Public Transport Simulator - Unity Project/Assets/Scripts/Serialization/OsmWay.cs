using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// This software has been further expanded by Alen Smajic (2020).

/*
    Copyright (c) 2017 Sloan Kelly

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
/// This class uses the way data from the XML files and stores its information.
/// It contains information about the roads and the railroads.
/// </summary>
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

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="node">XML node</param>
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

    /// <summary>
    /// Checks the tags of the way data if it contains road or railroad information.
    /// </summary>
    /// <param name="node">XML node</param>
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

    /// <summary>
    /// Fills the NodeID list with the Node IDs that are part of the way.
    /// </summary>
    /// <param name="node">XML node</param>
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

