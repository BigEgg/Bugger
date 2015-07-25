using Bugger.Base.Documents;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Bugger.Base.PlugIns.Configs
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
        /// Gets or sets the Plug-in's dependency Plug-ins.
        /// </summary>
        /// <value>
        /// The Plug-in's dependency Plug-ins.
        /// </value>
        [XmlArray("Dependencies")]
        [XmlArrayItem("PlugIn")]
        public List<Guid> DependencyPlugIns { get; set; }
    }
}
