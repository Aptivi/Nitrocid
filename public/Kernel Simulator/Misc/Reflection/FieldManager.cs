
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Reflection;
using KS.Kernel.Debugging;
using KS.Languages;

namespace KS.Misc.Reflection
{
    /// <summary>
    /// Field management module
    /// </summary>
    public static class FieldManager
    {

        /// <summary>
        /// Sets the value of a variable to the new value dynamically
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <param name="VariableValue">New value of variable</param>
        /// <param name="Internal">Whether the field is internal</param>
        public static void SetValue(string Variable, object VariableValue, bool Internal = false) => SetValue(Variable, VariableValue, null, Internal);

        /// <summary>
        /// Sets the value of a variable to the new value dynamically
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <param name="VariableValue">New value of variable</param>
        /// <param name="VariableType">Variable type</param>
        /// <param name="Internal">Whether the field is internal</param>
        public static void SetValue(string Variable, object VariableValue, Type VariableType, bool Internal = false)
        {
            // Get field for specified variable
            FieldInfo TargetField;
            if (VariableType is not null)
            {
                TargetField = GetField(Variable, VariableType, Internal);
            }
            else
            {
                TargetField = GetField(Variable, Internal);
            }

            // Set the variable if found
            if (TargetField is not null)
            {
                // The "obj" description says this: "The object whose field value will be set."
                // Apparently, SetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
                // Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
                DebugWriter.WriteDebug(DebugLevel.I, "Got field {0}. Setting to {1}...", TargetField.Name, VariableValue);
                TargetField.SetValue(Variable, VariableValue);
            }
            else
            {
                // Variable not found on any of the "flag" modules.
                DebugWriter.WriteDebug(DebugLevel.I, "Field {0} not found.", Variable);
                throw new Kernel.Exceptions.NoSuchReflectionVariableException(Translate.DoTranslation("Variable {0} is not found on any of the modules."), Variable);
            }
        }

        /// <summary>
        /// Gets the value of a variable dynamically 
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <param name="Internal">Whether the field is internal</param>
        /// <returns>Value of a variable</returns>
        public static object GetValue(string Variable, bool Internal = false) => GetValue(Variable, null, Internal);

        /// <summary>
        /// Gets the value of a variable dynamically 
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <param name="VariableType">Variable type</param>
        /// <param name="Internal">Whether the field is internal</param>
        /// <returns>Value of a variable</returns>
        public static object GetValue(string Variable, Type VariableType, bool Internal = false)
        {
            // Get field for specified variable
            FieldInfo TargetField;
            if (VariableType is not null)
            {
                TargetField = GetField(Variable, VariableType, Internal);
            }
            else
            {
                TargetField = GetField(Variable, Internal);
            }

            // Get the variable if found
            if (TargetField is not null)
            {
                // The "obj" description says this: "The object whose field value will be returned."
                // Apparently, GetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
                // Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
                DebugWriter.WriteDebug(DebugLevel.I, "Got field {0}.", TargetField.Name);
                return TargetField.GetValue(Variable);
            }
            else
            {
                // Variable not found on any of the "flag" modules.
                DebugWriter.WriteDebug(DebugLevel.I, "Field {0} not found.", Variable);
                throw new Kernel.Exceptions.NoSuchReflectionVariableException(Translate.DoTranslation("Variable {0} is not found on any of the modules."), Variable);
            }
        }

        /// <summary>
        /// Gets a field from variable name
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <param name="Type">Variable type</param>
        /// <param name="Internal">Whether the field is internal</param>
        /// <returns>Field information</returns>
        public static FieldInfo GetField(string Variable, Type Type, bool Internal = false)
        {
            // Get fields of specified type
            FieldInfo Field;
            if (Internal)
            {
                Field = Type.GetField(Variable, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            }
            else
            {
                Field = Type.GetField(Variable);
            }

            // Check if any of them contains the specified variable
            if (Field is not null)
            {
                return Field;
            }
            return null;
        }

        /// <summary>
        /// Gets a field from variable name
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <param name="Internal">Whether the field is internal</param>
        /// <returns>Field information</returns>
        public static FieldInfo GetField(string Variable, bool Internal = false)
        {
            Type[] PossibleTypes;
            FieldInfo PossibleField;

            // Get types of possible flag locations
            PossibleTypes = Assembly.GetExecutingAssembly().GetTypes();

            // Get fields of flag modules
            foreach (Type PossibleType in PossibleTypes)
            {
                if (Internal)
                {
                    PossibleField = PossibleType.GetField(Variable, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                }
                else
                {
                    PossibleField = PossibleType.GetField(Variable);
                }
                if (PossibleField is not null)
                    return PossibleField;
            }
            return null;
        }

        /// <summary>
        /// Checks the specified variable if it exists
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <param name="Internal">Whether the field is internal</param>
        public static bool CheckField(string Variable, bool Internal = false)
        {
            // Get field for specified variable
            var TargetField = GetField(Variable, Internal);

            // Set the variable if found
            return TargetField is not null;
        }

    }
}
