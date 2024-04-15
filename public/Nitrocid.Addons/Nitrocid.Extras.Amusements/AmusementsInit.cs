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
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Extras.Amusements.Commands;
using Nitrocid.Extras.Amusements.Screensavers;
using Nitrocid.Extras.Amusements.Settings;
using Nitrocid.Extras.Amusements.Splashes;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Misc.Splash;
using Nitrocid.Modifications;
using System.Linq;
using Textify.Data;

namespace Nitrocid.Extras.Amusements
{
    internal class AmusementsInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("backrace", /* Localizable */ "Do you back the wrong horse?",
                [
                    new CommandArgumentInfo()
                ], new BackRaceCommand()),

            new CommandInfo("hangman", /* Localizable */ "Starts the Hangman game",
                [
                    new CommandArgumentInfo(new[] {
                        new SwitchInfo("hardcore", /* Localizable */ "One wrong letter and you're hung!", new SwitchOptions()
                        {
                            ConflictsWith = ["practice"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("practice", /* Localizable */ "Test your Hangman skills by throwing a random letter.", new SwitchOptions()
                        {
                            ConflictsWith = ["hardcore"],
                            AcceptsValues = false
                        }),
                    })
                ], new HangmanCommand()),

            new CommandInfo("meteor", /* Localizable */ "You are a spaceship and the meteors are coming to destroy you. Can you save it?",
                [
                    new CommandArgumentInfo()
                ], new MeteorCommand()),

            new CommandInfo("meteordodge", /* Localizable */ "You are a spaceship and the meteors are coming to destroy you. Can you dodge them?",
                [
                    new CommandArgumentInfo()
                ], new MeteorDodgeCommand()),

            new CommandInfo("quote", /* Localizable */ "Gets a random quote",
                [
                    new CommandArgumentInfo()
                ], new QuoteCommand()),

            new CommandInfo("roulette", /* Localizable */ "Russian Roulette",
                [
                    new CommandArgumentInfo()
                ], new RouletteCommand()),

            new CommandInfo("shipduet", /* Localizable */ "Two spaceships are on a fight with each other. One shot and the spaceship will blow. This is a local two-player game.",
                [
                    new CommandArgumentInfo()
                ], new ShipDuetCommand()),

            new CommandInfo("snaker", /* Localizable */ "The snake game!",
                [
                    new CommandArgumentInfo()
                ], new SnakerCommand()),

            new CommandInfo("solver", /* Localizable */ "See if you can solve mathematical equations on time",
                [
                    new CommandArgumentInfo()
                ], new SolverCommand()),

            new CommandInfo("speedpress", /* Localizable */ "See if you can press a key on time",
                [
                    new CommandArgumentInfo(new[] {
                        new SwitchInfo("e", /* Localizable */ "Starts the game in easy difficulty", new SwitchOptions()
                        {
                            ConflictsWith = ["m", "h", "v", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("m", /* Localizable */ "Starts the game in medium difficulty", new SwitchOptions()
                        {
                            ConflictsWith = ["v", "h", "e", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("h", /* Localizable */ "Starts the game in hard difficulty", new SwitchOptions()
                        {
                            ConflictsWith = ["m", "v", "e", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("v", /* Localizable */ "Starts the game in very hard difficulty", new SwitchOptions()
                        {
                            ConflictsWith = ["m", "h", "e", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("c", /* Localizable */ "Starts the game in custom difficulty. Please note that the custom timeout in milliseconds should be written as argument.", new SwitchOptions()
                        {
                            ConflictsWith = ["m", "h", "v", "e"],
                            ArgumentsRequired = true
                        })
                    })
                ], new SpeedPressCommand()),

            new CommandInfo("wordle", /* Localizable */ "The Wordle game simulator",
                [
                    new CommandArgumentInfo(new[] {
                        new SwitchInfo("orig", /* Localizable */ "Play the Wordle game originally", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    })
                ], new WordleCommand()),

            new CommandInfo("2018", /* Localizable */ "Commemorates the 5-year anniversary of the kernel release",
                [
                    new CommandArgumentInfo()
                ], new AnniversaryCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasAmusements);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        internal static AmusementsSaversConfig SaversConfig =>
            (AmusementsSaversConfig)Config.baseConfigurations[nameof(AmusementsSaversConfig)];

        internal static AmusementsSplashesConfig SplashConfig =>
            (AmusementsSplashesConfig)Config.baseConfigurations[nameof(AmusementsSplashesConfig)];

        internal static AmusementsConfig AmusementsConfig =>
            (AmusementsConfig)Config.baseConfigurations[nameof(AmusementsConfig)];

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        private readonly SplashInfo quote = new("Quote", new SplashQuote(), false);

        void IAddon.FinalizeAddon()
        { }

        void IAddon.StartAddon()
        {
            // Initialize everything
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
            ScreensaverManager.AddonSavers.Add("meteor", new MeteorDisplay());
            ScreensaverManager.AddonSavers.Add("meteordodge", new MeteorDodgeDisplay());
            ScreensaverManager.AddonSavers.Add("quote", new QuoteDisplay());
            ScreensaverManager.AddonSavers.Add("shipduet", new ShipDuetDisplay());
            ScreensaverManager.AddonSavers.Add("snaker", new SnakerDisplay());
            SplashManager.builtinSplashes.Add(quote);

            // Initialize configuration in a way that no mod can play with them
            var saversConfig = new AmusementsSaversConfig();
            ConfigTools.RegisterBaseSetting(saversConfig);

            // Splashes...
            var splashesConfig = new AmusementsSplashesConfig();
            ConfigTools.RegisterBaseSetting(splashesConfig);

            // Main...
            var config = new AmusementsConfig();
            ConfigTools.RegisterBaseSetting(config);
        }

        void IAddon.StopAddon()
        {
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ScreensaverManager.AddonSavers.Remove("meteor");
            ScreensaverManager.AddonSavers.Remove("meteordodge");
            ScreensaverManager.AddonSavers.Remove("quote");
            ScreensaverManager.AddonSavers.Remove("shipduet");
            ScreensaverManager.AddonSavers.Remove("snaker");
            SplashManager.builtinSplashes.Remove(quote);
            ConfigTools.UnregisterBaseSetting(nameof(AmusementsSaversConfig));
            ConfigTools.UnregisterBaseSetting(nameof(AmusementsSplashesConfig));
            ConfigTools.UnregisterBaseSetting(nameof(AmusementsConfig));
        }
    }
}
