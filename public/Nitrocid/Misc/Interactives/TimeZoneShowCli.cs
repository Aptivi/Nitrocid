//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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

using KS.Languages;
using System.Collections.Generic;
using System.Collections;
using KS.ConsoleBase.Interactive;
using KS.Kernel.Time.Timezones;
using KS.Kernel.Time.Renderers;

namespace KS.Misc.Interactives
{
    /// <summary>
    /// Time zone showing class
    /// </summary>
    public class TimeZoneShowCli : BaseInteractiveTui, IInteractiveTui
    {

        private static readonly string[] zones = TimeZones.GetTimeZoneNames();

        /// <summary>
        /// Time zone showing CLI bindings
        /// </summary>
        public override List<InteractiveTuiBinding> Bindings { get; set; } = [];

        /// <inheritdoc/>
        public override IEnumerable PrimaryDataSource =>
            zones;

        /// <inheritdoc/>
        public override int RefreshInterval =>
            1000;

        /// <inheritdoc/>
        public override string GetInfoFromItem(object item)
        {
            string selectedZone = (string)item;
            var time = TimeZones.GetTimeZoneTimes()[selectedZone];
            var info = TimeZones.GetZoneInfo(selectedZone);
            return
                $"{Translate.DoTranslation("Date")}: {TimeDateRenderers.RenderDate(time)}\n" +
                $"{Translate.DoTranslation("Time")}: {TimeDateRenderers.RenderTime(time)}\n" +
                $"{Translate.DoTranslation("Display name")}: {info.DisplayName}\n" +
                $"{Translate.DoTranslation("Standard name")}: {info.StandardName}";
        }

        /// <inheritdoc/>
        public override void RenderStatus(object item)
        {
            string selectedZone = (string)item;
            var time = TimeZones.GetTimeZoneTimes()[selectedZone];
            Status = $"{TimeDateRenderers.Render(time)}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item)
        {
            string selectedZone = (string)item;
            return selectedZone;
        }

    }
}
