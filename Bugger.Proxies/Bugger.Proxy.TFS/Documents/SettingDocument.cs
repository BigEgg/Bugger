using BigEgg.Framework.Applications.ViewModels;
using Bugger.Domain.Models;
using System;
using System.ComponentModel;

namespace Bugger.Proxy.TFS.Documents
{
    public class SettingDocument : DataModel
    {
        #region Fields
        private readonly PropertyMappingDictionary propertyMappingCollection;
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

            UserName = string.Empty;
            Password = string.Empty;
            BugFilterField = string.Empty;
            BugFilterValue = string.Empty;
            PriorityRed = string.Empty;
        }

        #region Properties
        public PropertyMappingDictionary PropertyMappingCollection { get { return this.propertyMappingCollection; } }

        public Uri ConnectUri { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string BugFilterField { get; set; }

        public string BugFilterValue { get; set; }

        public string PriorityRed { get; set; }
        #endregion
    }
}
