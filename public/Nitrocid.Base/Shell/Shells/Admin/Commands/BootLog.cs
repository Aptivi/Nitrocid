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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Misc.Splash;

namespace Nitrocid.Base.Shell.Shells.Admin.Commands
{
    /// <summary>
    /// Gets the current boot log
    /// </summary>
    /// <remarks>
    /// This command gets the current boot log from the splash report buffer.
    /// </remarks>
    class BootLogCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var logLines = SplashReport.LogBuffer;
            foreach (var line in logLines)
            {
                var finalColor =
                    line.Severity == SplashReportSeverity.Error ? ThemeColorType.Error :
                    line.Severity == SplashReportSeverity.Warning ? ThemeColorType.Warning :
                    ThemeColorType.NeutralText;
                TextWriterColor.Write($"[{line.Time}] [{line.Progress}%] [{line.Severity}] : ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(line.RenderedMessage, true, finalColor);
            }
            return 0;
        }
    }
}
