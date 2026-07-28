//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
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
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.Json
{
    /// <summary>
    /// JSON shell tools
    /// </summary>
    public partial class JsonShell : BaseShell, IShell
    {

        /// <summary>
        /// Opens the JSON file
        /// </summary>
        /// <param name="File">Target file. We recommend you to use <see cref="FilesystemTools.NeutralizePath(string, bool)"></see> to neutralize path.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool OpenJsonFile(string File)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to open file {0}...", vars: [File]);
                FileStream = new FileStream(File, FileMode.Open);
                var JsonFileReader = new StreamReader(FileStream);
                string JsonFileContents = FilesystemTools.ReadToEndAndSeek(ref JsonFileReader);
                FileToken = JToken.Parse(!string.IsNullOrWhiteSpace(JsonFileContents) ? JsonFileContents : "{}");
                FileTokenOrig = JToken.Parse(!string.IsNullOrWhiteSpace(JsonFileContents) ? JsonFileContents : "{}");
                DebugWriter.WriteDebug(DebugLevel.I, "File {0} is open. Length: {1}, Pos: {2}", vars: [File, FileStream.Length, FileStream.Position]);
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
        public bool CloseJsonFile()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to close file...");
                FileStream?.Close();
                FileStream = null;
                DebugWriter.WriteDebug(DebugLevel.I, "File is no longer open.");
                FileToken = JToken.Parse("{}");
                FileTokenOrig = JToken.Parse("{}");
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
        public bool SaveFile(bool ClearJson) =>
            SaveFile(ClearJson, (Formatting)ShellsInit.ShellsConfig.JsonShellFormatting);

        /// <summary>
        /// Saves JSON file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool SaveFile(bool ClearJson, Formatting Formatting)
        {
            try
            {
                if (FileStream is null)
                    throw new KernelException(KernelExceptionType.HexEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NOTOPEN"));
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to save file...");
                FileStream.SetLength(0L);
                DebugWriter.WriteDebug(DebugLevel.I, "Length set to 0.");
                var FileLinesByte = Encoding.Default.GetBytes(JsonConvert.SerializeObject(FileToken, Formatting));
                DebugWriter.WriteDebug(DebugLevel.I, "Converted lines to bytes. Length: {0}", vars: [FileLinesByte.Length]);
                FileStream.Write(FileLinesByte, 0, FileLinesByte.Length);
                FileStream.Flush();
                DebugWriter.WriteDebug(DebugLevel.I, "File is saved.");
                if (ClearJson)
                    FileToken = JToken.Parse("{}");
                FileTokenOrig = JToken.Parse("{}");
                FileTokenOrig = FileToken;
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
        public static void HandleAutoSaveJsonFile(JsonShell? shell)
        {
            if (shell is null)
                throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            if (ShellsInit.ShellsConfig.JsonEditAutoSaveFlag)
            {
                try
                {
                    Thread.Sleep(ShellsInit.ShellsConfig.JsonEditAutoSaveInterval * 1000);
                    if (shell.FileStream is not null)
                        shell.SaveFile(false);
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
        public bool WasJsonEdited() =>
            !JToken.DeepEquals(FileToken, FileTokenOrig);

        /// <summary>
        /// Gets the root type
        /// </summary>
        /// <returns>Root JToken type</returns>
        public JTokenType DetermineRootType() =>
            FileToken.Root.Type;

        /// <summary>
        /// Gets the root type
        /// </summary>
        /// <param name="path">Path to the target object, array, or property</param>
        /// <returns>Root JToken type</returns>
        public JTokenType DetermineType(string path)
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
        public JToken GetToken(string path)
        {
            if (FileStream is not null)
            {
                var TargetToken = FileToken.SelectToken(path);
                if (TargetToken is not null)
                    return TargetToken;
                else
                    throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NOTOKEN"));
            }
            else
                throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_JSONTOOLS_EXCEPTION_STREAMNOTOPEN"));
        }

        /// <summary>
        /// Gets a token in the JSON file. It returns null if not found.
        /// </summary>
        /// <param name="path">The path to a token. You can use JSONPath.</param>
        public JToken? GetTokenSafe(string path)
        {
            if (FileStream is not null)
            {
                var TargetToken = FileToken.SelectToken(path);
                if (TargetToken is not null)
                    return TargetToken;
                else
                    return null;
            }
            else
                throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_JSONTOOLS_EXCEPTION_STREAMNOTOPEN"));
        }

        /// <summary>
        /// Gets a token in the JSON file. It returns null if not found.
        /// </summary>
        /// <param name="parentToken">Where is the target token found?</param>
        /// <param name="path">The path to a token. You can use JSONPath.</param>
        public JToken? GetTokenSafe(string parentToken, string path)
        {
            if (FileStream is not null)
            {
                var TargetToken = GetToken(parentToken);
                TargetToken = TargetToken.SelectToken(path);
                if (TargetToken is not null)
                    return TargetToken;
                else
                    return null;
            }
            else
                throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_JSONTOOLS_EXCEPTION_STREAMNOTOPEN"));
        }

        /// <summary>
        /// Adds a new object, array, or property to the current JSON file
        /// </summary>
        /// <param name="parent">Where is the target to perform an operation on? Use JSONPath.</param>
        /// <param name="type">Either object, array, property, or raw</param>
        /// <param name="propName">Property name. Must be empty for non-object parent token type</param>
        /// <param name="value">Value. It'll be automatically processed into the form of ["value"] for arrays, {} for objects, "value" for properties, and value for raw.</param>
        public void Add(string parent, string type, string propName, string value)
        {
            // First, do some sanity checks, starting from the parent token
            var parentToken = GetTokenSafe(parent) ??
                throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NOPARENTTOKEN"), parent);

            // Then, the new object type
            if (!type.Equals("array", StringComparison.OrdinalIgnoreCase) &&
                !type.Equals("object", StringComparison.OrdinalIgnoreCase) &&
                !type.Equals("property", StringComparison.OrdinalIgnoreCase) &&
                !type.Equals("raw", StringComparison.OrdinalIgnoreCase))
                throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_INVALIDTYPE"), type);

            // Then, the new object's property name (if applicable)
            var parentTokenType = DetermineType(parent);
            if (parentTokenType != JTokenType.Object && !string.IsNullOrEmpty(propName))
                throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NEWITEMADDFAILED_UNNAMED"), parentTokenType.ToString());

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
                        throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NEWITEMADDFAILED_NAMED"), propName, parentTokenType.ToString());
                    break;
                case "object":
                    if (parentTokenType == JTokenType.Object && !string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{{}}");
                    else if (parentTokenType != JTokenType.Object && string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{{}}");
                    else
                        throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NEWITEMADDFAILED_NAMED"), propName, parentTokenType.ToString());
                    break;
                case "property":
                    if (parentTokenType == JTokenType.Object && !string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"\"{value}\"");
                    else
                        throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NEWITEMADDFAILED_UNNAMED"), parentTokenType.ToString());
                    break;
                case "raw":
                    if (parentTokenType == JTokenType.Object && !string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{value}");
                    else if (parentTokenType != JTokenType.Object && string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{value}");
                    else
                        throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NEWITEMADDFAILED_NAMED"), propName, parentTokenType.ToString());
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
        public void Set(string parent, string type, string propName, string value)
        {
            // First, do some sanity checks, starting from the parent token
            var parentToken = GetTokenSafe(parent) ??
                throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NOPARENTTOKEN"), parent);

            // Then, the new object type
            if (!type.Equals("array", StringComparison.OrdinalIgnoreCase) &&
                !type.Equals("object", StringComparison.OrdinalIgnoreCase) &&
                !type.Equals("property", StringComparison.OrdinalIgnoreCase) &&
                !type.Equals("raw", StringComparison.OrdinalIgnoreCase))
                throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_INVALIDTYPE"), type);

            // Then, the new object's property name (if applicable)
            var parentTokenType = DetermineType(parent);
            if (parentTokenType != JTokenType.Object && !string.IsNullOrEmpty(propName))
                throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NEWITEMADDFAILED_UNNAMED"), parentTokenType.ToString());

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
                        throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NEWITEMADDFAILED_NAMED"), propName, parentTokenType.ToString());
                    break;
                case "object":
                    if (parentTokenType == JTokenType.Object && !string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{{}}");
                    else if (parentTokenType != JTokenType.Object && string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{{}}");
                    else
                        throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NEWITEMADDFAILED_NAMED"), propName, parentTokenType.ToString());
                    break;
                case "property":
                    if (parentTokenType == JTokenType.Object && !string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"\"{value}\"");
                    else
                        throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NEWITEMADDFAILED_UNNAMED"), parentTokenType.ToString());
                    break;
                case "raw":
                    if (parentTokenType == JTokenType.Object && !string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{value}");
                    else if (parentTokenType != JTokenType.Object && string.IsNullOrEmpty(propName))
                        newToken = JToken.Parse($"{value}");
                    else
                        throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NEWITEMADDFAILED_NAMED"), propName, parentTokenType.ToString());
                    break;
            }
            switch (parentTokenType)
            {
                case JTokenType.Object:
                    if (parentToken[propName] is null)
                        throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NOPROPWITHINPARENT"), propName, parent, parentTokenType.ToString());
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
        public void Remove(string parent)
        {
            // First, do some sanity checks, starting from the parent token
            var parentToken = GetTokenSafe(parent) ??
                throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NOPARENTTOKEN"), parent);
            if (parentToken.Parent is null)
                throw new KernelException(KernelExceptionType.JsonEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_EXCEPTION_NOPARENTTOKEN"), parent);

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
        public string SerializeToString(string Property)
        {
            var TargetToken = GetToken(Property);
            return JsonConvert.SerializeObject(TargetToken, Formatting.Indented);
        }

    }
}
