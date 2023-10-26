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

using KS.ConsoleBase.Colors;
using KS.Kernel.Configuration;
using KS.Kernel.Configuration.Instances;
using KS.Kernel.Configuration.Settings;
using KS.Misc.Text;
using Newtonsoft.Json;

namespace Nitrocid.SplashPacks.Settings
{
    /// <summary>
    /// Configuration instance for splashes (to be serialized)
    /// </summary>
    public class ExtraSplashesConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries =>
            ConfigTools.GetSettingsEntries(Resources.AddonResources.AddonSplashSettings);

        /// <summary>
        /// [Simple] The progress text location
        /// </summary>
        public int SimpleProgressTextLocation { get; set; } = (int)TextLocation.Top;
        /// <summary>
        /// [Progress] The progress color
        /// </summary>
        public string ProgressProgressColor { get; set; } = KernelColorTools.GetColor(KernelColorType.Progress).PlainSequence;
        /// <summary>
        /// [Progress] The progress text location
        /// </summary>
        public int ProgressProgressTextLocation { get; set; } = (int)TextLocation.Top;
        /// <summary>
        /// [PowerLineProgress] The progress color
        /// </summary>
        public string PowerLineProgressProgressColor { get; set; } = KernelColorTools.GetColor(KernelColorType.Progress).PlainSequence;
        /// <summary>
        /// [PowerLineProgress] The progress text location
        /// </summary>
        public int PowerLineProgressProgressTextLocation { get; set; } = (int)TextLocation.Top;
        /// <summary>
        /// [Quote] The progress text location
        /// </summary>
        public int QuoteProgressTextLocation { get; set; } = (int)TextLocation.Top;
    }
}
