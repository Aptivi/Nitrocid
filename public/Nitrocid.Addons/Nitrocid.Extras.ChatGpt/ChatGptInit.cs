﻿//
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
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Nitrocid.Extras.ChatGpt.Gpt;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.ChatGpt
{
    internal class ChatGptInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "chatgpt",
                new CommandInfo("chatgpt", /* Localizable */ "Loads the ChatGPT shell with your API key associated with your account",
                    new[]
                    {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "apikey")
                        }),
                    }, new ChatGptCommandExec())
            },
        };

        string IAddon.AddonName => "Extras - ChatGPT Unofficial Client";

        AddonType IAddon.AddonType => AddonType.Optional;

        void IAddon.FinalizeAddon()
        {
            ShellManager.reservedShells.Add("ChatGptShell");
            ShellManager.RegisterShell("ChatGptShell", new ChatGptShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.availableShells.Remove("ChatGptShell");
            PromptPresetManager.CurrentPresets.Remove("ChatGptShell");
            ShellManager.reservedShells.Remove("ChatGptShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());
        }
    }
}