using System.Collections.Generic;
using System.Xml;

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
/// This class contains information about the public transport lines. Using
/// these information, we can determine which ways are railroads and which 
/// are roads. We can also determine which nodes are stations.
/// </summary>
class OsmRelation : BaseOsm
{
    public bool Route { get; private set; }

    public string TransportType { get; private set; }

    public string Name { get; private set; }

    public List<ulong> StoppingNodeIDs { get; private set; }

    public List<ulong> WayIDs { get; private set; }

    public List<string> StationNames { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="node">XML node</param>
    public OsmRelation(XmlNode node)
    {
        StoppingNodeIDs = new List<ulong>();
        WayIDs = new List<ulong>();
        StationNames = new List<string>();

        IsPublicRoute(node);
    }

    /// <summary>
    /// Checks the tags of the Relation nodes if it contains public transport
    /// information. 
    /// </summary>
    /// <param name="node">XML node</param>
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

    /// <summary>
    /// Stores the name of the relation nodes.
    /// </summary>
    /// <param name="node">XML node</param>
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

    /// <summary>
    /// Stores the IDs of the ways and nodes which are contained within the
    /// relation nodes.
    /// </summary>
    /// <param name="node">XML node</param>
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