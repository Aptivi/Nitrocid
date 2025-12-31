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

using System.Diagnostics;
using System.IO;
using System.Text;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Files;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Drivers.Encryption;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Calculates the sum of a file
    /// </summary>
    /// <remarks>
    /// Calculating the hash sum of files is important, because it lets users verify if the file is corrupt or not. It calculates the sum of a file using the available algorithms.
    /// </remarks>
    class SumFileCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string file = FilesystemTools.NeutralizePath(parameters.ArgumentsList[1]);
            string @out = "";
            bool UseRelative = parameters.ContainsSwitch("-relative");
            var FileBuilder = new StringBuilder();
            if (parameters.ArgumentsList.Length >= 3)
            {
                @out = FilesystemTools.NeutralizePath(parameters.ArgumentsList[2]);
            }
            if (FilesystemTools.FileExists(file))
            {
                if (DriverHandler.IsRegistered(DriverTypes.Encryption, parameters.ArgumentsList[0]))
                {
                    // Time when you're on a breakpoint is counted
                    var spent = new Stopwatch();
                    spent.Start();
                    string encrypted = Encryption.GetEncryptedFile(file, parameters.ArgumentsList[0]);
                    TextWriterColor.Write(encrypted);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_VERIFY_TIMESPENT"), spent.ElapsedMilliseconds);
                    if (UseRelative)
                    {
                        FileBuilder.AppendLine($"- {parameters.ArgumentsList[1]}: {encrypted} ({parameters.ArgumentsList[0]})");
                    }
                    else
                    {
                        FileBuilder.AppendLine($"- {file}: {encrypted} ({parameters.ArgumentsList[0]})");
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
                        string encrypted = Encryption.GetEncryptedFile(file, driverName);
                        TextWriterColor.Write($"{driverName}: {encrypted}");
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_VERIFY_TIMESPENT"), spent.ElapsedMilliseconds);
                        if (UseRelative)
                        {
                            FileBuilder.AppendLine($"- {parameters.ArgumentsList[1]}: {encrypted} ({driverName})");
                        }
                        else
                        {
                            FileBuilder.AppendLine($"- {file}: {encrypted} ({driverName})");
                        }
                        spent.Stop();
                    }
                }
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_VERIFY_ALGORITHMINVALID"), true, ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Encryption);
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
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_VERIFY_NOTFOUND"), true, ThemeColorType.Error, file);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Encryption);
            }
        }

    }
}
