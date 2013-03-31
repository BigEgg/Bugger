using BigEgg.Framework.Foundation;
using System;

namespace Bugger.Proxys.TFS.Models
{
    internal class MappingPair : Model
    {
        #region Fields
        private readonly string propertyName;
        private string fieldName;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingPair"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the Bug class' property.</param>
        /// <exception cref="System.ArgumentException">propertyName</exception>
        public MappingPair(string propertyName)
            : this(propertyName, string.Empty)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingPair"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the Bug class' property.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <exception cref="System.ArgumentException">The name of the TFS field.</exception>
        public MappingPair(string propertyName, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) { throw new ArgumentException("propertyName"); }

            this.propertyName = propertyName;
            this.fieldName = fieldName;
        }

        #region Properties
        /// <summary>
        /// Gets the name of the Bug class' property.
        /// </summary>
        /// <value>
        /// The name of the Bug class' property.
        /// </value>
        public string PropertyName
        {
            get { return this.propertyName; }
        }

        /// <summary>
        /// Gets or sets the name of the TFS field.
        /// </summary>
        /// <value>
        /// The name of the TFS field.
        /// </value>
        public string FieldName
        {
            get { return this.fieldName; }
            set
            {
                if (this.fieldName != value)
                {
                    this.fieldName = value;
                    RaisePropertyChanged("FieldName");
                }
            }
        }
        #endregion
    }
}
