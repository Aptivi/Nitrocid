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

using System;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;

#if NKS_EXTENSIONS
using Nitrocid.Base.Kernel.Extensions;
#endif

namespace Nitrocid.Base.Network.Types.RSS
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
#if NKS_EXTENSIONS
                    var addonType = InterAddonTools.GetTypeFromAddon(KnownAddons.AddonShellPacks, "Nitrocid.ShellPacks.Tools.RSSShellTools");
                    var Feed = InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.AddonShellPacks, "GetFirstArticle", addonType, Config.MainConfig.RssHeadlineUrl);
                    if (Feed is (string feedTitle, string articleTitle))
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RSS_LATESTNEWS") + " {0}: ", false, ThemeColorType.ListEntry, feedTitle);
                        TextWriterColor.Write(articleTitle, true, ThemeColorType.ListValue);
                    }
#else
                    throw new KernelException(KernelExceptionType.AddonManagement, LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RSS_LATESTNEWS_NEEDSADDON"));
#endif
                }
                catch (KernelException ex) when (ex.ExceptionType == KernelExceptionType.AddonManagement)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RSS_LATESTNEWS_NEEDSADDON"), true, ThemeColorType.Tip);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RSS_FETCHFAILED"), true, ThemeColorType.Error);
                }
            }
        }
    }
}
