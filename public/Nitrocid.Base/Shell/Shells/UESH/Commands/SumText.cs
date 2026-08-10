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
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Drivers.Encryption;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Calculates the sum of a text
    /// </summary>
    /// <remarks>
    /// It calculates the sum of a text using the available algorithms.
    /// </remarks>
    class SumTextCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "sumtext";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUMTEXT_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "algorithm/all", new CommandArgumentPartOptions()
                    {
                        AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames(),
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_ALGORITHM_DESC"
                    }),
                    new CommandArgumentPart(true, "text", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMTEXT_ARGUMENT_TEXT_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string driverName = parameters.ArgumentsList[0];
            string text = parameters.ArgumentsList[1];
            if (DriverHandler.IsRegistered(DriverTypes.Encryption, driverName))
                ProcessEncryptionDriver(driverName, text);
            else if (parameters.ArgumentsList[0] == "all")
            {
                foreach (string targetDriverName in DriverHandler.GetDriverNames<IEncryptionDriver>())
                    ProcessEncryptionDriver(targetDriverName, text);
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_VERIFY_ALGORITHMINVALID"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Encryption);
            }
            return 0;
        }

        private void ProcessEncryptionDriver(string driverName, string text)
        {
            if (DriverHandler.IsRegistered(DriverTypes.Encryption, driverName))
            {
                // Time when you're on a breakpoint is counted
                var spent = new Stopwatch();
                spent.Start();
                string encrypted = Encryption.GetEncryptedString(text, driverName);
                TextWriterColor.Write(encrypted);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_VERIFY_TIMESPENT"), spent.ElapsedMilliseconds);
                spent.Stop();
            }
        }

    }
}
