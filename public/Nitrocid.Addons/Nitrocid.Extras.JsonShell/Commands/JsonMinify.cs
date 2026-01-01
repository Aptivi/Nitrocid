//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Nitrocid.Files.Operations;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Textify.Tools;

namespace Nitrocid.Extras.JsonShell.Commands
{
    /// <summary>
    /// Minifies a JSON file
    /// </summary>
    /// <remarks>
    /// This command parses the JSON file to minify it. It can be wrapped and saved to output file using the command-line redirection.
    /// </remarks>
    class JsonMinifyCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string JsonFile = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
            string JsonOutputFile;
            string MinifiedJson;

            if (Checking.FileExists(JsonFile))
            {
                // Minify the JSON and display it on screen
                MinifiedJson = JsonTools.MinifyJson(JsonFile);
                TextWriterColor.Write(MinifiedJson);

                // Minify it to an output file specified (optional)
                if (parameters.ArgumentsList.Length > 1)
                {
                    JsonOutputFile = FilesystemTools.NeutralizePath(parameters.ArgumentsList[1]);
                    Writing.WriteContentsText(JsonOutputFile, MinifiedJson);
                }
                variableValue = MinifiedJson;
                return 0;
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("File {0} not found."), true, KernelColorType.Error, JsonFile);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.JsonEditor);
            }
        }

    }
}
