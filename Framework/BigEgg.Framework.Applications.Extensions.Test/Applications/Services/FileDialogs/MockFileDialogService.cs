using BigEgg.Framework.Applications.Extensions.Applications.Services.FileDialogs;
using System.Collections.Generic;

namespace BigEgg.Framework.Applications.Extensions.Test.Applications.Services.FileDialogs
{
    public class MockFileDialogService : IFileDialogService
    {
        public FileDialogResult Result { get; set; }

        public object Owner { get; private set; }

        public FileDialogType FileDialogType { get; private set; }

        public IEnumerable<FileType> FileTypes { get; private set; }

        public FileType DefaultFileType { get; private set; }

        public string DefaultFileName { get; private set; }


        public FileDialogResult ShowOpenFileDialog(object owner, IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName)
        {
            FileDialogType = FileDialogType.OpenFileDialog;
            Owner = owner;
            FileTypes = fileTypes;
            DefaultFileType = defaultFileType;
            DefaultFileName = defaultFileName;
            return Result;
        }

        public FileDialogResult ShowSaveFileDialog(object owner, IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName)
        {
            FileDialogType = FileDialogType.SaveFileDialog;
            Owner = owner;
            FileTypes = fileTypes;
            DefaultFileType = defaultFileType;
            DefaultFileName = defaultFileName;
            return Result;
        }
    }


    public enum FileDialogType
    {
        None,
        OpenFileDialog,
        SaveFileDialog
    }
}
