
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Files.Extensions;
using KS.Kernel.Configuration;
using KS.Kernel.Extensions;
using KS.Languages;
using KS.Misc.Screensaver;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.ShellBase.Switches;
using Nitrocid.Extras.BassBoom.Commands;
using Nitrocid.Extras.BassBoom.Screensavers;
using Nitrocid.Extras.BassBoom.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#if NET7_0
using BassBoom.Basolia;
using BassBoom.Basolia.File;
using BassBoom.Basolia.Format;
#endif

namespace Nitrocid.Extras.BassBoom
{
    internal class BassBoomInit : IAddon
    {
        private readonly ExtensionHandler handler = new(
            ".mp3",
            (_) => InfoBoxColor.WriteInfoBoxKernelColor(Translate.DoTranslation("You'll be able to play music soon. Hang tight!"), KernelColorType.Warning),
            (path) =>
            {
                // Get the ID3 metadata
                FileTools.OpenFile(path);
                AudioInfoTools.GetId3Metadata(out var metadatav1, out var metadatav2);
                FileTools.CloseFile();

                // Now, populate the necessary variables
                string musicName =
                    !string.IsNullOrEmpty(metadatav2.Title) ? metadatav2.Title :
                    !string.IsNullOrEmpty(metadatav1.Title) ? metadatav1.Title :
                    Path.GetFileNameWithoutExtension(path);
                string musicArtist =
                    !string.IsNullOrEmpty(metadatav2.Artist) ? metadatav2.Artist :
                    !string.IsNullOrEmpty(metadatav1.Artist) ? metadatav1.Artist :
                    Translate.DoTranslation("Unknown Artist");
                string musicGenre =
                    !string.IsNullOrEmpty(metadatav2.Genre) ? metadatav2.Genre :
                    metadatav1.GenreIndex >= 0 ? $"{metadatav1.Genre} [{metadatav1.GenreIndex}]" :
                    Translate.DoTranslation("Unknown Genre");
                return
                    $"{Translate.DoTranslation("Song name")}: {musicName}\n" +
                    $"{Translate.DoTranslation("Song artist")}: {musicArtist}\n" +
                    $"{Translate.DoTranslation("Song genre")}: {musicGenre}";
            }
        );
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "playlyric",
                new CommandInfo("playlyric", ShellType.Shell, /* Localizable */ "Plays a lyric file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "lyric.lrc"),
                        }, Array.Empty<SwitchInfo>())
                    }, new PlayLyricCommand())
            },
            { "playsound",
                new CommandInfo("playsound", ShellType.Shell, /* Localizable */ "Plays a sound",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "musicFile"),
                        }, Array.Empty<SwitchInfo>())
                    }, new PlaySoundCommand())
            },
        };

        string IAddon.AddonName => "Extras - BassBoom";

        AddonType IAddon.AddonType => AddonType.Optional;

        internal static BassBoomSaversConfig SaversConfig =>
            (BassBoomSaversConfig)Config.baseConfigurations[nameof(BassBoomSaversConfig)];

        internal static BassBoomConfig BassBoomConfig =>
            (BassBoomConfig)Config.baseConfigurations[nameof(BassBoomConfig)];

        void IAddon.StartAddon()
        {
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());
            ScreensaverManager.Screensavers.Add("lyrics", new LyricsDisplay());

            // Then, initialize configuration in a way that no mod can play with them
            var saversConfig = new BassBoomSaversConfig();
            var bbConfig = new BassBoomConfig();
            ConfigTools.RegisterBaseSetting(saversConfig);
            ConfigTools.RegisterBaseSetting(bbConfig);

#if NET7_0
            // Additionally, register a custom extension handler that handles music playback
            InitBasolia.Init();
            ExtensionHandlerTools.extensionHandlers.Add(handler);
#endif
        }

        void IAddon.StopAddon()
        {
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());
            ScreensaverManager.Screensavers.Remove("lyrics");
            ConfigTools.UnregisterBaseSetting(nameof(BassBoomSaversConfig));
            ConfigTools.UnregisterBaseSetting(nameof(BassBoomConfig));
            ExtensionHandlerTools.extensionHandlers.Remove(handler);
        }

        void IAddon.FinalizeAddon()
        { }
    }
}
