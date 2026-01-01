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

using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;
using Nitrocid.Extras.NameGen.Commands;
using Nitrocid.Extras.NameGen.Screensavers;
using Nitrocid.Extras.NameGen.Settings;
using Nitrocid.Kernel.Configuration;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Shells;
using Nitrocid.Misc.Screensaver;
using System.Linq;

namespace Nitrocid.Extras.NameGen
{
    internal class NameGenInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
                new CommandInfo("findfirstname", /* Localizable */ "First name finder",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(false, "term", new CommandArgumentPartOptions()
                            {
                                ArgumentDescription = /* Localizable */ "Search term"
                            }),
                            new CommandArgumentPart(false, "nameprefix", new CommandArgumentPartOptions()
                            {
                                ArgumentDescription = /* Localizable */ "Name prefix to search"
                            }),
                            new CommandArgumentPart(false, "namesuffix", new CommandArgumentPartOptions()
                            {
                                ArgumentDescription = /* Localizable */ "Name suffix to search"
                            }),
                        ],
                        [
                            new SwitchInfo("t", /* Localizable */ "Generate nametags (umlauts are currently not supported)", new SwitchOptions()
                            {
                                AcceptsValues = false
                            }),
                            new SwitchInfo("male", /* Localizable */ "Generate names using the male names list", new SwitchOptions()
                            {
                                ConflictsWith = ["female", "both"],
                                AcceptsValues = false,
                            }),
                            new SwitchInfo("female", /* Localizable */ "Generate names using the female names list", new SwitchOptions()
                            {
                                ConflictsWith = ["male", "both"],
                                AcceptsValues = false,
                            }),
                            new SwitchInfo("both", /* Localizable */ "Generate names using the unified names list", new SwitchOptions()
                            {
                                ConflictsWith = ["female", "male"],
                                AcceptsValues = false,
                            }),
                        ], true)
                    ], new FindFirstNameCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

                new CommandInfo("findsurname", /* Localizable */ "Surname finder",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(false, "term", new CommandArgumentPartOptions()
                            {
                                ArgumentDescription = /* Localizable */ "Search term"
                            }),
                            new CommandArgumentPart(false, "surnameprefix", new CommandArgumentPartOptions()
                            {
                                ArgumentDescription = /* Localizable */ "Surname prefix to search"
                            }),
                            new CommandArgumentPart(false, "surnamesuffix", new CommandArgumentPartOptions()
                            {
                                ArgumentDescription = /* Localizable */ "Surname suffix to search"
                            }),
                        ], true)
                    ], new FindSurnameCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

                new CommandInfo("genname", /* Localizable */ "Name and surname generator",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "namescount", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true,
                                ArgumentDescription = /* Localizable */ "How many names to generate?"
                            }),
                            new CommandArgumentPart(false, "nameprefix", new CommandArgumentPartOptions()
                            {
                                ArgumentDescription = /* Localizable */ "Name prefix to search"
                            }),
                            new CommandArgumentPart(false, "namesuffix", new CommandArgumentPartOptions()
                            {
                                ArgumentDescription = /* Localizable */ "Name suffix to search"
                            }),
                            new CommandArgumentPart(false, "surnameprefix", new CommandArgumentPartOptions()
                            {
                                ArgumentDescription = /* Localizable */ "Surname prefix to search"
                            }),
                            new CommandArgumentPart(false, "surnamesuffix", new CommandArgumentPartOptions()
                            {
                                ArgumentDescription = /* Localizable */ "Surname suffix to search"
                            }),
                        ],
                        [
                            new SwitchInfo("t", /* Localizable */ "Generate nametags (umlauts are currently not supported)", new SwitchOptions()
                            {
                                AcceptsValues = false
                            }),
                            new SwitchInfo("male", /* Localizable */ "Generate names using the male names list", new SwitchOptions()
                            {
                                ConflictsWith = ["female", "both"],
                                AcceptsValues = false,
                            }),
                            new SwitchInfo("female", /* Localizable */ "Generate names using the female names list", new SwitchOptions()
                            {
                                ConflictsWith = ["male", "both"],
                                AcceptsValues = false,
                            }),
                            new SwitchInfo("both", /* Localizable */ "Generate names using the unified names list", new SwitchOptions()
                            {
                                ConflictsWith = ["female", "male"],
                                AcceptsValues = false,
                            }),
                        ], true)
                    ], new GenNameCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasNameGen);

        internal static NameGenSaversConfig SaversConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(NameGenSaversConfig)) ? (NameGenSaversConfig)Config.baseConfigurations[nameof(NameGenSaversConfig)] : Config.GetFallbackKernelConfig<NameGenSaversConfig>();

        void IAddon.StartAddon()
        {
            // Initialize everything
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
            ScreensaverManager.AddonSavers.Add("personlookup", new PersonLookupDisplay());

            // Then, initialize configuration in a way that no mod can play with them
            var saversConfig = new NameGenSaversConfig();
            ConfigTools.RegisterBaseSetting(saversConfig);
        }

        void IAddon.StopAddon()
        {
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ScreensaverManager.AddonSavers.Remove("personlookup");
            ConfigTools.UnregisterBaseSetting(nameof(NameGenSaversConfig));
        }

        void IAddon.FinalizeAddon()
        { }
    }
}
