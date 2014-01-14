using BigEgg.Framework.Applications.ViewModels;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Models.Attributes;
using System;
using System.Linq;
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

            IgnoreMappingAttribute ignore = new IgnoreMappingAttribute() { Ignore = true };
            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(TFSBug));
            foreach (PropertyDescriptor propertyDescriptor in propertyDescriptorCollection.Cast<PropertyDescriptor>()
                                                                                          .Where(x => !x.Attributes.Contains(ignore)))
            {
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
