
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

using System.Collections.Generic;
using System.Data;
using System.Linq;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs.Styles;
using KS.Kernel.Configuration;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.Prompts.Presets.FTP;
using KS.Shell.Prompts.Presets.Hex;
using KS.Shell.Prompts.Presets.HTTP;
using KS.Shell.Prompts.Presets.Json;
using KS.Shell.Prompts.Presets.Mail;
using KS.Shell.Prompts.Presets.RAR;
using KS.Shell.Prompts.Presets.RSS;
using KS.Shell.Prompts.Presets.SFTP;
using KS.Shell.Prompts.Presets.Test;
using KS.Shell.Prompts.Presets.Text;
using KS.Shell.Prompts.Presets.UESH;
using KS.Shell.Prompts.Presets.ZIP;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Prompts
{
    public static class PromptPresetManager
    {

        // Shell presets
        internal readonly static Dictionary<string, PromptPresetBase> UESHShellPresets = new() { { "Default", new DefaultPreset() }, { "PowerLine1", new PowerLine1Preset() }, { "PowerLine2", new PowerLine2Preset() }, { "PowerLine3", new PowerLine3Preset() }, { "PowerLineBG1", new PowerLineBG1Preset() }, { "PowerLineBG2", new PowerLineBG2Preset() }, { "PowerLineBG3", new PowerLineBG3Preset() } };
        internal readonly static Dictionary<string, PromptPresetBase> TestShellPresets = new() { { "Default", new TestDefaultPreset() }, { "PowerLine1", new TestPowerLine1Preset() }, { "PowerLine2", new TestPowerLine2Preset() }, { "PowerLine3", new TestPowerLine3Preset() }, { "PowerLineBG1", new TestPowerLineBG1Preset() }, { "PowerLineBG2", new TestPowerLineBG2Preset() }, { "PowerLineBG3", new TestPowerLineBG3Preset() } };
        internal readonly static Dictionary<string, PromptPresetBase> ZipShellPresets = new() { { "Default", new ZipDefaultPreset() }, { "PowerLine1", new ZipPowerLine1Preset() }, { "PowerLine2", new ZipPowerLine2Preset() }, { "PowerLine3", new ZipPowerLine3Preset() }, { "PowerLineBG1", new ZipPowerLineBG1Preset() }, { "PowerLineBG2", new ZipPowerLineBG2Preset() }, { "PowerLineBG3", new ZipPowerLineBG3Preset() } };
        internal readonly static Dictionary<string, PromptPresetBase> TextShellPresets = new() { { "Default", new TextDefaultPreset() }, { "PowerLine1", new TextPowerLine1Preset() }, { "PowerLine2", new TextPowerLine2Preset() }, { "PowerLine3", new TextPowerLine3Preset() }, { "PowerLineBG1", new TextPowerLineBG1Preset() }, { "PowerLineBG2", new TextPowerLineBG2Preset() }, { "PowerLineBG3", new TextPowerLineBG3Preset() } };
        internal readonly static Dictionary<string, PromptPresetBase> SFTPShellPresets = new() { { "Default", new SFTPDefaultPreset() }, { "PowerLine1", new SftpPowerLine1Preset() }, { "PowerLine2", new SftpPowerLine2Preset() }, { "PowerLine3", new SftpPowerLine3Preset() }, { "PowerLineBG1", new SftpPowerLineBG1Preset() }, { "PowerLineBG2", new SftpPowerLineBG2Preset() }, { "PowerLineBG3", new SftpPowerLineBG3Preset() } };
        internal readonly static Dictionary<string, PromptPresetBase> RSSShellPresets = new() { { "Default", new RSSDefaultPreset() }, { "PowerLine1", new RSSPowerLine1Preset() }, { "PowerLine2", new RSSPowerLine2Preset() }, { "PowerLine3", new RSSPowerLine3Preset() }, { "PowerLineBG1", new RSSPowerLineBG1Preset() }, { "PowerLineBG2", new RSSPowerLineBG2Preset() }, { "PowerLineBG3", new RSSPowerLineBG3Preset() } };
        internal readonly static Dictionary<string, PromptPresetBase> MailShellPresets = new() { { "Default", new MailDefaultPreset() }, { "PowerLine1", new MailPowerLine1Preset() }, { "PowerLine2", new MailPowerLine2Preset() }, { "PowerLine3", new MailPowerLine3Preset() }, { "PowerLineBG1", new MailPowerLineBG1Preset() }, { "PowerLineBG2", new MailPowerLineBG2Preset() }, { "PowerLineBG3", new MailPowerLineBG3Preset() } };
        internal readonly static Dictionary<string, PromptPresetBase> JsonShellPresets = new() { { "Default", new JsonDefaultPreset() }, { "PowerLine1", new JsonPowerLine1Preset() }, { "PowerLine2", new JsonPowerLine2Preset() }, { "PowerLine3", new JsonPowerLine3Preset() }, { "PowerLineBG1", new JsonPowerLineBG1Preset() }, { "PowerLineBG2", new JsonPowerLineBG2Preset() }, { "PowerLineBG3", new JsonPowerLineBG3Preset() } };
        internal readonly static Dictionary<string, PromptPresetBase> HTTPShellPresets = new() { { "Default", new HTTPDefaultPreset() }, { "PowerLine1", new HTTPPowerLine1Preset() }, { "PowerLine2", new HTTPPowerLine2Preset() }, { "PowerLine3", new HTTPPowerLine3Preset() }, { "PowerLineBG1", new HTTPPowerLineBG1Preset() }, { "PowerLineBG2", new HTTPPowerLineBG2Preset() }, { "PowerLineBG3", new HTTPPowerLineBG3Preset() } };
        internal readonly static Dictionary<string, PromptPresetBase> HexShellPresets = new() { { "Default", new HexDefaultPreset() }, { "PowerLine1", new HexPowerLine1Preset() }, { "PowerLine2", new HexPowerLine2Preset() }, { "PowerLine3", new HexPowerLine3Preset() }, { "PowerLineBG1", new HexPowerLineBG1Preset() }, { "PowerLineBG2", new HexPowerLineBG2Preset() }, { "PowerLineBG3", new HexPowerLineBG3Preset() } };
        internal readonly static Dictionary<string, PromptPresetBase> FTPShellPresets = new() { { "Default", new FTPDefaultPreset() }, { "PowerLine1", new FtpPowerLine1Preset() }, { "PowerLine2", new FtpPowerLine2Preset() }, { "PowerLine3", new FtpPowerLine3Preset() }, { "PowerLineBG1", new FtpPowerLineBG1Preset() }, { "PowerLineBG2", new FtpPowerLineBG2Preset() }, { "PowerLineBG3", new FtpPowerLineBG3Preset() } };
        internal readonly static Dictionary<string, PromptPresetBase> RARShellPresets = new() { { "Default", new RarDefaultPreset() }, { "PowerLine1", new RarPowerLine1Preset() }, { "PowerLine2", new RarPowerLine2Preset() }, { "PowerLine3", new RarPowerLine3Preset() }, { "PowerLineBG1", new RarPowerLineBG1Preset() }, { "PowerLineBG2", new RarPowerLineBG2Preset() }, { "PowerLineBG3", new RarPowerLineBG3Preset() } };

        // Custom shell presets used by mods
        internal readonly static Dictionary<string, PromptPresetBase> UESHCustomShellPresets = new();
        internal readonly static Dictionary<string, PromptPresetBase> TestCustomShellPresets = new();
        internal readonly static Dictionary<string, PromptPresetBase> ZipCustomShellPresets = new();
        internal readonly static Dictionary<string, PromptPresetBase> TextCustomShellPresets = new();
        internal readonly static Dictionary<string, PromptPresetBase> SFTPCustomShellPresets = new();
        internal readonly static Dictionary<string, PromptPresetBase> RSSCustomShellPresets = new();
        internal readonly static Dictionary<string, PromptPresetBase> MailCustomShellPresets = new();
        internal readonly static Dictionary<string, PromptPresetBase> JsonCustomShellPresets = new();
        internal readonly static Dictionary<string, PromptPresetBase> HTTPCustomShellPresets = new();
        internal readonly static Dictionary<string, PromptPresetBase> HexCustomShellPresets = new();
        internal readonly static Dictionary<string, PromptPresetBase> FTPCustomShellPresets = new();
        internal readonly static Dictionary<string, PromptPresetBase> RARCustomShellPresets = new();

        // Current presets
        internal static PromptPresetBase UESHShellCurrentPreset = UESHShellPresets["Default"];
        internal static PromptPresetBase TestShellCurrentPreset = TestShellPresets["Default"];
        internal static PromptPresetBase ZipShellCurrentPreset = ZipShellPresets["Default"];
        internal static PromptPresetBase TextShellCurrentPreset = TextShellPresets["Default"];
        internal static PromptPresetBase SFTPShellCurrentPreset = SFTPShellPresets["Default"];
        internal static PromptPresetBase RSSShellCurrentPreset = RSSShellPresets["Default"];
        internal static PromptPresetBase MailShellCurrentPreset = MailShellPresets["Default"];
        internal static PromptPresetBase JsonShellCurrentPreset = JsonShellPresets["Default"];
        internal static PromptPresetBase HTTPShellCurrentPreset = HTTPShellPresets["Default"];
        internal static PromptPresetBase HexShellCurrentPreset = HexShellPresets["Default"];
        internal static PromptPresetBase FTPShellCurrentPreset = FTPShellPresets["Default"];
        internal static PromptPresetBase RARShellCurrentPreset = FTPShellPresets["Default"];

        /// <summary>
        /// Sets the shell preset
        /// </summary>
        /// <param name="PresetName">The preset name</param>
        public static void SetPreset(string PresetName, ShellType ShellType, bool ThrowOnNotFound = true)
        {
            var Presets = GetPresetsFromShell(ShellType);
            var CustomPresets = GetCustomPresetsFromShell(ShellType);

            // Check to see if we have the preset
            if (Presets.ContainsKey(PresetName))
            {
                SetPresetInternal(PresetName, ShellType, Presets);
            }
            else if (CustomPresets.ContainsKey(PresetName))
            {
                SetPresetInternal(PresetName, ShellType, CustomPresets);
            }
            else if (ThrowOnNotFound)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Preset {0} for {1} doesn't exist. Throwing...", PresetName, ShellType.ToString());
                throw new NoSuchShellPresetException(Translate.DoTranslation("The specified preset {0} is not found."), PresetName);
            }
            else
            {
                SetPresetInternal("Default", ShellType, Presets);
            }
        }

        /// <summary>
        /// Sets the preset
        /// </summary>
        /// <param name="PresetName">The preset name</param>
        /// <param name="ShellType">The shell type</param>
        internal static void SetPresetInternal(string PresetName, ShellType ShellType, Dictionary<string, PromptPresetBase> Presets)
        {
            switch (ShellType)
            {
                case ShellType.Shell:
                    {
                        UESHShellCurrentPreset = Presets[PresetName];
                        ConfigTools.SetConfigValue(Config.ConfigCategory.Shell, "Prompt Preset", PresetName);
                        break;
                    }
                case ShellType.TestShell:
                    {
                        TestShellCurrentPreset = Presets[PresetName];
                        ConfigTools.SetConfigValue(Config.ConfigCategory.Shell, "Test Shell Prompt Preset", PresetName);
                        break;
                    }
                case ShellType.ZIPShell:
                    {
                        ZipShellCurrentPreset = Presets[PresetName];
                        ConfigTools.SetConfigValue(Config.ConfigCategory.Shell, "Zip Shell Prompt Preset", PresetName);
                        break;
                    }
                case ShellType.TextShell:
                    {
                        TextShellCurrentPreset = Presets[PresetName];
                        ConfigTools.SetConfigValue(Config.ConfigCategory.Shell, "Text Edit Prompt Preset", PresetName);
                        break;
                    }
                case ShellType.SFTPShell:
                    {
                        SFTPShellCurrentPreset = Presets[PresetName];
                        ConfigTools.SetConfigValue(Config.ConfigCategory.Shell, "SFTP Prompt Preset", PresetName);
                        break;
                    }
                case ShellType.RSSShell:
                    {
                        RSSShellCurrentPreset = Presets[PresetName];
                        ConfigTools.SetConfigValue(Config.ConfigCategory.Shell, "RSS Prompt Preset", PresetName);
                        break;
                    }
                case ShellType.MailShell:
                    {
                        MailShellCurrentPreset = Presets[PresetName];
                        ConfigTools.SetConfigValue(Config.ConfigCategory.Shell, "Mail Prompt Preset", PresetName);
                        break;
                    }
                case ShellType.JsonShell:
                    {
                        JsonShellCurrentPreset = Presets[PresetName];
                        ConfigTools.SetConfigValue(Config.ConfigCategory.Shell, "JSON Shell Prompt Preset", PresetName);
                        break;
                    }
                case ShellType.HTTPShell:
                    {
                        HTTPShellCurrentPreset = Presets[PresetName];
                        ConfigTools.SetConfigValue(Config.ConfigCategory.Shell, "HTTP Shell Prompt Preset", PresetName);
                        break;
                    }
                case ShellType.HexShell:
                    {
                        HexShellCurrentPreset = Presets[PresetName];
                        ConfigTools.SetConfigValue(Config.ConfigCategory.Shell, "Hex Edit Prompt Preset", PresetName);
                        break;
                    }
                case ShellType.FTPShell:
                    {
                        FTPShellCurrentPreset = Presets[PresetName];
                        ConfigTools.SetConfigValue(Config.ConfigCategory.Shell, "FTP Prompt Preset", PresetName);
                        break;
                    }
                case ShellType.RARShell:
                    {
                        RARShellCurrentPreset = Presets[PresetName];
                        ConfigTools.SetConfigValue(Config.ConfigCategory.Shell, "RAR Shell Prompt Preset", PresetName);
                        break;
                    }
            }
        }

        /// <summary>
        /// Gets the current preset base from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static PromptPresetBase GetCurrentPresetBaseFromShell(ShellType ShellType)
        {
            switch (ShellType)
            {
                case ShellType.Shell:
                    {
                        return UESHShellCurrentPreset;
                    }
                case ShellType.TestShell:
                    {
                        return TestShellCurrentPreset;
                    }
                case ShellType.ZIPShell:
                    {
                        return ZipShellCurrentPreset;
                    }
                case ShellType.TextShell:
                    {
                        return TextShellCurrentPreset;
                    }
                case ShellType.SFTPShell:
                    {
                        return SFTPShellCurrentPreset;
                    }
                case ShellType.RSSShell:
                    {
                        return RSSShellCurrentPreset;
                    }
                case ShellType.MailShell:
                    {
                        return MailShellCurrentPreset;
                    }
                case ShellType.JsonShell:
                    {
                        return JsonShellCurrentPreset;
                    }
                case ShellType.HTTPShell:
                    {
                        return HTTPShellCurrentPreset;
                    }
                case ShellType.HexShell:
                    {
                        return HexShellCurrentPreset;
                    }
                case ShellType.FTPShell:
                    {
                        return FTPShellCurrentPreset;
                    }
                case ShellType.RARShell:
                    {
                        return RARShellCurrentPreset;
                    }

                default:
                    {
                        return UESHShellCurrentPreset;
                    }
            }
        }

        /// <summary>
        /// Gets the predefined presets from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, PromptPresetBase> GetPresetsFromShell(ShellType ShellType)
        {
            switch (ShellType)
            {
                case ShellType.Shell:
                    {
                        return UESHShellPresets;
                    }
                case ShellType.TestShell:
                    {
                        return TestShellPresets;
                    }
                case ShellType.ZIPShell:
                    {
                        return ZipShellPresets;
                    }
                case ShellType.TextShell:
                    {
                        return TextShellPresets;
                    }
                case ShellType.SFTPShell:
                    {
                        return SFTPShellPresets;
                    }
                case ShellType.RSSShell:
                    {
                        return RSSShellPresets;
                    }
                case ShellType.MailShell:
                    {
                        return MailShellPresets;
                    }
                case ShellType.JsonShell:
                    {
                        return JsonShellPresets;
                    }
                case ShellType.HTTPShell:
                    {
                        return HTTPShellPresets;
                    }
                case ShellType.HexShell:
                    {
                        return HexShellPresets;
                    }
                case ShellType.FTPShell:
                    {
                        return FTPShellPresets;
                    }
                case ShellType.RARShell:
                    {
                        return RARShellPresets;
                    }

                default:
                    {
                        return UESHShellPresets;
                    }
            }
        }

        /// <summary>
        /// Gets the custom presets (defined by mods) from the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static Dictionary<string, PromptPresetBase> GetCustomPresetsFromShell(ShellType ShellType)
        {
            switch (ShellType)
            {
                case ShellType.Shell:
                    {
                        return UESHCustomShellPresets;
                    }
                case ShellType.TestShell:
                    {
                        return TestCustomShellPresets;
                    }
                case ShellType.ZIPShell:
                    {
                        return ZipCustomShellPresets;
                    }
                case ShellType.TextShell:
                    {
                        return TextCustomShellPresets;
                    }
                case ShellType.SFTPShell:
                    {
                        return SFTPCustomShellPresets;
                    }
                case ShellType.RSSShell:
                    {
                        return RSSCustomShellPresets;
                    }
                case ShellType.MailShell:
                    {
                        return MailCustomShellPresets;
                    }
                case ShellType.JsonShell:
                    {
                        return JsonCustomShellPresets;
                    }
                case ShellType.HTTPShell:
                    {
                        return HTTPCustomShellPresets;
                    }
                case ShellType.HexShell:
                    {
                        return HexCustomShellPresets;
                    }
                case ShellType.FTPShell:
                    {
                        return FTPCustomShellPresets;
                    }
                case ShellType.RARShell:
                    {
                        return RARCustomShellPresets;
                    }

                default:
                    {
                        return UESHCustomShellPresets;
                    }
            }
        }

        /// <summary>
        /// Writes the shell prompt
        /// </summary>
        /// <param name="ShellType">Shell type</param>
        public static void WriteShellPrompt(ShellType ShellType)
        {
            var CurrentPresetBase = GetCurrentPresetBaseFromShell(ShellType);
            TextWriterColor.Write(CurrentPresetBase.PresetPrompt, false, ColorTools.ColTypes.Input);

            // Set input color in case custom preset didn't set the input color as instructed
            ColorTools.SetInputColor();
        }

        /// <summary>
        /// Prompts a user to select the preset
        /// </summary>
        public static void PromptForPresets()
        {
            var ShellType = ShellStart.ShellStack[ShellStart.ShellStack.Count - 1].ShellType;
            var Presets = GetPresetsFromShell(ShellType);

            // Add the custom presets to the local dictionary
            foreach (string PresetName in GetCustomPresetsFromShell(ShellType).Keys)
                Presets.Add(PresetName, Presets[PresetName]);

            // Now, prompt the user
            var PresetNames = Presets.Keys.ToArray();
            var PresetDisplays = Presets.Values.Select(Preset => Preset.PresetPrompt).ToArray();
            string SelectedPreset = ChoiceStyle.PromptChoice(Translate.DoTranslation("Select preset for {0}:").FormatString(ShellType), string.Join("/", PresetNames), PresetDisplays, ChoiceStyle.ChoiceOutputType.Modern, true);
            SetPreset(SelectedPreset, ShellType);
        }

    }
}