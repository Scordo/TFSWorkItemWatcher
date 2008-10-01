using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TFSWIWatcher.BL.WorkItemRelated
{
    [XmlType, Serializable]
    public class WorkItemChangedFields
    {
        [XmlArrayItem("Field", IsNullable = false)]
        public List<WorkItemIntegerField> IntegerFields { get; set; }

        [XmlArrayItemAttribute("Field", IsNullable = false)]
        public List<WorkItemStringField> StringFields { get; set; }
    }
}