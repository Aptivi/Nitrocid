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

using Nitrocid.Drivers;
using Nitrocid.Drivers.Encoding;
using Nitrocid.Files;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Decodes the file
    /// </summary>
    /// <remarks>
    /// This command will decode an encoded file.
    /// </remarks>
    class DecodeFileCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool useCustomAlgorithm = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-algorithm");
            string algorithm = useCustomAlgorithm ? SwitchManager.GetSwitchValue(parameters.SwitchesList, "-algorithm") : DriverHandler.CurrentEncodingDriverLocal.DriverName;
            string path = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
            string keyValue = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-key");
            string ivValue = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-iv");
            var driver = DriverHandler.GetDriver<IEncodingDriver>(algorithm);
            driver.Initialize();
            if (string.IsNullOrEmpty(keyValue) && string.IsNullOrEmpty(ivValue))
                driver.DecodeFile(path);
            else
            {
                byte[] key = driver.ComposeBytesFromString(keyValue);
                byte[] iv = driver.ComposeBytesFromString(ivValue);
                driver.DecodeFile(path, key, iv);
            }
            return 0;
        }
    }
}
