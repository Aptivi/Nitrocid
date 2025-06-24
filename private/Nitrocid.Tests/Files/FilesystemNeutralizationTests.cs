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

using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Nitrocid.Base.Kernel;

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
    }
}
