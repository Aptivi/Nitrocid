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
using System.Text;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Colors.Transformation;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Textify.Data.Figlet;
using Textify.General;

namespace Nitrocid.Base.Misc.Splash.Splashes
{
    class SplashWelcome : BaseSplash, ISplash
    {
        private bool cleared = false;
        private SimpleProgress progress = new(0, 100);

        // Standalone splash information
        public override string SplashName => "Welcome";

        /*
         *   The welcome splash will look like this (conceptual, 60x14 screen):
         *   
         *   :------------------------------- 60 -------------------------------
         *   :
         *   :    +----------------------------------------------------------+ (box frame color)
         *   :    |                                                          |
         *   :    |                        Nitrocid                          | (progress color, figlet, 5 blocks of height)
         *   :    |                                                          |
         *   1    |                        ModeText                          | (neutral color)
         *   4    |                                                          |
         *   :    |  ProgressReport                                          | (progress report color)
         *   :    |  ------------------------------------------------- 100%  | (progress theme color)
         *   :    |                                                          |
         *   :    +----------------------------------------------------------+
         *   :
         *   :------------------------------- 60 -------------------------------
         *   
         *   This is adjusted, depending on the terminal size.
         */

        // Actual logic
        public override string Opening(SplashContext context)
        {
            var builder = new StringBuilder();
            var progressColor = ThemeColorsTools.GetColor(ThemeColorType.Progress);
            progress.Position = 0;
            progress.Indeterminate = !Config.SplashConfig.WelcomeShowProgress;
            progress.ProgressForegroundColor = TransformationTools.GetDarkBackground(progressColor);
            progress.ProgressActiveForegroundColor = progressColor;
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
            string modeText =
                // TODO: NKS_MISC_SPLASHES_WELCOME_INITING -> "Initializing"
                // TODO: NKS_MISC_SPLASHES_WELCOME_SHUTTINGDOWN -> "Shutting down"
                // TODO: NKS_MISC_SPLASHES_WELCOME_RESTARTING -> "Restarting"
                (context == SplashContext.Preboot ? LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_INITING") :
                 context == SplashContext.ShuttingDown ? LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_SHUTTINGDOWN") :
                 context == SplashContext.Rebooting ? LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_RESTARTING") :
                 context == SplashContext.StartingUp ? LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_STARTING") :
                 LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_LOADING")) + "...";
            modeText +=
                KernelEntry.SafeMode ? $" - {LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_SAFEMODE")}"  :
                KernelEntry.Maintenance ? $" - {LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_MAINTENANCE")}" :
                KernelEntry.DebugMode ? $" - {LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME_DEBUGMODE")}" :
                "";

            // Write an infobox border
            int height = ConsoleWrapper.WindowHeight - 4;
            int width = ConsoleWrapper.WindowWidth - 8;
            int posX = ConsoleWrapper.WindowWidth / 2 - width / 2 - 1;
            int posY = ConsoleWrapper.WindowHeight / 2 - height / 2 - 1;
            string versionStr = $"{KernelReleaseInfo.ApiVersion}";
            var border = new BoxFrame()
            {
                Left = posX,
                Top = posY,
                Width = width,
                Height = height,
                UseColors = true,
                Text = versionStr,
            };
            builder.Append(border.Render());

            // Write the program name
            int interiorPosX = posX + 3;
            int interiorWidth = width - 6;
            progress.Width = interiorWidth;
            string text = $"Nitrocid KS {KernelReleaseInfo.VersionFullStr}";
            var figFont = FigletTools.GetFigletFont("thin");
            int figHeight = FigletTools.GetFigletHeight(text, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight - 2;
            var nameText = new AlignedFigletText(figFont)
            {
                Left = interiorPosX,
                Top = consoleY,
                Width = interiorWidth,
                UseColors = true,
                ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Banner),
                OneLine = true,
                Text = text,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            builder.Append(nameText.Render());

            // Write the mode text
            int modePosY = ConsoleWrapper.WindowHeight / 2 + figHeight - 1;
            var modeTextRenderer = new AlignedText()
            {
                Left = interiorPosX,
                Top = modePosY,
                Width = interiorWidth,
                UseColors = true,
                OneLine = true,
                Text = modeText,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            builder.Append(modeTextRenderer.Render());
            return builder.ToString();
        }

        public override string Closing(SplashContext context, out bool delayRequired)
        {
            var builder = new StringBuilder();
            cleared = false;
            builder.Append(
                base.Opening(context)
            );
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing...");

            // Check if delay is required
            delayRequired =
                context == SplashContext.ShuttingDown && Config.MainConfig.DelayOnShutdown ||
                context == SplashContext.StartingUp;
            if (!delayRequired)
                return builder.ToString();

            // Write a glorious Welcome screen
            Color col = ThemeColorsTools.GetColor(ThemeColorType.Stage);
            string text =
                context == SplashContext.StartingUp ?
                LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME") :
                LanguageTools.GetLocalized("NKS_KERNEL_STARTING_GOODBYE");
            var figFont = FigletTools.GetFigletFont("thin");
            int figHeight = FigletTools.GetFigletHeight(text, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight - 2;
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
            builder.Append(figText.Render());

            // Write the version
            int versionPosY = ConsoleWrapper.WindowHeight / 2 + figHeight - 1;
            var versionTextRenderer = new AlignedText()
            {
                Top = versionPosY,
                UseColors = true,
                OneLine = true,
                Text = $"Nitrocid KS {KernelReleaseInfo.VersionFullStr} ({KernelReleaseInfo.ApiVersion})",
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            builder.Append(versionTextRenderer.Render());

            // Check if delay is required
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

            // Get infobox position info
            int height = ConsoleWrapper.WindowHeight - 4;
            int width = ConsoleWrapper.WindowWidth - 8;
            int posX = ConsoleWrapper.WindowWidth / 2 - width / 2 - 1;
            int posY = ConsoleWrapper.WindowHeight / 2 + height / 2 - 3;

            // Write the progress report
            int interiorPosX = posX + 4;
            int progressReportPosY = posY;
            var modeTextEraser = new Eraser()
            {
                Left = posX + 1,
                Top = progressReportPosY,
                Width = width - 2,
                Height = 1,
            };
            var modeTextRenderer = new AlignedText()
            {
                Left = interiorPosX,
                Top = progressReportPosY,
                Width = width - 2,
                UseColors = true,
                OneLine = true,
                Text = ProgressReport.FormatString(Vars),
                ForegroundColor = col,
            };
            builder.Append(
                modeTextEraser.Render() +
                modeTextRenderer.Render()
            );

            // Write the progress bar
            int progressBarPosY = posY + 1;
            progress.Position = Progress;
            progress.Width = width - 6;
            builder.Append(
                RendererTools.RenderRenderable(progress, new(interiorPosX, progressBarPosY))
            );
            return builder.ToString();
        }

    }
}
