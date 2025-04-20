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

using System.Collections.Generic;
using System;
using System.Threading;
using Terminaux.Colors;
using Textify.Data.Words;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Inputs;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    internal class Wordle
    {

        public static void InitializeWordle(bool orig = false, WordleWordDifficulty wordDifficulty = WordleWordDifficulty.Common)
        {
            string RandomWord = WordManager.GetRandomWordConditional(8, "", "", orig ? 5 : 0, wordDifficulty == WordleWordDifficulty.Uncommon ? WordDataType.Words : WordDataType.CommonWords);
            bool done = false;
            int currentGuessTry = 1;
            int currentGuessChar = 1;
            int maxGuesses = orig ? 5 : 8;
            char[,] currentTries = new char[maxGuesses, RandomWord.Length];

            while (!done)
            {
                ConsoleWrapper.CursorVisible = false;
                ConsoleWrapper.Clear();

                // Make 1x1 boxes
                RenderBoxes(RandomWord, maxGuesses, currentTries);

                // Let the user decide the character
                var pressedChar = Input.ReadKey();
                switch (pressedChar.Key)
                {
                    case ConsoleKey.Escape:
                        // User decided to escape
                        done = true;
                        break;
                    default:
                        // Add a character
                        currentTries[currentGuessTry - 1, currentGuessChar - 1] = pressedChar.KeyChar;
                        currentGuessChar++;
                        if (currentGuessChar > RandomWord.Length)
                        {
                            currentGuessChar = 1;
                            List<char> chars = [];

                            // Check to see if the word formed within the current guess try is right
                            for (int i = 0; i < RandomWord.Length; i++)
                                chars.Add(currentTries[currentGuessTry - 1, i]);
                            if (string.Join("", chars) == RandomWord)
                            {
                                RenderBoxes(RandomWord, maxGuesses, currentTries);
                                TextWriterWhereColor.WriteWhereColor(Translate.DoTranslation("You guessed the right word! You win!"), 0, ConsoleWrapper.WindowHeight - 1, false, ConsoleColors.Green);
                                Thread.Sleep(3000);
                                done = true;
                                break;
                            }
                            currentGuessTry++;
                            if (currentGuessTry > maxGuesses)
                            {
                                RenderBoxes(RandomWord, maxGuesses, currentTries);
                                TextWriterWhereColor.WriteWhereColor(Translate.DoTranslation("Game over"), 0, ConsoleWrapper.WindowHeight - 1, false, ConsoleColors.Red);
                                Thread.Sleep(3000);
                                done = true;
                            }
                        }
                        break;
                }
            }

            // Clean up
            ConsoleWrapper.Clear();
        }

        private static void RenderBoxes(string RandomWord, int maxGuesses, char[,] currentTries)
        {
            var boxColorNeutral = new Color(ConsoleColors.Silver);
            var boxColorRightChar = new Color(ConsoleColors.Green);
            var boxColorMatchingChar = new Color(ConsoleColors.DarkOrange);

            // Make 1x1 boxes according to the maximum guesses and the current word length and put their character positions
            // to the store
            for (int g = 1; g <= maxGuesses; g++)
            {
                bool isWrongAttempt = false;
                for (int l = 1; l <= RandomWord.Length; l++)
                {
                    var finalColor = boxColorNeutral;
                    char currChar = currentTries[g - 1, l - 1];

                    // If the character in the current guess matches the character in the random word, judge whether this guess
                    // is a wrong attempt and set the colors as necessary.
                    if (currentTries[g - 1, l - 1] == RandomWord[l - 1])
                    {
                        if (isWrongAttempt)
                            finalColor = boxColorMatchingChar;
                        else
                            finalColor = boxColorRightChar;
                    }
                    else
                        // User put a wrong character in the current guess.
                        isWrongAttempt = true;

                    // Render the 1x1 box
                    int marginX = 2;
                    int marginY = 1;
                    int boxExteriorLength = 3;
                    int currentX = marginX + (boxExteriorLength + 2) * (l - 1);
                    int currentY = marginY + boxExteriorLength * (g - 1);
                    var border = new Border()
                    {
                        Left = currentX,
                        Top = currentY,
                        Width = 3,
                        Height = 1,
                        Color = finalColor,
                    };
                    TextWriterRaw.WriteRaw(border.Render());

                    // Render a character inside it
                    TextWriterWhereColor.WriteWhere(currChar.ToString(), currentX + 2, currentY + 1);
                }
            }
        }

    }

    enum WordleWordDifficulty
    {
        Common,
        Uncommon,
    }
}
