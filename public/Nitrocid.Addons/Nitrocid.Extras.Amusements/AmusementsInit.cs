//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using Nitrocid.Extras.Amusements.Commands;
using Nitrocid.Extras.Amusements.Localized;
using Nitrocid.Extras.Amusements.Screensavers;
using Nitrocid.Extras.Amusements.Settings;
using Nitrocid.Extras.Amusements.Splashes;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Misc.Splash;
using System.Linq;
using Nitrocid.Base.Shell.Homepage;
using Nitrocid.Extras.Amusements.Amusements.Games;
using Nitrocid.Base.Languages;

namespace Nitrocid.Extras.Amusements
{
    internal class AmusementsInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("backrace", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_BACKRACE_DESC", new BackRaceCommand()),

            new CommandInfo("hangman", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_HANGMAN_DESC",
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("hardcore", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_HANGMAN_SWITCH_HARDCORE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["practice"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("practice", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_HANGMAN_SWITCH_PRACTICE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["hardcore"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("common", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SWITCH_COMMON_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["uncommon"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("uncommon", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SWITCH_UNCOMMON_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["common"],
                            AcceptsValues = false
                        }),
                    ])
                ], new HangmanCommand()),

            new CommandInfo("meteor", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_METEOR_DESC", new MeteorCommand()),

            new CommandInfo("meteordodge", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_METERORDODGE_DESC", new MeteorDodgeCommand()),

            new CommandInfo("pong", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_PONG_DESC", new PongCommand()),

            new CommandInfo("quote", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_QUOTE_DESC", new QuoteCommand()),

            new CommandInfo("roulette", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_ROULETTE_DESC", new RouletteCommand()),

            new CommandInfo("shipduet", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SHIPDUET_DESC", new ShipDuetCommand()),

            new CommandInfo("snaker", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SNAKER_DESC", new SnakerCommand()),

            new CommandInfo("solver", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SOLVER_DESC", new SolverCommand()),

            new CommandInfo("speedpress", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SPEEDPRESS_DESC",
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("e", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SPEEDPRESS_SWITCH_E_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["m", "h", "v", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("m", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SPEEDPRESS_SWITCH_M_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["v", "h", "e", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("h", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SPEEDPRESS_SWITCH_H_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["m", "v", "e", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("v", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SPEEDPRESS_SWITCH_V_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["m", "h", "e", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("c", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SPEEDPRESS_SWITCH_C_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["m", "h", "v", "e"],
                            ArgumentsRequired = true
                        })
                    ])
                ], new SpeedPressCommand()),

            new CommandInfo("wordle", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_WORDLE_DESC",
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("orig", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_WORDLE_SWITCH_ORIG_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("common", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SWITCH_COMMON_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["uncommon"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("uncommon", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_SWITCH_UNCOMMON_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["common"],
                            AcceptsValues = false
                        }),
                    ])
                ], new WordleCommand()),

            new CommandInfo("2018", /* Localizable */ "NKS_AMUSEMENTS_COMMAND_2018_DESC", new AnniversaryCommand()),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasAmusements);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasAmusements);

        internal static AmusementsSaversConfig SaversConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(AmusementsSaversConfig)) ? (AmusementsSaversConfig)Config.baseConfigurations[nameof(AmusementsSaversConfig)] : Config.GetFallbackKernelConfig<AmusementsSaversConfig>();

        internal static AmusementsSplashesConfig SplashConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(AmusementsSplashesConfig)) ? (AmusementsSplashesConfig)Config.baseConfigurations[nameof(AmusementsSplashesConfig)] : Config.GetFallbackKernelConfig<AmusementsSplashesConfig>();

        internal static AmusementsConfig AmusementsConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(AmusementsConfig)) ? (AmusementsConfig)Config.baseConfigurations[nameof(AmusementsConfig)] : Config.GetFallbackKernelConfig<AmusementsConfig>();

        private readonly SplashInfo quote = new("Quote", new SplashQuote(), false);

        public void FinalizeAddon()
        {
            // Add the amusements to the homepage
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_BACKRACE", BackRace.OpenBackRace);
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_HANGMAN", () => Hangman.InitializeHangman(HangmanDifficulty.None));
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_METEORDODGE", () => MeteorShooter.InitializeMeteor(false, true));
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_METEORSHOOTER", () => MeteorShooter.InitializeMeteor());
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_PONG", Pong.InitializePong);
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_SHIPDUET", () => ShipDuetShooter.InitializeShipDuet());
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_SNAKER", () => Snaker.InitializeSnaker(false));
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_WORDLE", () => Wordle.InitializeWordle());
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_WORDLEORIG", () => Wordle.InitializeWordle(true));
        }

        public void StartAddon()
        {
            // Initialize everything
            LanguageTools.AddCustomAction(AddonName, new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
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

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ScreensaverManager.AddonSavers.Remove("meteor");
            ScreensaverManager.AddonSavers.Remove("meteordodge");
            ScreensaverManager.AddonSavers.Remove("quote");
            ScreensaverManager.AddonSavers.Remove("shipduet");
            ScreensaverManager.AddonSavers.Remove("snaker");
            SplashManager.builtinSplashes.Remove(quote);
            ConfigTools.UnregisterBaseSetting(nameof(AmusementsSaversConfig));
            ConfigTools.UnregisterBaseSetting(nameof(AmusementsSplashesConfig));
            ConfigTools.UnregisterBaseSetting(nameof(AmusementsConfig));

            // Remove all options
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_BACKRACE");
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_HANGMAN");
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_METEORDODGE");
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_METEORSHOOTER");
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_PONG");
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_SHIPDUET");
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_SNAKER");
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_WORDLE");
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_AMUSEMENTS_HOMEPAGE_WORDLEORIG");
        }
    }
}
