//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers;
using KS.Drivers.Encoding;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Switches;
using System;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Encodes the text
    /// </summary>
    /// <remarks>
    /// This command will encode a text.
    /// </remarks>
    class EncodeTextCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string algorithm = parameters.ArgumentsList.Length > 1 ? parameters.ArgumentsList[1] : DriverHandler.CurrentEncodingDriverLocal.DriverName;
            string orig = parameters.ArgumentsList[0];
            string keyValue = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-key");
            string ivValue = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-iv");
            var driver = DriverHandler.GetDriver<IEncodingDriver>(algorithm);
            driver.Initialize();
            byte[] encoded;
            byte[] key = driver.IsSymmetric ? driver.Key : Array.Empty<byte>();
            byte[] iv = driver.IsSymmetric ? driver.Iv : Array.Empty<byte>();
            if (string.IsNullOrEmpty(keyValue) && string.IsNullOrEmpty(ivValue))
                encoded = driver.GetEncodedString(orig);
            else
            {
                key = driver.ComposeBytesFromString(keyValue);
                iv = driver.ComposeBytesFromString(ivValue);
                encoded = driver.GetEncodedString(orig, key, iv);
            }
            string decomposed = driver.DecomposeBytesFromString(encoded);
            TextWriterColor.WriteKernelColor(decomposed, true, KernelColorType.Success);
            if (driver.IsSymmetric)
            {
                string keyDecomposed = driver.DecomposeBytesFromString(key);
                string ivDecomposed = driver.DecomposeBytesFromString(iv);
                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Key used") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor(keyDecomposed, true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Initialization vector used") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor(ivDecomposed, true, KernelColorType.ListValue);
            }
            return 0;
        }
    }
}
