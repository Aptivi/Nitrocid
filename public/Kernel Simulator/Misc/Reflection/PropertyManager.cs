
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
using System.Linq.Expressions;
using System.Reflection;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;

namespace KS.Misc.Reflection
{
    /// <summary>
    /// Property management module
    /// </summary>
    public static class PropertyManager
    {

        private static readonly Dictionary<string, Action<object>> cachedSetters = new();
        private static readonly Dictionary<string, Func<object>> cachedGetters = new();

        /// <summary>
        /// Sets the value of a property to the new value dynamically
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <param name="VariableValue">New value</param>
        public static void SetPropertyValue(string Variable, object VariableValue) => 
            SetPropertyValue(Variable, VariableValue, null);

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
                // Expressions are claimed that it's faster than Reflection, but let's see!
                DebugWriter.WriteDebug(DebugLevel.I, "Got field {0}. Setting to {1}...", TargetProperty.Name, VariableValue);
                ExpressionSetPropertyValue(TargetProperty, VariableValue);
            }
            else
            {
                // Variable not found on any of the "flag" modules.
                DebugWriter.WriteDebug(DebugLevel.I, "Property {0} not found.", Variable);
                throw new KernelException(KernelExceptionType.NoSuchReflectionVariable, Translate.DoTranslation("Variable {0} is not found on any of the modules."), Variable);
            }
        }

        private static void ExpressionSetPropertyValue(PropertyInfo propertyInfo, object value)
        {
            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            string cachedName = $"{propertyInfo.DeclaringType.FullName} - {propertyInfo.Name}";
            if (cachedSetters.ContainsKey(cachedName))
            {
                var cachedExpression = cachedSetters[cachedName];
                cachedExpression(value);
                return;
            }

            var argumentParam = Expression.Parameter(typeof(object));
            var convExpr = Expression.Convert(argumentParam, propertyInfo.PropertyType);
            var callExpr = Expression.Call(propertyInfo.GetSetMethod(), convExpr);

            var expression = Expression.Lambda<Action<object>>(callExpr, argumentParam).Compile();

            cachedSetters.Add(cachedName, expression);
            var finalValue = Convert.ChangeType(value, propertyInfo.PropertyType);
            expression(finalValue);
        }

        /// <summary>
        /// Gets the value of a property dynamically 
        /// </summary>
        /// <param name="Variable">Property name. Use operator NameOf to get name.</param>
        /// <returns>Value of a property</returns>
        public static object GetPropertyValue(string Variable) => GetPropertyValue(Variable, null);

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
                DebugWriter.WriteDebug(DebugLevel.I, "Got field {0}.", TargetProperty.Name);
                return ExpressionGetPropertyValue(TargetProperty);
            }
            else
            {
                // Variable not found on any of the "flag" modules.
                DebugWriter.WriteDebug(DebugLevel.I, "Property {0} not found.", Variable);
                throw new KernelException(KernelExceptionType.NoSuchReflectionVariable, Translate.DoTranslation("Variable {0} is not found on any of the modules."), Variable);
            }
        }

        private static object ExpressionGetPropertyValue(PropertyInfo propertyInfo)
        {
            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));

            string cachedName = $"{propertyInfo.DeclaringType.FullName} - {propertyInfo.Name}";
            if (cachedGetters.ContainsKey(cachedName))
            {
                var cachedExpression = cachedGetters[cachedName];
                return cachedExpression();
            }

            var callExpr = Expression.Call(propertyInfo.GetGetMethod());
            var convExpr = Expression.Convert(callExpr, typeof(object));

            var expression = Expression.Lambda<Func<object>>(convExpr).Compile();

            cachedGetters.Add(cachedName, expression);
            return expression();
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
            PossibleTypes = ReflectionCommon.KernelTypes;

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
                var PropertyValue = ExpressionGetPropertyValue(VarProperty);
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
