//
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

using KS.Kernel.Extensions;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Nitrocid.Extras.ColorConvert.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

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
        };

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasColorConvert);

        AddonType IAddon.AddonType => AddonType.Optional;

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
