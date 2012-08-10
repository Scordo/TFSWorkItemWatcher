using System;
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

            XElement mailTransformationFileElement = element.Element("MailTransformationFile");

            if (mailTransformationFileElement == null)
                throw new ConfigurationErrorsException("Could not find MailTransformationFile-Node in MailNotifyConfig-Node.");

            settings.MailTransformationFile = mailTransformationFileElement.Value.Trim();

            if (settings.MailTransformationFile.Length == 0)
                throw new ConfigurationErrorsException("MailTransformationFile-Node is empty in MailNotifyConfig-Node.");

            if (!Path.IsPathRooted(settings.MailTransformationFile))
            {
                string currentDirectory = Path.GetDirectoryName(new Uri(typeof(WorkItemChangedSubscriber).Assembly.CodeBase).LocalPath);
                settings.MailTransformationFile = Path.Combine(currentDirectory, settings.MailTransformationFile);
            }

            if (!File.Exists(settings.MailTransformationFile))
                throw new ConfigurationErrorsException(string.Format("MailTransformationFile-Node has an invalid path in it: {0}", settings.MailTransformationFile));

            XElement devMailElement = element.Element("DevMail");

            if (devMailElement != null)
                settings.DevMail = devMailElement.Value.Trim();

            return settings;
        }
    }
}