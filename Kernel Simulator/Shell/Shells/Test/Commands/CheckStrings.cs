using KS.ConsoleBase.Colors;

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

using KS.Files.Read;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Test.Commands
{
    /// <summary>
    /// Checks the specified strings in a separate text file if they exist in the localization files found in the resources of KS (found in the language JSON file)
    /// </summary>
    class Test_CheckStringsCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string TextPath = ListArgsOnly[0];
            var LocalizedStrings = Translate.PrepareDict("eng");
            var Texts = FileRead.ReadContents(TextPath);
            foreach (string Text in Texts)
            {
                if (LocalizedStrings.ContainsKey(Text))
                {
                    TextWriterColor.Write("[+] {0}", true, ColorTools.ColTypes.Success, Text);
                }
                else
                {
                    TextWriterColor.Write("[-] {0}", true, ColorTools.ColTypes.Neutral, Text);
                }
            }
        }

    }
}