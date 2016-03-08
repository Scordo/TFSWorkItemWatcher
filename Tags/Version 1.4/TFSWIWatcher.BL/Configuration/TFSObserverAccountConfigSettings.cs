using System.Text.RegularExpressions;

namespace TFSWIWatcher.BL.Configuration
{
    public class TFSObserverAccountConfigSettings
    {
        #region Properties

        public string RegexPattern { get; internal set; }
        public RegexOptions RegexOptions { get; internal set; }
        public string ObserversFieldName { get; internal set; }
        public TfsProjectConfigurationList Projects { get; internal set; }

        #endregion
    }
}