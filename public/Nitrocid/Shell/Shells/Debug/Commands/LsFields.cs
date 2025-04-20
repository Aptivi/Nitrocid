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
using Terminaux.Writer.FancyWriters;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters;

namespace Nitrocid.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can list all the available fields
    /// </summary>
    /// <remarks>
    /// This command lets you list all the available fields that Nitrocid KS registered.
    /// </remarks>
    class LsFieldsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // List all available fields on all the kernel types
            foreach (var type in ReflectionCommon.KernelTypes)
            {
                try
                {
                    var fields = FieldManager.GetFields(type);
                    if (fields.Count > 0)
                    {
                        // Write the field names and their values
                        SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("List of fields for") + $" {type.Name}", true);
                        TextWriters.WriteList(fields);
                    }
                }
                catch (Exception ex)
                {
                    if (!SwitchManager.ContainsSwitch(parameters.SwitchesList, "-suppress"))
                        TextWriters.Write(Translate.DoTranslation("Failed to get field info for") + $" {type.Name}: {ex.Message}", KernelColorType.Error);
                }
            }
            return 0;
        }

        public override int ExecuteDumb(CommandParameters parameters, ref string variableValue)
        {
            // List all available fields on all the kernel types
            foreach (var type in ReflectionCommon.KernelTypes)
            {
                try
                {
                    var fields = FieldManager.GetFields(type);
                    if (fields.Count > 0)
                    {
                        // Write the field names and their values
                        SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("List of fields for") + $" {type.Name}", true);
                        foreach (var field in fields)
                            TextWriterColor.Write($"  - {field.Key} [{field.Value}]");
                    }
                }
                catch (Exception ex)
                {
                    if (!SwitchManager.ContainsSwitch(parameters.SwitchesList, "-suppress"))
                        TextWriters.Write(Translate.DoTranslation("Failed to get field info for") + $" {type.Name}: {ex.Message}", KernelColorType.Error);
                }
            }
            return 0;
        }

    }
}
