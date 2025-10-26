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
using Nitrocid.Extras.BassBoom.Commands;
using Nitrocid.Extras.BassBoom.Screensavers;
using Nitrocid.Extras.BassBoom.Settings;
using System;
using System.Collections.Generic;
using BassBoom.Basolia;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Base.Misc.Screensaver;
using System.Linq;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Nitrocid.Core.Languages;

namespace Nitrocid.Extras.BassBoom
{
    internal class BassBoomInit : IAddon
    {
        internal static Version? mpgVer;
        internal static Version? outVer;
        internal static Color white = new(ConsoleColors.White);

        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("lyriclines", /* Localizable */ "NKS_BASSBOOM_COMMAND_LYRICLINES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "lyric.lrc", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_BASSBOOM_COMMAND_ARGUMENT_LYRICLRC_DESC"
                        }),
                    ])
                ], new LyricLinesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("playlyric", /* Localizable */ "NKS_BASSBOOM_COMMAND_PLAYLYRIC_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "lyric.lrc", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_BASSBOOM_COMMAND_ARGUMENT_LYRICLRC_DESC"
                        }),
                    ])
                ], new PlayLyricCommand()),

            new CommandInfo("playsound", /* Localizable */ "NKS_BASSBOOM_COMMAND_PLAYSOUND_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "musicFile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_BASSBOOM_COMMAND_ARGUMENT_MUSICFILE_DESC"
                        }),
                    ])
                ], new PlaySoundCommand()),

            new CommandInfo("netfminfo", /* Localizable */ "NKS_BASSBOOM_COMMAND_NETFMINFO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "hostname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_BASSBOOM_COMMAND_NETFMINFO_ARGUMENT_HOSTNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "port", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_BASSBOOM_COMMAND_NETFMINFO_ARGUMENT_PORT_DESC"
                        }),
                    ])
                ], new NetFmInfoCommand()),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasBassBoom);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasBassBoom);

        internal static BassBoomSaversConfig SaversConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(BassBoomSaversConfig)) ? (BassBoomSaversConfig)Config.baseConfigurations[nameof(BassBoomSaversConfig)] : Config.GetFallbackKernelConfig<BassBoomSaversConfig>();

        internal static BassBoomConfig BassBoomConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(BassBoomConfig)) ? (BassBoomConfig)Config.baseConfigurations[nameof(BassBoomConfig)] : Config.GetFallbackKernelConfig<BassBoomConfig>();

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.BassBoom.Resources.Languages.Output.Localizations", typeof(BassBoomInit).Assembly));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
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

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ScreensaverManager.AddonSavers.Remove("lyrics");
            ConfigTools.UnregisterBaseSetting(nameof(BassBoomSaversConfig));
            ConfigTools.UnregisterBaseSetting(nameof(BassBoomConfig));
        }

        public void FinalizeAddon()
        { }
    }
}
