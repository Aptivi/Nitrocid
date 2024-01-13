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

namespace Nitrocid.Extras.ColorConvert
{
    internal class ColorConvertInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "colorhextorgb",
                new CommandInfo("colorhextorgb", /* Localizable */ "Converts the hexadecimal representation of the color to RGB numbers.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToRgbCommand())
            },

            { "colorhextorgbks",
                new CommandInfo("colorhextorgbks", /* Localizable */ "Converts the hexadecimal representation of the color to RGB numbers in KS format.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToRgbKSCommand())
            },
            
            { "colorhextoryb",
                new CommandInfo("colorhextoryb", /* Localizable */ "Converts the hexadecimal representation of the color to RYB numbers.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToRybCommand())
            },

            { "colorhextorybks",
                new CommandInfo("colorhextorybks", /* Localizable */ "Converts the hexadecimal representation of the color to RYB numbers in KS format.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToRybKSCommand())
            },

            { "colorhextocmyk",
                new CommandInfo("colorhextocmyk", /* Localizable */ "Converts the hexadecimal representation of the color to CMYK numbers.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToCmykCommand())
            },

            { "colorhextocmykks",
                new CommandInfo("colorhextocmykks", /* Localizable */ "Converts the hexadecimal representation of the color to CMYK numbers in KS format.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToCmykKSCommand())
            },

            { "colorhextocmy",
                new CommandInfo("colorhextocmy", /* Localizable */ "Converts the hexadecimal representation of the color to CMY numbers.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToCmyCommand())
            },

            { "colorhextocmyks",
                new CommandInfo("colorhextocmyks", /* Localizable */ "Converts the hexadecimal representation of the color to CMY numbers in KS format.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToCmyKSCommand())
            },

            { "colorhextohsl",
                new CommandInfo("colorhextohsl", /* Localizable */ "Converts the hexadecimal representation of the color to HSL numbers.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToHslCommand())
            },

            { "colorhextohslks",
                new CommandInfo("colorhextohslks", /* Localizable */ "Converts the hexadecimal representation of the color to HSL numbers in KS format.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToHslKSCommand())
            },

            { "colorhextohsv",
                new CommandInfo("colorhextohsv", /* Localizable */ "Converts the hexadecimal representation of the color to HSV numbers.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToHsvCommand())
            },

            { "colorhextohsvks",
                new CommandInfo("colorhextohsvks", /* Localizable */ "Converts the hexadecimal representation of the color to HSV numbers in KS format.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToHsvKSCommand())
            },

            { "colorhextoyiq",
                new CommandInfo("colorhextoyiq", /* Localizable */ "Converts the hexadecimal representation of the color to YIQ numbers.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToYiqCommand())
            },

            { "colorhextoyiqks",
                new CommandInfo("colorhextoyiqks", /* Localizable */ "Converts the hexadecimal representation of the color to YIQ numbers in KS format.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToYiqKSCommand())
            },

            { "colorhextoyuv",
                new CommandInfo("colorhextoyuv", /* Localizable */ "Converts the hexadecimal representation of the color to YUV numbers.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToYuvCommand())
            },

            { "colorhextoyuvks",
                new CommandInfo("colorhextoyuvks", /* Localizable */ "Converts the hexadecimal representation of the color to YUV numbers in KS format.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "#RRGGBB"),
                        ], true)
                    ], new ColorHexToYuvKSCommand())
            },

            { "colorrgbtohex",
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
                    ], new ColorRgbToHexCommand())
            },

            { "colorrgbtocmyk",
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
                    ], new ColorRgbToCmykCommand())
            },

            { "colorrgbtocmykks",
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
                    ], new ColorRgbToCmykKSCommand())
            },

            { "colorrgbtocmy",
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
                    ], new ColorRgbToCmyCommand())
            },

            { "colorrgbtocmyks",
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
                    ], new ColorRgbToCmyKSCommand())
            },

            { "colorrgbtohsl",
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
                    ], new ColorRgbToHslCommand())
            },

            { "colorrgbtohslks",
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
                    ], new ColorRgbToHslKSCommand())
            },

            { "colorrgbtohsv",
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
                    ], new ColorRgbToHsvCommand())
            },

            { "colorrgbtohsvks",
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
                    ], new ColorRgbToHsvKSCommand())
            },

            { "colorrgbtoryb",
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
                    ], new ColorRgbToRybCommand())
            },

            { "colorrgbtorybks",
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
                    ], new ColorRgbToRybKSCommand())
            },

            { "colorrgbtoyiq",
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
                    ], new ColorRgbToYiqCommand())
            },

            { "colorrgbtoyiqks",
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
                    ], new ColorRgbToYiqKSCommand())
            },

            { "colorrgbtoyuv",
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
                    ], new ColorRgbToYuvCommand())
            },

            { "colorrgbtoyuvks",
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
                    ], new ColorRgbToYuvKSCommand())
            },

            { "colorrybtohex",
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
                    ], new ColorRybToHexCommand())
            },

            { "colorrybtocmyk",
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
                    ], new ColorRybToCmykCommand())
            },

            { "colorrybtocmykks",
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
                    ], new ColorRybToCmykKSCommand())
            },

            { "colorrybtocmy",
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
                    ], new ColorRybToCmyCommand())
            },

            { "colorrybtocmyks",
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
                    ], new ColorRybToCmyKSCommand())
            },

            { "colorrybtohsl",
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
                    ], new ColorRybToHslCommand())
            },

            { "colorrybtohslks",
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
                    ], new ColorRybToHslKSCommand())
            },

            { "colorrybtohsv",
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
                    ], new ColorRybToHsvCommand())
            },

            { "colorrybtohsvks",
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
                    ], new ColorRybToHsvKSCommand())
            },

            { "colorrybtorgb",
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
                    ], new ColorRybToRgbCommand())
            },

            { "colorrybtorgbks",
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
                    ], new ColorRybToRgbKSCommand())
            },

            { "colorrybtoyiq",
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
                    ], new ColorRybToYiqCommand())
            },

            { "colorrybtoyiqks",
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
                    ], new ColorRybToYiqKSCommand())
            },

            { "colorrybtoyuv",
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
                    ], new ColorRybToYuvCommand())
            },

            { "colorrybtoyuvks",
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
                    ], new ColorRybToYuvKSCommand())
            },

            { "colorhsltohex",
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
                    ], new ColorHslToHexCommand())
            },

            { "colorhsltocmyk",
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
                    ], new ColorHslToCmykCommand())
            },

            { "colorhsltocmykks",
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
                    ], new ColorHslToCmykKSCommand())
            },

            { "colorhsltocmy",
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
                    ], new ColorHslToCmyCommand())
            },

            { "colorhsltocmyks",
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
                    ], new ColorHslToCmyKSCommand())
            },

            { "colorhsltohsv",
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
                    ], new ColorHslToHsvCommand())
            },

            { "colorhsltohsvks",
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
                    ], new ColorHslToHsvKSCommand())
            },

            { "colorhsltorgb",
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
                    ], new ColorHslToRgbCommand())
            },

            { "colorhsltorgbks",
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
                    ], new ColorHslToRgbKSCommand())
            },

            { "colorhsltoryb",
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
                    ], new ColorHslToRybCommand())
            },

            { "colorhsltorybks",
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
                    ], new ColorHslToRybKSCommand())
            },

            { "colorhsltoyiq",
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
                    ], new ColorHslToYiqCommand())
            },

            { "colorhsltoyiqks",
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
                    ], new ColorHslToYiqKSCommand())
            },

            { "colorhsltoyuv",
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
                    ], new ColorHslToYuvCommand())
            },

            { "colorhsltoyuvks",
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
                    ], new ColorHslToYuvKSCommand())
            },

            { "colorhsvtohex",
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
                    ], new ColorHsvToHexCommand())
            },

            { "colorhsvtocmyk",
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
                    ], new ColorHsvToCmykCommand())
            },

            { "colorhsvtocmykks",
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
                    ], new ColorHsvToCmykKSCommand())
            },

            { "colorhsvtocmy",
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
                    ], new ColorHsvToCmyCommand())
            },

            { "colorhsvtocmyks",
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
                    ], new ColorHsvToCmyKSCommand())
            },

            { "colorhsvtohsl",
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
                    ], new ColorHsvToHslCommand())
            },

            { "colorhsvtohslks",
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
                    ], new ColorHsvToHslKSCommand())
            },

            { "colorhsvtorgb",
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
                    ], new ColorHsvToRgbCommand())
            },

            { "colorhsvtorgbks",
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
                    ], new ColorHsvToRgbKSCommand())
            },

            { "colorhsvtoryb",
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
                    ], new ColorHsvToRybCommand())
            },

            { "colorhsvtorybks",
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
                    ], new ColorHsvToRybKSCommand())
            },

            { "colorhsvtoyiq",
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
                    ], new ColorHsvToYiqCommand())
            },

            { "colorhsvtoyiqks",
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
                    ], new ColorHsvToYiqKSCommand())
            },

            { "colorhsvtoyuv",
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
                    ], new ColorHsvToYuvCommand())
            },

            { "colorhsvtoyuvks",
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
                    ], new ColorHsvToYuvKSCommand())
            },

            { "colorcmyktohex",
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
                    ], new ColorCmykToHexCommand())
            },

            { "colorcmyktorgb",
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
                    ], new ColorCmykToRgbCommand())
            },

            { "colorcmyktorgbks",
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
                    ], new ColorCmykToRgbKSCommand())
            },

            { "colorcmyktoryb",
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
                    ], new ColorCmykToRybCommand())
            },

            { "colorcmyktorybks",
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
                    ], new ColorCmykToRybKSCommand())
            },

            { "colorcmyktohsl",
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
                    ], new ColorCmykToHslCommand())
            },

            { "colorcmyktohslks",
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
                    ], new ColorCmykToHslKSCommand())
            },

            { "colorcmyktohsv",
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
                    ], new ColorCmykToHsvCommand())
            },

            { "colorcmyktohsvks",
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
                    ], new ColorCmykToHsvKSCommand())
            },

            { "colorcmyktocmy",
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
                    ], new ColorCmykToCmyCommand())
            },

            { "colorcmyktocmyks",
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
                    ], new ColorCmykToCmyKSCommand())
            },

            { "colorcmyktoyiq",
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
                    ], new ColorCmykToYiqCommand())
            },

            { "colorcmyktoyiqks",
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
                    ], new ColorCmykToYiqKSCommand())
            },

            { "colorcmyktoyuv",
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
                    ], new ColorCmykToYuvCommand())
            },

            { "colorcmyktoyuvks",
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
                    ], new ColorCmykToYuvKSCommand())
            },

            { "colorcmytohex",
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
                    ], new ColorCmyToHexCommand())
            },

            { "colorcmytocmyk",
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
                    ], new ColorCmyToCmykCommand())
            },

            { "colorcmytocmykks",
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
                    ], new ColorCmyToCmykKSCommand())
            },

            { "colorcmytohsl",
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
                    ], new ColorCmyToHslCommand())
            },

            { "colorcmytohslks",
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
                    ], new ColorCmyToHslKSCommand())
            },

            { "colorcmytohsv",
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
                    ], new ColorCmyToHsvCommand())
            },

            { "colorcmytohsvks",
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
                    ], new ColorCmyToHsvKSCommand())
            },

            { "colorcmytorgb",
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
                    ], new ColorCmyToRgbCommand())
            },

            { "colorcmytorgbks",
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
                    ], new ColorCmyToRgbKSCommand())
            },

            { "colorcmytoryb",
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
                    ], new ColorCmyToRybCommand())
            },

            { "colorcmytorybks",
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
                    ], new ColorCmyToRybKSCommand())
            },

            { "colorcmytoyiq",
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
                    ], new ColorCmyToYiqCommand())
            },

            { "colorcmytoyiqks",
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
                    ], new ColorCmyToYiqKSCommand())
            },

            { "colorcmytoyuv",
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
                    ], new ColorCmyToYuvCommand())
            },

            { "colorcmytoyuvks",
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
                    ], new ColorCmyToYuvKSCommand())
            },

            { "coloryiqtohex",
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
                    ], new ColorYiqToHexCommand())
            },

            { "coloryiqtocmyk",
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
                    ], new ColorYiqToCmykCommand())
            },

            { "coloryiqtocmykks",
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
                    ], new ColorYiqToCmykKSCommand())
            },

            { "coloryiqtocmy",
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
                    ], new ColorYiqToCmyCommand())
            },

            { "coloryiqtocmyks",
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
                    ], new ColorYiqToCmyKSCommand())
            },

            { "coloryiqtohsl",
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
                    ], new ColorYiqToHslCommand())
            },

            { "coloryiqtohslks",
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
                    ], new ColorYiqToHslKSCommand())
            },

            { "coloryiqtohsv",
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
                    ], new ColorYiqToHsvCommand())
            },

            { "coloryiqtohsvks",
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
                    ], new ColorYiqToHsvKSCommand())
            },

            { "coloryiqtorgb",
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
                    ], new ColorYiqToRgbCommand())
            },

            { "coloryiqtorgbks",
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
                    ], new ColorYiqToRgbKSCommand())
            },

            { "coloryiqtoryb",
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
                    ], new ColorYiqToRybCommand())
            },

            { "coloryiqtorybks",
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
                    ], new ColorYiqToRybKSCommand())
            },

            { "coloryiqtoyuv",
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
                    ], new ColorYiqToYuvCommand())
            },

            { "coloryiqtoyuvks",
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
                    ], new ColorYiqToYuvKSCommand())
            },

            { "coloryuvtohex",
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
                    ], new ColorYuvToHexCommand())
            },

            { "coloryuvtocmyk",
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
                    ], new ColorYuvToCmykCommand())
            },

            { "coloryuvtocmykks",
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
                    ], new ColorYuvToCmykKSCommand())
            },

            { "coloryuvtocmy",
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
                    ], new ColorYuvToCmyCommand())
            },

            { "coloryuvtocmyks",
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
                    ], new ColorYuvToCmyKSCommand())
            },

            { "coloryuvtohsl",
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
                    ], new ColorYuvToHslCommand())
            },

            { "coloryuvtohslks",
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
                    ], new ColorYuvToHslKSCommand())
            },

            { "coloryuvtohsv",
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
                    ], new ColorYuvToHsvCommand())
            },

            { "coloryuvtohsvks",
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
                    ], new ColorYuvToHsvKSCommand())
            },

            { "coloryuvtorgb",
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
                    ], new ColorYuvToRgbCommand())
            },

            { "coloryuvtorgbks",
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
                    ], new ColorYuvToRgbKSCommand())
            },

            { "coloryuvtoryb",
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
                    ], new ColorYuvToRybCommand())
            },

            { "coloryuvtorybks",
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
                    ], new ColorYuvToRybKSCommand())
            },

            { "coloryuvtoyiq",
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
                    ], new ColorYuvToYiqCommand())
            },

            { "coloryuvtoyiqks",
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
                    ], new ColorYuvToYiqKSCommand())
            },
        };

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasColorConvert);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands.Values]);

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Keys]);

        void IAddon.FinalizeAddon()
        { }
    }
}
