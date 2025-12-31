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

using System.Collections.Generic;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Prompts;
using Terminaux.Shell.Arguments;
using Nitrocid.Base.Shell.Shells.Hex.Commands;
using Nitrocid.Base.Shell.Shells.Hex.Presets;

namespace Nitrocid.Base.Shell.Shells.Hex
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
            new CommandInfo("addbyte", /* Localizable */ "NKS_SHELL_SHELLS_HEX_COMMAND_ADDBYTE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "byte", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_ADDBYTE_ARGUMENT_BYTE_DESC"
                        })
                    ])
                ], new AddByteCommand()),

            new CommandInfo("addbytes", /* Localizable */ "NKS_SHELL_SHELLS_HEX_COMMAND_ADDBYTES_DESC", new AddBytesCommand()),

            new CommandInfo("addbyteto", /* Localizable */ "NKS_SHELL_SHELLS_HEX_COMMAND_ADDBYTESTO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "byte", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_ADDBYTE_ARGUMENT_BYTE_DESC"
                        }),
                        new CommandArgumentPart(true, "pos", new()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_ADDBYTETO_ARGUMENT_BYTEPOS_DESC"
                        })
                    ])
                ], new AddByteToCommand()),

            new CommandInfo("clear", /* Localizable */ "NKS_SHELL_SHELLS_HEX_COMMAND_CLEAR_DESC", new ClearCommand()),

            new CommandInfo("delbyte", /* Localizable */ "NKS_SHELL_SHELLS_HEX_COMMAND_DELBYTE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "bytenumber", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_ADDBYTETO_ARGUMENT_BYTEPOS_DESC"
                        })
                    ])
                ], new DelByteCommand()),

            new CommandInfo("delbytes", /* Localizable */ "NKS_SHELL_SHELLS_HEX_COMMAND_DELBYTES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "startbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_DELBYTES_ARGUMENT_STARTBYTE_DESC"
                        }),
                        new CommandArgumentPart(false, "endbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_DELBYTES_ARGUMENT_ENDBYTE_DESC"
                        })
                    ])
                ], new DelBytesCommand()),

            new CommandInfo("exitnosave", /* Localizable */ "NKS_SHELL_SHELLS_HEX_COMMAND_EXITNOSAVE_DESC", new ExitNoSaveCommand()),

            new CommandInfo("print", /* Localizable */ "NKS_SHELL_SHELLS_HEX_COMMAND_PRINT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "startbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_DELBYTES_ARGUMENT_STARTBYTE_DESC"
                        }),
                        new CommandArgumentPart(false, "endbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_DELBYTES_ARGUMENT_ENDBYTE_DESC"
                        })
                    ])
                ], new PrintCommand(), CommandFlags.Wrappable),

            new CommandInfo("querybyte", /* Localizable */ "NKS_SHELL_SHELLS_HEX_COMMAND_QUERYBYTE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "byte", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_ADDBYTE_ARGUMENT_BYTE_DESC"
                        }),
                        new CommandArgumentPart(false, "startbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_DELBYTES_ARGUMENT_STARTBYTE_DESC"
                        }),
                        new CommandArgumentPart(false, "endbyte", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_DELBYTES_ARGUMENT_ENDBYTE_DESC"
                        })
                    ])
                ], new QueryByteCommand(), CommandFlags.Wrappable),

            new CommandInfo("replace", /* Localizable */ "NKS_SHELL_SHELLS_HEX_COMMAND_REPLACE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "byte", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_REPLACE_ARGUMENT_BYTE_DESC"
                        }),
                        new CommandArgumentPart(true, "replacebyte", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_HEX_REPLACE_ARGUMENT_REPLACEBYTE_DESC"
                        })
                    ])
                ], new ReplaceCommand()),

            new CommandInfo("save", /* Localizable */ "NKS_SHELL_SHELLS_HEXTEXT_COMMAND_SAVE_DESC", new SaveCommand()),

            new CommandInfo("tui", /* Localizable */ "NKS_SHELL_SHELLS_HEX_COMMAND_TUI_DESC", new TuiCommand()),
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
