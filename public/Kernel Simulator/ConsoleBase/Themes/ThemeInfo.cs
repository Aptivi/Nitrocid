
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.IO;
using ColorSeq;
using Newtonsoft.Json.Linq;

namespace KS.ConsoleBase.Themes
{
    /// <summary>
    /// Theme information class
    /// </summary>
    public class ThemeInfo
    {

        /// <summary>
        /// Input color set by theme
        /// </summary>
        public Color ThemeInputColor { get; private set; }
        /// <summary>
        /// License color set by theme
        /// </summary>
        public Color ThemeLicenseColor { get; private set; }
        /// <summary>
        /// Continuable kernel error color set by theme
        /// </summary>
        public Color ThemeContKernelErrorColor { get; private set; }
        /// <summary>
        /// Uncontinuable kernel error color set by theme
        /// </summary>
        public Color ThemeUncontKernelErrorColor { get; private set; }
        /// <summary>
        /// Host name color set by theme
        /// </summary>
        public Color ThemeHostNameShellColor { get; private set; }
        /// <summary>
        /// User name color set by theme
        /// </summary>
        public Color ThemeUserNameShellColor { get; private set; }
        /// <summary>
        /// Background color set by theme
        /// </summary>
        public Color ThemeBackgroundColor { get; private set; }
        /// <summary>
        /// Neutral text color set by theme
        /// </summary>
        public Color ThemeNeutralTextColor { get; private set; }
        /// <summary>
        /// List entry color set by theme
        /// </summary>
        public Color ThemeListEntryColor { get; private set; }
        /// <summary>
        /// List value color set by theme
        /// </summary>
        public Color ThemeListValueColor { get; private set; }
        /// <summary>
        /// Stage color set by theme
        /// </summary>
        public Color ThemeStageColor { get; private set; }
        /// <summary>
        /// General error color set by theme
        /// </summary>
        public Color ThemeErrorColor { get; private set; }
        /// <summary>
        /// General warning color set by theme
        /// </summary>
        public Color ThemeWarningColor { get; private set; }
        /// <summary>
        /// Option color set by theme
        /// </summary>
        public Color ThemeOptionColor { get; private set; }
        /// <summary>
        /// Banner color set by theme
        /// </summary>
        public Color ThemeBannerColor { get; private set; }
        /// <summary>
        /// Input color set by theme
        /// </summary>
        public Color ThemeNotificationTitleColor { get; private set; }
        /// <summary>
        /// License color set by theme
        /// </summary>
        public Color ThemeNotificationDescriptionColor { get; private set; }
        /// <summary>
        /// Continuable kernel error color set by theme
        /// </summary>
        public Color ThemeNotificationProgressColor { get; private set; }
        /// <summary>
        /// Uncontinuable kernel error color set by theme
        /// </summary>
        public Color ThemeNotificationFailureColor { get; private set; }
        /// <summary>
        /// Host name color set by theme
        /// </summary>
        public Color ThemeQuestionColor { get; private set; }
        /// <summary>
        /// User name color set by theme
        /// </summary>
        public Color ThemeSuccessColor { get; private set; }
        /// <summary>
        /// Background color set by theme
        /// </summary>
        public Color ThemeUserDollarColor { get; private set; }
        /// <summary>
        /// Neutral text color set by theme
        /// </summary>
        public Color ThemeTipColor { get; private set; }
        /// <summary>
        /// List entry color set by theme
        /// </summary>
        public Color ThemeSeparatorTextColor { get; private set; }
        /// <summary>
        /// List value color set by theme
        /// </summary>
        public Color ThemeSeparatorColor { get; private set; }
        /// <summary>
        /// Stage color set by theme
        /// </summary>
        public Color ThemeListTitleColor { get; private set; }
        /// <summary>
        /// General error color set by theme
        /// </summary>
        public Color ThemeDevelopmentWarningColor { get; private set; }
        /// <summary>
        /// General warning color set by theme
        /// </summary>
        public Color ThemeStageTimeColor { get; private set; }
        /// <summary>
        /// Option color set by theme
        /// </summary>
        public Color ThemeProgressColor { get; private set; }
        /// <summary>
        /// Banner color set by theme
        /// </summary>
        public Color ThemeBackOptionColor { get; private set; }
        /// <summary>
        /// Low priority notification border color set by theme
        /// </summary>
        public Color ThemeLowPriorityBorderColor { get; private set; }
        /// <summary>
        /// Medium priority notification border color set by theme
        /// </summary>
        public Color ThemeMediumPriorityBorderColor { get; private set; }
        /// <summary>
        /// High priority notification border color set by theme
        /// </summary>
        public Color ThemeHighPriorityBorderColor { get; private set; }
        /// <summary>
        /// Table separator color set by theme
        /// </summary>
        public Color ThemeTableSeparatorColor { get; private set; }
        /// <summary>
        /// Table header color set by theme
        /// </summary>
        public Color ThemeTableHeaderColor { get; private set; }
        /// <summary>
        /// Table value color set by theme
        /// </summary>
        public Color ThemeTableValueColor { get; private set; }
        /// <summary>
        /// Selected option color set by theme
        /// </summary>
        public Color ThemeSelectedOptionColor { get; private set; }
        /// <summary>
        /// Alternative option color set by theme
        /// </summary>
        public Color ThemeAlternativeOptionColor { get; private set; }

        /// <summary>
        /// Generates a new theme info from KS resources
        /// </summary>
        /// <param name="ThemeResourceName">Theme name (must match resource name)</param>
        public ThemeInfo(string ThemeResourceName) : this(JToken.Parse(Properties.Resources.Resources.ResourceManager.GetString(ThemeResourceName)))
        {
        }

        /// <summary>
        /// Generates a new theme info from file stream
        /// </summary>
        /// <param name="ThemeFileStream">Theme file stream reader</param>
        public ThemeInfo(StreamReader ThemeFileStream) : this(JToken.Parse(ThemeFileStream.ReadToEnd()))
        {
        }

        /// <summary>
        /// Generates a new theme info from theme resource JSON
        /// </summary>
        /// <param name="ThemeResourceJson">Theme resource JSON</param>
        protected ThemeInfo(JToken ThemeResourceJson)
        {
            ThemeInputColor = new Color(ThemeResourceJson.SelectToken("InputColor").ToString());
            ThemeLicenseColor = new Color(ThemeResourceJson.SelectToken("LicenseColor").ToString());
            ThemeContKernelErrorColor = new Color(ThemeResourceJson.SelectToken("ContKernelErrorColor").ToString());
            ThemeUncontKernelErrorColor = new Color(ThemeResourceJson.SelectToken("UncontKernelErrorColor").ToString());
            ThemeHostNameShellColor = new Color(ThemeResourceJson.SelectToken("HostNameShellColor").ToString());
            ThemeUserNameShellColor = new Color(ThemeResourceJson.SelectToken("UserNameShellColor").ToString());
            ThemeBackgroundColor = new Color(ThemeResourceJson.SelectToken("BackgroundColor").ToString());
            ThemeNeutralTextColor = new Color(ThemeResourceJson.SelectToken("NeutralTextColor").ToString());
            ThemeListEntryColor = new Color(ThemeResourceJson.SelectToken("ListEntryColor").ToString());
            ThemeListValueColor = new Color(ThemeResourceJson.SelectToken("ListValueColor").ToString());
            ThemeStageColor = new Color(ThemeResourceJson.SelectToken("StageColor").ToString());
            ThemeErrorColor = new Color(ThemeResourceJson.SelectToken("ErrorColor").ToString());
            ThemeWarningColor = new Color(ThemeResourceJson.SelectToken("WarningColor").ToString());
            ThemeOptionColor = new Color(ThemeResourceJson.SelectToken("OptionColor").ToString());
            ThemeBannerColor = new Color(ThemeResourceJson.SelectToken("BannerColor").ToString());
            ThemeNotificationTitleColor = new Color(ThemeResourceJson.SelectToken("NotificationTitleColor").ToString());
            ThemeNotificationDescriptionColor = new Color(ThemeResourceJson.SelectToken("NotificationDescriptionColor").ToString());
            ThemeNotificationProgressColor = new Color(ThemeResourceJson.SelectToken("NotificationProgressColor").ToString());
            ThemeNotificationFailureColor = new Color(ThemeResourceJson.SelectToken("NotificationFailureColor").ToString());
            ThemeQuestionColor = new Color(ThemeResourceJson.SelectToken("QuestionColor").ToString());
            ThemeSuccessColor = new Color(ThemeResourceJson.SelectToken("SuccessColor").ToString());
            ThemeUserDollarColor = new Color(ThemeResourceJson.SelectToken("UserDollarColor").ToString());
            ThemeTipColor = new Color(ThemeResourceJson.SelectToken("TipColor").ToString());
            ThemeSeparatorTextColor = new Color(ThemeResourceJson.SelectToken("SeparatorTextColor").ToString());
            ThemeSeparatorColor = new Color(ThemeResourceJson.SelectToken("SeparatorColor").ToString());
            ThemeListTitleColor = new Color(ThemeResourceJson.SelectToken("ListTitleColor").ToString());
            ThemeDevelopmentWarningColor = new Color(ThemeResourceJson.SelectToken("DevelopmentWarningColor").ToString());
            ThemeStageTimeColor = new Color(ThemeResourceJson.SelectToken("StageTimeColor").ToString());
            ThemeProgressColor = new Color(ThemeResourceJson.SelectToken("ProgressColor").ToString());
            ThemeBackOptionColor = new Color(ThemeResourceJson.SelectToken("BackOptionColor").ToString());
            ThemeLowPriorityBorderColor = new Color(ThemeResourceJson.SelectToken("LowPriorityBorderColor").ToString());
            ThemeMediumPriorityBorderColor = new Color(ThemeResourceJson.SelectToken("MediumPriorityBorderColor").ToString());
            ThemeHighPriorityBorderColor = new Color(ThemeResourceJson.SelectToken("HighPriorityBorderColor").ToString());
            ThemeTableSeparatorColor = new Color(ThemeResourceJson.SelectToken("TableSeparatorColor").ToString());
            ThemeTableHeaderColor = new Color(ThemeResourceJson.SelectToken("TableHeaderColor").ToString());
            ThemeTableValueColor = new Color(ThemeResourceJson.SelectToken("TableValueColor").ToString());
            ThemeSelectedOptionColor = new Color(ThemeResourceJson.SelectToken("SelectedOptionColor").ToString());
            ThemeAlternativeOptionColor = new Color(ThemeResourceJson.SelectToken("AlternativeOptionColor").ToString());
        }

    }
}
