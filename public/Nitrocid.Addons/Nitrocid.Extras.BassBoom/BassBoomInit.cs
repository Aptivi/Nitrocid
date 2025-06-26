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

// Parts of the code were taken from BassBoom. License notes below:

//   BassBoom  Copyright (C) 2023  Aptivi
// 
//   This file is part of BassBoom
// 
//   BassBoom is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
// 
//   BassBoom is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
// 
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Extras.BassBoom.Commands;
using Nitrocid.Extras.BassBoom.Screensavers;
using Nitrocid.Extras.BassBoom.Settings;
using System;
using System.Collections.Generic;
using BassBoom.Basolia;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Misc.Screensaver;
using System.Linq;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.Extras.BassBoom
{
    internal class BassBoomInit : IAddon
    {
        internal static Version? mpgVer;
        internal static Version? outVer;
        internal static Color white = new(ConsoleColors.White);

        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("lyriclines", /* Localizable */ "Gets all lyric lines from the lyric file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "lyric.lrc", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to the lyric file"
                        }),
                    ])
                ], new LyricLinesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("playlyric", /* Localizable */ "Plays a lyric file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "lyric.lrc", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to the lyric file"
                        }),
                    ])
                ], new PlayLyricCommand()),

            new CommandInfo("playsound", /* Localizable */ "Plays a sound",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "musicFile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to an MP3 file"
                        }),
                    ])
                ], new PlaySoundCommand()),

            new CommandInfo("netfminfo", /* Localizable */ "Gets information about your online radio station",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "hostname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Host name that hosts the MPEG radio station"
                        }),
                        new CommandArgumentPart(true, "port", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Port to the MPEG radio station"
                        }),
                    ])
                ], new NetFmInfoCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasBassBoom);

        internal static BassBoomSaversConfig SaversConfig =>
            (BassBoomSaversConfig)Config.baseConfigurations[nameof(BassBoomSaversConfig)];

        internal static BassBoomConfig BassBoomConfig =>
            (BassBoomConfig)Config.baseConfigurations[nameof(BassBoomConfig)];

        void IAddon.StartAddon()
        {
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
            ScreensaverManager.AddonSavers.Add("lyrics", new LyricsDisplay());

            // Then, initialize configuration in a way that no mod can play with them
            var saversConfig = new BassBoomSaversConfig();
            var bbConfig = new BassBoomConfig();
            ConfigTools.RegisterBaseSetting(saversConfig);
            ConfigTools.RegisterBaseSetting(bbConfig);

            // Additionally, register a custom extension handler that handles music playback
            if (!InitBasolia.BasoliaInitialized)
                InitBasolia.Init();

            // Initialize versions
            mpgVer = InitBasolia.MpgLibVersion;
            outVer = InitBasolia.OutLibVersion;
        }

        void IAddon.StopAddon()
        {
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ScreensaverManager.AddonSavers.Remove("lyrics");
            ConfigTools.UnregisterBaseSetting(nameof(BassBoomSaversConfig));
            ConfigTools.UnregisterBaseSetting(nameof(BassBoomConfig));
        }

        void IAddon.FinalizeAddon()
        { }
    }
}
