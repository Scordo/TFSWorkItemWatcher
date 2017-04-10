# TFSWorkItemWatcher
TFSWIWatcher is a service which allows you to register for watching workitems in Team Foundation Server. The service can be configured to use different logic to retrieve accounts to be notified and to deliver messages to the accounts to be notified.

By default TFSWIWatcher watches all changes of workitems and determines the accounts to be notified by looking up the texbox in a new tab of the workitem called Observers. This tab holds a list of account names - each account name on a singe line:

![Observer-Tab](https://github.com/Scordo/TFSWorkItemWatcher/blob/master/TFSWIWatcher.Resources/Documentation/Resources/ObserverTab.jpg)

TFSWIWatcher will then determine the email addresses of the provided account names and will send a mail to each account.

**Features**
* different possibilities to authenticate against tfs 
* multiple possibilities to determine accounts to notify 
* multiple possibilities to notify accounts 
* possibility to extend the authentication, observer and notify mechanism
