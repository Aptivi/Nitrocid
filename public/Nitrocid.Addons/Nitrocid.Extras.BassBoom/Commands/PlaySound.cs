//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using BassBoom.Basolia.File;
using BassBoom.Basolia.Format;
using BassBoom.Basolia.Playback;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Files;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using System;
using System.IO;
using System.Threading;
using Terminaux.Base;
using Terminaux.Inputs;
using BassBoom.Basolia;

namespace Nitrocid.Extras.BassBoom.Commands
{
    /// <summary>
    /// Plays a sound file
    /// </summary>
    /// <remarks>
    /// This command allows you to play a sound file.
    /// </remarks>
    class PlaySoundCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string path = parameters.ArgumentsList[0];
            var media = new BasoliaMedia();
            path = FilesystemTools.NeutralizePath(path);
            if (!FilesystemTools.FileExists(path))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_NOSOUNDFILE"), ThemeColorType.Error);
                return 29;
            }
            try
            {
                FileTools.OpenFile(media, path);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_OPENEDSOUNDFILE"), ThemeColorType.Success);
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_CANTOPENSOUNDFILE") + $" {ex.Message}", ThemeColorType.Error);
                return ex.HResult;
            }
            if (FileTools.IsOpened(media))
            {
                try
                {
                    // They must be done before playing
                    int total = AudioInfoTools.GetDuration(media, true);
                    AudioInfoTools.GetId3Metadata(media, out var managedV1, out var managedV2);

                    // Play now!
                    PlaybackTools.PlayAsync(media);
                    if (!SpinWait.SpinUntil(() => PlaybackTools.GetState(media) == PlaybackState.Playing, 15000))
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_TIMEOUT"), ThemeColorType.Error);
                        return 30;
                    }

                    // Print song info
                    string musicName =
                        !string.IsNullOrEmpty(managedV2.Title) ? managedV2.Title :
                        !string.IsNullOrEmpty(managedV1.Title) ? managedV1.Title :
                        Path.GetFileNameWithoutExtension(path);
                    string musicArtist =
                        !string.IsNullOrEmpty(managedV2.Artist) ? managedV2.Artist :
                        !string.IsNullOrEmpty(managedV1.Artist) ? managedV1.Artist :
                        LanguageTools.GetLocalized("NKS_BASSBOOM_UNKNOWNARTIST");
                    string musicGenre =
                        !string.IsNullOrEmpty(managedV2.Genre) ? managedV2.Genre :
                        managedV1.GenreIndex >= 0 ? $"{managedV1.Genre} [{managedV1.GenreIndex}]" :
                        LanguageTools.GetLocalized("NKS_BASSBOOM_UNKNOWNGENRE");
                    var totalSpan = AudioInfoTools.GetDurationSpanFromSamples(media, total);
                    string duration = totalSpan.ToString();

                    // Write the entries
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_NAME"), musicName);
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_ARTIST"), musicArtist);
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_GENRE"), musicGenre);
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_DURATION"), duration);

                    // Wait until the song stops or the user bails
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_STOPPLAYING"), ThemeColorType.Tip);
                    while (PlaybackTools.GetState(media) == PlaybackState.Playing)
                    {
                        if (ConsoleWrapper.KeyAvailable)
                        {
                            var key = Input.ReadKey();
                            if (key.Key == ConsoleKey.Q)
                                PlaybackTools.Stop(media);
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
                    FileTools.CloseFile(media);
                }
            }
            return 0;
        }

    }
}
