using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Properties;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Bugger.Proxy.TFS.Documents
{
    public class AutoFillMapDocumentType
    {
        #region Fields
        private const string FileExtension = ".autofill";
        private static string filePath;
        #endregion

        /// <summary>
        /// Initializes the <see cref="AutoFillMapDocumentType"/> class.
        /// </summary>
        static AutoFillMapDocumentType()
        {
            filePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                Resources.ProxyName + FileExtension);
        }

        #region Properties
        /// <summary>
        /// Gets the auto fill file's path.
        /// </summary>
        /// <value>
        /// The auto fill file's path.
        /// </value>
        public static string FilePath { get { return filePath; } }
        #endregion


        #region Methods
        #region Public Methods
        /// <summary>
        /// Create a new auto fill document.
        /// </summary>
        /// <returns>A new auto fill document</returns>
        public static AutoFillMapDocument New()
        {
            return new AutoFillMapDocument();
        }

        /// <summary>
        /// Create the auto fill document.
        /// </summary>
        /// <returns>The auto fill document</returns>
        public static AutoFillMapDocument Open()
        {
            AutoFillMapDocument document = new AutoFillMapDocument();
            XElement root;

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                root = XDocument.Load(fs).Root;
                fs.Flush();
                fs.Close();
            }

            foreach (var element in root.Elements())
            {
                var autoFillMap = new AutoFillMapModel();

                var isMandatory = true;
                if (element.HasAttributes)
                {
                    if (!bool.TryParse(element.Attribute("IsMandatory").Value, out isMandatory))
                    {
                        isMandatory = true;
                    }
                }
                autoFillMap.IsMandatory = isMandatory;

                foreach (var fieldName in element.Value.Split(';').ToList())
                {
                    autoFillMap.FieldsName.Add(fieldName);
                }

                document.AutoFillFields.Add(element.Name.LocalName, autoFillMap);
            }

            return document;
        }

        /// <summary>
        /// Saves the specified auto fill document.
        /// </summary>
        /// <param name="document">The auto fill document.</param>
        public static void Save(AutoFillMapDocument document)
        {
            var autoFillElement = new XElement("AutoFill");
            foreach (var keypair in document.AutoFillFields)
            {
                var child = new XElement(keypair.Key, string.Join(";", keypair.Value.FieldsName));
                if (!keypair.Value.IsMandatory)
                {
                    child.Add(new XAttribute("IsMandatory", "False"));
                }
            }

            XDocument autoFillDocument = new XDocument(
                new XComment("TFS Proxy Auto Fill Map Settings"),
                autoFillElement
            );

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                autoFillDocument.Save(fs);
                fs.Flush();
                fs.Close();
            }
        }
        #endregion
        #endregion
    }
}
