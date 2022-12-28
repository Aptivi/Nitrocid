
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs.Styles;
using KS.Drivers.RNG;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using System;

namespace KS.Misc.Games
{
    internal static class RussianRoulette
    {
        internal static void InitializeRoulette()
        {
            // First, tell them to select either true or false
            int bet = SelectionStyle.PromptSelection(Translate.DoTranslation("What's your bet?"), "T/F") - 1;
            if (bet == -2)
                return;

            // Then, compare the value to the randomly selected value for the roulette
            bool unlucky = RandomDriver.RandomRussianRoulette();
            if (Convert.ToBoolean(bet) == unlucky)
                TextWriterColor.Write(Translate.DoTranslation("You guessed it right!"));
            else
                TextWriterColor.Write(Translate.DoTranslation("You got it wrong."));

            // Finally, check if the user is lucky or not
            if (unlucky)
                TextWriterColor.Write(Translate.DoTranslation("But, you're unlucky. Loser."), true, KernelColorType.Warning);
            else
                TextWriterColor.Write(Translate.DoTranslation("You're lucky! Winner!"), true, KernelColorType.Success);
        }
    }
}
