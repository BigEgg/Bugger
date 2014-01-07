using BigEgg.Framework.Foundation;
using System;

namespace Bugger.Proxy.TFS.Models
{
    public class CheckString : Model
    {
        private bool isChecked;
        private string name;

        public CheckString(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("name"); }

            this.name = name;
        }

        public string Name { get { return this.name; } }

        public bool IsChecked
        {
            get { return this.isChecked; }
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    RaisePropertyChanged("IsChecked");
                }
            }
        }
    }
}
