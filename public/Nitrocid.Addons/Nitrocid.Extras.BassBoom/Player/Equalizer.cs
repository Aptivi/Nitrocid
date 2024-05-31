//
// BassBoom  Copyright (C) 2023  Aptivi
//
// This file is part of BassBoom
//
// BassBoom is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BassBoom is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//
using BassBoom.Basolia;
using System;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Terminaux.Reader;
using Terminaux.Inputs;
using System.Collections.Generic;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Base.Extensions;
using Nitrocid.Languages;

namespace Nitrocid.Extras.BassBoom.Player
{
    internal static class Equalizer
    {
        internal static bool exiting = false;
        internal static int currentBandIdx = 0;

        internal static void OpenEqualizer(Screen screen)
        {
            // First, initialize a screen part to handle drawing
            ScreenPart screenPart = new();
            screenPart.AddDynamicText(HandleDraw);
            screen.RemoveBufferedParts();
            screen.AddBufferedPart("BassBoom Player - Equalizer", screenPart);

            // Then, clear the screen to draw our TUI
            while (!exiting)
            {
                try
                {
                    // Render the buffer
                    ScreenTools.Render();

                    // Handle the keystroke
                    var keystroke = TermReader.ReadKey();
                    HandleKeypress(keystroke);
                }
                catch (BasoliaException bex)
                {
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("There's an error with Basolia when trying to process the equalizer operation.") + "\n\n" + bex.Message);
                }
                catch (BasoliaOutException bex)
                {
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("There's an error with Basolia output when trying to process the equalizer operation.") + "\n\n" + bex.Message);
                }
                catch (Exception ex)
                {
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("There's an unknown error when trying to process the equalizer operation.") + "\n\n" + ex.Message);
                }
            }

            // Restore state
            exiting = false;
            screen.RemoveBufferedParts();
            ColorTools.LoadBack();
        }

        private static void HandleKeypress(ConsoleKeyInfo keystroke)
        {
            switch (keystroke.Key)
            {
                case ConsoleKey.RightArrow:
                    {
                        double eq = EqualizerControls.GetCachedEqualizer(currentBandIdx);
                        eq += 0.05d;
                        EqualizerControls.SetEqualizer(currentBandIdx, eq);
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    {
                        double eq = EqualizerControls.GetCachedEqualizer(currentBandIdx);
                        eq -= 0.05d;
                        EqualizerControls.SetEqualizer(currentBandIdx, eq);
                    }
                    break;
                case ConsoleKey.UpArrow:
                    currentBandIdx--;
                    if (currentBandIdx < 0)
                        currentBandIdx = 0;
                    break;
                case ConsoleKey.DownArrow:
                    currentBandIdx++;
                    if (currentBandIdx > 31)
                        currentBandIdx = 31;
                    break;
                case ConsoleKey.R:
                    EqualizerControls.ResetEqualizers();
                    break;
                case ConsoleKey.Q:
                    exiting = true;
                    break;
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
                $"[<-|->] {Translate.DoTranslation("Change")}" +
                $" - [UP|DOWN] {Translate.DoTranslation("Select Band")}" +
                $" - [R] {Translate.DoTranslation("Reset")}" +
                $" - [Q] {Translate.DoTranslation("Exit")}";
            drawn.Append(CenteredTextColor.RenderCentered(ConsoleWrapper.WindowHeight - 2, keystrokes));

            // Print the separator and the music file info
            string separator = new('═', ConsoleWrapper.WindowWidth);
            drawn.Append(CenteredTextColor.RenderCentered(ConsoleWrapper.WindowHeight - 4, separator));

            // Write powered by...
            drawn.Append(TextWriterWhereColor.RenderWhere($"╣ {Translate.DoTranslation("Powered by BassBoom")} ╠", 2, ConsoleWrapper.WindowHeight - 4));

            // Write current song
            if (Common.cachedInfos.Count > 0)
            {
                if (Common.isRadioMode)
                    drawn.Append(RadioControls.RenderStationName());
                else
                    drawn.Append(PlayerControls.RenderSongName(Common.CurrentCachedInfo.MusicPath));
            }
            else
                drawn.Append(
                    TextWriterWhereColor.RenderWhere(ConsoleClearing.GetClearLineToRightSequence(), 0, 1) +
                    CenteredTextColor.RenderCentered(1, Translate.DoTranslation("Not playing. Music player is idle."), ConsoleColors.White)
                );

            // Now, print the list of bands and their values.
            var choices = new List<InputChoiceInfo>();
            int startPos = 4;
            int endPos = ConsoleWrapper.WindowHeight - 6;
            int bandsPerPage = endPos - startPos;
            for (int i = 0; i < 32; i++)
            {
                // Get the equalizer value for this band
                double val = EqualizerControls.GetEqualizer(i);
                string eqType =
                    i == 0 ? Translate.DoTranslation("Bass") :
                    i == 1 ? Translate.DoTranslation("Upper Mid") :
                    i > 1 ? Translate.DoTranslation("Treble") :
                    Translate.DoTranslation("Unknown band type");

                // Now, render it
                string bandData = $"[{val:0.00}] {Translate.DoTranslation("Equalizer Band")} #{i + 1} - {eqType}";
                choices.Add(new($"{i + 1}", bandData));
            }
            drawn.Append(
                BoxFrameColor.RenderBoxFrame(2, 3, ConsoleWrapper.WindowWidth - 6, bandsPerPage) +
                SelectionInputTools.RenderSelections([.. choices], 3, 4, currentBandIdx, bandsPerPage, ConsoleWrapper.WindowWidth - 6, selectedForegroundColor: new Color(ConsoleColors.Green), foregroundColor: new Color(ConsoleColors.Silver))
            );
            return drawn.ToString();
        }
    }
}
