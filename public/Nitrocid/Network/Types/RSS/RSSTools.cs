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
                    var addonType = InterAddonTools.GetTypeFromAddon(KnownAddons.ExtrasRssShell, "Nitrocid.Extras.RssShell.Tools.RSSShellTools");
                    var Feed = InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasRssShell, "GetFirstArticle", addonType, Config.MainConfig.RssHeadlineUrl);
                    if (Feed is (string feedTitle, string articleTitle))
                    {
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RSS_LATESTNEWS") + " {0}: ", false, KernelColorType.ListEntry, feedTitle);
                        TextWriters.Write(articleTitle, true, KernelColorType.ListValue);
                    }
                }
                catch (KernelException ex) when (ex.ExceptionType == KernelExceptionType.AddonManagement)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RSS_LATESTNEWS_NEEDSADDON"), true, KernelColorType.Tip);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RSS_FETCHFAILED"), true, KernelColorType.Error);
                }
            }
        }
    }
}
