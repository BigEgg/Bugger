using BigEgg.Framework.Foundation;
using System;

namespace Bugger.Proxy.TFS.Models
{
    public class CheckString : Model
    {
        private bool isChecked;
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckString"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentException">name</exception>
        public CheckString(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("name"); }

            this.name = name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get { return this.name; } }

        /// <summary>
        /// Gets or sets a value indicating whether this item is checked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this item is checked; otherwise, <c>false</c>.
        /// </value>
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
