﻿using PCLExt.FileStorage.Extensions;

namespace PCLExt.FileStorage.Folders
{
    /// <summary>
    /// A folder where the app is running.
    /// </summary>
    public class DocumentsRootFolder : BaseFolder
    {
        /// <summary>
        /// Returns the folder where the app is running.
        /// </summary>
#if !PORTABLE
        public DocumentsRootFolder() : base(GetDocumentsFolder()) { }
        private static IFolder GetDocumentsFolder()
        {
#if ANDROID || __IOS__
            return new DefaultFolderImplementation(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));
#elif DESKTOP || __MACOS__ || NETSTANDARD2_0
            return new DefaultFolderImplementation(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)).GetDataFolder();
#elif WINDOWS_UWP
            return null;
            // TODO
            return new DefaultFolderImplementation(Windows.Storage.KnownFolders.DocumentsLibrary.Path);
#endif
        }
#else
        public DocumentsRootFolder() : base(null) => throw Exceptions.ExceptionsHelper.NotImplementedInReferenceAssembly();
#endif
    }
}