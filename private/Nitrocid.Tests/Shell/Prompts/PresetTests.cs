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

using Terminaux.Shell.Prompts;
using Terminaux.Shell.Shells;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Nitrocid.Tests.Shell.Prompts
{
    [TestClass]
    public class PresetTests
    {
        /// <summary>
        /// Tests setting preset
        /// </summary>
        [TestMethod]
        [DataRow("PowerLine1", "Shell", "PowerLine1")]
        [DataRow("PowerLine1", "AdminShell", "PowerLine1")]
        [DataRow("PowerLine1", "DebugShell", "PowerLine1")]
        [DataRow("PowerLine1", "HexShell", "PowerLine1")]
        [DataRow("PowerLine1", "TextShell", "PowerLine1")]
        [Description("Action")]
        public void TestSetPresetDry(string presetName, ShellType type, string expected)
        {
            PromptPresetManager.SetPreset(presetName, type);
            string baseName = PromptPresetManager.GetCurrentPresetBaseFromShell(type).PresetName;
            baseName.ShouldBe(expected);
        }

        /// <summary>
        /// Tests setting preset
        /// </summary>
        [TestMethod]
        [DataRow("PowerLine1", "Shell", "PowerLine1")]
        [DataRow("PowerLine1", "AdminShell", "PowerLine1")]
        [DataRow("PowerLine1", "DebugShell", "PowerLine1")]
        [DataRow("PowerLine1", "HexShell", "PowerLine1")]
        [DataRow("PowerLine1", "TextShell", "PowerLine1")]
        [Description("Action")]
        public void TestSetPresetDry(string presetName, string type, string expected)
        {
            PromptPresetManager.SetPreset(presetName, type);
            string baseName = PromptPresetManager.GetCurrentPresetBaseFromShell(type).PresetName;
            baseName.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting preset list from shell
        /// </summary>
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
        [Description("Action")]
        public void TestGetPresetsFromShell(ShellType type)
        {
            var presets = PromptPresetManager.GetPresetsFromShell(type);
            presets.ShouldNotBeNull();
            presets.ShouldNotBeEmpty();
            presets.ShouldContainKey("PowerLine1");
        }

        /// <summary>
        /// Tests getting preset list from shell
        /// </summary>
        [TestMethod]
        [DataRow("Shell")]
        [DataRow("AdminShell")]
        [DataRow("DebugShell")]
        [DataRow("HexShell")]
        [DataRow("TextShell")]
        [Description("Action")]
        public void TestGetPresetsFromShell(string type)
        {
            var presets = PromptPresetManager.GetPresetsFromShell(type);
            presets.ShouldNotBeNull();
            presets.ShouldNotBeEmpty();
            presets.ShouldContainKey("PowerLine1");
        }
    }
}
