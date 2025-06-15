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

using System;
using System.Collections.Generic;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Terminaux.Colors;
using Textify.Data.Words;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    static class Hangman
    {

        /// <summary>
        /// Initializes the game
        /// </summary>
        public static void InitializeHangman(HangmanDifficulty difficulty, HangmanWordDifficulty wordDifficulty = HangmanWordDifficulty.Common)
        {
            // Clear the screen
            ConsoleWrapper.CursorVisible = false;
            KernelColorTools.LoadBackground();

            // Get a random word and populate some variables
            string RandomWord = WordManager.GetRandomWord(wordDifficulty == HangmanWordDifficulty.Uncommon ? WordDataType.Words : WordDataType.CommonWords);
            bool hung = false;
            int currentAttempt = 0;
            List<char> gotChars = [];
            List<char> wrongChars = [];

            // Add a buffer
            Screen screen = new();
            ScreenPart part = new();

            // Add a UI rendering logic
            Color hangerSeq = new(RandomDriver.Random(255), RandomDriver.Random(255), RandomDriver.Random(255));
            Color wrongSeq = new(RandomDriver.Random(255), RandomDriver.Random(255), RandomDriver.Random(255));
            Color underscoresSeq = new(RandomDriver.Random(255), RandomDriver.Random(255), RandomDriver.Random(255));
            part.AddDynamicText(() =>
            {
                StringBuilder builder = new();

                // Draw how many underscores to place
                var underscoresRender = new StringBuilder();
                for (int i = 0; i < RandomWord.Length; i++)
                    underscoresRender.Append(gotChars.Contains(RandomWord[i]) ? $"{RandomWord[i]} " : "_ ");
                int underscoresPosX = ConsoleWrapper.WindowWidth / 2 - underscoresRender.Length / 2;
                int underscoresPosY = ConsoleWrapper.WindowHeight - 2;
                DebugWriter.WriteDebug(DebugLevel.I, "{0} cells for underscore length", vars: [underscoresRender.Length]);
                builder.Append(
                    TextWriterWhereColor.RenderWhereColor(underscoresRender.ToString(), underscoresPosX, underscoresPosY, underscoresSeq)
                );

                // Draw the wrong characters written
                string wrongRender = string.Join(" ", wrongChars);
                int wrongPosX = ConsoleWrapper.WindowWidth / 2 - wrongRender.Length / 2;
                int wrongPosY = ConsoleWrapper.WindowHeight - 4;
                DebugWriter.WriteDebug(DebugLevel.I, "{0} cells for {1} wrong letters", vars: [wrongRender.Length, wrongChars.Count]);
                builder.Append(
                    TextWriterWhereColor.RenderWhereColor(wrongRender.ToString(), wrongPosX, wrongPosY, wrongSeq)
                );

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
                builder.Append(
                    TextWriterWhereColor.RenderWhereColor(hangerSprite, hangerPosX, hangerPosY, hangerSeq)
                );

                // Now, draw the hung man based on attempts
                if (currentAttempt >= 1)
                {
                    // Draw the head
                    int headPosX = 14;
                    int headPosY = 5;
                    builder.Append(
                        TextWriterWhereColor.RenderWhereColor("*", headPosX, headPosY, hangerSeq)
                    );
                }
                if (currentAttempt >= 2)
                {
                    // Draw the body
                    int bodyPosX = 14;
                    int bodyPosY = 6;
                    builder.Append(
                        TextWriterWhereColor.RenderWhereColor("|", bodyPosX, bodyPosY, hangerSeq) +
                        TextWriterWhereColor.RenderWhereColor("|", bodyPosX, bodyPosY + 1, hangerSeq)
                    );
                }
                if (currentAttempt >= 3)
                {
                    // Draw the first hand
                    int handPosX = 13;
                    int handPosY = 6;
                    builder.Append(
                        TextWriterWhereColor.RenderWhereColor("-", handPosX, handPosY, hangerSeq)
                    );
                }
                if (currentAttempt >= 4)
                {
                    // Draw the second hand
                    int handPosX = 15;
                    int handPosY = 6;
                    builder.Append(
                        TextWriterWhereColor.RenderWhereColor("-", handPosX, handPosY, hangerSeq)
                    );
                }
                if (currentAttempt >= 5)
                {
                    // Draw the first leg
                    int legPosX = 13;
                    int legPosY = 8;
                    builder.Append(
                        TextWriterWhereColor.RenderWhereColor("/", legPosX, legPosY, hangerSeq)
                    );
                }
                if (currentAttempt >= 6)
                {
                    // Draw the second leg
                    int legPosX = 15;
                    int legPosY = 8;
                    builder.Append(
                        TextWriterWhereColor.RenderWhereColor("\\", legPosX, legPosY, hangerSeq)
                    );
                    hung = true;
                }

                // Return the result
                return builder.ToString();
            });
            screen.AddBufferedPart("Hangman - UI", part);
            ScreenTools.SetCurrent(screen);

            // Main game loop
            while (!hung)
            {
                // Render it
                ScreenTools.Render();

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
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_AMUSEMENTS_HANGMAN_YOULOSE"));
                }
                else if (won)
                {
                    // Won the game
                    hung = true;
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_AMUSEMENTS_HANGMAN_YOUWIN"));
                }
                else
                {
                    // Wait for character
                    var input = Input.ReadKey();
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
            ScreenTools.UnsetCurrent(screen);
            ConsoleWrapper.CursorVisible = true;
            KernelColorTools.LoadBackground();
        }

    }

    enum HangmanDifficulty
    {
        None = 0,
        Hardcore,
        Practice
    }

    enum HangmanWordDifficulty
    {
        Common,
        Uncommon,
    }
}
