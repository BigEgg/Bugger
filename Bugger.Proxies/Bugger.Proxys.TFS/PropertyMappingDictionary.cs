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

        public MappingModel(string key)
        {
            this.key = key;
        }

        #region Properties
        public string Key
        {
            get { return this.key; }
        }

        public string Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                RaisePropertyChanged("Value");
            }
        }
        #endregion
    }

    [Serializable]
    public class PropertyMappingDictionary : ObservableCollection<MappingModel>
    {
        #region Properties
        public string this[string key]
        {
            get
            {
                if (!ContainsKey(key))
                {
                    throw new ArgumentException("Key not found");
                }

                string result;
                TryGetValue(key, out result);
                return result;
            }
            set
            {
                if (ContainsKey(key))
                {
                    var result = base.Items.FirstOrDefault((i) => Equals(key, i.Key));
                    result.Value = value;
                }
                else
                {
                    throw new ArgumentException("Key not found");
                }
            }
        }

        public ICollection<string> Keys
        {
            get { return base.Items.Select(x => x.Key).ToList(); }
        }

        public ICollection<string> Values
        {
            get { return base.Items.Select(x => x.Value).ToList(); }
        }
        #endregion

        #region Methods
        public void Add(string key, string value)
        {
            if (ContainsKey(key))
            {
                throw new ArgumentException("The dictionary already contains the key");
            }
            base.Add(new MappingModel(key) { Value = value });
        }

        public bool ContainsKey(string key)
        {
            var result = base.Items.FirstOrDefault(pair => key == pair.Key);
            return result != null;
        }

        public bool Remove(string key)
        {
            var pairs = base.Items.Where(pair => key == pair.Key).ToList();
            foreach (var pair in pairs)
            {
                base.Items.Remove(pair);
            }
            return pairs.Count > 0;
        }

        public bool TryGetValue(string key, out string value)
        {
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