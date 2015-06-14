using BigEgg.Framework.Applications.Foundation;
using System.ComponentModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BigEgg.Framework.Applications.Extensions.Applications
{
    /// <summary>
    /// Represents a recent file.
    /// </summary>
    public sealed class RecentFile : Model, IXmlSerializable
    {
        private bool isPinned;
        private string path;


        /// <summary>
        /// This constructor overload is reserved and should not be used. It is used internally by the XmlSerializer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RecentFile() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentFile"/> class.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <exception cref="ArgumentException">The argument path must not be null or empty.</exception>
        public RecentFile(string path)
        {
            Preconditions.NotNullOrWhiteSpace(path, "The argument path must not be null or empty.");
            this.path = path;
        }


        /// <summary>
        /// Gets or sets a value indicating whether this recent file is pinned.
        /// </summary>
        public bool IsPinned
        {
            get { return isPinned; }
            set { SetProperty(ref isPinned, value); }
        }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        public string Path { get { return path; } }


        XmlSchema IXmlSerializable.GetSchema() { return null; }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Preconditions.NotNull(reader, "reader");

            isPinned = bool.Parse(reader.GetAttribute("IsPinned"));
            path = reader.ReadElementContentAsString();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Preconditions.NotNull(writer, "writer");

            writer.WriteAttributeString("IsPinned", isPinned.ToString());
            writer.WriteString(path);
        }
    }
}
