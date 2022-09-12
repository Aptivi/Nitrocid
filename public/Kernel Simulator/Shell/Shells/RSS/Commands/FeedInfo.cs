
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

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.RSS.Commands
{
    /// <summary>
    /// Gets feed information
    /// </summary>
    /// <remarks>
    /// If you want to know more about the current RSS feed, you can use this command to get some information about it. It currently provides you the title, the link, the description, the feed type, and the number of articles.
    /// </remarks>
    class RSS_FeedInfoCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            TextWriterColor.Write("- " + Translate.DoTranslation("Title:") + " ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(RSSShellCommon.RSSFeedInstance.FeedTitle, true, ColorTools.ColTypes.ListValue);
            TextWriterColor.Write("- " + Translate.DoTranslation("Link:") + " ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(RSSShellCommon.RSSFeedInstance.FeedUrl, true, ColorTools.ColTypes.ListValue);
            TextWriterColor.Write("- " + Translate.DoTranslation("Description:") + " ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(RSSShellCommon.RSSFeedInstance.FeedDescription, true, ColorTools.ColTypes.ListValue);
            TextWriterColor.Write("- " + Translate.DoTranslation("Feed type:") + " ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(RSSShellCommon.RSSFeedInstance.FeedType.ToString(), true, ColorTools.ColTypes.ListValue);
            TextWriterColor.Write("- " + Translate.DoTranslation("Number of articles:") + " ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(RSSShellCommon.RSSFeedInstance.FeedArticles.Count.ToString(), true, ColorTools.ColTypes.ListValue);
        }

    }
}