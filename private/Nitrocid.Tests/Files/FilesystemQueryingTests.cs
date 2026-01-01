//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Nitrocid.Files.Operations.Querying;
using Nitrocid.Files.Paths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.IO;
using Nitrocid.Kernel;

namespace Nitrocid.Tests.Files
{

    [TestClass]
    public class FilesystemQueryingTests
    {

        /// <summary>
        /// Tests checking if file exists
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestFileExists()
        {
            string TargetFile = Path.GetFullPath("TestData/TestText.txt");
            string TargetFile2 = Path.GetFullPath("TestData/TestTexts.txt");
            Checking.FileExists(TargetFile).ShouldBeTrue();
            Checking.FileExists(TargetFile2).ShouldBeFalse();
        }

        /// <summary>
        /// Tests checking if directory exists
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestDirectoryExists()
        {
            string TargetDirectory = Path.GetFullPath("EmptyFiles");
            string TargetDirectory2 = Path.GetFullPath("EmptyFile");
            Checking.FolderExists(TargetDirectory).ShouldBeTrue();
            Checking.FolderExists(TargetDirectory2).ShouldBeFalse();
        }

        /// <summary>
        /// Tests getting the kernel path for each entry
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetKernelPaths()
        {
            foreach (KernelPathType PathType in Enum.GetValues(typeof(KernelPathType)))
            {
                Console.WriteLine($"Path type: {PathType}");
                string TargetKernelPath = PathsManagement.GetKernelPath(PathType);
                Console.WriteLine($"Got path: {TargetKernelPath}");
                TargetKernelPath.ShouldNotBeNullOrEmpty();
            }
        }

        /// <summary>
        /// Tests checking for path registration
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestIsKernelPathRegistered()
        {
            foreach (KernelPathType PathType in Enum.GetValues(typeof(KernelPathType)))
            {
                Console.WriteLine($"Path type: {PathType}");
                string type = PathsManagement.GetKernelPathName(PathType);
                bool regged = PathsManagement.IsKernelPathRegistered(type);
                regged.ShouldBeTrue();
            }
        }

        /// <summary>
        /// Tests checking for built-in path registration
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestIsKernelPathBuiltin()
        {
            foreach (KernelPathType PathType in Enum.GetValues(typeof(KernelPathType)))
            {
                Console.WriteLine($"Path type: {PathType}");
                string type = PathsManagement.GetKernelPathName(PathType);
                bool regged = PathsManagement.IsKernelPathBuiltin(type);
                regged.ShouldBeTrue();
            }
        }

        /// <summary>
        /// Tests the custom path registration, querying, and unregistration
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestCustomPath()
        {
            string type = "Custom";
            PathsManagement.RegisterKernelPath(type, InitTest.PathToTestSlotFolder);
            string path = PathsManagement.GetKernelPath(type);
            PathsManagement.UnregisterKernelPath(type);
            path.ShouldBe(InitTest.PathToTestSlotFolder);
        }

        /// <summary>
        /// Tests getting a numbered file name
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetNumberedFileName()
        {
            string numbered = Getting.GetNumberedFileName(InitTest.PathToTestSlotFolder, "testnum.txt");
            numbered.ShouldContain("testnum-0.txt");
        }

        /// <summary>
        /// Tests getting a random file name
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetRandomFileName()
        {
            string numbered = Getting.GetRandomFileName();
            numbered.ShouldNotBeEmpty();
            Parsing.TryParsePath(numbered).ShouldBeTrue();
        }

        /// <summary>
        /// Tests getting a random directory name
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetRandomFolderName()
        {
            string numbered = Getting.GetRandomFolderName();
            numbered.ShouldNotBeEmpty();
            Parsing.TryParsePath(numbered).ShouldBeTrue();
        }

        /// <summary>
        /// Tests trying to parse the path name
        /// </summary>
        [TestMethod]
        [DataRow("/usr/bin", true)]
        [DataRow("/usr/bin\0", false)]
        [Description("Querying")]
        public void TestTryParsePathUnix(string Path, bool expected)
        {
            if (KernelPlatform.IsOnUnix())
            {
                bool actual = Parsing.TryParsePath(Path);
                actual.ShouldBe(expected);
            }
        }

        /// <summary>
        /// Tests trying to parse the path name
        /// </summary>
        [TestMethod]
        [DataRow(@"C:\Windows", true)]
        [DataRow(@"C:\Windows<>", false)]
        [Description("Querying")]
        public void TestTryParsePathWindows(string Path, bool expected)
        {
            if (KernelPlatform.IsOnWindows())
            {
                bool actual = Parsing.TryParsePath(Path);
                actual.ShouldBe(expected);
            }
        }

        /// <summary>
        /// Tests trying to parse the file name
        /// </summary>
        [TestMethod]
        [DataRow("Windows", true)]
        [DataRow(@"Windows/System32\", false)]
        [Description("Querying")]
        public void TestTryParseFileName(string Path, bool expected)
        {
            bool actual = Parsing.TryParseFileName(Path);
            actual.ShouldBe(expected);
        }
    }
}
