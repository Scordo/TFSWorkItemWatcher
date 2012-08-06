using System;
using System.Configuration;
using ConfigurationElement=System.Configuration.ConfigurationElement;

namespace TFSWIWatcher.BL.Configuration
{
    public abstract class ConfigurationElementCollection<T> : ConfigurationElementCollection where T : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ProviderConfigSettings"/> is deserialized.
        /// </summary>
        /// <value><c>true</c> if deserialized; otherwise, <c>false</c>.</value>
        public bool Deserialized { get; set; }

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return (ConfigurationElement)Activator.CreateInstance(typeof(T));
        }

        /// <summary>
        /// Gets or sets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <value></value>
        public T this[int index]
        {
            get
            {
                return (T) BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Gets the <see cref="T"/> with the specified key.
        /// </summary>
        /// <value></value>
        public new T this[string key]
        {
            get { return (T) BaseGet(key); }
        }

        /// <summary>
        /// Called after deserialization.
        /// </summary>
        protected override void PostDeserialize()
        {
            Deserialized = true;
        } 
    }
}