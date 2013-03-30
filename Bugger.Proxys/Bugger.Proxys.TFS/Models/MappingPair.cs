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

        public MappingPair(string propertyName)
            : this(propertyName, string.Empty)
        { }

        public MappingPair(string propertyName, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(this.propertyName)) { throw new ArgumentException("propertyName"); }

            this.propertyName = propertyName;
            this.fieldName = fieldName;
        }

        #region Properties
        public string PropertyName
        {
            get { return this.propertyName; }
        }

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
