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

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Textify.General;
using Nitrocid.Base.Languages;
using SpecProbe.Software.Platform;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows the link to Nitrocid's wiki and its API doc for mods.
    /// </summary>
    class UserManualCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_USERMANUAL_MAINLINK"));
            TextWriterColor.Write(
                PlatformHelper.IsOnUnix() ?
                $"    {CharManager.GetEsc()}]8;;https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/{CharManager.GetEsc()}\\Manual page{CharManager.GetEsc()}]8;;{CharManager.GetEsc()}\\" :
                "    https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/",
                true, ThemeColorType.Tip
            );
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_USERMANUAL_APILINK"));
            TextWriterColor.Write(
                PlatformHelper.IsOnUnix() ?
                $"    {CharManager.GetEsc()}]8;;https://aptivi.github.io/Nitrocid{CharManager.GetEsc()}\\API documentation{CharManager.GetEsc()}]8;;{CharManager.GetEsc()}\\" :
                "    https://aptivi.github.io/Nitrocid",
                true, ThemeColorType.Tip);
            return 0;
        }

        public override int ExecuteDumb(CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_USERMANUAL_MAINLINK"));
            TextWriterColor.Write("    https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/", true, ThemeColorType.Tip);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_USERMANUAL_APILINK"));
            TextWriterColor.Write("    https://aptivi.github.io/Nitrocid", true, ThemeColorType.Tip);
            return 0;
        }

    }
}
