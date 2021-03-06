﻿using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.WorkItemTracking.Server;
using log4net;
using Microsoft.TeamFoundation.Framework.Server;
using TFSWIWatcher.BL.Configuration;

namespace TFSWIWatcher.BL.Providers
{
    public class MailNotifyProvider : INotifyProvider
    {
        #region Non Public Members

        private MailNotifyConfigSettings _config;
        private static readonly ILog _log = LogManager.GetLogger(typeof(MailNotifyProvider));
        
        #endregion

        #region INotifyProvider Members

        void INotifyProvider.Initialize(XElement configRootElement)
        {
            _log.Debug("Start: Initializing.");
            
            try
            {
                _config = MailNotifyConfigSettingsDeserializer.LoadFromXElement(configRootElement.Element("MailNotifyConfig"));
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

            if (context.ConfigSettings.IsDevelopment && _config.DevMail != null)
            {
                _log.DebugFormat("Running in dev-mode using mail {0}.", _config.DevMail);
                email = _config.DevMail;
            }
            else
                email = GetEMailOfUser(context.TeamProjectCollection, observerAccount.Trim());

            if (email != null && email.Trim().Length > 0)
            {
                _log.DebugFormat("Email for Account {0} is {1}.", observerAccount, email); 
                SendMail(observerAccount, email, context);
            }
            else
                _log.WarnFormat("Could not determine email for Account {0}. WorkItem-ID: {1}", observerAccount, context.WorkItemID);

            _log.DebugFormat("Finish: Notifying Account {0}.", observerAccount);
        }

        private void SendMail(string observerAccount, string email, WorkItemChangedContext context)
        {
            _log.DebugFormat("Start: Sending mail to account {0} using email {1}.", observerAccount, email);

            try
            {
                IVssRequestContext deploymentRequestContext = context.RequestContext.To(TeamFoundationHostType.Deployment);
                TeamFoundationMailService mailService = deploymentRequestContext.GetService<TeamFoundationMailService>();

                MailMessage mail = new MailMessage();
                mail.To.Add(new MailAddress(email, observerAccount));

                //set the content
                mail.Subject = $"Workitem {context.WorkItemID} [{context.WorkItemChangeInfo.WorkItemTitle}] has changed...";
                mail.Body = GetTransformedHtml(context.WorkItemChangedEvent);
                mail.IsBodyHtml = true;

                //send the message
                mailService.QueueMailJob(deploymentRequestContext, mail);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error while sending mail to {0} for account {1}: {2}", email, observerAccount, ex);
                throw;
            }

            _log.DebugFormat("Finish: Sending mail to account {0} using email {1}.", observerAccount, email);
        }

        private string GetTransformedHtml(WorkItemChangedEvent workItemChangedEvent)
        {
            try
            {
                using (TextReader styleTextReader = new StreamReader(_config.MailTransformationFile, Encoding.UTF8))
                {
                    using (XmlReader styleXmlReader = new XmlTextReader(styleTextReader))
                    {
                        using (TextReader workitemTextReader = new StringReader(SerializeToXml(workItemChangedEvent)))
                        {
                            using (XmlReader workitemXmlReader = new XmlTextReader(workitemTextReader))
                            {
                                using (TextWriter resultTextWriter = new StringWriter())
                                {
                                    using (XmlWriter resultXmlWriter = new XmlTextWriter(resultTextWriter))
                                    {
                                        XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
                                        xslCompiledTransform.Load(styleXmlReader, null, new TfsXmlResolver(_config.MailTransformationFile));
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

        private string SerializeToXml(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            using (StringWriter writer = new StringWriter())
            {
                new XmlSerializer(instance.GetType()).Serialize(writer, instance);

                return writer.ToString();
            }
        }

        /// <summary>
        /// Gets the E mail of user.
        /// </summary>
        /// <param name="projectCollection">The project collection.</param>
        /// <param name="domainAndUsername">The domain and username.</param>
        /// <returns></returns>
        public static string GetEMailOfUser(TfsTeamProjectCollection projectCollection, string domainAndUsername)
        {
            if (domainAndUsername != null && domainAndUsername.Contains("@"))
            {
                // username is an email address --> return the email
                return domainAndUsername.Trim();
            }
            
            IIdentityManagementService identityManagement = (IIdentityManagementService)projectCollection.GetService(typeof(IIdentityManagementService));
            TeamFoundationIdentity identity = identityManagement.ReadIdentity(IdentitySearchFactor.AccountName, domainAndUsername, MembershipQuery.None, ReadIdentityOptions.ExtendedProperties | ReadIdentityOptions.IncludeReadFromSource);

            if (identity == null)
                return null;

            return GetPropertyValueAsString(identity, "ConfirmedNotificationAddress") ??
                   GetPropertyValueAsString(identity, "CustomNotificationAddress") ??
                   GetPropertyValueAsString(identity, "Mail");
        }

        private static string GetPropertyValueAsString(TeamFoundationIdentity identity, string propertyName, bool nullOrEmptyAsNull = true)
        {
            if (!identity.TryGetProperty(IdentityPropertyScope.Both, propertyName, out object propertValue))
                return null;
            string result = Convert.ToString(propertValue);

            return nullOrEmptyAsNull && string.IsNullOrWhiteSpace(result)  ? null : result;
        }

        #endregion
    }
}