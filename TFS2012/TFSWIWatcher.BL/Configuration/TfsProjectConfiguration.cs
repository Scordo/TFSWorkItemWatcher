using System;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;

namespace TFSWIWatcher.BL.Configuration
{
	public class TfsProjectConfiguration
	{
		#region Members

		private ReadOnlyCollection<string> _workitemTypes;

		#endregion

		#region Properties

		public string Name { get; private set; }
		public ReadOnlyCollection<string> WorkitemTypes 
		{ 
			get { return _workitemTypes ?? (_workitemTypes = new List<string>().AsReadOnly()); }
			private set { _workitemTypes = value; } 
		}

		#endregion

		#region Constructor

		public TfsProjectConfiguration(XmlNode projectNode)
		{
			if (projectNode == null)
				throw new ArgumentNullException("projectNode");

			XmlAttribute nameAttribute = projectNode.Attributes["name"];

			if (nameAttribute == null)
				throw new ConfigurationErrorsException("Could not find name-Attribute in Project-Node.", projectNode);

			Name = nameAttribute.Value.Trim();

			List<string> workitemTypes = new List<string>();
			
			foreach (XmlNode witNode in projectNode.SelectNodes("WIT"))
			{
				XmlAttribute includeAttribute = witNode.Attributes["include"];

				if (includeAttribute == null)
					throw new ConfigurationErrorsException("Could not find include-Attribute in WIT-Node.", witNode);

				workitemTypes.Add(includeAttribute.Value.Trim());
			}

			WorkitemTypes = workitemTypes.AsReadOnly();
		}

		#endregion

		#region Methods

		public bool IsWorkitemTypeSupported(string workitemType)
		{
			if (workitemType == null)
				throw new ArgumentNullException("workitemType");

			return WorkitemTypes.Count == 0 || WorkitemTypes.Contains(workitemType);
		}

		#endregion
	}
}