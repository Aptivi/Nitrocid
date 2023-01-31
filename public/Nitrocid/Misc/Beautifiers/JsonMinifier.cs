
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

using System.IO;
using KS.Files;
using KS.Kernel.Debugging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.Misc.Beautifiers
{
    /// <summary>
    /// JSON file minifier module
    /// </summary>
    public static class JsonMinifier
    {

        /// <summary>
        /// Minifies the JSON text contained in the file.
        /// </summary>
        /// <param name="JsonFile">Path to JSON file. It's automatically neutralized using <see cref="Filesystem.NeutralizePath(string, bool)"/>.</param>
        /// <returns>Minified JSON</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string MinifyJson(string JsonFile)
        {
            // Neutralize the file path
            DebugWriter.WriteDebug(DebugLevel.I, "Neutralizing json file {0}...", JsonFile);
            JsonFile = Filesystem.NeutralizePath(JsonFile, true);
            DebugWriter.WriteDebug(DebugLevel.I, "Got json file {0}...", JsonFile);

            // Try to minify JSON
            string JsonFileContents = File.ReadAllText(JsonFile);
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
