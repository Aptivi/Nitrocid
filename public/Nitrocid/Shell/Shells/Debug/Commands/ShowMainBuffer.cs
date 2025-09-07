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

using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel;
using Nitrocid.Languages;
using Terminaux.Base.Extensions;
using Terminaux.Shell.Commands;

namespace Nitrocid.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// Shows the main buffer for five seconds
    /// </summary>
    /// <remarks>
    /// This command will show the main buffer for five seconds
    /// </remarks>
    class ShowMainBufferCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (KernelPlatform.IsOnWindows())
            {
                TextWriters.Write(Translate.DoTranslation("This command is not supported on Windows."), KernelColorType.Error);
                return 33;
            }
            if (KernelEntry.UseAltBuffer)
                ConsoleMisc.PreviewMainBuffer();
            return 0;
        }

    }
}
