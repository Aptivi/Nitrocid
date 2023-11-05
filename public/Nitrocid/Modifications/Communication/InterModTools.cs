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
using KS.Users.Permissions;

namespace KS.Modifications.Communication
{
    /// <summary>
    /// Inter-Mod Communication tools
    /// </summary>
    public static class InterModTools
    {

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
            var modParts = modInfo.ModParts;
            foreach (var modPart in modParts.Keys)
            {
                // Get a mod part
                var mod = modParts[modPart];
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available functions from mod {0} part {1}...", modInfo.ModName, modPart);

                // Get a list of functions
                var functions = mod.PartScript.PubliclyAvailableFunctions;
                if (functions is null || functions.Count == 0)
                    continue;

                // Assuming that we have functions, get a single function containing that name
                if (!functions.ContainsKey(functionName))
                    continue;

                // Assuming that we have that function, get a single function delegate
                var function = functions[functionName];
                if (function is null)
                    continue;

                // The function instance is valid. Try to dynamically invoke it.
                return function.DynamicInvoke(args: parameters);
            }
            return null;
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
            var modParts = modInfo.ModParts;
            foreach (var modPart in modParts.Keys)
            {
                // Get a mod part
                var mod = modParts[modPart];
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available properties from mod {0} part {1}...", modInfo.ModName, modPart);

                // Get a list of properties
                var propertys = mod.PartScript.PubliclyAvailableProperties;
                if (propertys is null || propertys.Count == 0)
                    continue;

                // Assuming that we have properties, get a single property containing that name
                if (!propertys.ContainsKey(propertyName))
                    continue;

                // Assuming that we have that property, get a single property delegate
                var property = propertys[propertyName];
                if (property is null)
                    continue;

                // Check to see if this property is static
                var get = property.GetGetMethod();
                if (get is null)
                    continue;

                // The property instance is valid. Try to get a value from it.
                return get.Invoke(null, null);
            }
            return null;
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
            var modParts = modInfo.ModParts;
            foreach (var modPart in modParts.Keys)
            {
                // Get a mod part
                var mod = modParts[modPart];
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available properties from mod {0} part {1}...", modInfo.ModName, modPart);

                // Get a list of properties
                var propertys = mod.PartScript.PubliclyAvailableProperties;
                if (propertys is null || propertys.Count == 0)
                    continue;

                // Assuming that we have properties, get a single property containing that name
                if (!propertys.ContainsKey(propertyName))
                    continue;

                // Assuming that we have that property, get a single property delegate
                var property = propertys[propertyName];
                if (property is null)
                    continue;

                // Check to see if this property is static
                var set = property.GetSetMethod();
                if (set is null)
                    continue;

                // The property instance is valid. Try to get a value from it.
                set.Invoke(null, new[] { value });
            }
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
            var modParts = modInfo.ModParts;
            foreach (var modPart in modParts.Keys)
            {
                // Get a mod part
                var mod = modParts[modPart];
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available fields from mod {0} part {1}...", modInfo.ModName, modPart);

                // Get a list of fields
                var fields = mod.PartScript.PubliclyAvailableFields;
                if (fields is null || fields.Count == 0)
                    continue;

                // Assuming that we have fields, get a single field containing that name
                if (!fields.ContainsKey(fieldName))
                    continue;

                // Assuming that we have that field, get a single field delegate
                var field = fields[fieldName];
                if (field is null)
                    continue;

                // Check to see if this field is static
                if (!field.IsStatic)
                    continue;
                var get = field.GetValue(null);
                if (get is null)
                    continue;
                return get;
            }
            return null;
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
            var modParts = modInfo.ModParts;
            foreach (var modPart in modParts.Keys)
            {
                // Get a mod part
                var mod = modParts[modPart];
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to get list of available fields from mod {0} part {1}...", modInfo.ModName, modPart);

                // Get a list of fields
                var fields = mod.PartScript.PubliclyAvailableFields;
                if (fields is null || fields.Count == 0)
                    continue;

                // Assuming that we have fields, get a single field containing that name
                if (!fields.ContainsKey(fieldName))
                    continue;

                // Assuming that we have that field, get a single field delegate
                var field = fields[fieldName];
                if (field is null)
                    continue;

                // Check to see if this field is static
                if (!field.IsStatic)
                    continue;

                // The field instance is valid. Try to get a value from it.
                field.SetValue(null, value);
            }
        }

    }
}
