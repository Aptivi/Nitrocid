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

using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nitrocid.Base.Kernel.Extensions
{
    /// <summary>
    /// Inter-Addon Communication tools
    /// </summary>
    public static class InterAddonTools
    {
        /// <summary>
        /// Lists all the available types from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <returns>List of function names</returns>
        /// <exception cref="KernelException"></exception>
        public static Type[] ListAvailableTypes(KnownAddons addonName) =>
            ListAvailableTypes(InterAddonTranslations.GetAddonName(addonName));

        /// <summary>
        /// Lists all the available types from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <returns>List of function names</returns>
        /// <exception cref="KernelException"></exception>
        public static Type[] ListAvailableTypes(string addonName)
        {
            var asm = GetAddonAssembly(addonName);
            var types = asm.GetTypes().Where((type) => type.IsPublic && type.IsVisible);
            return types.ToArray();
        }

        /// <summary>
        /// Gets a type from an addon
        /// </summary>
        /// <param name="addonName">Addon name</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <returns>List of function names</returns>
        /// <exception cref="KernelException"></exception>
        public static Type GetTypeFromAddon(KnownAddons addonName, string typeName) =>
            GetTypeFromAddon(InterAddonTranslations.GetAddonName(addonName), typeName);

        /// <summary>
        /// Gets a type from an addon
        /// </summary>
        /// <param name="addonName">Addon name</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <returns>List of function names</returns>
        /// <exception cref="KernelException"></exception>
        public static Type GetTypeFromAddon(string addonName, string typeName)
        {
            // Get the addon types and get the type
            var types = ListAvailableTypes(addonName);
            var type = types.SingleOrDefault(t => t.FullName == typeName) ??
                throw new KernelException(KernelExceptionType.AddonManagement, LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_INTERADDON_EXCEPTION_CLASSTYPENOTFOUND"));
            return type;
        }

        /// <summary>
        /// Lists all the available functions from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <returns>List of function names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, MethodInfo> ListAvailableFunctions(KnownAddons addonName, string typeName) =>
            ListAvailableFunctions(InterAddonTranslations.GetAddonName(addonName), typeName);

        /// <summary>
        /// Lists all the available functions from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <returns>List of function names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, MethodInfo> ListAvailableFunctions(KnownAddons addonName, Type type) =>
            ListAvailableFunctions(InterAddonTranslations.GetAddonName(addonName), type);

        /// <summary>
        /// Lists all the available functions from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <returns>List of function names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, MethodInfo> ListAvailableFunctions(string addonName, string typeName)
        {
            // Get the type
            var type = GetTypeFromAddon(addonName, typeName);
            return ListAvailableFunctions(addonName, type);
        }

        /// <summary>
        /// Lists all the available functions from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <returns>List of function names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, MethodInfo> ListAvailableFunctions(string addonName, Type type)
        {
            // Verify the type before proceeding
            VerifyType(addonName, type);

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
        /// Lists all the available properties from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <returns>List of property names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, PropertyInfo> ListAvailableProperties(KnownAddons addonName, string typeName) =>
            ListAvailableProperties(InterAddonTranslations.GetAddonName(addonName), typeName);

        /// <summary>
        /// Lists all the available properties from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <returns>List of property names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, PropertyInfo> ListAvailableProperties(KnownAddons addonName, Type type) =>
            ListAvailableProperties(InterAddonTranslations.GetAddonName(addonName), type);

        /// <summary>
        /// Lists all the available properties from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <returns>List of property names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, PropertyInfo> ListAvailableProperties(string addonName, string typeName)
        {
            // Get the type
            var type = GetTypeFromAddon(addonName, typeName);
            return ListAvailableProperties(addonName, type);
        }

        /// <summary>
        /// Lists all the available properties from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <returns>List of property names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, PropertyInfo> ListAvailableProperties(string addonName, Type type)
        {
            // Verify the type before proceeding
            VerifyType(addonName, type);

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
        /// Lists all the available fields from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <returns>List of field names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, FieldInfo> ListAvailableFields(KnownAddons addonName, string typeName) =>
            ListAvailableFields(InterAddonTranslations.GetAddonName(addonName), typeName);

        /// <summary>
        /// Lists all the available fields from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <returns>List of field names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, FieldInfo> ListAvailableFields(KnownAddons addonName, Type type) =>
            ListAvailableFields(InterAddonTranslations.GetAddonName(addonName), type);

        /// <summary>
        /// Lists all the available fields from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <returns>List of field names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, FieldInfo> ListAvailableFields(string addonName, string typeName)
        {
            // Get the type
            var type = GetTypeFromAddon(addonName, typeName);
            return ListAvailableFields(addonName, type);
        }

        /// <summary>
        /// Lists all the available fields from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <returns>List of field names</returns>
        /// <exception cref="KernelException"></exception>
        public static Dictionary<string, FieldInfo> ListAvailableFields(string addonName, Type type)
        {
            // Verify the type before proceeding
            VerifyType(addonName, type);

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
        /// Executes a custom addon function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available addon functions from an addon type.</param>
        public static object? ExecuteCustomAddonFunction(KnownAddons addonName, string functionName, string typeName) =>
            ExecuteCustomAddonFunction(InterAddonTranslations.GetAddonName(addonName), functionName, typeName, null);

        /// <summary>
        /// Executes a custom addon function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available addon functions from an addon type.</param>
        /// <param name="parameters">Parameters to execute the function with</param>
        public static object? ExecuteCustomAddonFunction(KnownAddons addonName, string functionName, string typeName, params object?[]? parameters) =>
            ExecuteCustomAddonFunction(InterAddonTranslations.GetAddonName(addonName), functionName, typeName, parameters);

        /// <summary>
        /// Executes a custom addon function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available addon functions from an addon type.</param>
        public static object? ExecuteCustomAddonFunction(string addonName, string functionName, string typeName) =>
            ExecuteCustomAddonFunction(addonName, functionName, typeName, null);

        /// <summary>
        /// Executes a custom addon function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available addon functions from an addon type.</param>
        /// <param name="parameters">Parameters to execute the function with</param>
        public static object? ExecuteCustomAddonFunction(string addonName, string functionName, string typeName, params object?[]? parameters)
        {
            // Get the type
            var type = GetTypeFromAddon(addonName, typeName);
            return ExecuteCustomAddonFunction(addonName, functionName, type, parameters);
        }

        /// <summary>
        /// Executes a custom addon function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available addon functions from an addon type.</param>
        public static object? ExecuteCustomAddonFunction(KnownAddons addonName, string functionName, Type type) =>
            ExecuteCustomAddonFunction(InterAddonTranslations.GetAddonName(addonName), functionName, type, null);

        /// <summary>
        /// Executes a custom addon function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available addon functions from an addon type.</param>
        /// <param name="parameters">Parameters to execute the function with</param>
        public static object? ExecuteCustomAddonFunction(KnownAddons addonName, string functionName, Type type, params object?[]? parameters) =>
            ExecuteCustomAddonFunction(InterAddonTranslations.GetAddonName(addonName), functionName, type, parameters);

        /// <summary>
        /// Executes a custom addon function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available addon functions from an addon type.</param>
        public static object? ExecuteCustomAddonFunction(string addonName, string functionName, Type type) =>
            ExecuteCustomAddonFunction(addonName, functionName, type, null);

        /// <summary>
        /// Executes a custom addon function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available addon functions from an addon type.</param>
        /// <param name="parameters">Parameters to execute the function with</param>
        public static object? ExecuteCustomAddonFunction(string addonName, string functionName, Type type, params object?[]? parameters)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.InteraddonCommunication);

            // Verify the type before proceeding
            VerifyType(addonName, type);

            // Get a function
            var function = GetFunctionInfo(addonName, functionName, type);
            if (function is null)
                return null;

            // The function instance is valid. Try to dynamically invoke it.
            return MethodManager.InvokeMethodStatic(function, parameters);
        }

        /// <summary>
        /// Gets a value from the custom addon property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available addon properties from an addon type.</param>
        public static object? GetCustomAddonPropertyValue(KnownAddons addonName, string propertyName, string typeName) =>
            GetCustomAddonPropertyValue(InterAddonTranslations.GetAddonName(addonName), propertyName, typeName);

        /// <summary>
        /// Gets a value from the custom addon property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available addon properties from an addon type.</param>
        public static object? GetCustomAddonPropertyValue(string addonName, string propertyName, string typeName)
        {
            // Get the type
            var type = GetTypeFromAddon(addonName, typeName);
            return GetCustomAddonPropertyValue(addonName, propertyName, type);
        }

        /// <summary>
        /// Gets a value from the custom addon property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available addon properties from an addon type.</param>
        public static object? GetCustomAddonPropertyValue(KnownAddons addonName, string propertyName, Type type) =>
            GetCustomAddonPropertyValue(InterAddonTranslations.GetAddonName(addonName), propertyName, type);

        /// <summary>
        /// Gets a value from the custom addon property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available addon properties from an addon type.</param>
        public static object? GetCustomAddonPropertyValue(string addonName, string propertyName, Type type)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.InteraddonCommunication);

            // Verify the type before proceeding
            VerifyType(addonName, type);

            // Get the property
            var property = GetPropertyInfo(addonName, propertyName, type);
            if (property is null)
                return null;

            // Try to get a value from it.
            return PropertyManager.GetPropertyValue(property);
        }

        /// <summary>
        /// Sets a value from the custom addon property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available addon properties from an addon type.</param>
        /// <param name="value">Value to set the property to</param>
        public static void SetCustomAddonPropertyValue(KnownAddons addonName, string propertyName, string typeName, object? value) =>
            SetCustomAddonPropertyValue(InterAddonTranslations.GetAddonName(addonName), propertyName, typeName, value);

        /// <summary>
        /// Sets a value from the custom addon property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available addon properties from an addon type.</param>
        /// <param name="value">Value to set the property to</param>
        public static void SetCustomAddonPropertyValue(string addonName, string propertyName, string typeName, object? value)
        {
            // Get the type
            var type = GetTypeFromAddon(addonName, typeName);
            SetCustomAddonPropertyValue(addonName, propertyName, type, value);
        }

        /// <summary>
        /// Sets a value from the custom addon property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available addon properties from an addon type.</param>
        /// <param name="value">Value to set the property to</param>
        public static void SetCustomAddonPropertyValue(KnownAddons addonName, string propertyName, Type type, object? value) =>
            SetCustomAddonPropertyValue(InterAddonTranslations.GetAddonName(addonName), propertyName, type, value);

        /// <summary>
        /// Sets a value from the custom addon property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available addon properties from an addon type.</param>
        /// <param name="value">Value to set the property to</param>
        public static void SetCustomAddonPropertyValue(string addonName, string propertyName, Type type, object? value)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.InteraddonCommunication);

            // Verify the type before proceeding
            VerifyType(addonName, type);

            // Get the property
            var property = GetPropertyInfo(addonName, propertyName, type);
            if (property is null)
                return;

            // Try to set a value in it.
            PropertyManager.SetPropertyValue(property, value);
        }

        /// <summary>
        /// Gets a value from the custom addon field
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="fieldName">Field name to query. You can use the <see cref="ListAvailableFields(string, Type)"/> method to query all available addon fields from an addon type.</param>
        public static object? GetCustomAddonFieldValue(KnownAddons addonName, string fieldName, string typeName) =>
            GetCustomAddonFieldValue(InterAddonTranslations.GetAddonName(addonName), fieldName, typeName);

        /// <summary>
        /// Gets a value from the custom addon field
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="fieldName">Field name to query. You can use the <see cref="ListAvailableFields(string, Type)"/> method to query all available addon fields from an addon type.</param>
        public static object? GetCustomAddonFieldValue(string addonName, string fieldName, string typeName)
        {
            // Get the type
            var type = GetTypeFromAddon(addonName, typeName);
            return GetCustomAddonFieldValue(addonName, fieldName, type);
        }

        /// <summary>
        /// Gets a value from the custom addon field
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="fieldName">Field name to query. You can use the <see cref="ListAvailableFields(string, Type)"/> method to query all available addon fields from an addon type.</param>
        public static object? GetCustomAddonFieldValue(KnownAddons addonName, string fieldName, Type type) =>
            GetCustomAddonFieldValue(InterAddonTranslations.GetAddonName(addonName), fieldName, type);

        /// <summary>
        /// Gets a value from the custom addon field
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="fieldName">Field name to query. You can use the <see cref="ListAvailableFields(string, Type)"/> method to query all available addon fields from an addon type.</param>
        public static object? GetCustomAddonFieldValue(string addonName, string fieldName, Type type)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.InteraddonCommunication);

            // Verify the type before proceeding
            VerifyType(addonName, type);

            // Get the field
            var field = GetFieldInfo(addonName, fieldName, type) ??
                throw new KernelException(KernelExceptionType.AddonManagement, LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_INTERADDON_EXCEPTION_FIELDINFO") + $" {addonName} -> {fieldName}");

            // Try to get a value from it.
            return FieldManager.GetFieldValue(field);
        }

        /// <summary>
        /// Sets a value from the custom addon field
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="fieldName">Field name to query. You can use the <see cref="ListAvailableFields(string, Type)"/> method to query all available addon fields from an addon type.</param>
        /// <param name="value">Value to set the field to</param>
        public static void SetCustomAddonFieldValue(KnownAddons addonName, string fieldName, string typeName, object? value) =>
            SetCustomAddonFieldValue(InterAddonTranslations.GetAddonName(addonName), fieldName, typeName, value);

        /// <summary>
        /// Sets a value from the custom addon field
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="fieldName">Field name to query. You can use the <see cref="ListAvailableFields(string, Type)"/> method to query all available addon fields from an addon type.</param>
        /// <param name="value">Value to set the field to</param>
        public static void SetCustomAddonFieldValue(string addonName, string fieldName, string typeName, object? value)
        {
            // Get the type
            var type = GetTypeFromAddon(addonName, typeName);
            SetCustomAddonFieldValue(addonName, fieldName, type, value);
        }

        /// <summary>
        /// Sets a value from the custom addon field
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="fieldName">Field name to query. You can use the <see cref="ListAvailableFields(string, Type)"/> method to query all available addon fields from an addon type.</param>
        /// <param name="value">Value to set the field to</param>
        public static void SetCustomAddonFieldValue(KnownAddons addonName, string fieldName, Type type, object? value) =>
            SetCustomAddonFieldValue(InterAddonTranslations.GetAddonName(addonName), fieldName, type, value);

        /// <summary>
        /// Sets a value from the custom addon field
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="fieldName">Field name to query. You can use the <see cref="ListAvailableFields(string, Type)"/> method to query all available addon fields from an addon type.</param>
        /// <param name="value">Value to set the field to</param>
        public static void SetCustomAddonFieldValue(string addonName, string fieldName, Type type, object? value)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.InteraddonCommunication);

            // Verify the type before proceeding
            VerifyType(addonName, type);

            // Get the field
            var field = GetFieldInfo(addonName, fieldName, type) ??
                throw new KernelException(KernelExceptionType.AddonManagement, LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_INTERADDON_EXCEPTION_FIELDINFO") + $" {addonName} -> {fieldName}");

            // Try to set a value in it.
            FieldManager.SetFieldValue(field, value);
        }

        /// <summary>
        /// Gets the function parameters from a function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available addon functions from an addon type.</param>
        /// <returns>An array of <see cref="ParameterInfo"/> if there are any; null if there is no function</returns>
        public static ParameterInfo[]? GetFunctionParameters(KnownAddons addonName, string functionName, string typeName) =>
            GetFunctionParameters(InterAddonTranslations.GetAddonName(addonName), functionName, typeName);

        /// <summary>
        /// Gets the function parameters from a function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available addon functions from an addon type.</param>
        /// <returns>An array of <see cref="ParameterInfo"/> if there are any; null if there is no function</returns>
        public static ParameterInfo[]? GetFunctionParameters(KnownAddons addonName, string functionName, Type type) =>
            GetFunctionParameters(InterAddonTranslations.GetAddonName(addonName), functionName, type);

        /// <summary>
        /// Gets the function parameters from a function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available addon functions from an addon type.</param>
        /// <returns>An array of <see cref="ParameterInfo"/> if there are any; null if there is no function</returns>
        public static ParameterInfo[]? GetFunctionParameters(string addonName, string functionName, string typeName)
        {
            // Get the type
            var type = GetTypeFromAddon(addonName, typeName);
            return GetFunctionParameters(addonName, functionName, type);
        }

        /// <summary>
        /// Gets the function parameters from a function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="functionName">Function name to query. You can use the <see cref="ListAvailableFunctions(string, Type)"/> method to query all available addon functions from an addon type.</param>
        /// <returns>An array of <see cref="ParameterInfo"/> if there are any; null if there is no function</returns>
        public static ParameterInfo[]? GetFunctionParameters(string addonName, string functionName, Type type)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.InteraddonCommunication);

            // Verify the type before proceeding
            VerifyType(addonName, type);

            // Get the parameters
            var function = GetFunctionInfo(addonName, functionName, type);
            if (function is null)
                return null;

            // Get the function parameters
            return function.GetParameters();
        }

        /// <summary>
        /// Gets the property setter parameters from a property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available addon properties from an addon type.</param>
        /// <returns>An array of <see cref="ParameterInfo"/> if there are any; null if there is no property</returns>
        public static ParameterInfo[]? GetSetPropertyParameters(KnownAddons addonName, string propertyName, string typeName) =>
            GetSetPropertyParameters(InterAddonTranslations.GetAddonName(addonName), propertyName, typeName);

        /// <summary>
        /// Gets the property setter parameters from a property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available addon properties from an addon type.</param>
        /// <returns>An array of <see cref="ParameterInfo"/> if there are any; null if there is no property</returns>
        public static ParameterInfo[]? GetSetPropertyParameters(KnownAddons addonName, string propertyName, Type type) =>
            GetSetPropertyParameters(InterAddonTranslations.GetAddonName(addonName), propertyName, type);

        /// <summary>
        /// Gets the property setter parameters from a property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="typeName">Full name of the type found in an addon (must be public and static)</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available addon properties from an addon type.</param>
        /// <returns>An array of <see cref="ParameterInfo"/> if there are any; null if there is no property</returns>
        public static ParameterInfo[]? GetSetPropertyParameters(string addonName, string propertyName, string typeName)
        {
            // Get the type
            var type = GetTypeFromAddon(addonName, typeName);
            return GetSetPropertyParameters(addonName, propertyName, type);
        }

        /// <summary>
        /// Gets the property setter parameters from a property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="type">Type found in an addon (must be public and static). You can get this type using <see cref="GetTypeFromAddon(string, string)"/>.</param>
        /// <param name="propertyName">Property name to query. You can use the <see cref="ListAvailableProperties(string, Type)"/> method to query all available addon properties from an addon type.</param>
        /// <returns>An array of <see cref="ParameterInfo"/> if there are any; null if there is no property</returns>
        public static ParameterInfo[]? GetSetPropertyParameters(string addonName, string propertyName, Type type)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.InteraddonCommunication);

            // Verify the type before proceeding
            VerifyType(addonName, type);

            // Get the parameters
            var property = GetPropertyInfo(addonName, propertyName, type);
            if (property is null)
                return null;

            // Get the property parameters
            var get = property.GetSetMethod();
            if (get is null)
                return null;
            return get.GetParameters();
        }

        private static MethodInfo? GetFunctionInfo(string addonName, string functionName, Type type)
        {
            // Get the addon functions
            var functions = ListAvailableFunctions(addonName, type);

            // Assuming that we have functions, get a single function containing that name
            if (!functions.TryGetValue(functionName, out MethodInfo? methodInfo))
                throw new KernelException(KernelExceptionType.AddonManagement, LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_INTERADDON_EXCEPTION_FUNCTIONNOTFOUND"), functionName, addonName);

            // Assuming that we have that function, get a single function delegate
            return methodInfo;
        }

        private static PropertyInfo? GetPropertyInfo(string addonName, string propertyName, Type type)
        {
            // Get the addon properties
            var properties = ListAvailableProperties(addonName, type);

            // Assuming that we have properties, get a single property containing that name
            if (!properties.TryGetValue(propertyName, out PropertyInfo? propertyInfo))
                throw new KernelException(KernelExceptionType.AddonManagement, LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_INTERADDON_EXCEPTION_PROPERTYNOTFOUND"), propertyName, addonName);

            // Assuming that we have that property, get a single property delegate
            var property = propertyInfo;
            return property;
        }

        private static FieldInfo? GetFieldInfo(string addonName, string fieldName, Type type)
        {
            // Get the addon fields
            var fields = ListAvailableFields(addonName, type);

            // Assuming that we have fields, get a single field containing that name
            if (!fields.TryGetValue(fieldName, out FieldInfo? fieldInfo))
                throw new KernelException(KernelExceptionType.AddonManagement, LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_INTERADDON_EXCEPTION_FIELDNOTFOUND"), fieldName, addonName);

            // Assuming that we have that field, get a single field delegate
            var field = fieldInfo;
            if (field is null)
                return null;

            // Check to see if this field is static
            if (!field.IsStatic)
                return null;
            return field;
        }

        private static Assembly GetAddonAssembly(string addonName)
        {
            // Get the addon
            var addonInfo = AddonTools.GetAddon(addonName) ??
                throw new KernelException(KernelExceptionType.AddonManagement, LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_INTERADDON_EXCEPTION_CUSTOMFUNCTIONADDONNONEXISTENT"));

            // Now, check the list of available functions
            var addon = addonInfo.Addon;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available functions from addon {0}...", vars: [addonInfo.AddonName]);

            // Get an assembly
            return addon.GetType().Assembly;
        }

        private static void VerifyType(string addonName, Type type)
        {
            // Check to see if this type is a valid addon type
            var addonTypes = ListAvailableTypes(addonName);
            if (!addonTypes.Contains(type))
                throw new KernelException(KernelExceptionType.AddonManagement, LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_INTERADDON_EXCEPTION_NOTADDONCLASS") + $": {type.FullName}");
        }
    }
}
