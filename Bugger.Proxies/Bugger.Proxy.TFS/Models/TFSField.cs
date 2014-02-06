using BigEgg.Framework.Foundation;
using System;
using System.Collections.ObjectModel;

namespace Bugger.Proxy.TFS.Models
{
    public class TFSField : Model
    {
        #region Fields
        private readonly string name;
        private readonly ObservableCollection<string> allowedValues;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TFSField"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentException">name</exception>
        public TFSField(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("name"); }

            this.name = name;
            this.allowedValues = new ObservableCollection<string>();
        }

        #region Properties
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the allowed values.
        /// </summary>
        /// <value>
        /// The allowed values.
        /// </value>
        public ObservableCollection<string> AllowedValues
        {
            get { return this.allowedValues; }
        }
        #endregion
    }
}
