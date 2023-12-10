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

using System.IO;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Themes;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;

namespace Nitrocid.Tests.ConsoleBase
{

    [TestFixture]
    public class ThemeInfoInitializationTests
    {

        /// <summary>
        /// Tests initializing an instance of ThemeInfo from KS resources
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeThemeInfoFromResources()
        {
            // Create instance
            var ThemeInfoInstance = new ThemeInfo();

            // Check for null
            ThemeInfoInstance.ThemeColors.ShouldNotBeNull();
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(KernelColorType)).Length - 1; typeIndex++)
            {
                KernelColorType type = ThemeInfoInstance.ThemeColors.Keys.ElementAt(typeIndex);
                ThemeInfoInstance.ThemeColors[type].ShouldNotBeNull();
            }
        }

        /// <summary>
        /// Tests initializing an instance of ThemeInfo from all KS resources
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestGetThemeInfoFromAllResources()
        {
            var installedThemes = ThemeTools.GetInstalledThemes();
            foreach (string ResourceName in ThemeTools.GetInstalledThemes().Keys)
            {
                // Special naming cases
                string ThemeName = ResourceName.Replace(" ", "_");
                switch (ResourceName)
                {
                    case "3Y-Diamond":
                        {
                            ThemeName = "_3Y_Diamond";
                            break;
                        }
                }

                // Create instance
                var ThemeInfoInstance = installedThemes[ThemeName];

                // Check for null
                ThemeInfoInstance.ThemeColors.ShouldNotBeNull();
                for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(KernelColorType)).Length - 1; typeIndex++)
                {
                    KernelColorType type = ThemeInfoInstance.ThemeColors.Keys.ElementAt(typeIndex);
                    ThemeInfoInstance.ThemeColors[type].ShouldNotBeNull();
                }
            }
        }

        /// <summary>
        /// Tests initializing an instance of ThemeInfo from file
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeThemeInfoFromFile()
        {
            // Create instance
            string SourcePath = Path.GetFullPath("TestData/Hacker.json");
            var ThemeInfoStream = new StreamReader(SourcePath);
            var ThemeInfoInstance = new ThemeInfo(ThemeInfoStream);
            ThemeInfoStream.Close();

            // Check for null
            ThemeInfoInstance.ThemeColors.ShouldNotBeNull();
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(KernelColorType)).Length - 1; typeIndex++)
            {
                KernelColorType type = ThemeInfoInstance.ThemeColors.Keys.ElementAt(typeIndex);
                ThemeInfoInstance.ThemeColors[type].ShouldNotBeNull();
            }
        }

        /// <summary>
        /// Tests initializing an instance of ThemeInfo from file
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeThemeInfoFromFilePath()
        {
            // Create instance
            string SourcePath = Path.GetFullPath("TestData/Hacker.json");
            var ThemeInfoInstance = new ThemeInfo(SourcePath);

            // Check for null
            ThemeInfoInstance.ThemeColors.ShouldNotBeNull();
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(KernelColorType)).Length - 1; typeIndex++)
            {
                KernelColorType type = ThemeInfoInstance.ThemeColors.Keys.ElementAt(typeIndex);
                ThemeInfoInstance.ThemeColors[type].ShouldNotBeNull();
            }
        }

    }
}
