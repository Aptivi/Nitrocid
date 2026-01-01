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

using System.Threading;
using Terminaux.Colors;
using Terminaux.Sequences;
using Textify.Data.Figlet;
using System;
using System.Text;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Splash;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Languages;
using Nitrocid.Kernel;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Nitrocid.Kernel.Configuration;
using Terminaux.Colors.Transformation.Contrast;
using Terminaux.Base.Extensions;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashFigProgress : BaseSplash, ISplash
    {

        private int dotStep = 0;

        // Standalone splash information
        public override string SplashName => "FigProgress";

        // Actual logic
        public override string Opening(SplashContext context)
        {
            var builder = new StringBuilder();
            builder.Append(
                base.Opening(context)
            );

            // Write a glorious Welcome screen
            Color col = KernelColorTools.GetColor(KernelColorType.Stage);
            string text = $"{SplashReport.Progress}%";
            var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
            int figWidth = FigletTools.GetFigletWidth(text, figFont) / 2;
            int figHeight = FigletTools.GetFigletHeight(text, figFont) / 2;
            int consoleX, consoleY;
            if (figWidth >= ConsoleWrapper.WindowWidth || figHeight >= ConsoleWrapper.WindowHeight)
            {
                // The figlet won't fit, so use small text
                consoleX = ConsoleWrapper.WindowWidth / 2 - text.Length / 2;
                consoleY = ConsoleWrapper.WindowHeight / 2;
                builder.Append(
                    col.VTSequenceForeground +
                    TextWriterWhereColor.RenderWhere(text, consoleX, consoleY, true)
                );
            }
            else
            {
                // Write the figlet.
                consoleX = ConsoleWrapper.WindowWidth / 2 - figWidth;
                consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                var figletText = new FigletText(figFont)
                {
                    Text = text,
                    ForegroundColor = col,
                };
                builder.Append(
                    ContainerTools.RenderRenderable(figletText, new(consoleX, consoleY))
                );
                consoleY += figHeight * 2;
            }

            // Render the kernel version
            var versionText = new AlignedText()
            {
                Text =
                    context == SplashContext.Preboot ? Translate.DoTranslation("Please wait while the kernel is initializing...") :
                    context == SplashContext.ShuttingDown ? Translate.DoTranslation("Please wait while the kernel is shutting down...") :
                    $"{Translate.DoTranslation("Starting")} {KernelReleaseInfo.ConsoleTitle}...",
                Top = consoleY + 2,
                ForegroundColor = col,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle
                }
            };
            builder.Append(
                versionText.Render()
            );
            return builder.ToString();
        }

        public override string Display(SplashContext context)
        {
            var builder = new StringBuilder();
            try
            {
                Color firstColor = KernelColorTools.GetColor(KernelColorType.Background).Brightness == ColorBrightness.Light ? new(ConsoleColors.Black) : new(ConsoleColors.White);
                Color secondColor = KernelColorTools.GetColor(KernelColorType.Success);
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                Color firstDotColor = dotStep >= 1 ? secondColor : firstColor;
                Color secondDotColor = dotStep >= 2 ? secondColor : firstColor;
                Color thirdDotColor = dotStep >= 3 ? secondColor : firstColor;
                Color fourthDotColor = dotStep >= 4 ? secondColor : firstColor;
                Color fifthDotColor = dotStep >= 5 ? secondColor : firstColor;

                // Write the three dots
                string dots =
                    $"{firstDotColor.VTSequenceForeground}* " +
                    $"{secondDotColor.VTSequenceForeground}* " +
                    $"{thirdDotColor.VTSequenceForeground}* " +
                    $"{fourthDotColor.VTSequenceForeground}* " +
                    $"{fifthDotColor.VTSequenceForeground}*";
                int dotsPosX = (ConsoleWrapper.WindowWidth / 2) - (VtSequenceTools.FilterVTSequences(dots).Length / 2);
                int dotsPosY = ConsoleWrapper.WindowHeight - 2;
                builder.Append(TextWriterWhereColor.RenderWhere(dots, dotsPosX, dotsPosY));
                dotStep++;
                if (dotStep > 5)
                    dotStep = 0;
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
            return builder.ToString();
        }

        public override string Closing(SplashContext context, out bool delayRequired)
        {
            var builder = new StringBuilder();
            builder.Append(
                base.Opening(context)
            );
            DebugWriter.WriteDebug(DebugLevel.I, "Splash closing...");

            if (context == SplashContext.Showcase || context == SplashContext.Preboot)
            {
                delayRequired = false;
                return builder.ToString();
            }

            // Write a glorious Welcome screen
            Color col = KernelColorTools.GetColor(KernelColorType.Stage);
            string text =
                (context == SplashContext.StartingUp ?
                 Translate.DoTranslation("Welcome!") :
                 Translate.DoTranslation("Goodbye!"))
                .ToUpper();
            var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
            var figFontFallback = FigletTools.GetFigletFont("small");
            int figWidth = FigletTools.GetFigletWidth(text, figFont) / 2;
            int figHeight = FigletTools.GetFigletHeight(text, figFont) / 2;
            int figWidthFallback = FigletTools.GetFigletWidth(text, figFontFallback) / 2;
            int figHeightFallback = FigletTools.GetFigletHeight(text, figFontFallback) / 2;
            int width = ConsoleWrapper.WindowWidth;
            int height = ConsoleWrapper.WindowHeight;
            int consoleX = (width / 2) - figWidth;
            int consoleY = (height / 2) - figHeight;
            var figletText = new FigletText(figFont)
            {
                Text = text,
                ForegroundColor = col,
            };
            if (consoleX < 0 || consoleY < 0)
            {
                // The figlet won't fit, so use small text
                consoleX = (width / 2) - figWidthFallback;
                consoleY = (height / 2) - figHeightFallback;
                if (consoleX < 0 || consoleY < 0)
                {
                    // The fallback figlet also won't fit, so use smaller text
                    consoleX = (width / 2) - (text.Length / 2);
                    consoleY = height / 2;
                    builder.Append(
                        TextWriterWhereColor.RenderWhere(text, consoleX, consoleY, true)
                    );
                }
                else
                {
                    // Write the figlet.
                    figletText.Font = figFontFallback;
                    builder.Append(
                        ContainerTools.RenderRenderable(figletText, new(consoleX, consoleY))
                    );
                    consoleY += figHeightFallback * 2;
                }
            }
            else
            {
                // Write the figlet.
                builder.Append(
                    ContainerTools.RenderRenderable(figletText, new(consoleX, consoleY))
                );
                consoleY += figHeight * 2;
            }

            // Render the kernel version
            var versionText = new AlignedText()
            {
                Text = KernelReleaseInfo.ConsoleTitle,
                Top = consoleY + 2,
                ForegroundColor = col,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle
                }
            };
            builder.Append(
                versionText.Render()
            );
            delayRequired = true;
            return builder.ToString();
        }

        public override string Report(int Progress, string ProgressReport, params object[] Vars) =>
            ReportProgress(Progress, ProgressReport, KernelColorType.Stage, Vars);

        public override string ReportWarning(int Progress, string WarningReport, Exception? ExceptionInfo, params object[] Vars) =>
            ReportProgress(Progress, WarningReport, KernelColorType.Warning, Vars);

        public override string ReportError(int Progress, string ErrorReport, Exception? ExceptionInfo, params object[] Vars) =>
            ReportProgress(Progress, ErrorReport, KernelColorType.Error, Vars);

        private string ReportProgress(int Progress, string ProgressReport, KernelColorType colorType, params object[] Vars)
        {
            var builder = new StringBuilder();
            Color col = KernelColorTools.GetColor(colorType);
            string percentage = $"{Progress}%";
            var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
            int figHeight = FigletTools.GetFigletHeight(percentage, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            for (int i = consoleY; i <= consoleY + figHeight; i++)
                builder.Append(TextWriterWhereColor.RenderWhere(ConsoleClearing.GetClearLineToRightSequence(), 0, i, true));
            var percentageFiglet = new AlignedFigletText(figFont, percentage)
            {
                Top = consoleY,
                ForegroundColor = col,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle
                }
            };
            var progressText = new AlignedText()
            {
                Text = ProgressReport.FormatString(Vars),
                Top = consoleY - 2,
                ForegroundColor = col,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle
                }
            };
            builder.Append(
                percentageFiglet.Render() +
                TextWriterWhereColor.RenderWhere(ConsoleClearing.GetClearLineToRightSequence(), 0, consoleY - 2, true) +
                progressText.Render()
            );
            return builder.ToString();
        }

    }
}
