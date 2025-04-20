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
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can get a field value
    /// </summary>
    /// <remarks>
    /// This command lets you get a value from a specified field.
    /// </remarks>
    class GetFieldValueCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // List all available fields on all the kernel types
            string fieldName = parameters.ArgumentsList[0];
            foreach (var type in ReflectionCommon.KernelTypes)
            {
                try
                {
                    var field = FieldManager.GetField(fieldName, type);
                    if (field is null || !field.IsStatic)
                        continue;

                    // Write the field name and its value
                    SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("Field info for") + $" {type.Name}::{fieldName}", KernelColorTools.GetColor(KernelColorType.ListTitle));
                    TextWriters.Write(Translate.DoTranslation("Value") + $": ", false, KernelColorType.ListEntry);
                    TextWriters.Write($"{field.GetValue(null)}", KernelColorType.ListValue);
                }
                catch (Exception ex)
                {
                    TextWriters.Write(Translate.DoTranslation("Failed to get field info for") + $" {type.Name}::{fieldName}: {ex.Message}", KernelColorType.Error);
                }
            }
            return 0;
        }

    }
}
