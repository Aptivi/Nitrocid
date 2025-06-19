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
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Files;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Textify.Tools;

namespace Nitrocid.ShellPacks.Commands
{
    /// <summary>
    /// Beautifies a JSON file
    /// </summary>
    /// <remarks>
    /// This command parses the JSON file to beautify it. It can be wrapped and saved to output file using the command-line redirection.
    /// </remarks>
    class JsonBeautifyCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string JsonFile = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
            string JsonOutputFile;
            string BeautifiedJson;

            if (FilesystemTools.FileExists(JsonFile))
            {
                // Beautify the JSON and display it on screen
                BeautifiedJson = JsonTools.BeautifyJson(JsonFile);
                TextWriterColor.Write(BeautifiedJson);

                // Beautify it to an output file specified (optional)
                if (parameters.ArgumentsList.Length > 1)
                {
                    JsonOutputFile = FilesystemTools.NeutralizePath(parameters.ArgumentsList[1]);
                    FilesystemTools.WriteContentsText(JsonOutputFile, BeautifiedJson);
                }
                variableValue = BeautifiedJson;
                return 0;
            }
            else
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_JSON_FILENOTFOUND"), true, KernelColorType.Error, JsonFile);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.JsonEditor);
            }
        }

    }
}
