
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

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
                new CommandInfo("colorhextorgb", ShellType.Shell, /* Localizable */ "Converts the hexadecimal representation of the color to RGB numbers.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToRgbCommand())
            },

            { "colorhextorgbks",
                new CommandInfo("colorhextorgbks", ShellType.Shell, /* Localizable */ "Converts the hexadecimal representation of the color to RGB numbers in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToRgbKSCommand())
            },

            { "colorhextocmyk",
                new CommandInfo("colorhextocmyk", ShellType.Shell, /* Localizable */ "Converts the hexadecimal representation of the color to CMYK numbers.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToCmykCommand())
            },

            { "colorhextocmykks",
                new CommandInfo("colorhextocmykks", ShellType.Shell, /* Localizable */ "Converts the hexadecimal representation of the color to CMYK numbers in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToCmykKSCommand())
            },

            { "colorhextohsl",
                new CommandInfo("colorhextohsl", ShellType.Shell, /* Localizable */ "Converts the hexadecimal representation of the color to HSL numbers.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToHslCommand())
            },

            { "colorhextohslks",
                new CommandInfo("colorhextohslks", ShellType.Shell, /* Localizable */ "Converts the hexadecimal representation of the color to HSL numbers in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToHslKSCommand())
            },

            { "colorrgbtohex",
                new CommandInfo("colorrgbtohex", ShellType.Shell, /* Localizable */ "Converts the color RGB numbers to hex.",
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
                new CommandInfo("colorrgbtocmyk", ShellType.Shell, /* Localizable */ "Converts the color RGB numbers to CMYK.",
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
                new CommandInfo("colorrgbtocmykks", ShellType.Shell, /* Localizable */ "Converts the color RGB numbers to CMYK in KS format.",
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

            { "colorrgbtohsl",
                new CommandInfo("colorrgbtohsl", ShellType.Shell, /* Localizable */ "Converts the color RGB numbers to HSL.",
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
                new CommandInfo("colorrgbtohslks", ShellType.Shell, /* Localizable */ "Converts the color RGB numbers to HSL in KS format.",
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

            { "colorhsltohex",
                new CommandInfo("colorhsltohex", ShellType.Shell, /* Localizable */ "Converts the color HSL numbers to hex.",
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
                new CommandInfo("colorhsltocmyk", ShellType.Shell, /* Localizable */ "Converts the color HSL numbers to CMYK.",
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
                new CommandInfo("colorhsltocmykks", ShellType.Shell, /* Localizable */ "Converts the color HSL numbers to CMYK in KS format.",
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

            { "colorhsltorgb",
                new CommandInfo("colorhsltorgb", ShellType.Shell, /* Localizable */ "Converts the color HSL numbers to RGB.",
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
                new CommandInfo("colorhsltorgbks", ShellType.Shell, /* Localizable */ "Converts the color HSL numbers to RGB in KS format.",
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

            { "colorcmyktohex",
                new CommandInfo("colorcmyktohex", ShellType.Shell, /* Localizable */ "Converts the color CMYK numbers to hex.",
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
                new CommandInfo("colorcmyktorgb", ShellType.Shell, /* Localizable */ "Converts the color CMYK numbers to RGB.",
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
                new CommandInfo("colorcmyktorgbks", ShellType.Shell, /* Localizable */ "Converts the color CMYK numbers to RGB in KS format.",
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
                new CommandInfo("colorcmyktohsl", ShellType.Shell, /* Localizable */ "Converts the color CMYK numbers to HSL.",
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
                new CommandInfo("colorcmyktohslks", ShellType.Shell, /* Localizable */ "Converts the color CMYK numbers to HSL in KS format.",
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
