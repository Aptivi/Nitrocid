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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files.Operations;
using Nitrocid.Shell.ShellBase.Commands;
using Textify.Tools;

namespace Nitrocid.Extras.JsonShell.Commands
{
    /// <summary>
    /// Shows a difference between two JSON files
    /// </summary>
    class JsonDiffCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var source = JToken.Parse(Reading.ReadContentsText(parameters.ArgumentsList[0]));
            var target = JToken.Parse(Reading.ReadContentsText(parameters.ArgumentsList[1]));
            var diff = JsonTools.FindDifferences(source, target);
            TextWriters.Write(diff.ToString(Formatting.Indented), KernelColorType.NeutralText);
            return 0;
        }
    }
}
