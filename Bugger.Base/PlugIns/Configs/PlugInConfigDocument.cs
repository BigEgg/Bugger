using Bugger.Documents;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Bugger.PlugIns.Configs
{
    /// <summary>
    /// The document store all configuration information about the Plug-in
    /// </summary>
    [XmlRoot("Configuration")]
    public class PlugInConfigDocument : IDocument
    {
        /// <summary>
        /// Gets or sets the Plug-in's information.
        /// </summary>
        /// <value>
        /// The Plug-in's information.
        /// </value>
        [XmlElement("Info")]
        public PlugInInfo PlugInInfo { get; set; }

        /// <summary>
        /// Gets or sets the type of the Plug-in.
        /// </summary>
        /// <value>
        /// The type of the Plug-in.
        /// </value>
        [XmlElement("Type")]
        public PlugInType PlugInType { get; set; }

        /// <summary>
        /// Gets or sets the assemblies that related with the Plug-in.
        /// </summary>
        /// <value>
        /// The assemblies that related with the Plug-in.
        /// </value>
        [XmlArray("Assemblies")]
        [XmlArrayItem("Name")]
        public List<string> AssemblyNames { get; set; }

        /// <summary>
        /// Gets or sets the Plug-in's dependent Plug-ins.
        /// </summary>
        /// <value>
        /// The Plug-in's dependent Plug-ins.
        /// </value>
        [XmlArray("Dependencies")]
        [XmlArrayItem("Dependency")]
        public List<DependentPlugIn> DependentPlugIns { get; set; }
    }
}
