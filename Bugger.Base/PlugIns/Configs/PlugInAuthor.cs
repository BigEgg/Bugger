using System.Xml.Serialization;

namespace Bugger.PlugIns.Configs
{
    /// <summary>
    /// The Plug-in's author's basic information.
    /// </summary>
    [XmlRoot("Author")]
    public class PlugInAuthor
    {
        /// <summary>
        /// Gets or sets the author name of the Plug-in.
        /// </summary>
        /// <value>
        /// The author name of the Plug-in.
        /// </value>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email address of the author.
        /// </summary>
        /// <value>
        /// The email address of the author.
        /// </value>
        [XmlText]
        public string EmailAddress { get; set; }
    }
}