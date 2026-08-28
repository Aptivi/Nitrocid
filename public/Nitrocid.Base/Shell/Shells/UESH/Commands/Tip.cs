//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Base.Kernel.Starting;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Gets a random tip
    /// </summary>
    /// <remarks>
    /// You can learn more about what the kernel can do using this command to get a random tip.
    /// </remarks>
    class TipCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "tip";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_TIP_DESC");

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string tip = WelcomeMessage.GetRandomTip();
            TextWriterColor.Write(
                "* " + LanguageTools.GetLocalized("NKS_KERNEL_STARTING_TIPS_PREFIX") + " " + tip, true, ThemeColorType.Tip);
            return 0;
        }

    }
}
