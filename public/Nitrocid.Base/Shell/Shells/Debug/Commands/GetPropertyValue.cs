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
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can get a property value
    /// </summary>
    /// <remarks>
    /// This command lets you get a value from a specified property.
    /// </remarks>
    class GetPropertyValueCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // List all available properties on all the kernel types
            string propertyName = parameters.ArgumentsList[0];
            foreach (var type in ReflectionCommon.KernelTypes)
            {
                try
                {
                    var property = PropertyManager.GetProperty(propertyName, type);
                    if (property is null || (!property.GetMethod?.IsStatic ?? false))
                        continue;

                    // Write the property name and its value
                    SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_GETPROPERTYVALUE_TITLE") + $" {type.Name}::{propertyName}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_GETFIELDVALUE_VALUE") + $": ", false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($"{property.GetValue(null)}", ThemeColorType.ListValue);
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_GETPROPERTYVALUE_FAILED") + $" {type.Name}::{propertyName}: {ex.Message}", ThemeColorType.Error);
                }
            }
            return 0;
        }

    }
}
