//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Nitrocid.Files;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Encodes the file
    /// </summary>
    /// <remarks>
    /// This command will encode a file.
    /// </remarks>
    class EncodeFileCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string algorithm = parameters.ArgumentsList.Length > 1 ? parameters.ArgumentsList[1] : DriverHandler.CurrentEncodingDriverLocal.DriverName;
            string path = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
            string keyValue = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-key");
            string ivValue = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-iv");
            var driver = DriverHandler.GetDriver<IEncodingDriver>(algorithm);
            driver.Initialize();
            byte[] key = driver.IsSymmetric ? driver.Key : [];
            byte[] iv = driver.IsSymmetric ? driver.Iv : [];
            if (string.IsNullOrEmpty(keyValue) && string.IsNullOrEmpty(ivValue))
                driver.EncodeFile(path);
            else
            {
                key = driver.ComposeBytesFromString(keyValue);
                iv = driver.ComposeBytesFromString(ivValue);
                driver.EncodeFile(path, key, iv);
            }
            if (driver.IsSymmetric)
            {
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
