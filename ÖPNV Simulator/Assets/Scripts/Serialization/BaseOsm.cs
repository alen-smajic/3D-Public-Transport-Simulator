using System;
using System.Globalization;
using System.Xml;

// Diese Klasse wird von den anderen Klassen innerhalb des Serialization Ordners
// vererbt und dient zur extrahierung von Daten innerhalb der Datensätze der XML Datei.
class BaseOsm
{
    // Extrahiert die Daten innerhalb der Datensätze und passt deren Datentyp an.
    protected T GetAttribute<T>(string attrName, XmlAttributeCollection attributes)
    {
        string strValue = attributes[attrName].Value;
        return (T)Convert.ChangeType(strValue, typeof(T));
    }

    // Extrahiert die Daten innerhalb der Datensätze die speziell den Datentyp float haben.
    protected float GetFloat(string attrName, XmlAttributeCollection attributes)
    {
        string strValue = attributes[attrName].Value;
        return float.Parse(strValue, new CultureInfo("en-US").NumberFormat);
    }
}



