
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers;
using KS.Drivers.Encoding;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text.Probers.Placeholder;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Switches;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Gets the key and the initialization vector for symmetrical encoding
    /// </summary>
    /// <remarks>
    /// This command will get the key and the initialization vector from the specified symmetrical encoding driver.
    /// </remarks>
    class GetKeyIvCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string algorithm = parameters.ArgumentsList[0];
            var driver = DriverHandler.GetDriver<IEncodingDriver>(algorithm);
            driver.Initialize();
            if (!driver.IsSymmetric)
            {
                TextWriterColor.Write(Translate.DoTranslation("Only symmetric encoding algorithms which use both the key and the initialization vector are supported."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Encoding;
            }

            // Now, get the key and the IV
            byte[] key = driver.Key;
            byte[] iv = driver.Iv;
            string keyDecomposed = driver.DecomposeBytesFromString(key);
            string ivDecomposed = driver.DecomposeBytesFromString(iv);
            TextWriterColor.Write("- " + Translate.DoTranslation("Key used") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(keyDecomposed, true, KernelColorType.ListValue);
            TextWriterColor.Write("- " + Translate.DoTranslation("Initialization vector used") + ": ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(ivDecomposed, true, KernelColorType.ListValue);
            variableValue = $"[{keyDecomposed}, {ivDecomposed}]";
            return 0;
        }
    }
}
