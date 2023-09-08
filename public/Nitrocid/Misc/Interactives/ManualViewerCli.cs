
// Nitrocid KS  Copyright (C) 2018-2019  Aptivi
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

using KS.ConsoleBase.Interactive;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Languages;
using KS.Misc.Text;
using KS.Modifications.ManPages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace KS.Misc.Interactives
{
    /// <summary>
    /// Manual viewer class
    /// </summary>
    public class ManualViewerCli : BaseInteractiveTui, IInteractiveTui
    {
        internal string modName = "";

        /// <summary>
        /// Manual viewer keybindings
        /// </summary>
        public override List<InteractiveTuiBinding> Bindings { get; set; } = new()
        {
            // Operations
            new InteractiveTuiBinding(/* Localizable */ "Info",        ConsoleKey.F1, (manual, _) => ShowManualInfo(manual), true)
        };

        /// <inheritdoc/>
        public override IEnumerable PrimaryDataSource =>
            PageManager.ListAllPagesByMod(modName);

        /// <inheritdoc/>
        public override string GetInfoFromItem(object item)
        {
            // Get some info from the manual
            Manual selectedManual = (Manual)item;
            bool hasTitle = !string.IsNullOrEmpty(selectedManual.Title);
            bool hasBody = !string.IsNullOrEmpty(selectedManual.Body.ToString());

            // Generate the rendered text
            string finalRenderedManualTitle = hasTitle ?
                $"{selectedManual.Title} [v{selectedManual.Revision}]" :
                $"{selectedManual.Name} [v{selectedManual.Revision}]";
            string finalRenderedManualBody = hasBody ?
                selectedManual.Body.ToString() :
                Translate.DoTranslation("Unfortunately, this manual page doesn't have any contents. However, this documentation might have been found under the mod vendor's webpage. If you still can't find this documentation, ask the developers of the mod for more information.");

            // Render them to the second pane
            return
                finalRenderedManualTitle + CharManager.NewLine +
                new string('-', finalRenderedManualTitle.Length) + CharManager.NewLine + CharManager.NewLine +
                finalRenderedManualBody + CharManager.NewLine + CharManager.NewLine +
                Translate.DoTranslation("Presented to you by") + $" {selectedManual.ModName}";
            ;
        }

        /// <inheritdoc/>
        public override void RenderStatus(object item)
        {
            // Get some info from the manual
            Manual selectedManual = (Manual)item;
            bool hasTitle = !string.IsNullOrEmpty(selectedManual.Title);

            // Generate the rendered text
            string finalRenderedManualName = hasTitle ?
                selectedManual.Title :
                selectedManual.Name;

            // Render them to the status
            Status = finalRenderedManualName;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item)
        {
            Manual manual = (Manual)item;
            return manual.Name;
        }

        private static void ShowManualInfo(object item)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            Manual manual = (Manual)item;
            bool hasTitle = !string.IsNullOrEmpty(manual.Title);
            bool hasBody = !string.IsNullOrEmpty(manual.Body.ToString());

            string finalRenderedManualTitle = hasTitle ?
                $"{manual.Title}" :
                $"{manual.Name}";
            string finalRenderedManualBody = hasBody ?
                Translate.DoTranslation("Content length") + $": {manual.Body.Length}" :
                Translate.DoTranslation("No contents.");
            string finalRenderedManualRevision = hasBody ?
                $"v{manual.Revision}" :
                Translate.DoTranslation("No revision.");
            finalInfoRendered.AppendLine(finalRenderedManualTitle);
            finalInfoRendered.AppendLine(finalRenderedManualBody);
            finalInfoRendered.AppendLine(finalRenderedManualRevision);
            finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));

            // Now, render the info box
            InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
            RedrawRequired = true;
        }
    }
}
