
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Extensification.StreamReaderExts;
using KS.Files;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.Shells.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet.Security;

namespace KS.Misc.Editors.JsonShell
{
    /// <summary>
    /// JSON shell tools
    /// </summary>
    public static class JsonTools
    {

        /// <summary>
        /// Opens the JSON file
        /// </summary>
        /// <param name="File">Target file. We recommend you to use <see cref="Filesystem.NeutralizePath(string, bool)"></see> to neutralize path.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool JsonShell_OpenJsonFile(string File)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to open file {0}...", File);
                JsonShellCommon.JsonShell_FileStream = new FileStream(File, FileMode.Open);
                var JsonFileReader = new StreamReader(JsonShellCommon.JsonShell_FileStream);
                string JsonFileContents = JsonFileReader.ReadToEndAndSeek();
                JsonShellCommon.JsonShell_FileToken = JToken.Parse(!string.IsNullOrWhiteSpace(JsonFileContents) ? JsonFileContents : "{}");
                JsonShellCommon.JsonShell_FileTokenOrig = JToken.Parse(!string.IsNullOrWhiteSpace(JsonFileContents) ? JsonFileContents : "{}");
                DebugWriter.WriteDebug(DebugLevel.I, "File {0} is open. Length: {1}, Pos: {2}", File, JsonShellCommon.JsonShell_FileStream.Length, JsonShellCommon.JsonShell_FileStream.Position);
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
        public static bool JsonShell_CloseTextFile()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to close file...");
                JsonShellCommon.JsonShell_FileStream.Close();
                JsonShellCommon.JsonShell_FileStream = null;
                DebugWriter.WriteDebug(DebugLevel.I, "File is no longer open.");
                JsonShellCommon.JsonShell_FileToken = JToken.Parse("{}");
                JsonShellCommon.JsonShell_FileTokenOrig = JToken.Parse("{}");
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
        public static bool JsonShell_SaveFile(bool ClearJson) => JsonShell_SaveFile(ClearJson, JsonShellCommon.JsonShell_Formatting);

        /// <summary>
        /// Saves JSON file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool JsonShell_SaveFile(bool ClearJson, Formatting Formatting)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to save file...");
                JsonShellCommon.JsonShell_FileStream.SetLength(0L);
                DebugWriter.WriteDebug(DebugLevel.I, "Length set to 0.");
                var FileLinesByte = Encoding.Default.GetBytes(JsonConvert.SerializeObject(JsonShellCommon.JsonShell_FileToken, Formatting));
                DebugWriter.WriteDebug(DebugLevel.I, "Converted lines to bytes. Length: {0}", FileLinesByte.Length);
                JsonShellCommon.JsonShell_FileStream.Write(FileLinesByte, 0, FileLinesByte.Length);
                JsonShellCommon.JsonShell_FileStream.Flush();
                DebugWriter.WriteDebug(DebugLevel.I, "File is saved.");
                if (ClearJson)
                {
                    JsonShellCommon.JsonShell_FileToken = JToken.Parse("{}");
                }
                JsonShellCommon.JsonShell_FileTokenOrig = JToken.Parse("{}");
                JsonShellCommon.JsonShell_FileTokenOrig = JsonShellCommon.JsonShell_FileToken;
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
        public static void JsonShell_HandleAutoSaveJsonFile()
        {
            if (JsonShellCommon.JsonShell_AutoSaveFlag)
            {
                try
                {
                    Thread.Sleep(JsonShellCommon.JsonShell_AutoSaveInterval * 1000);
                    if (JsonShellCommon.JsonShell_FileStream is not null)
                    {
                        JsonShell_SaveFile(false);
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
        public static bool JsonShell_WasJsonEdited() => !JToken.DeepEquals(JsonShellCommon.JsonShell_FileToken, JsonShellCommon.JsonShell_FileTokenOrig);

        /// <summary>
        /// Gets a property in the JSON file
        /// </summary>
        /// <param name="Property">The property. You can use JSONPath.</param>
        public static JToken JsonShell_GetProperty(string Property)
        {
            if (JsonShellCommon.JsonShell_FileStream is not null)
            {
                var TargetToken = JsonShellCommon.JsonShell_FileToken.SelectToken(Property);
                if (TargetToken is not null)
                {
                    return TargetToken;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The property inside the JSON file isn't found."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.JsonEditor, Translate.DoTranslation("The JSON editor hasn't opened a file stream yet."));
            }
        }

        /// <summary>
        /// Adds a new object to the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where is the target array found?</param>
        /// <param name="Key">Name of property containing the array</param>
        /// <param name="Value">The new value</param>
        public static void JsonShell_AddNewObject(string ParentProperty, string Key, JToken Value)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
            var TargetToken = JsonShell_GetProperty(ParentProperty);
            JToken PropertyToken = TargetToken[Key];

            // Check to see if we're dealing with the array
            if (PropertyToken.Type == JTokenType.Array)
                ((JArray)PropertyToken).Add(Value);
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
        }

        /// <summary>
        /// Adds a new object to the current JSON file by index
        /// </summary>
        /// <param name="ParentProperty">Where is the target array found?</param>
        /// <param name="Index">Index number of an array to add the value to</param>
        /// <param name="Value">The new value</param>
        public static void JsonShell_AddNewObjectIndexed(string ParentProperty, int Index, JToken Value)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
            var TargetToken = JsonShell_GetProperty(ParentProperty);
            JToken PropertyToken = TargetToken.ElementAt(Index);

            // Check to see if we're dealing with the array
            if (PropertyToken.Type == JTokenType.Array)
                ((JArray)PropertyToken).Add(Value);
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
        }

        /// <summary>
        /// Adds a new property to the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where to place the new property?</param>
        /// <param name="Key">New property</param>
        /// <param name="Value">The value for the new property</param>
        public static void JsonShell_AddNewProperty(string ParentProperty, string Key, JToken Value)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
            var TargetToken = JsonShell_GetProperty(ParentProperty);
            JObject TokenObject = (JObject)TargetToken;
            TokenObject.Add(Key, Value);
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
        }

        /// <summary>
        /// Adds a new array to the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where to place the new array?</param>
        /// <param name="Key">New array</param>
        /// <param name="Values">The values for the new array</param>
        public static void JsonShell_AddNewArray(string ParentProperty, string Key, JArray Values)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
            var TargetToken = JsonShell_GetProperty(ParentProperty);
            JObject TokenObject = (JObject)TargetToken;
            TokenObject.Add(Key, Values);
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
        }

        /// <summary>
        /// Removes a property from the current JSON file
        /// </summary>
        /// <param name="Property">The property. You can use JSONPath.</param>
        public static void JsonShell_RemoveProperty(string Property)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
            var TargetToken = JsonShell_GetProperty(Property);
            TargetToken.Parent.Remove();
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
        }

        /// <summary>
        /// Removes an object from the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where is the target object found?</param>
        /// <param name="ObjectName">The object name. You can use JSONPath.</param>
        public static void JsonShell_RemoveObject(string ParentProperty, string ObjectName)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
            var TargetToken = JsonShell_GetProperty(ParentProperty);
            JToken PropertyToken = TargetToken[ObjectName];
            PropertyToken.Remove();
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
        }

        /// <summary>
        /// Removes an object from the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where is the target object found?</param>
        /// <param name="Index">Index number of an array to add the value to</param>
        public static void JsonShell_RemoveObjectIndexed(string ParentProperty, int Index)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
            var TargetToken = JsonShell_GetProperty(ParentProperty);
            JToken PropertyToken = TargetToken.ElementAt(Index);
            PropertyToken.Remove();
            DebugWriter.WriteDebug(DebugLevel.I, "New file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
        }

        /// <summary>
        /// Serializes the property to the string
        /// </summary>
        /// <param name="Property">The property. You can use JSONPath.</param>
        public static string JsonShell_SerializeToString(string Property)
        {
            var TargetToken = JsonShell_GetProperty(Property);
            return JsonConvert.SerializeObject(TargetToken, Formatting.Indented);
        }

    }
}
