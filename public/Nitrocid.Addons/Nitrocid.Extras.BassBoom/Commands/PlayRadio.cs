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

using System;
using System.Threading;
using BassBoom.Basolia.Media;
using BassBoom.Basolia.Media.Playback;
using Nitrocid.Base.Languages;
using Terminaux.Inputs;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.BassBoom.Commands
{
    /// <summary>
    /// Plays a radio station
    /// </summary>
    /// <remarks>
    /// This command allows you to play a radio station.
    /// </remarks>
    class PlayRadioCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "playradio";

        // TODO: NKS_BASSBOOM_COMMAND_PLAYRADIO_DESC -> Plays a radio station
        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_BASSBOOM_COMMAND_PLAYRADIO_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "radioUrl", new CommandArgumentPartOptions()
                    {
                        // TODO: NKS_BASSBOOM_COMMAND_ARGUMENT_RADIOURL_DESC -> Path to a radio station
                        ArgumentDescription = /* Localizable */ "NKS_BASSBOOM_COMMAND_ARGUMENT_RADIOURL_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string path = parameters.ArgumentsList[0];
            var media = new BasoliaMedia();
            try
            {
                // TODO: NKS_BASSBOOM_OPENEDRADIOFILE -> Opened radio station successfully.
                media.OpenUrl(path);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_OPENEDRADIOFILE"), ThemeColorType.Success);
            }
            catch (Exception ex)
            {
                // TODO: NKS_BASSBOOM_CANTOPENRADIOFILE -> Can't open radio station.
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_CANTOPENRADIOFILE") + $" {ex.Message}", ThemeColorType.Error);
                return ex.HResult;
            }
            if (media.IsOpened())
            {
                try
                {
                    // Play now!
                    media.PlayAsync();
                    if (!SpinWait.SpinUntil(() => media.GetState() == PlaybackState.Playing, 15000))
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_TIMEOUT"), ThemeColorType.Error);
                        return 30;
                    }

                    // Wait until the song stops or the user bails
                    string nowPlaying = "";
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_STOPPLAYING"), ThemeColorType.Tip);
                    while (media.GetState() == PlaybackState.Playing)
                    {
                        // Get currently playing song
                        string newNowPlaying = media.GetRadioNowPlaying();
                        if (newNowPlaying != nowPlaying)
                        {
                            // We have new song from a radio station
                            // TODO: NKS_BASSBOOM_NOWPLAYING -> Now playing
                            nowPlaying = newNowPlaying;
                            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_NOWPLAYING"), nowPlaying);
                        }

                        // Get input
                        InputEventInfo eventInfo = Input.ReadPointerOrKeyNoBlock();
                        if (eventInfo.EventType == InputEventType.Keyboard)
                        {
                            if (eventInfo.ConsoleKeyInfo is ConsoleKeyInfo cki && cki.Key == ConsoleKey.Q)
                                media.Stop();
                        }
                    }
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_CANTPLAY") + $" {ex.Message}", ThemeColorType.Error);
                    return ex.HResult;
                }
                finally
                {
                    media.CloseFile();
                }
            }
            return 0;
        }

    }
}
