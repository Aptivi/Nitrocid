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

using System;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Extensions;

namespace Nitrocid.Network.Types.RSS
{
    /// <summary>
    /// RSS tools module
    /// </summary>
    public static class RSSTools
    {
        /// <summary>
        /// Show a headline on login
        /// </summary>
        public static void ShowHeadlineLogin()
        {
            if (Config.MainConfig.ShowHeadlineOnLogin)
            {
                try
                {
                    var Feed = InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasRssShell, "GetFirstArticle", Config.MainConfig.RssHeadlineUrl);
                    if (Feed is (string feedTitle, string articleTitle))
                    {
                        TextWriters.Write(Translate.DoTranslation("Latest news from") + " {0}: ", false, KernelColorType.ListEntry, feedTitle);
                        TextWriters.Write(articleTitle, true, KernelColorType.ListValue);
                    }
                }
                catch (KernelException ex) when (ex.ExceptionType == KernelExceptionType.AddonManagement)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriters.Write(Translate.DoTranslation("To be able to get the latest news, you must install the RSS Shell Extras addon. You can use the 'getaddons' command to get all the addons!"), true, KernelColorType.Tip);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriters.Write(Translate.DoTranslation("Failed to get the latest news."), true, KernelColorType.Error);
                }
            }
        }
    }
}
