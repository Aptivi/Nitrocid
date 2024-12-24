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

using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nitrocid.Kernel.Extensions
{
    /// <summary>
    /// Inter-Addon Communication tools
    /// </summary>
    public static class InterAddonTools
    {

        /// <summary>
        /// Lists all the available functions from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <returns>List of function names</returns>
        /// <exception cref="KernelException"></exception>
        public static string[] ListAvailableFunctions(string addonName)
        {
            var funcs = new List<string>();

            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.InteraddonCommunication);

            // Get the addon
            var addonInfo = AddonTools.GetAddon(addonName) ??
                throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("Can't obtain list of functions"));

            // Get a list of functions
            var functions = addonInfo.Addon.PubliclyAvailableFunctions;
            if (functions is null || functions.Count == 0)
                return [];

            // Add all of the functions!
            funcs.AddRange(functions.Keys);
            return [.. funcs];
        }

        /// <summary>
        /// Lists all the available properties from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <returns>List of property names</returns>
        /// <exception cref="KernelException"></exception>
        public static string[] ListAvailableProperties(string addonName)
        {
            var funcs = new List<string>();

            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.InteraddonCommunication);

            // Get the addon
            var addonInfo = AddonTools.GetAddon(addonName) ??
                throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("Can't obtain list of properties"));

            // Get a list of properties
            var properties = addonInfo.Addon.PubliclyAvailableProperties;
            if (properties is null || properties.Count == 0)
                return [];

            // Add all of the properties!
            funcs.AddRange(properties.Keys);
            return [.. funcs];
        }

        /// <summary>
        /// Lists all the available fields from the addon name
        /// </summary>
        /// <param name="addonName">Addon name to check</param>
        /// <returns>List of field names</returns>
        /// <exception cref="KernelException"></exception>
        public static string[] ListAvailableFields(string addonName)
        {
            var funcs = new List<string>();

            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.InteraddonCommunication);

            // Get the addon
            var addonInfo = AddonTools.GetAddon(addonName) ??
                throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("Can't obtain list of fields"));

            // Get a list of fields
            var fields = addonInfo.Addon.PubliclyAvailableFields;
            if (fields is null || fields.Count == 0)
                return [];

            // Add all of the fields!
            funcs.AddRange(fields.Keys);
            return [.. funcs];
        }

        /// <summary>
        /// Executes a custom addon function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="functionName">Function name defined in the <see cref="IAddon.PubliclyAvailableFunctions"/> dictionary to query</param>
        public static object? ExecuteCustomAddonFunction(KnownAddons addonName, string functionName) =>
            ExecuteCustomAddonFunction(InterAddonTranslations.GetAddonName(addonName), functionName, null);

        /// <summary>
        /// Executes a custom addon function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="functionName">Function name defined in the <see cref="IAddon.PubliclyAvailableFunctions"/> dictionary to query</param>
        /// <param name="parameters">Parameters to execute the function with</param>
        public static object? ExecuteCustomAddonFunction(KnownAddons addonName, string functionName, params object?[]? parameters) =>
            ExecuteCustomAddonFunction(InterAddonTranslations.GetAddonName(addonName), functionName, parameters);

        /// <summary>
        /// Executes a custom addon function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="functionName">Function name defined in the <see cref="IAddon.PubliclyAvailableFunctions"/> dictionary to query</param>
        public static object? ExecuteCustomAddonFunction(string addonName, string functionName) =>
            ExecuteCustomAddonFunction(addonName, functionName, null);

        /// <summary>
        /// Executes a custom addon function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="functionName">Function name defined in the <see cref="IAddon.PubliclyAvailableFunctions"/> dictionary to query</param>
        /// <param name="parameters">Parameters to execute the function with</param>
        public static object? ExecuteCustomAddonFunction(string addonName, string functionName, params object?[]? parameters)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.InteraddonCommunication);

            // Get a function
            var function = GetFunctionDelegate(addonName, functionName);
            if (function is null)
                return null;

            // The function instance is valid. Try to dynamically invoke it.
            return function.DynamicInvoke(args: parameters);
        }

        /// <summary>
        /// Gets a value from the custom addon property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="propertyName">Property name defined in the <see cref="IAddon.PubliclyAvailableProperties"/> dictionary to query</param>
        public static object? GetCustomAddonPropertyValue(KnownAddons addonName, string propertyName) =>
            GetCustomAddonPropertyValue(InterAddonTranslations.GetAddonName(addonName), propertyName);

        /// <summary>
        /// Gets a value from the custom addon property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="propertyName">Property name defined in the <see cref="IAddon.PubliclyAvailableProperties"/> dictionary to query</param>
        public static object? GetCustomAddonPropertyValue(string addonName, string propertyName)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.InteraddonCommunication);

            // Get the property
            var property = GetPropertyInfo(addonName, propertyName);
            if (property is null)
                return null;

            // Check to see if this property is static
            var get = property.GetGetMethod();
            if (get is null)
                return null;

            // The property instance is valid. Try to get a value from it.
            return get.Invoke(null, null);
        }

        /// <summary>
        /// Sets a value from the custom addon property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="propertyName">Property name defined in the <see cref="IAddon.PubliclyAvailableProperties"/> dictionary to query</param>
        /// <param name="value">Value to set the property to</param>
        public static void SetCustomAddonPropertyValue(KnownAddons addonName, string propertyName, object? value) =>
            SetCustomAddonPropertyValue(InterAddonTranslations.GetAddonName(addonName), propertyName, value);

        /// <summary>
        /// Sets a value from the custom addon property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="propertyName">Property name defined in the <see cref="IAddon.PubliclyAvailableProperties"/> dictionary to query</param>
        /// <param name="value">Value to set the property to</param>
        public static void SetCustomAddonPropertyValue(string addonName, string propertyName, object? value)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.InteraddonCommunication);

            // Get the property
            var property = GetPropertyInfo(addonName, propertyName);
            if (property is null)
                return;

            // Check to see if this property is static
            var set = property.GetSetMethod();
            if (set is null)
                return;

            // The property instance is valid. Try to get a value from it.
            set.Invoke(null, [value]);
        }

        /// <summary>
        /// Gets a value from the custom addon field
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="fieldName">Field name defined in the <see cref="IAddon.PubliclyAvailableFields"/> dictionary to query</param>
        public static object? GetCustomAddonFieldValue(KnownAddons addonName, string fieldName) =>
            GetCustomAddonFieldValue(InterAddonTranslations.GetAddonName(addonName), fieldName);

        /// <summary>
        /// Gets a value from the custom addon field
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="fieldName">Field name defined in the <see cref="IAddon.PubliclyAvailableFields"/> dictionary to query</param>
        public static object? GetCustomAddonFieldValue(string addonName, string fieldName)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.InteraddonCommunication);

            // Get the field
            var field = GetFieldInfo(addonName, fieldName) ??
                throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("Can't get field info for") + $" {addonName} -> {fieldName}");

            // The field instance is valid. Try to get a value from it.
            var get = field.GetValue(null);
            return get;
        }

        /// <summary>
        /// Sets a value from the custom addon field
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="fieldName">Field name defined in the <see cref="IAddon.PubliclyAvailableFields"/> dictionary to query</param>
        /// <param name="value">Value to set the field to</param>
        public static void SetCustomAddonFieldValue(KnownAddons addonName, string fieldName, object? value) =>
            SetCustomAddonFieldValue(InterAddonTranslations.GetAddonName(addonName), fieldName, value);

        /// <summary>
        /// Sets a value from the custom addon field
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="fieldName">Field name defined in the <see cref="IAddon.PubliclyAvailableFields"/> dictionary to query</param>
        /// <param name="value">Value to set the field to</param>
        public static void SetCustomAddonFieldValue(string addonName, string fieldName, object? value)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.InteraddonCommunication);

            // Get the field
            var field = GetFieldInfo(addonName, fieldName) ??
                throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("Can't get field info for") + $" {addonName} -> {fieldName}");

            // The field instance is valid. Try to set a value.
            field.SetValue(null, value);
        }

        /// <summary>
        /// Gets the function parameters from a function
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="functionName">Function name defined in the <see cref="IAddon.PubliclyAvailableFunctions"/> dictionary to query</param>
        /// <returns>An array of <see cref="ParameterInfo"/> if there are any; null if there is no function</returns>
        public static ParameterInfo[]? GetFunctionParameters(string addonName, string functionName)
        {
            var function = GetFunctionDelegate(addonName, functionName);
            if (function is null)
                return null;

            // Get the function parameters
            return function.Method.GetParameters();
        }

        /// <summary>
        /// Gets the property setter parameters from a property
        /// </summary>
        /// <param name="addonName">The addon name to query</param>
        /// <param name="propertyName">Property name defined in the <see cref="IAddon.PubliclyAvailableProperties"/> dictionary to query</param>
        /// <returns>An array of <see cref="ParameterInfo"/> if there are any; null if there is no property</returns>
        public static ParameterInfo[]? GetSetPropertyParameters(string addonName, string propertyName)
        {
            var property = GetPropertyInfo(addonName, propertyName);
            if (property is null)
                return null;

            // Get the property parameters
            var get = property.GetSetMethod();
            if (get is null)
                return null;
            return get.GetParameters();
        }

        private static Delegate? GetFunctionDelegate(string addonName, string functionName)
        {
            // Get the addon
            var addonInfo = AddonTools.GetAddon(addonName) ??
                throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("Can't execute custom function with non-existent addon"));

            // Now, check the list of available functions
            var addon = addonInfo.Addon;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available functions from addon {0}...", addonInfo.AddonName);

            // Get a list of functions
            var functions = addon.PubliclyAvailableFunctions;
            if (functions is null || functions.Count == 0)
                return null;

            // Assuming that we have functions, get a single function containing that name
            if (!functions.TryGetValue(functionName, out Delegate? @delegate))
                throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("Can't find function '{0}' in addon '{1}'."), functionName, addonInfo.AddonName);

            // Assuming that we have that function, get a single function delegate
            var function = @delegate;
            return function;
        }

        private static PropertyInfo? GetPropertyInfo(string addonName, string propertyName)
        {
            // Get the addon
            var addonInfo = AddonTools.GetAddon(addonName) ??
                throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("Can't execute custom function with non-existent addon"));

            // Now, check the list of available properties
            var addon = addonInfo.Addon;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available properties from addon {0}...", addonInfo.AddonName);

            // Get a list of properties
            var properties = addon.PubliclyAvailableProperties;
            if (properties is null || properties.Count == 0)
                return null;

            // Assuming that we have properties, get a single property containing that name
            if (!properties.TryGetValue(propertyName, out PropertyInfo? propertyInfo))
                throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("Can't find property '{0}' in addon '{1}'."), propertyName, addonInfo.AddonName);

            // Assuming that we have that property, get a single property delegate
            var property = propertyInfo;
            return property;
        }

        private static FieldInfo? GetFieldInfo(string addonName, string fieldName)
        {
            // Get the addon
            var addonInfo = AddonTools.GetAddon(addonName) ??
                throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("Can't execute custom function with non-existent addon"));

            // Now, check the list of available fields
            var addon = addonInfo.Addon;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available fields from addon {0}...", addonInfo.AddonName);

            // Get a list of fields
            var fields = addon.PubliclyAvailableFields;
            if (fields is null || fields.Count == 0)
                return null;

            // Assuming that we have fields, get a single field containing that name
            if (!fields.TryGetValue(fieldName, out FieldInfo? fieldInfo))
                throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("Can't find field '{0}' in addon '{1}'."), fieldName, addonInfo.AddonName);

            // Assuming that we have that field, get a single field delegate
            var field = fieldInfo;
            if (field is null)
                return null;

            // Check to see if this field is static
            if (!field.IsStatic)
                return null;
            return field;
        }

    }
}
