//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.Files;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Nitrocid.Kernel;

namespace Nitrocid.Tests.Files
{

    [TestClass]
    public class FilesystemNeutralizationTests
    {

        /// <summary>
        /// Tests path neutralization on a folder in home directory
        /// </summary>
        [TestMethod]
        [Description("Neutralization")]
        public void TestNeutralizePaths()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            string TestPath = "Documents";
            string ExpectedPath = InitTest.PathToTestSlotFolder + "/" + TestPath;
            string NeutPath = FilesystemTools.NeutralizePath(TestPath);
            NeutPath.ShouldBe(ExpectedPath);
        }

        /// <summary>
        /// Tests path neutralization on a folder in a custom directory
        /// </summary>
        [TestMethod]
        [Description("Neutralization")]
        public void TestNeutralizePathsCustom()
        {
            if (KernelPlatform.IsOnUnix())
            {
                string TestPath = "sources.list";
                string TargetPath = "/etc/apt";
                string NeutPath = FilesystemTools.NeutralizePath(TestPath, TargetPath);
                NeutPath.ShouldBe(TargetPath + "/" + TestPath);
            }
            else
            {
                string TestPath = "cmd.exe";
                string TargetPath = "C:/WINDOWS/SYSTEM32";
                string NeutPath = FilesystemTools.NeutralizePath(TestPath, TargetPath);
                NeutPath.ShouldBe(TargetPath + "/" + TestPath);
            }
        }

        /// <summary>
        /// Tests throwing on invalid path
        /// </summary>
        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("/usr/lib")]
        [DataRow("C:/Program Files")]
        [DataRow("Music")]
        [Description("Neutralization")]
        public void TestThrowOnInvalidPathValid(string path) =>
            Should.NotThrow(() => FilesystemTools.ThrowOnInvalidPath(path));

        /// <summary>
        /// Tests throwing on invalid path
        /// </summary>
        [TestMethod]
        [DataRow("\\\\.\\globalroot\\device\\condrv\\kernelconnect")]
        [DataRow("C:\\$i30")]
        [Description("Neutralization")]
        public void TestThrowOnInvalidPathInvalid(string path)
        {
            if (KernelPlatform.IsOnWindows())
                Should.Throw(() => FilesystemTools.ThrowOnInvalidPath(path), typeof(KernelException));
        }
    }
}
