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
using System.Text;
using System.Threading;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Languages;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Transformation;
using Terminaux.Inputs;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Graphical.Shapes;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    internal static class Simon
    {

        internal static void InitializeSimon()
        {
            int colorPresses = 1;
            int currentLevel = 0;
            bool done = false;
            int currentColorNum = 0;
            var simonScreen = new Screen();
            var simonScreenPart = new ScreenPart();
            var simonScreenPartHighlight = new ScreenPart();

            // Helper function for variables
            static (int angleStart, int angleEnd, Color color, string colorString) GetArcInfo(int colorNumber)
            {
                (int angleStart, int angleEnd) =
                    colorNumber == 1 ? (90, 180) :
                    colorNumber == 2 ? (360, 90) :
                    colorNumber == 3 ? (270, 360) :
                    (180, 270);
                (Color color, string colorString) =
                    colorNumber == 1 ? (ConsoleColors.Lime, LanguageTools.GetLocalized("NKS_AMUSEMENTS_SIMON_SIMONSAYS_GREEN")) :
                    colorNumber == 2 ? (ConsoleColors.Red, LanguageTools.GetLocalized("NKS_AMUSEMENTS_SIMON_SIMONSAYS_RED")) :
                    colorNumber == 3 ? (ConsoleColors.Blue, LanguageTools.GetLocalized("NKS_AMUSEMENTS_SIMON_SIMONSAYS_BLUE")) :
                    (ConsoleColors.Yellow, LanguageTools.GetLocalized("NKS_AMUSEMENTS_SIMON_SIMONSAYS_YELLOW"));
                return (angleStart, angleEnd, color, colorString);
            }
            static Arc GetSimonArc(int colorNumber, bool lightColor = false)
            {
                (int angleStart, int angleEnd, Color color, _) = GetArcInfo(colorNumber);
                int width = ConsoleWrapper.WindowWidth / 2;
                int height = ConsoleWrapper.WindowHeight - 6;
                int panelPosX = width - height;
                int panelPosY = 1;
                var highlightBuffer = new StringBuilder();
                var arcStroke = new Arc(height, panelPosX, panelPosY, lightColor ? TransformationTools.Lighten(color, 30) : color)
                {
                    AngleStart = angleStart,
                    AngleEnd = angleEnd,
                    OuterRadius = height / 2,
                    InnerRadius = height / 2 - 4,
                };
                return arcStroke;
            }

            // Make the screen part for the game
            simonScreenPart.AddDynamicText(() =>
            {
                // Make an invisible panel positions for Simon
                var gameBuffer = new StringBuilder();

                // Print the Simon arcs starting from Green (1) to Yellow (4)
                var arcStrokeGreen = GetSimonArc(1);
                gameBuffer.Append(arcStrokeGreen.Render());
                var arcStrokeRed = GetSimonArc(2);
                gameBuffer.Append(arcStrokeRed.Render());
                var arcStrokeBlue = GetSimonArc(3);
                gameBuffer.Append(arcStrokeBlue.Render());
                var arcStrokeYellow = GetSimonArc(4);
                gameBuffer.Append(arcStrokeYellow.Render());

                // Print the instructions here
                int instructionsPosY = ConsoleWrapper.WindowHeight - 2;
                var message = new AlignedText()
                {
                    Left = 0,
                    Top = instructionsPosY,
                    Width = ConsoleWrapper.WindowWidth,
                    Height = 1,
                    OneLine = true,
                    Text = "\x1b[1K" +
                        (done ? LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMON_GAMEOVER") :
                         LanguageTools.GetLocalized("NKS_AMUSEMENTS_SIMON_PENDING")) + "\x1b[K",
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    },
                    ForegroundColor =
                        done ? ConsoleColors.Red :
                        ConsoleColors.White,
                };
                gameBuffer.Append(message.Render());
                return gameBuffer.ToString();
            });
            simonScreen.AddBufferedPart("Simon", simonScreenPart);
            simonScreen.OverlayPart = simonScreenPartHighlight;
            ScreenTools.SetCurrent(simonScreen);

            // Main loop
            int[] expectedPatterns = [];
            while (!done)
            {
                // Make an array of expected patterns based on the number of required presses
                if (currentLevel != colorPresses)
                {
                    expectedPatterns = new int[colorPresses];
                    for (int i = 0; i < colorPresses; i++)
                    {
                        // Select one of 1 (Green), 2 (Red), 3 (Blue), 4 (Yellow)
                        expectedPatterns[i] = RandomDriver.Random(1, 4);
                        (int angleStart, int angleEnd, Color color, string colorString) = GetArcInfo(expectedPatterns[i]);

                        simonScreenPartHighlight.AddDynamicText(() =>
                        {
                            // Highlight the chosen color
                            var highlightBuffer = new StringBuilder();
                            var arcStroke = GetSimonArc(expectedPatterns[i], true);
                            highlightBuffer.Append(arcStroke.Render());

                            // Write a chosen color
                            int instructionsPosY = ConsoleWrapper.WindowHeight - 2;
                            var message = new AlignedText()
                            {
                                Left = 0,
                                Top = instructionsPosY,
                                Width = ConsoleWrapper.WindowWidth,
                                Height = 1,
                                OneLine = true,
                                Text = "\x1b[1K" + LanguageTools.GetLocalized("NKS_AMUSEMENTS_SIMON_SIMONSAYS").FormatString(colorString) + "\x1b[K",
                                Settings = new()
                                {
                                    Alignment = TextAlignment.Middle
                                },
                                ForegroundColor = color,
                            };
                            highlightBuffer.Append(message.Render());
                            return highlightBuffer.ToString();
                        });
                        ScreenTools.Render();
                        Thread.Sleep(500);
                        simonScreenPartHighlight.Clear();
                        ScreenTools.Render();
                    }
                    currentLevel = colorPresses;
                }

                // Let the user choose the colors
                ScreenTools.Render();
                simonScreenPartHighlight.Clear();
                var pressedChar = Input.ReadKey();
                var expectedColor = expectedPatterns[currentColorNum];
                switch (pressedChar.Key)
                {
                    case ConsoleKey.Escape:
                        // User decided to escape
                        done = true;
                        break;
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            // Green (upper left)
                            simonScreenPartHighlight.AddDynamicText(() =>
                            {
                                var arcStroke = GetSimonArc(1, true);
                                return arcStroke.Render();
                            });
                            if (expectedColor != 1)
                                done = true;
                            else
                                currentColorNum++;
                            if (currentColorNum >= expectedPatterns.Length)
                            {
                                currentColorNum = 0;
                                colorPresses++;
                            }
                            ScreenTools.Render();
                            if (done)
                                Thread.Sleep(3000);
                            simonScreenPartHighlight.Clear();
                        }
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            // Red (upper right)
                            simonScreenPartHighlight.AddDynamicText(() =>
                            {
                                var arcStroke = GetSimonArc(2, true);
                                return arcStroke.Render();
                            });
                            if (expectedColor != 2)
                                done = true;
                            else
                                currentColorNum++;
                            if (currentColorNum >= expectedPatterns.Length)
                            {
                                currentColorNum = 0;
                                colorPresses++;
                            }
                            ScreenTools.Render();
                            if (done)
                                Thread.Sleep(3000);
                            simonScreenPartHighlight.Clear();
                        }
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        {
                            // Blue (lower right)
                            simonScreenPartHighlight.AddDynamicText(() =>
                            {
                                var arcStroke = GetSimonArc(3, true);
                                return arcStroke.Render();
                            });
                            if (expectedColor != 3)
                                done = true;
                            else
                                currentColorNum++;
                            if (currentColorNum >= expectedPatterns.Length)
                            {
                                currentColorNum = 0;
                                colorPresses++;
                            }
                            ScreenTools.Render();
                            if (done)
                                Thread.Sleep(3000);
                            simonScreenPartHighlight.Clear();
                        }
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        {
                            // Yellow (lower left)
                            simonScreenPartHighlight.AddDynamicText(() =>
                            {
                                var arcStroke = GetSimonArc(4, true);
                                return arcStroke.Render();
                            });
                            if (expectedColor != 4)
                                done = true;
                            else
                                currentColorNum++;
                            if (currentColorNum >= expectedPatterns.Length)
                            {
                                currentColorNum = 0;
                                colorPresses++;
                            }
                            ScreenTools.Render();
                            if (done)
                                Thread.Sleep(3000);
                            simonScreenPartHighlight.Clear();
                        }
                        break;
                }
            }

            // Clean up
            ScreenTools.UnsetCurrent(simonScreen);
            ConsoleWrapper.Clear();
        }
    }
}
