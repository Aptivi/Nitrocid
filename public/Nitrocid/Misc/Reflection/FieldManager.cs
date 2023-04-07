
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;

namespace KS.Misc.Reflection
{
    /// <summary>
    /// Field management module
    /// </summary>
    public static class FieldManager
    {

        private static readonly Dictionary<string, Action<object>> cachedSetters = new();
        private static readonly Dictionary<string, Func<object>> cachedGetters = new();

        /// <summary>
        /// Sets the value of a variable to the new value dynamically
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <param name="VariableValue">New value of variable</param>
        public static void SetValue(string Variable, object VariableValue) =>
            SetValue(Variable, VariableValue, null);

        /// <summary>
        /// Sets the value of a variable to the new value dynamically
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <param name="VariableValue">New value of variable</param>
        /// <param name="VariableType">Variable type</param>
        public static void SetValue(string Variable, object VariableValue, Type VariableType)
        {
            // Get field for specified variable
            FieldInfo TargetField;
            if (VariableType is not null)
            {
                TargetField = GetField(Variable, VariableType);
            }
            else
            {
                TargetField = GetField(Variable);
            }

            // Set the variable if found
            if (TargetField is not null)
            {
                // Expressions are claimed that it's faster than Reflection, but let's see!
                DebugWriter.WriteDebug(DebugLevel.I, "Got field {0}. Setting to {1}...", TargetField.Name, VariableValue);
                ExpressionSetFieldValue(TargetField, VariableValue);
            }
            else
            {
                // Variable not found on any of the "flag" modules.
                DebugWriter.WriteDebug(DebugLevel.I, "Field {0} not found.", Variable);
                throw new KernelException(KernelExceptionType.NoSuchReflectionVariable, Translate.DoTranslation("Variable {0} is not found on any of the modules."), Variable);
            }
        }

        private static void ExpressionSetFieldValue(FieldInfo fieldInfo, object value)
        {
            if (fieldInfo is null)
                throw new ArgumentNullException(nameof(fieldInfo));

            string cachedName = $"{fieldInfo.DeclaringType.FullName} - {fieldInfo.Name}";
            if (cachedSetters.ContainsKey(cachedName))
            {
                var cachedExpression = cachedSetters[cachedName];
                cachedExpression(value);
                return;
            }

            var argumentParam = Expression.Parameter(typeof(object));
            var assignExpr = Expression.Field(null, fieldInfo.DeclaringType, fieldInfo.Name);
            var convertExpr = Expression.Convert(argumentParam, assignExpr.Type);
            var setExpr = Expression.Assign(assignExpr, convertExpr);

            var expression = Expression.Lambda<Action<object>>(setExpr, argumentParam).Compile();

            cachedSetters.Add(cachedName, expression);
            expression(value);
        }

        /// <summary>
        /// Gets the value of a variable dynamically 
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <param name="Index">Index from the enumerable</param>
        /// <returns>Value of a variable</returns>
        public static object GetValueFromEnumerable(string Variable, int Index) =>
            GetValueFromEnumerable(Variable, null, Index);

        /// <summary>
        /// Gets the value of a variable dynamically 
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <param name="VariableType">Variable type</param>
        /// <param name="Index">Index from the enumerable</param>
        /// <returns>Value of a variable from the enumerable, or null if not an enumerable</returns>
        public static object GetValueFromEnumerable(string Variable, Type VariableType, int Index)
        {
            var value = GetValue(Variable, VariableType);
            if (value is IEnumerable values)
            {
                int idx = 0;
                foreach (object val in values)
                {
                    if (idx == Index)
                        return val;
                    idx++;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the value of a variable dynamically 
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <returns>Value of a variable</returns>
        public static object GetValue(string Variable) =>
            GetValue(Variable, null);

        /// <summary>
        /// Gets the value of a variable dynamically 
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <param name="VariableType">Variable type</param>
        /// <param name="UseGeneral">Whether to use the general kernel types</param>
        /// <returns>Value of a variable</returns>
        public static object GetValue(string Variable, Type VariableType, bool UseGeneral = false)
        {
            // Get field for specified variable
            FieldInfo TargetField;
            if (VariableType is not null)
                TargetField = GetField(Variable, VariableType);
            else if (UseGeneral)
                TargetField = GetFieldGeneral(Variable);
            else
                TargetField = GetField(Variable);

            // Get the variable if found
            if (TargetField is not null)
            {
                // The "obj" description says this: "The object whose field value will be returned."
                // Apparently, GetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
                // Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
                DebugWriter.WriteDebug(DebugLevel.I, "Got field {0}.", TargetField.Name);
                return ExpressionGetFieldValue(TargetField);
            }
            else
            {
                // Variable not found on any of the "flag" modules.
                DebugWriter.WriteDebug(DebugLevel.I, "Field {0} not found.", Variable);
                throw new KernelException(KernelExceptionType.NoSuchReflectionVariable, Translate.DoTranslation("Variable {0} is not found on any of the modules."), Variable);
            }
        }

        private static object ExpressionGetFieldValue(FieldInfo fieldInfo)
        {
            if (fieldInfo is null)
                throw new ArgumentNullException(nameof(fieldInfo));

            string cachedName = $"{fieldInfo.DeclaringType.FullName} - {fieldInfo.Name}";
            if (cachedGetters.ContainsKey(cachedName))
            {
                var cachedExpression = cachedGetters[cachedName];
                return cachedExpression();
            }

            var assignExpr = Expression.Field(null, fieldInfo.DeclaringType, fieldInfo.Name);
            var convExpr = Expression.Convert(assignExpr, typeof(object));

            var expression = Expression.Lambda<Func<object>>(convExpr).Compile();

            cachedGetters.Add(cachedName, expression);
            return expression();
        }

        /// <summary>
        /// Gets a field from variable name
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <param name="Type">Variable type</param>
        /// <returns>Field information</returns>
        public static FieldInfo GetField(string Variable, Type Type)
        {
            // Get fields of specified type
            FieldInfo Field;
            Field = Type.GetField(Variable);

            // Check if any of them contains the specified variable
            if (Field is not null)
                return Field;
            return null;
        }

        /// <summary>
        /// Gets a field from variable name
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <returns>Field information</returns>
        public static FieldInfo GetField(string Variable)
        {
            Type[] PossibleTypes;
            FieldInfo PossibleField;

            // Get types of possible flag locations
            PossibleTypes = ReflectionCommon.KernelConfigTypes;

            // Get fields of flag modules
            foreach (Type PossibleType in PossibleTypes)
            {
                PossibleField = PossibleType.GetField(Variable);
                if (PossibleField is not null)
                    return PossibleField;
            }
            return null;
        }

        /// <summary>
        /// Gets a field from variable name generally
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <returns>Field information</returns>
        public static FieldInfo GetFieldGeneral(string Variable)
        {
            Type[] PossibleTypes;
            FieldInfo PossibleField;

            // Get types of possible flag locations
            PossibleTypes = ReflectionCommon.KernelTypes;

            // Get fields of flag modules
            foreach (Type PossibleType in PossibleTypes)
            {
                PossibleField = PossibleType.GetField(Variable);
                if (PossibleField is not null)
                    return PossibleField;
            }
            return null;
        }

        /// <summary>
        /// Checks the specified variable if it exists
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        public static bool CheckField(string Variable)
        {
            // Get field for specified variable
            var TargetField = GetField(Variable);

            // Set the variable if found
            return TargetField is not null;
        }

    }
}
