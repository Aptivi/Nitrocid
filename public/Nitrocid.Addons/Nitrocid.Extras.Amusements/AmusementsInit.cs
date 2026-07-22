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

using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;
using Nitrocid.Extras.Amusements.Commands;
using Nitrocid.Extras.Amusements.Screensavers;
using Nitrocid.Extras.Amusements.Settings;
using Nitrocid.Extras.Amusements.Splashes;
using Nitrocid.Kernel.Configuration;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Misc.Screensaver;
using Terminaux.Shell.Shells;
using Nitrocid.Misc.Splash;
using System.Linq;
using Nitrocid.Shell.Homepage;
using Nitrocid.Extras.Amusements.Amusements.Games;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Amusements
{
    internal class AmusementsInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("backrace", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_BACKRACE_DESC"), new BackRaceCommand()),

            new CommandInfo("hangman", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_HANGMAN_DESC"),
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("hardcore", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_HANGMAN_SWITCH_HARDCORE_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["practice"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("practice", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_HANGMAN_SWITCH_PRACTICE_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["hardcore"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("common", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_SWITCH_COMMON_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["uncommon"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("uncommon", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_SWITCH_UNCOMMON_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["common"],
                            AcceptsValues = false
                        }),
                    ])
                ], new HangmanCommand()),

            new CommandInfo("meteor", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_METEOR_DESC"), new MeteorCommand()),

            new CommandInfo("meteordodge", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_METERORDODGE_DESC"), new MeteorDodgeCommand()),

            new CommandInfo("pong", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_PONG_DESC"), new PongCommand()),

            new CommandInfo("quote", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_QUOTE_DESC"), new QuoteCommand()),

            new CommandInfo("roulette", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_ROULETTE_DESC"), new RouletteCommand()),

            new CommandInfo("shipduet", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_SHIPDUET_DESC"), new ShipDuetCommand()),

            new CommandInfo("snaker", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_SNAKER_DESC"), new SnakerCommand()),

            new CommandInfo("solver", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_SOLVER_DESC"), new SolverCommand()),

            new CommandInfo("speedpress", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_SPEEDPRESS_DESC"),
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("e", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_SPEEDPRESS_SWITCH_E_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["m", "h", "v", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("m", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_SPEEDPRESS_SWITCH_M_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["v", "h", "e", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("h", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_SPEEDPRESS_SWITCH_H_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["m", "v", "e", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("v", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_SPEEDPRESS_SWITCH_V_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["m", "h", "e", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("c", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_SPEEDPRESS_SWITCH_C_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["m", "h", "v", "e"],
                            ArgumentsRequired = true
                        })
                    ])
                ], new SpeedPressCommand()),

            new CommandInfo("wordle", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_WORDLE_DESC"),
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("orig", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_WORDLE_SWITCH_ORIG_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("common", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_SWITCH_COMMON_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["uncommon"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("uncommon", LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_SWITCH_UNCOMMON_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["common"],
                            AcceptsValues = false
                        }),
                    ])
                ], new WordleCommand()),
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
            HomepageTools.RegisterBuiltinAction(LanguageTools.GetLocalized("NKS_AMUSEMENTS_HOMEPAGE_BACKRACE"), BackRace.OpenBackRace);
            HomepageTools.RegisterBuiltinAction(LanguageTools.GetLocalized("NKS_AMUSEMENTS_HOMEPAGE_HANGMAN"), () => Hangman.InitializeHangman(HangmanDifficulty.None));
            HomepageTools.RegisterBuiltinAction(LanguageTools.GetLocalized("NKS_AMUSEMENTS_HOMEPAGE_METEORDODGE"), () => MeteorShooter.InitializeMeteor(false, true));
            HomepageTools.RegisterBuiltinAction(LanguageTools.GetLocalized("NKS_AMUSEMENTS_HOMEPAGE_METEORSHOOTER"), () => MeteorShooter.InitializeMeteor());
            HomepageTools.RegisterBuiltinAction(LanguageTools.GetLocalized("NKS_AMUSEMENTS_HOMEPAGE_PONG"), Pong.InitializePong);
            HomepageTools.RegisterBuiltinAction(LanguageTools.GetLocalized("NKS_AMUSEMENTS_HOMEPAGE_SHIPDUET"), () => ShipDuetShooter.InitializeShipDuet());
            HomepageTools.RegisterBuiltinAction(LanguageTools.GetLocalized("NKS_AMUSEMENTS_HOMEPAGE_SNAKER"), () => Snaker.InitializeSnaker(false));
            HomepageTools.RegisterBuiltinAction(LanguageTools.GetLocalized("NKS_AMUSEMENTS_HOMEPAGE_WORDLE"), () => Wordle.InitializeWordle());
            HomepageTools.RegisterBuiltinAction(LanguageTools.GetLocalized("NKS_AMUSEMENTS_HOMEPAGE_WORDLEORIG"), () => Wordle.InitializeWordle(true));
        }

        public void StartAddon()
        {
            // Initialize everything
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Amusements.Resources.Languages.Output.Localizations", typeof(AmusementsInit).Assembly));
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
            HomepageTools.UnregisterBuiltinAction("Horse Racing");
            HomepageTools.UnregisterBuiltinAction("Hangman");
            HomepageTools.UnregisterBuiltinAction("Meteor Dodge");
            HomepageTools.UnregisterBuiltinAction("Meteor Shooter");
            HomepageTools.UnregisterBuiltinAction("Pong");
            HomepageTools.UnregisterBuiltinAction("Ship Duet");
            HomepageTools.UnregisterBuiltinAction("Snaker");
            HomepageTools.UnregisterBuiltinAction("Wordle");
            HomepageTools.UnregisterBuiltinAction("Wordle (original)");
        }
    }
}
