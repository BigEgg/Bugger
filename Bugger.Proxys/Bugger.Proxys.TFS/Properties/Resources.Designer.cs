﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bugger.Proxys.TFS.Properties
{


    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Bugger.Proxys.TFS.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot connect to the TFS server, please check your uri or credential information is correct..
        /// </summary>
        internal static string CannotConnect {
            get {
                return ResourceManager.GetString("CannotConnect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The setting file cannot be opened..
        /// </summary>
        internal static string CannotOpenFile {
            get {
                return ResourceManager.GetString("CannotOpenFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot get the fields information from the TFS server. Please do this later..
        /// </summary>
        internal static string CannotQueryFields {
            get {
                return ResourceManager.GetString("CannotQueryFields", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid server name/URL.
        /// </summary>
        internal static string InvalidUrl {
            get {
                return ResourceManager.GetString("InvalidUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path is mandatory..
        /// </summary>
        internal static string PathMandatory {
            get {
                return ResourceManager.GetString("PathMandatory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The port is mandatory..
        /// </summary>
        internal static string PortMandatory {
            get {
                return ResourceManager.GetString("PortMandatory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The port must between {1} and {2}..
        /// </summary>
        internal static string PortRange {
            get {
                return ResourceManager.GetString("PortRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TFS.
        /// </summary>
        internal static string ProxyName {
            get {
                return ResourceManager.GetString("ProxyName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server name/URL is mandatory..
        /// </summary>
        internal static string ServerNameMandatory {
            get {
                return ResourceManager.GetString("ServerNameMandatory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Set Team Foundation Server.
        /// </summary>
        internal static string UriHelpDialogTitle {
            get {
                return ResourceManager.GetString("UriHelpDialogTitle", resourceCulture);
            }
        }
    }
}
