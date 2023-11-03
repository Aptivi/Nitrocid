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

using System;
using KS.ConsoleBase.Colors;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Languages;
using Syndian.Instance;
using KS.ConsoleBase.Writers.ConsoleWriters;

namespace KS.Network.RSS
{
    /// <summary>
    /// RSS tools module
    /// </summary>
    public static class RSSTools
    {
        /// <summary>
        /// Whether to show the RSS headline each login
        /// </summary>
        public static bool ShowHeadlineOnLogin =>
            Config.MainConfig.ShowHeadlineOnLogin;
        /// <summary>
        /// RSS headline URL
        /// </summary>
        public static string RssHeadlineUrl =>
            Config.MainConfig.RssHeadlineUrl;

        /// <summary>
        /// Show a headline on login
        /// </summary>
        public static void ShowHeadlineLogin()
        {
            if (ShowHeadlineOnLogin)
            {
                try
                {
                    var Feed = new RSSFeed(RssHeadlineUrl, RSSFeedType.Infer);
                    if (Feed.FeedArticles.Count > 0)
                    {
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Latest news from") + " {0}: ", false, KernelColorType.ListEntry, Feed.FeedTitle);
                        TextWriterColor.WriteKernelColor(Feed.FeedArticles[0].ArticleTitle, true, KernelColorType.ListValue);
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Failed to get the latest news."), true, KernelColorType.Error);
                }
            }
        }
    }
}
