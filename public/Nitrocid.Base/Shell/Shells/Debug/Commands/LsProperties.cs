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

using System;
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Themes.Colors;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can list all the available properties
    /// </summary>
    /// <remarks>
    /// This command lets you list all the available properties that Nitrocid KS registered.
    /// </remarks>
    class LsPropertiesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // List all available properties on all the kernel types
            foreach (var type in ReflectionCommon.KernelTypes)
            {
                try
                {
                    var properties = PropertyManager.GetProperties(type);
                    if (properties.Count > 0)
                    {
                        // Write the property names and their values
                        SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSPROPERTIES_TITLE") + $" {type.Name}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                        ListWriterColor.WriteList(properties);
                    }
                }
                catch (Exception ex)
                {
                    if (!parameters.ContainsSwitch("-suppress"))
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_GETPROPERTYVALUE_FAILED") + $" {type.Name}: {ex.Message}", ThemeColorType.Error);
                }
            }
            return 0;
        }

        public override int ExecuteDumb(CommandParameters parameters, ref string variableValue)
        {
            // List all available properties on all the kernel types
            foreach (var type in ReflectionCommon.KernelTypes)
            {
                try
                {
                    var properties = PropertyManager.GetProperties(type);
                    if (properties.Count > 0)
                    {
                        // Write the property names and their values
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSPROPERTIES_TITLE") + $" {type.Name}", true);
                        foreach (var property in properties)
                            TextWriterColor.Write($"  - {property.Key} [{property.Value}]");
                    }
                }
                catch (Exception ex)
                {
                    if (!parameters.ContainsSwitch("-suppress"))
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_GETPROPERTYVALUE_FAILED") + $" {type.Name}: {ex.Message}", ThemeColorType.Error);
                }
            }
            return 0;
        }

    }
}
