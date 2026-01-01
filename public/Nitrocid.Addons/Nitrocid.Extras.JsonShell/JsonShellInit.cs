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

using Nitrocid.Extras.JsonShell.Commands;
using Nitrocid.Extras.JsonShell.Json;
using Nitrocid.Extras.JsonShell.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.JsonShell
{
    internal class JsonShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("jsondiff", /* Localizable */ "Shows the difference between two JSON files",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file1", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "First JSON file"
                        }),
                        new CommandArgumentPart(true, "file2", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Second JSON file"
                        }),
                    ])
                ], new JsonDiffCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("jsonbeautify", /* Localizable */ "Beautifies the JSON file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "jsonfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to JSON file"
                        }),
                        new CommandArgumentPart(true, "output", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to output JSON file"
                        }),
                    ], true)
                ], new JsonBeautifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("jsonminify", /* Localizable */ "Minifies the JSON file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "jsonfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to JSON file"
                        }),
                        new CommandArgumentPart(true, "output", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to output JSON file"
                        }),
                    ], true)
                ], new JsonMinifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasJsonShell);

        internal static JsonConfig JsonConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(JsonConfig)) ? (JsonConfig)Config.baseConfigurations[nameof(JsonConfig)] : Config.GetFallbackKernelConfig<JsonConfig>();

        void IAddon.FinalizeAddon()
        {
            var config = new JsonConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterShell("JsonShell", new JsonShellInfo());
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterShell("JsonShell");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(JsonConfig));
        }
    }
}
