using System;
using System.Globalization;
using System.Xml;

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
/// This class is being inherited by the other classes within the Serialization folder
/// and is being used for data extraction from the OSM XML files.
/// </summary>
class BaseOsm
{
    /// <summary>
    /// Exctracts the data values from the data source and converts their data type.
    /// </summary>
    /// <typeparam name="T">data type</typeparam>
    /// <param name="attrName">name of the attribute</param>
    /// <param name="attributes">the collection of attributes within the data</param>
    /// <returns>The value of the attribute converted to the required type</returns>
    protected T GetAttribute<T>(string attrName, XmlAttributeCollection attributes)
    {
        string strValue = attributes[attrName].Value;
        return (T)Convert.ChangeType(strValue, typeof(T));
    }

    /// <summary>
    /// Extracts the data values from the data source wich are of type float.
    /// </summary>
    /// <param name="attrName">name of the attribute</param>
    /// <param name="attributes">the collection of attributes within the data</param>
    /// <returns>The value of the attribute converted to float</returns>
    protected float GetFloat(string attrName, XmlAttributeCollection attributes)
    {
        string strValue = attributes[attrName].Value;
        return float.Parse(strValue, new CultureInfo("en-US").NumberFormat);
    }
}



