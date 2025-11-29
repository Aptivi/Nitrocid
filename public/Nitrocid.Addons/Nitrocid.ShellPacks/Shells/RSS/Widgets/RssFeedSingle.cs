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

using System.Text;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Widgets;
using Nitrocid.Base.Kernel.Debugging;
using System;
using Nitrocid.ShellPacks.Tools;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.ShellPacks.Shells.RSS.Widgets
{
    internal class RssFeedSingle : BaseWidget, IWidget
    {
        (string feedTitle, string articleTitle)? article = null;

        public override string Cleanup(int left, int top, int width, int height)
        {
            article = null;
            return "";
        }

        public override string Initialize(int left, int top, int width, int height) =>
            "";

        public override string Render(int left, int top, int width, int height)
        {
            var display = new StringBuilder();

            // Write a single RSS feed
            var feedRendered = new BoundedText()
            {
                Left = left,
                Top = top,
                Width = width,
                Height = height,
                Text = UpdateHeadline(),
            };
            display.Append(feedRendered.Render());
            return display.ToString();
        }

        private string UpdateHeadline()
        {
            try
            {
                article ??= RSSShellTools.GetFirstArticle(Config.MainConfig.RssHeadlineUrl);
                if (article is (string feedTitle, string articleTitle))
                    return LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_RSSFEED_FROM") + $" {feedTitle}: {articleTitle}";
                return LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_RSSFEED_NOFEED");
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_FETCHFAILED");
            }
        }
    }
}
