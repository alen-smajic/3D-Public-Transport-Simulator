using System.Xml;
using UnityEngine;
using System.Collections.Generic;

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
/// Extracts the Node data from the OSM XML file and stores its values.
/// This data includes information about object position, roads, railroads,
/// stations etc.
/// </summary>
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

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="node">XML node</param>
    public OsmNode(XmlNode node)
    {
        TransportLines = new List<string>();

        ID = GetAttribute<ulong>("id", node.Attributes);

        Latitude = GetFloat("lat", node.Attributes);
        Longitude = GetFloat("lon", node.Attributes);

        // Longitude and Latitude are being converted to Unity coordinates using the 
        // Mercator projection.
        X = (float)MercatorProjection.lonToX(Longitude);
        Y = (float)MercatorProjection.latToY(Latitude);

        findName(node);
    }

    /// <summary>
    /// Every instance stores the name of the node for the case its a station.
    /// </summary>
    /// <param name="node">XML node</param>
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
