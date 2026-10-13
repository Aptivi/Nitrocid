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

using System.Collections.Generic;
using Nitrocid.Base.Kernel.Power;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments.Base;
using Terminaux.Shell.Arguments.Base.Help;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Arguments.CommandLineArguments
{
    class HelpArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(ArgumentParameters parameters)
        {
            Dictionary<string, ArgumentInfo> arguments = new(KernelArguments.AvailableCMDLineArgs);
            foreach (var argument in KernelArguments.OutArgs)
                arguments.Add(argument.Key, argument.Value);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_ARGUMENTS_HELP_LISTING"), true, ThemeColorType.ListTitle);
            ArgumentHelpPrint.ShowArgsHelp(arguments);
            PowerManager.hardShutdown = true;
            PowerManager.KernelShutdown = true;
        }

    }
}
