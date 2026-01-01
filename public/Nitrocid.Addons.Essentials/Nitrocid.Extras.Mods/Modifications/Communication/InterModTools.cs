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

using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using Nitrocid.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nitrocid.Extras.Mods.Modifications.Communication
{
    /// <summary>
    /// Inter-Mod Communication tools
    /// </summary>
    public static class InterModTools
    {
        /// <summary>
        /// Lists all the available types from the mod name
        /// </summary>
        /// <param name="modName">Mod name to check</param>
        /// <returns>List of function names</returns>
        /// <exception cref="KernelException"></exception>
        public static Type[] ListAvailableTypes(string modName)
        {
            var asm = GetModAssembly(modName);
            var types = asm.GetTypes().Where((type) => type.IsPublic && type.IsVisible);
            return types.ToArray();
        }

        /// <summary>
        /// Gets a type from an mod
        /// </summary>
        /// <param name="modName">Mod name</param>
        /// <param name="typeName">Full name of the type found in an mod (must be public and static)</param>
        /// <returns>List of function names</returns>
        /// <exception cref="KernelException"></exception>
        public static Type GetTypeFromMod(string modName, string typeName)
        {
            // Get the mod types and get the type
            var types = ListAvailableTypes(modName);
            var type = types.SingleOrDefault(t => t.FullName == typeName) ??
                throw new KernelException(KernelExceptionType.ModManagement, Translate.DoTranslation("Can't return non-existent class type from mod assembly"));
            return type;
        }

        /// <summary>
        /// Lists all the available functions from the mod name
        /// </summary>
        /// <param name="modName">Mod name to check</param>
        /// <param name="typeName">Full name of the type found in an mod (must be public and static)</param>
        /// <returns>List of function names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, MethodInfo> ListAvailableFunctions(string modName, string typeName)
        {
            // Get the type
            var type = GetTypeFromMod(modName, typeName);
            return ListAvailableFunctions(modName, type);
        }

        /// <summary>
        /// Lists all the available functions from the mod name
        /// </summary>
        /// <param name="modName">Mod name to check</param>
        /// <param name="type">Type found in an mod (must be public and static). You can get this type using <see cref="GetTypeFromMod(string, string)"/>.</param>
        /// <returns>List of function names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, MethodInfo> ListAvailableFunctions(string modName, Type type)
        {
            // Verify the type before proceeding
            VerifyType(modName, type);

            // Now, get the methods from the type
            var methods = type.GetMethods();

            // Convert this array to the dictionary
            Dictionary<string, MethodInfo> functions = [];
            foreach (var method in methods)
            {
                string name = method.Name;
                string finalName = name;

                // Make sure that overloaded methods that have the same name don't conflict with another method
                int iter = 1;
                while (functions.ContainsKey(finalName))
                {
                    iter++;
                    finalName = name + iter;
                }

                // Add the entry
                functions.Add(finalName, method);
            }
            return functions;
        }

        /// <summary>
        /// Lists all the available properties from the mod name
        /// </summary>
        /// <param name="modName">Mod name to check</param>
        /// <param name="typeName">Full name of the type found in an mod (must be public and static)</param>
        /// <returns>List of property names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, PropertyInfo> ListAvailableProperties(string modName, string typeName)
        {
            // Get the type
            var type = GetTypeFromMod(modName, typeName);
            return ListAvailableProperties(modName, type);
        }

        /// <summary>
        /// Lists all the available properties from the mod name
        /// </summary>
        /// <param name="modName">Mod name to check</param>
        /// <param name="type">Type found in an mod (must be public and static). You can get this type using <see cref="GetTypeFromMod(string, string)"/>.</param>
        /// <returns>List of property names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, PropertyInfo> ListAvailableProperties(string modName, Type type)
        {
            // Verify the type before proceeding
            VerifyType(modName, type);

            // Now, get the properties from the type
            var properties = type.GetProperties();

            // Convert this array to the dictionary
            Dictionary<string, PropertyInfo> finalProperties = [];
            foreach (var property in properties)
            {
                string name = property.Name;
                finalProperties.Add(name, property);
            }
            return finalProperties;
        }

        /// <summary>
        /// Lists all the available fields from the mod name
        /// </summary>
        /// <param name="modName">Mod name to check</param>
        /// <param name="typeName">Full name of the type found in an mod (must be public and static)</param>
        /// <returns>List of field names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, FieldInfo> ListAvailableFields(string modName, string typeName)
        {
            // Get the type
            var type = GetTypeFromMod(modName, typeName);
            return ListAvailableFields(modName, type);
        }

        /// <summary>
        /// Lists all the available fields from the mod name
        /// </summary>
        /// <param name="modName">Mod name to check</param>
        /// <param name="type">Type found in an mod (must be public and static). You can get this type using <see cref="GetTypeFromMod(string, string)"/>.</param>
        /// <returns>List of field names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, FieldInfo> ListAvailableFields(string modName, Type type)
        {
            // Verify the type before proceeding
            VerifyType(modName, type);

            // Now, get the fields from the type
            var fields = type.GetFields();

            // Convert this array to the dictionary
            Dictionary<string, FieldInfo> finalFields = [];
            foreach (var field in fields)
            {
                string name = field.Name;
                finalFields.Add(name, field);
            }
            return finalFields;
        }

        /// <summary>
        /// Executes a custom mod function
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="typeName">Full name of the type found in an mod (must be public and static)</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available mod functions from an mod type.</param>
        public static object? ExecuteCustomModFunction(string modName, string functionName, string typeName) =>
            ExecuteCustomModFunction(modName, functionName, typeName, null);

        /// <summary>
        /// Executes a custom mod function
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="typeName">Full name of the type found in an mod (must be public and static)</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available mod functions from an mod type.</param>
        /// <param name="parameters">Parameters to execute the function with</param>
        public static object? ExecuteCustomModFunction(string modName, string functionName, string typeName, params object?[]? parameters)
        {
            // Get the type
            var type = GetTypeFromMod(modName, typeName);
            return ExecuteCustomModFunction(modName, functionName, type, parameters);
        }

        /// <summary>
        /// Executes a custom mod function
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="type">Type found in an mod (must be public and static). You can get this type using <see cref="GetTypeFromMod(string, string)"/>.</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available mod functions from an mod type.</param>
        public static object? ExecuteCustomModFunction(string modName, string functionName, Type type) =>
            ExecuteCustomModFunction(modName, functionName, type, null);

        /// <summary>
        /// Executes a custom mod function
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="type">Type found in an mod (must be public and static). You can get this type using <see cref="GetTypeFromMod(string, string)"/>.</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available mod functions from an mod type.</param>
        /// <param name="parameters">Parameters to execute the function with</param>
        public static object? ExecuteCustomModFunction(string modName, string functionName, Type type, params object?[]? parameters)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Verify the type before proceeding
            VerifyType(modName, type);

            // Get a function
            var function = GetFunctionInfo(modName, functionName, type);
            if (function is null)
                return null;

            // The function instance is valid. Try to dynamically invoke it.
            return MethodManager.InvokeMethodStatic(function, parameters);
        }

        /// <summary>
        /// Gets a value from the custom mod property
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="typeName">Full name of the type found in an mod (must be public and static)</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available mod properties from an mod type.</param>
        public static object? GetCustomModPropertyValue(string modName, string propertyName, string typeName)
        {
            // Get the type
            var type = GetTypeFromMod(modName, typeName);
            return GetCustomModPropertyValue(modName, propertyName, type);
        }

        /// <summary>
        /// Gets a value from the custom mod property
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="type">Type found in an mod (must be public and static). You can get this type using <see cref="GetTypeFromMod(string, string)"/>.</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available mod properties from an mod type.</param>
        public static object? GetCustomModPropertyValue(string modName, string propertyName, Type type)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Verify the type before proceeding
            VerifyType(modName, type);

            // Get the property
            var property = GetPropertyInfo(modName, propertyName, type);
            if (property is null)
                return null;

            // Try to get a value from it.
            return PropertyManager.GetPropertyValue(property);
        }

        /// <summary>
        /// Sets a value from the custom mod property
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="typeName">Full name of the type found in an mod (must be public and static)</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available mod properties from an mod type.</param>
        /// <param name="value">Value to set the property to</param>
        public static void SetCustomModPropertyValue(string modName, string propertyName, string typeName, object? value)
        {
            // Get the type
            var type = GetTypeFromMod(modName, typeName);
            SetCustomModPropertyValue(modName, propertyName, type, value);
        }

        /// <summary>
        /// Sets a value from the custom mod property
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="type">Type found in an mod (must be public and static). You can get this type using <see cref="GetTypeFromMod(string, string)"/>.</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available mod properties from an mod type.</param>
        /// <param name="value">Value to set the property to</param>
        public static void SetCustomModPropertyValue(string modName, string propertyName, Type type, object? value)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Verify the type before proceeding
            VerifyType(modName, type);

            // Get the property
            var property = GetPropertyInfo(modName, propertyName, type);
            if (property is null)
                return;

            // Try to set a value in it.
            PropertyManager.SetPropertyValue(property, value);
        }

        /// <summary>
        /// Gets a value from the custom mod field
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="typeName">Full name of the type found in an mod (must be public and static)</param>
        /// <param name="fieldName">Field name to query. You can use the <see cref="ListAvailableFields(string, Type)"/> method to query all available mod fields from an mod type.</param>
        public static object? GetCustomModFieldValue(string modName, string fieldName, string typeName)
        {
            // Get the type
            var type = GetTypeFromMod(modName, typeName);
            return GetCustomModFieldValue(modName, fieldName, type);
        }

        /// <summary>
        /// Gets a value from the custom mod field
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="type">Type found in an mod (must be public and static). You can get this type using <see cref="GetTypeFromMod(string, string)"/>.</param>
        /// <param name="fieldName">Field name to query. You can use the <see cref="ListAvailableFields(string, Type)"/> method to query all available mod fields from an mod type.</param>
        public static object? GetCustomModFieldValue(string modName, string fieldName, Type type)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Verify the type before proceeding
            VerifyType(modName, type);

            // Get the field
            var field = GetFieldInfo(modName, fieldName, type) ??
                throw new KernelException(KernelExceptionType.ModManagement, Translate.DoTranslation("Can't get field info for") + $" {modName} -> {fieldName}");

            // Try to get a value from it.
            return FieldManager.GetFieldValue(field);
        }

        /// <summary>
        /// Sets a value from the custom mod field
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="typeName">Full name of the type found in an mod (must be public and static)</param>
        /// <param name="fieldName">Field name to query. You can use the <see cref="ListAvailableFields(string, Type)"/> method to query all available mod fields from an mod type.</param>
        /// <param name="value">Value to set the field to</param>
        public static void SetCustomModFieldValue(string modName, string fieldName, string typeName, object? value)
        {
            // Get the type
            var type = GetTypeFromMod(modName, typeName);
            SetCustomModFieldValue(modName, fieldName, type, value);
        }

        /// <summary>
        /// Sets a value from the custom mod field
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="type">Type found in an mod (must be public and static). You can get this type using <see cref="GetTypeFromMod(string, string)"/>.</param>
        /// <param name="fieldName">Field name to query. You can use the <see cref="ListAvailableFields(string, Type)"/> method to query all available mod fields from an mod type.</param>
        /// <param name="value">Value to set the field to</param>
        public static void SetCustomModFieldValue(string modName, string fieldName, Type type, object? value)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Verify the type before proceeding
            VerifyType(modName, type);

            // Get the field
            var field = GetFieldInfo(modName, fieldName, type) ??
                throw new KernelException(KernelExceptionType.ModManagement, Translate.DoTranslation("Can't get field info for") + $" {modName} -> {fieldName}");

            // Try to set a value in it.
            FieldManager.SetFieldValue(field, value);
        }

        /// <summary>
        /// Gets the function parameters from a function
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="typeName">Full name of the type found in an mod (must be public and static)</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available mod functions from an mod type.</param>
        /// <returns>An array of <see cref="ParameterInfo"/> if there are any; null if there is no function</returns>
        public static ParameterInfo[]? GetFunctionParameters(string modName, string functionName, string typeName)
        {
            // Get the type
            var type = GetTypeFromMod(modName, typeName);
            return GetFunctionParameters(modName, functionName, type);
        }

        /// <summary>
        /// Gets the function parameters from a function
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="type">Type found in an mod (must be public and static). You can get this type using <see cref="GetTypeFromMod(string, string)"/>.</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available mod functions from an mod type.</param>
        /// <returns>An array of <see cref="ParameterInfo"/> if there are any; null if there is no function</returns>
        public static ParameterInfo[]? GetFunctionParameters(string modName, string functionName, Type type)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Verify the type before proceeding
            VerifyType(modName, type);

            // Get the parameters
            var function = GetFunctionInfo(modName, functionName, type);
            if (function is null)
                return null;

            // Get the function parameters
            return function.GetParameters();
        }

        /// <summary>
        /// Gets the property setter parameters from a property
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="typeName">Full name of the type found in an mod (must be public and static)</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available mod properties from an mod type.</param>
        /// <returns>An array of <see cref="ParameterInfo"/> if there are any; null if there is no property</returns>
        public static ParameterInfo[]? GetSetPropertyParameters(string modName, string propertyName, string typeName)
        {
            // Get the type
            var type = GetTypeFromMod(modName, typeName);
            return GetSetPropertyParameters(modName, propertyName, type);
        }

        /// <summary>
        /// Gets the property setter parameters from a property
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="type">Type found in an mod (must be public and static). You can get this type using <see cref="GetTypeFromMod(string, string)"/>.</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available mod properties from an mod type.</param>
        /// <returns>An array of <see cref="ParameterInfo"/> if there are any; null if there is no property</returns>
        public static ParameterInfo[]? GetSetPropertyParameters(string modName, string propertyName, Type type)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Verify the type before proceeding
            VerifyType(modName, type);

            // Get the parameters
            var property = GetPropertyInfo(modName, propertyName, type);
            if (property is null)
                return null;

            // Get the property parameters
            var get = property.GetSetMethod();
            if (get is null)
                return null;
            return get.GetParameters();
        }

        private static MethodInfo? GetFunctionInfo(string modName, string functionName, Type type)
        {
            // Get the mod functions
            var functions = ListAvailableFunctions(modName, type);

            // Assuming that we have functions, get a single function containing that name
            if (!functions.TryGetValue(functionName, out MethodInfo? methodInfo))
                throw new KernelException(KernelExceptionType.ModManagement, Translate.DoTranslation("Can't find function '{0}' in mod '{1}'."), functionName, modName);

            // Assuming that we have that function, get a single function delegate
            return methodInfo;
        }

        private static PropertyInfo? GetPropertyInfo(string modName, string propertyName, Type type)
        {
            // Get the mod properties
            var properties = ListAvailableProperties(modName, type);

            // Assuming that we have properties, get a single property containing that name
            if (!properties.TryGetValue(propertyName, out PropertyInfo? propertyInfo))
                throw new KernelException(KernelExceptionType.ModManagement, Translate.DoTranslation("Can't find property '{0}' in mod '{1}'."), propertyName, modName);

            // Assuming that we have that property, get a single property delegate
            var property = propertyInfo;
            return property;
        }

        private static FieldInfo? GetFieldInfo(string modName, string fieldName, Type type)
        {
            // Get the mod fields
            var fields = ListAvailableFields(modName, type);

            // Assuming that we have fields, get a single field containing that name
            if (!fields.TryGetValue(fieldName, out FieldInfo? fieldInfo))
                throw new KernelException(KernelExceptionType.ModManagement, Translate.DoTranslation("Can't find field '{0}' in mod '{1}'."), fieldName, modName);

            // Assuming that we have that field, get a single field delegate
            var field = fieldInfo;
            if (field is null)
                return null;

            // Check to see if this field is static
            if (!field.IsStatic)
                return null;
            return field;
        }

        private static Assembly GetModAssembly(string modName)
        {
            // Get the mod
            var modInfo = ModManager.GetMod(modName) ??
                throw new KernelException(KernelExceptionType.ModManagement, Translate.DoTranslation("Can't execute custom function with non-existent mod"));

            // Now, check the list of available functions
            var mod = modInfo.ModScript;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available functions from mod {0}...", vars: [modInfo.ModName]);

            // Get an assembly
            return mod.GetType().Assembly;
        }

        private static void VerifyType(string modName, Type type)
        {
            // Check to see if this type is a valid mod type
            var modTypes = ListAvailableTypes(modName);
            if (!modTypes.Contains(type))
                throw new KernelException(KernelExceptionType.ModManagement, Translate.DoTranslation("This class is not an mod class") + $": {type.FullName}");
        }
    }
}
