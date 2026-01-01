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
    /// Takes you to the snake game
    /// </summary>
    /// <remarks>
    /// This game lets you play the snake game found in old Nokia phones.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Key</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>ARROW UP</term>
    /// <description>Moves your snake up</description>
    /// </item>
    /// <item>
    /// <term>ARROW DOWN</term>
    /// <description>Moves your snake down</description>
    /// </item>
    /// <item>
    /// <term>ARROW LEFT</term>
    /// <description>Moves your snake left</description>
    /// </item>
    /// <item>
    /// <term>ARROW RIGHT</term>
    /// <description>Moves your snake right</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class SnakerCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            Snaker.InitializeSnaker(false);
            ConsoleWrapper.Clear();
            return 0;
        }

    }
}
