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

using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Nitrocid.Files;

namespace Nitrocid.Tests.Files
{

    [TestClass]
    public class FilesystemSettingTests
    {

        /// <summary>
        /// Tests current directory setting
        /// </summary>
        [TestMethod]
        [Description("Setting")]
        public void TestSetCurrDir()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            string Path = PathsManagement.ExecPath;
            FilesystemTools.SetCurrDir(Path);
            Path.ShouldBe(FilesystemTools.CurrentDir);
        }

        /// <summary>
        /// Tests current directory setting
        /// </summary>
        [TestMethod]
        [Description("Setting")]
        public void TestTrySetCurrDir()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            string Path = PathsManagement.ExecPath;
            FilesystemTools.TrySetCurrDir(Path).ShouldBeTrue();
            Path.ShouldBe(FilesystemTools.CurrentDir);
        }

        /// <summary>
        /// Tests saving the current directory value
        /// </summary>
        [TestMethod]
        [Description("Manipulation")]
        public void TestSaveCurrDir()
        {
            Config.MainConfig.CurrentDir = InitTest.PathToTestSlotFolder;
            Config.CreateConfig();
            Config.MainConfig.CurrentDir.ShouldBe(InitTest.PathToTestSlotFolder);
        }

    }
}
