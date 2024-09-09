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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.MiscWriters;

namespace Nitrocid.LocaleCheckerStandalone.Analyzers
{
    internal static class LocalizableResourcesAnalyzer
    {
        internal static string[] GetUnlocalizedStrings()
        {
            List<string> unlocalizedStrings = [];

            // Open every resource except the English translations file
            var resourceNames = EntryPoint.thisAssembly.GetManifestResourceNames().Except([
                "Nitrocid.LocaleCheckerStandalone.eng.json",
            ]);
            foreach (var resourceName in resourceNames)
            {
                // Open the resource and load it to a JSON token instance
                var stream = EntryPoint.thisAssembly.GetManifestResourceStream(resourceName) ??
                    throw new Exception($"Opening the {resourceName} resource stream has failed.");
                var reader = new StreamReader(stream);
                var jsonReader = new JsonTextReader(reader);
                var document = JToken.Load(jsonReader) ??
                    throw new Exception($"Unable to parse JSON for {resourceName}.");

                // Determine if this is a theme or a settings entries list
                var themeMetadata = document.Type == JTokenType.Array ? null : document["Metadata"];
                if (themeMetadata is not null)
                {
                    // It's a theme. Get its description and its localizable boolean value
                    string description = ((string?)themeMetadata["Description"] ?? "").Replace("\\\"", "\"");
                    bool localizable = (bool?)themeMetadata["Localizable"] ?? false;
                    if (!string.IsNullOrEmpty(description) && localizable && !EntryPoint.localizationList.Contains(description))
                    {
                        TextWriterColor.Write($"{nameof(NLOC0001)}Resources: {resourceName}: Unlocalized theme description found: {description}", true, ConsoleColors.Yellow);
                        unlocalizedStrings.Add(description);
                    }
                }
                else if (document.Type == JTokenType.Array)
                {
                    // It's likely a settings entry list, but verify
                    foreach (var settingsEntryList in document)
                    {
                        // Check the description and the display
                        string description = ((string?)settingsEntryList["Desc"] ?? "").Replace("\\\"", "\"");
                        string displayAs = ((string?)settingsEntryList["DisplayAs"] ?? "").Replace("\\\"", "\"");
                        if (!string.IsNullOrEmpty(description) && !EntryPoint.localizationList.Contains(description))
                        {
                            TextWriterColor.Write($"{nameof(NLOC0001)}Resources: {resourceName}: Unlocalized settings description found: {description}", true, ConsoleColors.Yellow);
                            unlocalizedStrings.Add(description);
                        }
                        if (!string.IsNullOrEmpty(displayAs) && !EntryPoint.localizationList.Contains(displayAs))
                        {
                            TextWriterColor.Write($"{nameof(NLOC0001)}Resources: {resourceName}: Unlocalized settings display found: {displayAs}", true, ConsoleColors.Yellow);
                            unlocalizedStrings.Add(displayAs);
                        }

                        // Now, check the keys
                        JArray? keys = (JArray?)settingsEntryList["Keys"];
                        if (keys is null || keys.Count == 0)
                            continue;
                        foreach (var key in keys)
                        {
                            string keyName = ((string?)key["Name"] ?? "").Replace("\\\"", "\"");
                            string keyDesc = ((string?)key["Description"] ?? "").Replace("\\\"", "\"");
                            if (!string.IsNullOrEmpty(keyName) && !EntryPoint.localizationList.Contains(keyName))
                            {
                                TextWriterColor.Write($"{nameof(NLOC0001)}Resources: {resourceName}: Unlocalized key name found: {keyName}", true, ConsoleColors.Yellow);
                                unlocalizedStrings.Add(keyName);
                            }
                            if (!string.IsNullOrEmpty(keyDesc) && !EntryPoint.localizationList.Contains(keyDesc))
                            {
                                TextWriterColor.Write($"{nameof(NLOC0001)}Resources: {resourceName}: Unlocalized key description found: {keyDesc}", true, ConsoleColors.Yellow);
                                unlocalizedStrings.Add(keyDesc);
                            }
                        }
                    }
                }
            }
            return [.. unlocalizedStrings];
        }
    }
}
