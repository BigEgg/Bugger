using BigEgg.Framework.Applications.ViewModels;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace Bugger.Proxys.TFS.Documents
{
    internal class SettingDocumentType : DataModel
    {
        #region Fields
        private const string FileExtension = ".setting";
        private string filePath;
        #endregion

        public SettingDocumentType()
        {
            this.filePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().FullName) + FileExtension);
        }

        #region Properties
        public string FilePath { get { return this.filePath; } }
        #endregion

        #region Methods
        #region Public Methods
        public SettingDocument New()
        {
            return new SettingDocument();
        }

        public SettingDocument Open()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SettingDocument));

            using (StreamReader sr = new StreamReader(this.filePath))
            {
                return serializer.Deserialize(sr) as SettingDocument;
            }
        }

        public void Save(SettingDocument document)
        {
            XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
            xns.Add(string.Empty, string.Empty);
            XmlSerializer serializer = new XmlSerializer(typeof(SettingDocument));

            using (StreamWriter sw = new StreamWriter(this.filePath))
            {
                serializer.Serialize(sw, document, xns);
            }
        }
        #endregion
        #endregion
    }
}
