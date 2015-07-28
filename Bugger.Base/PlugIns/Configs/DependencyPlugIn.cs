using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Bugger.PlugIns.Configs
{
    [XmlRoot("Dependency")]
    public class DependencyPlugIn
    {
        private Version minimumSupportPlugInVersion;
        private Version maximumSupportBuggerVersion;

        /// <summary>
        /// Gets or sets the Plug-in's unique id.
        /// </summary>
        /// <value>
        /// The Plug-in's unique id.
        /// </value>
        [XmlAttribute("Guid")]
        public Guid PlugInGuid { get; set; }

        /// <summary>
        /// Gets or sets the dependency type.
        /// </summary>
        /// <value>
        /// The dependency type.
        /// </value>
        [XmlElement("Type")]
        public DependencyType DependencyType { get; set; }

        /// <summary>
        /// Gets or sets the group id when the dependency type is OneOfGroup.
        /// </summary>
        /// <value>
        /// The group id when the dependency type is OneOfGroup.
        /// </value>
        [XmlElement("Group", IsNullable = true)]
        public int? GroupId { get; set; }

        /// <summary>
        /// Gets the Plug-in's minimum support dependency Plug-in's version.
        /// </summary>
        /// <value>
        /// The Plug-in's minimum support dependency Plug-in's version.
        /// </value>
        [XmlIgnore]
        public Version MinimumSupportPlugInVersion { get { return minimumSupportPlugInVersion; } }

        [XmlElement("MinimumSupportPlugInVersion")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string MinimumSupportPlugInVersionStr
        {
            get
            {
                return minimumSupportPlugInVersion == null ? string.Empty : minimumSupportPlugInVersion.ToString();
            }
            set
            {
                minimumSupportPlugInVersion = new Version(value);
            }
        }

        /// <summary>
        /// Gets the Plug-in's maximum support Plug-in's version.
        /// </summary>
        /// <value>
        /// The Plug-in's maximum support Plug-in's version.
        /// </value>
        [XmlIgnore]
        public Version MaximumSupportPlugInVersion
        {
            get { return maximumSupportBuggerVersion; }
        }

        [XmlElement("MaximumSupportPlugInVersion", IsNullable = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string MaximumSupportPlugInVersionStr
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
