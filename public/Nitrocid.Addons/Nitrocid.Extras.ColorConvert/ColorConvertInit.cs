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
using Nitrocid.Shell.ShellBase.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Modifications;
using System.Linq;

namespace Nitrocid.Extras.ColorConvert
{
    internal class ColorConvertInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("colorto", /* Localizable */ "Converts the source color model to the target color model in numbers.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sourceModelName", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz"],
                            ArgumentDescription = /* Localizable */ "Source color model"
                        }),
                        new CommandArgumentPart(true, "targetModelName", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz"],
                            ArgumentDescription = /* Localizable */ "Target color model"
                        }),
                        new CommandArgumentPart(true, "number1", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "First number"
                        }),
                        new CommandArgumentPart(true, "number2", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Second number"
                        }),
                        new CommandArgumentPart(true, "number3", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Third number"
                        }),
                        new CommandArgumentPart(false, "number4", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Fourth number"
                        }),
                    ], true)
                ], new ColorToCommand()),

            new CommandInfo("colortoks", /* Localizable */ "Converts the source color model to the target color model in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sourceModelName", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz"],
                            ArgumentDescription = /* Localizable */ "Source color model"
                        }),
                        new CommandArgumentPart(true, "targetModelName", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz"],
                            ArgumentDescription = /* Localizable */ "Target color model"
                        }),
                        new CommandArgumentPart(true, "number1", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "First number"
                        }),
                        new CommandArgumentPart(true, "number2", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Second number"
                        }),
                        new CommandArgumentPart(true, "number3", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Third number"
                        }),
                        new CommandArgumentPart(false, "number4", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Fourth number"
                        }),
                    ], true)
                ], new ColorToKSCommand()),
            
            new CommandInfo("colortohex", /* Localizable */ "Converts the source color model to hex.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sourceModelName", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz"],
                            ArgumentDescription = /* Localizable */ "Source color model"
                        }),

                        new CommandArgumentPart(true, "number1", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "First number"
                        }),
                        new CommandArgumentPart(true, "number2", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Second number"
                        }),
                        new CommandArgumentPart(true, "number3", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Third number"
                        }),
                        new CommandArgumentPart(false, "number4", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Fourth number"
                        }),
                    ], true)
                ], new ColorToHexCommand()),
            
            new CommandInfo("colorspecto", /* Localizable */ "Converts the source color model using the color specifier to the target color model.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "targetModelName", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz"],
                            ArgumentDescription = /* Localizable */ "Target color model"
                        }),
                        new CommandArgumentPart(true, "specifier", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Color specifier"
                        }),
                    ], true)
                ], new ColorSpecToCommand()),
            
            new CommandInfo("colorspectoks", /* Localizable */ "Converts the source color model using the color specifier to the target color model in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "targetModelName", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["rgb", "ryb", "cmy", "cmyk", "hsv", "hsl", "yiq", "yuv", "xyz"],
                            ArgumentDescription = /* Localizable */ "Target color model"
                        }),
                        new CommandArgumentPart(true, "specifier", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Color specifier"
                        }),
                    ], true)
                ], new ColorSpecToKSCommand()),
            
            new CommandInfo("colorspectohex", /* Localizable */ "Converts the source color model using the color specifier to the target color model in hex.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "specifier", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Color specifier"
                        }),
                    ], true)
                ], new ColorSpecToHexCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasColorConvert);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);

        void IAddon.FinalizeAddon()
        { }
    }
}
