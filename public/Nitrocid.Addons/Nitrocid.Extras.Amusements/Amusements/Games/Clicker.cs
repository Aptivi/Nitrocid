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

using System.Collections.Generic;
using System;
using System.Threading;
using Terminaux.Colors;
using Textify.Data.Words;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Languages;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Inputs;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Base.Buffered;
using System.Text;
using Textify.General;
using Nitrocid.Base.Drivers.RNG;
using Terminaux.Writer.CyclicWriters.Graphical.Rulers;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Graphical.Shapes;
using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Kernel.Debugging;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    internal static class Clicker
    {

        internal static void InitializeClicker()
        {
            int funds = 0;
            int fundIncrementationFactor = 1;
            int[] incrementationFactors = [10, 50, 100, 250, 500, 1000, 2500, 5000, 10000];
            int[] costs = [15, 2500, 10000, 50000, 150000, 750000, 1250000, 5000000, 10000000];
            bool done = false;
            int lockedBoxes = 8;
            int incrementSpeed = 1000;
            var clickerScreen = new Screen();
            var clickerScreenPart = new ScreenPart();
            var incrementerThread = new KernelThread("Clicker incrementation thread", true, () =>
            {
                try
                {
                    while (true)
                    {
                        funds += fundIncrementationFactor;
                        Thread.Sleep(incrementSpeed);
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Stopping incrementation thread...");
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            });

            // Make the screen part for the game
            clickerScreenPart.AddDynamicText(() =>
            {
                // Make a panel for the Clicker game
                int width = ConsoleWrapper.WindowWidth / 2;
                int height = ConsoleWrapper.WindowHeight - 6;
                int panelPosX = width / 2;
                int panelPosY = 1;
                int secondBoxPosX = panelPosX + width / 3;
                int secondBoxPosY = 1 + height / 3;
                int thirdBoxPosX = panelPosX + width * 2 / 3;
                int thirdBoxPosY = 1 + height * 2 / 3;
                var boxFrame = new BoxFrame()
                {
                    Left = panelPosX,
                    Top = panelPosY,
                    Width = width,
                    Height = height,
                    Rulers =
                    [
                        new(secondBoxPosX - panelPosX, RulerOrientation.Vertical),
                        new(thirdBoxPosX - panelPosX, RulerOrientation.Vertical),
                        new(secondBoxPosY - panelPosY, RulerOrientation.Horizontal),
                        new(thirdBoxPosY - panelPosY, RulerOrientation.Horizontal),
                    ]
                };
                var gameBuffer = new StringBuilder();
                gameBuffer.Append(boxFrame.Render());

                // Print the unlocked boxes in their appropriate boxes
                void PrintBox(int spotX, int spotY, int boxWidth, int boxHeight, int boxNum)
                {
                    bool isLocked = boxNum >= 9 - lockedBoxes;
                    var eraser = new Eraser()
                    {
                        Left = spotX,
                        Top = spotY,
                        Width = boxWidth,
                        Height = boxHeight,
                    };
                    var boxText = new AlignedText()
                    {
                        Left = spotX,
                        Top = spotY + (boxHeight / 2) - 1,
                        Width = boxWidth,
                        Height = boxHeight,
                        ForegroundColor = isLocked ? ConsoleColors.Red : ConsoleColors.Lime,
                        Text = $"\U0001fa99 ${incrementationFactors[boxNum - 1]} \U0001fa99",
                        Settings = new()
                        {
                            Alignment = TextAlignment.Middle
                        }
                    };
                    var lockedText = new AlignedText()
                    {
                        Left = spotX,
                        Top = spotY + (boxHeight / 2) + 1,
                        Width = boxWidth,
                        Height = boxHeight,
                        Text = $"🔒 ${costs[boxNum - 1]} 🔒",
                        Settings = new()
                        {
                            Alignment = TextAlignment.Middle
                        }
                    };
                    gameBuffer.Append(eraser.Render());
                    gameBuffer.Append(boxText.Render());
                    if (isLocked)
                        gameBuffer.Append(lockedText.Render());
                }
                {
                    int spotX = panelPosX + 1;
                    int spotY = panelPosY + 1;
                    int boxWidth = secondBoxPosX - panelPosX;
                    int boxHeight = secondBoxPosY - panelPosY;
                    PrintBox(spotX, spotY, boxWidth, boxHeight, 1);
                }
                {
                    int spotX = secondBoxPosX + 2;
                    int spotY = panelPosY + 1;
                    int boxWidth = secondBoxPosX - panelPosX - 1;
                    int boxHeight = secondBoxPosY - panelPosY;
                    PrintBox(spotX, spotY, boxWidth, boxHeight, 2);
                }
                {
                    int spotX = thirdBoxPosX + 2;
                    int spotY = panelPosY + 1;
                    int boxWidth = secondBoxPosX - panelPosX - 1;
                    int boxHeight = secondBoxPosY - panelPosY;
                    PrintBox(spotX, spotY, boxWidth, boxHeight, 3);
                }
                {
                    int spotX = panelPosX + 1;
                    int spotY = secondBoxPosY + 2;
                    int boxWidth = thirdBoxPosX - secondBoxPosX;
                    int boxHeight = thirdBoxPosY - secondBoxPosY - 1;
                    PrintBox(spotX, spotY, boxWidth, boxHeight, 4);
                }
                {
                    int spotX = secondBoxPosX + 2;
                    int spotY = secondBoxPosY + 2;
                    int boxWidth = thirdBoxPosX - secondBoxPosX - 1;
                    int boxHeight = thirdBoxPosY - secondBoxPosY - 1;
                    PrintBox(spotX, spotY, boxWidth, boxHeight, 5);
                }
                {
                    int spotX = thirdBoxPosX + 2;
                    int spotY = secondBoxPosY + 2;
                    int boxWidth = thirdBoxPosX - secondBoxPosX - 1;
                    int boxHeight = thirdBoxPosY - secondBoxPosY - 1;
                    PrintBox(spotX, spotY, boxWidth, boxHeight, 6);
                }
                {
                    int spotX = panelPosX + 1;
                    int spotY = thirdBoxPosY + 2;
                    int boxWidth = panelPosX + width - thirdBoxPosX;
                    int boxHeight = height - thirdBoxPosY;
                    PrintBox(spotX, spotY, boxWidth, boxHeight, 7);
                }
                {
                    int spotX = secondBoxPosX + 2;
                    int spotY = thirdBoxPosY + 2;
                    int boxWidth = panelPosX + width - thirdBoxPosX - 1;
                    int boxHeight = height - thirdBoxPosY;
                    PrintBox(spotX, spotY, boxWidth, boxHeight, 8);
                }
                {
                    int spotX = thirdBoxPosX + 2;
                    int spotY = thirdBoxPosY + 2;
                    int boxWidth = panelPosX + width - thirdBoxPosX - 1;
                    int boxHeight = height - thirdBoxPosY;
                    PrintBox(spotX, spotY, boxWidth, boxHeight, 9);
                }

                // Print the winner here, or the instructions here
                int instructionsPosY = ConsoleWrapper.WindowHeight - 2;
                var message = new AlignedText()
                {
                    Left = 0,
                    Top = instructionsPosY,
                    Width = ConsoleWrapper.WindowWidth,
                    Height = 1,
                    OneLine = true,
                    Text = "\x1b[1K" + $"${funds} | ${fundIncrementationFactor}/{incrementSpeed} ms" + "\x1b[K",
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    },
                };
                gameBuffer.Append(message.Render());
                return gameBuffer.ToString();
            });
            clickerScreen.CycleFrequency = incrementSpeed;
            clickerScreen.AddBufferedPart("Clicker", clickerScreenPart);
            ScreenTools.SetCurrent(clickerScreen);
            ScreenTools.SetCurrentCyclic(clickerScreen);
            ScreenTools.StartCyclicScreen();

            // Helper function for box unlock
            void UnlockOrGain(int boxNum)
            {
                int boxIdx = boxNum - 1;
                if (lockedBoxes >= 9 - boxNum && funds >= costs[boxIdx])
                {
                    // Unlock the box
                    funds -= costs[boxIdx];
                    lockedBoxes--;
                    fundIncrementationFactor += incrementationFactors[boxIdx];
                    fundIncrementationFactor += RandomDriver.Random(15, 250);
                    incrementSpeed -= 100;
                    clickerScreen.CycleFrequency = incrementSpeed;
                }
                else if (lockedBoxes < 9 - boxNum)
                {
                    // Box is unlocked
                    funds += incrementationFactors[boxIdx];
                }
            }

            // Main loop
            incrementerThread.Start();
            while (!done)
            {
                ScreenTools.Render();

                // Let the user decide the box
                var pressedChar = Input.ReadKey();
                switch (pressedChar.Key)
                {
                    case ConsoleKey.Escape:
                        // User decided to escape
                        done = true;
                        break;
                    case ConsoleKey.Q:
                        // Unlock the first box
                        UnlockOrGain(1);
                        break;
                    case ConsoleKey.W:
                        // Unlock the second box
                        UnlockOrGain(2);
                        break;
                    case ConsoleKey.E:
                        // Unlock the third box
                        UnlockOrGain(3);
                        break;
                    case ConsoleKey.A:
                        // Unlock the fourth box
                        UnlockOrGain(4);
                        break;
                    case ConsoleKey.S:
                        // Unlock the fifth box
                        UnlockOrGain(5);
                        break;
                    case ConsoleKey.D:
                        // Unlock the sixth box
                        UnlockOrGain(6);
                        break;
                    case ConsoleKey.Z:
                        // Unlock the seventh box
                        UnlockOrGain(7);
                        break;
                    case ConsoleKey.X:
                        // Unlock the eighth box
                        UnlockOrGain(8);
                        break;
                    case ConsoleKey.C:
                        // Unlock the ninth box
                        UnlockOrGain(9);
                        break;
                }
            }

            // Clean up
            ScreenTools.UnsetCurrentCyclic();
            ScreenTools.StopCyclicScreen();
            incrementerThread.Stop();
            ConsoleWrapper.Clear();
        }
    }
}
