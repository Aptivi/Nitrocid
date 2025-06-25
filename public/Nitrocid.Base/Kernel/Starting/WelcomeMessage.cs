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

using Textify.Data.Figlet;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Textify.Tools.Placeholder;
using Textify.General;
using Terminaux.Base;
using Terminaux.Inputs;
using System;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Drivers.RNG;

namespace Nitrocid.Base.Kernel.Starting
{
    internal static class WelcomeMessage
    {
        internal static string customBanner = "";
        internal static string[] tips = [];

        internal static bool ShowTip { get; set; }

        internal static string GetCustomBanner()
        {
            // The default message to write
            string MessageWrite = "     --> " + LanguageTools.GetLocalized("NKS_KERNEL_STARTING_WELCOMEMESSAGE") + " v{0} <--     ";

            // Check to see if user specified custom message
            if (!string.IsNullOrWhiteSpace(customBanner))
                MessageWrite = PlaceParse.ProbePlaces(customBanner);

            // Just return the result
            return MessageWrite;
        }

        internal static void WriteMessage()
        {
            if (!Config.MainConfig.EnableSplash)
            {
                ConsoleWrapper.CursorVisible = false;

                // The default message to write
                string MessageWrite = GetCustomBanner();

                // Finally, write the message
                if (Config.MainConfig.StartScroll)
                    TextWriterSlowColor.WriteSlowlyColor(MessageWrite, true, 10d, ThemeColorsTools.GetColor(ThemeColorType.Banner), KernelReleaseInfo.VersionFullStr);
                else
                    TextWriterColor.Write(MessageWrite, true, ThemeColorType.Banner, KernelReleaseInfo.VersionFullStr);

                string FigletRenderedBanner = FigletTools.RenderFiglet($"{KernelReleaseInfo.VersionFullStr}", Config.MainConfig.DefaultFigletFontName);
                TextWriterColor.Write(CharManager.NewLine + FigletRenderedBanner + CharManager.NewLine);
                ConsoleWrapper.CursorVisible = true;
            }
        }

        internal static void WriteLicense()
        {
            SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_WELCOMEMESSAGE_LICENSE_TITLE"), ThemeColorsTools.GetColor(ThemeColorType.Stage));
            TextWriterColor.Write(GetLicenseString(), true, ThemeColorType.License);
        }

        internal static string GetLicenseString() =>
            $"""
            {LanguageTools.GetLocalized("NKS_KERNEL_STARTING_WELCOMEMESSAGE_LICENSE1")}

                Nitrocid KS  Copyright (C) 2018-2025  Aptivi
                This program comes with ABSOLUTELY NO WARRANTY, not even
                MERCHANTABILITY or FITNESS for particular purposes.
                This is free software, and you are welcome to redistribute it
                under certain conditions; See COPYING file in source code.

            * {LanguageTools.GetLocalized("NKS_KERNEL_STARTING_WELCOMEMESSAGE_LICENSE2")} http://www.gnu.org/licenses/
            """;

        internal static void ShowUnusualEnvironmentWarning()
        {
            string message = LanguageTools.GetLocalized("NKS_KERNEL_STARTING_UNUSUALENV");
            string message2 = LanguageTools.GetLocalized("NKS_KERNEL_STARTING_UNUSUALENV_DESC");

            // Show unusual environment notice
            if (Config.MainConfig.EnableSplash)
            {
                InputChoiceInfo[] answers = [
                    new InputChoiceInfo("ok", LanguageTools.GetLocalized("NKS_KERNEL_STARTING_DEVDISMISS_OK")),
                ];
                InfoBoxButtonsColor.WriteInfoBoxButtons(
                    answers,
                    message + "\n\n" + message2, new InfoBoxSettings()
                    {
                        Title = LanguageTools.GetLocalized("NKS_KERNEL_STARTING_UNUSUALENV_TITLE"),
                        ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Warning)
                    }
                );
            }
            else
            {
                TextWriterColor.Write($"* {message}", true, ThemeColorType.Warning);
                TextWriterColor.Write($"* {message2}", true, ThemeColorType.Warning);
            }
        }

        internal static string GetRandomTip()
        {
            // Get a random tip
            string tip = LanguageTools.GetLocalized("NKS_KERNEL_STARTING_TIPS_ADDONNEEDED");
            if (tips.Length > 0)
            {
                int tipIdx = RandomDriver.RandomIdx(tips.Length);
                tip = LanguageTools.GetLocalized(tips[tipIdx]);
            }
            return tip;
        }

        internal static void ShowRandomTip()
        {
            // Get a random tip and print it
            TextWriterColor.Write(
                "* " + LanguageTools.GetLocalized("NKS_KERNEL_STARTING_TIPS_PREFIX") + " " + GetRandomTip(), true, ThemeColorType.Tip);
        }
    }
}
