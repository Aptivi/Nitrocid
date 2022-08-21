
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
using System.Collections.Generic;
using System.Reflection;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Reflection
{
    public static class PropertyManager
    {

        /// <summary>
        /// Sets the value of a property to the new value dynamically
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <param name="VariableValue">New value</param>
        public static void SetPropertyValue(string Variable, object VariableValue)
        {
            SetPropertyValue(Variable, VariableValue, null);
        }

        /// <summary>
        /// Sets the value of a property to the new value dynamically
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <param name="VariableValue">New value</param>
        /// <param name="VariableType">Property type</param>
        public static void SetPropertyValue(string Variable, object VariableValue, Type VariableType)
        {
            // Get field for specified variable
            PropertyInfo TargetProperty;
            if (VariableType is not null)
            {
                TargetProperty = GetProperty(Variable, VariableType);
            }
            else
            {
                TargetProperty = GetProperty(Variable);
            }

            // Set the variable if found
            if (TargetProperty is not null)
            {
                // The "obj" description says this: "The object whose field value will be set."
                // Apparently, SetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
                // Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
                DebugWriter.Wdbg(DebugLevel.I, "Got field {0}. Setting to {1}...", TargetProperty.Name, VariableValue);
                TargetProperty.SetValue(Variable, VariableValue);
            }
            else
            {
                // Variable not found on any of the "flag" modules.
                DebugWriter.Wdbg(DebugLevel.I, "Property {0} not found.", Variable);
                throw new Kernel.Exceptions.NoSuchReflectionVariableException(Translate.DoTranslation("Variable {0} is not found on any of the modules."), Variable);
            }
        }

        /// <summary>
        /// Gets the value of a property dynamically 
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <returns>Value of a property</returns>
        public static object GetPropertyValue(string Variable)
        {
            return GetPropertyValue(Variable, null);
        }

        /// <summary>
        /// Gets the value of a property dynamically 
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <param name="VariableType">Property type</param>
        /// <returns>Value of a property</returns>
        public static object GetPropertyValue(string Variable, Type VariableType)
        {
            // Get field for specified variable
            PropertyInfo TargetProperty;
            if (VariableType is not null)
            {
                TargetProperty = GetProperty(Variable, VariableType);
            }
            else
            {
                TargetProperty = GetProperty(Variable);
            }

            // Get the variable if found
            if (TargetProperty is not null)
            {
                // The "obj" description says this: "The object whose field value will be returned."
                // Apparently, GetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
                // Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
                DebugWriter.Wdbg(DebugLevel.I, "Got field {0}.", TargetProperty.Name);
                return TargetProperty.GetValue(Variable);
            }
            else
            {
                // Variable not found on any of the "flag" modules.
                DebugWriter.Wdbg(DebugLevel.I, "Property {0} not found.", Variable);
                throw new Kernel.Exceptions.NoSuchReflectionVariableException(Translate.DoTranslation("Variable {0} is not found on any of the modules."), Variable);
            }
        }

        /// <summary>
        /// Gets a property from variable name
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <param name="Type">Property type</param>
        /// <returns>Property information</returns>
        public static PropertyInfo GetProperty(string Variable, Type Type)
        {
            // Get fields of specified type
            var PropertyInstance = Type.GetProperty(Variable);

            // Check if any of them contains the specified variable
            if (PropertyInstance is not null)
            {
                return PropertyInstance;
            }
            return null;
        }

        /// <summary>
        /// Gets a property from variable name
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <returns>Property information</returns>
        public static PropertyInfo GetProperty(string Variable)
        {
            Type[] PossibleTypes;
            PropertyInfo PossibleProperty;

            // Get types of possible flag locations
            PossibleTypes = Assembly.GetExecutingAssembly().GetTypes();

            // Get fields of flag modules
            foreach (Type PossibleType in PossibleTypes)
            {
                PossibleProperty = PossibleType.GetProperty(Variable);
                if (PossibleProperty is not null)
                    return PossibleProperty;
            }
            return null;
        }

        /// <summary>
        /// Checks the specified property if it exists
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        public static bool CheckProperty(string Variable)
        {
            // Get field for specified variable
            var TargetProperty = GetProperty(Variable);

            // Set the variable if found
            return TargetProperty is not null;
        }

        /// <summary>
        /// Gets the value of a property in the type of a variable dynamically
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <param name="Property">Property name from within the variable type</param>
        /// <returns>Value of a property</returns>
        public static object GetPropertyValueInVariable(string Variable, string Property)
        {
            return GetPropertyValueInVariable(Variable, Property, null);
        }

        /// <summary>
        /// Gets the value of a property in the type of a variable dynamically
        /// </summary>
        /// <param name="Variable">Variable name. Use operator NameOf to get name.</param>
        /// <param name="Property">Property name from within the variable type</param>
        /// <param name="VariableType">Variable type</param>
        /// <returns>Value of a property</returns>
        public static object GetPropertyValueInVariable(string Variable, string Property, Type VariableType)
        {
            // Get field for specified variable
            FieldInfo TargetField;
            if (VariableType is not null)
            {
                TargetField = FieldManager.GetField(Variable, VariableType, true);
            }
            else
            {
                TargetField = FieldManager.GetField(Variable, true);
            }

            // Get the variable if found
            if (TargetField is not null)
            {
                // Now, get the property
                DebugWriter.Wdbg(DebugLevel.I, "Got field {0}.", TargetField.Name);
                var TargetProperty = TargetField.FieldType.GetProperty(Property);
                object TargetValue;
                if (VariableType is not null)
                {
                    TargetValue = FieldManager.GetValue(Variable, VariableType, true);
                }
                else
                {
                    TargetValue = FieldManager.GetValue(Variable, true);
                }

                // Get the property value if found
                if (TargetProperty is not null)
                {
                    return TargetProperty.GetValue(TargetValue);
                }
                else
                {
                    // Property not found on any of the "flag" modules.
                    DebugWriter.Wdbg(DebugLevel.I, "Property {0} not found.", Property);
                    throw new Kernel.Exceptions.NoSuchReflectionPropertyException(Translate.DoTranslation("Property {0} is not found on any of the modules."), Property);
                }
            }
            else
            {
                // Variable not found on any of the "flag" modules.
                DebugWriter.Wdbg(DebugLevel.I, "Field {0} not found.", Variable);
                throw new Kernel.Exceptions.NoSuchReflectionVariableException(Translate.DoTranslation("Variable {0} is not found on any of the modules."), Variable);
            }
        }

        /// <summary>
        /// Gets the properties from the type dynamically
        /// </summary>
        /// <param name="VariableType">Variable type</param>
        /// <returns>Dictionary containing all properties</returns>
        public static Dictionary<string, object> GetProperties(Type VariableType)
        {
            // Get field for specified variable
            var Properties = VariableType.GetProperties();
            var PropertyDict = new Dictionary<string, object>();

            // Get the properties and get their values
            foreach (PropertyInfo VarProperty in Properties)
            {
                var PropertyValue = VarProperty.GetValue(VariableType);
                PropertyDict.Add(VarProperty.Name, PropertyValue);
            }
            return PropertyDict;
        }

        /// <summary>
        /// Gets the properties from the type without evaluation
        /// </summary>
        /// <param name="VariableType">Variable type</param>
        /// <returns>Dictionary containing all properties</returns>
        public static Dictionary<string, Type> GetPropertiesNoEvaluation(Type VariableType)
        {
            // Get field for specified variable
            var Properties = VariableType.GetProperties();
            var PropertyDict = new Dictionary<string, Type>();

            // Get the properties and get their values
            foreach (PropertyInfo VarProperty in Properties)
                PropertyDict.Add(VarProperty.Name, VarProperty.PropertyType);
            return PropertyDict;
        }

    }
}