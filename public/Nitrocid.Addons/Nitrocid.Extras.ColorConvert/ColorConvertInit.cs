
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

            { "colorrgbtohex",
                new CommandInfo("colorrgbtohex", ShellType.Shell, /* Localizable */ "Converts the color RGB numbers to hex.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "R"),
                            new CommandArgumentPart(true, "G"),
                            new CommandArgumentPart(true, "B"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorRgbToHexCommand())
            },
        };

        string IAddon.AddonName => "Extras - Dictionary";

        AddonType IAddon.AddonType => AddonType.Optional;

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());

        void IAddon.FinalizeAddon()
        { }
    }
}
