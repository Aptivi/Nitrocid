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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Textify.General;
using Terminaux.Inputs;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    /// <summary>
    /// Speed press game module
    /// </summary>
    public static class SpeedPress
    {

        internal static int speedPressTimeout = 3000;

        /// <summary>
        /// Current difficulty for the game
        /// </summary>
        public static SpeedPressDifficulty SpeedPressCurrentDifficulty =>
            (SpeedPressDifficulty)AmusementsInit.AmusementsConfig.SpeedPressCurrentDifficulty;
        /// <summary>
        /// Timeout in milliseconds before declaring that the time is up
        /// </summary>
        public static int SpeedPressTimeout
        {
            get => AmusementsInit.AmusementsConfig.SpeedPressTimeout;
            set => AmusementsInit.AmusementsConfig.SpeedPressTimeout = value < 0 ? 3000 : value;
        }

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
            var SpeedTimeout = 0;
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
            TextWriters.Write(Translate.DoTranslation("Press ESC to exit.") + CharManager.NewLine, true, KernelColorType.Tip);
            while (WrittenChar.Key != ConsoleKey.Escape)
            {
                // Select a random character
                SelectedChar = Convert.ToChar(RandomEngine.Next(97, 122));

                // Prompt user for character
                TextWriterColor.Write(Translate.DoTranslation("Current character:") + " {0}", SelectedChar);
                TextWriters.Write("> ", false, KernelColorType.Input);
                var (result, provided) = Input.ReadKeyTimeout(TimeSpan.FromMilliseconds(SpeedTimeout));
                WrittenChar = result;
                TextWriterRaw.Write();

                // Check to see if the user has pressed the correct character
                if (provided)
                {
                    if (WrittenChar.KeyChar == SelectedChar)
                    {
                        TextWriters.Write(Translate.DoTranslation("You've pressed the right character!"), true, KernelColorType.Success);
                    }
                    else if (WrittenChar.Key != ConsoleKey.Escape)
                    {
                        TextWriters.Write(Translate.DoTranslation("You've pressed the wrong character."), true, KernelColorType.Warning);
                    }
                }
                else
                {
                    TextWriterRaw.Write();
                    TextWriters.Write(Translate.DoTranslation("Character not pressed on time."), true, KernelColorType.Warning);
                }
            }
        }

    }
}
