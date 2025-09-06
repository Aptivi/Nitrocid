//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Extras.HttpShell.Tools;
using Terminaux.Shell.Commands;

namespace Nitrocid.Extras.HttpShell.HTTP.Commands
{
    /// <summary>
    /// Lists headers from the request header
    /// </summary>
    class LsHeaderCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var headers = HttpTools.HttpListHeaders();
            foreach (var header in headers)
            {
                TextWriters.Write("  - {0}: ", false, KernelColorType.ListEntry, header.Item1);
                TextWriters.Write("{0}", true, KernelColorType.ListValue, header.Item2);
            }
            return 0;
        }

    }
}
