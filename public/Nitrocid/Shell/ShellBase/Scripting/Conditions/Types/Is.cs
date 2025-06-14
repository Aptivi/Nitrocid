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

using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Text.Probers.Regexp;
using System;
using System.Collections.Generic;
using Textify.General;

namespace Nitrocid.Shell.ShellBase.Scripting.Conditions.Types
{
    /// <summary>
    /// Checks to see if a UESH variable is of the correct type
    /// </summary>
    public class IsCondition : BaseCondition, ICondition
    {
        internal static Dictionary<string, Func<string, bool>> DataTypes = new()
        {
            { "null",       (value) => value is null },
            { "string",     (value) => value is not null },
            { "fullstring", (value) => value is not null && !string.IsNullOrEmpty(value) },
            { "numeric",    (value) => value is not null && TextTools.IsStringNumeric(value) },
            { "byte",       (value) => value is not null && sbyte.TryParse(value, out _) },
            { "i8",         (value) => value is not null && sbyte.TryParse(value, out _) },
            { "ubyte",      (value) => value is not null && byte.TryParse(value, out _) },
            { "u8",         (value) => value is not null && byte.TryParse(value, out _) },
            { "int16",      (value) => value is not null && short.TryParse(value, out _) },
            { "short",      (value) => value is not null && short.TryParse(value, out _) },
            { "i16",        (value) => value is not null && short.TryParse(value, out _) },
            { "int32",      (value) => value is not null && int.TryParse(value, out _) },
            { "integer",    (value) => value is not null && int.TryParse(value, out _) },
            { "i32",        (value) => value is not null && int.TryParse(value, out _) },
            { "int64",      (value) => value is not null && long.TryParse(value, out _) },
            { "long",       (value) => value is not null && long.TryParse(value, out _) },
            { "i64",        (value) => value is not null && long.TryParse(value, out _) },
            { "uint16",     (value) => value is not null && ushort.TryParse(value, out _) },
            { "ushort",     (value) => value is not null && ushort.TryParse(value, out _) },
            { "u16",        (value) => value is not null && ushort.TryParse(value, out _) },
            { "uint32",     (value) => value is not null && uint.TryParse(value, out _) },
            { "uinteger",   (value) => value is not null && uint.TryParse(value, out _) },
            { "u32",        (value) => value is not null && uint.TryParse(value, out _) },
            { "uint64",     (value) => value is not null && ulong.TryParse(value, out _) },
            { "ulong",      (value) => value is not null && ulong.TryParse(value, out _) },
            { "u64",        (value) => value is not null && ulong.TryParse(value, out _) },
            { "decimal",    (value) => value is not null && decimal.TryParse(value, out _) },
            { "float",      (value) => value is not null && float.TryParse(value, out _) },
            { "f32",        (value) => value is not null && float.TryParse(value, out _) },
            { "double",     (value) => value is not null && double.TryParse(value, out _) },
            { "f64",        (value) => value is not null && double.TryParse(value, out _) },
            { "bool",       (value) => value is not null && bool.TryParse(value, out _) },
            { "regex",      (value) => value is not null && RegexpTools.IsValidRegex(value) },
        };

        /// <inheritdoc/>
        public override string ConditionName => "is";

        /// <inheritdoc/>
        public override int ConditionPosition { get; } = 2;

        /// <inheritdoc/>
        public override int ConditionRequiredArguments { get; } = 3;

        /// <inheritdoc/>
        public override bool IsConditionSatisfied(string FirstVariable, string SecondVariable)
        {
            // SecondVariable is actually a data type needed for parsing.
            if (!DataTypes.TryGetValue(SecondVariable, out Func<string, bool>? dataFunc))
                throw new KernelException(KernelExceptionType.UESHConditionParse, LanguageTools.GetLocalized("NKS_SHELL_BASE_SCRIPTING_CONDITIONS_EXCEPTION_INVALIDDATATYPE"), SecondVariable);

            // Get the action needed to get the comparer and test the condition defined above
            return dataFunc(FirstVariable);
        }

    }
}
