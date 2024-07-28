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

using BassBoom.Basolia;
using BassBoom.Basolia.File;
using BassBoom.Basolia.Playback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Terminaux.Reader;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Inputs;
using Nitrocid.Languages;
using BassBoom.Basolia.Exceptions;

namespace Nitrocid.Extras.BassBoom.Player
{
    internal static class PlayerTui
    {
        internal static Thread playerThread;
        internal static int position = 0;
        internal static readonly List<string> passedMusicPaths = [];

        public static void PlayerLoop()
        {
            Common.volume = PlaybackTools.GetVolume().baseLinear;

            // Populate the screen
            Screen playerScreen = new();
            ScreenTools.SetCurrent(playerScreen);

            // Make a screen part to draw our TUI
            ScreenPart screenPart = new();

            // Handle drawing
            screenPart.AddDynamicText(HandleDraw);

            // Current duration
            int hue = 0;
            screenPart.AddDynamicText(() =>
            {
                if (Common.CurrentCachedInfo is null)
                    return "";
                var buffer = new StringBuilder();
                position = FileTools.IsOpened ? PlaybackPositioningTools.GetCurrentDuration() : 0;
                var posSpan = FileTools.IsOpened ? PlaybackPositioningTools.GetCurrentDurationSpan() : new();
                var disco = PlaybackTools.Playing && Common.enableDisco ? new Color($"hsl:{hue};50;50") : BassBoomInit.white;
                if (PlaybackTools.Playing)
                {
                    hue++;
                    if (hue >= 360)
                        hue = 0;
                }
                string indicator =
                    $"╣ {Translate.DoTranslation("Seek")}: {PlayerControls.seekRate:0.00} | " +
                    $"{Translate.DoTranslation("Volume")}: {Common.volume:0.00} ╠";
                string lyric = Common.CurrentCachedInfo.LyricInstance is not null ? Common.CurrentCachedInfo.LyricInstance.GetLastLineCurrent() : "";
                string finalLyric = string.IsNullOrWhiteSpace(lyric) ? "..." : lyric;
                buffer.Append(
                    ProgressBarColor.RenderProgress(100 * (position / (double)Common.CurrentCachedInfo.Duration), 2, ConsoleWrapper.WindowHeight - 8, ConsoleWrapper.WindowWidth - 6, disco, disco) +
                    TextWriterWhereColor.RenderWhereColor($"╣ {posSpan} / {Common.CurrentCachedInfo.DurationSpan} ╠", 4, ConsoleWrapper.WindowHeight - 8, disco) +
                    TextWriterWhereColor.RenderWhereColor(indicator, ConsoleWrapper.WindowWidth - indicator.Length - 4, ConsoleWrapper.WindowHeight - 8, disco) +
                    CenteredTextColor.RenderCentered(ConsoleWrapper.WindowHeight - 6, Common.CurrentCachedInfo.LyricInstance is not null && PlaybackTools.Playing ? $"╣ {finalLyric} ╠" : "", disco)
                );
                return buffer.ToString();
            });

            // Render the buffer
            playerScreen.AddBufferedPart("BassBoom Player", screenPart);
            playerScreen.ResetResize = false;

            // Then, the main loop
            while (!Common.exiting)
            {
                Thread.Sleep(1);
                try
                {
                    if (!playerScreen.CheckBufferedPart("BassBoom Player"))
                        playerScreen.AddBufferedPart("BassBoom Player", screenPart);
                    ScreenTools.Render();

                    // Handle the keystroke
                    if (ConsoleWrapper.KeyAvailable)
                    {
                        var keystroke = TermReader.ReadKey();
                        if (PlaybackTools.Playing)
                            HandleKeypressPlayMode(keystroke, playerScreen);
                        else
                            HandleKeypressIdleMode(keystroke, playerScreen);
                    }
                }
                catch (BasoliaException bex)
                {
                    if (PlaybackTools.Playing)
                        PlaybackTools.Stop();
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("There's an error with Basolia when trying to process the music file.") + "\n\n" + bex.Message);
                    playerScreen.RequireRefresh();
                }
                catch (BasoliaOutException bex)
                {
                    if (PlaybackTools.Playing)
                        PlaybackTools.Stop();
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("There's an error with Basolia output when trying to process the music file.") + "\n\n" + bex.Message);
                    playerScreen.RequireRefresh();
                }
                catch (Exception ex)
                {
                    if (PlaybackTools.Playing)
                        PlaybackTools.Stop();
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("There's an unknown error when trying to process the music file.") + "\n\n" + ex.Message);
                    playerScreen.RequireRefresh();
                }
            }

            // Close the file if open
            if (FileTools.IsOpened)
                FileTools.CloseFile();

            // Restore state
            ConsoleWrapper.CursorVisible = true;
            ColorTools.LoadBack();
            playerScreen.RemoveBufferedParts();
            ScreenTools.UnsetCurrent(playerScreen);
        }

        private static void HandleKeypressIdleMode(ConsoleKeyInfo keystroke, Screen playerScreen)
        {
            switch (keystroke.Key)
            {
                case ConsoleKey.Spacebar:
                    playerThread = new(HandlePlay);
                    PlayerControls.Play();
                    break;
                case ConsoleKey.B:
                    PlayerControls.SeekBeginning();
                    PlayerControls.PreviousSong();
                    break;
                case ConsoleKey.N:
                    PlayerControls.SeekBeginning();
                    PlayerControls.NextSong();
                    break;
                case ConsoleKey.I:
                    PlayerControls.ShowSongInfo();
                    playerScreen.RequireRefresh();
                    break;
                case ConsoleKey.A:
                    PlayerControls.PromptForAddSong();
                    playerScreen.RequireRefresh();
                    break;
                case ConsoleKey.S:
                    PlayerControls.PromptForAddDirectory();
                    playerScreen.RequireRefresh();
                    break;
                case ConsoleKey.R:
                    PlayerControls.Stop(false);
                    PlayerControls.SeekBeginning();
                    if (keystroke.Modifiers == ConsoleModifiers.Control)
                        PlayerControls.RemoveAllSongs();
                    else
                        PlayerControls.RemoveCurrentSong();
                    break;
                case ConsoleKey.C:
                    if (Common.CurrentCachedInfo is null)
                        return;
                    if (keystroke.Modifiers == ConsoleModifiers.Shift)
                        PlayerControls.SeekTo(Common.CurrentCachedInfo.RepeatCheckpoint);
                    else
                        Common.CurrentCachedInfo.RepeatCheckpoint = PlaybackPositioningTools.GetCurrentDurationSpan();
                    break;
                default:
                    Common.HandleKeypressCommon(keystroke, playerScreen, false);
                    break;
            }
        }

        private static void HandleKeypressPlayMode(ConsoleKeyInfo keystroke, Screen playerScreen)
        {
            switch (keystroke.Key)
            {
                case ConsoleKey.RightArrow:
                    if (keystroke.Modifiers == ConsoleModifiers.Control)
                        PlayerControls.seekRate += 0.05d;
                    else
                        PlayerControls.SeekForward();
                    break;
                case ConsoleKey.LeftArrow:
                    if (keystroke.Modifiers == ConsoleModifiers.Control)
                        PlayerControls.seekRate -= 0.05d;
                    else
                        PlayerControls.SeekBackward();
                    break;
                case ConsoleKey.B:
                    PlayerControls.Stop(false);
                    PlayerControls.SeekBeginning();
                    PlayerControls.PreviousSong();
                    playerThread = new(HandlePlay);
                    PlayerControls.Play();
                    break;
                case ConsoleKey.F:
                    PlayerControls.SeekPreviousLyric();
                    break;
                case ConsoleKey.G:
                    PlayerControls.SeekNextLyric();
                    break;
                case ConsoleKey.J:
                    PlayerControls.SeekCurrentLyric();
                    break;
                case ConsoleKey.K:
                    PlayerControls.SeekWhichLyric();
                    playerScreen.RequireRefresh();
                    break;
                case ConsoleKey.N:
                    PlayerControls.Stop(false);
                    PlayerControls.SeekBeginning();
                    PlayerControls.NextSong();
                    playerThread = new(HandlePlay);
                    PlayerControls.Play();
                    break;
                case ConsoleKey.Spacebar:
                    PlayerControls.Pause();
                    break;
                case ConsoleKey.R:
                    PlayerControls.Stop(false);
                    PlayerControls.SeekBeginning();
                    if (keystroke.Modifiers == ConsoleModifiers.Control)
                        PlayerControls.RemoveAllSongs();
                    else
                        PlayerControls.RemoveCurrentSong();
                    break;
                case ConsoleKey.Escape:
                    PlayerControls.Stop();
                    break;
                case ConsoleKey.I:
                    PlayerControls.ShowSongInfo();
                    playerScreen.RequireRefresh();
                    break;
                case ConsoleKey.S:
                    PlayerControls.PromptSeek();
                    playerScreen.RequireRefresh();
                    break;
                case ConsoleKey.D:
                    PlayerControls.Pause();
                    Common.HandleKeypressCommon(keystroke, playerScreen, false);
                    playerThread = new(HandlePlay);
                    PlayerControls.Play();
                    playerScreen.RequireRefresh();
                    break;
                case ConsoleKey.C:
                    if (Common.CurrentCachedInfo is null)
                        return;
                    if (keystroke.Modifiers == ConsoleModifiers.Shift)
                        PlayerControls.SeekTo(Common.CurrentCachedInfo.RepeatCheckpoint);
                    else
                        Common.CurrentCachedInfo.RepeatCheckpoint = PlaybackPositioningTools.GetCurrentDurationSpan();
                    break;
                default:
                    Common.HandleKeypressCommon(keystroke, playerScreen, false);
                    break;
            }
        }

        private static void HandlePlay()
        {
            try
            {
                foreach (var musicFile in Common.cachedInfos.Skip(Common.currentPos - 1))
                {
                    if (!Common.advance || Common.exiting)
                        return;
                    else
                        Common.populate = true;
                    Common.currentPos = Common.cachedInfos.IndexOf(musicFile) + 1;
                    PlayerControls.PopulateMusicFileInfo(musicFile.MusicPath);
                    TextWriterRaw.WritePlain(PlayerControls.RenderSongName(musicFile.MusicPath), false);
                    if (Common.paused)
                    {
                        Common.paused = false;
                        PlaybackPositioningTools.SeekToFrame(position);
                    }
                    PlaybackTools.Play();
                }
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Playback failure") + $": {ex.Message}");
                Common.failedToPlay = true;
            }
        }

        private static string HandleDraw()
        {
            // Prepare things
            var drawn = new StringBuilder();
            ConsoleWrapper.CursorVisible = false;

            // First, print the keystrokes
            string keystrokes =
                "[SPACE] " + Translate.DoTranslation("Play/Pause") +
                " - [ESC] " + Translate.DoTranslation("Stop") +
                " - [Q] " + Translate.DoTranslation("Exit") +
                " - [H] " + Translate.DoTranslation("Help");
            drawn.Append(CenteredTextColor.RenderCentered(ConsoleWrapper.WindowHeight - 2, keystrokes));

            // Print the separator and the music file info
            string separator = new('═', ConsoleWrapper.WindowWidth);
            drawn.Append(CenteredTextColor.RenderCentered(ConsoleWrapper.WindowHeight - 4, separator));

            // Write powered by...
            drawn.Append(TextWriterWhereColor.RenderWhere($"╣ {Translate.DoTranslation("Powered by BassBoom")} ╠", 2, ConsoleWrapper.WindowHeight - 4));

            // In case we have no songs in the playlist...
            if (Common.cachedInfos.Count == 0)
            {
                if (passedMusicPaths.Count > 0)
                {
                    foreach (string path in passedMusicPaths)
                    {
                        PlayerControls.PopulateMusicFileInfo(path);
                        Common.populate = true;
                    }
                    passedMusicPaths.Clear();
                }
                else
                {
                    int height = (ConsoleWrapper.WindowHeight - 6) / 2;
                    drawn.Append(CenteredTextColor.RenderCentered(height, Translate.DoTranslation("Press 'A' to insert a single song to the playlist, or 'S' to insert the whole music library.")));
                    return drawn.ToString();
                }
            }

            // Populate music file info, as necessary
            if (Common.populate)
                PlayerControls.PopulateMusicFileInfo(Common.CurrentCachedInfo.MusicPath);
            drawn.Append(PlayerControls.RenderSongName(Common.CurrentCachedInfo.MusicPath));

            // Now, print the list of songs.
            var choices = new List<InputChoiceInfo>();
            int startPos = 4;
            int endPos = ConsoleWrapper.WindowHeight - 10;
            int songsPerPage = endPos - startPos;
            int max = Common.cachedInfos.Select((_, idx) => idx).Max((idx) => $"  {idx + 1}) ".Length);
            for (int i = 0; i < Common.cachedInfos.Count; i++)
            {
                // Populate the first pane
                var (musicName, musicArtist, _) = PlayerControls.GetMusicNameArtistGenre(i);
                string duration = Common.cachedInfos[i].DurationSpan;
                string songPreview = $"[{duration}] {musicArtist} - {musicName}";
                choices.Add(new($"{i + 1}", songPreview));
            }
            drawn.Append(
                BoxFrameColor.RenderBoxFrame(2, 3, ConsoleWrapper.WindowWidth - 6, songsPerPage) +
                SelectionInputTools.RenderSelections([.. choices], 3, 4, Common.currentPos - 1, songsPerPage, ConsoleWrapper.WindowWidth - 6, selectedForegroundColor: new Color(ConsoleColors.Green), foregroundColor: new Color(ConsoleColors.Silver))
            );
            return drawn.ToString();
        }
    }
}
