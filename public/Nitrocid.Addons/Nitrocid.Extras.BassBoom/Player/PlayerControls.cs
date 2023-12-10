//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

// This file was taken from BassBoom. License notes below:

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

using BassBoom.Basolia.File;
using BassBoom.Basolia.Format;
using BassBoom.Basolia.Format.Cache;
using BassBoom.Basolia.Lyrics;
using BassBoom.Basolia.Playback;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs.Styles.Infobox;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Languages;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Nitrocid.Extras.BassBoom.Player
{
    internal static class PlayerControls
    {
        internal static double seekRate = 3.0d;

        internal static void RaiseVolume()
        {
            PlayerTui.volume += 0.05;
            if (PlayerTui.volume > 1)
                PlayerTui.volume = 1;
            PlaybackTools.SetVolume(PlayerTui.volume);
        }

        internal static void LowerVolume()
        {
            PlayerTui.volume -= 0.05;
            if (PlayerTui.volume < 0)
                PlayerTui.volume = 0;
            PlaybackTools.SetVolume(PlayerTui.volume);
        }

        internal static void SeekForward()
        {
            // In case we have no songs in the playlist...
            if (PlayerTui.musicFiles.Count == 0)
                return;

            PlayerTui.position += (int)(PlayerTui.formatInfo.rate * seekRate);
            if (PlayerTui.position > PlayerTui.total)
                PlayerTui.position = PlayerTui.total;
            PlaybackPositioningTools.SeekToFrame(PlayerTui.position);
        }

        internal static void SeekBackward()
        {
            // In case we have no songs in the playlist...
            if (PlayerTui.musicFiles.Count == 0)
                return;

            PlayerTui.position -= (int)(PlayerTui.formatInfo.rate * seekRate);
            if (PlayerTui.position < 0)
                PlayerTui.position = 0;
            PlaybackPositioningTools.SeekToFrame(PlayerTui.position);
        }

        internal static void SeekBeginning()
        {
            // In case we have no songs in the playlist...
            if (PlayerTui.musicFiles.Count == 0)
                return;

            PlaybackPositioningTools.SeekToTheBeginning();
            PlayerTui.position = 0;
        }

        internal static void Play()
        {
            // In case we have no songs in the playlist...
            if (PlayerTui.musicFiles.Count == 0)
                return;

            if (PlaybackTools.State == PlaybackState.Stopped)
                // There could be a chance that the music has fully stopped without any user interaction.
                PlaybackPositioningTools.SeekToTheBeginning();
            PlayerTui.advance = true;
            PlayerTui.rerender = true;
            PlayerTui.playerThread.Start();
            SpinWait.SpinUntil(() => PlaybackTools.Playing || PlayerTui.failedToPlay);
            PlayerTui.failedToPlay = false;
        }

        internal static void Pause()
        {
            PlayerTui.advance = false;
            PlayerTui.paused = true;
            PlaybackTools.Pause();
        }

        internal static void Stop(bool resetCurrentSong = true)
        {
            PlayerTui.advance = false;
            PlayerTui.paused = false;
            if (resetCurrentSong)
                PlayerTui.currentSong = 1;
            PlaybackTools.Stop();
        }

        internal static void NextSong()
        {
            // In case we have no songs in the playlist...
            if (PlayerTui.musicFiles.Count == 0)
                return;

            PlayerTui.currentSong++;
            if (PlayerTui.currentSong > PlayerTui.musicFiles.Count)
                PlayerTui.currentSong = 1;
        }

        internal static void PreviousSong()
        {
            // In case we have no songs in the playlist...
            if (PlayerTui.musicFiles.Count == 0)
                return;

            PlayerTui.currentSong--;
            if (PlayerTui.currentSong <= 0)
                PlayerTui.currentSong = PlayerTui.musicFiles.Count;
        }

        internal static void PromptForAddSong()
        {
            string path = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a path to the music file"));
            if (File.Exists(path))
            {
                int currentPos = PlayerTui.position;
                PlayerTui.populate = true;
                PopulateMusicFileInfo(path);
                PlayerTui.populate = true;
                PopulateMusicFileInfo(PlayerTui.musicFiles[PlayerTui.currentSong - 1]);
                PlaybackPositioningTools.SeekToFrame(currentPos);
            }
            else
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("File \"{0}\" doesn't exist."), path);
            PlayerTui.rerender = true;
        }

        internal static void PromptForAddDirectory()
        {
            string path = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a path to the music library directory"));
            if (Directory.Exists(path))
            {
                int currentPos = PlayerTui.position;
                var musicFiles = Directory.GetFiles(path, "*.mp3");
                if (musicFiles.Length > 0)
                {
                    foreach (string musicFile in musicFiles)
                    {
                        PlayerTui.populate = true;
                        PopulateMusicFileInfo(musicFile);
                    }
                    PlayerTui.populate = true;
                    PopulateMusicFileInfo(PlayerTui.musicFiles[PlayerTui.currentSong - 1]);
                    PlaybackPositioningTools.SeekToFrame(currentPos);
                }
            }
            else
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Music library directory is not found."));
            PlayerTui.rerender = true;
        }

        internal static void Exit()
        {
            PlayerTui.exiting = true;
            PlayerTui.advance = false;
        }

        internal static bool TryOpenMusicFile(string musicPath)
        {
            try
            {
                if (FileTools.IsOpened)
                    FileTools.CloseFile();
                FileTools.OpenFile(musicPath);
                FileTools.CloseFile();
                return true;
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Can't open {0}: {1}"), true, musicPath, ex.Message);
            }
            return false;
        }

        internal static void PopulateMusicFileInfo(string musicPath)
        {
            // Try to open the file after loading the library
            if (PlaybackTools.Playing || !PlayerTui.populate)
                return;
            PlayerTui.populate = false;
            if (!TryOpenMusicFile(musicPath))
                return;
            FileTools.OpenFile(musicPath);
            if (PlayerTui.cachedInfos.Any((csi) => csi.MusicPath == musicPath))
            {
                var instance = PlayerTui.cachedInfos.Single((csi) => csi.MusicPath == musicPath);
                PlayerTui.total = instance.Duration;
                PlayerTui.formatInfo = instance.FormatInfo;
                PlayerTui.totalSpan = AudioInfoTools.GetDurationSpanFromSamples(PlayerTui.total, PlayerTui.formatInfo);
                PlayerTui.frameInfo = instance.FrameInfo;
                PlayerTui.managedV1 = instance.MetadataV1;
                PlayerTui.managedV2 = instance.MetadataV2;
                PlayerTui.lyricInstance = instance.LyricInstance;
                if (!PlayerTui.musicFiles.Contains(musicPath))
                    PlayerTui.musicFiles.Add(musicPath);
            }
            else
            {
                PlayerTui.rerender = true;
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Loading BassBoom to open {0}..."), false, musicPath);
                PlayerTui.total = AudioInfoTools.GetDuration(true);
                PlayerTui.totalSpan = AudioInfoTools.GetDurationSpanFromSamples(PlayerTui.total);
                PlayerTui.formatInfo = FormatTools.GetFormatInfo();
                PlayerTui.frameInfo = AudioInfoTools.GetFrameInfo();
                AudioInfoTools.GetId3Metadata(out PlayerTui.managedV1, out PlayerTui.managedV2);

                // Try to open the lyrics
                OpenLyrics(musicPath);
                var instance = new CachedSongInfo(musicPath, PlayerTui.managedV1, PlayerTui.managedV2, PlayerTui.total, PlayerTui.formatInfo, PlayerTui.frameInfo, PlayerTui.lyricInstance);
                PlayerTui.cachedInfos.Add(instance);
            }
            TextWriterWhereColor.WriteWhere(new string(' ', ConsoleWrapper.WindowWidth), 0, 1);
            if (!PlayerTui.musicFiles.Contains(musicPath))
                PlayerTui.musicFiles.Add(musicPath);
        }

        internal static string RenderSongName(string musicPath)
        {
            // Render the song name
            var (musicName, musicArtist, _) = GetMusicNameArtistGenre(musicPath);

            // Print the music name
            return CenteredTextColor.RenderCentered(1, Translate.DoTranslation("Now playing") + ": {0} - {1}", KernelColorTools.GetColor(KernelColorType.NeutralText), KernelColorTools.GetColor(KernelColorType.Background), musicArtist, musicName);
        }

        internal static (string musicName, string musicArtist, string musicGenre) GetMusicNameArtistGenre(string musicPath)
        {
            var metadatav2 = PlayerTui.managedV2;
            var metadatav1 = PlayerTui.managedV1;
            string musicName =
                !string.IsNullOrEmpty(metadatav2.Title) ? metadatav2.Title :
                !string.IsNullOrEmpty(metadatav1.Title) ? metadatav1.Title :
                Path.GetFileNameWithoutExtension(musicPath);
            string musicArtist =
                !string.IsNullOrEmpty(metadatav2.Artist) ? metadatav2.Artist :
                !string.IsNullOrEmpty(metadatav1.Artist) ? metadatav1.Artist :
                Translate.DoTranslation("Unknown Artist");
            string musicGenre =
                !string.IsNullOrEmpty(metadatav2.Genre) ? metadatav2.Genre :
                metadatav1.GenreIndex >= 0 ? $"{metadatav1.Genre} [{metadatav1.GenreIndex}]" :
                Translate.DoTranslation("Unknown Genre");
            return (musicName, musicArtist, musicGenre);
        }

        internal static (string musicName, string musicArtist, string musicGenre) GetMusicNameArtistGenre(int cachedInfoIdx)
        {
            var cachedInfo = PlayerTui.cachedInfos[cachedInfoIdx];
            var metadatav2 = cachedInfo.MetadataV2;
            var metadatav1 = cachedInfo.MetadataV1;
            var path = cachedInfo.MusicPath;
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
            return (musicName, musicArtist, musicGenre);
        }

        internal static void OpenLyrics(string musicPath)
        {
            string lyricsPath = Path.GetDirectoryName(musicPath) + "/" + Path.GetFileNameWithoutExtension(musicPath) + ".lrc";
            try
            {
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Trying to open lyrics file") + " {0}...", false, lyricsPath);
                if (File.Exists(lyricsPath))
                    PlayerTui.lyricInstance = LyricReader.GetLyrics(lyricsPath);
                else
                    PlayerTui.lyricInstance = null;
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Can't open lyrics file") + " {0}... {1}", lyricsPath, ex.Message);
            }
        }

        internal static void RemoveCurrentSong()
        {
            // In case we have no songs in the playlist...
            if (PlayerTui.musicFiles.Count == 0)
                return;

            PlayerTui.cachedInfos.RemoveAt(PlayerTui.currentSong - 1);
            PlayerTui.musicFiles.RemoveAt(PlayerTui.currentSong - 1);
            if (PlayerTui.musicFiles.Count > 0)
            {
                PlayerTui.currentSong--;
                if (PlayerTui.currentSong == 0)
                    PlayerTui.currentSong = 1;
                PlayerTui.populate = true;
                PopulateMusicFileInfo(PlayerTui.musicFiles[PlayerTui.currentSong - 1]);
            }
            PlayerTui.rerender = true;
        }

        internal static void RemoveAllSongs()
        {
            // In case we have no songs in the playlist...
            if (PlayerTui.musicFiles.Count == 0)
                return;

            for (int i = PlayerTui.musicFiles.Count; i > 0; i--)
                RemoveCurrentSong();
        }

        internal static void PromptSeek()
        {
            // In case we have no songs in the playlist...
            if (PlayerTui.musicFiles.Count == 0)
                return;

            // Prompt the user to set the current position to the specified time
            string time = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Write the target position in this format") + ": HH:MM:SS");
            if (TimeSpan.TryParse(time, out TimeSpan duration))
            {
                PlayerTui.position = (int)(PlayerTui.cachedInfos[PlayerTui.currentSong - 1].FormatInfo.rate * duration.TotalSeconds);
                if (PlayerTui.position > PlayerTui.total)
                    PlayerTui.position = PlayerTui.total;
                PlaybackPositioningTools.SeekToFrame(PlayerTui.position);
            }
            PlayerTui.rerender = true;
        }

        internal static void ShowHelp()
        {
            InfoBoxColor.WriteInfoBox(
                $$"""
                -- {{Translate.DoTranslation("Available keystrokes")}} --

                [SPACE]             {{Translate.DoTranslation("Play/Pause")}}
                [ESC]               {{Translate.DoTranslation("Stop")}}
                [Q]                 {{Translate.DoTranslation("Exit")}}
                [UP/DOWN]           {{Translate.DoTranslation("Volume control")}}
                [<-/->]             {{Translate.DoTranslation("Seek control")}}
                [CTRL] + [<-/->]    {{Translate.DoTranslation("Seek duration control")}}
                [I]                 {{Translate.DoTranslation("Song info")}}
                [A]                 {{Translate.DoTranslation("Add a music file")}}
                [S]                 {{Translate.DoTranslation("Add a music directory to the playlist")}}
                [B]                 {{Translate.DoTranslation("Previous song")}}
                [N]                 {{Translate.DoTranslation("Next song")}}
                [R]                 {{Translate.DoTranslation("Remove current song")}}
                [CTRL] + [R]        {{Translate.DoTranslation("Remove all songs")}}
                [S]                 {{Translate.DoTranslation("Selectively seek")}}
                """
            );
            PlayerTui.rerender = true;
        }

        internal static void ShowSongInfo()
        {
            var textsBuilder = new StringBuilder();
            foreach (var text in PlayerTui.managedV2.Texts)
                textsBuilder.AppendLine($"T - {text.Item1}: {text.Item2}");
            foreach (var text in PlayerTui.managedV2.Extras)
                textsBuilder.AppendLine($"E - {text.Item1}: {text.Item2}");
            InfoBoxColor.WriteInfoBox(
                $$"""
                -- {{Translate.DoTranslation("Song info")}} --

                {{Translate.DoTranslation("Artist")}}: {{(!string.IsNullOrEmpty(PlayerTui.managedV2.Artist) ? PlayerTui.managedV2.Artist : !string.IsNullOrEmpty(PlayerTui.managedV1.Artist) ? PlayerTui.managedV1.Artist : Translate.DoTranslation("Unknown"))}}
                {{Translate.DoTranslation("Title")}}: {{(!string.IsNullOrEmpty(PlayerTui.managedV2.Title) ? PlayerTui.managedV2.Title : !string.IsNullOrEmpty(PlayerTui.managedV1.Title) ? PlayerTui.managedV1.Title : "")}}
                {{Translate.DoTranslation("Album")}}: {{(!string.IsNullOrEmpty(PlayerTui.managedV2.Album) ? PlayerTui.managedV2.Album : !string.IsNullOrEmpty(PlayerTui.managedV1.Album) ? PlayerTui.managedV1.Album : "")}}
                {{Translate.DoTranslation("Genre")}}: {{(!string.IsNullOrEmpty(PlayerTui.managedV2.Genre) ? PlayerTui.managedV2.Genre : !string.IsNullOrEmpty(PlayerTui.managedV1.Genre.ToString()) ? PlayerTui.managedV1.Genre.ToString() : "")}}
                {{Translate.DoTranslation("Comment")}}: {{(!string.IsNullOrEmpty(PlayerTui.managedV2.Comment) ? PlayerTui.managedV2.Comment : !string.IsNullOrEmpty(PlayerTui.managedV1.Comment) ? PlayerTui.managedV1.Comment : "")}}
                {{Translate.DoTranslation("Duration")}}: {{PlayerTui.totalSpan}}
                {{Translate.DoTranslation("Lyrics")}}: {{(PlayerTui.lyricInstance is not null ? $"{PlayerTui.lyricInstance.Lines.Count} " + Translate.DoTranslation("lines") : Translate.DoTranslation("No lyrics"))}}
                
                -- {{Translate.DoTranslation("Layer info")}} --

                {{Translate.DoTranslation("Version")}}: {{PlayerTui.frameInfo.Version}}
                {{Translate.DoTranslation("Layer")}}: {{PlayerTui.frameInfo.Layer}}
                {{Translate.DoTranslation("Rate")}}: {{PlayerTui.frameInfo.Rate}}
                {{Translate.DoTranslation("Mode")}}: {{PlayerTui.frameInfo.Mode}}
                {{Translate.DoTranslation("Mode Ext")}}: {{PlayerTui.frameInfo.ModeExt}}
                {{Translate.DoTranslation("Frame Size")}}: {{PlayerTui.frameInfo.FrameSize}}
                {{Translate.DoTranslation("Flags")}}: {{PlayerTui.frameInfo.Flags}}
                {{Translate.DoTranslation("Emphasis")}}: {{PlayerTui.frameInfo.Emphasis}}
                {{Translate.DoTranslation("Bitrate")}}: {{PlayerTui.frameInfo.BitRate}}
                {{Translate.DoTranslation("ABR Rate")}}: {{PlayerTui.frameInfo.AbrRate}}
                {{Translate.DoTranslation("Variable bitrate")}}: {{PlayerTui.frameInfo.Vbr}}

                -- {{Translate.DoTranslation("Texts and Extras")}} --

                {{textsBuilder}}
                """
            );
            PlayerTui.rerender = true;
        }
    }
}
