using BigEgg.Framework.Applications.ViewModels;
using Bugger.Proxys.Models;
using Bugger.Proxys.TFS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Bugger.Proxys.TFS.Documents
{
    internal class SettingDocument : DataModel
    {
        #region Fields
        private readonly List<MappingPair> propertyMappingList;
        private Uri connectUri;
        private string userName;
        private string password;
        private string bugFilterField;
        private string bugFilterValue;
        private string priorityRed;
        #endregion

        public SettingDocument()
        {
            this.propertyMappingList = new List<MappingPair>();

            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(Bug));
            foreach (PropertyDescriptor propertyDescriptor in propertyDescriptorCollection)
            {
                MappingPair mappingPair = new MappingPair(propertyDescriptor.Name);
                AddWeakEventListener(mappingPair, MappingPairPropertyChanged);
                this.propertyMappingList.Add(mappingPair);
            }
        }

        #region Properties
        [XmlElement(Order = 7)]
        public List<MappingPair> PropertyMappingList { get { return this.propertyMappingList; } }

        [XmlElement(Order = 1)]
        public Uri ConnectUri
        {
            get { return this.connectUri; }
            set 
            { 
                this.connectUri = value;
                RaisePropertyChanged("ConnectUri");
            }
        }

        [XmlElement(Order = 2)]
        public string UserName
        {
            get { return this.userName; }
            set
            {
                if (this.userName != value)
                {
                    this.userName = value;
                    RaisePropertyChanged("UserName");
                }
            }
        }

        [XmlElement(Order = 3)]
        public string Password
        {
            get { return this.password; }
            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }

        [XmlElement(Order = 4)]
        public string BugFilterField
        {
            get { return this.bugFilterField; }
            set
            {
                if (this.bugFilterField != value)
                {
                    this.bugFilterField = value;
                    RaisePropertyChanged("BugFilterField");
                }
            }
        }

        [XmlElement(Order = 5)]
        public string BugFilterValue
        {
            get { return this.bugFilterValue; }
            set
            {
                if (this.bugFilterValue != value)
                {
                    this.bugFilterValue = value;
                    RaisePropertyChanged("BugFilterValue");
                }
            }
        }

        [XmlElement(Order = 6)]
        public string PriorityRed
        {
            get { return this.priorityRed; }
            set
            {
                if (this.priorityRed != value)
                {
                    this.priorityRed = value;
                    RaisePropertyChanged("PriorityRed");
                }
            }
        }
        #endregion

        #region Methdos
        #region Private Methods
        private void MappingPairPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("PropertyMappingList");
        }
        #endregion
        #endregion
    }
}
