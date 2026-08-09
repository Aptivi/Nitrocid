//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
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
using Terminaux.Shell.Shells;
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

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var difficulty =
                parameters.ContainsSwitch("-e") ? SpeedPress.SpeedPressDifficulty.Easy :
                parameters.ContainsSwitch("-h") ? SpeedPress.SpeedPressDifficulty.Hard :
                parameters.ContainsSwitch("-v") ? SpeedPress.SpeedPressDifficulty.VeryHard :
                parameters.ContainsSwitch("-c") ? SpeedPress.SpeedPressDifficulty.Custom :
                SpeedPress.SpeedPressDifficulty.Medium;

            // Set up custom timeout
            int customTimeout = SpeedPress.SpeedPressTimeout;
            if (difficulty == SpeedPress.SpeedPressDifficulty.Custom)
            {
                string customTimeoutStr = parameters.GetSwitchValue("-c");
                customTimeout = int.Parse(customTimeoutStr);
            }

            // Initialize the game
            SpeedPress.InitializeSpeedPress(difficulty, customTimeout);
            return 0;
        }

    }
}
