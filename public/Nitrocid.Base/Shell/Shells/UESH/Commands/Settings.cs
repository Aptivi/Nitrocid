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

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;
using Nitrocid.Base.Kernel.Configuration.Instances;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Security.Permissions;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lets you change kernel settings
    /// </summary>
    /// <remarks>
    /// This command starts up the Settings application, which allows you to change the kernel settings available to you. It's the successor to the defunct Nitrocid KS Configuration Tool application, and is native to the kernel.
    /// It starts with the list of sections to start from. Once the user selects one, they'll be greeted with various options that are configurable. When they choose one, they'll be able to change the setting there.
    /// If you just want to try out a setting without saving to the configuration file, you can change a setting and exit it immediately. It only survives the current session until you decide to save the changes to the configuration file.
    /// Some settings allow you to specify a string, a number, or by the usage of another API, like the ColorWheel() tool.
    /// In the string or long string values, if you used the /clear value, it will blank out the value. In some settings, if you just pressed ENTER, it'll use the same value that the kernel uses at the moment.
    /// We've made sure that this application is user-friendly.
    /// <br></br>
    /// <br></br>
    /// For the screensaver and splashes, refer to the command usage below.
    /// <br></br>
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-sel</term>
    /// <description>Opens the legacy selection-style-based settings instead of the interactive TUI</description>
    /// </item>
    /// <item>
    /// <term>-saver</term>
    /// <description>Opens the screensaver settings</description>
    /// </item>
    /// <item>
    /// <term>-addonsaver</term>
    /// <description>Opens the extra screensaver settings</description>
    /// </item>
    /// <item>
    /// <term>-splash</term>
    /// <description>Opens the splash settings</description>
    /// </item>
    /// <item>
    /// <term>-addonsplash</term>
    /// <description>Opens the extra splash settings</description>
    /// </item>
    /// <item>
    /// <term>-driver</term>
    /// <description>Opens the driver settings</description>
    /// </item>
    /// <item>
    /// <term>-type=&lt;typeClassName&gt;</term>
    /// <description>Opens the custom settings</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class SettingsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!PermissionsTools.IsPermissionGranted(PermissionTypes.RunStrictCommands) &&
                !UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", vars: [parameters.CommandText]);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_NEEDSPERM"), true, ThemeColorType.Error, parameters.CommandText);
                return -4;
            }

            bool useType = parameters.ContainsSwitch("-type");
            var typeFinal = useType ? SwitchManager.GetSwitchValue(parameters.SwitchesList, "-type") : nameof(KernelMainConfig);
            if (parameters.SwitchesList.Length > 0 && !useType)
            {
                bool isSaver = parameters.ContainsSwitch("-saver");
                bool isAddonSaver = parameters.ContainsSwitch("-addonsaver");
                bool isSplash = parameters.ContainsSwitch("-splash");
                bool isAddonSplash = parameters.ContainsSwitch("-addonsplash");
                bool isDriver = parameters.ContainsSwitch("-driver");
                if (isSaver)
                    typeFinal = nameof(KernelSaverConfig);
                else if (isAddonSaver)
                {
                    if (ConfigTools.IsCustomSettingBuiltin("ExtraSaversConfig"))
                        typeFinal = "ExtraSaversConfig";
                    else
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SETTINGS_ADDIITONALSAVERS"), true, ThemeColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.Config);
                    }
                }
                if (isSplash)
                    typeFinal = nameof(KernelSplashConfig);
                else if (isAddonSplash)
                {
                    if (ConfigTools.IsCustomSettingBuiltin("ExtraSplashesConfig"))
                        typeFinal = "ExtraSplashesConfig";
                    else
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SETTINGS_ADDIITONALSPLASHES"), true, ThemeColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.Config);
                    }
                }
                else if (isDriver)
                    typeFinal = nameof(KernelDriverConfig);
            }
            SettingsApp.OpenMainPage(typeFinal);
            return 0;
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SETTINGS_TYPELISTING") + ": ", true, ThemeColorType.Tip);
            TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SETTINGS_BASE") + ": ", true, ThemeColorType.ListTitle);
            ListWriterColor.WriteList(Config.baseConfigurations.Keys);
            TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SETTINGS_CUSTOM") + ": ", true, ThemeColorType.ListTitle);
            ListWriterColor.WriteList(Config.customConfigurations.Keys);
        }

    }
}
