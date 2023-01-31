
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using KS.ConsoleBase.Colors;
using KS.Drivers;
using KS.Drivers.Encryption;
using KS.Files;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Calculates the sum of files
    /// </summary>
    /// <remarks>
    /// Calculating the hash sum of files is important, because it lets users verify if the file is corrupt or not. It calculates the sum of files using either the MD5, SHA1, SHA256, or SHA512 algorithms.
    /// </remarks>
    class SumFilesCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string folder = Filesystem.NeutralizePath(ListArgsOnly[1]);
            string @out = "";
            bool UseRelative = ListSwitchesOnly.Contains("-relative");
            var FileBuilder = new StringBuilder();
            if (!(ListArgsOnly.Length < 3))
            {
                @out = Filesystem.NeutralizePath(ListArgsOnly[2]);
            }
            if (Checking.FolderExists(folder))
            {
                foreach (string file in Directory.EnumerateFiles(folder, "*", SearchOption.TopDirectoryOnly))
                {
                    string finalFile = Filesystem.NeutralizePath(file);
                    SeparatorWriterColor.WriteSeparator(finalFile, true);
                    if (DriverHandler.IsRegistered(DriverTypes.Encryption, ListArgsOnly[0]))
                    {
                        var spent = new Stopwatch();
                        spent.Start(); // Time when you're on a breakpoint is counted
                        string encrypted = Encryption.GetEncryptedFile(finalFile, ListArgsOnly[0]);
                        TextWriterColor.Write(encrypted);
                        TextWriterColor.Write(Translate.DoTranslation("Time spent: {0} milliseconds"), spent.ElapsedMilliseconds);
                        if (UseRelative)
                        {
                            FileBuilder.AppendLine($"- {ListArgsOnly[1]}: {encrypted} ({ListArgsOnly[0]})");
                        }
                        else
                        {
                            FileBuilder.AppendLine($"- {finalFile}: {encrypted} ({ListArgsOnly[0]})");
                        }
                        spent.Stop();
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Invalid encryption algorithm."), true, KernelColorType.Error);
                        break;
                    }
                    TextWriterColor.Write();
                }
                if (!string.IsNullOrEmpty(@out))
                {
                    var FStream = new StreamWriter(@out);
                    FStream.Write(FileBuilder.ToString());
                    FStream.Flush();
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("{0} is not found."), true, KernelColorType.Error, folder);
            }
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"));
            TextWriterColor.Write("  -relative: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Uses relative path instead of absolute"), true, KernelColorType.ListValue);
        }

    }
}
