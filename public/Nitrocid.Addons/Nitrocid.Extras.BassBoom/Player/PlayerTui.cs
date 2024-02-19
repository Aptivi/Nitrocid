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

using BassBoom.Basolia;
using BassBoom.Basolia.File;
using BassBoom.Basolia.Format;
using BassBoom.Basolia.Format.Cache;
using BassBoom.Basolia.Lyrics;
using BassBoom.Basolia.Playback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Base.Buffered;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Languages;
using Terminaux.Writer.FancyWriters;
using Terminaux.Inputs;
using Textify.General;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Base.Extensions;
using Terminaux.Reader;

namespace Nitrocid.Extras.BassBoom.Player
{
    internal static class PlayerTui
    {
        internal static Thread playerThread;
        internal static Lyric lyricInstance = null;
        internal static FrameInfo frameInfo = null;
        internal static Id3V1Metadata managedV1 = null;
        internal static Id3V2Metadata managedV2 = null;
        internal static TimeSpan totalSpan = new();
        internal static int total = 0;
        internal static (long rate, int channels, int encoding) formatInfo = new();
        internal static bool rerender = true;
        internal static int currentSong = 1;
        internal static double volume = 1.0;
        internal static bool exiting = false;
        internal static int position = 0;
        internal static bool advance = false;
        internal static bool populate = true;
        internal static bool paused = false;
        internal static bool failedToPlay = false;
        internal static string cachedLyric = "";
        internal static readonly List<string> musicFiles = [];
        internal static readonly List<CachedSongInfo> cachedInfos = [];

        public static void PlayerLoop()
        {
            // Prevent screensaver lock
            ScreensaverManager.PreventLock();

            volume = PlaybackTools.GetVolume().baseLinear;
            exiting = false;
            rerender = true;
            paused = false;
            populate = true;
            advance = false;

            // Populate the screen
            Screen playerScreen = new();
            ScreenTools.SetCurrent(playerScreen);

            // First, clear the screen to draw our TUI
            while (!exiting)
            {
                ScreenPart screenPart = new();
                Thread.Sleep(1);
                try
                {
                    // Redraw if necessary
                    if (ConsoleResizeHandler.WasResized(true))
                        rerender = true;
                    bool wasRerendered = rerender;
                    if (rerender)
                    {
                        rerender = false;
                        screenPart.AddDynamicText(HandleDraw);
                    }

                    // Current duration
                    position = FileTools.IsOpened ? PlaybackPositioningTools.GetCurrentDuration() : 0;
                    var posSpan = FileTools.IsOpened ? PlaybackPositioningTools.GetCurrentDurationSpan() : new();
                    string indicator =
                        Translate.DoTranslation("Seek") + $": {PlayerControls.seekRate:0.00} | " +
                        Translate.DoTranslation("Volume") + $": {volume:0.00}";
                    screenPart.AddDynamicText(() =>
                    {
                        var buffer = new StringBuilder();
                        buffer.Append(
                            ProgressBarColor.RenderProgress(100 * (position / (double)total), 2, ConsoleWrapper.WindowHeight - 8, 3, 3, KernelColorTools.GetColor(KernelColorType.Progress), ColorTools.GetGray(), KernelColorTools.GetColor(KernelColorType.Background)) +
                            TextWriterWhereColor.RenderWhere($"{posSpan} / {totalSpan}", 3, ConsoleWrapper.WindowHeight - 9, KernelColorTools.GetColor(KernelColorType.NeutralText), KernelColorTools.GetColor(KernelColorType.Background)) +
                            TextWriterWhereColor.RenderWhere(indicator, ConsoleWrapper.WindowWidth - indicator.Length - 3, ConsoleWrapper.WindowHeight - 9, KernelColorTools.GetColor(KernelColorType.NeutralText), KernelColorTools.GetColor(KernelColorType.Background))
                        );
                        return buffer.ToString();
                    });

                    // Get the lyrics
                    if (PlaybackTools.Playing)
                    {
                        // Print the lyrics, if any
                        if (lyricInstance is not null)
                        {
                            string current = lyricInstance.GetLastLineCurrent();
                            if (current != cachedLyric || wasRerendered)
                            {
                                cachedLyric = current;
                                screenPart.AddDynamicText(() =>
                                {
                                    var buffer = new StringBuilder();
                                    buffer.Append(
                                        TextWriterWhereColor.RenderWhere(ConsoleClearing.GetClearLineToRightSequence(), 0, ConsoleWrapper.WindowHeight - 10, KernelColorTools.GetColor(KernelColorType.NeutralText), KernelColorTools.GetColor(KernelColorType.Background)) +
                                        CenteredTextColor.RenderCentered(ConsoleWrapper.WindowHeight - 10, lyricInstance.GetLastLineCurrent(), KernelColorTools.GetColor(KernelColorType.NeutralText), KernelColorTools.GetColor(KernelColorType.Background))
                                    );
                                    return buffer.ToString();
                                });
                            }
                        }
                        else
                            cachedLyric = "";
                    }
                    else
                    {
                        cachedLyric = "";
                        screenPart.AddDynamicText(() =>
                        {
                            var buffer = new StringBuilder();
                            buffer.Append(
                                TextWriterWhereColor.RenderWhere(ConsoleClearing.GetClearLineToRightSequence(), 0, ConsoleWrapper.WindowHeight - 10, KernelColorTools.GetColor(KernelColorType.NeutralText), KernelColorTools.GetColor(KernelColorType.Background))
                            );
                            return buffer.ToString();
                        });
                    }

                    // Render the buffer
                    playerScreen.AddBufferedPart("BassBoom Player", screenPart);
                    ScreenTools.Render();

                    // Handle the keystroke
                    if (ConsoleWrapper.KeyAvailable)
                    {
                        var keystroke = TermReader.ReadKey();
                        if (PlaybackTools.Playing)
                            HandleKeypressPlayMode(keystroke);
                        else
                            HandleKeypressIdleMode(keystroke);
                    }
                }
                catch (BasoliaException bex)
                {
                    if (PlaybackTools.Playing)
                        PlaybackTools.Stop();
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("There's an error with Basolia when trying to process the music file.") + "\n\n" + bex.Message);
                    rerender = true;
                }
                catch (BasoliaOutException bex)
                {
                    if (PlaybackTools.Playing)
                        PlaybackTools.Stop();
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("There's an error with Basolia output when trying to process the music file.") + "\n\n" + bex.Message);
                    rerender = true;
                }
                catch (Exception ex)
                {
                    if (PlaybackTools.Playing)
                        PlaybackTools.Stop();
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("There's an unknown error when trying to process the music file.") + "\n\n" + ex.Message);
                    rerender = true;
                }
                playerScreen.RemoveBufferedParts();
            }

            // Close the file if open
            if (FileTools.IsOpened)
                FileTools.CloseFile();

            // Restore state
            ConsoleWrapper.CursorVisible = true;
            ColorTools.LoadBack();
            ScreensaverManager.AllowLock();
            ScreenTools.UnsetCurrent(playerScreen);
        }

        private static void HandleKeypressIdleMode(ConsoleKeyInfo keystroke)
        {
            switch (keystroke.Key)
            {
                case ConsoleKey.UpArrow:
                    PlayerControls.RaiseVolume();
                    break;
                case ConsoleKey.DownArrow:
                    PlayerControls.LowerVolume();
                    break;
                case ConsoleKey.Spacebar:
                    playerThread = new(HandlePlay);
                    PlayerControls.Play();
                    break;
                case ConsoleKey.B:
                    PlayerControls.SeekBeginning();
                    PlayerControls.PreviousSong();
                    playerThread = new(HandlePlay);
                    PlayerControls.Play();
                    break;
                case ConsoleKey.N:
                    PlayerControls.SeekBeginning();
                    PlayerControls.NextSong();
                    playerThread = new(HandlePlay);
                    PlayerControls.Play();
                    break;
                case ConsoleKey.H:
                    PlayerControls.ShowHelp();
                    break;
                case ConsoleKey.I:
                    PlayerControls.ShowSongInfo();
                    break;
                case ConsoleKey.A:
                    PlayerControls.PromptForAddSong();
                    break;
                case ConsoleKey.S:
                    PlayerControls.PromptForAddDirectory();
                    break;
                case ConsoleKey.R:
                    PlayerControls.Stop(false);
                    PlayerControls.SeekBeginning();
                    if (keystroke.Modifiers == ConsoleModifiers.Control)
                        PlayerControls.RemoveAllSongs();
                    else
                        PlayerControls.RemoveCurrentSong();
                    break;
                case ConsoleKey.Q:
                    PlayerControls.Exit();
                    break;
            }
        }

        private static void HandleKeypressPlayMode(ConsoleKeyInfo keystroke)
        {
            switch (keystroke.Key)
            {
                case ConsoleKey.UpArrow:
                    PlayerControls.RaiseVolume();
                    break;
                case ConsoleKey.DownArrow:
                    PlayerControls.LowerVolume();
                    break;
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
                case ConsoleKey.H:
                    PlayerControls.ShowHelp();
                    break;
                case ConsoleKey.I:
                    PlayerControls.ShowSongInfo();
                    break;
                case ConsoleKey.S:
                    PlayerControls.PromptSeek();
                    break;
                case ConsoleKey.Q:
                    PlayerControls.Exit();
                    break;
            }
        }

        private static void HandlePlay()
        {
            try
            {
                foreach (var musicFile in musicFiles.Skip(currentSong - 1))
                {
                    if (!advance || exiting)
                        return;
                    else
                        populate = true;
                    currentSong = musicFiles.IndexOf(musicFile) + 1;
                    PlayerControls.PopulateMusicFileInfo(musicFile);
                    TextWriterRaw.WritePlain(PlayerControls.RenderSongName(musicFile), false);
                    if (paused)
                    {
                        paused = false;
                        PlaybackPositioningTools.SeekToFrame(position);
                    }
                    PlaybackTools.Play();
                }
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Playback failure") + $": {ex.Message}");
                failedToPlay = true;
            }
            finally
            {
                lyricInstance = null;
                rerender = true;
            }
        }

        private static string HandleDraw()
        {
            // Prepare things
            var drawn = new StringBuilder();
            ConsoleWrapper.CursorVisible = false;
            ColorTools.LoadBack();

            // First, print the keystrokes
            string keystrokes =
                "[SPACE] " + Translate.DoTranslation("Play/Pause") +
                " - [ESC] " + Translate.DoTranslation("Stop") +
                " - [Q] " + Translate.DoTranslation("Exit") +
                " - [H] " + Translate.DoTranslation("Help");
            drawn.Append(CenteredTextColor.RenderCentered(ConsoleWrapper.WindowHeight - 2, keystrokes));

            // Print the separator and the music file info
            string separator = new('=', ConsoleWrapper.WindowWidth);
            drawn.Append(CenteredTextColor.RenderCentered(ConsoleWrapper.WindowHeight - 4, separator));

            // Write powered by...
            drawn.Append(TextWriterWhereColor.RenderWhere($"[ {Translate.DoTranslation("Powered by BassBoom")} ]", 2, ConsoleWrapper.WindowHeight - 4));

            // In case we have no songs in the playlist...
            if (musicFiles.Count == 0)
            {
                int height = (ConsoleWrapper.WindowHeight - 10) / 2;
                drawn.Append(CenteredTextColor.RenderCentered(height, Translate.DoTranslation("Press 'A' to insert a single song to the playlist, or 'S' to insert the whole music library.")));
                return drawn.ToString();
            }

            // Populate music file info, as necessary
            if (populate)
                PlayerControls.PopulateMusicFileInfo(musicFiles[currentSong - 1]);
            drawn.Append(PlayerControls.RenderSongName(musicFiles[currentSong - 1]));

            // Now, print the list of songs.
            int startPos = 3;
            int endPos = ConsoleWrapper.WindowHeight - 10;
            int songsPerPage = endPos - startPos;
            int currentPage = (currentSong - 1) / songsPerPage;
            int startIndex = songsPerPage * currentPage;
            var playlist = new StringBuilder();
            for (int i = 0; i <= songsPerPage - 1; i++)
            {
                // Populate the first pane
                string finalEntry = "";
                int finalIndex = i + startIndex;
                if (finalIndex <= musicFiles.Count - 1)
                {
                    // Here, it's getting uglier as we don't have ElementAt() in IEnumerable, too!
                    var (musicName, musicArtist, _) = PlayerControls.GetMusicNameArtistGenre(finalIndex);
                    string duration = cachedInfos[finalIndex].DurationSpan;
                    string renderedDuration = $"[{duration}]";
                    string dataObject = $"  {musicArtist} - {musicName}".Truncate(ConsoleWrapper.WindowWidth - renderedDuration.Length - 5);
                    string spaces = new(' ', ConsoleWrapper.WindowWidth - 4 - duration.Length - dataObject.Length);
                    finalEntry = dataObject + spaces + renderedDuration;
                }

                // Render an entry
                var finalForeColor = finalIndex == currentSong - 1 ? new Color(ConsoleColors.Green) : new Color(ConsoleColors.Silver);
                int top = startPos + finalIndex - startIndex;
                playlist.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(1, top + 1)}" +
                    $"{finalForeColor.VTSequenceForeground}" +
                    finalEntry +
                    new string(' ', ConsoleWrapper.WindowWidth - finalEntry.Length)
                );
            }
            drawn.Append(playlist);
            return drawn.ToString();
        }
    }
}
