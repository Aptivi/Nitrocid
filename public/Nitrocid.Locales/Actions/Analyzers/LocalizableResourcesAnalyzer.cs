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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nitrocid.Analyzers.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nitrocid.Locales.Actions.Analyzers
{
    internal static class LocalizableResourcesAnalyzer
    {
        internal static IEnumerable<string> GetResourceNames() =>
             EntryPoint.thisAssembly.GetManifestResourceNames().Except([
                 "Nitrocid.Locales.eng.json",
             ]);

        internal static string[] GetUnlocalizedStrings()
        {
            List<string> unlocalizedStrings = [];

            // Open every resource except the English translations file
            var resourceNames = GetResourceNames();
            foreach (var resourceName in resourceNames)
                unlocalizedStrings.AddRange(GetUnlocalizedStrings(resourceName));
            return [.. unlocalizedStrings];
        }

        internal static string[] GetLocalizedStrings()
        {
            List<string> localizedStrings = [];

            // Open every resource except the English translations file
            var resourceNames = GetResourceNames();
            foreach (var resourceName in resourceNames)
                localizedStrings.AddRange(GetLocalizedStrings(resourceName));
            return [.. localizedStrings];
        }

        internal static string[] GetUnlocalizedStrings(string resourceName, bool reportMode = false)
        {
            List<string> unlocalizedStrings = [];
            HashSet<string> source =
                reportMode ? [.. Reporter.localizationList] : Checker.localizationList;

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
                if (!string.IsNullOrWhiteSpace(description) && localizable && !source.Contains(description))
                {
                    if (!reportMode)
                    {
                        var location = AnalyzerTools.GenerateLocation(themeMetadata["Description"], descriptionOrig, resourceName);
                        AnalyzerTools.PrintFromLocation(location, resourceName, "NLOC0003", $"Unlocalized theme description found: {description}");
                    }
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
                    if (!string.IsNullOrWhiteSpace(description) && !source.Contains(description))
                    {
                        if (!reportMode)
                        {
                            var location = AnalyzerTools.GenerateLocation(settingsEntryList["Desc"], descriptionOrig, resourceName);
                            AnalyzerTools.PrintFromLocation(location, resourceName, "NLOC0003", $"Unlocalized settings description found: {description}");
                        }
                        unlocalizedStrings.Add(description);
                    }
                    if (!string.IsNullOrWhiteSpace(displayAs) && !source.Contains(displayAs))
                    {
                        if (!reportMode)
                        {
                            var location = AnalyzerTools.GenerateLocation(settingsEntryList["DisplayAs"], displayAsOrig, resourceName);
                            AnalyzerTools.PrintFromLocation(location, resourceName, "NLOC0003", $"Unlocalized settings display found: {displayAs}");
                        }
                        unlocalizedStrings.Add(displayAs);
                    }
                    if (!string.IsNullOrWhiteSpace(knownAddonDisplay) && !source.Contains(knownAddonDisplay))
                    {
                        if (!reportMode)
                        {
                            var location = AnalyzerTools.GenerateLocation(settingsEntryList["display"], knownAddonDisplayOrig, resourceName);
                            AnalyzerTools.PrintFromLocation(location, resourceName, "NLOC0003", $"Unlocalized known addon display found: {knownAddonDisplay}");
                        }
                        unlocalizedStrings.Add(knownAddonDisplay);
                    }

                    // Helper function to check a key, because a key can be a multivar
                    void CheckKeys(JArray keys)
                    {
                        foreach (var key in keys)
                        {
                            string keyNameOrig = (string?)key["Name"] ?? "";
                            string keyType = (string?)key["Type"] ?? "";
                            string keyDescOrig = (string?)key["Description"] ?? "";
                            string keyName = keyNameOrig.Replace("\\\"", "\"");
                            string keyDesc = keyDescOrig.Replace("\\\"", "\"");
                            if (!string.IsNullOrWhiteSpace(keyName) && !source.Contains(keyName))
                            {
                                if (!reportMode)
                                {
                                    var location = AnalyzerTools.GenerateLocation(key["Name"], keyNameOrig, resourceName);
                                    AnalyzerTools.PrintFromLocation(location, resourceName, "NLOC0003", $"Unlocalized key name found: {keyName}");
                                }
                                unlocalizedStrings.Add(keyName);
                            }
                            if (!string.IsNullOrWhiteSpace(keyDesc) && !source.Contains(keyDesc))
                            {
                                if (!reportMode)
                                {
                                    var location = AnalyzerTools.GenerateLocation(key["Description"], keyDescOrig, resourceName);
                                    AnalyzerTools.PrintFromLocation(location, resourceName, "NLOC0003", $"Unlocalized key description found: {keyDesc}");
                                }
                                unlocalizedStrings.Add(keyDesc);
                            }
                            if (!string.IsNullOrWhiteSpace(keyType) && keyType == "SMultivar")
                            {
                                var multiVarKeys = (JArray?)key["Variables"];
                                if (multiVarKeys is null || multiVarKeys.Count == 0)
                                    continue;
                                CheckKeys(multiVarKeys);
                            }
                        }
                    }

                    // Now, check the keys
                    JArray? keys = (JArray?)settingsEntryList["Keys"];
                    if (keys is null || keys.Count == 0)
                        continue;
                    CheckKeys(keys);
                }
            }
            return [.. unlocalizedStrings.Distinct()];
        }

        internal static string[] GetLocalizedStrings(string resourceName, bool reportMode = false)
        {
            List<string> localizedStrings = [];
            HashSet<string> source =
                reportMode ? [.. Reporter.localizationList] : [.. Cleaner.localizationList];

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
                if (!string.IsNullOrWhiteSpace(description) && localizable && source.Contains(description))
                    localizedStrings.Add(description);
            }
            else if (document.Type == JTokenType.Array)
            {
                // It's likely a settings entry list, but verify
                foreach (var settingsEntryList in document)
                {
                    // Check the description and the display
                    string description = ((string?)settingsEntryList["Desc"] ?? "").Replace("\\\"", "\"");
                    string displayAs = ((string?)settingsEntryList["DisplayAs"] ?? "").Replace("\\\"", "\"");
                    string knownAddonDisplay = ((string?)settingsEntryList["display"] ?? "").Replace("\\\"", "\"");
                    if (!string.IsNullOrWhiteSpace(description) && source.Contains(description))
                        localizedStrings.Add(description);
                    if (!string.IsNullOrWhiteSpace(displayAs) && source.Contains(displayAs))
                        localizedStrings.Add(displayAs);
                    if (!string.IsNullOrWhiteSpace(knownAddonDisplay) && source.Contains(knownAddonDisplay))
                        localizedStrings.Add(knownAddonDisplay);

                    // Helper function to check a key, because a key can be a multivar
                    void CheckKeys(JArray keys)
                    {
                        foreach (var key in keys)
                        {
                            string keyType = (string?)key["Type"] ?? "";
                            string keyName = ((string?)key["Name"] ?? "").Replace("\\\"", "\"");
                            string keyDesc = ((string?)key["Description"] ?? "").Replace("\\\"", "\"");
                            if (!string.IsNullOrWhiteSpace(keyName) && source.Contains(keyName))
                                localizedStrings.Add(keyName);
                            if (!string.IsNullOrWhiteSpace(keyDesc) && source.Contains(keyDesc))
                                localizedStrings.Add(keyDesc);
                            if (!string.IsNullOrWhiteSpace(keyType) && keyType == "SMultivar")
                            {
                                var multiVarKeys = (JArray?)key["Variables"];
                                if (multiVarKeys is null || multiVarKeys.Count == 0)
                                    continue;
                                CheckKeys(multiVarKeys);
                            }
                        }
                    }

                    // Now, check the keys
                    JArray? keys = (JArray?)settingsEntryList["Keys"];
                    if (keys is null || keys.Count == 0)
                        continue;
                    CheckKeys(keys);
                }
            }
            return [.. localizedStrings.Distinct()];
        }
    }
}
