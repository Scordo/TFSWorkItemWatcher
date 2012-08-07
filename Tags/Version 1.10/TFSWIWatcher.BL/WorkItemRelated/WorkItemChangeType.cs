using System;
using System.Xml.Serialization;

namespace TFSWIWatcher.BL.WorkItemRelated
{
    [XmlType, Serializable]
    public enum WorkItemChangeType
    {
        /// <summary>
        /// The Work Item has been created
        /// </summary>
        New,

        /// <summary>
        /// The Work Item has been changed
        /// </summary>
        Change
    }
}