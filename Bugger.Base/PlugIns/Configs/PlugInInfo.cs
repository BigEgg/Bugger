using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Bugger.PlugIns.Configs
{
    /// <summary>
    /// The Plug-in's basic information.
    /// </summary>
    [XmlRoot("Info")]
    public class PlugInInfo
    {
        private Version version;
        private Version minimumSupportBuggerVersion;
        private Version maximumSupportBuggerVersion;

        /// <summary>
        /// Gets the Plug-in's unique id.
        /// </summary>
        /// <value>
        /// The Plug-in's unique id.
        /// </value>
        [XmlAttribute]
        public Guid PlugInGuid { get; set; }

        /// <summary>
        /// Gets the Plug-in name.
        /// </summary>
        /// <value>
        /// The Plug-in name.
        /// </value>
        [XmlElement]
        public string Name { get; set; }

        /// <summary>
        /// Gets the description of the Plug-in.
        /// </summary>
        /// <value>
        /// The description of the Plug-in.
        /// </value>
        [XmlElement]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the authors of the Plug-in.
        /// </summary>
        /// <value>
        /// The authors of the Plug-in.
        /// </value>
        [XmlArray]
        [XmlArrayItem(Type = typeof(PlugInAuthor), ElementName = "Author")]
        public List<PlugInAuthor> Authors { get; set; }

        /// <summary>
        /// Gets the Plug-in's version.
        /// </summary>
        /// <value>
        /// The Plug-in's version.
        /// </value>
        [XmlIgnore]
        public Version Version { get { return version; } }

        [XmlElement("Version")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string VersionStr
        {
            get
            {
                return version == null ? string.Empty : version.ToString();
            }
            set
            {
                version = new Version(value);
            }
        }

        /// <summary>
        /// Gets the Plug-in's minimum support bugger version.
        /// </summary>
        /// <value>
        /// The Plug-in's minimum support bugger version.
        /// </value>
        [XmlIgnore]
        public Version MinimumSupportBuggerVersion { get { return minimumSupportBuggerVersion; } }

        [XmlElement("MinimumSupportBuggerVersion")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string MinimumSupportBuggerVersionStr
        {
            get
            {
                return minimumSupportBuggerVersion == null ? string.Empty : minimumSupportBuggerVersion.ToString();
            }
            set
            {
                minimumSupportBuggerVersion = new Version(value);
            }
        }

        /// <summary>
        /// Gets the Plug-in's maximum support bugger version.
        /// </summary>
        /// <value>
        /// The Plug-in's maximum support bugger version.
        /// </value>
        [XmlIgnore]
        public Version MaximumSupportBuggerVersion
        {
            get { return maximumSupportBuggerVersion; }
        }

        [XmlElement("MaximumSupportBuggerVersion", IsNullable = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string MaximumSupportBuggerVersionStr
        {
            get
            {
                return maximumSupportBuggerVersion == null ? string.Empty : maximumSupportBuggerVersion.ToString();
            }
            set
            {
                if (value == null)
                {
                    maximumSupportBuggerVersion = null;
                }
                else
                {
                    maximumSupportBuggerVersion = new Version(value);
                }
            }
        }
    }
}
