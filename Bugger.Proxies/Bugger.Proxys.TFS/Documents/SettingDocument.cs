using BigEgg.Framework.Applications.ViewModels;
using Bugger.Domain.Models;
using Bugger.Proxy.TFS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Bugger.Proxy.TFS.Documents
{
    public class SettingDocument : DataModel
    {
        #region Fields
        private readonly ReadOnlyCollection<MappingPair> propertyMappingList;
        private Uri connectUri;
        private string userName;
        private string password;
        private string bugFilterField;
        private string bugFilterValue;
        private string priorityRed;
        #endregion

        public SettingDocument()
        {
            List<MappingPair> mappingList = new List<MappingPair>();

            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(Bug));
            foreach (PropertyDescriptor propertyDescriptor in propertyDescriptorCollection)
            {
                if (propertyDescriptor.Name == "Type")
                    continue;

                MappingPair mappingPair = new MappingPair(propertyDescriptor.Name);
                AddWeakEventListener(mappingPair, MappingPairPropertyChanged);
                mappingList.Add(mappingPair);
            }

            this.HasChanged = false;
            this.propertyMappingList = new ReadOnlyCollection<MappingPair>(mappingList);
        }

        #region Properties
        internal bool HasChanged { get; set; }

        public ReadOnlyCollection<MappingPair> PropertyMappingList { get { return this.propertyMappingList; } }

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

        #region Methdos
        #region Private Methods
        private void MappingPairPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.HasChanged = true;
            RaisePropertyChanged("PropertyMappingList");
        }
        #endregion
        #endregion
    }
}
