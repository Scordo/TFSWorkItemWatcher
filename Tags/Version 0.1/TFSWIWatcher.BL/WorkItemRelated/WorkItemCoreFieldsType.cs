using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TFSWIWatcher.BL.WorkItemRelated
{
    [XmlType, Serializable]
    public class WorkItemCoreFieldsType
    {
        [XmlArrayItemAttribute("Field", IsNullable = false)]
        public List<WorkItemIntegerField> IntegerFields { get; set; }

        [XmlArrayItem("Field", IsNullable = false)]
        public List<WorkItemStringField> StringFields { get; set; }
    }
}