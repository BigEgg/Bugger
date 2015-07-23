﻿namespace BigEgg.Framework.Applications.Extensions.Applications.Services.FileDialogs
{
    /// <summary>
    /// Contains the result information about the work with the file dialog box.
    /// </summary>
    public class FileDialogResult
    {
        private readonly string fileName;
        private readonly FileType selectedFileType;


        /// <summary>
        /// Initializes a new instance of the <see cref="FileDialogResult"/> class with null values.
        /// Use this constructor when the user canceled the file dialog box.
        /// </summary>
        protected FileDialogResult()
            : this(null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDialogResult"/> class.
        /// </summary>
        /// <param name="fileName">The filename entered by the user.</param>
        /// <param name="selectedFileType">The file type selected by the user.</param>
        public FileDialogResult(string fileName, FileType selectedFileType)
        {
            this.fileName = fileName;
            this.selectedFileType = selectedFileType;
        }


        /// <summary>
        /// Gets the filename entered by the user or <c>null</c> when the user canceled the dialog box.
        /// </summary>
        public string FileName { get { return fileName; } }

        /// <summary>
        /// Gets the file type selected by the user or <c>null</c> when the user canceled the dialog box.
        /// </summary>
        public FileType SelectedFileType { get { return selectedFileType; } }

        /// <summary>
        /// Gets a value indicating whether this instance contains valid data. This property returns <c>false</c>
        /// when the user canceled the file dialog box.
        /// </summary>
        public bool IsValid { get { return FileName != null && SelectedFileType != null; } }


        /// <summary>
        /// Get the filename with extension entered by the user or <c>null</c> when the user canceled the dialog box.
        /// </summary>
        public string GetFullFileName()
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                return fileName.EndsWith(selectedFileType.FileExtension)
                    ? fileName
                    : fileName + selectedFileType.FileExtension;
            }
            else
                return null;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="FileDialogResult"/> class with null values.
        /// </summary>
        public static FileDialogResult CancelResult()
        {
            return new FileDialogResult();
        }
    }
}
