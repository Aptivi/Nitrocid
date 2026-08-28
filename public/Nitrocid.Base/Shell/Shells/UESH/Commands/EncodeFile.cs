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

using Nitrocid.Base.Drivers;
using Nitrocid.Base.Drivers.Encoding;
using Nitrocid.Base.Drivers.EncodingAsymmetric;
using Nitrocid.Base.Files;
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
    /// Encodes the file
    /// </summary>
    /// <remarks>
    /// This command will encode a file.
    /// </remarks>
    class EncodeFileCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "encodefile";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ENCODEFILE_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "file", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ENCODEFILE_ARGUMENT_FILE_DESC"
                    }),
                ],
                [
                    new SwitchInfo("key", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_KEY_DESC", new SwitchOptions()
                    {
                        ArgumentsRequired = true,
                    }),
                    new SwitchInfo("iv", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC", new SwitchOptions()
                    {
                        ArgumentsRequired = true,
                    }),
                    new SwitchInfo("algorithm", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC", new SwitchOptions()
                    {
                        ArgumentsRequired = true,
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            bool useCustomAlgorithm = parameters.ContainsSwitch("-algorithm");
            string algorithm = useCustomAlgorithm ? SwitchManager.GetSwitchValue(parameters.SwitchesList, "-algorithm") : DriverHandler.CurrentEncodingDriverLocal.DriverName;
            string path = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
            string keyValue = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-key");
            string ivValue = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-iv");
            bool isAsymmetric = DriverHandler.IsRegistered<IEncodingAsymmetricDriver>(algorithm);
            if (isAsymmetric)
            {
                // Initialize the driver
                var driver = DriverHandler.GetDriver<IEncodingAsymmetricDriver>(algorithm);
                driver.Initialize();

                // Now, encode the file
                driver.EncodeFile(path);
            }
            else
            {
                // Initialize the driver
                var driver = DriverHandler.GetDriver<IEncodingDriver>(algorithm);
                driver.Initialize();
                byte[] key = driver.Key;
                byte[] iv = driver.Iv;

                // Encode the target file using the key and the IV
                if (string.IsNullOrEmpty(keyValue) && string.IsNullOrEmpty(ivValue))
                    driver.EncodeFile(path);
                else
                {
                    key = driver.ComposeBytesFromString(keyValue);
                    iv = driver.ComposeBytesFromString(ivValue);
                    driver.EncodeFile(path, key, iv);
                }

                // Now, print out the key and the IV used
                string keyDecomposed = driver.DecomposeBytesFromString(key);
                string ivDecomposed = driver.DecomposeBytesFromString(iv);
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ENCODEFILE_KEYUSED") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(keyDecomposed, true, ThemeColorType.ListValue);
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ENCODEFILE_IVUSED") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(ivDecomposed, true, ThemeColorType.ListValue);
            }
            return 0;
        }
    }
}
