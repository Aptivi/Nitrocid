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

using System;
using System.IO;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Locales.Serializer;
using Terminaux.Colors.Data;

namespace Nitrocid.Locales.Actions
{
    internal static class Generator
    {
        internal static void Execute(bool custom = true, bool addon = true, bool normal = true, bool dry = false, bool copyToResources = false, string toSearch = "")
        {
            try
            {
                // Get the translation folders
                string translations = Path.GetFullPath("Translations");
                string translationsAddon = Path.GetFullPath("AddonTranslations");
                string customs = Path.GetFullPath("CustomLanguages");

                // Warn if dry
                if (dry)
                    TextWriterColor.WriteColor("Running in dry mode. No changes will be made. Take out the -dry switch if you really want to apply changes. Look at the debug window.", ConsoleColors.Yellow);

                // Now, do the job!
                if (normal)
                    LanguageGenerator.GenerateLocaleFiles(translations, toSearch, copyToResources, dry);
                if (addon)
                    LanguageGenerator.GenerateLocaleFiles(translationsAddon, toSearch, copyToResources, dry);
                if (custom)
                    LanguageGenerator.GenerateLocaleFiles(customs, toSearch, copyToResources, dry);
            }
            catch (Exception ex)
            {
                TextWriterColor.WriteColor("Unexpected error in converter:" + $" {ex.Message}", ConsoleColors.Red);
                TextWriterColor.WriteColor(ex.StackTrace ?? "No stack trace!", ConsoleColors.Red);
            }
        }
    }
}
