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

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Extras.ColorConvert.Commands;
using Nitrocid.Extras.ColorConvert.Localized;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using System.Linq;
using Nitrocid.Languages;

namespace Nitrocid.Extras.ColorConvert
{
    internal class ColorConvertInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("colorto", /* Localizable */ "NKS_COLORCONVERT_COMMAND_COLORTO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sourceModelName", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz"],
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_SOURCEMODELNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "targetModelName", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz"],
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_TARGETMODELNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "number1", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_NUMBER1_DESC"
                        }),
                        new CommandArgumentPart(true, "number2", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_NUMBER2_DESC"
                        }),
                        new CommandArgumentPart(true, "number3", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_NUMBER3_DESC"
                        }),
                        new CommandArgumentPart(false, "number4", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_NUMBER4_DESC"
                        }),
                    ], true)
                ], new ColorToCommand()),

            new CommandInfo("colortoks", /* Localizable */ "NKS_COLORCONVERT_COMMAND_COLORTOKS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sourceModelName", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz"],
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_SOURCEMODELNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "targetModelName", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz"],
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_TARGETMODELNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "number1", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_NUMBER1_DESC"
                        }),
                        new CommandArgumentPart(true, "number2", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_NUMBER2_DESC"
                        }),
                        new CommandArgumentPart(true, "number3", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_NUMBER3_DESC"
                        }),
                        new CommandArgumentPart(false, "number4", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_NUMBER4_DESC"
                        }),
                    ], true)
                ], new ColorToKSCommand()),
            
            new CommandInfo("colortohex", /* Localizable */ "NKS_COLORCONVERT_COMMAND_COLORTOHEX_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sourceModelName", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz"],
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_SOURCEMODELNAME_DESC"
                        }),

                        new CommandArgumentPart(true, "number1", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_NUMBER1_DESC"
                        }),
                        new CommandArgumentPart(true, "number2", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_NUMBER2_DESC"
                        }),
                        new CommandArgumentPart(true, "number3", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_NUMBER3_DESC"
                        }),
                        new CommandArgumentPart(false, "number4", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_NUMBER4_DESC"
                        }),
                    ], true)
                ], new ColorToHexCommand()),
            
            new CommandInfo("colorspecto", /* Localizable */ "NKS_COLORCONVERT_COMMAND_COLORSPECTO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "targetModelName", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz"],
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_TARGETMODELNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "specifier", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_SPECIFIER_DESC"
                        }),
                    ], true)
                ], new ColorSpecToCommand()),
            
            new CommandInfo("colorspectoks", /* Localizable */ "NKS_COLORCONVERT_COMMAND_COLORSPECTOKS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "targetModelName", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz"],
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_TARGETMODELNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "specifier", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_SPECIFIER_DESC"
                        }),
                    ], true)
                ], new ColorSpecToKSCommand()),
            
            new CommandInfo("colorspectohex", /* Localizable */ "NKS_COLORCONVERT_COMMAND_COLORSPECTOHEX_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "specifier", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_COLORCONVERT_COMMAND_ARGUMENT_SPECIFIER_DESC"
                        }),
                    ], true)
                ], new ColorSpecToHexCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasColorConvert);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.ColorConvert", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.ColorConvert");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
        }

        void IAddon.FinalizeAddon()
        { }
    }
}
