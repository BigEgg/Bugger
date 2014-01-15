using BigEgg.Framework.Foundation;
using System;

namespace Bugger.Applications.Models
{
    /// <summary>
    /// The model class for the ComboBox that can let people check the items.
    /// </summary>
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
        /// Gets the name of the item.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get { return this.name; } }

        /// <summary>
        /// Gets or sets a value indicating whether the item is checked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the item is checked; otherwise, <c>false</c>.
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
