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
using System.Text;
using System.Threading;
using Terminaux.Inputs;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Terminaux.Colors;
using Textify.Words;
using Terminaux.Base;
using Terminaux.Reader;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    static class Hangman
    {

        /// <summary>
        /// Initializes the game
        /// </summary>
        public static void InitializeHangman(HangmanDifficulty difficulty)
        {
            // Get a random word and populate some variables
            string RandomWord = WordManager.GetRandomWord();
            bool hung = false;
            int currentAttempt = 0;
            List<char> gotChars = [];
            List<char> wrongChars = [];

            while (!hung)
            {
                ConsoleWrapper.CursorVisible = false;
                ConsoleWrapper.Clear();

                // Draw how many underscores to place
                var underscoresRender = new StringBuilder();
                for (int i = 0; i < RandomWord.Length; i++)
                    underscoresRender.Append(gotChars.Contains(RandomWord[i]) ? $"{RandomWord[i]} " : "_ ");
                int underscoresPosX = ConsoleWrapper.WindowWidth / 2 - underscoresRender.Length / 2;
                int underscoresPosY = ConsoleWrapper.WindowHeight - 2;
                Color underscoresSeq = new(RandomDriver.Random(255), RandomDriver.Random(255), RandomDriver.Random(255));
                DebugWriter.WriteDebug(DebugLevel.I, "{0} cells for underscore length", underscoresRender.Length);
                TextWriterWhereColor.WriteWhereColor(underscoresRender.ToString(), underscoresPosX, underscoresPosY, underscoresSeq);

                // Draw the wrong characters written
                string wrongRender = string.Join(" ", wrongChars);
                int wrongPosX = ConsoleWrapper.WindowWidth / 2 - wrongRender.Length / 2;
                int wrongPosY = ConsoleWrapper.WindowHeight - 4;
                Color wrongSeq = new(RandomDriver.Random(255), RandomDriver.Random(255), RandomDriver.Random(255));
                DebugWriter.WriteDebug(DebugLevel.I, "{0} cells for {1} wrong letters", wrongRender.Length, wrongChars.Count);
                TextWriterWhereColor.WriteWhereColor(wrongRender.ToString(), wrongPosX, wrongPosY, wrongSeq);

                // Draw the hanger
                string hangerSprite =
                    @" /---------\" + "\n" +
                    @" |         |" + "\n" +
                    @" |         |" + "\n" +
                    @" |          " + "\n" +
                    @" |          " + "\n" +
                    @" |          " + "\n" +
                    @" |          " + "\n" +
                    @" |          " + "\n" +
                    @"---         " + "\n"
                ;
                int hangerPosX = 3;
                int hangerPosY = 2;
                Color hangerSeq = new(RandomDriver.Random(255), RandomDriver.Random(255), RandomDriver.Random(255));
                TextWriterWhereColor.WriteWhereColor(hangerSprite, hangerPosX, hangerPosY, hangerSeq);

                // Now, draw the hung man based on attempts
                if (currentAttempt >= 1)
                {
                    // Draw the head
                    int headPosX = 14;
                    int headPosY = 5;
                    TextWriterWhereColor.WriteWhereColor("*", headPosX, headPosY, hangerSeq);
                }
                if (currentAttempt >= 2)
                {
                    // Draw the body
                    int bodyPosX = 14;
                    int bodyPosY = 6;
                    TextWriterWhereColor.WriteWhereColor("|", bodyPosX, bodyPosY, hangerSeq);
                    TextWriterWhereColor.WriteWhereColor("|", bodyPosX, bodyPosY + 1, hangerSeq);
                }
                if (currentAttempt >= 3)
                {
                    // Draw the first hand
                    int handPosX = 13;
                    int handPosY = 6;
                    TextWriterWhereColor.WriteWhereColor("-", handPosX, handPosY, hangerSeq);
                }
                if (currentAttempt >= 4)
                {
                    // Draw the second hand
                    int handPosX = 15;
                    int handPosY = 6;
                    TextWriterWhereColor.WriteWhereColor("-", handPosX, handPosY, hangerSeq);
                }
                if (currentAttempt >= 5)
                {
                    // Draw the first leg
                    int legPosX = 13;
                    int legPosY = 8;
                    TextWriterWhereColor.WriteWhereColor("/", legPosX, legPosY, hangerSeq);
                }
                if (currentAttempt >= 6)
                {
                    // Draw the second leg
                    int legPosX = 15;
                    int legPosY = 8;
                    TextWriterWhereColor.WriteWhereColor("\\", legPosX, legPosY, hangerSeq);
                    hung = true;
                }

                // Check to see if we won the game
                bool won = false;
                List<char> queried = [];
                foreach (char ch in RandomWord)
                {
                    if (gotChars.Contains(ch))
                        queried.Add(ch);
                    if (string.Join("", queried) == RandomWord)
                        won = true;
                }

                // See if the man is completely hung
                if (hung)
                {
                    // Lost the game
                    string hungStr = Translate.DoTranslation("You're hung!");
                    int hungPosX = ConsoleWrapper.WindowWidth / 2 - hungStr.Length / 2;
                    int hungPosY = ConsoleWrapper.WindowHeight - 6;
                    TextWriterWhereColor.WriteWhereColor(hungStr, hungPosX, hungPosY, hangerSeq);
                    Thread.Sleep(5000);
                }
                else if (won)
                {
                    // Won the game
                    hung = true;
                    string wonStr = Translate.DoTranslation("You win!");
                    int wonPosX = ConsoleWrapper.WindowWidth / 2 - wonStr.Length / 2;
                    int wonPosY = ConsoleWrapper.WindowHeight - 6;
                    TextWriterWhereColor.WriteWhereColor(wonStr, wonPosX, wonPosY, hangerSeq);
                    Thread.Sleep(5000);
                }
                else
                {
                    // Wait for character
                    var input = TermReader.ReadKey();
                    var inputChar = input.KeyChar;
                    if (RandomWord.Contains(inputChar))
                    {
                        // Right character! Add it to the right characters list
                        if (!gotChars.Contains(inputChar))
                            gotChars.Add(inputChar);
                    }
                    else if (input.Key == ConsoleKey.Escape)
                    {
                        // Escaping.
                        hung = true;
                    }
                    else
                    {
                        // Wrong character!
                        if (!wrongChars.Contains(inputChar))
                        {
                            // Increase the number of attempts based on the difficulty
                            if (difficulty == HangmanDifficulty.Hardcore)
                                currentAttempt = 6;
                            else if (difficulty == HangmanDifficulty.None)
                                currentAttempt++;
                            wrongChars.Add(inputChar);
                        }
                    }
                }
            }

            // Clear after ourselves
            ConsoleWrapper.CursorVisible = true;
            ConsoleWrapper.Clear();
        }

    }

    enum HangmanDifficulty
    {
        None = 0,
        Hardcore,
        Practice
    }
}
