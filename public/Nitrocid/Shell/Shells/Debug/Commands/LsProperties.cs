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

using System;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Misc.Reflection;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Shell.Shells.Debug.Commands
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
                        SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSPROPERTIES_TITLE") + $" {type.Name}", KernelColorTools.GetColor(KernelColorType.ListTitle));
                        TextWriters.WriteList(properties);
                    }
                }
                catch (Exception ex)
                {
                    if (!SwitchManager.ContainsSwitch(parameters.SwitchesList, "-suppress"))
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_GETPROPERTYVALUE_FAILED") + $" {type.Name}: {ex.Message}", KernelColorType.Error);
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
                    if (!SwitchManager.ContainsSwitch(parameters.SwitchesList, "-suppress"))
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_GETPROPERTYVALUE_FAILED") + $" {type.Name}: {ex.Message}", KernelColorType.Error);
                }
            }
            return 0;
        }

    }
}
