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

using System.Collections.Generic;
using Nitrocid.Files;
using Nitrocid.Languages;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;

namespace Nitrocid.Extras.LanguageStudio.Studio
{
    internal class LanguageStudioCli : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        internal Dictionary<string, List<string>> translatedLines = [];
        internal List<string> englishLines = [];
        internal string pathToTranslations = "";

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

        internal void DoTranslate(object? line)
        {
            if (line is null)
                return;
            if (CurrentPane == 2)
            {
                // Requested to remove language
                string lang = (string)line;
                int lineIdx = FirstPaneCurrentSelection - 1;
                var translatedLines = this.translatedLines[lang];
                string translated = translatedLines[lineIdx];
                translated = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_TRANSLATIONPROMPT", "Nitrocid.Extras.LanguageStudio") + $" \"{translated}\"");
                translated = string.IsNullOrWhiteSpace(translated) ? translatedLines[lineIdx] : translated;
                translatedLines[lineIdx] = translated;
            }
        }

        internal void Add()
        {
            if (CurrentPane == 1)
            {
                // Requested to add string
                string newString = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_NEWSTRINGPROMPT", "Nitrocid.Extras.LanguageStudio"));
                englishLines.Add(newString);
                var lines = translatedLines;
                foreach (var translatedLang in lines.Keys)
                    lines[translatedLang].Add(translatedLang == "eng" ? newString : "???");
            }
        }

        internal void Remove(int idx)
        {
            if (CurrentPane == 1)
            {
                // Requested to remove string
                var lines = translatedLines;
                englishLines.RemoveAt(idx);
                foreach (var kvp in lines)
                    kvp.Value.RemoveAt(idx);
            }
        }

        internal void Save()
        {
            InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_APP_SAVINGLANG", "Nitrocid.Extras.LanguageStudio"));
            var lines = translatedLines;
            var pathToTranslations = this.pathToTranslations;
            foreach (var translatedLine in lines)
            {
                string language = translatedLine.Key;
                List<string> localizations = translatedLine.Value;
                string languagePath = $"{pathToTranslations}/{language}.txt";
                FilesystemTools.WriteContents(languagePath, [.. localizations]);
            }
            InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_LANGUAGESTUDIO_SAVESUCCESS", "Nitrocid.Extras.LanguageStudio") + $" {pathToTranslations}");
        }
    }
}
