using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigEgg.Framework.Utils
{
    /// <summary>
    /// Extends the <see cref="Path"/> class with new methods.
    /// </summary>
    public static class PathExtension
    {
        /// <summary>
        /// Get the relative path fo the specific two path.
        /// </summary>
        /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param> 
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param> 
        /// <returns>The relative path from the start directory to the end path.</returns> 
        /// <exception cref="ArgumentException">This exception is thrown when the fromPath or toPath is not a valid path.</exception>
        /// <example>
        /// @"..\..\regedit.exe" = GetRelativePath(@"D:\Windows\Web\Wallpaper\", @"D:\Windows\regedit.exe" );
        /// </example>
        public static string GetRelativePath(this string fromPath, string toPath)
        {
            Preconditions.NotNullOrWhiteSpace(fromPath, "fromPath");
            Preconditions.NotNullOrWhiteSpace(toPath, "toPath");
            Preconditions.Check(() => Path.IsPathRooted(fromPath), "fromPath");
            Preconditions.Check(() => Path.IsPathRooted(toPath), "toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath.Replace('/', Path.DirectorySeparatorChar);
        }
    }
}
