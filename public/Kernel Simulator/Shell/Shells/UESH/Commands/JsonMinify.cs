
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.IO;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Beautifiers;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Minifies a JSON file
    /// </summary>
    /// <remarks>
    /// This command parses the JSON file to minify it. It can be wrapped and saved to output file using the command-line redirection.
    /// </remarks>
    class JsonMinifyCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string JsonFile = Filesystem.NeutralizePath(ListArgsOnly[0]);
            string JsonOutputFile;
            string MinifiedJson;

            if (Checking.FileExists(JsonFile))
            {
                // Minify the JSON and display it on screen
                MinifiedJson = JsonMinifier.MinifyJson(JsonFile);
                TextWriterColor.Write(MinifiedJson);

                // Minify it to an output file specified (optional)
                if (ListArgsOnly.Length > 1)
                {
                    JsonOutputFile = Filesystem.NeutralizePath(ListArgsOnly[1]);
                    File.WriteAllText(JsonOutputFile, MinifiedJson);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("File {0} not found."), true, KernelColorType.Error, JsonFile);
            }
        }

    }
}