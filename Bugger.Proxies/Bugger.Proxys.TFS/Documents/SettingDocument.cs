using BigEgg.Framework.Applications.ViewModels;
using Bugger.Domain.Models;
using Bugger.Proxy.TFS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Bugger.Proxy.TFS.Documents
{
    public class SettingDocument : DataModel
    {
        #region Fields
        private readonly PropertyMappingDictionary propertyMappingCollection;
        private Uri connectUri;
        private string userName;
        private string password;
        private string bugFilterField;
        private string bugFilterValue;
        private string priorityRed;
        #endregion

        public SettingDocument()
        {
            this.propertyMappingCollection = new PropertyMappingDictionary();

            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(Bug));
            foreach (PropertyDescriptor propertyDescriptor in propertyDescriptorCollection)
            {
                if (propertyDescriptor.Name == "Type") continue;

                this.propertyMappingCollection.Add(propertyDescriptor.Name, string.Empty);
            }

            AddWeakEventListener(this.propertyMappingCollection, PropertyMappingCollectionChanged);

            this.HasChanged = false;
        }

        #region Properties
        internal bool HasChanged { get; set; }

        public PropertyMappingDictionary PropertyMappingCollection { get { return this.propertyMappingCollection; } }

        public Uri ConnectUri
        {
            get { return this.connectUri; }
            set 
            { 
                this.connectUri = value;
                this.HasChanged = true;
                RaisePropertyChanged("ConnectUri");
            }
        }

        public string UserName
        {
            get { return this.userName; }
            set
            {
                if (this.userName != value)
                {
                    this.userName = value;
                    this.HasChanged = true;
                    RaisePropertyChanged("UserName");
                }
            }
        }

        public string Password
        {
            get { return this.password; }
            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    this.HasChanged = true;
                    RaisePropertyChanged("Password");
                }
            }
        }

        public string BugFilterField
        {
            get { return this.bugFilterField; }
            set
            {
                if (this.bugFilterField != value)
                {
                    this.bugFilterField = value;
                    this.HasChanged = true;
                    RaisePropertyChanged("BugFilterField");
                }
            }
        }

        public string BugFilterValue
        {
            get { return this.bugFilterValue; }
            set
            {
                if (this.bugFilterValue != value)
                {
                    this.bugFilterValue = value;
                    this.HasChanged = true;
                    RaisePropertyChanged("BugFilterValue");
                }
            }
        }

        public string PriorityRed
        {
            get { return this.priorityRed; }
            set
            {
                if (this.priorityRed != value)
                {
                    this.priorityRed = value;
                    this.HasChanged = true;
                    RaisePropertyChanged("PriorityRed");
                }
            }
        }
        #endregion

        #region Methods
        #region Private Methods

        private void PropertyMappingCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.HasChanged = true;
            RaisePropertyChanged("PropertyMappingCollection");
        }

        #endregion
        #endregion
    }
}
