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

using Nitrocid.Extras.Amusements.Amusements.Games;
using Nitrocid.Shell.ShellBase.Commands;
using Terminaux.Base;

namespace Nitrocid.Extras.Amusements.Commands
{
    /// <summary>
    /// Takes you to the ping-pong game
    /// </summary>
    /// <remarks>
    /// This game lets you play the ping-pong game.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Key</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>ARROW UP</term>
    /// <description>Player 1: Moves your pad up</description>
    /// </item>
    /// <item>
    /// <term>ARROW DOWN</term>
    /// <description>Player 1: Moves your pad down</description>
    /// </item>
    /// <item>
    /// <term>W</term>
    /// <description>Player 2: Moves your pad up</description>
    /// </item>
    /// <item>
    /// <term>S</term>
    /// <description>Player 2: Moves your pad down</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class PongCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            Pong.InitializePong();
            ConsoleWrapper.Clear();
            return 0;
        }

    }
}
