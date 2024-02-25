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

//
// BassBoom  Copyright (C) 2023  Aptivi
//
// This file is part of Nitrocid KS
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
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base.Extensions;
using Terminaux.Reader;
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
            screen.AddBufferedPart("BassBoom Player - Equalizer", screenPart);
            if (screen.CheckBufferedPart("BassBoom Player"))
                screen.RemoveBufferedPart("BassBoom Player");

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
            if (screen.CheckBufferedPart("BassBoom Player - Equalizer"))
                screen.RemoveBufferedPart("BassBoom Player - Equalizer");
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

            // Print the separator
            string separator = new('=', ConsoleWrapper.WindowWidth);
            drawn.Append(CenteredTextColor.RenderCentered(ConsoleWrapper.WindowHeight - 4, separator));

            // Write powered by...
            drawn.Append(TextWriterWhereColor.RenderWhere($"[ {Translate.DoTranslation("Powered by BassBoom")} ]", 2, ConsoleWrapper.WindowHeight - 4));

            // Write current song
            if (PlayerTui.musicFiles.Count > 0)
                drawn.Append(PlayerControls.RenderSongName(PlayerTui.musicFiles[PlayerTui.currentSong - 1]));

            // Now, print the list of bands and their values.
            int startPos = 3;
            int endPos = ConsoleWrapper.WindowHeight - 5;
            int songsPerPage = endPos - startPos;
            int currentPage = currentBandIdx / songsPerPage;
            int startIndex = songsPerPage * currentPage;
            var eqs = new StringBuilder();
            for (int i = 0; i <= songsPerPage - 1; i++)
            {
                // Populate the first pane
                string finalEntry = "";
                int finalIndex = i + startIndex;
                bool selected = finalIndex == currentBandIdx;
                if (finalIndex <= 31)
                {
                    // Here, it's getting uglier as we don't have ElementAt() in IEnumerable, too!
                    double val = EqualizerControls.GetEqualizer(finalIndex);
                    string eqKey = $"Equalizer Band #{finalIndex + 1}";
                    string renderedVal = $"[{val:0.00}] {(selected ? "<<<" : "   ")}";
                    string dataObject = $"  {(selected ? ">>>" : "   ")} {eqKey}".Truncate(ConsoleWrapper.WindowWidth - renderedVal.Length - 5);
                    string spaces = new(' ', ConsoleWrapper.WindowWidth - 2 - renderedVal.Length - dataObject.Length);
                    finalEntry = dataObject + spaces + renderedVal;
                }

                // Render an entry
                var finalForeColor = selected ? new Color(ConsoleColors.Green) : new Color(ConsoleColors.Silver);
                int top = startPos + finalIndex - startIndex;
                eqs.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(1, top + 1)}" +
                    $"{finalForeColor.VTSequenceForeground}" +
                    finalEntry +
                    new string(' ', ConsoleWrapper.WindowWidth - finalEntry.Length) +
                    $"{ColorTools.CurrentForegroundColor.VTSequenceForeground}"
                );
            }
            drawn.Append(eqs);
            return drawn.ToString();
        }
    }
}
