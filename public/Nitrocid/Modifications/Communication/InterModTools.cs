//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace KS.Modifications.Communication
{
    /// <summary>
    /// Inter-Mod Communication tools
    /// </summary>
    public static class InterModTools
    {

        /// <summary>
        /// Lists all the available functions from the mod name
        /// </summary>
        /// <param name="modName">Mod name to check</param>
        /// <returns>List of function names</returns>
        /// <exception cref="KernelException"></exception>
        public static string[] ListAvailableFunctions(string modName)
        {
            var funcs = new List<string>();

            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Get the mod
            var modInfo = ModManager.GetMod(modName) ??
                throw new KernelException(KernelExceptionType.NoSuchMod, Translate.DoTranslation("Can't obtain list of functions"));

            // Now, check the list of available functions
            var mod = modInfo.ModScript;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available functions from mod {0}...", modInfo.ModName);

            // Get a list of functions
            var functions = mod.PubliclyAvailableFunctions;
            if (functions is null || functions.Count == 0)
                return [];

            // Add all of the functions!
            funcs.AddRange(functions.Keys);
            return [.. funcs];
        }

        /// <summary>
        /// Lists all the available properties from the mod name
        /// </summary>
        /// <param name="modName">Mod name to check</param>
        /// <returns>List of property names</returns>
        /// <exception cref="KernelException"></exception>
        public static string[] ListAvailableProperties(string modName)
        {
            var funcs = new List<string>();

            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Get the mod
            var modInfo = ModManager.GetMod(modName) ??
                throw new KernelException(KernelExceptionType.NoSuchMod, Translate.DoTranslation("Can't obtain list of properties"));

            // Now, check the list of available properties
            var mod = modInfo.ModScript;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available properties from mod {0}...", modInfo.ModName);

            // Get a list of properties
            var properties = mod.PubliclyAvailableProperties;
            if (properties is null || properties.Count == 0)
                return [];

            // Add all of the properties!
            funcs.AddRange(properties.Keys);
            return [.. funcs];
        }

        /// <summary>
        /// Lists all the available fields from the mod name
        /// </summary>
        /// <param name="modName">Mod name to check</param>
        /// <returns>List of field names</returns>
        /// <exception cref="KernelException"></exception>
        public static string[] ListAvailableFields(string modName)
        {
            var funcs = new List<string>();

            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Get the mod
            var modInfo = ModManager.GetMod(modName) ??
                throw new KernelException(KernelExceptionType.NoSuchMod, Translate.DoTranslation("Can't obtain list of fields"));

            // Now, check the list of available fields
            var mod = modInfo.ModScript;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available fields from mod {0}...", modInfo.ModName);

            // Get a list of fields
            var fields = mod.PubliclyAvailableFields;
            if (fields is null || fields.Count == 0)
                return [];

            // Add all of the fields!
            funcs.AddRange(fields.Keys);
            return [.. funcs];
        }

        /// <summary>
        /// Executes a custom mod function
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="functionName">Function name defined in the <see cref="IMod.PubliclyAvailableFunctions"/> dictionary to query</param>
        public static object ExecuteCustomModFunction(string modName, string functionName) =>
            ExecuteCustomModFunction(modName, functionName, null);

        /// <summary>
        /// Executes a custom mod function
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="functionName">Function name defined in the <see cref="IMod.PubliclyAvailableFunctions"/> dictionary to query</param>
        /// <param name="parameters">Parameters to execute the function with</param>
        public static object ExecuteCustomModFunction(string modName, string functionName, params object[] parameters)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Get the mod
            var modInfo = ModManager.GetMod(modName) ??
                throw new KernelException(KernelExceptionType.NoSuchMod, Translate.DoTranslation("Can't execute custom function with non-existent mod"));

            // Now, check the list of available functions
            var mod = modInfo.ModScript;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available functions from mod {0}...", modInfo.ModName);

            // Get a list of functions
            var functions = mod.PubliclyAvailableFunctions;
            if (functions is null || functions.Count == 0)
                return null;

            // Assuming that we have functions, get a single function containing that name
            if (!functions.TryGetValue(functionName, out Delegate function))
                return null;

            // Assuming that we have that function, get a single function delegate
            if (function is null)
                return null;

            // The function instance is valid. Try to dynamically invoke it.
            return function.DynamicInvoke(args: parameters);
        }

        /// <summary>
        /// Gets a value from the custom mod property
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="propertyName">Property name defined in the <see cref="IMod.PubliclyAvailableProperties"/> dictionary to query</param>
        public static object GetCustomModPropertyValue(string modName, string propertyName)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Get the mod
            var modInfo = ModManager.GetMod(modName) ??
                throw new KernelException(KernelExceptionType.NoSuchMod, Translate.DoTranslation("Can't execute custom property with non-existent mod"));

            // Now, check the list of available properties
            var mod = modInfo.ModScript;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available properties from mod {0}...", modInfo.ModName);

            // Get a list of properties
            var properties = mod.PubliclyAvailableProperties;
            if (properties is null || properties.Count == 0)
                return null;

            // Assuming that we have properties, get a single property containing that name
            if (!properties.TryGetValue(propertyName, out PropertyInfo property))
                return null;

            // Assuming that we have that property, get a single property delegate
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
        /// Sets a value from the custom mod property
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="propertyName">Property name defined in the <see cref="IMod.PubliclyAvailableProperties"/> dictionary to query</param>
        /// <param name="value">Value to set the property to</param>
        public static void SetCustomModPropertyValue(string modName, string propertyName, object value)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Get the mod
            var modInfo = ModManager.GetMod(modName) ??
                throw new KernelException(KernelExceptionType.NoSuchMod, Translate.DoTranslation("Can't execute custom property with non-existent mod"));

            // Now, check the list of available properties
            var mod = modInfo.ModScript;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available properties from mod {0}...", modInfo.ModName);

            // Get a list of properties
            var properties = mod.PubliclyAvailableProperties;
            if (properties is null || properties.Count == 0)
                return;

            // Assuming that we have properties, get a single property containing that name
            if (!properties.TryGetValue(propertyName, out PropertyInfo property))
                return;

            // Assuming that we have that property, get a single property delegate
            if (property is null)
                return;

            // Check to see if this property is static
            var set = property.GetSetMethod();
            if (set is null)
                return;

            // The property instance is valid. Try to get a value from it.
            set.Invoke(null, new[] { value });
        }

        /// <summary>
        /// Gets a value from the custom mod field
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="fieldName">Field name defined in the <see cref="IMod.PubliclyAvailableFields"/> dictionary to query</param>
        public static object GetCustomModFieldValue(string modName, string fieldName)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Get the mod
            var modInfo = ModManager.GetMod(modName) ??
                throw new KernelException(KernelExceptionType.NoSuchMod, Translate.DoTranslation("Can't execute custom field with non-existent mod"));

            // Now, check the list of available fields
            var mod = modInfo.ModScript;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available fields from mod {0}...", modInfo.ModName);

            // Get a list of fields
            var fields = mod.PubliclyAvailableFields;
            if (fields is null || fields.Count == 0)
                return null;

            // Assuming that we have fields, get a single field containing that name
            if (!fields.TryGetValue(fieldName, out FieldInfo field))
                return null;

            // Assuming that we have that field, get a single field delegate
            if (field is null)
                return null;

            // Check to see if this field is static
            if (!field.IsStatic)
                return null;
            var get = field.GetValue(null);
            if (get is null)
                return null;
            return get;
        }

        /// <summary>
        /// Sets a value from the custom mod field
        /// </summary>
        /// <param name="modName">The mod name to query</param>
        /// <param name="fieldName">Field name defined in the <see cref="IMod.PubliclyAvailableFields"/> dictionary to query</param>
        /// <param name="value">Value to set the field to</param>
        public static void SetCustomModFieldValue(string modName, string fieldName, object value)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.IntermodCommunication);

            // Get the mod
            var modInfo = ModManager.GetMod(modName) ??
                throw new KernelException(KernelExceptionType.NoSuchMod, Translate.DoTranslation("Can't execute custom field with non-existent mod"));

            // Now, check the list of available fields
            var mod = modInfo.ModScript;
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available fields from mod {0}...", modInfo.ModName);

            // Get a list of fields
            var fields = mod.PubliclyAvailableFields;
            if (fields is null || fields.Count == 0)
                return;

            // Assuming that we have fields, get a single field containing that name
            if (!fields.TryGetValue(fieldName, out FieldInfo field))
                return;

            // Assuming that we have that field, get a single field delegate
            if (field is null)
                return;

            // Check to see if this field is static
            if (!field.IsStatic)
                return;

            // The field instance is valid. Try to get a value from it.
            field.SetValue(null, value);
        }

    }
}
