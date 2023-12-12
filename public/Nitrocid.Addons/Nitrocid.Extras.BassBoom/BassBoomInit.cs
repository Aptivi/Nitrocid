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

using KS.Kernel.Configuration;
using KS.Kernel.Extensions;
using KS.Misc.Screensaver;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Nitrocid.Extras.BassBoom.Commands;
using Nitrocid.Extras.BassBoom.Screensavers;
using Nitrocid.Extras.BassBoom.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using BassBoom.Basolia;
using KS.Files.Extensions;
using Nitrocid.Extras.BassBoom.Player;
using KS.Files.Paths;

namespace Nitrocid.Extras.BassBoom
{
    internal class BassBoomInit : IAddon
    {
        private readonly ExtensionHandler[] handlers = [
            new(".mp3", "Mp3BassBoom", PlayerHandler.Handle, PlayerHandler.InfoHandle),
            new(".mp2", "Mp3BassBoom", PlayerHandler.Handle, PlayerHandler.InfoHandle),
            new(".mpg", "Mp3BassBoom", PlayerHandler.Handle, PlayerHandler.InfoHandle),
            new(".mpa", "Mp3BassBoom", PlayerHandler.Handle, PlayerHandler.InfoHandle),
            new(".mpga", "Mp3BassBoom", PlayerHandler.Handle, PlayerHandler.InfoHandle),
        ];

        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "lyriclines",
                new CommandInfo("lyriclines", /* Localizable */ "Gets all lyric lines from the lyric file",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "lyric.lrc"),
                        })
                    ], new LyricLinesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "musicplayer",
                new CommandInfo("musicplayer", /* Localizable */ "Opens an interactive music player",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "musicFile"),
                        })
                    ], new MusicPlayerCommand())
            },

            { "playlyric",
                new CommandInfo("playlyric", /* Localizable */ "Plays a lyric file",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "lyric.lrc"),
                        })
                    ], new PlayLyricCommand())
            },

            { "playsound",
                new CommandInfo("playsound", /* Localizable */ "Plays a sound",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "musicFile"),
                        })
                    ], new PlaySoundCommand())
            },
        };

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasBassBoom);

        AddonType IAddon.AddonType => AddonType.Optional;

        internal static BassBoomSaversConfig SaversConfig =>
            (BassBoomSaversConfig)Config.baseConfigurations[nameof(BassBoomSaversConfig)];

        internal static BassBoomConfig BassBoomConfig =>
            (BassBoomConfig)Config.baseConfigurations[nameof(BassBoomConfig)];

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        void IAddon.StartAddon()
        {
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands.Values]);
            ScreensaverManager.AddonSavers.Add("lyrics", new LyricsDisplay());

            // Then, initialize configuration in a way that no mod can play with them
            var saversConfig = new BassBoomSaversConfig();
            var bbConfig = new BassBoomConfig();
            ConfigTools.RegisterBaseSetting(saversConfig);
            ConfigTools.RegisterBaseSetting(bbConfig);

            var privateReflection = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField;
            var startInfoType = typeof(InitBasolia);
            var envVarsField = startInfoType.GetField("_basoliaInited", privateReflection);
            var inited = (bool)envVarsField.GetValue(null);

            // Additionally, register a custom extension handler that handles music playback
            if (!inited)
                InitBasolia.Init(PathsManagement.AddonsPath + "/Extras.BassBoom");
            ExtensionHandlerTools.extensionHandlers.AddRange(handlers);
        }

        void IAddon.StopAddon()
        {
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Keys]);
            ScreensaverManager.AddonSavers.Remove("lyrics");
            ConfigTools.UnregisterBaseSetting(nameof(BassBoomSaversConfig));
            ConfigTools.UnregisterBaseSetting(nameof(BassBoomConfig));
            foreach (var handler in handlers)
                ExtensionHandlerTools.extensionHandlers.Remove(handler);
        }

        void IAddon.FinalizeAddon()
        { }
    }
}
