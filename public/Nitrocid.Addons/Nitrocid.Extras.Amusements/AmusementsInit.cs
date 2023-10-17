
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

using KS.Kernel.Configuration;
using KS.Kernel.Extensions;
using KS.Misc.Screensaver;
using KS.Misc.Splash;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.ShellBase.Switches;
using Nitrocid.Extras.Amusements.Commands;
using Nitrocid.Extras.Amusements.Screensavers;
using Nitrocid.Extras.Amusements.Settings;
using Nitrocid.Extras.Amusements.Splashes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.Amusements
{
    internal class AmusementsInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "backrace",
                new CommandInfo("backrace", ShellType.Shell, /* Localizable */ "Do you back the wrong horse?",
                    new[] {
                        new CommandArgumentInfo()
                    }, new BackRaceCommand())
            },

            { "hangman",
                new CommandInfo("hangman", ShellType.Shell, /* Localizable */ "Starts the Hangman game",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), new[] {
                            new SwitchInfo("hardcore", /* Localizable */ "One wrong letter and you're hung!", new SwitchOptions()
                            {
                                ConflictsWith = new string[] { "practice" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("practice", /* Localizable */ "Test your Hangman skills by throwing a random letter.", new SwitchOptions()
                            {
                                ConflictsWith = new string[] { "hardcore" },
                                AcceptsValues = false
                            }),
                        })
                    }, new HangmanCommand())
            },

            { "meteor",
                new CommandInfo("meteor", ShellType.Shell, /* Localizable */ "You are a spaceship and the meteors are coming to destroy you. Can you save it?",
                    new[] {
                        new CommandArgumentInfo()
                    }, new MeteorCommand())
            },

            { "quote",
                new CommandInfo("quote", ShellType.Shell, /* Localizable */ "Gets a random quote",
                    new[] {
                        new CommandArgumentInfo()
                    }, new QuoteCommand())
            },

            { "roulette",
                new CommandInfo("roulette", ShellType.Shell, /* Localizable */ "Russian Roulette",
                    new[] {
                        new CommandArgumentInfo()
                    }, new RouletteCommand())
            },

            { "shipduet",
                new CommandInfo("shipduet", ShellType.Shell, /* Localizable */ "Two spaceships are on a fight with each other. One shot and the spaceship will blow. This is a local two-player game.",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ShipDuetCommand())
            },

            { "snaker",
                new CommandInfo("snaker", ShellType.Shell, /* Localizable */ "The snake game!",
                    new[] {
                        new CommandArgumentInfo()
                    }, new SnakerCommand())
            },

            { "solver",
                new CommandInfo("solver", ShellType.Shell, /* Localizable */ "See if you can solve mathematical equations on time",
                    new[] {
                        new CommandArgumentInfo()
                    }, new SolverCommand())
            },

            { "speedpress",
                new CommandInfo("speedpress", ShellType.Shell, /* Localizable */ "See if you can press a key on time",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), new[] {
                            new SwitchInfo("e", /* Localizable */ "Starts the game in easy difficulty", new SwitchOptions()
                            {
                                ConflictsWith = new string[] { "m", "h", "v", "c" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("m", /* Localizable */ "Starts the game in medium difficulty", new SwitchOptions()
                            {
                                ConflictsWith = new string[] { "v", "h", "e", "c" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("h", /* Localizable */ "Starts the game in hard difficulty", new SwitchOptions()
                            {
                                ConflictsWith = new string[] { "m", "v", "e", "c" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("v", /* Localizable */ "Starts the game in very hard difficulty", new SwitchOptions()
                            {
                                ConflictsWith = new string[] { "m", "h", "e", "c" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("c", /* Localizable */ "Starts the game in custom difficulty. Please note that the custom timeout in milliseconds should be written as argument.", new SwitchOptions()
                            {
                                ConflictsWith = new string[] { "m", "h", "v", "e" },
                                ArgumentsRequired = true
                            })
                        })
                    }, new SpeedPressCommand())
            },

            { "wordle",
                new CommandInfo("wordle", ShellType.Shell, /* Localizable */ "The Wordle game simulator",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), new[] {
                            new SwitchInfo("orig", /* Localizable */ "Play the Wordle game originally", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        })
                    }, new WordleCommand())
            },

            // Hidden
            { "2015",
                new CommandInfo("2015", ShellType.Shell, /* Localizable */ "Starts the joke program, HDD Uncleaner 2015.",
                    new[] {
                        new CommandArgumentInfo()
                    }, new HddUncleanerCommand(), CommandFlags.Hidden)
            },

            { "2018",
                new CommandInfo("2018", ShellType.Shell, /* Localizable */ "Commemorates the 5-year anniversary of the kernel release",
                    new[] {
                        new CommandArgumentInfo()
                    }, new AnniversaryCommand(), CommandFlags.Hidden)
            },
        };

        string IAddon.AddonName => "Extras - Amusements";

        AddonType IAddon.AddonType => AddonType.Optional;

        internal static AmusementsSaversConfig SaversConfig =>
            (AmusementsSaversConfig)Config.baseConfigurations[nameof(AmusementsSaversConfig)];

        internal static AmusementsSplashesConfig SplashConfig =>
            (AmusementsSplashesConfig)Config.baseConfigurations[nameof(AmusementsSplashesConfig)];

        internal static AmusementsConfig AmusementsConfig =>
            (AmusementsConfig)Config.baseConfigurations[nameof(AmusementsConfig)];

        void IAddon.FinalizeAddon()
        { }

        void IAddon.StartAddon()
        {
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());
            ScreensaverManager.Screensavers.Add("meteor", new MeteorDisplay());
            ScreensaverManager.Screensavers.Add("quote", new QuoteDisplay());
            ScreensaverManager.Screensavers.Add("shipduet", new ShipDuetDisplay());
            ScreensaverManager.Screensavers.Add("snaker", new SnakerDisplay());
            SplashManager.InstalledSplashes.Add("Quote", new SplashInfo("Quote", new SplashQuote(), false));

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
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());
            ScreensaverManager.Screensavers.Remove("meteor");
            ScreensaverManager.Screensavers.Remove("quote");
            ScreensaverManager.Screensavers.Remove("shipduet");
            ScreensaverManager.Screensavers.Remove("snaker");
            SplashManager.InstalledSplashes.Remove("Quote");
            ConfigTools.UnregisterBaseSetting(nameof(AmusementsSaversConfig));
            ConfigTools.UnregisterBaseSetting(nameof(AmusementsSplashesConfig));
            ConfigTools.UnregisterBaseSetting(nameof(AmusementsConfig));
        }
    }
}
