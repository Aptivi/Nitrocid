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
using System.Collections.Generic;
using System.Text;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Misc.Splash;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Themes.Colors;
using Terminaux.Colors.Transformation;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Textify.General;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashLogbox : BaseSplash, ISplash
    {
        private List<string> progresses = [];
        private bool cleared = false;
        private SimpleProgress progress = new(0, 100);

        // Standalone splash information
        public override string SplashName => "Logbox";

        // Actual logic
        public override string Opening(SplashContext context)
        {
            var builder = new StringBuilder();
            var progressColor = ThemeColorsTools.GetColor(ThemeColorType.Progress);
            progress.Position = 0;
            progress.Indeterminate = !SplashPackInit.SplashConfig.LogboxShowProgress;
            progress.ProgressForegroundColor = TransformationTools.GetDarkBackground(progressColor);
            progress.ProgressActiveForegroundColor = progressColor;
            progress.ProgressBackgroundColor = ConsoleColoring.CurrentBackgroundColor;
            if (ConsoleResizeHandler.WasResized(true))
                cleared = false;
            if (!cleared)
            {
                cleared = true;
                builder.Append(
                    base.Opening(context)
                );
            }

            // Write an infobox border
            int height = ConsoleWrapper.WindowHeight - 4;
            int width = ConsoleWrapper.WindowWidth - 8;
            int posX = ConsoleWrapper.WindowWidth / 2 - width / 2 - 1;
            int posY = ConsoleWrapper.WindowHeight / 2 - height / 2 - 1;
            string versionStr = $"Nitrocid KS {KernelReleaseInfo.VersionFullStr} ({KernelReleaseInfo.ApiVersion})";
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
            return builder.ToString();
        }

        public override string Display(SplashContext context)
        {
            var builder = new StringBuilder();

            // Write a border for reported progresses
            int height = ConsoleWrapper.WindowHeight - 4;
            int width = ConsoleWrapper.WindowWidth - 8;
            int posX = ConsoleWrapper.WindowWidth / 2 - width / 2 - 1;
            int posY = ConsoleWrapper.WindowHeight / 2 - height / 2 - 1;
            int reportedProgressesHeight = height - 6;
            int reportedProgressesWidth = width - 8;
            int reportedProgressesPosX = posX + 4;
            int reportedProgressesPosY = posY + 2;
            var border = new BoxFrame()
            {
                Left = reportedProgressesPosX,
                Top = reportedProgressesPosY,
                Width = reportedProgressesWidth,
                Height = reportedProgressesHeight,
                UseColors = true,
            };
            builder.Append(border.Render());

            // Erase the list
            var listEraser = new Eraser()
            {
                Left = reportedProgressesPosX + 1,
                Top = reportedProgressesPosY + 1,
                Width = reportedProgressesWidth,
                Height = reportedProgressesHeight,
            };
            builder.Append(listEraser.Render());

            // Write a list of reported progresses
            int processedProgresses = 0;
            for (int i = progresses.Count - 1; i >= 0 && processedProgresses < reportedProgressesHeight; i--)
            {
                string prog = progresses[i].Truncate(reportedProgressesWidth);
                int progPosY = reportedProgressesPosY + reportedProgressesHeight - processedProgresses;
                builder.Append(ConsolePositioning.RenderChangePosition(reportedProgressesPosX + 1, progPosY));
                builder.Append(prog);
                processedProgresses++;
            }

            // Return the result
            return builder.ToString();
        }

        public override string Closing(SplashContext context, out bool delayRequired)
        {
            cleared = false;
            delayRequired = false;
            if (context == SplashContext.ShuttingDown || context == SplashContext.Rebooting)
                progresses.Clear();
            return "";
        }

        public override string Report(int Progress, string ProgressReport, params object[] Vars) =>
            ReportProgress(Progress, "[I] " + ProgressReport, ThemeColorType.Stage, Vars);

        public override string ReportWarning(int Progress, string WarningReport, Exception? ExceptionInfo, params object[] Vars) =>
            ReportProgress(Progress, "[W] " + WarningReport, ThemeColorType.Warning, Vars);

        public override string ReportError(int Progress, string ErrorReport, Exception? ExceptionInfo, params object[] Vars) =>
            ReportProgress(Progress, "[E] " + ErrorReport, ThemeColorType.Error, Vars);

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

            // Write the progress bar
            int progressBarPosY = posY + 1;
            progress.Position = Progress;
            progress.Width = width - 6;
            builder.Append(
                RendererTools.RenderRenderable(progress, new(interiorPosX, progressBarPosY))
            );

            // Return the resulting string
            string reportedProgress = ConsoleColoring.RenderSetConsoleColor(col) + ProgressReport.FormatString(Vars);
            if ((progresses.Count > 0 && progresses[^1] != reportedProgress) || progresses.Count == 0)
                progresses.Add(reportedProgress);
            return builder.ToString();
        }

    }
}
