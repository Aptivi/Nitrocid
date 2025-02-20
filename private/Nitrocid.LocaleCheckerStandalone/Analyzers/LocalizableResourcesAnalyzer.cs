﻿//
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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nitrocid.Analyzers.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;

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
                    string descriptionOrig = (string?)themeMetadata["Description"] ?? "";
                    string description = descriptionOrig.Replace("\\\"", "\"");
                    bool localizable = (bool?)themeMetadata["Localizable"] ?? false;
                    if (!string.IsNullOrWhiteSpace(description) && localizable && !EntryPoint.localizationList.Contains(description))
                    {
                        var location = AnalyzerTools.GenerateLocation(themeMetadata["Description"], descriptionOrig, resourceName);
                        AnalyzerTools.PrintFromLocation(location, resourceName, "NLOC0003", $"Unlocalized theme description found: {description}");
                        unlocalizedStrings.Add(description);
                    }
                }
                else if (document.Type == JTokenType.Array)
                {
                    // It's likely a settings entry list, but verify
                    foreach (var settingsEntryList in document)
                    {
                        // Check the description and the display
                        string descriptionOrig = (string?)settingsEntryList["Desc"] ?? "";
                        string displayAsOrig = (string?)settingsEntryList["DisplayAs"] ?? "";
                        string knownAddonDisplayOrig = (string?)settingsEntryList["display"] ?? "";
                        string description = descriptionOrig.Replace("\\\"", "\"");
                        string displayAs = displayAsOrig.Replace("\\\"", "\"");
                        string knownAddonDisplay = knownAddonDisplayOrig.Replace("\\\"", "\"");
                        if (!string.IsNullOrWhiteSpace(description) && !EntryPoint.localizationList.Contains(description))
                        {
                            var location = AnalyzerTools.GenerateLocation(settingsEntryList["Desc"], descriptionOrig, resourceName);
                            AnalyzerTools.PrintFromLocation(location, resourceName, "NLOC0003", $"Unlocalized settings description found: {description}");
                            unlocalizedStrings.Add(description);
                        }
                        if (!string.IsNullOrWhiteSpace(displayAs) && !EntryPoint.localizationList.Contains(displayAs))
                        {
                            var location = AnalyzerTools.GenerateLocation(settingsEntryList["DisplayAs"], displayAsOrig, resourceName);
                            AnalyzerTools.PrintFromLocation(location, resourceName, "NLOC0003", $"Unlocalized settings display found: {displayAs}");
                            unlocalizedStrings.Add(displayAs);
                        }
                        if (!string.IsNullOrWhiteSpace(knownAddonDisplay) && !EntryPoint.localizationList.Contains(knownAddonDisplay))
                        {
                            var location = AnalyzerTools.GenerateLocation(settingsEntryList["display"], knownAddonDisplayOrig, resourceName);
                            AnalyzerTools.PrintFromLocation(location, resourceName, "NLOC0003", $"Unlocalized known addon display found: {knownAddonDisplay}");
                            unlocalizedStrings.Add(knownAddonDisplay);
                        }

                        // Now, check the keys
                        JArray? keys = (JArray?)settingsEntryList["Keys"];
                        if (keys is null || keys.Count == 0)
                            continue;
                        foreach (var key in keys)
                        {
                            string keyNameOrig = (string?)key["Name"] ?? "";
                            string keyDescOrig = (string?)key["Description"] ?? "";
                            string keyName = keyNameOrig.Replace("\\\"", "\"");
                            string keyDesc = keyDescOrig.Replace("\\\"", "\"");
                            if (!string.IsNullOrWhiteSpace(keyName) && !EntryPoint.localizationList.Contains(keyName))
                            {
                                var location = AnalyzerTools.GenerateLocation(key["Name"], keyNameOrig, resourceName);
                                AnalyzerTools.PrintFromLocation(location, resourceName, "NLOC0003", $"Unlocalized key name found: {keyName}");
                                unlocalizedStrings.Add(keyName);
                            }
                            if (!string.IsNullOrWhiteSpace(keyDesc) && !EntryPoint.localizationList.Contains(keyDesc))
                            {
                                var location = AnalyzerTools.GenerateLocation(key["Description"], keyDescOrig, resourceName);
                                AnalyzerTools.PrintFromLocation(location, resourceName, "NLOC0003", $"Unlocalized key description found: {keyDesc}");
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
