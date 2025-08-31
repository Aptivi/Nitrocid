﻿//
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
using System.Collections.Generic;
using System.Threading;
using Terminaux.Colors;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Threading;
using Nitrocid.Languages;
using Nitrocid.Drivers.RNG;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Configuration;
using Terminaux.Inputs;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    /// <summary>
    /// ShipDuet shooter game module
    /// </summary>
    public static class ShipDuetShooter
    {

        internal readonly static KernelThread ShipDuetDrawThread = new("ShipDuet Shooter Draw Thread", true, DrawGame);
        internal static bool GameEnded = false;
        internal static bool GameExiting = false;
        internal static int shipDuetSpeed = 10;
        private static bool player1Won = false;
        private static bool player2Won = false;
        private static int SpaceshipHeightPlayer1 = 0;
        private readonly static int MaxBulletsPlayer1 = 10;
        private readonly static List<(int, int)> BulletsPlayer1 = [];
        private static int SpaceshipHeightPlayer2 = 0;
        private readonly static int MaxBulletsPlayer2 = 10;
        private readonly static List<(int, int)> BulletsPlayer2 = [];
        private readonly static List<(int, int)> Stars = [];

        /// <summary>
        /// Use PowerLine characters for the spaceship?
        /// </summary>
        public static bool ShipDuetUsePowerLine =>
            AmusementsInit.AmusementsConfig.ShipDuetUsePowerLine;
        /// <summary>
        /// ShipDuet speed in milliseconds
        /// </summary>
        public static int ShipDuetSpeed
        {
            get => AmusementsInit.AmusementsConfig.ShipDuetSpeed;
            set => AmusementsInit.AmusementsConfig.ShipDuetSpeed = value < 0 ? 10 : value;
        }

        /// <summary>
        /// Initializes the ShipDuet game
        /// </summary>
        public static void InitializeShipDuet(bool simulation = false)
        {
            // Clear screen
            ColorTools.LoadBackDry(0);

            // Clear all bullets
            BulletsPlayer1.Clear();
            BulletsPlayer2.Clear();
            Stars.Clear();
            player1Won = false;
            player2Won = false;

            // Make the spaceship height in the center
            SpaceshipHeightPlayer1 = SpaceshipHeightPlayer2 = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);

            // Start the draw thread
            ShipDuetDrawThread.Stop();
            ShipDuetDrawThread.Start();

            // Remove the cursor
            ConsoleWrapper.CursorVisible = false;

            // Now, handle input or simulate keypresses
            if (!simulation)
            {
                // Player mode
                ConsoleKeyInfo Keypress;
                while (!GameEnded)
                {
                    if (ConsoleWrapper.KeyAvailable)
                    {
                        // Read the key and handle it
                        Keypress = Input.ReadKey();
                        HandleKeypress(Keypress.Key);
                    }
                }
            }
            else
            {
                // Simulation mode
                ConsoleKey Keypress = 0;
                ConsoleKey[] possibleKeys =
                [
                    // Player 1
                    ConsoleKey.UpArrow,
                    ConsoleKey.DownArrow,
                    ConsoleKey.Enter,

                    // Player 2
                    ConsoleKey.W,
                    ConsoleKey.S,
                    ConsoleKey.Spacebar
                ];
                while (!GameEnded)
                {
                    float PossibilityToChange = (float)RandomDriver.RandomDouble();
                    if ((int)Math.Round(PossibilityToChange) == 1)
                        Keypress = possibleKeys[RandomDriver.RandomIdx(possibleKeys.Length)];

                    // Select command based on key value
                    HandleKeypress(Keypress);
                    ScreensaverManager.Delay(100);
                }
            }

            // Stop the draw thread since the game ended
            ShipDuetDrawThread.Wait();
            ShipDuetDrawThread.Stop();
            GameEnded = false;
            GameExiting = false;
        }

        private static void HandleKeypress(ConsoleKey Keypress)
        {
            switch (Keypress)
            {
                case ConsoleKey.UpArrow:
                    if (SpaceshipHeightPlayer1 > 0)
                        SpaceshipHeightPlayer1 -= 1;
                    break;
                case ConsoleKey.DownArrow:
                    if (SpaceshipHeightPlayer1 < ConsoleWrapper.WindowHeight - 1)
                        SpaceshipHeightPlayer1 += 1;
                    break;
                case ConsoleKey.Enter:
                    if (BulletsPlayer1.Count < MaxBulletsPlayer1)
                        BulletsPlayer1.Add((1, SpaceshipHeightPlayer1));
                    break;
                case ConsoleKey.W:
                    if (SpaceshipHeightPlayer2 > 0)
                        SpaceshipHeightPlayer2 -= 1;
                    break;
                case ConsoleKey.S:
                    if (SpaceshipHeightPlayer2 < ConsoleWrapper.WindowHeight - 1)
                        SpaceshipHeightPlayer2 += 1;
                    break;
                case ConsoleKey.Spacebar:
                    if (BulletsPlayer2.Count < MaxBulletsPlayer2)
                        BulletsPlayer2.Add((ConsoleWrapper.WindowWidth - 2, SpaceshipHeightPlayer2));
                    break;
                case ConsoleKey.Escape:
                    GameEnded = true;
                    GameExiting = true;
                    break;
            }
        }

        private static void DrawGame()
        {
            try
            {
                while (!GameEnded)
                {
                    // Buffer
                    var buffer = new StringBuilder();

                    // Clear only the relevant parts
                    for (int y = 0; y < ConsoleWrapper.WindowHeight; y++)
                    {
                        if (y != SpaceshipHeightPlayer1)
                            buffer.Append(
                                ColorTools.RenderSetConsoleColor(new Color(ConsoleColors.Black), true) +
                                TextWriterWhereColor.RenderWhere(" ", 0, y)
                            );
                        if (y != SpaceshipHeightPlayer2)
                            buffer.Append(
                                ColorTools.RenderSetConsoleColor(new Color(ConsoleColors.Black), true) +
                                TextWriterWhereColor.RenderWhere(" ", ConsoleWrapper.WindowWidth - 1, y)
                            );
                    }

                    // Move the Player 1 bullets right
                    for (int Bullet = 0; Bullet <= BulletsPlayer1.Count - 1; Bullet++)
                    {
                        buffer.Append(
                            ColorTools.RenderSetConsoleColor(new Color(ConsoleColors.Black), true) +
                            TextWriterWhereColor.RenderWhere(" ", BulletsPlayer1[Bullet].Item1, BulletsPlayer1[Bullet].Item2)
                        );
                        int BulletX = BulletsPlayer1[Bullet].Item1 + 1;
                        int BulletY = BulletsPlayer1[Bullet].Item2;
                        BulletsPlayer1[Bullet] = (BulletX, BulletY);
                    }

                    // Move the Player 2 bullets left
                    for (int Bullet = 0; Bullet <= BulletsPlayer2.Count - 1; Bullet++)
                    {
                        buffer.Append(
                            ColorTools.RenderSetConsoleColor(new Color(ConsoleColors.Black), true) +
                            TextWriterWhereColor.RenderWhere(" ", BulletsPlayer2[Bullet].Item1, BulletsPlayer2[Bullet].Item2)
                        );
                        int BulletX = BulletsPlayer2[Bullet].Item1 - 1;
                        int BulletY = BulletsPlayer2[Bullet].Item2;
                        BulletsPlayer2[Bullet] = (BulletX, BulletY);
                    }

                    // Move the stars left
                    for (int Star = 0; Star <= Stars.Count - 1; Star++)
                    {
                        buffer.Append(
                            ColorTools.RenderSetConsoleColor(new Color(ConsoleColors.Black), true) +
                            TextWriterWhereColor.RenderWhere(" ", Stars[Star].Item1, Stars[Star].Item2)
                        );
                        int StarX = Stars[Star].Item1 - 1;
                        int StarY = Stars[Star].Item2;
                        Stars[Star] = (StarX, StarY);
                    }

                    // If any bullet is out of X range, delete it
                    for (int BulletIndex = BulletsPlayer1.Count - 1; BulletIndex >= 0; BulletIndex -= 1)
                    {
                        var Bullet = BulletsPlayer1[BulletIndex];
                        if (Bullet.Item1 >= ConsoleWrapper.WindowWidth)
                        {
                            // The bullet went beyond. Remove it.
                            BulletsPlayer1.RemoveAt(BulletIndex);
                        }
                    }
                    for (int BulletIndex = BulletsPlayer2.Count - 1; BulletIndex >= 0; BulletIndex -= 1)
                    {
                        var Bullet = BulletsPlayer2[BulletIndex];
                        if (Bullet.Item1 < 0)
                        {
                            // The bullet went beyond. Remove it.
                            BulletsPlayer2.RemoveAt(BulletIndex);
                        }
                    }

                    // If any star is out of X range, delete it
                    for (int StarIndex = Stars.Count - 1; StarIndex >= 0; StarIndex -= 1)
                    {
                        var Star = Stars[StarIndex];
                        if (Star.Item1 < 0)
                        {
                            // The star went beyond. Remove it.
                            Stars.RemoveAt(StarIndex);
                        }
                    }

                    // Add new star if guaranteed
                    bool StarShowGuaranteed = RandomDriver.RandomChance(10);
                    if (StarShowGuaranteed)
                    {
                        int StarX = ConsoleWrapper.WindowWidth - 1;
                        int StarY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                        Stars.Add((StarX, StarY));
                    }

                    // Draw the stars, the bullet, and the spaceship if any of them are updated
                    buffer.Append(DrawSpaceships());
                    for (int BulletIndex = BulletsPlayer1.Count - 1; BulletIndex >= 0; BulletIndex -= 1)
                    {
                        var Bullet = BulletsPlayer1[BulletIndex];
                        buffer.Append(DrawBullet(Bullet.Item1, Bullet.Item2));
                    }
                    for (int BulletIndex = BulletsPlayer2.Count - 1; BulletIndex >= 0; BulletIndex -= 1)
                    {
                        var Bullet = BulletsPlayer2[BulletIndex];
                        buffer.Append(DrawBullet(Bullet.Item1, Bullet.Item2));
                    }
                    for (int StarIndex = Stars.Count - 1; StarIndex >= 0; StarIndex -= 1)
                    {
                        var Star = Stars[StarIndex];
                        char StarSymbol = '*';
                        int StarX = Star.Item1;
                        int StarY = Star.Item2;
                        buffer.Append(
                            new Color(ConsoleColors.White).VTSequenceForeground +
                            ColorTools.RenderSetConsoleColor(new Color(ConsoleColors.Black), true) +
                            TextWriterWhereColor.RenderWhere(Convert.ToString(StarSymbol), StarX, StarY, false)
                        );
                    }

                    // Check to see if the spaceship is blown up by the opposing spaceship
                    for (int BulletIndex = BulletsPlayer1.Count - 1; BulletIndex >= 0; BulletIndex -= 1)
                    {
                        var Bullet = BulletsPlayer1[BulletIndex];
                        if (Bullet.Item1 == ConsoleWrapper.WindowWidth - 1 & Bullet.Item2 == SpaceshipHeightPlayer2)
                        {
                            // The spaceship crashed! Game ended and Player 1 won.
                            GameEnded = true;
                            player1Won = true;
                        }
                    }
                    for (int BulletIndex = BulletsPlayer2.Count - 1; BulletIndex >= 0; BulletIndex -= 1)
                    {
                        var Bullet = BulletsPlayer2[BulletIndex];
                        if (Bullet.Item1 == 0 & Bullet.Item2 == SpaceshipHeightPlayer1)
                        {
                            // The spaceship crashed! Game ended and Player 2 won.
                            GameEnded = true;
                            player2Won = true;
                        }
                    }

                    // Wait for a few milliseconds
                    TextWriterRaw.WritePlain(buffer.ToString(), false);
                    ThreadManager.SleepNoBlock(ShipDuetSpeed, ShipDuetDrawThread);
                }
            }
            catch (ThreadInterruptedException)
            {
                GameExiting = true;
            }
            // Game is over. Move to the Finally block.
            catch (Exception ex)
            {
                // Game is over with an unexpected error.
                try
                {
                    TextWriterWhereColor.WriteWhereColor(Translate.DoTranslation("Unexpected error") + ": {0}", 0, ConsoleWrapper.WindowHeight - 1, false, ConsoleColors.Red, vars: ex.Message);
                    ThreadManager.SleepNoBlock(3000L, ShipDuetDrawThread);
                }
                catch
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.E, "Can't display error message on meteor shooter.");
                }
                finally
                {
                    GameExiting = true;
                    ConsoleWrapper.Clear();
                }
            }
            finally
            {
                // Write who is the winner
                if (!GameExiting)
                {
                    try
                    {
                        if (player1Won && player2Won || !player1Won && !player2Won)
                            TextWriterWhereColor.WriteWhereColor(Translate.DoTranslation("It's a draw."), 0, ConsoleWrapper.WindowHeight - 1, false, ConsoleColors.Red);
                        else if (player1Won || player2Won)
                            TextWriterWhereColor.WriteWhereColor(Translate.DoTranslation("Player {0} wins!"), 0, ConsoleWrapper.WindowHeight - 1, false, ConsoleColors.Red, vars: player1Won ? 1 : 2);
                        ThreadManager.SleepNoBlock(3000L, ShipDuetDrawThread);
                    }
                    catch
                    {
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.E, "Can't display game over on meteor shooter.");
                    }
                }
                ConsoleWrapper.Clear();
            }
        }

        private static string DrawSpaceships()
        {
            var builder = new StringBuilder();
            char PowerLineSpaceshipP1 = Convert.ToChar(0xE0B0);
            char PowerLineSpaceshipP2 = Convert.ToChar(0xE0B2);
            char SpaceshipSymbolP1 = ShipDuetUsePowerLine ? PowerLineSpaceshipP1 : '>';
            char SpaceshipSymbolP2 = ShipDuetUsePowerLine ? PowerLineSpaceshipP2 : '<';
            builder.Append(
                TextWriterWhereColor.RenderWhereColorBack(Convert.ToString(SpaceshipSymbolP1), 0, SpaceshipHeightPlayer1, false, ConsoleColors.Green, ConsoleColors.Black) +
                TextWriterWhereColor.RenderWhereColorBack(Convert.ToString(SpaceshipSymbolP2), ConsoleWrapper.WindowWidth - 1, SpaceshipHeightPlayer2, false, ConsoleColors.DarkGreen, ConsoleColors.Black)
            );
            return builder.ToString();
        }

        private static string DrawBullet(int BulletX, int BulletY)
        {
            char BulletSymbol = '-';
            return TextWriterWhereColor.RenderWhereColorBack(Convert.ToString(BulletSymbol), BulletX, BulletY, false, ConsoleColors.Aqua, ConsoleColors.Black);
        }

    }
}
