
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

using KS.Network.RSS;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using System.Linq;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Opens an RSS shell
    /// </summary>
    /// <remarks>
    /// You can interact with the RSS shell to connect to a feed server and interact with them.
    /// </remarks>
    class RssCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (ListArgsOnly.Length > 0)
                ShellStart.StartShell(ShellType.RSSShell, ListArgsOnly[0]);
            else if (ListSwitchesOnly.Length > 0 && ListSwitchesOnly.Contains("-m"))
                ShellStart.StartShell(ShellType.RSSShell);
            else
                ShellStart.StartShell(ShellType.RSSShell, RSSTools.RssHeadlineUrl);
        }

    }
}
