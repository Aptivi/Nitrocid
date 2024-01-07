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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Terminaux.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase;
using Terminaux.Writer.FancyWriters;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Drivers.RNG;
using Terminaux.Inputs;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    static class BackRace
    {
        internal static void OpenBackRace()
        {
            // Clear the screen
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();

            // Some essential variables
            int chance = 30;
            BackRaceHorse[] horses;
            Color[] horseColors;

            // Reset all the horses
            void ResetAll()
            {
                horses =
                [
                    new BackRaceHorse(1),
                    new BackRaceHorse(2),
                    new BackRaceHorse(3),
                    new BackRaceHorse(4),
                    new BackRaceHorse(5),
                ];
                horseColors =
                [
                    ColorTools.GetRandomColor(ColorType.TrueColor),
                    ColorTools.GetRandomColor(ColorType.TrueColor),
                    ColorTools.GetRandomColor(ColorType.TrueColor),
                    ColorTools.GetRandomColor(ColorType.TrueColor),
                    ColorTools.GetRandomColor(ColorType.TrueColor)
                ];
            }
            ResetAll();

            // Main game loop
            bool exiting = false;
            bool racing = false;
            int selected = 1;
            int winner = 0;
            while (!exiting)
            {
                // We need to show five boxes and five progress bars representing the horses
                int consoleSixthsHeight = ConsoleWrapper.WindowHeight / 6;
                int boxLeft = 1;
                int boxWidth = 4;
                int progressLeft = 7;
                for (int i = 0; i < 5; i++)
                {
                    // Indicate the selected horse by coloring it white
                    int height = consoleSixthsHeight * i + 2;
                    var finalColor = i + 1 == selected ? ConsoleColors.White : horseColors[i];
                    var horse = horses[i];
                    BorderColor.WriteBorder(boxLeft, height, boxWidth, 1, finalColor);
                    TextWriterWhereColor.WriteWhereColor($"{horse.HorseProgress:000}%", 2, height + 1, finalColor);
                    ProgressBarColor.WriteProgress(horse.HorseProgress, progressLeft, height, finalColor, finalColor);
                }

                // Check to see if we're on the rest mode or on the race mode
                string bindings = Translate.DoTranslation("[ENTER] Start the race | [ESC] Exit | [UP/DOWN] Move selection");
                int bindingsPositionX = ConsoleWrapper.WindowWidth / 2 - bindings.Length / 2;
                int bindingsPositionY = ConsoleWrapper.WindowHeight - 2;
                if (racing)
                {
                    // Race mode. Wipe the keybindings
                    TextWriters.WriteWhere(new string(' ', bindings.Length), bindingsPositionX, bindingsPositionY, KernelColorType.NeutralText);

                    // Write the positions
                    var horsesSorted = horses
                        .OrderByDescending((progress) => progress.HorseProgress)
                        .ToArray();
                    List<string> positions = [];
                    for (int i = 0; i < horsesSorted.Length; i++)
                        positions.Add(Translate.DoTranslation("Horse") + $" {horsesSorted[i].HorseNumber}: #{i + 1}");
                    string renderedPositions = string.Join(" | ", positions);
                    int positionsPositionX = ConsoleWrapper.WindowWidth / 2 - renderedPositions.Length / 2;
                    TextWriters.WriteWhere(renderedPositions, positionsPositionX, bindingsPositionY, KernelColorType.NeutralText);

                    // Update each horse with their own movement
                    for (int i = 0; i < 5; i++)
                    {
                        // Check to see if the horse is moving
                        bool isMoving = RandomDriver.RandomChance(chance);
                        if (isMoving)
                            horses[i].HorseProgress += 1;

                        // Check for the winner
                        if (horses[i].HorseProgress >= 100)
                        {
                            racing = false;
                            winner = i + 1;
                        }
                    }

                    // Wait for a few milliseconds
                    Thread.Sleep(100);

                    // If the user chose the same horse that won, congratulate the user.
                    if (selected == winner)
                    {
                        InfoBoxColor.WriteInfoBoxColor(Translate.DoTranslation("Your horse won the race!"), KernelColorTools.GetColor(KernelColorType.Success));
                        ConsoleWrapper.Clear();
                        ResetAll();
                    }
                    else if (winner > 0)
                    {
                        InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Your horse lost the race!"));
                        ConsoleWrapper.Clear();
                        ResetAll();
                    }
                }
                else
                {
                    // Rest mode. Write the keybindings
                    TextWriters.WriteWhere(bindings, bindingsPositionX, bindingsPositionY, KernelColorType.NeutralText);

                    // Wait for the input
                    winner = 0;
                    var input = Input.DetectKeypress().Key;
                    switch (input)
                    {
                        case ConsoleKey.UpArrow:
                            // Selected the previous horse
                            selected--;
                            if (selected < 1)
                                selected = 5;
                            break;
                        case ConsoleKey.DownArrow:
                            // Selected the next horse
                            selected++;
                            if (selected > 5)
                                selected = 1;
                            break;
                        case ConsoleKey.Enter:
                            // Started the race
                            racing = true;
                            break;
                        case ConsoleKey.Escape:
                            // Exited the game
                            exiting = true;
                            break;
                    }
                }
            }
            ConsoleWrapper.Clear();
        }
    }

    internal class BackRaceHorse
    {
        internal int HorseNumber { get; private set; }
        internal int HorseProgress { get; set; } = 0;

        internal BackRaceHorse(int horseNumber)
        {
            HorseNumber = horseNumber;
        }
    }
}
