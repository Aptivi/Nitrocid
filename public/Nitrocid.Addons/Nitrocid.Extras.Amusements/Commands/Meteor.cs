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

namespace Nitrocid.Extras.Amusements.Commands
{
    /// <summary>
    /// You're the spaceship and the meteors are destroying you. Can you save it?
    /// </summary>
    /// <remarks>
    /// This command runs a game that lets you shoot all the meteors coming to your spaceship. You must shoot all the meteors before one of them destroys your spaceship!
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Key</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>ARROW UP</term>
    /// <description>Moves your spaceship up</description>
    /// </item>
    /// <item>
    /// <term>ARROW DOWN</term>
    /// <description>Moves your spaceship down</description>
    /// </item>
    /// <item>
    /// <term>SPACE</term>
    /// <description>Fire your laser</description>
    /// </item>
    /// <item>
    /// <term>ESC</term>
    /// <description>Exit game</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class MeteorCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            MeteorShooter.InitializeMeteor();
            return 0;
        }
    }
}
