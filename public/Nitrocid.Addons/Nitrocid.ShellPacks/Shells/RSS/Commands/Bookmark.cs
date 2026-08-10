//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.ShellPacks.Shells.RSS.Tools;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.RSS.Commands
{
    /// <summary>
    /// Bookmarks current feed
    /// </summary>
    /// <remarks>
    /// If you want to bookmark the current feed that you're in, you can use this command.
    /// </remarks>
    class BookmarkCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "bookmark";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_COMMAND_BOOKMARK_DESC");

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var rssShell = (RSSShell?)shell ??
                throw new KernelException(KernelExceptionType.RSSShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            RSSBookmarkManager.AddRSSFeedToBookmark(rssShell.feedInstance?.FeedUrl ?? "");
            return 0;
        }
    }
}
