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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files;
using Nitrocid.Files.LineEndings;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Converts the line endings
    /// </summary>
    /// <remarks>
    /// If you have a text file that needs a change for its line endings, you can use this command to convert the line endings to your platform's format, or the format of your choice by using these switches:
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-w</term>
    /// <description>Converts the line endings to the Windows format (CR + LF)</description>
    /// </item>
    /// <item>
    /// <term>-u</term>
    /// <description>Converts the line endings to the Unix format (LF)</description>
    /// </item>
    /// <item>
    /// <term>-m</term>
    /// <description>Converts the line endings to the Mac OS 9 format (CR)</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class ConvertLineEndingsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string TargetTextFile = parameters.ArgumentsList[0];
            var TargetLineEnding = FilesystemTools.NewlineStyle;
            bool force = false;
            if (parameters.SwitchesList.Length != 0)
            {
                if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-w"))
                    TargetLineEnding = FilesystemNewlineStyle.CRLF;
                if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-u"))
                    TargetLineEnding = FilesystemNewlineStyle.LF;
                if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-m"))
                    TargetLineEnding = FilesystemNewlineStyle.CR;
                if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-force"))
                    force = true;
            }

            // Convert the line endings
            if (FilesystemTools.IsBinaryFile(TargetTextFile) && !force)
            {
                TextWriters.Write(Translate.DoTranslation("Can't convert line endings on a binary file since it results in file corruption."), true, KernelColorType.Error);
                return 7;
            }
            FilesystemTools.ConvertLineEndings(TargetTextFile, TargetLineEnding, force);
            return 0;
        }

    }
}
