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

using System.Diagnostics;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers;
using Nitrocid.Drivers.Encryption;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Calculates the sum of a text
    /// </summary>
    /// <remarks>
    /// It calculates the sum of a text using the available algorithms.
    /// </remarks>
    class SumTextCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string text = parameters.ArgumentsList[1];
            if (DriverHandler.IsRegistered(DriverTypes.Encryption, parameters.ArgumentsList[0]))
            {
                // Time when you're on a breakpoint is counted
                var spent = new Stopwatch();
                spent.Start();
                string encrypted = Encryption.GetEncryptedString(text, parameters.ArgumentsList[0]);
                TextWriterColor.Write(encrypted);
                TextWriterColor.Write(Translate.DoTranslation("Time spent: {0} milliseconds"), spent.ElapsedMilliseconds);
                spent.Stop();
            }
            else if (parameters.ArgumentsList[0] == "all")
            {
                foreach (string driverName in DriverHandler.GetDriverNames<IEncryptionDriver>())
                {
                    // Time when you're on a breakpoint is counted
                    var spent = new Stopwatch();
                    spent.Start();
                    string encrypted = Encryption.GetEncryptedString(text, driverName);
                    TextWriterColor.Write($"{driverName}: {encrypted}");
                    TextWriterColor.Write(Translate.DoTranslation("Time spent: {0} milliseconds"), spent.ElapsedMilliseconds);
                    spent.Stop();
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Invalid encryption algorithm."), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Encryption);
            }
            return 0;
        }

    }
}
