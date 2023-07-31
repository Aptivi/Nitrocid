
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
using KS.Files.Folders;
using KS.Kernel.Configuration;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Files
{

    [TestFixture]
    public class FilesystemSettingTests
    {

        /// <summary>
        /// Tests current directory setting
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestSetCurrDir()
        {
            Config.MainConfig.CurrentDir = Paths.HomePath;
            string Path = Paths.AppDataPath;
            CurrentDirectory.SetCurrDir(Path);
            Path.ShouldBe(CurrentDirectory.CurrentDir);
        }

        /// <summary>
        /// Tests current directory setting
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestTrySetCurrDir()
        {
            Config.MainConfig.CurrentDir = Paths.HomePath;
            string Path = Paths.AppDataPath;
            CurrentDirectory.TrySetCurrDir(Path).ShouldBeTrue();
            Path.ShouldBe(CurrentDirectory.CurrentDir);
        }

        /// <summary>
        /// Tests saving the current directory value
        /// </summary>
        [Test]
        [Description("Manipulation")]
        public void TestSaveCurrDir()
        {
            Config.MainConfig.CurrentDir = Paths.HomePath;
            Config.CreateConfig();
            Config.MainConfig.CurrentDir.ShouldBe(Paths.HomePath);
        }

    }
}
