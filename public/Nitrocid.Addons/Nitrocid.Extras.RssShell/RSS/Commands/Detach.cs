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

using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;

namespace Nitrocid.Extras.RssShell.RSS.Commands
{
    /// <summary>
    /// Detaches the shell from the current working server
    /// </summary>
    /// <remarks>
    /// If you want to detach the shell from the current working server, but don't want to disconnect from it, use this command.
    /// <br></br>
    /// </remarks>
    class DetachCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var shell = (RSSShell?)ShellManager.ShellStack[^1].ShellBase ??
                throw new KernelException(KernelExceptionType.RSSShell, Translate.DoTranslation("The last shell is not this shell to detach from."));
            shell.detaching = true;
            ShellManager.KillShell();
            return 0;
        }

    }
}
