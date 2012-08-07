using System;
using System.Xml;
using System.Collections.Generic;

namespace TFSWIWatcher.BL.Configuration
{
	public class TfsProjectConfigurationList : List<TfsProjectConfiguration>
	{
		#region Constructor

		public TfsProjectConfigurationList() 
		{
			
		}

		public TfsProjectConfigurationList(XmlNode node)
		{
			if (node == null)
				return;

			foreach (XmlNode projectNode in node.SelectNodes("Project"))
			{
				Add(new TfsProjectConfiguration(projectNode));
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
			
			return config == null ? false : config.IsWorkitemTypeSupported(workitemType);
		}

		#endregion
	}
}