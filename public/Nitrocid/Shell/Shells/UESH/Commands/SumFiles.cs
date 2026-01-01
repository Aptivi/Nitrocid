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

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Files;
using Nitrocid.Drivers;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Terminaux.Writer.FancyWriters;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Drivers.Encryption;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Files.Operations.Querying;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Calculates the sum of files
    /// </summary>
    /// <remarks>
    /// Calculating the hash sum of files is important, because it lets users verify if the file is corrupt or not. It calculates the sum of files using the available algorithms.
    /// </remarks>
    class SumFilesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string folder = FilesystemTools.NeutralizePath(parameters.ArgumentsList[1]);
            string @out = "";
            bool UseRelative = parameters.SwitchesList.Contains("-relative");
            var FileBuilder = new StringBuilder();
            if (parameters.ArgumentsList.Length >= 3)
            {
                @out = FilesystemTools.NeutralizePath(parameters.ArgumentsList[2]);
            }
            if (Checking.FolderExists(folder))
            {
                foreach (string file in Directory.GetFiles(folder, "*", SearchOption.TopDirectoryOnly))
                {
                    string finalFile = FilesystemTools.NeutralizePath(file);
                    SeparatorWriterColor.WriteSeparator(finalFile, true);
                    if (DriverHandler.IsRegistered(DriverTypes.Encryption, parameters.ArgumentsList[0]))
                    {
                        // Time when you're on a breakpoint is counted
                        var spent = new Stopwatch();
                        spent.Start();
                        string encrypted = Encryption.GetEncryptedFile(finalFile, parameters.ArgumentsList[0]);
                        TextWriterColor.Write(encrypted);
                        TextWriterColor.Write(Translate.DoTranslation("Time spent: {0} milliseconds"), spent.ElapsedMilliseconds);
                        if (UseRelative)
                        {
                            FileBuilder.AppendLine($"- {parameters.ArgumentsList[1]}: {encrypted} ({parameters.ArgumentsList[0]})");
                        }
                        else
                        {
                            FileBuilder.AppendLine($"- {finalFile}: {encrypted} ({parameters.ArgumentsList[0]})");
                        }
                        spent.Stop();
                    }
                    else if (parameters.ArgumentsList[0] == "all")
                    {
                        foreach (string driverName in DriverHandler.GetDriverNames<IEncryptionDriver>())
                        {
                            // Time when you're on a breakpoint is counted
                            var spent = new Stopwatch();
                            spent.Start();
                            string encrypted = Encryption.GetEncryptedFile(finalFile, driverName);
                            TextWriterColor.Write($"{driverName}: {encrypted}");
                            TextWriterColor.Write(Translate.DoTranslation("Time spent: {0} milliseconds"), spent.ElapsedMilliseconds);
                            if (UseRelative)
                            {
                                FileBuilder.AppendLine($"- {parameters.ArgumentsList[1]}: {encrypted} ({driverName})");
                            }
                            else
                            {
                                FileBuilder.AppendLine($"- {finalFile}: {encrypted} ({driverName})");
                            }
                            spent.Stop();
                        }
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("Invalid encryption algorithm."), true, KernelColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.Encryption);
                    }
                    TextWriterRaw.Write();
                }
                if (!string.IsNullOrEmpty(@out))
                {
                    var FStream = new StreamWriter(@out);
                    FStream.Write(FileBuilder.ToString());
                    FStream.Flush();
                }
                return 0;
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("{0} is not found."), true, KernelColorType.Error, folder);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Encryption);
            }
        }

    }
}
