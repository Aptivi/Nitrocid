//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Nitrocid.Files;
using Nitrocid.Files.Operations;
using Nitrocid.Kernel.Debugging;
using Textify.General;

namespace Nitrocid.Misc.Text
{
    /// <summary>
    /// JSON shell tools
    /// </summary>
    public static class JsonTextTools
    {

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
            return JsonTools.BeautifyJsonText(JsonFileContents);
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
            return JsonTools.MinifyJsonText(JsonFileContents);
        }

        /// <summary>
        /// Finds the JSON object differences between the two JSON object tokens
        /// </summary>
        /// <param name="sourceObj">Source object token</param>
        /// <param name="targetObj">Target object token</param>
        /// <returns>A JSON object containing differences for objects</returns>
        public static JObject FindDifferences(JToken sourceObj, JToken targetObj)
        {
            var diff = new JObject();
            if (!JToken.DeepEquals(targetObj, sourceObj))
            {
                switch (targetObj.Type)
                {
                    case JTokenType.Object:
                        {
                            var addedKeys = ((JObject)targetObj).Properties().Select(c => c.Name).Except(((JObject)sourceObj).Properties().Select(c => c.Name));
                            var removedKeys = ((JObject)sourceObj).Properties().Select(c => c.Name).Except(((JObject)targetObj).Properties().Select(c => c.Name));
                            var changedKeys = ((JObject)targetObj).Properties().Where(c => !JToken.DeepEquals(c.Value, sourceObj[c.Name])).Select(c => c.Name);
                            foreach (var k in addedKeys)
                            {
                                diff[$"+{k}"] = new JObject
                                {
                                    ["+"] = targetObj[k].Path
                                };
                                DebugWriter.WriteDebug(DebugLevel.I, "Extra addition {0}", targetObj[k].Path);
                            }
                            foreach (var k in removedKeys)
                            {
                                diff[$"-{k}"] = new JObject
                                {
                                    ["-"] = sourceObj[k].Path
                                };
                                DebugWriter.WriteDebug(DebugLevel.I, "Extra subtraction {0}", sourceObj[k].Path);
                            }
                            foreach (var k in changedKeys)
                            {
                                diff[$"*{k}"] = new JObject
                                {
                                    ["*"] = new JObject
                                    {
                                        ["source"] = sourceObj[k],
                                        ["target"] = targetObj[k],
                                    }
                                };
                                DebugWriter.WriteDebug(DebugLevel.I, "Changed: {0}, {1}", sourceObj[k]?.Path, targetObj[k]?.Path);
                            }
                        }
                        break;
                    case JTokenType.Array:
                        {
                            diff["+"] = new JArray(((JArray)targetObj).Except(sourceObj));
                            diff["-"] = new JArray(((JArray)sourceObj).Except(targetObj));
                            DebugWriter.WriteDebug(DebugLevel.I, "Additions: {0}, Removals: {1}", diff["+"].Count(), diff["-"].Count());
                        }
                        break;
                    default:
                        DebugWriter.WriteDebug(DebugLevel.I, "Whole diff.");
                        diff["+"] = targetObj;
                        diff["-"] = sourceObj;
                        break;
                }
            }
            return diff;
        }

    }
}
