using System;
using System.Xml.Serialization;

namespace TFSWIWatcher.BL.WorkItemRelated
{
    [XmlType, Serializable]
    public class WorkItemTextField
    {
        public string Name { get; set; }

        public string ReferenceName { get; set; }

        public string Value { get; set; }
    }
}