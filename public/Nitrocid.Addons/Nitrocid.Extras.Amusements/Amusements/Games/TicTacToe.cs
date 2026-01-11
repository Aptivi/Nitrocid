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

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    internal static class TicTacToe
    {

        internal static void InitializeTicTacToe(bool computerMode)
        {
            int turnNumber = 1;
            int winner = -1;
            int computerTurn = RandomDriver.Random(1, 2);
            bool done = false;
            int[,] grid = new int[3, 3];
            var ticTacToeScreen = new Screen();
            var ticTacToeScreenPart = new ScreenPart();

            // Make the screen part for the game
            ticTacToeScreenPart.AddDynamicText(() =>
            {
                // Make a panel for the Tic-Tac-Toe game
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

                // Print the X's and O's in their appropriate boxes
                void PrintXO(int spotX, int spotY, int boxWidth, int boxHeight, int gridX, int gridY)
                {
                    int gridTurn = grid[gridX, gridY];
                    if (gridTurn > 0)
                    {
                        if (gridTurn == 1)
                        {
                            // It's an X
                            var firstStroke = new Line()
                            {
                                StartPos = new(spotX + 1, spotY),
                                EndPos = new(spotX + 1 + boxWidth - 2, spotY + 1 + boxHeight - 2),
                                DoubleWidth = false,
                                Color = ConsoleColors.Red,
                            };
                            var secondStroke = new Line()
                            {
                                StartPos = new(spotX + 1 + boxWidth - 2, spotY),
                                EndPos = new(spotX + 1, spotY + 1 + boxHeight - 2),
                                DoubleWidth = false,
                                Color = ConsoleColors.Red,
                            };
                            gameBuffer.Append(firstStroke.Render());
                            gameBuffer.Append(secondStroke.Render());
                        }
                        else if (gridTurn == 2)
                        {
                            // It's an O
                            var arcStroke = new Arc(boxHeight - 1, spotX + 3, spotY, ConsoleColors.Lime)
                            {
                                AngleStart = 360,
                                AngleEnd = 360,
                                OuterRadius = (boxHeight - 1) / 2,
                                InnerRadius = (boxHeight - 1) / 2 - 4,
                            };
                            gameBuffer.Append(arcStroke.Render());
                        }
                    }
                }
                {
                    int spotX = panelPosX + 1;
                    int spotY = panelPosY + 1;
                    int boxWidth = secondBoxPosX - panelPosX;
                    int boxHeight = secondBoxPosY - panelPosY;
                    PrintXO(spotX, spotY, boxWidth, boxHeight, 0, 0);
                }
                {
                    int spotX = secondBoxPosX + 2;
                    int spotY = panelPosY + 1;
                    int boxWidth = secondBoxPosX - panelPosX - 1;
                    int boxHeight = secondBoxPosY - panelPosY;
                    PrintXO(spotX, spotY, boxWidth, boxHeight, 1, 0);
                }
                {
                    int spotX = thirdBoxPosX + 2;
                    int spotY = panelPosY + 1;
                    int boxWidth = secondBoxPosX - panelPosX - 1;
                    int boxHeight = secondBoxPosY - panelPosY;
                    PrintXO(spotX, spotY, boxWidth, boxHeight, 2, 0);
                }
                {
                    int spotX = panelPosX + 1;
                    int spotY = secondBoxPosY + 2;
                    int boxWidth = thirdBoxPosX - secondBoxPosX;
                    int boxHeight = thirdBoxPosY - secondBoxPosY - 1;
                    PrintXO(spotX, spotY, boxWidth, boxHeight, 0, 1);
                }
                {
                    int spotX = secondBoxPosX + 2;
                    int spotY = secondBoxPosY + 2;
                    int boxWidth = thirdBoxPosX - secondBoxPosX - 1;
                    int boxHeight = thirdBoxPosY - secondBoxPosY - 1;
                    PrintXO(spotX, spotY, boxWidth, boxHeight, 1, 1);
                }
                {
                    int spotX = thirdBoxPosX + 2;
                    int spotY = secondBoxPosY + 2;
                    int boxWidth = thirdBoxPosX - secondBoxPosX - 1;
                    int boxHeight = thirdBoxPosY - secondBoxPosY - 1;
                    PrintXO(spotX, spotY, boxWidth, boxHeight, 2, 1);
                }
                {
                    int spotX = panelPosX + 1;
                    int spotY = thirdBoxPosY + 2;
                    int boxWidth = panelPosX + width - thirdBoxPosX;
                    int boxHeight = height - thirdBoxPosY;
                    PrintXO(spotX, spotY, boxWidth, boxHeight, 0, 2);
                }
                {
                    int spotX = secondBoxPosX + 2;
                    int spotY = thirdBoxPosY + 2;
                    int boxWidth = panelPosX + width - thirdBoxPosX - 1;
                    int boxHeight = height - thirdBoxPosY;
                    PrintXO(spotX, spotY, boxWidth, boxHeight, 1, 2);
                }
                {
                    int spotX = thirdBoxPosX + 2;
                    int spotY = thirdBoxPosY + 2;
                    int boxWidth = panelPosX + width - thirdBoxPosX - 1;
                    int boxHeight = height - thirdBoxPosY;
                    PrintXO(spotX, spotY, boxWidth, boxHeight, 2, 2);
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
                    Text = "\x1b[1K" +
                        (winner == 0 ? LanguageTools.GetLocalized("NKS_AMUSEMENTS_TICTACTOE_DRAW") :
                         winner > 0 ? LanguageTools.GetLocalized("NKS_AMUSEMENTS_TICTACTOE_PLAYERWON").FormatString(winner) :
                         LanguageTools.GetLocalized("NKS_AMUSEMENTS_TICTACTOE_WHOSETURN").FormatString(turnNumber)) + "\x1b[K",
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    },
                    ForegroundColor =
                        winner == 0 ? ConsoleColors.Yellow :
                        winner > 0 ? ConsoleColors.Lime :
                        ConsoleColors.White,
                };
                gameBuffer.Append(message.Render());
                return gameBuffer.ToString();
            });
            ticTacToeScreen.AddBufferedPart("Tic-Tac-Toe", ticTacToeScreenPart);
            ScreenTools.SetCurrent(ticTacToeScreen);

            // Helper function to switch turns
            void SwitchTurn()
            {
                turnNumber++;
                if (turnNumber > 2)
                    turnNumber = 1;
            }
            void CheckWinner(int player)
            {
                if (player == 0)
                    return;
                if (grid[0, 0] == player && grid[0, 1] == player && grid[0, 2] == player)
                    winner = player;
                else if (grid[1, 0] == player && grid[1, 1] == player && grid[1, 2] == player)
                    winner = player;
                else if (grid[2, 0] == player && grid[2, 1] == player && grid[2, 2] == player)
                    winner = player;
                else if (grid[0, 0] == player && grid[1, 0] == player && grid[2, 0] == player)
                    winner = player;
                else if (grid[0, 1] == player && grid[1, 1] == player && grid[2, 1] == player)
                    winner = player;
                else if (grid[0, 2] == player && grid[1, 2] == player && grid[2, 2] == player)
                    winner = player;
                else if (grid[0, 0] == player && grid[1, 1] == player && grid[2, 2] == player)
                    winner = player;
                else if (grid[0, 2] == player && grid[1, 1] == player && grid[2, 0] == player)
                    winner = player;
                else if (grid[0, 0] != 0 && grid[1, 0] != 0 && grid[2, 0] != 0 &&
                         grid[0, 1] != 0 && grid[1, 1] != 0 && grid[2, 1] != 0 &&
                         grid[0, 2] != 0 && grid[1, 2] != 0 && grid[2, 2] != 0)
                    winner = 0;
            }

            // Main loop
            while (!done)
            {
                ScreenTools.Render();

                // Let the user decide the position
                if (winner < 0)
                {
                    if (computerMode && turnNumber == computerTurn)
                    {
                        Thread.Sleep(RandomDriver.Random(5000));
                        while (true)
                        {
                            int gridX = RandomDriver.RandomIdx(3);
                            int gridY = RandomDriver.RandomIdx(3);
                            if (grid[gridX, gridY] == 0)
                            {
                                grid[gridX, gridY] = turnNumber;
                                CheckWinner(turnNumber);
                                SwitchTurn();
                                break;
                            }
                        }
                    }
                    else
                    {
                        var pressedChar = Input.ReadKey();
                        switch (pressedChar.Key)
                        {
                            case ConsoleKey.Escape:
                                // User decided to escape
                                done = true;
                                break;
                            case ConsoleKey.Q:
                                // 1, 1
                                if (grid[0, 0] == 0)
                                    grid[0, 0] = turnNumber;
                                CheckWinner(turnNumber);
                                SwitchTurn();
                                break;
                            case ConsoleKey.W:
                                // 2, 1
                                if (grid[1, 0] == 0)
                                    grid[1, 0] = turnNumber;
                                CheckWinner(turnNumber);
                                SwitchTurn();
                                break;
                            case ConsoleKey.E:
                                // 3, 1
                                if (grid[2, 0] == 0)
                                    grid[2, 0] = turnNumber;
                                CheckWinner(turnNumber);
                                SwitchTurn();
                                break;
                            case ConsoleKey.A:
                                // 1, 2
                                if (grid[0, 1] == 0)
                                    grid[0, 1] = turnNumber;
                                CheckWinner(turnNumber);
                                SwitchTurn();
                                break;
                            case ConsoleKey.S:
                                // 2, 2
                                if (grid[1, 1] == 0)
                                    grid[1, 1] = turnNumber;
                                CheckWinner(turnNumber);
                                SwitchTurn();
                                break;
                            case ConsoleKey.D:
                                // 3, 2
                                if (grid[2, 1] == 0)
                                    grid[2, 1] = turnNumber;
                                CheckWinner(turnNumber);
                                SwitchTurn();
                                break;
                            case ConsoleKey.Z:
                                // 1, 3
                                if (grid[0, 2] == 0)
                                    grid[0, 2] = turnNumber;
                                CheckWinner(turnNumber);
                                SwitchTurn();
                                break;
                            case ConsoleKey.X:
                                // 2, 3
                                if (grid[1, 2] == 0)
                                    grid[1, 2] = turnNumber;
                                CheckWinner(turnNumber);
                                SwitchTurn();
                                break;
                            case ConsoleKey.C:
                                // 3, 3
                                if (grid[2, 2] == 0)
                                    grid[2, 2] = turnNumber;
                                CheckWinner(turnNumber);
                                SwitchTurn();
                                break;
                        }
                    }
                }
                else if (winner >= 0)
                {
                    done = true;
                    Thread.Sleep(5000);
                }
            }

            // Clean up
            ScreenTools.UnsetCurrent(ticTacToeScreen);
            ConsoleWrapper.Clear();
        }
    }
}
