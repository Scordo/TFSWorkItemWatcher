using System;
using System.Xml.Serialization;

namespace TFSWIWatcher.BL.WorkItemRelated
{
    [XmlType, Serializable]
    public class WorkItemStringField
    {
        public string Name { get; set; }

        public string ReferenceName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}