//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Nitrocid.Locales.Serializer;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters;

namespace Nitrocid.Locales.Actions
{
    internal static class Trimmer
    {
        internal static void Execute(bool dry = false)
        {
            // Check to see if we're running on the KS repo
            string metadataFile = "../../../Nitrocid.Locales/Translations/Metadata.json";
            string addonMetadataFile = "../../../Nitrocid.Locales/AddonTranslations/Metadata.json";
            if (File.Exists(metadataFile) && File.Exists(addonMetadataFile))
            {
                // Iterate through all the language files for the main project
                TextWriterColor.Write("Checking for unused languages...");
                var langs = LocalizationLister.PopulateLanguages().Keys.ToArray();
                var metas = LocalizationLister.PopulateLanguageMetadata();
                List<string> redundant = [];
                foreach (var languagePath in langs)
                {
                    string langName = Path.GetFileNameWithoutExtension(languagePath);

                    // Check to see if that name is found in the metadata
                    bool found = false;
                    foreach (var metadata in metas)
                    {
                        if (metadata.ThreeLetterLanguageName == langName)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        redundant.Add(languagePath);
                }

                // Now, either list or delete all redundant files
                if (dry)
                {
                    var listing = new Listing()
                    {
                        Objects = redundant,
                        KeyColor = ConsoleColors.Yellow,
                        ValueColor = ConsoleColors.Red,
                    };
                    TextWriterRaw.WritePlain(listing.Render());
                }
                else
                {
                    foreach (var languagePath in redundant)
                    {
                        string langName = Path.GetFileNameWithoutExtension(languagePath);
                        TextWriterColor.Write($"Removing {langName} [{languagePath}]...");
                        File.Delete(languagePath);
                    }
                }
                if (redundant.Count == 0)
                    TextWriterColor.Write($"There are no redundant languages! Congratulations!");
                else
                    TextWriterColor.Write($"Removed all redundant languages! Please verify that only the redundant ones have been removed. You may need to remove their associated JSON files and their metadata.");
            }
            else
                TextWriterColor.Write("This internal program needs to be run within the Nitrocid KS repository.", true, ConsoleColors.Red);
        }
    }
}
