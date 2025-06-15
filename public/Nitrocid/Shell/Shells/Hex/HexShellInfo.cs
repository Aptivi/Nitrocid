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

using System.Collections.Generic;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.Prompts;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.Shells.Hex.Commands;
using Nitrocid.Shell.Shells.Hex.Presets;
using Nitrocid.Languages;

namespace Nitrocid.Shell.Shells.Hex
{
    /// <summary>
    /// Common hex shell class
    /// </summary>
    internal class HexShellInfo : BaseShellInfo<HexShell>, IShellInfo
    {

        /// <summary>
        /// Hex commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("addbyte", "NKS_SHELL_SHELLS_HEX_COMMAND_ADDBYTE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "byte", new()
                        {
                            ArgumentDescription = "NKS_SHELL_SHELLS_HEX_ADDBYTE_ARGUMENT_BYTE_DESC"
                        })
                    ])
                ], new AddByteCommand()),

            new CommandInfo("addbytes", "NKS_SHELL_SHELLS_HEX_COMMAND_ADDBYTES_DESC", new AddBytesCommand()),

            new CommandInfo("addbyteto", "NKS_SHELL_SHELLS_HEX_COMMAND_ADDBYTESTO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "byte", new()
                        {
                            ArgumentDescription = "NKS_SHELL_SHELLS_HEX_ADDBYTE_ARGUMENT_BYTE_DESC"
                        }),
                        new CommandArgumentPart(true, "pos", new()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELL_SHELLS_HEX_ADDBYTETO_ARGUMENT_BYTEPOS_DESC"
                        })
                    ])
                ], new AddByteToCommand()),

            new CommandInfo("clear", "NKS_SHELL_SHELLS_HEX_COMMAND_CLEAR_DESC", new ClearCommand()),

            new CommandInfo("delbyte", "NKS_SHELL_SHELLS_HEX_COMMAND_DELBYTE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "bytenumber", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELL_SHELLS_HEX_ADDBYTETO_ARGUMENT_BYTEPOS_DESC"
                        })
                    ])
                ], new DelByteCommand()),

            new CommandInfo("delbytes", "NKS_SHELL_SHELLS_HEX_COMMAND_DELBYTES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "startbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELL_SHELLS_HEX_DELBYTES_ARGUMENT_STARTBYTE_DESC"
                        }),
                        new CommandArgumentPart(false, "endbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELL_SHELLS_HEX_DELBYTES_ARGUMENT_ENDBYTE_DESC"
                        })
                    ])
                ], new DelBytesCommand()),

            new CommandInfo("exitnosave", "NKS_SHELL_SHELLS_HEX_COMMAND_EXITNOSAVE_DESC", new ExitNoSaveCommand()),

            new CommandInfo("print", "NKS_SHELL_SHELLS_HEX_COMMAND_PRINT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "startbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELL_SHELLS_HEX_DELBYTES_ARGUMENT_STARTBYTE_DESC"
                        }),
                        new CommandArgumentPart(false, "endbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELL_SHELLS_HEX_DELBYTES_ARGUMENT_ENDBYTE_DESC"
                        })
                    ])
                ], new PrintCommand(), CommandFlags.Wrappable),

            new CommandInfo("querybyte", "NKS_SHELL_SHELLS_HEX_COMMAND_QUERYBYTE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "byte", new()
                        {
                            ArgumentDescription = "NKS_SHELL_SHELLS_HEX_ADDBYTE_ARGUMENT_BYTE_DESC"
                        }),
                        new CommandArgumentPart(false, "startbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELL_SHELLS_HEX_DELBYTES_ARGUMENT_STARTBYTE_DESC"
                        }),
                        new CommandArgumentPart(false, "endbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELL_SHELLS_HEX_DELBYTES_ARGUMENT_ENDBYTE_DESC"
                        })
                    ])
                ], new QueryByteCommand(), CommandFlags.Wrappable),

            new CommandInfo("replace", "NKS_SHELL_SHELLS_HEX_COMMAND_REPLACE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "byte", new()
                        {
                            ArgumentDescription = "NKS_SHELL_SHELLS_HEX_REPLACE_ARGUMENT_BYTE_DESC"
                        }),
                        new CommandArgumentPart(true, "replacebyte", new()
                        {
                            ArgumentDescription = "NKS_SHELL_SHELLS_HEX_REPLACE_ARGUMENT_REPLACEBYTE_DESC"
                        })
                    ])
                ], new ReplaceCommand()),

            new CommandInfo("save", "NKS_SHELL_SHELLS_HEXTEXT_COMMAND_SAVE_DESC", new SaveCommand()),

            new CommandInfo("tui", "NKS_SHELL_SHELLS_HEX_COMMAND_TUI_DESC", new TuiCommand()),
        ];

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new HexDefaultPreset() },
            { "PowerLine1", new HexPowerLine1Preset() },
            { "PowerLine2", new HexPowerLine2Preset() },
            { "PowerLine3", new HexPowerLine3Preset() },
            { "PowerLineBG1", new HexPowerLineBG1Preset() },
            { "PowerLineBG2", new HexPowerLineBG2Preset() },
            { "PowerLineBG3", new HexPowerLineBG3Preset() }
        };
    }
}
