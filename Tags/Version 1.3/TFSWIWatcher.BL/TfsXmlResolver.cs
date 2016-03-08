using System;
using System.IO;
using System.Xml;

namespace TFSWIWatcher.BL
{
    public class TfsXmlResolver : XmlUrlResolver
    {
        private string XslParentFolderPath { get; set; }

        public TfsXmlResolver(string xslFilePath)
        {
            XslParentFolderPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(xslFilePath), @"..\"));
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            try
            {
                return base.GetEntity(absoluteUri, role, ofObjectToReturn);
            }
            catch (FileNotFoundException)
            {
                if (absoluteUri.IsFile && absoluteUri.AbsoluteUri.EndsWith("TeamFoundation.xsl"))
                {
                    absoluteUri = new Uri(Path.Combine(XslParentFolderPath, "TeamFoundation.xsl"));
                    return base.GetEntity(absoluteUri, role, ofObjectToReturn);
                }
                
                throw;
            }
        }
    }
}