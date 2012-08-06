using System;
using System.Xml.Serialization;

namespace TFSWIWatcher.BL.WorkItemRelated
{
    [XmlRoot(ElementName = "WorkItemChangedEvent", IsNullable = false), XmlType(AnonymousType = true), Serializable]
    public class WorkItemChangeInfo
    {
        public string PortfolioProject { get; set; }
        
        public string ProjectNodeId { get; set; }
        
        public string AreaPath { get; set; }
        
        public string Title { get; set; }
        
        public string WorkItemTitle { get; set; }
        
        public string Subscriber { get; set; }
        
        public string ChangerSid { get; set; }
        
        public string DisplayUrl { get; set; }
        
        public string TimeZone { get; set; }
        
        public string TimeZoneOffset { get; set; }
        
        public WorkItemChangeType ChangeType { get; set; }
        
        public WorkItemCoreFieldsType CoreFields { get; set; }

        [XmlArrayItemAttribute("Field", IsNullable = false)]
        public WorkItemTextField[] TextFields { get; set; }

        public WorkItemChangedFields ChangedFields { get; set; }

        [XmlArrayItemAttribute("Name", IsNullable = false)]
        public string[] AddedFiles { get; set; }

        [XmlArrayItemAttribute("FileId", IsNullable = false)]
        public string[] DeletedFiles { get; set; }

        [XmlArrayItemAttribute("Resource", IsNullable = false)]
        public string[] AddedResourceLinks { get; set; }

        [XmlArrayItemAttribute("LinkId", IsNullable = false)]
        public string[] DeletedResourceLinks { get; set; }

        [XmlArrayItemAttribute("LinkId", IsNullable = false)]
        public string[] ChangedResourceLinks { get; set; }

        [XmlArrayItemAttribute("WorkItemId", IsNullable = false)]
        public string[] AddedRelations { get; set; }

        [XmlArrayItemAttribute("WorkItemId", IsNullable = false)]
        public string[] DeletedRelations { get; set; }

        [XmlArrayItem("WorkItemId", IsNullable = false)]
        public string[] ChangedRelations { get; set; }
    }
}
