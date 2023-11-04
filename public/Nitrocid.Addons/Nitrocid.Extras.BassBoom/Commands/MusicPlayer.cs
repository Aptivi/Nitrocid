//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace Nitrocid.Extras.BassBoom.Commands
{
    /// <summary>
    /// Opens an interactive music player
    /// </summary>
    /// <remarks>
    /// This command allows you to play songs interactively.
    /// </remarks>
    class MusicPlayerCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // TODO: Once migrated to .NET 8.0, fill this command.
            TextWriterColor.Write("We're excited to bring to you a music player! However, it will work once we update Nitrocid to .NET 8.0.");
            return 0;
        }

    }
}
