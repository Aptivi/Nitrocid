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
using System.Collections.Generic;
using System.Reflection;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Misc.Reflection
{
    /// <summary>
    /// Property management module
    /// </summary>
    public static class PropertyManager
    {
        /// <summary>
        /// Sets the value of a property to the new value dynamically
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <param name="VariableValue">New value</param>
        public static void SetPropertyValue(string Variable, object? VariableValue) =>
            SetPropertyValue(Variable, VariableValue, null);

        /// <summary>
        /// Sets the value of a property to the new value dynamically
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <param name="VariableValue">New value</param>
        /// <param name="VariableType">Property type</param>
        public static void SetPropertyValue(string Variable, object? VariableValue, Type? VariableType)
        {
            // Get property for specified variable
            PropertyInfo? TargetProperty;
            if (VariableType is not null)
                TargetProperty = GetProperty(Variable, VariableType);
            else
                TargetProperty = GetProperty(Variable);

            // Set the variable if found
            SetPropertyValue(TargetProperty, VariableValue);
        }

        /// <summary>
        /// Sets the value of a property to the new value dynamically
        /// </summary>
        /// <param name="Variable">Property info instance.</param>
        /// <param name="VariableValue">New value</param>
        public static void SetPropertyValue(PropertyInfo? Variable, object? VariableValue)
        {
            // Check the property
            if (Variable is null)
            {
                // Variable not found on any of the modules.
                DebugWriter.WriteDebug(DebugLevel.E, "Property not found.");
                throw new KernelException(KernelExceptionType.NoSuchReflectionVariable, LanguageTools.GetLocalized("NKS_MISC_REFLECTION_EXCEPTION_NOSUCHVAR"));
            }

            // The "obj" description says this: "The object whose property value will be set."
            // Apparently, SetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
            // Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
            DebugWriter.WriteDebug(DebugLevel.I, "Got property {0}. Setting to {1}...", vars: [Variable.Name, VariableValue]);
            Variable.SetValue(Variable, VariableValue);
        }

        /// <summary>
        /// Sets the value of a property to the new value dynamically
        /// </summary>
        /// <param name="instance">Instance class to make changes on</param>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <param name="VariableValue">New value</param>
        public static void SetPropertyValueInstance<T>(T instance, string Variable, object? VariableValue) =>
            SetPropertyValueInstance(instance, Variable, VariableValue, null);

        /// <summary>
        /// Sets the value of a property to the new value dynamically
        /// </summary>
        /// <param name="instance">Instance class to make changes on</param>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <param name="VariableValue">New value</param>
        /// <param name="VariableType">Property type</param>
        public static void SetPropertyValueInstance<T>(T instance, string Variable, object? VariableValue, Type? VariableType)
        {
            // Get property for specified variable
            PropertyInfo? TargetProperty;
            if (VariableType is not null)
                TargetProperty = GetProperty(Variable, VariableType);
            else
                TargetProperty = GetProperty(Variable);

            // Set the variable if found
            SetPropertyValueInstance(instance, TargetProperty, VariableValue);
        }

        /// <summary>
        /// Sets the value of a property to the new value dynamically
        /// </summary>
        /// <param name="instance">Instance class to make changes on</param>
        /// <param name="Variable">Property info instance.</param>
        /// <param name="VariableValue">New value</param>
        public static void SetPropertyValueInstance<T>(T instance, PropertyInfo? Variable, object? VariableValue)
        {
            // Check the property
            if (Variable is null)
            {
                // Variable not found on any of the modules.
                DebugWriter.WriteDebug(DebugLevel.E, "Property not found in instance type {0}.", vars: [instance?.GetType().Name ?? "<unknown type>"]);
                throw new KernelException(KernelExceptionType.NoSuchReflectionVariable, LanguageTools.GetLocalized("NKS_MISC_REFLECTION_EXCEPTION_NOSUCHVAR"));
            }

            // This is how to set a value in instance variables.
            DebugWriter.WriteDebug(DebugLevel.I, "Got property {0}. Setting to {1} in instance type {2}...", vars: [Variable.Name, VariableValue, instance?.GetType().Name ?? "<unknown type>"]);
            Variable.SetValue(instance, VariableValue);
        }

        /// <summary>
        /// Sets the value of a property to the new value dynamically
        /// </summary>
        /// <param name="instance">Instance class to make changes on</param>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <param name="VariableValue">New value</param>
        /// <param name="VariableType">Property type</param>
        public static void SetPropertyValueInstanceExplicit(object instance, string Variable, object? VariableValue, Type? VariableType)
        {
            // Get property for specified variable
            PropertyInfo? TargetProperty = default;
            if (VariableType is not null)
                TargetProperty = GetProperty(Variable, VariableType);

            // Set the variable if found
            SetPropertyValueInstance(instance, TargetProperty, VariableValue);
        }

        /// <summary>
        /// Gets the value of a property dynamically 
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <returns>Value of a property</returns>
        public static object? GetPropertyValue(string Variable) =>
            GetPropertyValue(Variable, null);

        /// <summary>
        /// Gets the value of a property dynamically 
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <param name="VariableType">Property type</param>
        /// <returns>Value of a property</returns>
        public static object? GetPropertyValue(string Variable, Type? VariableType)
        {
            // Get property for specified variable
            PropertyInfo? TargetProperty;
            if (VariableType is not null)
                TargetProperty = GetProperty(Variable, VariableType);
            else
                TargetProperty = GetProperty(Variable);

            // Get the variable if found
            return GetPropertyValue(TargetProperty);
        }

        /// <summary>
        /// Gets the value of a property dynamically 
        /// </summary>
        /// <param name="Variable">Property info instance.</param>
        /// <returns>Value of a property</returns>
        public static object? GetPropertyValue(PropertyInfo? Variable)
        {
            // Check the property
            if (Variable is null)
            {
                // Variable not found on any of the modules.
                DebugWriter.WriteDebug(DebugLevel.E, "Property not found.");
                throw new KernelException(KernelExceptionType.NoSuchReflectionVariable, LanguageTools.GetLocalized("NKS_MISC_REFLECTION_EXCEPTION_NOSUCHVAR"));
            }

            // The "obj" description says this: "The object whose property value will be set."
            // Apparently, SetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
            // Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
            DebugWriter.WriteDebug(DebugLevel.I, "Got property {0}.", vars: [Variable.Name]);
            return Variable.GetValue(Variable);
        }

        /// <summary>
        /// Gets the value of a property dynamically 
        /// </summary>
        /// <param name="instance">Instance class to fetch value from</param>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <returns>Value of a property</returns>
        public static object? GetPropertyValueInstance<T>(T instance, string Variable) =>
            GetPropertyValueInstance(instance, Variable, null);

        /// <summary>
        /// Gets the value of a property dynamically 
        /// </summary>
        /// <param name="instance">Instance class to fetch value from</param>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <param name="VariableType">Property type</param>
        /// <returns>Value of a property</returns>
        public static object? GetPropertyValueInstance<T>(T instance, string Variable, Type? VariableType)
        {
            // Get property for specified variable
            PropertyInfo? TargetProperty;
            if (VariableType is not null)
                TargetProperty = GetProperty(Variable, VariableType);
            else
                TargetProperty = GetProperty(Variable);

            // Get the variable if found
            return GetPropertyValueInstance(instance, TargetProperty);
        }

        /// <summary>
        /// Gets the value of a property dynamically 
        /// </summary>
        /// <param name="instance">Instance class to fetch value from</param>
        /// <param name="Variable">Property info instance.</param>
        /// <returns>Value of a property</returns>
        public static object? GetPropertyValueInstance<T>(T instance, PropertyInfo? Variable)
        {
            // Check the property
            if (Variable is null)
            {
                // Variable not found on any of the modules.
                DebugWriter.WriteDebug(DebugLevel.E, "Property not found.");
                throw new KernelException(KernelExceptionType.NoSuchReflectionVariable, LanguageTools.GetLocalized("NKS_MISC_REFLECTION_EXCEPTION_NOSUCHVAR"));
            }

            // This is how to get a value in instance variables.
            DebugWriter.WriteDebug(DebugLevel.I, "Got property {0}.", vars: [Variable.Name]);
            return Variable.GetValue(instance);
        }

        /// <summary>
        /// Gets the value of a property dynamically 
        /// </summary>
        /// <param name="instance">Instance class to fetch value from</param>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <param name="VariableType">Property type</param>
        /// <returns>Value of a property</returns>
        public static object? GetPropertyValueInstanceExplicit(object instance, string Variable, Type? VariableType)
        {
            // Get property for specified variable
            PropertyInfo? TargetProperty = default;
            if (VariableType is not null)
                TargetProperty = GetProperty(Variable, VariableType);

            // Get the variable if found
            return GetPropertyValueInstance(instance, TargetProperty);
        }

        /// <summary>
        /// Gets a property from variable name
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <param name="Type">Property type</param>
        /// <returns>Property information</returns>
        public static PropertyInfo? GetProperty(string Variable, Type Type)
        {
            // Get properties of specified type
            var PropertyInstance = Type.GetProperty(Variable);

            // Check if any of them contains the specified variable
            if (PropertyInstance is not null)
                return PropertyInstance;
            return null;
        }

        /// <summary>
        /// Gets a property from variable name
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <returns>Property information</returns>
        public static PropertyInfo? GetProperty(string Variable)
        {
            Type[] PossibleTypes;
            PropertyInfo? PossibleProperty;

            // Get types of possible flag locations
            PossibleTypes = ReflectionCommon.KernelTypes;

            // Get properties of flag modules
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
            // Get property for specified variable
            var TargetProperty = GetProperty(Variable);

            // Set the variable if found
            return TargetProperty is not null;
        }

        /// <summary>
        /// Checks the specified property if it exists
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <param name="Type">Property type</param>
        public static bool CheckProperty(string Variable, Type Type)
        {
            // Get property for specified variable
            var TargetProperty = GetProperty(Variable, Type);

            // Set the variable if found
            return TargetProperty is not null;
        }

        /// <summary>
        /// Gets the properties from the type dynamically
        /// </summary>
        /// <param name="VariableType">Variable type</param>
        /// <returns>Dictionary containing all properties</returns>
        public static Dictionary<string, object?> GetProperties(Type VariableType)
        {
            // Get property for specified variable
            var Properties = VariableType.GetProperties();
            var PropertyDict = new Dictionary<string, object?>();

            // Get the properties and get their values
            foreach (PropertyInfo VarProperty in Properties)
            {
                try
                {
                    var PropertyValue = VarProperty.GetValue(VariableType);
                    PropertyDict.Add(VarProperty.Name, PropertyValue);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"Error getting property value {VarProperty.Name} for {VariableType.Name}: {ex.Message}");
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            return PropertyDict;
        }

        /// <summary>
        /// Gets the properties from the type dynamically
        /// </summary>
        /// <param name="VariableType">Variable type</param>
        /// <param name="instance">Instance</param>
        /// <returns>Dictionary containing all properties</returns>
        public static Dictionary<string, object?> GetProperties<T>(T instance, Type VariableType)
        {
            // Get property for specified variable
            var Properties = VariableType.GetProperties();
            var PropertyDict = new Dictionary<string, object?>();

            // Get the properties and get their values
            foreach (PropertyInfo VarProperty in Properties)
            {
                try
                {
                    var PropertyValue = VarProperty.GetValue(instance);
                    PropertyDict.Add(VarProperty.Name, PropertyValue);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"Error getting property value {VarProperty.Name} for {VariableType.Name}: {ex.Message}");
                    DebugWriter.WriteDebugStackTrace(ex);
                }
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
            // Get property for specified variable
            var Properties = VariableType.GetProperties();
            var PropertyDict = new Dictionary<string, Type>();

            // Get the properties and get their values
            foreach (PropertyInfo VarProperty in Properties)
                PropertyDict.Add(VarProperty.Name, VarProperty.PropertyType);
            return PropertyDict;
        }

        /// <summary>
        /// Gets all the properties from the type dynamically
        /// </summary>
        /// <returns>Dictionary containing all properties</returns>
        public static Dictionary<string, object?> GetAllProperties()
        {
            // Get property for specified variable
            var PropertyDict = new Dictionary<string, object?>();
            foreach (var type in ReflectionCommon.KernelTypes)
            {
                try
                {
                    var properties = GetProperties(type);
                    foreach (var property in properties)
                        PropertyDict.Add(property.Key, property.Value);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"Error getting property value for {type.Name}: {ex.Message}");
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            return PropertyDict;
        }

        /// <summary>
        /// Gets all the properties from the type without evaluation
        /// </summary>
        /// <returns>Dictionary containing all properties</returns>
        public static Dictionary<string, Type> GetAllPropertiesNoEvaluation()
        {
            // Get property for specified variable
            var PropertyDict = new Dictionary<string, Type>();
            foreach (var type in ReflectionCommon.KernelTypes)
            {
                try
                {
                    var properties = GetPropertiesNoEvaluation(type);
                    foreach (var property in properties)
                        PropertyDict.Add(property.Key, property.Value);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"Error getting property for {type.Name}: {ex.Message}");
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            return PropertyDict;
        }

        /// <summary>
        /// Gets all the properties from the type dynamically
        /// </summary>
        /// <param name="instance">Instance</param>
        /// <returns>Dictionary containing all properties</returns>
        public static Dictionary<string, object?> GetAllProperties<T>(T instance)
        {
            // Get property for specified variable
            var PropertyDict = new Dictionary<string, object?>();
            foreach (var type in ReflectionCommon.KernelTypes)
            {
                try
                {
                    var properties = GetProperties(instance, type);
                    foreach (var property in properties)
                        PropertyDict.Add(property.Key, property.Value);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"Error getting property value for {type.Name}: {ex.Message}");
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            return PropertyDict;
        }
    }
}
