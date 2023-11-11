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

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using KS.Files.Operations;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.Shells.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.Files.Editors.JsonShell
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
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to open file {0}...", File);
                JsonShellCommon.FileStream = new FileStream(File, FileMode.Open);
                var JsonFileReader = new StreamReader(JsonShellCommon.FileStream);
                string JsonFileContents = Reading.ReadToEndAndSeek(ref JsonFileReader);
                JsonShellCommon.FileToken = JToken.Parse(!string.IsNullOrWhiteSpace(JsonFileContents) ? JsonFileContents : "{}");
                JsonShellCommon.FileTokenOrig = JToken.Parse(!string.IsNullOrWhiteSpace(JsonFileContents) ? JsonFileContents : "{}");
                DebugWriter.WriteDebug(DebugLevel.I, "File {0} is open. Length: {1}, Pos: {2}", File, JsonShellCommon.FileStream.Length, JsonShellCommon.FileStream.Position);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Open file {0} failed: {1}", File, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Closes text file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool CloseTextFile()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to close file...");
                JsonShellCommon.FileStream.Close();
                JsonShellCommon.FileStream = null;
                DebugWriter.WriteDebug(DebugLevel.I, "File is no longer open.");
                JsonShellCommon.FileToken = JToken.Parse("{}");
                JsonShellCommon.FileTokenOrig = JToken.Parse("{}");
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Closing file failed: {0}", ex.Message);
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
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to save file...");
                JsonShellCommon.FileStream.SetLength(0L);
                DebugWriter.WriteDebug(DebugLevel.I, "Length set to 0.");
                var FileLinesByte = Encoding.Default.GetBytes(JsonConvert.SerializeObject(JsonShellCommon.FileToken, Formatting));
                DebugWriter.WriteDebug(DebugLevel.I, "Converted lines to bytes. Length: {0}", FileLinesByte.Length);
                JsonShellCommon.FileStream.Write(FileLinesByte, 0, FileLinesByte.Length);
                JsonShellCommon.FileStream.Flush();
                DebugWriter.WriteDebug(DebugLevel.I, "File is saved.");
                if (ClearJson)
                {
                    JsonShellCommon.FileToken = JToken.Parse("{}");
                }
                JsonShellCommon.FileTokenOrig = JToken.Parse("{}");
                JsonShellCommon.FileTokenOrig = JsonShellCommon.FileToken;
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Saving file failed: {0}", ex.Message);
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
                    {
                        SaveFile(false);
                    }
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
                {
                    return TargetToken;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The token inside the JSON file isn't found."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The JSON editor hasn't opened a file stream yet."));
            }
        }

        /// <summary>
        /// Gets a token in the JSON file. It returns null if not found.
        /// </summary>
        /// <param name="path">The path to a token. You can use JSONPath.</param>
        public static JToken GetTokenSafe(string path)
        {
            if (JsonShellCommon.FileStream is not null)
            {
                var TargetToken = JsonShellCommon.FileToken.SelectToken(path);
                if (TargetToken is not null)
                {
                    return TargetToken;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The JSON editor hasn't opened a file stream yet."));
            }
        }

        /// <summary>
        /// Gets a token in the JSON file. It returns null if not found.
        /// </summary>
        /// <param name="parentToken">Where is the target token found?</param>
        /// <param name="path">The path to a token. You can use JSONPath.</param>
        public static JToken GetTokenSafe(string parentToken, string path)
        {
            if (JsonShellCommon.FileStream is not null)
            {
                var TargetToken = GetToken(parentToken);
                TargetToken = TargetToken.SelectToken(path);
                if (TargetToken is not null)
                {
                    return TargetToken;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The JSON editor hasn't opened a file stream yet."));
            }
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
            if (type.ToLower() != "array" &&
                type.ToLower() != "object" &&
                type.ToLower() != "property" &&
                type.ToLower() != "raw")
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The specified type '{0}' is invalid."), type);

            // Then, the new object's property name (if applicable)
            var parentTokenType = DetermineType(parent);
            if (parentTokenType != JTokenType.Object && !string.IsNullOrEmpty(propName))
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("Can't append a new item with the property name with the parent token type of '{0}'."), parentTokenType.ToString());

            // Finally, parse the string JSON token
            JToken newToken = default;
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
            if (type.ToLower() != "array" &&
                type.ToLower() != "object" &&
                type.ToLower() != "property" &&
                type.ToLower() != "raw")
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The specified type '{0}' is invalid."), type);

            // Then, the new object's property name (if applicable)
            var parentTokenType = DetermineType(parent);
            if (parentTokenType != JTokenType.Object && !string.IsNullOrEmpty(propName))
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("Can't append a new item with the property name with the parent token type of '{0}'."), parentTokenType.ToString());

            // Finally, parse the string JSON token
            JToken newToken = default;
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
                    JsonShellCommon.FileToken[parent] = newToken;
                    break;
                case JTokenType.Object:
                    if (parentToken[propName] is null)
                        throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("Property name '{0}' within parent '{1}', type '{2}', doesn't exist"), propName, parent, parentTokenType.ToString());
                    parentToken[propName] = newToken;
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

            // Then, do the deletion
            parentToken.Remove();
        }

        /// <summary>
        /// Adds a new object to the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where is the target array found?</param>
        /// <param name="Key">Name of property containing the array</param>
        /// <param name="Value">The new value</param>
        [Obsolete($"Use {nameof(Add)}() instead.")]
        public static void AddNewObject(string ParentProperty, string Key, JToken Value)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.FileToken.Count());
            var TargetToken = GetToken(ParentProperty);
            JToken PropertyToken = TargetToken[Key];

            // Check to see if we're dealing with the array
            if (PropertyToken.Type == JTokenType.Array)
                ((JArray)PropertyToken).Add(Value);
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.FileToken.Count());
        }

        /// <summary>
        /// Adds a new object to the current JSON file by index
        /// </summary>
        /// <param name="ParentProperty">Where is the target array found?</param>
        /// <param name="Index">Index number of an array to add the value to</param>
        /// <param name="Value">The new value</param>
        [Obsolete($"Use {nameof(Add)}() instead.")]
        public static void AddNewObjectIndexed(string ParentProperty, int Index, JToken Value)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.FileToken.Count());
            var TargetToken = GetToken(ParentProperty);
            JToken PropertyToken = TargetToken.ElementAt(Index);

            // Check to see if we're dealing with the array
            if (PropertyToken.Type == JTokenType.Array)
                ((JArray)PropertyToken).Add(Value);
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.FileToken.Count());
        }

        /// <summary>
        /// Adds a new property to the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where to place the new property?</param>
        /// <param name="Key">New property</param>
        /// <param name="Value">The value for the new property</param>
        [Obsolete($"Use {nameof(Add)}() instead.")]
        public static void AddNewProperty(string ParentProperty, string Key, JToken Value)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.FileToken.Count());
            var TargetToken = GetToken(ParentProperty);
            JObject TokenObject = (JObject)TargetToken;
            TokenObject.Add(Key, Value);
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.FileToken.Count());
        }

        /// <summary>
        /// Adds a new array to the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where to place the new array?</param>
        /// <param name="Key">New array</param>
        /// <param name="Values">The values for the new array</param>
        [Obsolete($"Use {nameof(Add)}() instead.")]
        public static void AddNewArray(string ParentProperty, string Key, JArray Values)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.FileToken.Count());
            var TargetToken = GetToken(ParentProperty);
            JObject TokenObject = (JObject)TargetToken;
            TokenObject.Add(Key, Values);
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.FileToken.Count());
        }

        /// <summary>
        /// Removes a property from the current JSON file
        /// </summary>
        /// <param name="Property">The property. You can use JSONPath.</param>
        [Obsolete($"Use {nameof(Remove)}() instead.")]
        public static void RemoveProperty(string Property)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.FileToken.Count());
            var TargetToken = GetToken(Property);
            TargetToken.Parent.Remove();
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.FileToken.Count());
        }

        /// <summary>
        /// Removes an object from the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where is the target object found?</param>
        /// <param name="ObjectName">The object name. You can use JSONPath.</param>
        [Obsolete($"Use {nameof(Remove)}() instead.")]
        public static void RemoveObject(string ParentProperty, string ObjectName)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.FileToken.Count());
            var TargetToken = GetToken(ParentProperty);
            JToken PropertyToken = TargetToken[ObjectName];
            PropertyToken.Remove();
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.FileToken.Count());
        }

        /// <summary>
        /// Removes an object from the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where is the target object found?</param>
        /// <param name="Index">Index number of an array to add the value to</param>
        [Obsolete($"Use {nameof(Remove)}() instead.")]
        public static void RemoveObjectIndexed(string ParentProperty, int Index)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.FileToken.Count());
            var TargetToken = GetToken(ParentProperty);
            JToken PropertyToken = TargetToken.ElementAt(Index);
            PropertyToken.Remove();
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.FileToken.Count());
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

        /// <summary>
        /// Beautifies the JSON text contained in the file.
        /// </summary>
        /// <param name="JsonFile">Path to JSON file. It's automatically neutralized using <see cref="FilesystemTools.NeutralizePath(string, bool)"/>.</param>
        /// <returns>Beautified JSON</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string BeautifyJson(string JsonFile)
        {
            // Neutralize the file path
            DebugWriter.WriteDebug(DebugLevel.I, "Neutralizing json file {0}...", JsonFile);
            JsonFile = FilesystemTools.NeutralizePath(JsonFile, true);
            DebugWriter.WriteDebug(DebugLevel.I, "Got json file {0}...", JsonFile);

            // Try to beautify JSON
            string JsonFileContents = Reading.ReadContentsText(JsonFile);
            return BeautifyJsonText(JsonFileContents);
        }

        /// <summary>
        /// Beautifies the JSON text.
        /// </summary>
        /// <param name="JsonText">Contents of a minified JSON.</param>
        /// <returns>Beautified JSON</returns>
        public static string BeautifyJsonText(string JsonText)
        {
            // Make an instance of JToken with this text
            var JsonToken = JToken.Parse(JsonText);
            DebugWriter.WriteDebug(DebugLevel.I, "Created a token with text length of {0}", JsonText.Length);

            // Beautify JSON
            string BeautifiedJson = JsonConvert.SerializeObject(JsonToken, Formatting.Indented);
            DebugWriter.WriteDebug(DebugLevel.I, "Beautified the JSON text. Length: {0}", BeautifiedJson.Length);
            return BeautifiedJson;
        }

        /// <summary>
        /// Minifies the JSON text contained in the file.
        /// </summary>
        /// <param name="JsonFile">Path to JSON file. It's automatically neutralized using <see cref="FilesystemTools.NeutralizePath(string, bool)"/>.</param>
        /// <returns>Minified JSON</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string MinifyJson(string JsonFile)
        {
            // Neutralize the file path
            DebugWriter.WriteDebug(DebugLevel.I, "Neutralizing json file {0}...", JsonFile);
            JsonFile = FilesystemTools.NeutralizePath(JsonFile, true);
            DebugWriter.WriteDebug(DebugLevel.I, "Got json file {0}...", JsonFile);

            // Try to minify JSON
            string JsonFileContents = Reading.ReadContentsText(JsonFile);
            return MinifyJsonText(JsonFileContents);
        }

        /// <summary>
        /// Minifies the JSON text.
        /// </summary>
        /// <param name="JsonText">Contents of a beautified JSON.</param>
        /// <returns>Minified JSON</returns>
        public static string MinifyJsonText(string JsonText)
        {
            // Make an instance of JToken with this text
            var JsonToken = JToken.Parse(JsonText);
            DebugWriter.WriteDebug(DebugLevel.I, "Created a token with text length of {0}", JsonText.Length);

            // Minify JSON
            string MinifiedJson = JsonConvert.SerializeObject(JsonToken);
            DebugWriter.WriteDebug(DebugLevel.I, "Minified the JSON text. Length: {0}", MinifiedJson.Length);
            return MinifiedJson;
        }

    }
}
