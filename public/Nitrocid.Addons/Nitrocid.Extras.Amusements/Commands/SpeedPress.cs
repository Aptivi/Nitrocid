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

using System;
using Nitrocid.Extras.Amusements.Amusements.Games;
using Terminaux.Shell.Commands;
using Textify.General;

namespace Nitrocid.Extras.Amusements.Commands
{
    /// <summary>
    /// Launches the speed press game
    /// </summary>
    /// <remarks>
    /// This game will test your keystroke speed. It will only give you very little time to press a key before moving to the next one.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-e</term>
    /// <description>Easy</description>
    /// </item>
    /// <item>
    /// <term>-m</term>
    /// <description>Medium</description>
    /// </item>
    /// <item>
    /// <term>-h</term>
    /// <description>Hard</description>
    /// </item>
    /// <item>
    /// <term>-v</term>
    /// <description>Very Hard</description>
    /// </item>
    /// <item>
    /// <term>-c</term>
    /// <description>Custom. The timeout should be specified in milliseconds.</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class SpeedPressCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var Difficulty = SpeedPress.SpeedPressDifficulty.Medium;
            int CustomTimeout = SpeedPress.SpeedPressTimeout;
            if (parameters.ContainsSwitch("-e"))
                Difficulty = SpeedPress.SpeedPressDifficulty.Easy;
            if (parameters.ContainsSwitch("-m"))
                Difficulty = SpeedPress.SpeedPressDifficulty.Medium;
            if (parameters.ContainsSwitch("-h"))
                Difficulty = SpeedPress.SpeedPressDifficulty.Hard;
            if (parameters.ContainsSwitch("-v"))
                Difficulty = SpeedPress.SpeedPressDifficulty.VeryHard;
            if (parameters.ContainsSwitch("-c") & parameters.ArgumentsList.Length > 0 && TextTools.IsStringNumeric(parameters.ArgumentsList[0]))
            {
                Difficulty = SpeedPress.SpeedPressDifficulty.Custom;
                CustomTimeout = Convert.ToInt32(parameters.ArgumentsList[0]);
            }
            SpeedPress.InitializeSpeedPress(Difficulty, CustomTimeout);
            return 0;
        }

    }
}
