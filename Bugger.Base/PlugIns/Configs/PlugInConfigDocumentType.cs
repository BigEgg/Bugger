using Bugger.Base.Documents;
using System.ComponentModel.Composition;
using System.IO;
using System.Xml.Serialization;

namespace Bugger.Base.PlugIns.Configs
{
    /// <summary>
    /// The document type to pen and save the Plug-in's configuration document.
    /// </summary>
    [Export, Export(typeof(IDocumentType<PlugInConfigDocument>))]
    public class PlugInConfigDocumentType : IDocumentType<PlugInConfigDocument>
    {
        private const string FILE_EXTENSION = ".xml";


        /// <summary>
        /// Opens the Plug-in's configuration document with the specific file name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The Plug-in's configuration document.</returns>
        public PlugInConfigDocument Open(string fileName)
        {
            fileName = GetFileNameWithExtension(fileName);

            using (var stream = File.Open(fileName, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(PlugInConfigDocument));
                try
                {
                    return (PlugInConfigDocument)serializer.Deserialize(stream);
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Saves the Plug-in's configuration document with the specified file name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="document">The Plug-in's configuration document.</param>
        public void Save(string fileName, PlugInConfigDocument document)
        {
            fileName = GetFileNameWithExtension(fileName);

            using (var stream = File.Open(fileName, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(PlugInConfigDocument));
                serializer.Serialize(stream, document);
            }
        }


        private string GetFileNameWithExtension(string fileName)
        {
            if (!fileName.EndsWith(FILE_EXTENSION))
            {
                return fileName + FILE_EXTENSION;
            }
            else
            {
                return fileName;
            }
        }
    }
}
