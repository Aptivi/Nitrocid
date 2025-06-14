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
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Users;
using System.Linq;
using Nitrocid.Languages;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Changes your kernel culture
    /// </summary>
    /// <remarks>
    /// If you want to change your kernel culture without having to go to Settings (for scripting purposes), you can use this command.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the command.
    /// </remarks>
    class ChCultureCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool useUser = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-user");

            // Culture selection takes only one culture
            string culture = parameters.ArgumentsList[0];
            var cultures = CultureManager.GetCultureCodes();
            if (!cultures.Contains(culture))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERCULTURE_FAILURE") + $" {culture}", true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.NoSuchLanguage);
            }

            // Change the culture
            if (useUser)
            {
                UserManagement.CurrentUser.PreferredCulture = culture;
                UserManagement.SaveUsers();
            }
            else
                CultureManager.UpdateCulture(culture);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHLANG_RESTARTREQUIRED"));
            return 0;
        }

    }
}
