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

using Nitrocid.Extras.Amusements.Commands;
using Nitrocid.Extras.Amusements.Screensavers;
using Nitrocid.Extras.Amusements.Settings;
using Nitrocid.Extras.Amusements.Splashes;
using Nitrocid.Kernel.Configuration;
using Terminaux.Shell.Commands;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Misc.Splash;
using System.Linq;
using Nitrocid.Shell.Homepage;
using Nitrocid.Extras.Amusements.Amusements.Games;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Amusements
{
    internal class AmusementsInit : IAddon
    {
        private readonly BaseCommand[] addonCommands =
        [
            new BackRaceCommand(),
            new HangmanCommand(),
            new MeteorCommand(),
            new MeteorDodgeCommand(),
            new PongCommand(),
            new QuoteCommand(),
            new RouletteCommand(),
            new ShipDuetCommand(),
            new SnakerCommand(),
            new SolverCommand(),
            new SpeedPressCommand(),
            new WordleCommand(),
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
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Amusements.Resources.Languages.Output.Localizations", typeof(AmusementsInit).Assembly));
            CommandManager.RegisterCustomCommands("Shell", addonCommands);
            ScreensaverManager.AddonSavers.Add("meteor", new MeteorDisplay());
            ScreensaverManager.AddonSavers.Add("meteordodge", new MeteorDodgeDisplay());
            ScreensaverManager.AddonSavers.Add("quote", new QuoteDisplay());
            ScreensaverManager.AddonSavers.Add("shipduet", new ShipDuetDisplay());
            ScreensaverManager.AddonSavers.Add("snaker", new SnakerDisplay());
            lock (SplashManager.builtinSplashes)
            {
                SplashManager.builtinSplashes.Add(quote);
            }

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
            CommandManager.UnregisterCustomCommands("Shell", addonCommands);
            ScreensaverManager.AddonSavers.Remove("meteor");
            ScreensaverManager.AddonSavers.Remove("meteordodge");
            ScreensaverManager.AddonSavers.Remove("quote");
            ScreensaverManager.AddonSavers.Remove("shipduet");
            ScreensaverManager.AddonSavers.Remove("snaker");
            lock (SplashManager.builtinSplashes)
            {
                SplashManager.builtinSplashes.Remove(quote);
            }
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
