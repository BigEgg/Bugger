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

        public TFSField(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("name"); }

            this.name = name;
            this.allowedValues = new ObservableCollection<string>();
        }

        #region Properties
        public string Name
        {
            get { return this.name; }
        }

        public ObservableCollection<string> AllowedValues
        {
            get { return this.allowedValues; }
        }
        #endregion
    }
}
