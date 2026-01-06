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

                // Print the winner here, or the instructions here
                int instructionsPosY = ConsoleWrapper.WindowHeight - 2;
                var message = new AlignedText()
                {
                    Left = 0,
                    Top = instructionsPosY,
                    Width = ConsoleWrapper.WindowWidth,
                    Height = 1,
                    OneLine = true,

                    // TODO: NKS_AMUSEMENTS_TICTACTOE_WHOSETURN -> Player {0} is currently playing.
                    // TODO: NKS_AMUSEMENTS_TICTACTOE_DRAW -> It's a draw.
                    // TODO: NKS_AMUSEMENTS_TICTACTOE_PLAYERWON -> Player {0} wins the game!
                    Text =
                        winner == 0 ? LanguageTools.GetLocalized("NKS_AMUSEMENTS_TICTACTOE_DRAW") :
                        winner > 0 ? LanguageTools.GetLocalized("NKS_AMUSEMENTS_TICTACTOE_PLAYERWON").FormatString(winner) :
                        LanguageTools.GetLocalized("NKS_AMUSEMENTS_TICTACTOE_WHOSETURN").FormatString(turnNumber),
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
                                break;
                            }
                        }
                    }
                    var pressedChar = Input.ReadKey();
                    switch (pressedChar.Key)
                    {
                        case ConsoleKey.Escape:
                            // User decided to escape
                            done = true;
                            break;
                        case ConsoleKey.Q:
                            // 1, 1
                            grid[0, 0] = turnNumber;
                            SwitchTurn();
                            break;
                        case ConsoleKey.W:
                            // 2, 1
                            grid[1, 0] = turnNumber;
                            SwitchTurn();
                            break;
                        case ConsoleKey.E:
                            // 3, 1
                            grid[2, 0] = turnNumber;
                            SwitchTurn();
                            break;
                        case ConsoleKey.A:
                            // 1, 2
                            grid[0, 1] = turnNumber;
                            SwitchTurn();
                            break;
                        case ConsoleKey.S:
                            // 2, 2
                            grid[1, 1] = turnNumber;
                            SwitchTurn();
                            break;
                        case ConsoleKey.D:
                            // 3, 2
                            grid[2, 1] = turnNumber;
                            SwitchTurn();
                            break;
                        case ConsoleKey.Z:
                            // 1, 3
                            grid[0, 2] = turnNumber;
                            SwitchTurn();
                            break;
                        case ConsoleKey.X:
                            // 2, 3
                            grid[1, 2] = turnNumber;
                            SwitchTurn();
                            break;
                        case ConsoleKey.C:
                            // 3, 3
                            grid[2, 2] = turnNumber;
                            SwitchTurn();
                            break;
                    }
                }
                else if (winner > 0)
                {
                    done = true;
                    Thread.Sleep(5000);
                }
            }

            // Clean up
            ConsoleWrapper.Clear();
        }
    }
}
