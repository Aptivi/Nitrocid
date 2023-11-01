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
using KS.Shell.ShellBase.Switches;
using Nitrocid.Extras.ColorConvert.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.ColorConvert
{
    internal class ColorConvertInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "colorhextorgb",
                new CommandInfo("colorhextorgb", /* Localizable */ "Converts the hexadecimal representation of the color to RGB numbers.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToRgbCommand())
            },

            { "colorhextorgbks",
                new CommandInfo("colorhextorgbks", /* Localizable */ "Converts the hexadecimal representation of the color to RGB numbers in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToRgbKSCommand())
            },

            { "colorhextocmyk",
                new CommandInfo("colorhextocmyk", /* Localizable */ "Converts the hexadecimal representation of the color to CMYK numbers.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToCmykCommand())
            },

            { "colorhextocmykks",
                new CommandInfo("colorhextocmykks", /* Localizable */ "Converts the hexadecimal representation of the color to CMYK numbers in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToCmykKSCommand())
            },

            { "colorhextocmy",
                new CommandInfo("colorhextocmy", /* Localizable */ "Converts the hexadecimal representation of the color to CMY numbers.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToCmyCommand())
            },

            { "colorhextocmyks",
                new CommandInfo("colorhextocmyks", /* Localizable */ "Converts the hexadecimal representation of the color to CMY numbers in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToCmyKSCommand())
            },

            { "colorhextohsl",
                new CommandInfo("colorhextohsl", /* Localizable */ "Converts the hexadecimal representation of the color to HSL numbers.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToHslCommand())
            },

            { "colorhextohslks",
                new CommandInfo("colorhextohslks", /* Localizable */ "Converts the hexadecimal representation of the color to HSL numbers in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToHslKSCommand())
            },

            { "colorhextohsv",
                new CommandInfo("colorhextohsv", /* Localizable */ "Converts the hexadecimal representation of the color to HSV numbers.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToHsvCommand())
            },

            { "colorhextohsvks",
                new CommandInfo("colorhextohsvks", /* Localizable */ "Converts the hexadecimal representation of the color to HSV numbers in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToHsvKSCommand())
            },

            { "colorrgbtohex",
                new CommandInfo("colorrgbtohex", /* Localizable */ "Converts the color RGB numbers to hex.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorRgbToHexCommand())
            },

            { "colorrgbtocmyk",
                new CommandInfo("colorrgbtocmyk", /* Localizable */ "Converts the color RGB numbers to CMYK.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorRgbToCmykCommand())
            },

            { "colorrgbtocmykks",
                new CommandInfo("colorrgbtocmykks", /* Localizable */ "Converts the color RGB numbers to CMYK in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorRgbToCmykKSCommand())
            },

            { "colorrgbtocmy",
                new CommandInfo("colorrgbtocmy", /* Localizable */ "Converts the color RGB numbers to CMY.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorRgbToCmyCommand())
            },

            { "colorrgbtocmyks",
                new CommandInfo("colorrgbtocmyks", /* Localizable */ "Converts the color RGB numbers to CMY in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorRgbToCmyKSCommand())
            },

            { "colorrgbtohsl",
                new CommandInfo("colorrgbtohsl", /* Localizable */ "Converts the color RGB numbers to HSL.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorRgbToHslCommand())
            },

            { "colorrgbtohslks",
                new CommandInfo("colorrgbtohslks", /* Localizable */ "Converts the color RGB numbers to HSL in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorRgbToHslKSCommand())
            },

            { "colorrgbtohsv",
                new CommandInfo("colorrgbtohsv", /* Localizable */ "Converts the color RGB numbers to HSV.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorRgbToHsvCommand())
            },

            { "colorrgbtohsvks",
                new CommandInfo("colorrgbtohsvks", /* Localizable */ "Converts the color RGB numbers to HSV in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorRgbToHsvKSCommand())
            },

            { "colorhsltohex",
                new CommandInfo("colorhsltohex", /* Localizable */ "Converts the color HSL numbers to hex.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHslToHexCommand())
            },

            { "colorhsltocmyk",
                new CommandInfo("colorhsltocmyk", /* Localizable */ "Converts the color HSL numbers to CMYK.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHslToCmykCommand())
            },

            { "colorhsltocmykks",
                new CommandInfo("colorhsltocmykks", /* Localizable */ "Converts the color HSL numbers to CMYK in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHslToCmykKSCommand())
            },

            { "colorhsltocmy",
                new CommandInfo("colorhsltocmy", /* Localizable */ "Converts the color HSL numbers to CMY.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHslToCmyCommand())
            },

            { "colorhsltocmyks",
                new CommandInfo("colorhsltocmyks", /* Localizable */ "Converts the color HSL numbers to CMY in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHslToCmyKSCommand())
            },

            { "colorhsltohsv",
                new CommandInfo("colorhsltohsv", /* Localizable */ "Converts the color HSL numbers to HSV.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHslToHsvCommand())
            },

            { "colorhsltohsvks",
                new CommandInfo("colorhsltohsvks", /* Localizable */ "Converts the color HSL numbers to HSV in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHslToHsvKSCommand())
            },

            { "colorhsltorgb",
                new CommandInfo("colorhsltorgb", /* Localizable */ "Converts the color HSL numbers to RGB.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHslToRgbCommand())
            },

            { "colorhsltorgbks",
                new CommandInfo("colorhsltorgbks", /* Localizable */ "Converts the color HSL numbers to RGB in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHslToRgbKSCommand())
            },

            { "colorhsvtohex",
                new CommandInfo("colorhsvtohex", /* Localizable */ "Converts the color HSV numbers to hex.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHsvToHexCommand())
            },

            { "colorhsvtocmyk",
                new CommandInfo("colorhsvtocmyk", /* Localizable */ "Converts the color HSV numbers to CMYK.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHsvToCmykCommand())
            },

            { "colorhsvtocmykks",
                new CommandInfo("colorhsvtocmykks", /* Localizable */ "Converts the color HSV numbers to CMYK in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHsvToCmykKSCommand())
            },

            { "colorhsvtocmy",
                new CommandInfo("colorhsvtocmy", /* Localizable */ "Converts the color HSV numbers to CMY.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHsvToCmyCommand())
            },

            { "colorhsvtocmyks",
                new CommandInfo("colorhsvtocmyks", /* Localizable */ "Converts the color HSV numbers to CMY in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHsvToCmyKSCommand())
            },

            { "colorhsvtohsl",
                new CommandInfo("colorhsvtohsl", /* Localizable */ "Converts the color HSV numbers to HSL.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHsvToHslCommand())
            },

            { "colorhsvtohslks",
                new CommandInfo("colorhsvtohslks", /* Localizable */ "Converts the color HSV numbers to HSL in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHsvToHslKSCommand())
            },

            { "colorhsvtorgb",
                new CommandInfo("colorhsvtorgb", /* Localizable */ "Converts the color HSV numbers to RGB.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHsvToRgbCommand())
            },

            { "colorhsvtorgbks",
                new CommandInfo("colorhsvtorgbks", /* Localizable */ "Converts the color HSV numbers to RGB in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHsvToRgbKSCommand())
            },

            { "colorcmyktohex",
                new CommandInfo("colorcmyktohex", /* Localizable */ "Converts the color CMYK numbers to hex.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmykToHexCommand())
            },

            { "colorcmyktorgb",
                new CommandInfo("colorcmyktorgb", /* Localizable */ "Converts the color CMYK numbers to RGB.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmykToRgbCommand())
            },

            { "colorcmyktorgbks",
                new CommandInfo("colorcmyktorgbks", /* Localizable */ "Converts the color CMYK numbers to RGB in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmykToRgbKSCommand())
            },

            { "colorcmyktohsl",
                new CommandInfo("colorcmyktohsl", /* Localizable */ "Converts the color CMYK numbers to HSL.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmykToHslCommand())
            },

            { "colorcmyktohslks",
                new CommandInfo("colorcmyktohslks", /* Localizable */ "Converts the color CMYK numbers to HSL in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmykToHslKSCommand())
            },

            { "colorcmyktohsv",
                new CommandInfo("colorcmyktohsv", /* Localizable */ "Converts the color CMYK numbers to HSV.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmykToHsvCommand())
            },

            { "colorcmyktohsvks",
                new CommandInfo("colorcmyktohsvks", /* Localizable */ "Converts the color CMYK numbers to HSV in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmykToHsvKSCommand())
            },

            { "colorcmyktocmy",
                new CommandInfo("colorcmyktocmy", /* Localizable */ "Converts the color CMYK numbers to CMY.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmykToCmyCommand())
            },

            { "colorcmyktocmyks",
                new CommandInfo("colorcmyktocmyks", /* Localizable */ "Converts the color CMYK numbers to CMY in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmykToCmyKSCommand())
            },

            { "colorcmytohex",
                new CommandInfo("colorcmytohex", /* Localizable */ "Converts the color CMY numbers to hex.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmyToHexCommand())
            },

            { "colorcmytocmyk",
                new CommandInfo("colorcmytocmyk", /* Localizable */ "Converts the color CMY numbers to CMYK.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmyToCmykCommand())
            },

            { "colorcmytocmykks",
                new CommandInfo("colorcmytocmykks", /* Localizable */ "Converts the color CMY numbers to CMYK in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmyToCmykKSCommand())
            },

            { "colorcmytohsl",
                new CommandInfo("colorcmytohsl", /* Localizable */ "Converts the color CMY numbers to HSL.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmyToHslCommand())
            },

            { "colorcmytohslks",
                new CommandInfo("colorcmytohslks", /* Localizable */ "Converts the color CMY numbers to HSL in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmyToHslKSCommand())
            },

            { "colorcmytohsv",
                new CommandInfo("colorcmytohsv", /* Localizable */ "Converts the color CMY numbers to HSV.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmyToHsvCommand())
            },

            { "colorcmytohsvks",
                new CommandInfo("colorcmytohsvks", /* Localizable */ "Converts the color CMY numbers to HSV in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmyToHsvKSCommand())
            },

            { "colorcmytorgb",
                new CommandInfo("colorcmytorgb", /* Localizable */ "Converts the color CMY numbers to RGB.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmyToRgbCommand())
            },

            { "colorcmytorgbks",
                new CommandInfo("colorcmytorgbks", /* Localizable */ "Converts the color CMY numbers to RGB in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
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
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorCmyToRgbKSCommand())
            },
        };

        string IAddon.AddonName => "Extras - Color Converter";

        AddonType IAddon.AddonType => AddonType.Optional;

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());

        void IAddon.FinalizeAddon()
        { }
    }
}
