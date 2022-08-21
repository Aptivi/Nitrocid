using System;

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

using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Extensification.StreamReaderExts;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.Shells.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.Misc.Editors.JsonShell
{
    public static class JsonTools
    {

        /// <summary>
        /// Opens the JSON file
        /// </summary>
        /// <param name="File">Target file. We recommend you to use <see cref="NeutralizePath(String, Boolean)"></see> to neutralize path.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool JsonShell_OpenJsonFile(string File)
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Trying to open file {0}...", File);
                JsonShellCommon.JsonShell_FileStream = new FileStream(File, FileMode.Open);
                var JsonFileReader = new StreamReader(JsonShellCommon.JsonShell_FileStream);
                string JsonFileContents = JsonFileReader.ReadToEndAndSeek();
                JsonShellCommon.JsonShell_FileToken = JToken.Parse(!string.IsNullOrWhiteSpace(JsonFileContents) ? JsonFileContents : "{}");
                JsonShellCommon.JsonShell_FileTokenOrig = JToken.Parse(!string.IsNullOrWhiteSpace(JsonFileContents) ? JsonFileContents : "{}");
                DebugWriter.Wdbg(DebugLevel.I, "File {0} is open. Length: {1}, Pos: {2}", File, JsonShellCommon.JsonShell_FileStream.Length, JsonShellCommon.JsonShell_FileStream.Position);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Open file {0} failed: {1}", File, ex.Message);
                DebugWriter.WStkTrc(ex);
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
                DebugWriter.Wdbg(DebugLevel.I, "Trying to close file...");
                JsonShellCommon.JsonShell_FileStream.Close();
                JsonShellCommon.JsonShell_FileStream = null;
                DebugWriter.Wdbg(DebugLevel.I, "File is no longer open.");
                JsonShellCommon.JsonShell_FileToken = JToken.Parse("{}");
                JsonShellCommon.JsonShell_FileTokenOrig = JToken.Parse("{}");
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Closing file failed: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
                return false;
            }
        }

        /// <summary>
        /// Saves JSON file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool JsonShell_SaveFile(bool ClearJson)
        {
            return JsonShell_SaveFile(ClearJson, JsonShellCommon.JsonShell_Formatting);
        }

        /// <summary>
        /// Saves JSON file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool JsonShell_SaveFile(bool ClearJson, Formatting Formatting)
        {
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Trying to save file...");
                JsonShellCommon.JsonShell_FileStream.SetLength(0L);
                DebugWriter.Wdbg(DebugLevel.I, "Length set to 0.");
                var FileLinesByte = Encoding.Default.GetBytes(JsonConvert.SerializeObject(JsonShellCommon.JsonShell_FileToken, Formatting));
                DebugWriter.Wdbg(DebugLevel.I, "Converted lines to bytes. Length: {0}", FileLinesByte.Length);
                JsonShellCommon.JsonShell_FileStream.Write(FileLinesByte, 0, FileLinesByte.Length);
                JsonShellCommon.JsonShell_FileStream.Flush();
                DebugWriter.Wdbg(DebugLevel.I, "File is saved.");
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
                DebugWriter.Wdbg(DebugLevel.E, "Saving file failed: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
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
                    DebugWriter.WStkTrc(ex);
                }
            }
        }

        /// <summary>
        /// Was JSON edited?
        /// </summary>
        public static bool JsonShell_WasJsonEdited()
        {
            return !JToken.DeepEquals(JsonShellCommon.JsonShell_FileToken, JsonShellCommon.JsonShell_FileTokenOrig);
        }

        /// <summary>
        /// Gets a property in the JSON file
        /// </summary>
        /// <param name="[Property]">The property. You can use JSONPath.</param>
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
                    throw new ArgumentOutOfRangeException(nameof(Property), Property, Translate.DoTranslation("The property inside the JSON file isn't found."));
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("The JSON editor hasn't opened a file stream yet."));
            }
        }

        /// <summary>
        /// Adds a new property to the current JSON file
        /// </summary>
        /// <param name="ParentProperty">Where to place the new property?</param>
        /// <param name="Key">New property</param>
        /// <param name="Value">The value for the new property</param>
        public static void JsonShell_AddNewProperty(string ParentProperty, string Key, JToken Value)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
            var TargetToken = JsonShell_GetProperty(ParentProperty);
            JObject TokenObject = (JObject)TargetToken;
            TokenObject.Add(Key, Value);
            DebugWriter.Wdbg(DebugLevel.I, "New file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
        }

        /// <summary>
        /// Removes a property from the current JSON file
        /// </summary>
        /// <param name="[Property]">The property. You can use JSONPath.</param>
        public static void JsonShell_RemoveProperty(string Property)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Old file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
            var TargetToken = JsonShell_GetProperty(Property);
            TargetToken.Parent.Remove();
            DebugWriter.Wdbg(DebugLevel.I, "New file lines: {0}", JsonShellCommon.JsonShell_FileToken.Count());
        }

        /// <summary>
        /// Serializes the property to the string
        /// </summary>
        /// <param name="[Property]">The property. You can use JSONPath.</param>
        public static string JsonShell_SerializeToString(string Property)
        {
            var TargetToken = JsonShell_GetProperty(Property);
            return JsonConvert.SerializeObject(TargetToken, Formatting.Indented);
        }

    }
}