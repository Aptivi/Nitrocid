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
using System.Threading;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Languages;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Gradients;
using Terminaux.Inputs;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Graphical.Rulers;
using Terminaux.Writer.CyclicWriters.Graphical.Shapes;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.Data.Words;
using Textify.General;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    internal static class StreetRun
    {

        internal static void InitializeStreetRun()
        {
            bool crashed = false;
            int displayDelay = 500;
            int carSpeed = 0;
            int carLane = 2;
            string[] trafficTypes = ["🚗", "🚌", "🛻", "🚚", "🚛", "🚒", "🚕", "🚐"];
            List<(int trafficLane, double position, double speed, string type)> traffic = [];
            var streetRunScreen = new Screen();
            var streetRunScreenPart = new ScreenPart();

            // Make the screen part for the game
            streetRunScreenPart.AddDynamicText(() =>
            {
                // Make a panel for the StreetRun game
                int width = 11;
                int height = ConsoleWrapper.WindowHeight - 6;
                int panelPosX = ConsoleWrapper.WindowWidth / 2 - width / 2 - 1;
                int panelPosY = 1;
                int secondLaneX = panelPosX + 3;
                int thirdLaneX = panelPosX + 7;
                var eraser = new Eraser()
                {
                    Left = panelPosX,
                    Top = panelPosY,
                    Width = width,
                    Height = height + 1,
                };
                var boxFrame = new BoxFrame()
                {
                    Left = panelPosX,
                    Top = panelPosY,
                    Width = width,
                    Height = height,
                    Rulers =
                    [
                        new(secondLaneX - panelPosX, RulerOrientation.Vertical),
                        new(thirdLaneX - panelPosX, RulerOrientation.Vertical),
                    ]
                };
                var gameBuffer = new StringBuilder();
                gameBuffer.Append(eraser.Render());
                gameBuffer.Append(boxFrame.Render());

                // Print the car speed here
                int instructionsPosY = ConsoleWrapper.WindowHeight - 2;
                var message = new AlignedText()
                {
                    Left = 0,
                    Top = instructionsPosY,
                    Width = ConsoleWrapper.WindowWidth,
                    Height = 1,
                    OneLine = true,
                    Text = "\x1b[1K" + $"{carSpeed} ms" + "\x1b[K",
                    ForegroundColor = ColorGradients.StageLevelSmooth(ConsoleColors.Lime, ConsoleColors.Yellow, ConsoleColors.Red, carSpeed, 0, 450, false, 175, 350),
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    },
                };
                gameBuffer.Append(message.Render());

                // Render our car
                int currentCarPosX = carLane == 1 ? panelPosX : carLane == 2 ? secondLaneX + 1 : thirdLaneX + 1;
                gameBuffer.Append(
                    ConsolePositioning.RenderChangePosition(currentCarPosX + 1, panelPosY + height) +
                    (crashed ? "❌" : "🚘")
                );

                if (!crashed)
                {
                    // Increase car speed
                    if (carSpeed < 450)
                        carSpeed += 1;
                    streetRunScreen.CycleFrequency = displayDelay - carSpeed;

                    // Render existing traffic
                    for (int i = traffic.Count - 1; i >= 0; i--)
                    {
                        (int trafficLane, double position, double speed, string type) = traffic[i];
                        int trafficPosX = trafficLane == 1 ? panelPosX : trafficLane == 2 ? secondLaneX + 1 : thirdLaneX + 1;
                        int trafficPosY = (int)Math.Round(panelPosY + position + 1);
                        gameBuffer.Append(
                            ConsolePositioning.RenderChangePosition(trafficPosX + 1, trafficPosY) +
                            type
                        );
                        position += speed;
                        traffic[i] = (trafficLane, position, speed, type);
                        if (position >= height - 1)
                        {
                            traffic.RemoveAt(i);
                            if (trafficLane == carLane)
                                crashed = true;
                        }
                    }

                    // Check if we need to add new traffic
                    bool needsTraffic = RandomDriver.RandomChance(10);
                    if (needsTraffic)
                        traffic.Add((RandomDriver.Random(1, 3), 0, RandomDriver.RandomDouble(2.0), trafficTypes[RandomDriver.RandomIdx(trafficTypes.Length)]));
                }
                return gameBuffer.ToString();
            });
            streetRunScreen.CycleFrequency = displayDelay;
            streetRunScreen.AddBufferedPart("StreetRun", streetRunScreenPart);
            ScreenTools.SetCurrent(streetRunScreen);
            ScreenTools.SetCurrentCyclic(streetRunScreen);
            ScreenTools.StartCyclicScreen();

            // Main loop
            bool escaping = false;
            while (!escaping)
            {
                if (crashed)
                {
                    ScreenTools.UnsetCurrentCyclic();
                    ScreenTools.StopCyclicScreen();
                    ScreenTools.Render();
                    break;
                }
                ScreenTools.Render();

                // Let the user decide the box
                var pressedChar = Input.ReadKey();
                switch (pressedChar.Key)
                {
                    case ConsoleKey.Escape:
                        // User decided to escape
                        escaping = true;
                        break;
                    case ConsoleKey.LeftArrow:
                        // Go to left lane
                        if (carLane > 1)
                            carLane--;
                        break;
                    case ConsoleKey.RightArrow:
                        // Go to right lane
                        if (carLane < 3)
                            carLane++;
                        break;
                }
            }

            // Clean up
            if (escaping)
            {
                ScreenTools.UnsetCurrentCyclic();
                ScreenTools.StopCyclicScreen();
            }
            ScreenTools.UnsetCurrent(streetRunScreen);
            ConsoleWrapper.Clear();
        }
    }
}
