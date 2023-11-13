﻿//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Shell.Prompts;
using KS.Shell.ShellBase.Shells;
using NUnit.Framework;
using Shouldly;

namespace Nitrocid.Tests.Shell.Prompts
{
    [TestFixture]
    public class PresetTests
    {
        /// <summary>
        /// Tests setting preset
        /// </summary>
        [Test]
        [TestCase("PowerLine1", ShellType.Shell, ExpectedResult = "PowerLine1")]
        [TestCase("PowerLine1", ShellType.AdminShell, ExpectedResult = "PowerLine1")]
        [TestCase("PowerLine1", ShellType.DebugShell, ExpectedResult = "PowerLine1")]
        [TestCase("PowerLine1", ShellType.HexShell, ExpectedResult = "PowerLine1")]
        [TestCase("PowerLine1", ShellType.JsonShell, ExpectedResult = "PowerLine1")]
        [TestCase("PowerLine1", ShellType.TextShell, ExpectedResult = "PowerLine1")]
        [Description("Action")]
        public static string TestSetPresetDry(string presetName, ShellType type)
        {
            PromptPresetManager.SetPreset(presetName, type);
            return PromptPresetManager.GetCurrentPresetBaseFromShell(presetName).PresetName;
        }

        /// <summary>
        /// Tests setting preset
        /// </summary>
        [Test]
        [TestCase("PowerLine1", "Shell", ExpectedResult = "PowerLine1")]
        [TestCase("PowerLine1", "AdminShell", ExpectedResult = "PowerLine1")]
        [TestCase("PowerLine1", "DebugShell", ExpectedResult = "PowerLine1")]
        [TestCase("PowerLine1", "HexShell", ExpectedResult = "PowerLine1")]
        [TestCase("PowerLine1", "JsonShell", ExpectedResult = "PowerLine1")]
        [TestCase("PowerLine1", "TextShell", ExpectedResult = "PowerLine1")]
        [Description("Action")]
        public static string TestSetPresetDry(string presetName, string type)
        {
            PromptPresetManager.SetPreset(presetName, type);
            return PromptPresetManager.GetCurrentPresetBaseFromShell(presetName).PresetName;
        }

        /// <summary>
        /// Tests getting preset list from shell
        /// </summary>
        [Test]
        [TestCase(ShellType.Shell)]
        [TestCase(ShellType.AdminShell)]
        [TestCase(ShellType.DebugShell)]
        [TestCase(ShellType.HexShell)]
        [TestCase(ShellType.JsonShell)]
        [TestCase(ShellType.TextShell)]
        [Description("Action")]
        public static void TestGetPresetsFromShell(ShellType type)
        {
            var presets = PromptPresetManager.GetPresetsFromShell(type);
            presets.ShouldNotBeNull();
            presets.ShouldNotBeEmpty();
            presets.ShouldContainKey("PowerLine1");
        }

        /// <summary>
        /// Tests getting preset list from shell
        /// </summary>
        [Test]
        [TestCase("Shell")]
        [TestCase("AdminShell")]
        [TestCase("DebugShell")]
        [TestCase("HexShell")]
        [TestCase("JsonShell")]
        [TestCase("TextShell")]
        [Description("Action")]
        public static void TestGetPresetsFromShell(string type)
        {
            var presets = PromptPresetManager.GetPresetsFromShell(type);
            presets.ShouldNotBeNull();
            presets.ShouldNotBeEmpty();
            presets.ShouldContainKey("PowerLine1");
        }
    }
}