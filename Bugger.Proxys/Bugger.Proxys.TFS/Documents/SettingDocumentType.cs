using BigEgg.Framework.Applications.ViewModels;
using Bugger.Proxys.TFS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Bugger.Proxys.TFS.Documents
{
    internal class SettingDocumentType : DataModel
    {
        #region Fields
        private const string FileExtension = ".setting";
        private static string filePath;
        #endregion

        static SettingDocumentType()
        {
            filePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().FullName) + FileExtension);
        }

        #region Properties
        public static string FilePath { get { return filePath; } }
        #endregion

        #region Methods
        #region Public Methods
        public static SettingDocument New()
        {
            return new SettingDocument();
        }

        public static SettingDocument Open()
        {
            SettingDocument document = new SettingDocument();

            XElement root = XDocument.Load(filePath).Root;
            document.ConnectUri = new Uri(root.Element("Uri").Value);
            document.UserName = root.Element("UserName").Value;
            document.Password = root.Element("Password").Value;
            document.BugFilterField = root.Element("BugFilterField").Value;
            document.BugFilterValue = root.Element("BugFilterValue").Value;
            document.PriorityRed = root.Element("PriorityRed").Value;

            IEnumerable<XElement> elements = root.Element("PropertyMappings").Elements();
            foreach (XElement element in elements)
            {
                document.PropertyMappingList.First(x => x.PropertyName == element.Name).FieldName = element.Value;
            }

            return document;
        }

        public static void Save(SettingDocument document)
        {
            XElement propertyMappingElement = new XElement("PropertyMappings");
            foreach (MappingPair pair in document.PropertyMappingList)
            {
                propertyMappingElement.Add(new XElement(pair.PropertyName, pair.FieldName));
            }

            XDocument settingDocument = new XDocument(
                new XComment("TFS Proxy Settings"),
                new XElement("Settings",
                    new XElement("Uri", document.ConnectUri.AbsoluteUri),
                    new XElement("UserName", document.UserName),
                    new XElement("Password", document.Password),
                    new XElement("BugFilterField", document.BugFilterField),
                    new XElement("BugFilterValue", document.BugFilterValue),
                    new XElement("PriorityRed", document.PriorityRed),
                    propertyMappingElement
                )
            );

            settingDocument.Save(FilePath);
        }
        #endregion
        #endregion
    }
}
