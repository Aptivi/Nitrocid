//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
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
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Drivers.Encryption;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

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
        public override string Command => 
            "sumfile";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "algorithm/all", new CommandArgumentPartOptions()
                    {
                        AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames(),
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_ALGORITHM_DESC"
                    }),
                    new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_FILE_DESC"
                    }),
                    new CommandArgumentPart(false, "outputFile", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_OUTPUTFILE_DESC"
                    }),
                ],
                [
                    new SwitchInfo("relative", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_SWITCH_RELATIVE_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string algorithm = parameters.ArgumentsList[0];
            string relativeFile = parameters.ArgumentsList[1];
            string file = FilesystemTools.NeutralizePath(relativeFile);
            string @out = parameters.ArgumentsList.Length >= 3 ? FilesystemTools.NeutralizePath(parameters.ArgumentsList[2]) : "";
            bool useRelative = parameters.ContainsSwitch("-relative");
            var fileBuilder = new StringBuilder();
            if (FilesystemTools.FileExists(file))
            {
                if (DriverHandler.IsRegistered(DriverTypes.Encryption, algorithm))
                    fileBuilder.AppendLine(ProcessEncryptionDriver(algorithm, file, relativeFile, useRelative));
                else if (parameters.ArgumentsList[0] == "all")
                {
                    foreach (string driverName in DriverHandler.GetDriverNames<IEncryptionDriver>())
                        fileBuilder.AppendLine(ProcessEncryptionDriver(driverName, file, relativeFile, useRelative));
                }
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_VERIFY_ALGORITHMINVALID"), true, ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Encryption);
                }
                if (!string.IsNullOrEmpty(@out))
                {
                    var fileStream = new StreamWriter(@out);
                    fileStream.Write(fileBuilder.ToString());
                    fileStream.Flush();
                }
                return 0;
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_VERIFY_NOTFOUND"), true, ThemeColorType.Error, file);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Encryption);
            }
        }

        private string ProcessEncryptionDriver(string driverName, string file, string relativeFile, bool useRelative)
        {
            if (DriverHandler.IsRegistered(DriverTypes.Encryption, driverName))
            {
                // Time when you're on a breakpoint is counted
                var spent = new Stopwatch();
                spent.Start();
                string encrypted = Encryption.GetEncryptedFile(file, driverName);
                TextWriterColor.Write(encrypted);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_VERIFY_TIMESPENT"), spent.ElapsedMilliseconds);
                spent.Stop();
                if (useRelative)
                    return $"- {relativeFile}: {encrypted} ({driverName})";
                return $"- {file}: {encrypted} ({driverName})";
            }
            return "";
        }

    }
}
