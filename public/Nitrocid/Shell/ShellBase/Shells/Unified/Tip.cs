//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.MiscWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.ShellBase.Shells.Unified
{
    /// <summary>
    /// Gets a random tip
    /// </summary>
    /// <remarks>
    /// You can learn more about what the kernel can do using this command to get a random tip.
    /// </remarks>
    class TipUnifiedCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string tip = WelcomeMessage.GetRandomTip();
            TextWriterColor.WriteKernelColor(
                "* " + Translate.DoTranslation("Pro tip: Did you know") + " " + tip, true, KernelColorType.Tip);
            return 0;
        }

    }
}
