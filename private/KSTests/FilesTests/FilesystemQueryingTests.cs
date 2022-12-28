
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

using KS.Files;
using KS.Files.LineEndings;
using KS.Files.Querying;
using NUnit.Framework;
using Shouldly;
using System;
using System.Diagnostics;
using System.IO;

namespace KSTests.FilesTests
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
                Debug.WriteLine($"Path type: {PathType}");
                string TargetKernelPath = Paths.GetKernelPath(PathType);
                Debug.WriteLine($"Got path: {TargetKernelPath}");
                TargetKernelPath.ShouldNotBeNullOrEmpty();
            }
        }

        /// <summary>
        /// Tests trying to parse the path name
        /// </summary>
        [TestCase(@"C:\Windows", IncludePlatform = "win", ExpectedResult = true)]
        [TestCase(@"C:\Windows<>", IncludePlatform = "win", ExpectedResult = false)]
        [TestCase("/usr/bin", IncludePlatform = "linux,unix,macosx", ExpectedResult = true)]
        [TestCase("/usr/bin\0", IncludePlatform = "linux,unix,macosx", ExpectedResult = false)]
        [Description("Querying")]
        public bool TestTryParsePath(string Path) => Parsing.TryParsePath(Path);

        /// <summary>
        /// Tests trying to parse the file name
        /// </summary>
        [TestCase("Windows", ExpectedResult = true)]
        [TestCase(@"Windows/System32\", ExpectedResult = false)]
        [Description("Querying")]
        public bool TestTryParseFileName(string Path) => Parsing.TryParseFileName(Path);

        /// <summary>
        /// Tests trying to get the line ending from text file
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetLineEndingFromFile()
        {
            var ExpectedStyle = FilesystemNewlineStyle.LF;
            var ActualStyle = LineEndingsTools.GetLineEndingFromFile(Path.GetFullPath("TestData/TestText.txt"));
            ActualStyle.ShouldBe(ExpectedStyle);
        }

    }
}
