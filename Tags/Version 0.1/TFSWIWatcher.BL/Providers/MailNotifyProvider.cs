﻿using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;
using System.Net.Mail;
using System.Collections.Generic;
using log4net;

namespace TFSWIWatcher.BL.Providers
{
    public class MailNotifyProvider : INotifyProvider
    {
        #region Non Public Members

        private MailNotifyConfigSection _config;
        private static readonly ILog _log = LogManager.GetLogger(typeof(MailNotifyProvider));

        #endregion

        #region INotifyProvider Members

        void INotifyProvider.Initialize()
        {
            _log.Debug("Start: Initializing.");
            
            try
            {
                _config = MailNotifyConfigSection.GetFromConfig();
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error while trying to read config: {0}", ex);
                throw;
            }
            
            _log.Debug("Finish: Initializing.");
        }

        void INotifyProvider.Notify(List<string> observerAccounts, WorkItemChangedContext context)
        {
            _log.DebugFormat("Start: Notifying {0} Accounts.", observerAccounts.Count);

            foreach (string observerAccount in observerAccounts)
            {
                Notify(observerAccount, context);
            }

            _log.DebugFormat("Finish: Notifying {0} Accounts.", observerAccounts.Count);
        }

        #endregion

        #region Non Public Methods

        private void Notify(string observerAccount, WorkItemChangedContext context)
        {
            _log.DebugFormat("Start: Notifying Account {0}.", observerAccount);
            string email;

            if (ConfigurationManager.AppSettings["IsDev"] == "1" && _config.DevMail != null)
            {
                _log.DebugFormat("Running in dev-mode using mail {0}.", _config.DevMail);
                email = _config.DevMail;
            }
            else
                email = TFSHelper.GetEMailOfUser(context.TeamServer, observerAccount.Trim());

            if (email != null && email.Trim().Length > 0)
            {
                _log.DebugFormat("Email for Account {0} is {1}.", observerAccount, email); 
                SendMail(observerAccount, email, context);
            }
            else
                _log.WarnFormat("Could not determine email for Account {0}.", observerAccount);

            _log.DebugFormat("Finish: Notifying Account {0}.", observerAccount);
        }

        private void SendMail(string observerAccount, string email, WorkItemChangedContext context)
        {
            _log.DebugFormat("Start: Sending mail to account {0} using email {1}.", observerAccount, email);

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(new MailAddress(email, observerAccount));

                //set the content
                mail.Subject = string.Format("Workitem {0} [{1}] has changed...", context.WorkItemID, context.WorkItemChangeInfo.WorkItemTitle);
                mail.Body = GetTransformedHtml(context.NotifyXML);
                mail.IsBodyHtml = true;

                //send the message
                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);

            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error whiel sending mail to {0} for account {1}: {2}", email, observerAccount, ex);
                throw;
            }

            _log.DebugFormat("Finish: Sending mail to account {0} using email {1}.", observerAccount, email);
        }

        private string GetTransformedHtml(string xml)
        {
            try
            {
                string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                _log.DebugFormat("Setting current directory to {0}", currentPath);
                Directory.SetCurrentDirectory(currentPath);

                using (TextReader styleTextReader = new StreamReader(_config.MailTransformationFile))
                {
                    using (XmlReader styleXmlReader = new XmlTextReader(styleTextReader))
                    {
                        using (TextReader workitemTextReader = new StringReader(xml))
                        {
                            using (XmlReader workitemXmlReader = new XmlTextReader(workitemTextReader))
                            {
                                using (TextWriter resultTextWriter = new StringWriter())
                                {
                                    using (XmlWriter resultXmlWriter = new XmlTextWriter(resultTextWriter))
                                    {
                                        XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
                                        xslCompiledTransform.Load(styleXmlReader);

                                        xslCompiledTransform.Transform(workitemXmlReader, resultXmlWriter);

                                        return resultTextWriter.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error while transforming notify-xml to html using file {0}: {1}", _config.MailTransformationFile, ex);
                throw;
            }
        }

        #endregion
    }
}