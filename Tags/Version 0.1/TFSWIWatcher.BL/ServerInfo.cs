using System;
using System.Xml.Serialization;

namespace TFSWIWatcher.BL
{
    [XmlRoot(IsNullable = false, ElementName = "TeamFoundationServer"), XmlType(AnonymousType = true), Serializable]
    public class ServerInfo
    {
        [XmlAttribute("url")]
        public string Url { get; set; }
    }
}