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
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Inputs;
using Nitrocid.Languages;
using BassBoom.Basolia.Exceptions;
using Terminaux.Inputs.Styles;

namespace Nitrocid.Extras.BassBoom.Player
{
    internal static class Radio
    {
        internal static Thread? playerThread;

        public static void RadioLoop()
        {
            Common.exiting = false;
            Common.volume = PlaybackTools.GetVolume().baseLinear;
            Common.isRadioMode = true;

            // Populate the screen
            Screen radioScreen = new();
            ScreenTools.SetCurrent(radioScreen);

            // Make a screen part to draw our TUI
            ScreenPart screenPart = new();

            // Handle drawing
            screenPart.AddDynamicText(HandleDraw);

            // Current volume
            int hue = 0;
            screenPart.AddDynamicText(() =>
            {
                if (Common.CurrentCachedInfo is null)
                    return "";
                var buffer = new StringBuilder();
                string indicator = $"╣ {Translate.DoTranslation("Volume")}: {Common.volume:0.00} ╠";
                var disco = PlaybackTools.Playing && Common.enableDisco ? new Color($"hsl:{hue};50;50") : BassBoomInit.white;
                if (PlaybackTools.Playing)
                {
                    hue++;
                    if (hue >= 360)
                        hue = 0;
                }
                buffer.Append(
                    BoxFrameColor.RenderBoxFrame(2, ConsoleWrapper.WindowHeight - 8, ConsoleWrapper.WindowWidth - 6, 1, disco) +
                    TextWriterWhereColor.RenderWhereColor(indicator, ConsoleWrapper.WindowWidth - indicator.Length - 4, ConsoleWrapper.WindowHeight - 8, disco)
                );
                return buffer.ToString();
            });

            // Render the buffer
            radioScreen.AddBufferedPart("BassBoom Player", screenPart);
            radioScreen.ResetResize = false;

            // Then, the main loop
            while (!Common.exiting)
            {
                Thread.Sleep(1);
                try
                {
                    if (!radioScreen.CheckBufferedPart("BassBoom Player"))
                        radioScreen.AddBufferedPart("BassBoom Player", screenPart);
                    ScreenTools.Render();

                    // Handle the keystroke
                    if (ConsoleWrapper.KeyAvailable)
                    {
                        var keystroke = Input.ReadKey();
                        if (PlaybackTools.Playing)
                            HandleKeypressPlayMode(keystroke, radioScreen);
                        else
                            HandleKeypressIdleMode(keystroke, radioScreen);
                    }
                }
                catch (BasoliaException bex)
                {
                    if (PlaybackTools.Playing)
                        PlaybackTools.Stop();
                    InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("There's an error with Basolia when trying to process the music file.") + "\n\n" + bex.Message);
                    radioScreen.RequireRefresh();
                }
                catch (BasoliaOutException bex)
                {
                    if (PlaybackTools.Playing)
                        PlaybackTools.Stop();
                    InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("There's an error with Basolia output when trying to process the music file.") + "\n\n" + bex.Message);
                    radioScreen.RequireRefresh();
                }
                catch (Exception ex)
                {
                    if (PlaybackTools.Playing)
                        PlaybackTools.Stop();
                    InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("There's an unknown error when trying to process the music file.") + "\n\n" + ex.Message);
                    radioScreen.RequireRefresh();
                }
            }

            // Close the file if open
            if (FileTools.IsOpened)
                FileTools.CloseFile();

            // Restore state
            ConsoleWrapper.CursorVisible = true;
            ColorTools.LoadBack();
            radioScreen.RemoveBufferedParts();
            ScreenTools.UnsetCurrent(radioScreen);
        }

        private static void HandleKeypressIdleMode(ConsoleKeyInfo keystroke, Screen playerScreen)
        {
            switch (keystroke.Key)
            {
                case ConsoleKey.Spacebar:
                    playerThread = new(HandlePlay);
                    RadioControls.Play();
                    break;
                case ConsoleKey.B:
                    RadioControls.PreviousStation();
                    break;
                case ConsoleKey.N:
                    RadioControls.NextStation();
                    break;
                case ConsoleKey.I:
                    if (keystroke.Modifiers == ConsoleModifiers.Control)
                        RadioControls.ShowExtendedStationInfo();
                    else
                        RadioControls.ShowStationInfo();
                    playerScreen.RequireRefresh();
                    break;
                case ConsoleKey.A:
                    RadioControls.PromptForAddStation();
                    playerScreen.RequireRefresh();
                    break;
                case ConsoleKey.R:
                    RadioControls.Stop(false);
                    if (keystroke.Modifiers == ConsoleModifiers.Control)
                        RadioControls.RemoveAllStations();
                    else
                        RadioControls.RemoveCurrentStation();
                    break;
                default:
                    Common.HandleKeypressCommon(keystroke, playerScreen, true);
                    break;
            }
        }

        private static void HandleKeypressPlayMode(ConsoleKeyInfo keystroke, Screen playerScreen)
        {
            switch (keystroke.Key)
            {
                case ConsoleKey.B:
                    RadioControls.Stop(false);
                    RadioControls.PreviousStation();
                    playerThread = new(HandlePlay);
                    RadioControls.Play();
                    break;
                case ConsoleKey.N:
                    RadioControls.Stop(false);
                    RadioControls.NextStation();
                    playerThread = new(HandlePlay);
                    RadioControls.Play();
                    break;
                case ConsoleKey.Spacebar:
                    RadioControls.Pause();
                    break;
                case ConsoleKey.R:
                    RadioControls.Stop(false);
                    if (keystroke.Modifiers == ConsoleModifiers.Control)
                        RadioControls.RemoveAllStations();
                    else
                        RadioControls.RemoveCurrentStation();
                    break;
                case ConsoleKey.Escape:
                    RadioControls.Stop();
                    break;
                case ConsoleKey.I:
                    if (keystroke.Modifiers == ConsoleModifiers.Control)
                        RadioControls.ShowExtendedStationInfo();
                    else
                        RadioControls.ShowStationInfo();
                    playerScreen.RequireRefresh();
                    break;
                case ConsoleKey.D:
                    RadioControls.Pause();
                    Common.HandleKeypressCommon(keystroke, playerScreen, true);
                    playerThread = new(HandlePlay);
                    RadioControls.Play();
                    playerScreen.RequireRefresh();
                    break;
                default:
                    Common.HandleKeypressCommon(keystroke, playerScreen, true);
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
                    RadioControls.PopulateRadioStationInfo(musicFile.MusicPath);
                    TextWriterRaw.WritePlain(RadioControls.RenderStationName(), false);
                    if (Common.paused)
                        Common.paused = false;
                    PlaybackTools.Play();
                }
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Playback failure") + $": {ex.Message}");
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

            // In case we have no stations in the playlist...
            if (Common.cachedInfos.Count == 0)
            {
                int height = (ConsoleWrapper.WindowHeight - 6) / 2;
                drawn.Append(CenteredTextColor.RenderCentered(height, Translate.DoTranslation("Press 'A' to insert a radio station to the playlist.")));
                return drawn.ToString();
            }

            // Populate music file info, as necessary
            if (Common.CurrentCachedInfo is not null)
            {
                if (Common.populate)
                    RadioControls.PopulateRadioStationInfo(Common.CurrentCachedInfo.MusicPath);
                drawn.Append(RadioControls.RenderStationName());
            }

            // Now, print the list of stations.
            var choices = new List<InputChoiceInfo>();
            int startPos = 4;
            int endPos = ConsoleWrapper.WindowHeight - 10;
            int stationsPerPage = endPos - startPos;
            int max = Common.cachedInfos.Select((_, idx) => idx).Max((idx) => $"  {idx + 1}) ".Length);
            for (int i = 0; i < Common.cachedInfos.Count; i++)
            {
                // Populate the first pane
                string stationName = Common.cachedInfos[i].StationName;
                string duration = Common.cachedInfos[i].DurationSpan;
                string stationPreview = $"[{duration}] {stationName}";
                choices.Add(new($"{i + 1}", stationPreview));
            }
            drawn.Append(
                BoxFrameColor.RenderBoxFrame(2, 3, ConsoleWrapper.WindowWidth - 6, stationsPerPage) +
                SelectionInputTools.RenderSelections([.. choices], 3, 4, Common.currentPos - 1, stationsPerPage, ConsoleWrapper.WindowWidth - 6, selectedForegroundColor: new Color(ConsoleColors.Green), foregroundColor: new Color(ConsoleColors.Silver))
            );
            return drawn.ToString();
        }
    }
}
