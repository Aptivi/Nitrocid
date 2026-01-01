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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nitrocid.Extras.JsonShell.Commands;
using Nitrocid.Extras.JsonShell.Json;
using Nitrocid.Extras.JsonShell.Settings;
using Nitrocid.Extras.JsonShell.Tools;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Modifications;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

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
            ConfigTools.IsCustomSettingBuiltin(nameof(JsonConfig)) ? (JsonConfig)Config.baseConfigurations[nameof(JsonConfig)] : new JsonConfig();

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => new(new Dictionary<string, Delegate>()
        {
            { nameof(JsonTools.OpenJsonFile), new Func<string, bool>(JsonTools.OpenJsonFile) },
            { nameof(JsonTools.CloseJsonFile), new Func<bool>(JsonTools.CloseJsonFile) },
            { nameof(JsonTools.SaveFile), new Func<bool, bool>(JsonTools.SaveFile) },
            { nameof(JsonTools.SaveFile) + "2", new Func<bool, Formatting, bool>(JsonTools.SaveFile) },
            { nameof(JsonTools.WasJsonEdited), new Func<bool>(JsonTools.WasJsonEdited) },
            { nameof(JsonTools.DetermineRootType), new Func<JTokenType>(JsonTools.DetermineRootType) },
            { nameof(JsonTools.DetermineType), new Func<string, JTokenType>(JsonTools.DetermineType) },
            { nameof(JsonTools.GetToken), new Func<string, JToken>(JsonTools.GetToken) },
            { nameof(JsonTools.GetTokenSafe), new Func<string, JToken?>(JsonTools.GetTokenSafe) },
            { nameof(JsonTools.GetTokenSafe) + "2", new Func<string, string, JToken?>(JsonTools.GetTokenSafe) },
            { nameof(JsonTools.Add), new Action<string, string, string, string>(JsonTools.Add) },
            { nameof(JsonTools.Set), new Action<string, string, string, string>(JsonTools.Set) },
            { nameof(JsonTools.Remove), new Action<string>(JsonTools.Remove) },
            { nameof(JsonTools.SerializeToString), new Func<string, string>(JsonTools.SerializeToString) },
        });

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
        {
            var config = new JsonConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterAddonShell("JsonShell", new JsonShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterAddonShell("JsonShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(JsonConfig));
        }
    }
}
