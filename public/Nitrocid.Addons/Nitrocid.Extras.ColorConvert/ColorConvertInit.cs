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
            new CommandInfo("colorhextorgb", /* Localizable */ "Converts the hexadecimal representation of the color to RGB numbers.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToRgbCommand()),

            new CommandInfo("colorhextorgbks", /* Localizable */ "Converts the hexadecimal representation of the color to RGB numbers in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToRgbKSCommand()),
            
            new CommandInfo("colorhextoryb", /* Localizable */ "Converts the hexadecimal representation of the color to RYB numbers.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToRybCommand()),

            new CommandInfo("colorhextorybks", /* Localizable */ "Converts the hexadecimal representation of the color to RYB numbers in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToRybKSCommand()),

            new CommandInfo("colorhextocmyk", /* Localizable */ "Converts the hexadecimal representation of the color to CMYK numbers.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToCmykCommand()),

            new CommandInfo("colorhextocmykks", /* Localizable */ "Converts the hexadecimal representation of the color to CMYK numbers in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToCmykKSCommand()),

            new CommandInfo("colorhextocmy", /* Localizable */ "Converts the hexadecimal representation of the color to CMY numbers.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToCmyCommand()),

            new CommandInfo("colorhextocmyks", /* Localizable */ "Converts the hexadecimal representation of the color to CMY numbers in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToCmyKSCommand()),

            new CommandInfo("colorhextohsl", /* Localizable */ "Converts the hexadecimal representation of the color to HSL numbers.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToHslCommand()),

            new CommandInfo("colorhextohslks", /* Localizable */ "Converts the hexadecimal representation of the color to HSL numbers in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToHslKSCommand()),

            new CommandInfo("colorhextohsv", /* Localizable */ "Converts the hexadecimal representation of the color to HSV numbers.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToHsvCommand()),

            new CommandInfo("colorhextohsvks", /* Localizable */ "Converts the hexadecimal representation of the color to HSV numbers in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToHsvKSCommand()),

            new CommandInfo("colorhextoyiq", /* Localizable */ "Converts the hexadecimal representation of the color to YIQ numbers.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToYiqCommand()),

            new CommandInfo("colorhextoyiqks", /* Localizable */ "Converts the hexadecimal representation of the color to YIQ numbers in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToYiqKSCommand()),

            new CommandInfo("colorhextoyuv", /* Localizable */ "Converts the hexadecimal representation of the color to YUV numbers.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToYuvCommand()),

            new CommandInfo("colorhextoyuvks", /* Localizable */ "Converts the hexadecimal representation of the color to YUV numbers in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "#RRGGBB"),
                    ], true)
                ], new ColorHexToYuvKSCommand()),

            new CommandInfo("colorrgbtohex", /* Localizable */ "Converts the color RGB numbers to hex.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "G", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRgbToHexCommand()),

            new CommandInfo("colorrgbtocmyk", /* Localizable */ "Converts the color RGB numbers to CMYK.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "G", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRgbToCmykCommand()),

            new CommandInfo("colorrgbtocmykks", /* Localizable */ "Converts the color RGB numbers to CMYK in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "G", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRgbToCmykKSCommand()),

            new CommandInfo("colorrgbtocmy", /* Localizable */ "Converts the color RGB numbers to CMY.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "G", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRgbToCmyCommand()),

            new CommandInfo("colorrgbtocmyks", /* Localizable */ "Converts the color RGB numbers to CMY in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "G", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRgbToCmyKSCommand()),

            new CommandInfo("colorrgbtohsl", /* Localizable */ "Converts the color RGB numbers to HSL.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "G", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRgbToHslCommand()),

            new CommandInfo("colorrgbtohslks", /* Localizable */ "Converts the color RGB numbers to HSL in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "G", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRgbToHslKSCommand()),

            new CommandInfo("colorrgbtohsv", /* Localizable */ "Converts the color RGB numbers to HSV.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "G", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRgbToHsvCommand()),

            new CommandInfo("colorrgbtohsvks", /* Localizable */ "Converts the color RGB numbers to HSV in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "G", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRgbToHsvKSCommand()),

            new CommandInfo("colorrgbtoryb", /* Localizable */ "Converts the color RGB numbers to RYB.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "G", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRgbToRybCommand()),

            new CommandInfo("colorrgbtorybks", /* Localizable */ "Converts the color RGB numbers to RYB in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "G", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRgbToRybKSCommand()),

            new CommandInfo("colorrgbtoyiq", /* Localizable */ "Converts the color RGB numbers to YIQ.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "G", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRgbToYiqCommand()),

            new CommandInfo("colorrgbtoyiqks", /* Localizable */ "Converts the color RGB numbers to YIQ in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "G", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRgbToYiqKSCommand()),

            new CommandInfo("colorrgbtoyuv", /* Localizable */ "Converts the color RGB numbers to YUV.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "G", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRgbToYuvCommand()),

            new CommandInfo("colorrgbtoyuvks", /* Localizable */ "Converts the color RGB numbers to YUV in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "G", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRgbToYuvKSCommand()),

            new CommandInfo("colorrybtohex", /* Localizable */ "Converts the color RYB numbers to hex.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRybToHexCommand()),

            new CommandInfo("colorrybtocmyk", /* Localizable */ "Converts the color RYB numbers to CMYK.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRybToCmykCommand()),

            new CommandInfo("colorrybtocmykks", /* Localizable */ "Converts the color RYB numbers to CMYK in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRybToCmykKSCommand()),

            new CommandInfo("colorrybtocmy", /* Localizable */ "Converts the color RYB numbers to CMY.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRybToCmyCommand()),

            new CommandInfo("colorrybtocmyks", /* Localizable */ "Converts the color RYB numbers to CMY in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRybToCmyKSCommand()),

            new CommandInfo("colorrybtohsl", /* Localizable */ "Converts the color RYB numbers to HSL.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRybToHslCommand()),

            new CommandInfo("colorrybtohslks", /* Localizable */ "Converts the color RYB numbers to HSL in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRybToHslKSCommand()),

            new CommandInfo("colorrybtohsv", /* Localizable */ "Converts the color RYB numbers to HSV.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRybToHsvCommand()),

            new CommandInfo("colorrybtohsvks", /* Localizable */ "Converts the color RYB numbers to HSV in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRybToHsvKSCommand()),

            new CommandInfo("colorrybtorgb", /* Localizable */ "Converts the color RYB numbers to RGB.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRybToRgbCommand()),

            new CommandInfo("colorrybtorgbks", /* Localizable */ "Converts the color RYB numbers to RGB in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRybToRgbKSCommand()),

            new CommandInfo("colorrybtoyiq", /* Localizable */ "Converts the color RYB numbers to YIQ.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRybToYiqCommand()),

            new CommandInfo("colorrybtoyiqks", /* Localizable */ "Converts the color RYB numbers to YIQ in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRybToYiqKSCommand()),

            new CommandInfo("colorrybtoyuv", /* Localizable */ "Converts the color RYB numbers to YUV.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRybToYuvCommand()),

            new CommandInfo("colorrybtoyuvks", /* Localizable */ "Converts the color RYB numbers to YUV in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "R", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "B", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorRybToYuvKSCommand()),

            new CommandInfo("colorhsltohex", /* Localizable */ "Converts the color HSL numbers to hex.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "L", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHslToHexCommand()),

            new CommandInfo("colorhsltocmyk", /* Localizable */ "Converts the color HSL numbers to CMYK.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "L", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHslToCmykCommand()),

            new CommandInfo("colorhsltocmykks", /* Localizable */ "Converts the color HSL numbers to CMYK in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "L", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHslToCmykKSCommand()),

            new CommandInfo("colorhsltocmy", /* Localizable */ "Converts the color HSL numbers to CMY.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "L", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHslToCmyCommand()),

            new CommandInfo("colorhsltocmyks", /* Localizable */ "Converts the color HSL numbers to CMY in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "L", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHslToCmyKSCommand()),

            new CommandInfo("colorhsltohsv", /* Localizable */ "Converts the color HSL numbers to HSV.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "L", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHslToHsvCommand()),

            new CommandInfo("colorhsltohsvks", /* Localizable */ "Converts the color HSL numbers to HSV in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "L", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHslToHsvKSCommand()),

            new CommandInfo("colorhsltorgb", /* Localizable */ "Converts the color HSL numbers to RGB.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "L", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHslToRgbCommand()),

            new CommandInfo("colorhsltorgbks", /* Localizable */ "Converts the color HSL numbers to RGB in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "L", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHslToRgbKSCommand()),

            new CommandInfo("colorhsltoryb", /* Localizable */ "Converts the color HSL numbers to RYB.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "L", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHslToRybCommand()),

            new CommandInfo("colorhsltorybks", /* Localizable */ "Converts the color HSL numbers to RYB in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "L", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHslToRybKSCommand()),

            new CommandInfo("colorhsltoyiq", /* Localizable */ "Converts the color HSL numbers to YIQ.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "L", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHslToYiqCommand()),

            new CommandInfo("colorhsltoyiqks", /* Localizable */ "Converts the color HSL numbers to YIQ in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "L", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHslToYiqKSCommand()),

            new CommandInfo("colorhsltoyuv", /* Localizable */ "Converts the color HSL numbers to YUV.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "L", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHslToYuvCommand()),

            new CommandInfo("colorhsltoyuvks", /* Localizable */ "Converts the color HSL numbers to YUV in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "L", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHslToYuvKSCommand()),

            new CommandInfo("colorhsvtohex", /* Localizable */ "Converts the color HSV numbers to hex.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "V", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHsvToHexCommand()),

            new CommandInfo("colorhsvtocmyk", /* Localizable */ "Converts the color HSV numbers to CMYK.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "V", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHsvToCmykCommand()),

            new CommandInfo("colorhsvtocmykks", /* Localizable */ "Converts the color HSV numbers to CMYK in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "V", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHsvToCmykKSCommand()),

            new CommandInfo("colorhsvtocmy", /* Localizable */ "Converts the color HSV numbers to CMY.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "V", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHsvToCmyCommand()),

            new CommandInfo("colorhsvtocmyks", /* Localizable */ "Converts the color HSV numbers to CMY in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "V", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHsvToCmyKSCommand()),

            new CommandInfo("colorhsvtohsl", /* Localizable */ "Converts the color HSV numbers to HSL.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "V", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHsvToHslCommand()),

            new CommandInfo("colorhsvtohslks", /* Localizable */ "Converts the color HSV numbers to HSL in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "V", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHsvToHslKSCommand()),

            new CommandInfo("colorhsvtorgb", /* Localizable */ "Converts the color HSV numbers to RGB.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "V", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHsvToRgbCommand()),

            new CommandInfo("colorhsvtorgbks", /* Localizable */ "Converts the color HSV numbers to RGB in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "V", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHsvToRgbKSCommand()),

            new CommandInfo("colorhsvtoryb", /* Localizable */ "Converts the color HSV numbers to RYB.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "V", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHsvToRybCommand()),

            new CommandInfo("colorhsvtorybks", /* Localizable */ "Converts the color HSV numbers to RYB in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "V", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHsvToRybKSCommand()),

            new CommandInfo("colorhsvtoyiq", /* Localizable */ "Converts the color HSV numbers to YIQ.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "V", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHsvToYiqCommand()),

            new CommandInfo("colorhsvtoyiqks", /* Localizable */ "Converts the color HSV numbers to YIQ in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "V", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHsvToYiqKSCommand()),

            new CommandInfo("colorhsvtoyuv", /* Localizable */ "Converts the color HSV numbers to YUV.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "V", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHsvToYuvCommand()),

            new CommandInfo("colorhsvtoyuvks", /* Localizable */ "Converts the color HSV numbers to YUV in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "H", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "S", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "V", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorHsvToYuvKSCommand()),

            new CommandInfo("colorcmyktohex", /* Localizable */ "Converts the color CMYK numbers to hex.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "K", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmykToHexCommand()),

            new CommandInfo("colorcmyktorgb", /* Localizable */ "Converts the color CMYK numbers to RGB.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "K", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmykToRgbCommand()),

            new CommandInfo("colorcmyktorgbks", /* Localizable */ "Converts the color CMYK numbers to RGB in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "K", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmykToRgbKSCommand()),

            new CommandInfo("colorcmyktoryb", /* Localizable */ "Converts the color CMYK numbers to RYB.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "K", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmykToRybCommand()),

            new CommandInfo("colorcmyktorybks", /* Localizable */ "Converts the color CMYK numbers to RYB in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "K", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmykToRybKSCommand()),

            new CommandInfo("colorcmyktohsl", /* Localizable */ "Converts the color CMYK numbers to HSL.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "K", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmykToHslCommand()),

            new CommandInfo("colorcmyktohslks", /* Localizable */ "Converts the color CMYK numbers to HSL in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "K", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmykToHslKSCommand()),

            new CommandInfo("colorcmyktohsv", /* Localizable */ "Converts the color CMYK numbers to HSV.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "K", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmykToHsvCommand()),

            new CommandInfo("colorcmyktohsvks", /* Localizable */ "Converts the color CMYK numbers to HSV in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "K", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmykToHsvKSCommand()),

            new CommandInfo("colorcmyktocmy", /* Localizable */ "Converts the color CMYK numbers to CMY.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "K", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmykToCmyCommand()),

            new CommandInfo("colorcmyktocmyks", /* Localizable */ "Converts the color CMYK numbers to CMY in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "K", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmykToCmyKSCommand()),

            new CommandInfo("colorcmyktoyiq", /* Localizable */ "Converts the color CMYK numbers to YIQ.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "K", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmykToYiqCommand()),

            new CommandInfo("colorcmyktoyiqks", /* Localizable */ "Converts the color CMYK numbers to YIQ in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "K", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmykToYiqKSCommand()),

            new CommandInfo("colorcmyktoyuv", /* Localizable */ "Converts the color CMYK numbers to YUV.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "K", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmykToYuvCommand()),

            new CommandInfo("colorcmyktoyuvks", /* Localizable */ "Converts the color CMYK numbers to YUV in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "K", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmykToYuvKSCommand()),

            new CommandInfo("colorcmytohex", /* Localizable */ "Converts the color CMY numbers to hex.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmyToHexCommand()),

            new CommandInfo("colorcmytocmyk", /* Localizable */ "Converts the color CMY numbers to CMYK.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmyToCmykCommand()),

            new CommandInfo("colorcmytocmykks", /* Localizable */ "Converts the color CMY numbers to CMYK in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmyToCmykKSCommand()),

            new CommandInfo("colorcmytohsl", /* Localizable */ "Converts the color CMY numbers to HSL.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmyToHslCommand()),

            new CommandInfo("colorcmytohslks", /* Localizable */ "Converts the color CMY numbers to HSL in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmyToHslKSCommand()),

            new CommandInfo("colorcmytohsv", /* Localizable */ "Converts the color CMY numbers to HSV.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmyToHsvCommand()),

            new CommandInfo("colorcmytohsvks", /* Localizable */ "Converts the color CMY numbers to HSV in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmyToHsvKSCommand()),

            new CommandInfo("colorcmytorgb", /* Localizable */ "Converts the color CMY numbers to RGB.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmyToRgbCommand()),

            new CommandInfo("colorcmytorgbks", /* Localizable */ "Converts the color CMY numbers to RGB in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmyToRgbKSCommand()),

            new CommandInfo("colorcmytoryb", /* Localizable */ "Converts the color CMY numbers to RYB.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmyToRybCommand()),

            new CommandInfo("colorcmytorybks", /* Localizable */ "Converts the color CMY numbers to RYB in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmyToRybKSCommand()),

            new CommandInfo("colorcmytoyiq", /* Localizable */ "Converts the color CMY numbers to YIQ.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmyToYiqCommand()),

            new CommandInfo("colorcmytoyiqks", /* Localizable */ "Converts the color CMY numbers to YIQ in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmyToYiqKSCommand()),

            new CommandInfo("colorcmytoyuv", /* Localizable */ "Converts the color CMY numbers to YUV.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmyToYuvCommand()),

            new CommandInfo("colorcmytoyuvks", /* Localizable */ "Converts the color CMY numbers to YUV in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "C", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "M", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorCmyToYuvKSCommand()),

            new CommandInfo("coloryiqtohex", /* Localizable */ "Converts the color YIQ numbers to hex.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYiqToHexCommand()),

            new CommandInfo("coloryiqtocmyk", /* Localizable */ "Converts the color YIQ numbers to CMYK.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYiqToCmykCommand()),

            new CommandInfo("coloryiqtocmykks", /* Localizable */ "Converts the color YIQ numbers to CMYK in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYiqToCmykKSCommand()),

            new CommandInfo("coloryiqtocmy", /* Localizable */ "Converts the color YIQ numbers to CMY.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYiqToCmyCommand()),

            new CommandInfo("coloryiqtocmyks", /* Localizable */ "Converts the color YIQ numbers to CMY in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYiqToCmyKSCommand()),

            new CommandInfo("coloryiqtohsl", /* Localizable */ "Converts the color YIQ numbers to HSL.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYiqToHslCommand()),

            new CommandInfo("coloryiqtohslks", /* Localizable */ "Converts the color YIQ numbers to HSL in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYiqToHslKSCommand()),

            new CommandInfo("coloryiqtohsv", /* Localizable */ "Converts the color YIQ numbers to HSV.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYiqToHsvCommand()),

            new CommandInfo("coloryiqtohsvks", /* Localizable */ "Converts the color YIQ numbers to HSV in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYiqToHsvKSCommand()),

            new CommandInfo("coloryiqtorgb", /* Localizable */ "Converts the color YIQ numbers to RGB.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYiqToRgbCommand()),

            new CommandInfo("coloryiqtorgbks", /* Localizable */ "Converts the color YIQ numbers to RGB in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYiqToRgbKSCommand()),

            new CommandInfo("coloryiqtoryb", /* Localizable */ "Converts the color YIQ numbers to RYB.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYiqToRybCommand()),

            new CommandInfo("coloryiqtorybks", /* Localizable */ "Converts the color YIQ numbers to RYB in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYiqToRybKSCommand()),

            new CommandInfo("coloryiqtoyuv", /* Localizable */ "Converts the color YIQ numbers to YUV.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYiqToYuvCommand()),

            new CommandInfo("coloryiqtoyuvks", /* Localizable */ "Converts the color YIQ numbers to YUV in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYiqToYuvKSCommand()),

            new CommandInfo("coloryuvtohex", /* Localizable */ "Converts the color YUV numbers to hex.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYuvToHexCommand()),

            new CommandInfo("coloryuvtocmyk", /* Localizable */ "Converts the color YUV numbers to CMYK.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYuvToCmykCommand()),

            new CommandInfo("coloryuvtocmykks", /* Localizable */ "Converts the color YUV numbers to CMYK in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYuvToCmykKSCommand()),

            new CommandInfo("coloryuvtocmy", /* Localizable */ "Converts the color YUV numbers to CMY.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYuvToCmyCommand()),

            new CommandInfo("coloryuvtocmyks", /* Localizable */ "Converts the color YUV numbers to CMY in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYuvToCmyKSCommand()),

            new CommandInfo("coloryuvtohsl", /* Localizable */ "Converts the color YUV numbers to HSL.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYuvToHslCommand()),

            new CommandInfo("coloryuvtohslks", /* Localizable */ "Converts the color YUV numbers to HSL in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYuvToHslKSCommand()),

            new CommandInfo("coloryuvtohsv", /* Localizable */ "Converts the color YUV numbers to HSV.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYuvToHsvCommand()),

            new CommandInfo("coloryuvtohsvks", /* Localizable */ "Converts the color YUV numbers to HSV in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYuvToHsvKSCommand()),

            new CommandInfo("coloryuvtorgb", /* Localizable */ "Converts the color YUV numbers to RGB.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYuvToRgbCommand()),

            new CommandInfo("coloryuvtorgbks", /* Localizable */ "Converts the color YUV numbers to RGB in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYuvToRgbKSCommand()),

            new CommandInfo("coloryuvtoryb", /* Localizable */ "Converts the color YUV numbers to RYB.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYuvToRybCommand()),

            new CommandInfo("coloryuvtorybks", /* Localizable */ "Converts the color YUV numbers to RYB in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYuvToRybKSCommand()),

            new CommandInfo("coloryuvtoyiq", /* Localizable */ "Converts the color YUV numbers to YUV.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYuvToYiqCommand()),

            new CommandInfo("coloryuvtoyiqks", /* Localizable */ "Converts the color YUV numbers to YIQ in KS format.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Y", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "I", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                        new CommandArgumentPart(true, "Q", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true
                        }),
                    ], true)
                ], new ColorYuvToYiqKSCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasColorConvert);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);

        void IAddon.FinalizeAddon()
        { }
    }
}
