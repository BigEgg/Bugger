using System.Collections.Generic;

namespace BigEgg.Framework.Applications.Extensions.Applications.Services.FileDialog
{
    /// <summary>
    /// Provides method overloads for the <see cref="IFileDialogService"/> to simplify its usage.
    /// </summary>
    public static class SaveFileDialogServiceExtensions
    {

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="fileType">The supported file type.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="System.ArgumentNullException">service must not be null.</exception>
        /// <exception cref="System.ArgumentNullException">fileType must not be null.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, FileType fileType)
        {
            Preconditions.NotNull(service, "service");
            Preconditions.NotNull(fileType, "fileType");

            return service.ShowSaveFileDialog(null, new FileType[] { fileType }, fileType, null);
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="owner">The window that owns this SaveFileDialog.</param>
        /// <param name="fileType">The supported file type.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="System.ArgumentNullException">service must not be null.</exception>
        /// <exception cref="System.ArgumentNullException">owner must not be null.</exception>
        /// <exception cref="System.ArgumentNullException">fileType must not be null.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, object owner, FileType fileType)
        {
            Preconditions.NotNull(service, "service");
            Preconditions.NotNull(owner, "owner");
            Preconditions.NotNull(fileType, "fileType");

            return service.ShowSaveFileDialog(owner, new FileType[] { fileType }, fileType, null);
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="fileType">The supported file type.</param>
        /// <param name="defaultFileName">Default filename. The directory name is used as initial directory when it is specified.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="System.ArgumentNullException">service must not be null.</exception>
        /// <exception cref="System.ArgumentNullException">fileType must not be null.</exception>
        /// <exception cref="System.ArgumentNullException">defaultFileName must not be null.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, FileType fileType, string defaultFileName)
        {
            Preconditions.NotNull(service, "service");
            Preconditions.NotNull(fileType, "fileType");
            Preconditions.NotNull(defaultFileName, "defaultFileName");

            return service.ShowSaveFileDialog(null, new FileType[] { fileType }, fileType, defaultFileName);
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="owner">The window that owns this SaveFileDialog.</param>
        /// <param name="fileType">The supported file type.</param>
        /// <param name="defaultFileName">Default filename. The directory name is used as initial directory when it is specified.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="System.ArgumentNullException">service must not be null.</exception>
        /// <exception cref="System.ArgumentNullException">owner must not be null.</exception>
        /// <exception cref="System.ArgumentNullException">fileType must not be null.</exception>
        /// <exception cref="System.ArgumentNullException">defaultFileName must not be null.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, object owner, FileType fileType, string defaultFileName)
        {
            Preconditions.NotNull(service, "service");
            Preconditions.NotNull(owner, "owner");
            Preconditions.NotNull(fileType, "fileType");
            Preconditions.NotNull(defaultFileName, "defaultFileName");

            return service.ShowSaveFileDialog(owner, new FileType[] { fileType }, fileType, defaultFileName);
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="fileTypes">The supported file types.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="System.ArgumentNullException">service must not be null.</exception>
        /// <exception cref="System.ArgumentNullException">fileTypes must not be null.</exception>
        /// <exception cref="ArgumentException">fileTypes must contain at least one item.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, IEnumerable<FileType> fileTypes)
        {
            Preconditions.NotNull(service, "service");

            return service.ShowSaveFileDialog(null, fileTypes, null, null);
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="owner">The window that owns this SaveFileDialog.</param>
        /// <param name="fileTypes">The supported file types.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="System.ArgumentNullException">service must not be null.</exception>
        /// <exception cref="System.ArgumentNullException">owner must not be null.</exception>
        /// <exception cref="System.ArgumentNullException">fileTypes must not be null.</exception>
        /// <exception cref="System.ArgumentException">fileTypes must contain at least one item.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, object owner, IEnumerable<FileType> fileTypes)
        {
            Preconditions.NotNull(service, "service");
            Preconditions.NotNull(owner, "owner");

            return service.ShowSaveFileDialog(owner, fileTypes, null, null);
        }

        /// <summary>
        /// Shows the save file dialog box that allows a user to specify a filename to save a file as.
        /// </summary>
        /// <param name="service">The file dialog service.</param>
        /// <param name="fileTypes">The supported file types.</param>
        /// <param name="defaultFileType">Default file type.</param>
        /// <param name="defaultFileName">Default filename. The directory name is used as initial directory when it is specified.</param>
        /// <returns>A FileDialogResult object which contains the filename entered by the user.</returns>
        /// <exception cref="System.ArgumentNullException">service must not be null.</exception>
        /// <exception cref="System.ArgumentNullException">defaultFileType must not be null.</exception>
        /// <exception cref="System.ArgumentNullException">defaultFileName must not be null.</exception>
        /// <exception cref="System.ArgumentNullException">fileTypes must not be null.</exception>
        /// <exception cref="System.ArgumentException">fileTypes must contain at least one item.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName)
        {
            Preconditions.NotNull(service, "service");
            Preconditions.NotNull(defaultFileType, "defaultFileType");
            Preconditions.NotNull(defaultFileName, "defaultFileName");

            return service.ShowSaveFileDialog(null, fileTypes, defaultFileType, defaultFileName);
        }
    }
}
