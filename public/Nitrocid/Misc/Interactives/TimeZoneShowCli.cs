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
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Terminaux.Inputs.Interactive;
using Nitrocid.Kernel.Time.Timezones;
using System.Linq;
using Nitrocid.Kernel.Time;

namespace Nitrocid.Misc.Interactives
{
    /// <summary>
    /// Time zone showing class
    /// </summary>
    public class TimeZoneShowCli : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            TimeZones.GetTimeZoneTimes().Select((kvp) =>
                $"[{TimeDateRenderers.RenderDate(kvp.Value, FormatType.Short)} " +
                $"{TimeDateRenderers.RenderTime(kvp.Value, FormatType.Short)}] " +
                $"{kvp.Key}"
            );

        /// <inheritdoc/>
        public override int RefreshInterval =>
            1000;

        /// <inheritdoc/>
        public override string GetInfoFromItem(string item)
        {
            string selectedZone = item[(item.IndexOf(']') + 2)..];
            var time = TimeZones.GetTimeZoneTimes()[selectedZone];
            var info = TimeZones.GetZoneInfo(selectedZone);
            return
                $"{LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TZTUI_DATE")}: {TimeDateRenderers.RenderDate(time)}\n" +
                $"{LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TZTUI_TIME")}: {TimeDateRenderers.RenderTime(time)}\n" +
                $"{LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TZTUI_DISPLAY")}: {info.DisplayName}\n" +
                $"{LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TZTUI_STDNAME")}: {info.StandardName}";
        }

        /// <inheritdoc/>
        public override string GetStatusFromItem(string item)
        {
            string selectedZone = item[(item.IndexOf(']') + 2)..];
            var time = TimeZones.GetTimeZoneTimes()[selectedZone];
            return $"{TimeDateRenderers.Render(time)}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item) =>
            item;

    }
}
