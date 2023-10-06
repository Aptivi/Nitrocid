
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Files;
using KS.Files.Operations.Querying;
using NUnit.Framework;
using Shouldly;
using System;
using System.IO;

namespace Nitrocid.Tests.Files
{

    [TestFixture]
    public class FilesystemQueryingTests
    {

        /// <summary>
        /// Tests checking if file exists
        /// </summary>
        [Test]
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
        [Test]
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
        [Test]
        [Description("Querying")]
        public void TestGetKernelPaths()
        {
            foreach (KernelPathType PathType in Enum.GetValues(typeof(KernelPathType)))
            {
                Console.WriteLine($"Path type: {PathType}");
                string TargetKernelPath = Paths.GetKernelPath(PathType);
                Console.WriteLine($"Got path: {TargetKernelPath}");
                TargetKernelPath.ShouldNotBeNullOrEmpty();
            }
        }

        /// <summary>
        /// Tests getting a numbered file name
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetNumberedFileName()
        {
            string numbered = Getting.GetNumberedFileName(InitTest.PathToTestSlotFolder, "testnum.txt");
            numbered.ShouldContain("testnum-0.txt");
        }

        /// <summary>
        /// Tests getting a random file name
        /// </summary>
        [Test]
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
        [Test]
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
        [TestCase(@"C:\Windows", IncludePlatform = "win", ExpectedResult = true)]
        [TestCase(@"C:\Windows<>", IncludePlatform = "win", ExpectedResult = false)]
        [TestCase("/usr/bin", IncludePlatform = "linux,unix,macosx", ExpectedResult = true)]
        [TestCase("/usr/bin\0", IncludePlatform = "linux,unix,macosx", ExpectedResult = false)]
        [Description("Querying")]
        public bool TestTryParsePath(string Path) =>
            Parsing.TryParsePath(Path);

        /// <summary>
        /// Tests trying to parse the file name
        /// </summary>
        [TestCase("Windows", ExpectedResult = true)]
        [TestCase(@"Windows/System32\", ExpectedResult = false)]
        [Description("Querying")]
        public bool TestTryParseFileName(string Path) =>
            Parsing.TryParseFileName(Path);

    }
}
