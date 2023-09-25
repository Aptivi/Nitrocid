
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Figletize;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Drivers.RNG;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Languages;
using KS.Misc.Splash;
using KS.Misc.Text;
using KS.Misc.Text.Probers.Placeholder;
using System;

namespace KS.ConsoleBase.Writers.MiscWriters
{
    /// <summary>
    /// Welcome message writer
    /// </summary>
    public static class WelcomeMessage
    {

        internal static string customBanner = "";
        internal static string[] tips = Array.Empty<string>();

        /// <summary>
        /// The customized message banner to write. If none is specified, or if it only consists of whitespace, it uses the default message.
        /// </summary>
        public static string CustomBanner =>
            Config.MainConfig.CustomBanner;
        /// <summary>
        /// Current banner figlet font
        /// </summary>
        public static string BannerFigletFont =>
            Config.MainConfig.BannerFigletFont;

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
            if (!KernelFlags.EnableSplash)
            {
                ConsoleWrapper.CursorVisible = false;

                // The default message to write
                string MessageWrite = GetCustomBanner();

                // Finally, write the message
                if (KernelFlags.StartScroll)
                {
                    TextWriterSlowColor.WriteSlowly(MessageWrite, true, 10d, KernelColorType.Banner, KernelTools.KernelVersion.ToString());
                }
                else
                {
                    TextWriterColor.Write(MessageWrite, true, KernelColorType.Banner, KernelTools.KernelVersion.ToString());
                }

                string FigletRenderedBanner = FigletTools.RenderFiglet($"{KernelTools.KernelVersion}", BannerFigletFont);
                TextWriterColor.Write(CharManager.NewLine + FigletRenderedBanner + CharManager.NewLine);
                ConsoleWrapper.CursorVisible = true;
            }
        }

        /// <summary>
        /// Writes the license
        /// </summary>
        public static void WriteLicense()
        {
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("License information"), true, KernelColorType.Stage);
            TextWriterColor.Write(CharManager.NewLine + "    Nitrocid KS  Copyright (C) 2018-2023  Aptivi" +
                                  CharManager.NewLine + "    This program comes with ABSOLUTELY NO WARRANTY, not even " +
                                  CharManager.NewLine + "    MERCHANTABILITY or FITNESS for particular purposes." +
                                  CharManager.NewLine + "    This is free software, and you are welcome to redistribute it" +
                                  CharManager.NewLine + "    under certain conditions; See COPYING file in source code." + CharManager.NewLine, true, KernelColorType.License);
            TextWriterColor.Write("* " + Translate.DoTranslation("For more information about the terms and conditions of using this software, visit") + " http://www.gnu.org/licenses/", true, KernelColorType.License);
        }

        internal static void ShowDevelopmentDisclaimer()
        {
#if SPECIFIERREL
            // no-op
            return;
#else
            // Show development disclaimer
            SplashManager.BeginSplashOut();
            InfoBoxColor.WriteInfoBox(
#if SPECIFIERDEV
                Translate.DoTranslation("You're running the development version of the kernel. While you can experience upcoming features which may exist in the final release, you may run into bugs, instabilities, or even data loss. We recommend using the stable version, if possible.")
#elif SPECIFIERRC
                Translate.DoTranslation("You're running the release candidate version of the kernel. While you can experience the final touches, you may run into bugs, instabilities, or even data loss. We recommend using the stable version, if possible.")
#elif SPECIFIERREL == false
                Translate.DoTranslation("We recommend against running this version of the kernel, because it is unsupported. If you have downloaded this kernel from unknown sources, this message may appear. Please download from our official downloads page.")
#endif
            + "\n\n" + Translate.DoTranslation("Press any key to continue."), KernelColorType.DevelopmentWarning);
            SplashManager.EndSplashOut();
#endif
        }

        internal static void ShowDotnet7Disclaimer()
        {
#if NET7_0
            // Show .NET 7.0 version disclaimer
            // TODO: Remove this when .NET 8.0 releases on November and Nitrocid KS gets re-targeted to that version on December.
            SplashManager.BeginSplashOut();
            InfoBoxColor.WriteInfoBox("You're running a .NET 7.0 version of Nitrocid KS. This is going to be used as a testing ground to ensure that we can have smooth upgrade experience to .NET 8.0. Meanwhile, you can evaluate this version until .NET 8.0 gets released on November." + "\n\n" + Translate.DoTranslation("Press any key to continue."), KernelColorType.DevelopmentWarning);
            SplashManager.EndSplashOut();
#endif
        }
        
        internal static void ShowTip()
        {
            // Get a random tip and print it
            string tip = Translate.DoTranslation("that you can get extra tips from the kernel addon shipped with the full build of Nitrocid?");
            if (tips.Length > 0)
            {
                int tipIdx = RandomDriver.RandomIdx(tips.Length);
                tip = Translate.DoTranslation(tips[tipIdx]);
            }
            TextWriterColor.Write(
                "* " + Translate.DoTranslation("Pro tip: Did you know") + " " + tip, true, KernelColorType.Tip);
        }

    }
}
