using System;
using System.Xml.Serialization;

namespace TFSWIWatcher.BL.WorkItemRelated
{
    [XmlType, Serializable]
    public class WorkItemIntegerField
    {
        public string Name { get; set; }

        public string ReferenceName { get; set; }

        public int OldValue { get; set; }

        public int NewValue { get; set; }
    }
}