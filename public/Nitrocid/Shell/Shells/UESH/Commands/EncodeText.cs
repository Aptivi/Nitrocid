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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Drivers;
using Nitrocid.Drivers.Encoding;
using Nitrocid.Drivers.EncodingAsymmetric;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;

namespace Nitrocid.Shell.Shells.UESH.Commands
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
            bool useCustomAlgorithm = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-algorithm");
            string algorithm = useCustomAlgorithm ? SwitchManager.GetSwitchValue(parameters.SwitchesList, "-algorithm") : DriverHandler.CurrentEncodingDriverLocal.DriverName;
            string orig = parameters.ArgumentsText;
            string keyValue = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-key");
            string ivValue = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-iv");
            bool isAsymmetric = DriverHandler.IsRegistered<IEncodingAsymmetricDriver>(algorithm);
            if (isAsymmetric)
            {
                // Initialize the driver
                var driver = DriverHandler.GetDriver<IEncodingAsymmetricDriver>(algorithm);
                driver.Initialize();

                // Now, encode the text
                var encoded = driver.GetEncodedString(orig);
                string decomposed = driver.DecomposeBytesFromString(encoded);
                TextWriters.Write(decomposed, true, KernelColorType.Success);
                if (driver.TryRepresentAsText(encoded, out string? strEncoded))
                    TextWriters.Write(Translate.DoTranslation("Encoded as string") + $": {strEncoded}", true, KernelColorType.Success);
            }
            else
            {
                // Initialize the driver
                var driver = DriverHandler.GetDriver<IEncodingDriver>(algorithm);
                driver.Initialize();
                byte[] key = driver.Key;
                byte[] iv = driver.Iv;
                byte[] encoded;

                // Encode the target file using the key and the IV
                if (string.IsNullOrEmpty(keyValue) && string.IsNullOrEmpty(ivValue))
                    encoded = driver.GetEncodedString(orig);
                else
                {
                    key = driver.ComposeBytesFromString(keyValue);
                    iv = driver.ComposeBytesFromString(ivValue);
                    encoded = driver.GetEncodedString(orig, key, iv);
                }
                string decomposed = driver.DecomposeBytesFromString(encoded);
                TextWriters.Write(decomposed, true, KernelColorType.Success);
                if (driver.TryRepresentAsText(encoded, out string? strEncoded))
                    TextWriters.Write(Translate.DoTranslation("Encoded as string") + $": {strEncoded}", true, KernelColorType.Success);

                // Now, print out the key and the IV used
                string keyDecomposed = driver.DecomposeBytesFromString(key);
                string ivDecomposed = driver.DecomposeBytesFromString(iv);
                TextWriters.Write("- " + Translate.DoTranslation("Key used") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write(keyDecomposed, true, KernelColorType.ListValue);
                TextWriters.Write("- " + Translate.DoTranslation("Initialization vector used") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write(ivDecomposed, true, KernelColorType.ListValue);
            }
            return 0;
        }
    }
}
