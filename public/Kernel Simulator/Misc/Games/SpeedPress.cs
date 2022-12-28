
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.Misc.Games
{
    /// <summary>
    /// Speed press game module
    /// </summary>
    public static class SpeedPress
    {

        /// <summary>
        /// Current difficulty for the game
        /// </summary>
        public static SpeedPressDifficulty SpeedPressCurrentDifficulty = SpeedPressDifficulty.Medium;
        /// <summary>
        /// Timeout in milliseconds before declaring that the time is up
        /// </summary>
        public static int SpeedPressTimeout = 3000;

        /// <summary>
        /// SpeedPress difficulty
        /// </summary>
        public enum SpeedPressDifficulty
        {
            /// <summary>
            /// Easy difficulty (timeout for three seconds)
            /// </summary>
            Easy,
            /// <summary>
            /// Medium difficulty (timeout for a second and a half)
            /// </summary>
            Medium,
            /// <summary>
            /// Hard difficulty (timeout for a half second)
            /// </summary>
            Hard,
            /// <summary>
            /// Very hard difficulty (timeout for a quarter of a second)
            /// </summary>
            VeryHard,
            /// <summary>
            /// Custom difficulty (custom timeout according to either a switch or the kernel settings)
            /// </summary>
            Custom
        }

        /// <summary>
        /// Initializes the SpeedPress game
        /// </summary>
        /// <param name="Difficulty">The difficulty of the game</param>
        /// <param name="CustomTimeout">Custom game timeout</param>
        public static void InitializeSpeedPress(SpeedPressDifficulty Difficulty, int CustomTimeout = 0)
        {
            var SpeedTimeout = default(int);
            char SelectedChar;
            var WrittenChar = default(ConsoleKeyInfo);
            var RandomEngine = new Random();

            // Change timeout based on difficulty
            switch (Difficulty)
            {
                case SpeedPressDifficulty.Easy:
                    {
                        SpeedTimeout = 3000;
                        break;
                    }
                case SpeedPressDifficulty.Medium:
                    {
                        SpeedTimeout = 1500;
                        break;
                    }
                case SpeedPressDifficulty.Hard:
                    {
                        SpeedTimeout = 500;
                        break;
                    }
                case SpeedPressDifficulty.VeryHard:
                    {
                        SpeedTimeout = 250;
                        break;
                    }
                case SpeedPressDifficulty.Custom:
                    {
                        if (CustomTimeout > 0)
                        {
                            SpeedTimeout = Math.Abs(CustomTimeout);
                        }
                        else if (SpeedPressTimeout > 0)
                        {
                            SpeedTimeout = Math.Abs(SpeedPressTimeout);
                        }
                        else
                        {
                            SpeedTimeout = 1500;
                        }

                        break;
                    }
            }

            // Enter the loop until the user presses ESC
            TextWriterColor.Write(Translate.DoTranslation("Press ESC to exit.") + CharManager.NewLine, true, KernelColorType.Tip);
            while (!(WrittenChar.Key == ConsoleKey.Escape) | !(WrittenChar.Modifiers == ConsoleModifiers.Control) & WrittenChar.Key == ConsoleKey.C)
            {
                // Select a random character
                SelectedChar = Convert.ToChar(RandomEngine.Next(97, 122));

                // Prompt user for character
                try
                {
                    TextWriterColor.Write(Translate.DoTranslation("Current character:") + " {0}", SelectedChar);
                    TextWriterColor.Write("> ", false, KernelColorType.Input);
                    WrittenChar = Input.ReadKeyTimeout(false, TimeSpan.FromMilliseconds(SpeedTimeout));
                    TextWriterColor.Write();

                    // Check to see if the user has pressed the correct character
                    if (WrittenChar.KeyChar == SelectedChar)
                    {
                        TextWriterColor.Write(Translate.DoTranslation("You've pressed the right character!"), true, KernelColorType.Success);
                    }
                    else if (!(WrittenChar.Key == ConsoleKey.Escape) | !(WrittenChar.Modifiers == ConsoleModifiers.Control) & WrittenChar.Key == ConsoleKey.C)
                    {
                        TextWriterColor.Write(Translate.DoTranslation("You've pressed the wrong character."), true, KernelColorType.Warning);
                    }
                }
                catch (KernelException kex) when (kex.ExceptionType == KernelExceptionType.ConsoleReadTimeout)
                {
                    TextWriterColor.Write();
                    TextWriterColor.Write(Translate.DoTranslation("Character not pressed on time."), true, KernelColorType.Warning);
                }
            }
        }

    }
}
