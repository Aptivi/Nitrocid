//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Nitrocid.ConsoleBase.Colors;
using Terminaux.Inputs.Styles.Choice;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Languages;
using System;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    internal static class RussianRoulette
    {
        internal static void InitializeRoulette()
        {
            // First, tell them to select either true or false
            int bet =
                ChoiceStyle.PromptChoice(Translate.DoTranslation("What's your bet?"), [("t", "True"), ("f", "False")]) == "t" ? 1 : 0;

            // Then, compare the value to the randomly selected value for the roulette
            bool unlucky = RandomDriver.RandomRussianRoulette();
            if (Convert.ToBoolean(bet) == unlucky)
                TextWriterColor.Write(Translate.DoTranslation("You guessed it right!"));
            else
                TextWriterColor.Write(Translate.DoTranslation("You got it wrong."));

            // Finally, check if the user is lucky or not
            if (unlucky)
                TextWriters.Write(Translate.DoTranslation("But, you're unlucky. Loser."), true, KernelColorType.Warning);
            else
                TextWriters.Write(Translate.DoTranslation("You're lucky! Winner!"), true, KernelColorType.Success);
        }
    }
}
