//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.IO;
using System.Text.RegularExpressions;
using Nitrocid.Base.Files;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Shouldly.ShouldlyExtensionMethods;

namespace Nitrocid.Tests.Files
{

    [TestClass]
    public class FilesystemManipulationTests
    {

        /// <summary>
        /// Tests copying directory to directory
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestCopyDirectoryToDirectory()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            Directory.CreateDirectory(InitTest.PathToTestSlotFolder + "/TestDir");
            string SourcePath = "TestDir";
            string TargetPath = "TestDir2";
            Should.NotThrow(() => FilesystemTools.CopyFileOrDir(SourcePath, TargetPath));
        }

        /// <summary>
        /// Tests copying file to directory
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestCopyFileToDirectory()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            string SourcePath = Path.GetFullPath("TestData/TestText.txt");
            string TargetPath = "TestDir/";
            Should.NotThrow(() => FilesystemTools.CopyFileOrDir(SourcePath, TargetPath));
        }

        /// <summary>
        /// Tests copying file to file
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestCopyFileToFile()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            string SourcePath = Path.GetFullPath("TestData/TestText.txt");
            string TargetPath = "TestDir/Text.txt";
            Should.NotThrow(() => FilesystemTools.CopyFileOrDir(SourcePath, TargetPath));
        }

        /// <summary>
        /// Tests making directory
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestMakeDirectory()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            Should.NotThrow(() => FilesystemTools.MakeDirectory("NewDirectory"));
        }

        /// <summary>
        /// Tests making file
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestMakeFile()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            Should.NotThrow(() => FilesystemTools.MakeFile("NewFile.txt"));
        }

        /// <summary>
        /// Tests making file
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestMakeJsonFile()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            Should.NotThrow(() => FilesystemTools.MakeJsonFile("NewFile.json"));
        }

        /// <summary>
        /// Tests moving directory to directory
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestMoveDirectoryToDirectory()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            Directory.CreateDirectory(InitTest.PathToTestSlotFolder + "/TestMovedDir");
            string SourcePath = "TestMovedDir";
            string TargetPath = "TestMovedDir2";
            Should.NotThrow(() => FilesystemTools.MoveFileOrDir(SourcePath, TargetPath));
        }

        /// <summary>
        /// Tests moving file to directory
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestMoveFileToDirectory()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            string SourcePath = Path.GetFullPath("TestData/TestMove.txt");
            string TargetPath = "TestDir";
            Should.NotThrow(() => FilesystemTools.MoveFileOrDir(SourcePath, TargetPath));
        }

        /// <summary>
        /// Tests moving file to file
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestMoveFileToFile()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            string SourcePath = "TestDir/TestMove.txt";
            string TargetPath = Path.GetFullPath("TestData/TestMove.txt");
            Should.NotThrow(() => FilesystemTools.MoveFileOrDir(SourcePath, TargetPath));
        }

        /// <summary>
        /// Tests the theory of attribute removal implementation
        /// </summary>
        [TestMethod]
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
        [TestMethod]
        [Description("Manipulation")]
        public void TestRemoveDirectory()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            string TargetPath = FilesystemTools.NeutralizePath("TestDir2");
            Should.NotThrow(() => FilesystemTools.RemoveDirectory(TargetPath));
        }

        /// <summary>
        /// Tests removing file
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestRemoveFile()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            string TargetPath = "TestDir/Text.txt";
            Should.NotThrow(() => FilesystemTools.RemoveFile(TargetPath));
        }

        /// <summary>
        /// Tests searching file for string
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestSearchFileForString()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            string TargetPath = Path.GetFullPath("TestData/TestText.txt");
            var Matches = FilesystemTools.SearchFileForString(TargetPath, "test");
            Matches.ShouldNotBeNull();
            Matches.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests searching file for string using regular expressions
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestSearchFileForStringRegexp()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            string TargetPath = Path.GetFullPath("TestData/TestText.txt");
            var Matches = FilesystemTools.SearchFileForStringRegexp(TargetPath, new Regex("test"));
            Matches.ShouldNotBeNull();
            Matches.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests adding attribute
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestAddAttribute()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            string SourcePath = Path.GetFullPath("TestData/TestText.txt");
            Should.NotThrow(() => FilesystemTools.AddAttributeToFile(SourcePath, FileAttributes.Hidden));
        }

        /// <summary>
        /// Tests deleting attribute
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestDeleteAttribute()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            string SourcePath = Path.GetFullPath("TestData/TestText.txt");
            Should.NotThrow(() => FilesystemTools.RemoveAttributeFromFile(SourcePath, FileAttributes.Hidden));
        }

        /// <summary>
        /// Tests reading all lines without roadblocks
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestReadAllLinesNoBlock()
        {
            string PathToTestText = Path.GetFullPath("TestData/TestText.txt");
            var LinesTestText = FilesystemTools.ReadAllLinesNoBlock(PathToTestText);
            LinesTestText.ShouldBeOfType(typeof(string[]));
            LinesTestText.ShouldNotBeNull();
            LinesTestText.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests reading all lines
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestReadContents()
        {
            string PathToTestText = Path.GetFullPath("TestData/TestText.txt");
            var LinesTestText = FilesystemTools.ReadContents(PathToTestText);
            LinesTestText.ShouldBeOfType(typeof(string[]));
            LinesTestText.ShouldNotBeNull();
            LinesTestText.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests reading all text without roadblocks
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestReadAllTextNoBlock()
        {
            string PathToTestText = Path.GetFullPath("TestData/TestText.txt");
            string LinesTestText = FilesystemTools.ReadAllTextNoBlock(PathToTestText);
            LinesTestText.ShouldBeOfType(typeof(string));
            LinesTestText.ShouldNotBeNull();
            LinesTestText.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests reading all text
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestReadContentsText()
        {
            string PathToTestText = Path.GetFullPath("TestData/TestText.txt");
            string LinesTestText = FilesystemTools.ReadContentsText(PathToTestText);
            LinesTestText.ShouldBeOfType(typeof(string));
            LinesTestText.ShouldNotBeNull();
            LinesTestText.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests reading all lines without roadblocks
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestWriteAllLinesNoBlock()
        {
            string PathToTestText = Path.GetFullPath("TestData/TestText.txt");
            var LinesTestText = FilesystemTools.ReadContents(PathToTestText);
            LinesTestText.ShouldBeOfType(typeof(string[]));
            LinesTestText.ShouldNotBeNull();
            LinesTestText.ShouldNotBeEmpty();
            string PathToTestDestinationText = Path.GetFullPath($"TestData/{nameof(TestWriteAllLinesNoBlock)}.txt");
            FilesystemTools.WriteAllLinesNoBlock(PathToTestDestinationText, LinesTestText);
            var LinesTestText2 = FilesystemTools.ReadContents(PathToTestText);
            LinesTestText2.ShouldBeOfType(typeof(string[]));
            LinesTestText2.ShouldNotBeNull();
            LinesTestText2.ShouldNotBeEmpty();
            LinesTestText2.ShouldBe(LinesTestText);
        }

        /// <summary>
        /// Tests reading all lines
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestWriteContents()
        {
            string PathToTestText = Path.GetFullPath("TestData/TestText.txt");
            var LinesTestText = FilesystemTools.ReadContents(PathToTestText);
            LinesTestText.ShouldBeOfType(typeof(string[]));
            LinesTestText.ShouldNotBeNull();
            LinesTestText.ShouldNotBeEmpty();
            string PathToTestDestinationText = Path.GetFullPath($"TestData/{nameof(TestWriteContents)}.txt");
            FilesystemTools.WriteContents(PathToTestDestinationText, LinesTestText);
            var LinesTestText2 = FilesystemTools.ReadContents(PathToTestText);
            LinesTestText2.ShouldBeOfType(typeof(string[]));
            LinesTestText2.ShouldNotBeNull();
            LinesTestText2.ShouldNotBeEmpty();
            LinesTestText2.ShouldBe(LinesTestText);
        }

        /// <summary>
        /// Tests reading all text without roadblocks
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestWriteAllTextNoBlock()
        {
            string PathToTestText = Path.GetFullPath("TestData/TestText.txt");
            string LinesTestText = FilesystemTools.ReadContentsText(PathToTestText);
            LinesTestText.ShouldBeOfType(typeof(string));
            LinesTestText.ShouldNotBeNull();
            LinesTestText.ShouldNotBeEmpty();
            string PathToTestDestinationText = Path.GetFullPath($"TestData/{nameof(TestWriteAllTextNoBlock)}.txt");
            FilesystemTools.WriteAllTextNoBlock(PathToTestDestinationText, LinesTestText);
            string LinesTestText2 = FilesystemTools.ReadContentsText(PathToTestText);
            LinesTestText2.ShouldBeOfType(typeof(string));
            LinesTestText2.ShouldNotBeNull();
            LinesTestText2.ShouldNotBeEmpty();
            LinesTestText2.ShouldBe(LinesTestText);
        }

        /// <summary>
        /// Tests reading all text
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestWriteContentsText()
        {
            string PathToTestText = Path.GetFullPath("TestData/TestText.txt");
            string LinesTestText = FilesystemTools.ReadContentsText(PathToTestText);
            LinesTestText.ShouldBeOfType(typeof(string));
            LinesTestText.ShouldNotBeNull();
            LinesTestText.ShouldNotBeEmpty();
            string PathToTestDestinationText = Path.GetFullPath($"TestData/{nameof(TestWriteContentsText)}.txt");
            FilesystemTools.WriteContentsText(PathToTestDestinationText, LinesTestText);
            string LinesTestText2 = FilesystemTools.ReadContentsText(PathToTestText);
            LinesTestText2.ShouldBeOfType(typeof(string));
            LinesTestText2.ShouldNotBeNull();
            LinesTestText2.ShouldNotBeEmpty();
            LinesTestText2.ShouldBe(LinesTestText);
        }

        /// <summary>
        /// Tests getting lookup path list
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestGetPathList()
        {
            PathLookupTools.GetPathList().ShouldNotBeNull();
            PathLookupTools.GetPathList().ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests adding a neutralized path to lookup
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestAddToPathLookupNeutralized()
        {
            string Path = KernelPlatform.IsOnWindows() ? @"C:\Program Files\dotnet" : "/bin";
            string NeutralizedPath = FilesystemTools.NeutralizePath(Path);
            Should.NotThrow(() => PathLookupTools.AddToPathLookup(NeutralizedPath));
            Config.MainConfig.PathsToLookup.ShouldContain(NeutralizedPath);
        }

        /// <summary>
        /// Tests adding a non-neutralized path to lookup
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestAddToPathLookupNonNeutralized()
        {
            string Path = KernelPlatform.IsOnWindows() ? "dotnet" : "bin";
            string NeutralizedPath = FilesystemTools.NeutralizePath(Path);
            Should.NotThrow(() => PathLookupTools.AddToPathLookup(Path));
            Config.MainConfig.PathsToLookup.ShouldContain(NeutralizedPath);
        }

        /// <summary>
        /// Tests adding a neutralized path to lookup with the root path specified
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestAddToPathLookupNeutralizedWithRootPath()
        {
            string Path = KernelPlatform.IsOnWindows() ? @"C:\Program Files\dotnet" : "/bin";
            string RootPath = KernelPlatform.IsOnWindows() ? @"C:\Program Files" : "/";
            string NeutralizedPath = FilesystemTools.NeutralizePath(Path, RootPath);
            Should.NotThrow(() => PathLookupTools.AddToPathLookup(NeutralizedPath, RootPath));
            Config.MainConfig.PathsToLookup.ShouldContain(NeutralizedPath);
        }

        /// <summary>
        /// Tests adding a non-neutralized path to lookup with the root path specified
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestAddToPathLookupNonNeutralizedWithRootPath()
        {
            string Path = KernelPlatform.IsOnWindows() ? "dotnet" : "bin";
            string RootPath = KernelPlatform.IsOnWindows() ? @"C:\Program Files" : "/";
            string NeutralizedPath = FilesystemTools.NeutralizePath(Path, RootPath);
            Should.NotThrow(() => PathLookupTools.AddToPathLookup(Path, RootPath));
            Config.MainConfig.PathsToLookup.ShouldContain(NeutralizedPath);
        }

        /// <summary>
        /// Tests removing a neutralized path to lookup
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestRemoveFromPathLookupNeutralized()
        {
            string Path = KernelPlatform.IsOnWindows() ? @"C:\Program Files\dotnet" : "/bin";
            string NeutralizedPath = FilesystemTools.NeutralizePath(Path);
            Should.NotThrow(() => PathLookupTools.RemoveFromPathLookup(NeutralizedPath));
        }

        /// <summary>
        /// Tests removing a non-neutralized path to lookup
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestRemoveFromPathLookupNonNeutralized()
        {
            string Path = KernelPlatform.IsOnWindows() ? "dotnet" : "bin";
            string NeutralizedPath = FilesystemTools.NeutralizePath(Path);
            Should.NotThrow(() => PathLookupTools.RemoveFromPathLookup(Path));
        }

        /// <summary>
        /// Tests removing a neutralized path to lookup with the root path specified
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestRemoveFromPathLookupNeutralizedWithRootPath()
        {
            string Path = KernelPlatform.IsOnWindows() ? @"C:\Program Files\dotnet" : "/usr/bin";
            string RootPath = KernelPlatform.IsOnWindows() ? @"C:\Program Files" : "/";
            string NeutralizedPath = FilesystemTools.NeutralizePath(Path, RootPath);
            Should.NotThrow(() => PathLookupTools.RemoveFromPathLookup(NeutralizedPath, RootPath));
        }

        /// <summary>
        /// Tests removing a non-neutralized path to lookup with the root path specified
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestRemoveFromPathLookupNonNeutralizedWithRootPath()
        {
            string Path = KernelPlatform.IsOnWindows() ? "dotnet" : "usr/bin";
            string RootPath = KernelPlatform.IsOnWindows() ? @"C:\Program Files" : "/";
            string NeutralizedPath = FilesystemTools.NeutralizePath(Path, RootPath);
            Should.NotThrow(() => PathLookupTools.RemoveFromPathLookup(Path, RootPath));
            PathLookupTools.GetPathList().ShouldNotContain(NeutralizedPath);
        }

        /// <summary>
        /// Tests checking to see if the file exists in any of the lookup paths
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestFileExistsInPath()
        {
            string Path = KernelPlatform.IsOnWindows() ? "netstat.exe" : "bash";
            string? NeutralizedPath = "";
            PathLookupTools.FileExistsInPath(Path, ref NeutralizedPath).ShouldBeTrue();
            NeutralizedPath.ShouldNotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests creating filesystem entries list
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestCreateList()
        {
            var CreatedList = FilesystemTools.CreateList(InitTest.PathToTestSlotFolder);
            CreatedList.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests combining files
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestCombineFiles()
        {
            string PathToTestText = Path.GetFullPath("TestData/TestText.txt");
            string PathToTestTextToBeCombined = Path.GetFullPath("TestData/TestText.txt");
            var Combined = FilesystemTools.CombineTextFiles(PathToTestText, [PathToTestTextToBeCombined]);
            Combined.ShouldBeOfType(typeof(string[]));
            Combined.ShouldNotBeNull();
            Combined.ShouldNotBeEmpty();
            Combined.Length.ShouldBeGreaterThan(1);
        }

        /// <summary>
        /// Tests removing attribute
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestRemoveAttributeExtension()
        {
            var CreatedList = FilesystemTools.RemoveAttribute(FileAttributes.System, FileAttributes.System);
            CreatedList.ShouldNotHaveFlag(FileAttributes.System);
        }

    }
}
