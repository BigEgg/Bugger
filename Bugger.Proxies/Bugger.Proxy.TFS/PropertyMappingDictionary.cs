using BigEgg.Framework.Foundation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bugger.Proxy.TFS
{
    [Serializable]
    public class MappingModel : Model
    {
        #region Fields
        private string key;
        private string value;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingModel"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <exception cref="System.ArgumentNullException">key</exception>
        public MappingModel(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) { throw new ArgumentNullException("key"); }

            this.key = key;
            this.value = string.Empty;
        }

        #region Properties
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key
        {
            get { return this.key; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value
        {
            get { return this.value; }
            set
            {
                if (value != null && this.value != value)
                {
                    this.value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }
        #endregion
    }

    [Serializable]
    public class PropertyMappingDictionary : ObservableCollection<MappingModel>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>
        ///    The value associated with the specified key. If the specified key is not
        ///    found, a get operation throws a System.Collections.Generic.KeyNotFoundException,
        ///    and a set operation creates a new element with the specified key.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   The key is null or white space.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///   The property is retrieved and key does not exist in the collection.
        /// </exception>
        public string this[string key]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(key)) { throw new ArgumentNullException("The key is null or white space."); }
                if (!ContainsKey(key)) { throw new KeyNotFoundException("The property is retrieved and key does not exist in the collection."); }

                string result = string.Empty;
                TryGetValue(key, out result);
                return result;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(key)) { throw new ArgumentNullException("The key is null or white space."); }
                if (!ContainsKey(key)) { throw new KeyNotFoundException("The property is retrieved and key does not exist in the collection."); }

                var result = base.Items.FirstOrDefault((i) => Equals(key, i.Key));
                result.Value = value;
            }
        }

        /// <summary>
        /// Gets a collection containing the keys in the Bugger.Proxy.TFS.PropertyMappingDictionary.
        /// </summary>
        /// <value>
        ///   A System.Collections.Generic.List<string> containing the keys in the Bugger.Proxy.TFS.PropertyMappingDictionary.
        /// </value>
        public ICollection<string> Keys
        {
            get { return base.Items.Select(x => x.Key).ToList(); }
        }

        /// <summary>
        /// Gets a collection containing the values in the Bugger.Proxy.TFS.PropertyMappingDictionary.
        /// </summary>
        /// <value>
        ///   A System.Collections.Generic.List<string> containing the keys in the Bugger.Proxy.TFS.PropertyMappingDictionary.
        /// </value>
        public ICollection<string> Values
        {
            get { return base.Items.Select(x => x.Value).ToList(); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds the specified key and value to the Bugger.Proxy.TFS.PropertyMappingDictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        /// <exception cref="System.ArgumentNullException">The key is null or white space.</exception>
        /// <exception cref="System.ArgumentException">An element with the same key already exists in the Bugger.Proxy.TFS.PropertyMappingDictionary.</exception>
        public void Add(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key)) { throw new ArgumentNullException("The key is null or white space."); }
            if (ContainsKey(key)) { throw new ArgumentException("An element with the same key already exists in the Bugger.Proxy.TFS.PropertyMappingDictionary."); }

            base.Add(new MappingModel(key) { Value = value });
        }

        /// <summary>
        /// Determines whether the Bugger.Proxy.TFS.PropertyMappingDictionary contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the Bugger.Proxy.TFS.PropertyMappingDictionary.</param>
        /// <returns>
        ///   <c>True</c> if the Bugger.Proxy.TFS.PropertyMappingDictionary contains an element with the specified key;
        ///   otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The key is null or white space.</exception>
        public bool ContainsKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) { throw new ArgumentNullException("The key is null or white space."); }

            var result = base.Items.FirstOrDefault(pair => key == pair.Key);
            return result != null;
        }

        /// <summary>
        /// Removes the value with the specified key from the Bugger.Proxy.TFS.PropertyMappingDictionary
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        ///   <c>True</c> if the element is successfully found and removed; otherwise, <c>false</c>.
        ///   This method returns false if key is not found in the Bugger.Proxy.TFS.PropertyMappingDictionary.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The key is null or white space.</exception>
        public bool Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) { throw new ArgumentNullException("The key is null or white space."); }

            var pairs = base.Items.Where(pair => key == pair.Key).ToList();
            foreach (var pair in pairs)
            {
                base.Items.Remove(pair);
            }
            return pairs.Count > 0;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">
        ///   When this method returns, contains the value associated with the specified
        ///   key, if the key is found; otherwise, the default value for the type of the
        ///   value parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        ///   <c>True</c> if the Bugger.Proxy.TFS.PropertyMappingDictionary contains an element with the specified key;
        ///   otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The key is null or white space.</exception>
        public bool TryGetValue(string key, out string value)
        {
            if (string.IsNullOrWhiteSpace(key)) { throw new ArgumentNullException("The key is null or white space."); }

            value = string.Empty;

            if (!ContainsKey(key))
            {
                return false;
            }

            var result = base.Items.FirstOrDefault(pair => key == pair.Key);
            value = result.Value;
            return true;
        }
        #endregion
    }
}