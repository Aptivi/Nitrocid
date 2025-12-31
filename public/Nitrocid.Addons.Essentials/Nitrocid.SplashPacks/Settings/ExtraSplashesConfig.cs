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

using Newtonsoft.Json;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Configuration.Instances;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection.Internal;
using Nitrocid.Base.Misc.Text;

namespace Nitrocid.SplashPacks.Settings
{
    /// <summary>
    /// Configuration instance for splashes (to be serialized)
    /// </summary>
    public class ExtraSplashesConfig : BaseKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries
        {
            get
            {
                var dataStream = ResourcesManager.GetData("AddonSplashSettings.json", ResourcesType.Misc, typeof(ExtraSplashesConfig).Assembly) ??
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_SPLASHPACKS_EXCEPTION_SETTINGSENTRIES"));
                string dataString = ResourcesManager.ConvertToString(dataStream);
                return ConfigTools.GetSettingsEntries(dataString);
            }
        }

        #region Simple
        /// <summary>
        /// [Simple] The progress text location
        /// </summary>
        public int SimpleProgressTextLocation { get; set; } = (int)TextLocation.Top;
        #endregion

        #region Progress
        /// <summary>
        /// [Progress] The progress color
        /// </summary>
        public string ProgressProgressColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Progress).PlainSequence;
        /// <summary>
        /// [Progress] The progress text location
        /// </summary>
        public int ProgressProgressTextLocation { get; set; } = (int)TextLocation.Top;
        #endregion

        #region PowerLineProgress
        /// <summary>
        /// [PowerLineProgress] The progress color
        /// </summary>
        public string PowerLineProgressProgressColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Progress).PlainSequence;
        /// <summary>
        /// [PowerLineProgress] The progress text location
        /// </summary>
        public int PowerLineProgressProgressTextLocation { get; set; } = (int)TextLocation.Top;
        #endregion

        #region Quote
        /// <summary>
        /// [Quote] The progress text location
        /// </summary>
        public int QuoteProgressTextLocation { get; set; } = (int)TextLocation.Top;
        #endregion

        #region Welcome2024
        /// <summary>
        /// [Welcome2024] Show progress or not
        /// </summary>
        public bool Welcome2024ShowProgress { get; set; }
        #endregion
    }
}
