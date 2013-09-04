using System;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Xml.Linq;

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

		public TfsProjectConfiguration(XElement projectElement)
		{
			if (projectElement == null)
				throw new ArgumentNullException("projectElement");

			XAttribute nameAttribute = projectElement.Attribute("name");

			if (nameAttribute == null)
				throw new ConfigurationErrorsException("Could not find name-Attribute in Project-Node.");

			Name = nameAttribute.Value.Trim();

			List<string> workitemTypes = new List<string>();
			
			foreach (XElement witElement in projectElement.Elements("WIT"))
			{
				XAttribute includeAttribute = witElement.Attribute("include");

				if (includeAttribute == null)
					throw new ConfigurationErrorsException("Could not find include-Attribute in WIT-Node.");

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