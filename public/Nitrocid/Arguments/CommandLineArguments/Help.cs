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

using Nitrocid.Languages;
using Nitrocid.Kernel.Power;
using Nitrocid.Arguments.Help;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Arguments.CommandLineArguments
{
    class HelpArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(ArgumentParameters parameters)
        {
            TextWriters.Write(LanguageTools.GetLocalized("NKS_ARGUMENTS_HELP_LISTING"), true, KernelColorType.ListTitle);
            ArgumentHelpPrint.ShowArgsHelp();
            PowerManager.hardShutdown = true;
            PowerManager.KernelShutdown = true;
        }

    }
}
