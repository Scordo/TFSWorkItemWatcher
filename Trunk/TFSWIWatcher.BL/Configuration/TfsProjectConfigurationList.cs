using System;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TFSWIWatcher.BL.Configuration
{
	public class TfsProjectConfigurationList : List<TfsProjectConfiguration>
	{
		#region Constructor

		public TfsProjectConfigurationList() 
		{
			
		}

		public TfsProjectConfigurationList(XElement element)
		{
			if (element == null)
				return;

			foreach (XElement projectElement in element.Elements("Project"))
			{
				Add(new TfsProjectConfiguration(projectElement));
			}
		}

		public TfsProjectConfigurationList(IEnumerable<TfsProjectConfiguration> items): base(items)
		{

		}

		public TfsProjectConfigurationList(int capacity) : base(capacity)
		{

		}

		#endregion

		#region Methods

		public bool IsWorkitemTypeSupported(string project, string workitemType)
		{
			if (project == null)
				throw new ArgumentNullException("project");

			if (Count == 0)
				return true;

			TfsProjectConfiguration config = Find(p => p.Name == project);
			
			return config != null && config.IsWorkitemTypeSupported(workitemType);
		}

		#endregion
	}
}