using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notebook_L.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace UnitTest.FileSystem
{
    [TestClass]
    public class FileSystemTest
    {
        private const String name1 = "IBhuQPwts4LZbr1T";
        private const String name2 = "GQoID66Sxhp6JEn5";
        private const String name3 = "618k2LqLYh6iX7th";
        private const String name4 = "a0J1TDfRpGNZ5257";
        private const String name5 = "loGsC12EybO8QnQS";

        private async Task Initialization(IFileSystem fileSystem)
        {
            IFolder root = fileSystem.GetRootFolder();

            IEnumerable<IFile> files = await root.GetFilesAsync();
            IEnumerable<IFolder> folders = await root.GetFoldersAsync();

            foreach (IFile file in files)
            {
                await file.DeleteAsync();
            }

            foreach (IFolder folder in folders)
            {
                await folder.DeleteAsync();
            }
        }

        private async Task FileSystemGeneral(IFileSystem fileSystem)
        {
            IFolder root = fileSystem.GetRootFolder();

            Assert.AreEqual(fileSystem, root.UnderlyingFileSystem);
            Assert.AreEqual(null, await root.GetParentAsync());
        }

        private async Task FolderGeneral(IFileSystem fileSystem)
        {
            IFolder root = fileSystem.GetRootFolder();

            #region Test for CreateXXXAsync
            IFile file1 = await root.CreateFileAsync(name1);
            IFile file2 = await root.CreateFileAsync(name2);

            IFolder folder1 = await root.CreateFolderAsync(name3);
            IFolder folder2 = await root.CreateFolderAsync(name4);

            await Assert.ThrowsExceptionAsync<NameCollisionException>(async () => {
                await root.CreateFileAsync(name3);
            });
            await Assert.ThrowsExceptionAsync<NameCollisionException>(async () => {
                await root.CreateFolderAsync(name3);
            });

            await Assert.ThrowsExceptionAsync<NameCollisionException>(async () => {
                await root.CreateFileAsync(name3);
            });
            await Assert.ThrowsExceptionAsync<NameCollisionException>(async () => {
                await root.CreateFolderAsync(name3);
            });

            IFile file3 = await folder1.CreateFileAsync(name3);
            IFolder folder3 = await folder1.CreateFolderAsync(name4);
            #endregion

            #region Test for Property
            Assert.AreEqual(name1, file1.Name);
            Assert.AreEqual(name2, file2.Name);
            Assert.AreEqual(name3, folder1.Name);
            Assert.AreEqual(name4, folder2.Name);
            Assert.AreEqual(name3, file3.Name);
            Assert.AreEqual(name4, folder3.Name);

            Assert.AreEqual(folder1.Path, (await folder3.GetParentAsync()).Path);
            Assert.AreEqual(folder1.Path, (await file3.GetParentAsync()).Path);
            Assert.AreEqual(root.Path, (await folder1.GetParentAsync()).Path);
            Assert.AreEqual(root.Path, (await file1.GetParentAsync()).Path);

            Assert.AreEqual(fileSystem, folder3.UnderlyingFileSystem);
            Assert.AreEqual(fileSystem, file3.UnderlyingFileSystem);
            #endregion

            #region Test for GetParentAsync
            Assert.AreEqual(root.Path, (await (await folder3.GetParentAsync()).GetParentAsync()).Path);
            Assert.AreEqual(root.Name, (await (await folder3.GetParentAsync()).GetParentAsync()).Name);
            #endregion

            IEnumerable<IFile> files;
            IEnumerable<IFolder> folders;

            #region Test for GetXXXsAsync
            files = await root.GetFilesAsync();
            folders = await root.GetFoldersAsync();

            Assert.AreEqual(2, files.Count());
            Assert.AreEqual(2, folders.Count());

            Assert.IsTrue(files.Where((e) => e.Name == name1).Any());
            Assert.IsTrue(files.Where((e) => e.Name == name2).Any());
            Assert.IsTrue(folders.Where((e) => e.Name == name3).Any());
            Assert.IsTrue(folders.Where((e) => e.Name == name4).Any());
            #endregion

            #region Test for RenameAsync
            await folder1.RenameAsync(name5);
            folders = await root.GetFoldersAsync();

            Assert.AreEqual(name5, folder1.Name);
            Assert.AreEqual(2, folders.Count());

            Assert.IsTrue(folders.Where((e) => e.Name == name4).Any());
            Assert.IsTrue(folders.Where((e) => e.Name == name5).Any());
            #endregion

            #region Test for DeleteAsync
            await folder1.DeleteAsync();
            folders = await root.GetFoldersAsync();

            Assert.AreEqual(1, folders.Count());
            Assert.IsTrue(folders.Where((e) => e.Name == name4).Any());
            #endregion
        }

        private async Task FileGeneral(IFileSystem fileSystem)
        {
            IFolder root = fileSystem.GetRootFolder();
            IFolder folder = await root.CreateFolderAsync(name1);
            IFile file = await folder.CreateFileAsync(name1);

            #region Test for Property
            Assert.AreEqual(name1, file.Name);
            Assert.AreEqual(fileSystem, file.UnderlyingFileSystem);
            #endregion

            #region Test for GetParentAsync
            Assert.AreEqual(root.Path, (await (await file.GetParentAsync()).GetParentAsync()).Path);
            Assert.AreEqual(root.Name, (await (await file.GetParentAsync()).GetParentAsync()).Name);
            #endregion

            #region Test for XXXFileAsync
            IFile file1 = (await folder.GetFilesAsync()).First();

            const String srcText = name1 + name2 + name3 + name4;

            // create a text file for testing
            StorageFolder temp1 = await FileSystemFactory.CreateTemporaryFolderAsync();
            StorageFile srcFile = await temp1.CreateFileAsync("test_texture.txt");
            await FileIO.WriteTextAsync(srcFile, srcText);

            // check Property of file should be unmodified 
            await file1.SetFileAsync(srcFile);
            Assert.AreEqual(file.Name, file1.Name);
            Assert.AreEqual(file.Path, file1.Path);
            Assert.AreEqual(file.UnderlyingFileSystem, file1.UnderlyingFileSystem);

            // retrive the text file
            StorageFolder temp2 = await FileSystemFactory.CreateTemporaryFolderAsync();
            StorageFile resFile = await file.GetFileAsync(temp2);

            // assert Property of the StorageFile
            Assert.AreEqual(file.Name, resFile.Name);
            Assert.AreEqual(temp2.Path, (await resFile.GetParentAsync()).Path);

            // assert content
            String resText = await FileIO.ReadTextAsync(resFile);
            Assert.AreEqual(srcText, resText);
            #endregion

            IEnumerable<IFile> files;

            #region Test for RenameAsync
            await file.RenameAsync(name5);
            files = await folder.GetFilesAsync();

            Assert.AreEqual(name5, file.Name);
            Assert.AreEqual(1, files.Count());

            Assert.IsTrue(files.Where((e) => e.Name == name5).Any());
            #endregion

            #region Test for DeleteAsync
            await file.DeleteAsync();
            files = await folder.GetFilesAsync();

            Assert.AreEqual(0, files.Count());
            #endregion
        }

        [TestMethod]
        public async Task LocalFileSystem()
        {
            IFileSystem fileSystem = await FileSystemFactory.CreateFileSystemAsync(new FileSystemData
            {
                Name = "Test-LocalFileSystem",
                Source = FileSystemData.SourceType.Local,
                Credential = "No Credential Needed"
            });

            await Initialization(fileSystem);
            await FileSystemGeneral(fileSystem);

            await Initialization(fileSystem);
            await FolderGeneral(fileSystem);

            await Initialization(fileSystem);
            await FileGeneral(fileSystem);
        }
    }
}
