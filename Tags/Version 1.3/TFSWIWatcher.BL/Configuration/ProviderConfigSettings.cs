namespace TFSWIWatcher.BL.Configuration
{
    public class ProviderConfigSettings
    {
        #region Properties

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        /// <value>The name of the assembly.</value>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets the adapter provider class.
        /// </summary>
        /// <value>The adapter provider class.</value>
        public string ProviderClass { get; set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public string Parameters { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ProviderConfigSettings"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        #endregion

        #region Constructor

        public ProviderConfigSettings()
        {
            Enabled = true;
        }

        #endregion

    }
}