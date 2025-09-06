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

using System;
using Nitrocid.Files.Editors.HexEdit;
using Terminaux.Shell.Commands;

namespace Nitrocid.Shell.Shells.Hex.Commands
{
    /// <summary>
    /// Adds a new byte at the end of the file
    /// </summary>
    /// <remarks>
    /// You can use this command to add a new byte at the end of the file.
    /// </remarks>
    class AddByteCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            byte ByteContent = Convert.ToByte(parameters.ArgumentsText, 16);
            HexEditTools.AddNewByte(ByteContent);
            return 0;
        }

    }
}
