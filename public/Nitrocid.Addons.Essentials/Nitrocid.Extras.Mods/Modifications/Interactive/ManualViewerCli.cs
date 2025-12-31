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

using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Interactive;
using Nitrocid.Base.Languages;
using System.Collections.Generic;
using System.Text;
using Textify.General;
using Nitrocid.Extras.Mods.Modifications.ManPages;

namespace Nitrocid.Extras.Mods.Modifications.Interactive
{
    /// <summary>
    /// Manual viewer class
    /// </summary>
    public class ManualViewerCli : BaseInteractiveTui<Manual>, IInteractiveTui<Manual>
    {
        internal string modName = "";

        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                HelpTitle = /* Localizable */ "NKS_MODS_MANTUI_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_MODS_MANTUI_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_MODS_MANTUI_HELP01_BODY") + "\n\n" +
#pragma warning disable NLOC0001
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/advanced-and-power-users/inner-workings/inner-essentials/mod-manual-pages",
#pragma warning restore NLOC0001
            }
        ];

        /// <inheritdoc/>
        public override IEnumerable<Manual> PrimaryDataSource =>
            PageManager.ListAllPagesByMod(modName);

        /// <inheritdoc/>
        public override string GetInfoFromItem(Manual item)
        {
            // Get some info from the manual
            Manual selectedManual = item;
            bool hasTitle = !string.IsNullOrEmpty(selectedManual.Title);
            bool hasBody = !string.IsNullOrEmpty(selectedManual.Body?.ToString());

            // Generate the rendered text
            string noDocs = LanguageTools.GetLocalized("NKS_MODS_MANTUI_HASNOCONTENTS");
            string finalRenderedManualTitle = hasTitle ?
                $"{selectedManual.Title} [v{selectedManual.Revision}]" :
                $"{selectedManual.Name} [v{selectedManual.Revision}]";
            string finalRenderedManualBody = hasBody ?
                selectedManual.Body?.ToString() ?? noDocs :
                noDocs;

            // Render them to the second pane
            return
                finalRenderedManualTitle + CharManager.NewLine +
                new string('-', finalRenderedManualTitle.Length) + CharManager.NewLine + CharManager.NewLine +
                finalRenderedManualBody + CharManager.NewLine + CharManager.NewLine +
                LanguageTools.GetLocalized("NKS_MODS_MANTUI_PRESENTED") + $" {selectedManual.ModName}";
            ;
        }

        /// <inheritdoc/>
        public override string GetStatusFromItem(Manual item)
        {
            // Get some info from the manual
            Manual selectedManual = item;
            bool hasTitle = !string.IsNullOrEmpty(selectedManual.Title);

            // Generate the rendered text
            string finalRenderedManualName = hasTitle ?
                selectedManual.Title :
                selectedManual.Name;

            // Render them to the status
            return finalRenderedManualName;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(Manual item)
        {
            Manual manual = item;
            return manual.Name;
        }

        internal void ShowManualInfo(Manual? item)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            Manual? manual = item;
            if (manual is null)
                return;
            bool hasTitle = !string.IsNullOrEmpty(manual.Title);
            bool hasBody = !string.IsNullOrEmpty(manual.Body?.ToString());

            string finalRenderedManualTitle = hasTitle ?
                $"{manual.Title}" :
                $"{manual.Name}";
            string finalRenderedManualBody = hasBody ?
                LanguageTools.GetLocalized("NKS_MODS_MANTUI_CONTENTLENGTH") + $": {manual.Body?.Length}" :
                LanguageTools.GetLocalized("NKS_MODS_MANTUI_NOCONTENTS");
            string finalRenderedManualRevision = hasBody ?
                $"v{manual.Revision}" :
                LanguageTools.GetLocalized("NKS_MODS_MANTUI_NOREVISION");
            finalInfoRendered.AppendLine(finalRenderedManualTitle);
            finalInfoRendered.AppendLine(finalRenderedManualBody);
            finalInfoRendered.AppendLine(finalRenderedManualRevision);

            // Now, render the info box
            InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
        }
    }
}
