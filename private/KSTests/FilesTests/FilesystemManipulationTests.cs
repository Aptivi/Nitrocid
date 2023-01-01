
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.IO;
using System.Text.RegularExpressions;
using KS.Files;
using KS.Files.Attributes;
using KS.Files.Folders;
using KS.Files.Operations;
using KS.Files.PathLookup;
using KS.Files.Querying;
using KS.Files.Read;
using KS.Kernel;
using KS.Shell;
using NUnit.Framework;
using Shouldly;

namespace KSTests.FilesTests
{

    [TestFixture]
    public class FilesystemManipulationTests
    {

        /// <summary>
        /// Tests copying directory to directory
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestCopyDirectoryToDirectory()
        {
            CurrentDirectory.CurrentDir = Paths.HomePath;
            Directory.CreateDirectory(Paths.HomePath + "/TestDir");
            string SourcePath = "/TestDir";
            string TargetPath = "/TestDir2";
            Should.NotThrow(() => Copying.CopyFileOrDir(SourcePath, TargetPath));
        }

        /// <summary>
        /// Tests copying file to directory
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestCopyFileToDirectory()
        {
            CurrentDirectory.CurrentDir = Paths.HomePath;
            string SourcePath = Path.GetFullPath("TestData/TestText.txt");
            string TargetPath = "/Documents";
            Should.NotThrow(() => Copying.TryCopyFileOrDir(SourcePath, TargetPath));
        }

        /// <summary>
        /// Tests copying file to file
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestCopyFileToFile()
        {
            CurrentDirectory.CurrentDir = Paths.HomePath;
            string SourcePath = Path.GetFullPath("TestData/TestText.txt");
            string TargetPath = "/Documents/Text.txt";
            Should.NotThrow(() => Copying.TryCopyFileOrDir(SourcePath, TargetPath));
        }

        /// <summary>
        /// Tests making directory
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestMakeDirectory()
        {
            CurrentDirectory.CurrentDir = Paths.HomePath;
            Should.NotThrow(() => Making.TryMakeDirectory("/NewDirectory"));
        }

        /// <summary>
        /// Tests making file
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestMakeFile()
        {
            CurrentDirectory.CurrentDir = Paths.HomePath;
            Should.NotThrow(() => Making.TryMakeFile("/NewFile.txt"));
        }

        /// <summary>
        /// Tests making file
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestMakeJsonFile()
        {
            CurrentDirectory.CurrentDir = Paths.HomePath;
            Should.NotThrow(() => Making.TryMakeJsonFile("/NewFile.json"));
        }

        /// <summary>
        /// Tests moving directory to directory
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestMoveDirectoryToDirectory()
        {
            CurrentDirectory.CurrentDir = Paths.HomePath;
            Directory.CreateDirectory(Paths.HomePath + "/TestMovedDir");
            string SourcePath = "/TestMovedDir";
            string TargetPath = "/TestMovedDir2";
            Should.NotThrow(() => Moving.TryMoveFileOrDir(SourcePath, TargetPath));
        }

        /// <summary>
        /// Tests moving file to directory
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestMoveFileToDirectory()
        {
            CurrentDirectory.CurrentDir = Paths.HomePath;
            string SourcePath = Path.GetFullPath("TestData/TestMove.txt");
            string TargetPath = "/Documents";
            Should.NotThrow(() => Moving.TryMoveFileOrDir(SourcePath, TargetPath));
        }

        /// <summary>
        /// Tests moving file to file
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestMoveFileToFile()
        {
            CurrentDirectory.CurrentDir = Paths.HomePath;
            string SourcePath = "/Documents/TestMove.txt";
            string TargetPath = Path.GetFullPath("TestData/TestMove.txt");
            Should.NotThrow(() => Moving.TryMoveFileOrDir(SourcePath, TargetPath));
        }

        /// <summary>
        /// Tests attribute removal implementation
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestRemoveAttribute()
        {
            var ExpectedAttributes = FileAttributes.Encrypted | FileAttributes.Directory;
            var InitialAttributes = FileAttributes.Encrypted | FileAttributes.Directory | FileAttributes.Hidden;
            InitialAttributes = InitialAttributes.RemoveAttribute(FileAttributes.Hidden);
            InitialAttributes.ShouldBe(ExpectedAttributes);
        }

        /// <summary>
        /// Tests removing directory
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestRemoveDirectory()
        {
            CurrentDirectory.CurrentDir = Paths.HomePath;
            string TargetPath = Filesystem.NeutralizePath("/TestDir2");
            Should.NotThrow(() => Removing.TryRemoveDirectory(TargetPath));
        }

        /// <summary>
        /// Tests removing file
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestRemoveFile()
        {
            CurrentDirectory.CurrentDir = Paths.HomePath;
            string TargetPath = "/Documents/Text.txt";
            Should.NotThrow(() => Removing.TryRemoveFile(TargetPath));
        }

        /// <summary>
        /// Tests searching file for string
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestSearchFileForString()
        {
            CurrentDirectory.CurrentDir = Paths.HomePath;
            string TargetPath = Path.GetFullPath("TestData/TestText.txt");
            var Matches = Searching.SearchFileForString(TargetPath, "test");
            Matches.ShouldNotBeNull();
            Matches.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests searching file for string using regular expressions
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestSearchFileForStringRegexp()
        {
            CurrentDirectory.CurrentDir = Paths.HomePath;
            string TargetPath = Path.GetFullPath("TestData/TestText.txt");
            var Matches = Searching.SearchFileForStringRegexp(TargetPath, new Regex("test"));
            Matches.ShouldNotBeNull();
            Matches.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests adding attribute
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestAddAttribute()
        {
            CurrentDirectory.CurrentDir = Paths.HomePath;
            string SourcePath = Path.GetFullPath("TestData/TestText.txt");
            Should.NotThrow(() => AttributeManager.TryAddAttributeToFile(SourcePath, FileAttributes.Hidden));
        }

        /// <summary>
        /// Tests deleting attribute
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestDeleteAttribute()
        {
            CurrentDirectory.CurrentDir = Paths.HomePath;
            string SourcePath = Path.GetFullPath("TestData/TestText.txt");
            Should.NotThrow(() => AttributeManager.TryRemoveAttributeFromFile(SourcePath, FileAttributes.Hidden));
        }

        /// <summary>
        /// Tests reading all lines without roadblocks
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestReadAllLinesNoBlock()
        {
            string PathToTestText = Path.GetFullPath("TestData/TestText.txt");
            var LinesTestText = FileRead.ReadAllLinesNoBlock(PathToTestText);
            LinesTestText.ShouldBeOfType(typeof(string[]));
            LinesTestText.ShouldNotBeNull();
            LinesTestText.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests reading all lines
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestReadContents()
        {
            string PathToTestText = Path.GetFullPath("TestData/TestText.txt");
            var LinesTestText = FileRead.ReadContents(PathToTestText);
            LinesTestText.ShouldBeOfType(typeof(string[]));
            LinesTestText.ShouldNotBeNull();
            LinesTestText.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests getting lookup path list
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestGetPathList()
        {
            PathLookupTools.GetPathList().ShouldNotBeNull();
            PathLookupTools.GetPathList().ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests adding a neutralized path to lookup
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestAddToPathLookupNeutralized()
        {
            string Path = KernelPlatform.IsOnWindows() ? @"C:\Program Files\dotnet" : "/bin";
            string NeutralizedPath = Filesystem.NeutralizePath(Path);
            Should.NotThrow(() => PathLookupTools.TryAddToPathLookup(NeutralizedPath));
            Shell.PathsToLookup.ShouldContain(NeutralizedPath);
        }

        /// <summary>
        /// Tests adding a non-neutralized path to lookup
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestAddToPathLookupNonNeutralized()
        {
            string Path = KernelPlatform.IsOnWindows() ? "dotnet" : "bin";
            string NeutralizedPath = Filesystem.NeutralizePath(Path);
            Should.NotThrow(() => PathLookupTools.TryAddToPathLookup(Path));
            Shell.PathsToLookup.ShouldContain(NeutralizedPath);
        }

        /// <summary>
        /// Tests adding a neutralized path to lookup with the root path specified
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestAddToPathLookupNeutralizedWithRootPath()
        {
            string Path = KernelPlatform.IsOnWindows() ? @"C:\Program Files\dotnet" : "/bin";
            string RootPath = KernelPlatform.IsOnWindows() ? @"C:\Program Files" : "/";
            string NeutralizedPath = Filesystem.NeutralizePath(Path, RootPath);
            Should.NotThrow(() => PathLookupTools.TryAddToPathLookup(NeutralizedPath, RootPath));
            Shell.PathsToLookup.ShouldContain(NeutralizedPath);
        }

        /// <summary>
        /// Tests adding a non-neutralized path to lookup with the root path specified
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestAddToPathLookupNonNeutralizedWithRootPath()
        {
            string Path = KernelPlatform.IsOnWindows() ? "dotnet" : "bin";
            string RootPath = KernelPlatform.IsOnWindows() ? @"C:\Program Files" : "/";
            string NeutralizedPath = Filesystem.NeutralizePath(Path, RootPath);
            Should.NotThrow(() => PathLookupTools.TryAddToPathLookup(Path, RootPath));
            Shell.PathsToLookup.ShouldContain(NeutralizedPath);
        }

        /// <summary>
        /// Tests removing a neutralized path to lookup
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestRemoveFromPathLookupNeutralized()
        {
            string Path = KernelPlatform.IsOnWindows() ? @"C:\Program Files\dotnet" : "/bin";
            string NeutralizedPath = Filesystem.NeutralizePath(Path);
            Should.NotThrow(() => PathLookupTools.TryRemoveFromPathLookup(NeutralizedPath));
        }

        /// <summary>
        /// Tests removing a non-neutralized path to lookup
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestRemoveFromPathLookupNonNeutralized()
        {
            string Path = KernelPlatform.IsOnWindows() ? "dotnet" : "bin";
            string NeutralizedPath = Filesystem.NeutralizePath(Path);
            Should.NotThrow(() => PathLookupTools.TryRemoveFromPathLookup(Path));
        }

        /// <summary>
        /// Tests removing a neutralized path to lookup with the root path specified
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestRemoveFromPathLookupNeutralizedWithRootPath()
        {
            string Path = KernelPlatform.IsOnWindows() ? @"C:\Program Files\dotnet" : "/usr/bin";
            string RootPath = KernelPlatform.IsOnWindows() ? @"C:\Program Files" : "/";
            string NeutralizedPath = Filesystem.NeutralizePath(Path, RootPath);
            Should.NotThrow(() => PathLookupTools.TryRemoveFromPathLookup(NeutralizedPath, RootPath));
        }

        /// <summary>
        /// Tests removing a non-neutralized path to lookup with the root path specified
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestRemoveFromPathLookupNonNeutralizedWithRootPath()
        {
            string Path = KernelPlatform.IsOnWindows() ? "dotnet" : "usr/bin";
            string RootPath = KernelPlatform.IsOnWindows() ? @"C:\Program Files" : "/";
            string NeutralizedPath = Filesystem.NeutralizePath(Path, RootPath);
            Should.NotThrow(() => PathLookupTools.TryRemoveFromPathLookup(Path, RootPath));
            PathLookupTools.GetPathList().ShouldNotContain(NeutralizedPath);
        }

        /// <summary>
        /// Tests checking to see if the file exists in any of the lookup paths
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestFileExistsInPath()
        {
            string Path = KernelPlatform.IsOnWindows() ? "netstat.exe" : "bash";
            string NeutralizedPath = "";
            PathLookupTools.FileExistsInPath(Path, ref NeutralizedPath).ShouldBeTrue();
            NeutralizedPath.ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests creating filesystem entries list
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestCreateList()
        {
            var CreatedList = Listing.CreateList(Paths.HomePath);
            CreatedList.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests combining files
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestCombineFiles()
        {
            string PathToTestText = Path.GetFullPath("TestData/TestText.txt");
            string PathToTestTextToBeCombined = Path.GetFullPath("TestData/TestText.txt");
            var Combined = Combination.CombineTextFiles(PathToTestText, new[] { PathToTestTextToBeCombined });
            Combined.ShouldBeOfType(typeof(string[]));
            Combined.ShouldNotBeNull();
            Combined.ShouldNotBeEmpty();
            Combined.Length.ShouldBeGreaterThan(1);
        }

    }
}
