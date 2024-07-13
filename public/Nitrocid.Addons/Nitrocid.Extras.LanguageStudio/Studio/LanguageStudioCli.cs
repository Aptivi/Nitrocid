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
using Nitrocid.Files.Operations;
using Nitrocid.Languages;
using Nitrocid.LocaleGen.Core.Serializer;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;

namespace Nitrocid.Extras.LanguageStudio.Studio
{
    internal class LanguageStudioCli : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        internal Dictionary<string, List<string>> translatedLines = [];
        internal List<string> englishLines = [];
        internal string pathToTranslations = "";

        public override InteractiveTuiBinding[] Bindings { get; } =
        [
            // Operations
            new InteractiveTuiBinding("Translate", ConsoleKey.Enter,
                (line, _) => DoTranslate(line), true),
            new InteractiveTuiBinding("Add", ConsoleKey.A,
                (_, _) => Add(), true),
            new InteractiveTuiBinding("Remove", ConsoleKey.Delete,
                (_, idx) => Remove(idx)),
            new InteractiveTuiBinding("Save", ConsoleKey.F1,
                (_, _) => Save(), true),
        ];

        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            englishLines;

        /// <inheritdoc/>
        public override IEnumerable<string> SecondaryDataSource =>
            translatedLines.Keys;

        /// <inheritdoc/>
        public override bool SecondPaneInteractable =>
            true;

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetStatusFromItem(string item) =>
            item;

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item) =>
            item;

        private static void DoTranslate(object line)
        {
            if (InteractiveTuiStatus.CurrentPane == 2)
            {
                // Requested to remove language
                string lang = (string)line;
                int lineIdx = InteractiveTuiStatus.FirstPaneCurrentSelection - 1;
                var translatedLines = ((LanguageStudioCli)Instance).translatedLines[lang];
                string translated = translatedLines[lineIdx];
                translated = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Write your translation of") + $" \"{translated}\"");
                translated = string.IsNullOrWhiteSpace(translated) ? translatedLines[lineIdx] : translated;
                translatedLines[lineIdx] = translated;
            }
        }

        private static void Add()
        {
            if (InteractiveTuiStatus.CurrentPane == 1)
            {
                // Requested to add string
                string newString = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a new string"));
                ((LanguageStudioCli)Instance).englishLines.Add(newString);
                var lines = ((LanguageStudioCli)Instance).translatedLines;
                foreach (var translatedLang in lines.Keys)
                    lines[translatedLang].Add(translatedLang == "eng" ? newString : "???");
            }
        }

        private static void Remove(int idx)
        {
            if (InteractiveTuiStatus.CurrentPane == 1)
            {
                // Requested to remove string
                var lines = ((LanguageStudioCli)Instance).translatedLines;
                ((LanguageStudioCli)Instance).englishLines.RemoveAt(idx);
                foreach (var kvp in lines)
                    kvp.Value.RemoveAt(idx);
            }
        }

        private static void Save()
        {
            InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Saving language..."), false);
            var lines = ((LanguageStudioCli)Instance).translatedLines;
            var pathToTranslations = ((LanguageStudioCli)Instance).pathToTranslations;
            foreach (var translatedLine in lines)
            {
                string language = translatedLine.Key;
                List<string> localizations = translatedLine.Value;
                string languagePath = $"{pathToTranslations}/{language}.txt";
                Writing.WriteContents(languagePath, [.. localizations]);
            }
            LanguageGenerator.GenerateLocaleFiles(pathToTranslations);
        }
    }
}
