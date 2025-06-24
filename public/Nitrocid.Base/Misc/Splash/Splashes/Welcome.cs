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

using Terminaux.Colors;
using Textify.Data.Figlet;
using System;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Textify.General;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Colors.Transformation;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Splash;

namespace Nitrocid.Base.Misc.Splash.Splashes
{
    class SplashWelcome : BaseSplash, ISplash
    {
        private bool cleared = false;
        private ProgressBarNoText progress = new(0, 100);

        // Standalone splash information
        public override string SplashName => "Welcome";

        // Actual logic
        public override string Opening(SplashContext context)
        {
            var builder = new StringBuilder();
            progress.Position = 0;
            progress.Indeterminate = !Config.SplashConfig.WelcomeShowProgress;
            progress.Width = ConsoleWrapper.WindowWidth - 6;
            progress.ProgressForegroundColor = TransformationTools.GetDarkBackground(ThemeColorsTools.GetColor(ThemeColorType.Progress));
            progress.ProgressActiveForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Progress);
            progress.ProgressBackgroundColor = ColorTools.CurrentBackgroundColor;
            if (ConsoleResizeHandler.WasResized(true))
                cleared = false;
            if (!cleared)
            {
                cleared = true;
                builder.Append(
                    base.Opening(context)
                );
            }

            // Populate some text
            string text =
                (context == SplashContext.Preboot ?
                 LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_PLEASEWAIT") :
                 LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_LOADING"))
                .ToUpper();
            string bottomText =
                context == SplashContext.Preboot ? LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_PLEASEWAIT_INIT") :
                context == SplashContext.ShuttingDown ? LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_PLEASEWAIT_SHUTDOWN") :
                context == SplashContext.Rebooting ? LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_PLEASEWAIT_RESTART") :
                $"{LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_STARTING")} {KernelReleaseInfo.ConsoleTitle}";
            bottomText +=
                KernelEntry.SafeMode ? $" - {LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_SAFEMODE")}"  :
                KernelEntry.Maintenance ? $" - {LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_MAINTENANCE")}" :
                KernelEntry.DebugMode ? $" - {LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_DEBUGMODE")}" :
                "";

            // Write a glorious Welcome screen
            Color col = ThemeColorsTools.GetColor(ThemeColorType.Stage);
            var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
            int figHeight = FigletTools.GetFigletHeight(text, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            int bottomTextY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
            var figText = new AlignedFigletText(figFont)
            {
                Top = consoleY,
                Text = text,
                ForegroundColor = col,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            var bottomTextRenderer = new AlignedText()
            {
                Text = bottomText,
                ForegroundColor = col,
                Top = bottomTextY,
                OneLine = true,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            builder.Append(
                figText.Render() +
                bottomTextRenderer.Render()
            );
            return builder.ToString();
        }

        public override string Closing(SplashContext context, out bool delayRequired)
        {
            var builder = new StringBuilder();
            cleared = false;
            progress.Width = ConsoleWrapper.WindowWidth - 6;
            builder.Append(
                base.Opening(context)
            );
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing...");

            if (context == SplashContext.Showcase ||
                context == SplashContext.Preboot)
            {
                delayRequired = false;
                return builder.ToString();
            }

            // Write a glorious Welcome screen
            Color col = ThemeColorsTools.GetColor(ThemeColorType.Stage);
            string text =
                (context == SplashContext.StartingUp ?
                 LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME") :
                 LanguageTools.GetLocalized("NKS_KERNEL_STARTING_GOODBYE"))
                .ToUpper();
            var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
            int figHeight = FigletTools.GetFigletHeight(text, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            int bottomTextY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
            var figText = new AlignedFigletText(figFont)
            {
                Top = consoleY,
                Text = text,
                ForegroundColor = col,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            var bottomTextRenderer = new AlignedText()
            {
                Text = KernelReleaseInfo.ConsoleTitle,
                ForegroundColor = col,
                Top = bottomTextY,
                OneLine = true,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            builder.Append(
                figText.Render() +
                bottomTextRenderer.Render()
            );
            delayRequired =
                context == SplashContext.ShuttingDown && Config.MainConfig.DelayOnShutdown ||
                context != SplashContext.ShuttingDown && context != SplashContext.Rebooting;
            if ((context == SplashContext.ShuttingDown || context == SplashContext.Rebooting) && Config.MainConfig.BeepOnShutdown)
                ConsoleWrapper.Beep();
            return builder.ToString();
        }

        public override string Report(int Progress, string ProgressReport, params object[] Vars) =>
            ReportProgress(Progress, ProgressReport, ThemeColorType.Stage, Vars);

        public override string ReportWarning(int Progress, string WarningReport, Exception? ExceptionInfo, params object[] Vars) =>
            ReportProgress(Progress, WarningReport, ThemeColorType.Warning, Vars);

        public override string ReportError(int Progress, string ErrorReport, Exception? ExceptionInfo, params object[] Vars) =>
            ReportProgress(Progress, ErrorReport, ThemeColorType.Error, Vars);

        private string ReportProgress(int Progress, string ProgressReport, ThemeColorType colorType, params object[] Vars)
        {
            var builder = new StringBuilder();
            Color col = ThemeColorsTools.GetColor(colorType);
            string text =
                (SplashManager.CurrentSplashContext == SplashContext.StartingUp ?
                 LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME") :
                 LanguageTools.GetLocalized("NKS_KERNEL_STARTING_GOODBYE"))
                .ToUpper();
            var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
            int figHeight = FigletTools.GetFigletHeight(text, figFont) / 2;
            int progressTextY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            var report = new AlignedText()
            {
                Text = $"{Progress}% - {ProgressReport}".FormatString(Vars),
                ForegroundColor = col,
                Top = progressTextY - 2,
                OneLine = true,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            builder.Append(
                col.VTSequenceForeground +
                TextWriterWhereColor.RenderWhere(ConsoleClearing.GetClearLineToRightSequence(), 0, progressTextY - 2, true) +
                report.Render()
            );

            int posX = 2;
            int posY = ConsoleWrapper.WindowHeight - 2;
            progress.Position = Progress;
            progress.Width = ConsoleWrapper.WindowWidth - 6;
            builder.Append(
                RendererTools.RenderRenderable(progress, new(posX, posY))
            );
            return builder.ToString();
        }

    }
}
