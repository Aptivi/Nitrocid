﻿//
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

using System.Linq;
using Terminaux.Shell.Arguments.Base;
using Terminaux.Shell.Arguments.Base.Help;

namespace Nitrocid.Locales.Actions.Arguments
{
    internal class HelpArgument : ArgumentExecutor, IArgument
    {
        public override void Execute(ArgumentParameters parameters)
        {
            var arguments = EntryPoint.argumentsMain.ToDictionary((ai) => ai.Argument, (ai) => ai);
            if (parameters.ArgumentsList.Length > 0)
                ArgumentHelpPrint.ShowArgsHelp(parameters.ArgumentsList[0], arguments);
            else
                ArgumentHelpPrint.ShowArgsHelp(arguments);
        }
    }
}
