//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Figletize;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Languages;
using Nitrocid.Misc.Splash;
using Nitrocid.Misc.Text.Probers.Placeholder;
using Textify.General;
using Terminaux.Base;
using Terminaux.Inputs;

namespace Nitrocid.ConsoleBase.Writers.MiscWriters
{
    /// <summary>
    /// Welcome message writer
    /// </summary>
    public static class WelcomeMessage
    {

        internal static string customBanner = "";
        internal static string[] tips = [];

        /// <summary>
        /// The customized message banner to write. If none is specified, or if it only consists of whitespace, it uses the default message.
        /// </summary>
        public static string CustomBanner =>
            Config.MainConfig.CustomBanner;

        /// <summary>
        /// Show tips on log-in
        /// </summary>
        public static bool ShowTip { get; internal set; }

        /// <summary>
        /// Whether to show the app information on boot
        /// </summary>
        public static bool ShowAppInfoOnBoot =>
            Config.MainConfig.ShowAppInfoOnBoot;

        /// <summary>
        /// Enable marquee on startup
        /// </summary>
        public static bool StartScroll =>
            Config.MainConfig.StartScroll;

        /// <summary>
        /// Development notice acknowledged
        /// </summary>
        public static bool DevNoticeConsented =>
            Config.MainConfig.DevNoticeConsented;

        /// <summary>
        /// Gets the custom banner actual text with placeholders parsed
        /// </summary>
        public static string GetCustomBanner()
        {
            // The default message to write
            string MessageWrite = "     --> " + Translate.DoTranslation("Welcome to Nitrocid Kernel! - Version") + " v{0} <--     ";

            // Check to see if user specified custom message
            if (!string.IsNullOrWhiteSpace(customBanner))
                MessageWrite = PlaceParse.ProbePlaces(customBanner);

            // Just return the result
            return MessageWrite;
        }

        /// <summary>
        /// Writes the welcoming message to the console (welcome to kernel)
        /// </summary>
        public static void WriteMessage()
        {
            if (!SplashManager.EnableSplash)
            {
                ConsoleWrapper.CursorVisible = false;

                // The default message to write
                string MessageWrite = GetCustomBanner();

                // Finally, write the message
                if (StartScroll)
                    TextDynamicWriters.WriteSlowly(MessageWrite, true, 10d, KernelColorType.Banner, KernelMain.VersionFullStr);
                else
                    TextWriters.Write(MessageWrite, true, KernelColorType.Banner, KernelMain.VersionFullStr);

                string FigletRenderedBanner = FigletTools.RenderFiglet($"{KernelMain.VersionFullStr}", Config.MainConfig.DefaultFigletFontName);
                TextWriterColor.Write(CharManager.NewLine + FigletRenderedBanner + CharManager.NewLine);
                ConsoleWrapper.CursorVisible = true;
            }
        }

        /// <summary>
        /// Writes the license
        /// </summary>
        public static void WriteLicense()
        {
            TextFancyWriters.WriteSeparator(Translate.DoTranslation("License information"), true, KernelColorType.Stage);
            TextWriters.Write(GetLicenseString(), true, KernelColorType.License);
        }

        /// <summary>
        /// Gets the license string
        /// </summary>
        /// <returns></returns>
        public static string GetLicenseString() =>
            $"""
            
                Nitrocid KS  Copyright (C) 2018-2024  Aptivi
                This program comes with ABSOLUTELY NO WARRANTY, not even
                MERCHANTABILITY or FITNESS for particular purposes.
                This is free software, and you are welcome to redistribute it
                under certain conditions; See COPYING file in source code.

            * {Translate.DoTranslation("For more information about the terms and conditions of using this software, visit")} http://www.gnu.org/licenses/
            """;

        internal static void ShowDevelopmentDisclaimer()
        {
#if SPECIFIERREL
            // no-op
            return;
#else
            string message =
#if SPECIFIERDEV
                Translate.DoTranslation("You're running the development version of the kernel. While you can experience upcoming features which may exist in the final release, you may run into bugs, instabilities, or even data loss. We recommend using the stable version, if possible.")
#elif SPECIFIERRC
                Translate.DoTranslation("You're running the release candidate version of the kernel. While you can experience the final touches, you may run into bugs, instabilities, or even data loss. We recommend using the stable version, if possible.")
#elif SPECIFIERREL == false
                Translate.DoTranslation("We recommend against running this version of the kernel, because it is unsupported. If you have downloaded this kernel from unknown sources, this message may appear. Please download from our official downloads page.")
#endif
            ;
            string tip = Translate.DoTranslation("To dismiss forever, either press ENTER on the \"Acknowledged\" button here by highlighting it using the left arrow on your keyboard, or enable \"Development notice acknowledged\" in the kernel settings.");

            // Show development disclaimer
            if (SplashManager.EnableSplash)
            {
                InputChoiceInfo[] answers = [
                    new InputChoiceInfo("ok", Translate.DoTranslation("OK")),
                    new InputChoiceInfo("acknowledged", Translate.DoTranslation("Acknowledged")),
                ];
                int answer = InfoBoxButtonsColor.WriteInfoBoxButtonsColor(
                    Translate.DoTranslation("Development notice"),
                    answers,
                    $"{message}\n\n{tip}",
                    KernelColorTools.GetColor(KernelColorType.DevelopmentWarning)
                );
                if (answer == 1)
                    Config.MainConfig.DevNoticeConsented = true;
            }
            else
                TextWriters.Write($"* {message}\n* {tip}", true, KernelColorType.DevelopmentWarning);
#endif
        }

        internal static void ShowUnusualEnvironmentWarning()
        {
            string message = Translate.DoTranslation("You're running Nitrocid KS on an unusual environment. Please verify that you've started the kernel either directly or through GRILO. If you're sure that you've started the kernel in a usual way, it might be because you're running an unsupported version of Nitrocid KS.");
            string message2 = Translate.DoTranslation("Please note that running Nitrocid KS on an unusual environment means that some features are limited. You won't be able to load mods and configurations.");

            // Show unusual environment notice
            if (SplashManager.EnableSplash)
            {
                InputChoiceInfo[] answers = [
                    new InputChoiceInfo("ok", Translate.DoTranslation("OK")),
                ];
                int answer = InfoBoxButtonsColor.WriteInfoBoxButtonsColor(
                    Translate.DoTranslation("Unusual environment notice"),
                    answers,
                    message + "\n\n" + message2,
                    KernelColorTools.GetColor(KernelColorType.Warning)
                );
            }
            else
            {
                TextWriters.Write($"* {message}", true, KernelColorType.Warning);
                TextWriters.Write($"* {message2}", true, KernelColorType.Warning);
            }
        }

        internal static string GetRandomTip()
        {
            // Get a random tip
            string tip = Translate.DoTranslation("that you can get extra tips from the kernel addon shipped with the full build of Nitrocid?");
            if (tips.Length > 0)
            {
                int tipIdx = RandomDriver.RandomIdx(tips.Length);
                tip = Translate.DoTranslation(tips[tipIdx]);
            }
            return tip;
        }

        internal static void ShowRandomTip()
        {
            // Get a random tip and print it
            TextWriters.Write(
                "* " + Translate.DoTranslation("Pro tip: Did you know") + " " + GetRandomTip(), true, KernelColorType.Tip);
        }

    }
}
