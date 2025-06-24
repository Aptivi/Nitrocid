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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Drivers.Encoding;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Drivers.EncodingAsymmetric;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
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
            // Check the algorithm
            string algorithm = parameters.ArgumentsList.Length > 0 ? parameters.ArgumentsList[0] : DriverHandler.CurrentEncodingDriverLocal.DriverName;
            bool isAsymmetric = DriverHandler.IsRegistered<IEncodingAsymmetricDriver>(algorithm);
            if (isAsymmetric)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETKEYIV_NEEDSSYMMETRIC"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Encoding);
            }

            // Get the driver and initialize it
            var driver = DriverHandler.GetDriver<IEncodingDriver>(algorithm);
            driver.Initialize();

            // Now, get the key and the IV
            byte[] key = driver.Key;
            byte[] iv = driver.Iv;
            string keyDecomposed = driver.DecomposeBytesFromString(key);
            string ivDecomposed = driver.DecomposeBytesFromString(iv);
            TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ENCODEFILE_KEYUSED") + ": ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write(keyDecomposed, true, ThemeColorType.ListValue);
            TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ENCODEFILE_IVUSED") + ": ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write(ivDecomposed, true, ThemeColorType.ListValue);
            variableValue = $"[{keyDecomposed}, {ivDecomposed}]";
            return 0;
        }
    }
}
