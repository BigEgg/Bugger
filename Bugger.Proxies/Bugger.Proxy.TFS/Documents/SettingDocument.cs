using Bugger.Proxy.Models;
using System;

namespace Bugger.Proxy.TFS.Documents
{
    public class SettingDocument
    {
        #region Fields
        private readonly PropertyMappingDictionary propertyMappingCollection;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingDocument"/> class.
        /// </summary>
        public SettingDocument()
        {
            this.propertyMappingCollection = BugHelper.GetPropertyMappingDictionary();

            UserName = string.Empty;
            Password = string.Empty;
            BugFilterField = string.Empty;
            BugFilterValue = string.Empty;
            PriorityRed = string.Empty;
        }

        #region Properties
        /// <summary>
        /// Gets the property mapping collection.
        /// </summary>
        /// <value>
        /// The property mapping collection.
        /// </value>
        public PropertyMappingDictionary PropertyMappingCollection { get { return this.propertyMappingCollection; } }

        /// <summary>
        /// Gets or sets the connect URI.
        /// </summary>
        /// <value>
        /// The connect URI.
        /// </value>
        public Uri ConnectUri { get; set; }

        /// <summary>
        /// Gets or sets the TFS user name.
        /// </summary>
        /// <value>
        /// The TFS user name.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the bug filter field.
        /// </summary>
        /// <value>
        /// The bug filter field.
        /// </value>
        public string BugFilterField { get; set; }

        /// <summary>
        /// Gets or sets the bug filter value.
        /// </summary>
        /// <value>
        /// The bug filter value.
        /// </value>
        public string BugFilterValue { get; set; }

        /// <summary>
        /// Gets or sets the priority value to indicate which bug is the red gift.
        /// </summary>
        /// <value>
        /// The priority value.
        /// </value>
        public string PriorityRed { get; set; }
        #endregion
    }
}
