
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Linq;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;
using Nitrocid.Extras.Amusements.Amusements.Games;

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

        public override int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            var Difficulty = SpeedPress.SpeedPressDifficulty.Medium;
            int CustomTimeout = SpeedPress.SpeedPressTimeout;
            if (ListSwitchesOnly.Contains("-e"))
                Difficulty = SpeedPress.SpeedPressDifficulty.Easy;
            if (ListSwitchesOnly.Contains("-m"))
                Difficulty = SpeedPress.SpeedPressDifficulty.Medium;
            if (ListSwitchesOnly.Contains("-h"))
                Difficulty = SpeedPress.SpeedPressDifficulty.Hard;
            if (ListSwitchesOnly.Contains("-v"))
                Difficulty = SpeedPress.SpeedPressDifficulty.VeryHard;
            if (ListSwitchesOnly.Contains("-c") & ListArgsOnly.Length > 0 && TextTools.IsStringNumeric(ListArgsOnly[0]))
            {
                Difficulty = SpeedPress.SpeedPressDifficulty.Custom;
                CustomTimeout = Convert.ToInt32(ListArgsOnly[0]);
            }
            SpeedPress.InitializeSpeedPress(Difficulty, CustomTimeout);
            return 0;
        }

    }
}
