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

using System;
using System.IO;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nitrocid.Files;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.ShellPacks.Shells.Json;

namespace Nitrocid.ShellPacks.Tools
{
    /// <summary>
    /// JSON shell tools
    /// </summary>
    public static class JsonTools
    {

        /// <summary>
        /// Opens the JSON file
        /// </summary>
        /// <param name="File">Target file. We recommend you to use <see cref="FilesystemTools.NeutralizePath(string, bool)"></see> to neutralize path.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool OpenJsonFile(string File)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to open file {0}...", vars: [File]);
                JsonShellCommon.FileStream = new FileStream(File, FileMode.Open);
                var JsonFileReader = new StreamReader(JsonShellCommon.FileStream);
                string JsonFileContents = FilesystemTools.ReadToEndAndSeek(ref JsonFileReader);
                JsonShellCommon.FileToken = JToken.Parse(!string.IsNullOrWhiteSpace(JsonFileContents) ? JsonFileContents : "{}");
                JsonShellCommon.FileTokenOrig = JToken.Parse(!string.IsNullOrWhiteSpace(JsonFileContents) ? JsonFileContents : "{}");
                DebugWriter.WriteDebug(DebugLevel.I, "File {0} is open. Length: {1}, Pos: {2}", vars: [File, JsonShellCommon.FileStream.Length, JsonShellCommon.FileStream.Position]);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Open file {0} failed: {1}", vars: [File, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Closes the JSON file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool CloseJsonFile()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to close file...");
                JsonShellCommon.FileStream?.Close();
                JsonShellCommon.FileStream = null;
                DebugWriter.WriteDebug(DebugLevel.I, "File is no longer open.");
                JsonShellCommon.FileToken = JToken.Parse("{}");
                JsonShellCommon.FileTokenOrig = JToken.Parse("{}");
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Closing file failed: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Saves JSON file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SaveFile(bool ClearJson) =>
            SaveFile(ClearJson, JsonShellCommon.Formatting);

        /// <summary>
        /// Saves JSON file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SaveFile(bool ClearJson, Formatting Formatting)
        {
            try
            {
                if (JsonShellCommon.FileStream is null)
                    throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("JSON file is not open yet."));
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to save file...");
                JsonShellCommon.FileStream.SetLength(0L);
                DebugWriter.WriteDebug(DebugLevel.I, "Length set to 0.");
                var FileLinesByte = Encoding.Default.GetBytes(JsonConvert.SerializeObject(JsonShellCommon.FileToken, Formatting));
                DebugWriter.WriteDebug(DebugLevel.I, "Converted lines to bytes. Length: {0}", vars: [FileLinesByte.Length]);
                JsonShellCommon.FileStream.Write(FileLinesByte, 0, FileLinesByte.Length);
                JsonShellCommon.FileStream.Flush();
                DebugWriter.WriteDebug(DebugLevel.I, "File is saved.");
                if (ClearJson)
                    JsonShellCommon.FileToken = JToken.Parse("{}");
                JsonShellCommon.FileTokenOrig = JToken.Parse("{}");
                JsonShellCommon.FileTokenOrig = JsonShellCommon.FileToken;
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Saving file failed: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Handles autosave
        /// </summary>
        public static void HandleAutoSaveJsonFile()
        {
            if (JsonShellCommon.AutoSaveFlag)
            {
                try
                {
                    Thread.Sleep(JsonShellCommon.AutoSaveInterval * 1000);
                    if (JsonShellCommon.FileStream is not null)
                        SaveFile(false);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
        }

        /// <summary>
        /// Was JSON edited?
        /// </summary>
        public static bool WasJsonEdited() =>
            !JToken.DeepEquals(JsonShellCommon.FileToken, JsonShellCommon.FileTokenOrig);

        /// <summary>
        /// Gets the root type
        /// </summary>
        /// <returns>Root JToken type</returns>
        public static JTokenType DetermineRootType() =>
            JsonShellCommon.FileToken.Root.Type;

        /// <summary>
        /// Gets the root type
        /// </summary>
        /// <param name="path">Path to the target object, array, or property</param>
        /// <returns>Root JToken type</returns>
        public static JTokenType DetermineType(string path)
        {
            var token = GetTokenSafe(path);
            if (token is null)
                return JTokenType.None;
            return token.Type;
        }

        /// <summary>
        /// Gets a token in the JSON file
        /// </summary>
        /// <param name="path">The path to a token. You can use JSONPath.</param>
        public static JToken GetToken(string path)
        {
            if (JsonShellCommon.FileStream is not null)
            {
                var TargetToken = JsonShellCommon.FileToken.SelectToken(path);
                if (TargetToken is not null)
                    return TargetToken;
                else
                    throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The token inside the JSON file isn't found."));
            }
            else
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The JSON editor hasn't opened a file stream yet."));
        }

        /// <summary>
        /// Gets a token in the JSON file. It returns null if not found.
        /// </summary>
        /// <param name="path">The path to a token. You can use JSONPath.</param>
        public static JToken? GetTokenSafe(string path)
        {
            if (JsonShellCommon.FileStream is not null)
            {
                var TargetToken = JsonShellCommon.FileToken.SelectToken(path);
                if (TargetToken is not null)
                    return TargetToken;
                else
                    return null;
            }
            else
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The JSON editor hasn't opened a file stream yet."));
        }

        /// <summary>
        /// Gets a token in the JSON file. It returns null if not found.
        /// </summary>
        /// <param name="parentToken">Where is the target token found?</param>
        /// <param name="path">The path to a token. You can use JSONPath.</param>
        public static JToken? GetTokenSafe(string parentToken, string path)
        {
            if (JsonShellCommon.FileStream is not null)
            {
                var TargetToken = GetToken(parentToken);
                TargetToken = TargetToken.SelectToken(path);
                if (TargetToken is not null)
                    return TargetToken;
                else
                    return null;
            }
            else
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The JSON editor hasn't opened a file stream yet."));
        }

        /// <summary>
        /// Adds a new object, array, or property to the current JSON file
        /// </summary>
        /// <param name="parent">Where is the target to perform an operation on? Use JSONPath.</param>
        /// <param name="type">Either object, array, property, or raw</param>
        /// <param name="propName">Property name. Must be empty for non-object parent token type</param>
        /// <param name="value">Value. It'll be automatically processed into the form of ["value"] for arrays, {} for objects, "value" for properties, and value for raw.</param>
        public static void Add(string parent, string type, string propName, string value)
        {
            // First, do some sanity checks, starting from the parent token
            var parentToken = GetTokenSafe(parent) ??
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The parent token is not found. Make sure that you've written the path '{0}' correctly."), parent);

            // Then, the new object type
            if (!type.Equals("array", StringComparison.OrdinalIgnoreCase) &&
                !type.Equals("object", StringComparison.OrdinalIgnoreCase) &&
                !type.Equals("property", StringComparison.OrdinalIgnoreCase) &&
                !type.Equals("raw", StringComparison.OrdinalIgnoreCase))
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The specified type '{0}' is invalid."), type);

            // Then, the new object's property name (if applicable)
            var parentTokenType = DetermineType(parent);
            if (parentTokenType != JTokenType.Object && !string.IsNullOrEmpty(propName))
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("Can't append a new item with the property name with the parent token type of '{0}'."), parentTokenType.ToString());

            // Finally, parse the string JSON token
            JToken? newToken = default;
            switch (type.ToLower())
            {
                case "array":
                    if (parentTokenType == JTokenType.Object && !string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"[\"{value}\"]");
                    else if (parentTokenType != JTokenType.Object && string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"[\"{value}\"]");
                    else
                        throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("Can't append a new item with the property name '{0}' with the parent token type of '{1}'."), propName, parentTokenType.ToString());
                    break;
                case "object":
                    if (parentTokenType == JTokenType.Object && !string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{{}}");
                    else if (parentTokenType != JTokenType.Object && string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{{}}");
                    else
                        throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("Can't append a new item with the property name '{0}' with the parent token type of '{1}'."), propName, parentTokenType.ToString());
                    break;
                case "property":
                    if (parentTokenType == JTokenType.Object && !string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"\"{value}\"");
                    else
                        throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("Can't append a new item with the property name with the parent token type of '{0}'."), parentTokenType.ToString());
                    break;
                case "raw":
                    if (parentTokenType == JTokenType.Object && !string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{value}");
                    else if (parentTokenType != JTokenType.Object && string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{value}");
                    else
                        throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("Can't append a new item with the property name '{0}' with the parent token type of '{1}'."), propName, parentTokenType.ToString());
                    break;
            }
            switch (parentTokenType)
            {
                case JTokenType.Array:
                    if (newToken is not null)
                        ((JArray)parentToken).Add(newToken);
                    break;
                case JTokenType.Object:
                    ((JObject)parentToken).Add(propName, newToken);
                    break;
            }
        }

        /// <summary>
        /// Sets a value to an existing object, array, or property in the current JSON file
        /// </summary>
        /// <param name="parent">Where is the target to perform an operation on? Use JSONPath.</param>
        /// <param name="type">Either object, array, property, or raw</param>
        /// <param name="propName">Property name. Must be empty for non-object parent token type</param>
        /// <param name="value">Value. It'll be automatically processed into the form of ["value"] for arrays, {} for objects, "value" for properties, and value for raw.</param>
        public static void Set(string parent, string type, string propName, string value)
        {
            // First, do some sanity checks, starting from the parent token
            var parentToken = GetTokenSafe(parent) ??
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The parent token is not found. Make sure that you've written the path '{0}' correctly."), parent);

            // Then, the new object type
            if (!type.Equals("array", StringComparison.OrdinalIgnoreCase) &&
                !type.Equals("object", StringComparison.OrdinalIgnoreCase) &&
                !type.Equals("property", StringComparison.OrdinalIgnoreCase) &&
                !type.Equals("raw", StringComparison.OrdinalIgnoreCase))
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The specified type '{0}' is invalid."), type);

            // Then, the new object's property name (if applicable)
            var parentTokenType = DetermineType(parent);
            if (parentTokenType != JTokenType.Object && !string.IsNullOrEmpty(propName))
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("Can't append a new item with the property name with the parent token type of '{0}'."), parentTokenType.ToString());

            // Finally, parse the string JSON token
            JToken? newToken = default;
            switch (type.ToLower())
            {
                case "array":
                    if (parentTokenType == JTokenType.Object && !string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"[\"{value}\"]");
                    else if (parentTokenType != JTokenType.Object && string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"[\"{value}\"]");
                    else
                        throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("Can't append a new item with the property name '{0}' with the parent token type of '{1}'."), propName, parentTokenType.ToString());
                    break;
                case "object":
                    if (parentTokenType == JTokenType.Object && !string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{{}}");
                    else if (parentTokenType != JTokenType.Object && string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{{}}");
                    else
                        throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("Can't append a new item with the property name '{0}' with the parent token type of '{1}'."), propName, parentTokenType.ToString());
                    break;
                case "property":
                    if (parentTokenType == JTokenType.Object && !string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"\"{value}\"");
                    else
                        throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("Can't append a new item with the property name with the parent token type of '{0}'."), parentTokenType.ToString());
                    break;
                case "raw":
                    if (parentTokenType == JTokenType.Object && !string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{value}");
                    else if (parentTokenType != JTokenType.Object && string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{value}");
                    else
                        throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("Can't append a new item with the property name '{0}' with the parent token type of '{1}'."), propName, parentTokenType.ToString());
                    break;
            }
            switch (parentTokenType)
            {
                case JTokenType.Object:
                    if (parentToken[propName] is null)
                        throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("Property name '{0}' within parent '{1}', type '{2}', doesn't exist"), propName, parent, parentTokenType.ToString());
                    parentToken[propName] = newToken;
                    break;
                default:
                    if (newToken is not null)
                        parentToken.Replace(newToken);
                    break;
            }
        }

        /// <summary>
        /// Removes an object, array, or property from the current JSON file
        /// </summary>
        /// <param name="parent">Where is the target to perform an operation on? Use JSONPath.</param>
        public static void Remove(string parent)
        {
            // First, do some sanity checks, starting from the parent token
            var parentToken = GetTokenSafe(parent) ??
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The parent token is not found. Make sure that you've written the path '{0}' correctly."), parent);
            if (parentToken.Parent is null)
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The parent token is not found. Make sure that you've written the path '{0}' correctly."), parent);

            // Then, do the deletion
            if (parentToken.Type != JTokenType.Array && parentToken.Type != JTokenType.Object && parentToken.Type != JTokenType.Property ||
                parentToken.Parent.Type == JTokenType.Property)
                parentToken.Parent.Remove();
            else
                parentToken.Remove();
        }

        /// <summary>
        /// Serializes the property to the string
        /// </summary>
        /// <param name="Property">The property. You can use JSONPath.</param>
        public static string SerializeToString(string Property)
        {
            var TargetToken = GetToken(Property);
            return JsonConvert.SerializeObject(TargetToken, Formatting.Indented);
        }

    }
}
