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

using BassBoom.Basolia.File;
using BassBoom.Basolia.Format;
using BassBoom.Basolia.Playback;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
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
                TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_NOSOUNDFILE", "Nitrocid.Extras.BassBoom"), KernelColorType.Error);
                return 29;
            }
            try
            {
                FileTools.OpenFile(media, path);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_OPENEDSOUNDFILE", "Nitrocid.Extras.BassBoom"), KernelColorType.Success);
            }
            catch (Exception ex)
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_CANTOPENSOUNDFILE", "Nitrocid.Extras.BassBoom") + $" {ex.Message}", KernelColorType.Error);
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
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_TIMEOUT", "Nitrocid.Extras.BassBoom"), KernelColorType.Error);
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
                        LanguageTools.GetLocalized("NKS_BASSBOOM_UNKNOWNARTIST", "Nitrocid.Extras.BassBoom");
                    string musicGenre =
                        !string.IsNullOrEmpty(managedV2.Genre) ? managedV2.Genre :
                        managedV1.GenreIndex >= 0 ? $"{managedV1.Genre} [{managedV1.GenreIndex}]" :
                        LanguageTools.GetLocalized("NKS_BASSBOOM_UNKNOWNGENRE", "Nitrocid.Extras.BassBoom");
                    var totalSpan = AudioInfoTools.GetDurationSpanFromSamples(media, total);
                    string duration = totalSpan.ToString();

                    // Write the entries
                    TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_NAME", "Nitrocid.Extras.BassBoom"), musicName);
                    TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_ARTIST", "Nitrocid.Extras.BassBoom"), musicArtist);
                    TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_GENRE", "Nitrocid.Extras.BassBoom"), musicGenre);
                    TextWriters.WriteListEntry(LanguageTools.GetLocalized("NKS_BASSBOOM_DURATION", "Nitrocid.Extras.BassBoom"), duration);

                    // Wait until the song stops or the user bails
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_STOPPLAYING", "Nitrocid.Extras.BassBoom"), KernelColorType.Tip);
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
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_BASSBOOM_CANTPLAY", "Nitrocid.Extras.BassBoom") + $" {ex.Message}", KernelColorType.Error);
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
