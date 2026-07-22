//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
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
using Nitrocid.Languages;

namespace Nitrocid.Extras.JsonShell
{
    internal class JsonShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("jsondiff", LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_JSONDIFF_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file1", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_JSONDIFF_ARGUMENT_FILE1_DESC")
                        }),
                        new CommandArgumentPart(true, "file2", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_JSONDIFF_ARGUMENT_FILE2_DESC")
                        }),
                    ])
                ], new JsonDiffCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("jsonbeautify", LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_JSONBEAUTIFY_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "jsonfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_ARGUMENT_JSONFILE_DESC")
                        }),
                        new CommandArgumentPart(true, "output", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_ARGUMENT_OUTPUT_DESC")
                        }),
                    ], true)
                ], new JsonBeautifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("jsonminify", LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_JSONMINIFY_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "jsonfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_ARGUMENT_JSONFILE_DESC")
                        }),
                        new CommandArgumentPart(true, "output", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_ARGUMENT_OUTPUT_DESC")
                        }),
                    ], true)
                ], new JsonMinifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasJsonShell);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasJsonShell);

        internal static JsonConfig JsonConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(JsonConfig)) ? (JsonConfig)Config.baseConfigurations[nameof(JsonConfig)] : Config.GetFallbackKernelConfig<JsonConfig>();

        public void FinalizeAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.JsonShell.Resources.Languages.Output.Localizations", typeof(JsonShellInit).Assembly));
            var config = new JsonConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterShell("JsonShell", new JsonShellInfo());
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StartAddon()
        { }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            ShellManager.UnregisterShell("JsonShell");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(JsonConfig));
        }
    }
}
