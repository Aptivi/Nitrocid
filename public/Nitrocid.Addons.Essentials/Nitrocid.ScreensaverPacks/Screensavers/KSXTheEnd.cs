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

using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using System;
using Textify.Data.Figlet;
using Nitrocid.ScreensaverPacks.Animations.Glitch;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for KSXTheEnd
    /// </summary>
    public class KSXTheEndDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "KSXTheEnd";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages =>
            true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBackDry(new Color(0, 0, 0));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int step;
            int maxSteps = 6;
            Color green = new(0, 255, 0);
            Color red = new(255, 0, 0);
            Color pink = new(255, 0, 255);
            Color blue = new(0, 0, 255);
            Color black = new(0, 0, 0);
            Color white = new(255, 255, 255);
            var font = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
            int colorSteps = 30;
            int currentR = 0;
            int currentG = 0;
            int currentB = 0;
            int height = ConsoleWrapper.WindowHeight - 2;

            // Start stepping
            for (step = 1; step <= maxSteps; step++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;

                switch (step)
                {
                    // Step 1: Shows all the versions, but with 0.1.0.
                    case 1:
                        ColorTools.LoadBackDry(black);

                        // Get the color thresholds
                        double thresholdGR = green.RGB.R / (double)colorSteps;
                        double thresholdGG = green.RGB.G / (double)colorSteps;
                        double thresholdGB = green.RGB.B / (double)colorSteps;
                        double thresholdRR = red.RGB.R / (double)colorSteps;
                        double thresholdRG = red.RGB.G / (double)colorSteps;
                        double thresholdRB = red.RGB.B / (double)colorSteps;
                        double thresholdPR = pink.RGB.R / (double)colorSteps;
                        double thresholdPG = pink.RGB.G / (double)colorSteps;
                        double thresholdPB = pink.RGB.B / (double)colorSteps;
                        double thresholdBR = blue.RGB.R / (double)colorSteps;
                        double thresholdBG = blue.RGB.G / (double)colorSteps;
                        double thresholdBB = blue.RGB.B / (double)colorSteps;

                        // Now, transition
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR + thresholdGR);
                            currentG = (int)Math.Round(currentG + thresholdGG);
                            currentB = (int)Math.Round(currentB + thresholdGB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            int figHeight = FigletTools.GetFigletHeight("v0.0.1", font) / 2;
                            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                            var xText = new AlignedFigletText(font)
                            {
                                Text = "v0.0.1",
                                Top = consoleY,
                                ForegroundColor = col,
                                BackgroundColor = black,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            var dateText = new AlignedText()
                            {
                                Text = TimeDateRenderers.RenderDate(new DateTime(2018, 2, 22)),
                                Top = height,
                                ForegroundColor = col,
                                BackgroundColor = black,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            TextWriterRaw.WriteRaw(
                                xText.Render() +
                                dateText.Render()
                            );

                            // Sleep
                            ScreensaverManager.Delay(100, true);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR - thresholdGR);
                            currentG = (int)Math.Round(currentG - thresholdGG);
                            currentB = (int)Math.Round(currentB - thresholdGB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            int figHeight = FigletTools.GetFigletHeight("v0.0.1", font) / 2;
                            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                            var xText = new AlignedFigletText(font)
                            {
                                Text = "v0.0.1",
                                Top = consoleY,
                                ForegroundColor = col,
                                BackgroundColor = black,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            var dateText = new AlignedText()
                            {
                                Text = TimeDateRenderers.RenderDate(new DateTime(2018, 2, 22)),
                                Top = height,
                                ForegroundColor = col,
                                BackgroundColor = black,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            TextWriterRaw.WriteRaw(
                                xText.Render() +
                                dateText.Render()
                            );

                            // Sleep
                            ScreensaverManager.Delay(100, true);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR + thresholdRR);
                            currentG = (int)Math.Round(currentG + thresholdRG);
                            currentB = (int)Math.Round(currentB + thresholdRB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            int figHeight = FigletTools.GetFigletHeight("v0.0.16", font) / 2;
                            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                            var xText = new AlignedFigletText(font)
                            {
                                Text = "v0.0.16",
                                Top = consoleY,
                                ForegroundColor = col,
                                BackgroundColor = black,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            var dateText = new AlignedText()
                            {
                                Text = TimeDateRenderers.RenderDate(new DateTime(2021, 6, 12)),
                                Top = height,
                                ForegroundColor = col,
                                BackgroundColor = black,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            TextWriterRaw.WriteRaw(
                                xText.Render() +
                                dateText.Render()
                            );

                            // Sleep
                            ScreensaverManager.Delay(100, true);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR - thresholdRR);
                            currentG = (int)Math.Round(currentG - thresholdRG);
                            currentB = (int)Math.Round(currentB - thresholdRB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            int figHeight = FigletTools.GetFigletHeight("v0.0.16", font) / 2;
                            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                            var xText = new AlignedFigletText(font)
                            {
                                Text = "v0.0.16",
                                Top = consoleY,
                                ForegroundColor = col,
                                BackgroundColor = black,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            var dateText = new AlignedText()
                            {
                                Text = TimeDateRenderers.RenderDate(new DateTime(2021, 6, 12)),
                                Top = height,
                                ForegroundColor = col,
                                BackgroundColor = black,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            TextWriterRaw.WriteRaw(
                                xText.Render() +
                                dateText.Render()
                            );

                            // Sleep
                            ScreensaverManager.Delay(100, true);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR + thresholdPR);
                            currentG = (int)Math.Round(currentG + thresholdPG);
                            currentB = (int)Math.Round(currentB + thresholdPB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            int figHeight = FigletTools.GetFigletHeight("v0.0.24", font) / 2;
                            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                            var xText = new AlignedFigletText(font)
                            {
                                Text = "v0.0.24",
                                Top = consoleY,
                                ForegroundColor = col,
                                BackgroundColor = black,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            var dateText = new AlignedText()
                            {
                                Text = TimeDateRenderers.RenderDate(new DateTime(2022, 8, 2)),
                                Top = height,
                                ForegroundColor = col,
                                BackgroundColor = black,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            TextWriterRaw.WriteRaw(
                                xText.Render() +
                                dateText.Render()
                            );

                            // Sleep
                            ScreensaverManager.Delay(100, true);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR - thresholdPR);
                            currentG = (int)Math.Round(currentG - thresholdPG);
                            currentB = (int)Math.Round(currentB - thresholdPB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            int figHeight = FigletTools.GetFigletHeight("v0.0.24", font) / 2;
                            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                            var xText = new AlignedFigletText(font)
                            {
                                Text = "v0.0.24",
                                Top = consoleY,
                                ForegroundColor = col,
                                BackgroundColor = black,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            var dateText = new AlignedText()
                            {
                                Text = TimeDateRenderers.RenderDate(new DateTime(2022, 8, 2)),
                                Top = height,
                                ForegroundColor = col,
                                BackgroundColor = black,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            TextWriterRaw.WriteRaw(
                                xText.Render() +
                                dateText.Render()
                            );

                            // Sleep
                            ScreensaverManager.Delay(100, true);
                        }
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR + thresholdBR);
                            currentG = (int)Math.Round(currentG + thresholdBG);
                            currentB = (int)Math.Round(currentB + thresholdBB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            int figHeight = FigletTools.GetFigletHeight("v0.1.0", font) / 2;
                            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                            var xText = new AlignedFigletText(font)
                            {
                                Text = "v0.1.0",
                                Top = consoleY,
                                ForegroundColor = col,
                                BackgroundColor = black,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            var dateText = new AlignedText()
                            {
                                Text = TimeDateRenderers.RenderDate(new DateTime(2024, 3, 11)),
                                Top = height,
                                ForegroundColor = col,
                                BackgroundColor = black,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            TextWriterRaw.WriteRaw(
                                xText.Render() +
                                dateText.Render()
                            );

                            // Sleep
                            ScreensaverManager.Delay(100, true);
                        }
                        break;
                    // Step 2: 0.1.0 doesn't fade out, but a glitch shows
                    case 2:
                        for (int delayed = 0; delayed < 5000; delayed += 10)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            ScreensaverManager.Delay(10, true);
                            Glitch.GlitchAt();
                        }
                        break;
                    // Step 3: Flickering colors between white and each version's color
                    case 3:
                        int maxFlashes = 200;
                        for (int flashes = 0; flashes <= maxFlashes; flashes++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            bool showGreen = flashes % 2 == 0;
                            if (showGreen)
                                ColorTools.LoadBackDry(green);
                            else
                                ColorTools.LoadBackDry(white);
                            ScreensaverManager.Delay(50, true);
                        }
                        for (int flashes = 0; flashes <= maxFlashes; flashes++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            bool showRed = flashes % 2 == 0;
                            if (showRed)
                                ColorTools.LoadBackDry(red);
                            else
                                ColorTools.LoadBackDry(white);
                            ScreensaverManager.Delay(50, true);
                        }
                        for (int flashes = 0; flashes <= maxFlashes; flashes++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            bool showPink = flashes % 2 == 0;
                            if (showPink)
                                ColorTools.LoadBackDry(pink);
                            else
                                ColorTools.LoadBackDry(white);
                            ScreensaverManager.Delay(50, true);
                        }
                        for (int flashes = 0; flashes <= maxFlashes; flashes++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            bool showBlue = flashes % 2 == 0;
                            if (showBlue)
                                ColorTools.LoadBackDry(blue);
                            else
                                ColorTools.LoadBackDry(white);
                            ScreensaverManager.Delay(50, true);
                        }
                        break;
                    // Step 4: White background for a few seconds
                    case 4:
                        ColorTools.LoadBackDry(white);
                        ScreensaverManager.Delay(5000, true);
                        break;
                    // Step 5: "THE END" shows for a few seconds
                    case 5:
                        {
                            string theEnd = LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_KSX_THEEND").ToUpper();
                            int figHeight = FigletTools.GetFigletHeight(theEnd, font) / 2;
                            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                            var endText = new AlignedFigletText(font)
                            {
                                Text = theEnd,
                                Top = consoleY,
                                ForegroundColor = green,
                                BackgroundColor = white,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            TextWriterRaw.WriteRaw(endText.Render());
                        }
                        ScreensaverManager.Delay(5000, true);
                        break;
                    // Step 6: With the figlet text, the background fades out
                    case 6:
                        // Get the color thresholds
                        double thresholdR = green.RGB.R / (double)colorSteps;
                        double thresholdG = green.RGB.G / (double)colorSteps;
                        double thresholdB = green.RGB.B / (double)colorSteps;
                        double thresholdBGR = white.RGB.R / (double)colorSteps;
                        double thresholdBGG = white.RGB.G / (double)colorSteps;
                        double thresholdBGB = white.RGB.B / (double)colorSteps;
                        currentR = green.RGB.R;
                        currentG = green.RGB.G;
                        currentB = green.RGB.B;
                        int currentBGR = white.RGB.R;
                        int currentBGG = white.RGB.G;
                        int currentBGB = white.RGB.B;
                        for (int currentStep = 1; currentStep <= colorSteps; currentStep++)
                        {
                            if (ConsoleResizeHandler.WasResized(false))
                                break;
                            if (ScreensaverManager.Bailing)
                                return;

                            // Remove the values according to the threshold
                            currentR = (int)Math.Round(currentR - thresholdR);
                            currentG = (int)Math.Round(currentG - thresholdG);
                            currentB = (int)Math.Round(currentB - thresholdB);
                            currentBGR = (int)Math.Round(currentBGR - thresholdBGR);
                            currentBGG = (int)Math.Round(currentBGG - thresholdBGG);
                            currentBGB = (int)Math.Round(currentBGB - thresholdBGB);

                            // Now, make a color and fill the console with it
                            Color col = new(currentR, currentG, currentB);
                            Color colBG = new(currentBGR, currentBGG, currentBGB);
                            ColorTools.LoadBackDry(colBG);
                            string theEnd = LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_KSX_THEEND").ToUpper();
                            int figHeight = FigletTools.GetFigletHeight(theEnd, font) / 2;
                            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                            var endText = new AlignedFigletText(font)
                            {
                                Text = theEnd,
                                Top = consoleY,
                                ForegroundColor = col,
                                BackgroundColor = colBG,
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle,
                                }
                            };
                            TextWriterRaw.WriteRaw(endText.Render());

                            // Sleep
                            ScreensaverManager.Delay(100, true);
                        }
                        break;
                }
            }

            // Reset
            ColorTools.LoadBackDry(black);
        }

    }
}
