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

using System;
using System.Collections.Generic;
using System.IO;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.LocaleGen.Core.Serializer;
using Terminaux.Base.Checks;

namespace Nitrocid.LocaleGen
{
    internal static class Generator
    {

        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] Args)
        {
            // Check console
            ConsoleChecker.CheckConsole();

            // Parse for arguments
            var arguments = new List<string>();
            var switches = new List<string>();
            bool custom = true;
            bool addon = true;
            bool normal = true;
            bool dry = false;
            var copyToResources = false;
            string toSearch = "";
            if (Args.Length > 0)
            {
                // Separate between switches and arguments
                foreach (string Arg in Args)
                {
                    if (Arg.StartsWith("--"))
                    {
                        // It's a switch.
                        switches.Add(Arg);
                    }
                    else
                    {
                        // It's an argument.
                        arguments.Add(Arg);
                    }
                }

                // Change the values of custom and normal to match the switches provided
                custom = switches.Contains("--CustomOnly") || switches.Contains("--All");
                normal = switches.Contains("--NormalOnly") || switches.Contains("--All");
                addon = switches.Contains("--AddonOnly") || switches.Contains("--All");
                copyToResources = switches.Contains("--CopyToResources");
                dry = switches.Contains("--Dry");

                // Check to see if we're going to parse one language
                bool singular = switches.Contains("--Singular");
                if (singular & arguments.Count > 0)
                {
                    // Select the language to be searched
                    toSearch = arguments[0];
                }
                else if (singular)
                {
                    // We can't be singular without providing the language!
                    TextWriters.Write("Provide a language to generate.", true, KernelColorType.Error);
                    Environment.Exit(1);
                }

                // Check to see if we're going to show help
                if (switches.Contains("--Help"))
                {
                    TextWriterColor.Write("{0} [--CustomOnly|--NormalOnly|--All|--Singular|--CopyToResources|--Dry|--Help]", Path.GetFileName(Environment.GetCommandLineArgs()[0]));
                    Environment.Exit(1);
                }
            }

            try
            {
                // Get the translation folders
                string translations = Path.GetFullPath("Translations");
                string translationsAddon = Path.GetFullPath("AddonTranslations");
                string customs = Path.GetFullPath("CustomLanguages");

                // Warn if dry
                if (dry)
                    TextWriters.Write("Running in dry mode. No changes will be made. Take out the --Dry switch if you really want to apply changes. Look at the debug window.", true, KernelColorType.Warning);

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
                TextWriters.Write("Unexpected error in converter:" + $" {ex.Message}", true, KernelColorType.Error);
                TextWriters.Write(ex.StackTrace, true, KernelColorType.Error);
            }
        }
    }
}
