using System.Configuration;
using System.IO;
using System.Xml.Linq;

namespace TFSWIWatcher.BL.Configuration
{
    public class MailNotifyConfigSettingsDeserializer
    {
        public static MailNotifyConfigSettings LoadFromXElement(XElement element)
        {
            MailNotifyConfigSettings settings = new MailNotifyConfigSettings();

            XElement mailTransformationFileElement = element == null? null : element.Element("MailTransformationFile");

            if (mailTransformationFileElement != null)
                settings.MailTransformationFile = mailTransformationFileElement.Value.Trim();
            else
                settings.MailTransformationFile = Path.GetFullPath(Path.Combine(Util.GetPluginAssemblyFolderPath(), @"..\..\..\TFSJobAgent\Transforms\1033\WorkItemChangedEvent.xsl"));

            if (settings.MailTransformationFile.Length == 0)
                throw new ConfigurationErrorsException("MailTransformationFile-Node is empty in MailNotifyConfig-Node.");

            if (!Path.IsPathRooted(settings.MailTransformationFile))
                settings.MailTransformationFile = Path.GetFullPath(Path.Combine(Util.GetPluginAssemblyFolderPath(), settings.MailTransformationFile));

            if (!File.Exists(settings.MailTransformationFile))
                throw new ConfigurationErrorsException(string.Format("MailTransformationFile-Node has an invalid path in it: {0}", settings.MailTransformationFile));

            XElement devMailElement = element == null? null : element.Element("DevMail");

            if (devMailElement != null)
                settings.DevMail = devMailElement.Value.Trim();

            return settings;
        }
    }
}