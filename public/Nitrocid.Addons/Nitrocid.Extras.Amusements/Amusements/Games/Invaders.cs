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
using System.Threading;
using Terminaux.Colors;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Inputs;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Base.Extensions;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    /// <summary>
    /// Invaders shooter game module
    /// </summary>
    public static class Invaders
    {

        internal readonly static KernelThread InvadersDrawThread = new("Invaders Shooter Draw Thread", true, (dodge) => DrawGame());
        internal static bool GameEnded = false;
        internal static bool GameExiting = false;
        internal static int invadersSpeed = 10;
        private static int SpaceshipLeft = 0;
        private static int score = 0;
        private static int cycles = 0;
        private readonly static int MaxBullets = 10;
        private readonly static List<(int, int)> Bullets = [];
        private readonly static int MaxMeteors = 10;
        private readonly static List<(int, int)> Meteors = [];
        private readonly static List<(int, int)> enemies = [];

        /// <summary>
        /// Initializes the Invaders game
        /// </summary>
        public static void InitializeInvaders()
        {
            // Clear screen
            ConsoleColoring.LoadBackDry(0);

            // Clear all bullets, meteors, and enemies
            Bullets.Clear();
            Meteors.Clear();
            enemies.Clear();

            // Reset the score and the cycles
            cycles = 0;
            score = 0;

            // Make the spaceship width in the center
            SpaceshipLeft = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);

            // Start the draw thread
            InvadersDrawThread.Stop();
            InvadersDrawThread.Start();

            // Remove the cursor
            ConsoleWrapper.CursorVisible = false;

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

            // Stop the draw thread since the game ended
            InvadersDrawThread.Wait();
            InvadersDrawThread.Stop();
            GameEnded = false;
            GameExiting = false;
        }

        private static void HandleKeypress(ConsoleKey Keypress)
        {
            switch (Keypress)
            {
                case ConsoleKey.LeftArrow:
                    if (SpaceshipLeft > 0)
                        SpaceshipLeft -= 1;
                    break;
                case ConsoleKey.RightArrow:
                    if (SpaceshipLeft < ConsoleWrapper.WindowWidth - 1)
                        SpaceshipLeft += 1;
                    break;
                case ConsoleKey.Spacebar:
                    // Shoot the bullet!
                    if (Bullets.Count < MaxBullets)
                        Bullets.Add((SpaceshipLeft, ConsoleWrapper.WindowHeight - 1));
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
                // Important variables
                bool reversing = false;
                bool movingDown = false;
                bool movedDown = false;

                while (!GameEnded)
                {
                    // Populate enemies depending on console width and height
                    if (enemies.Count == 0)
                    {
                        int enemyColumns = (ConsoleWrapper.WindowWidth - 6) / 24;
                        int enemyRows = ConsoleWrapper.WindowHeight * 2 / 3 / 9;
                        for (int i = 0; i < enemyColumns; i++)
                        {
                            int columnX = 3 + (24 * i);
                            for (int j = 0; j < enemyRows; j++)
                            {
                                int columnY = 2 + (9 * j);
                                enemies.Add((columnX, columnY));
                            }
                        }
                    }

                    // Buffer
                    var buffer = new StringBuilder();
                    buffer.Append(
                        ConsolePositioning.RenderChangePosition(0, 0) +
                        ConsoleClearing.GetClearWholeScreenSequence()
                    );

                    // Show the score
                    buffer.Append(
                        new Color(ConsoleColors.Green).VTSequenceForeground() +
                        TextWriterWhereColor.RenderWhere($"{score}", ConsoleWrapper.WindowWidth - $"{score}".Length, 0)
                    );

                    // Move the meteors bottom
                    for (int Meteor = 0; Meteor <= Meteors.Count - 1; Meteor++)
                    {
                        int MeteorX = Meteors[Meteor].Item1;
                        int MeteorY = Meteors[Meteor].Item2 + 1;
                        Meteors[Meteor] = (MeteorX, MeteorY);
                    }

                    // Move the bullets up
                    for (int Bullet = 0; Bullet <= Bullets.Count - 1; Bullet++)
                    {
                        int BulletX = Bullets[Bullet].Item1;
                        int BulletY = Bullets[Bullet].Item2 - 1;
                        Bullets[Bullet] = (BulletX, BulletY);
                    }

                    // If any bullet is out of Y range, delete it
                    for (int BulletIndex = Bullets.Count - 1; BulletIndex >= 0; BulletIndex -= 1)
                    {
                        var Bullet = Bullets[BulletIndex];
                        if (Bullet.Item2 < 0)
                        {
                            // The bullet went beyond. Remove it.
                            Bullets.RemoveAt(BulletIndex);
                        }
                    }

                    // If any meteor is out of Y range, delete it
                    for (int MeteorIndex = Meteors.Count - 1; MeteorIndex >= 0; MeteorIndex -= 1)
                    {
                        var Meteor = Meteors[MeteorIndex];
                        if (Meteor.Item2 > ConsoleWrapper.WindowHeight - 1)
                        {
                            // The meteor went beyond. Remove it.
                            Meteors.RemoveAt(MeteorIndex);
                        }
                    }

                    // Add new meteor if guaranteed
                    double MeteorShowProbability = 10d / 100d;
                    bool MeteorShowGuaranteed = RandomDriver.RandomDouble() < MeteorShowProbability;
                    if (MeteorShowGuaranteed & Meteors.Count < MaxMeteors)
                    {
                        int MeteorX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth - 1);
                        int MeteorY = 0;
                        Meteors.Add((MeteorX, MeteorY));
                    }

                    // Draw the meteor, the bullet, and the spaceship if any of them are updated
                    buffer.Append(DrawSpaceship());
                    for (int MeteorIndex = Meteors.Count - 1; MeteorIndex >= 0; MeteorIndex -= 1)
                    {
                        var Meteor = Meteors[MeteorIndex];
                        buffer.Append(DrawMeteors(Meteor.Item1, Meteor.Item2));
                    }
                    for (int BulletIndex = Bullets.Count - 1; BulletIndex >= 0; BulletIndex -= 1)
                    {
                        var Bullet = Bullets[BulletIndex];
                        buffer.Append(DrawBullet(Bullet.Item1, Bullet.Item2));
                    }

                    // Draw the enemies, and move them, depending on if the last enemy or the first enemy is about to touch the right/left edges
                    if (cycles > 10)
                    {
                        for (int enemyIndex = enemies.Count - 1; enemyIndex >= 0; enemyIndex -= 1)
                        {
                            var enemy = enemies[enemyIndex];

                            // Check if enemies need to reverse the direction and to move down once
                            if ((enemy.Item1 + 24 > ConsoleWrapper.WindowWidth - 3 || enemy.Item1 < 3) && !movingDown && !movedDown)
                            {
                                reversing = !reversing;
                                movingDown = true;
                            }
                        }
                        for (int enemyIndex = enemies.Count - 1; enemyIndex >= 0; enemyIndex -= 1)
                        {
                            var enemy = enemies[enemyIndex];

                            // Check if enemies need to reverse the direction and to move down once
                            if (movingDown)
                            {
                                enemies[enemyIndex] = (enemy.Item1, enemy.Item2 + 1);
                                movedDown = true;
                            }
                            else if (movedDown || !movingDown)
                            {
                                enemies[enemyIndex] = (enemy.Item1 + (reversing ? -1 : 1), enemy.Item2);
                                movedDown = false;
                            }
                        }
                        movingDown = false;
                        cycles = 0;
                    }
                    for (int enemyIndex = enemies.Count - 1; enemyIndex >= 0; enemyIndex -= 1)
                    {
                        var enemy = enemies[enemyIndex];
                        buffer.Append(DrawEnemy(enemy.Item1, enemy.Item2));
                    }

                    // Check if the bullet has reached the enemy
                    for (int enemyIndex = enemies.Count - 1; enemyIndex >= 0; enemyIndex -= 1)
                    {
                        var enemy = enemies[enemyIndex];
                        for (int BulletIndex = Bullets.Count - 1; BulletIndex >= 0; BulletIndex -= 1)
                        {
                            var Bullet = Bullets[BulletIndex];
                            if (Bullet.Item1 >= enemy.Item1 && Bullet.Item1 <= enemy.Item1 + 22 &&
                                Bullet.Item2 >= enemy.Item2 && Bullet.Item2 <= enemy.Item2 + 8)
                            {
                                // The enemy got killed. Remove it, and remove the bullet.
                                enemies.RemoveAt(enemyIndex);
                                Bullets.RemoveAt(BulletIndex);
                                score++;
                            }
                        }
                    }

                    // Check if the enemy has reached the bottom
                    for (int enemyIndex = enemies.Count - 1; enemyIndex >= 0; enemyIndex -= 1)
                    {
                        var enemy = enemies[enemyIndex];
                        if (enemy.Item2 + 8 >= ConsoleWrapper.WindowHeight - 3)
                            GameEnded = true;
                    }

                    // Wait for a few milliseconds
                    TextWriterRaw.WritePlain(buffer.ToString(), false);
                    ThreadManager.SleepNoBlock(10, InvadersDrawThread);
                    cycles++;
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
                    TextWriterWhereColor.WriteWhereColor(LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMON_UNEXPECTEDERROR") + ": {0}", 0, ConsoleWrapper.WindowHeight - 1, false, ConsoleColors.Red, vars: ex.Message);
                    ThreadManager.SleepNoBlock(3000L, InvadersDrawThread);
                }
                catch
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.E, "Can't display error message on invaders shooter.");
                }
                finally
                {
                    GameExiting = true;
                    ConsoleWrapper.Clear();
                }
            }
            finally
            {
                // Write game over if not exiting
                if (!GameExiting)
                {
                    try
                    {
                        TextWriterWhereColor.WriteWhereColor(LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMON_GAMEOVER"), 0, ConsoleWrapper.WindowHeight - 1, false, ConsoleColors.Red);
                        ThreadManager.SleepNoBlock(3000L, InvadersDrawThread);
                    }
                    catch
                    {
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.E, "Can't display game over on invaders shooter.");
                    }
                }
                GameEnded = true;
                ConsoleWrapper.Clear();
            }
        }

        private static string DrawSpaceship()
        {
            char SpaceshipSymbol = '^';
            return TextWriterWhereColor.RenderWhereColorBack(Convert.ToString(SpaceshipSymbol), SpaceshipLeft, ConsoleWrapper.WindowHeight - 1, false, ConsoleColors.Green, ConsoleColors.Black);
        }

        private static string DrawMeteors(int MeteorX, int MeteorY)
        {
            char MeteorSymbol = '*';
            return TextWriterWhereColor.RenderWhereColorBack(Convert.ToString(MeteorSymbol), MeteorX, MeteorY, false, ConsoleColors.White, ConsoleColors.Black);
        }

        private static string DrawBullet(int BulletX, int BulletY)
        {
            char BulletSymbol = '|';
            return TextWriterWhereColor.RenderWhereColorBack(Convert.ToString(BulletSymbol), BulletX, BulletY, false, ConsoleColors.Aqua, ConsoleColors.Black);
        }

        private static string DrawEnemy(int enemyX, int enemyY)
        {
            var enemyShape = new Canvas()
            {
                Left = enemyX,
                Top = enemyY,
                Width = 11,
                Height = 8,
                Pixels =
                [
                    // Shape is:
                    //      1 2 3 4 5 6 7 8 9 0 1
                    //     +----------------------+
                    //   1 |    **          **    |
                    //   2 |      **      **      |
                    //   3 |    **************    |
                    //   4 |  ****  ******  ****  |
                    //   5 |**********************|
                    //   6 |**  **************  **|
                    //   7 |**  **          **  **|
                    //   8 |      ****  ****      |
                    //     +----------------------+
                    new(3, 1) { CellColor = ConsoleColors.Aqua },
                    new(9, 1) { CellColor = ConsoleColors.Aqua },

                    new(4, 2) { CellColor = ConsoleColors.Aqua },
                    new(8, 2) { CellColor = ConsoleColors.Aqua },

                    new(3, 3) { CellColor = ConsoleColors.Aqua },
                    new(4, 3) { CellColor = ConsoleColors.Aqua },
                    new(5, 3) { CellColor = ConsoleColors.Aqua },
                    new(6, 3) { CellColor = ConsoleColors.Aqua },
                    new(7, 3) { CellColor = ConsoleColors.Aqua },
                    new(8, 3) { CellColor = ConsoleColors.Aqua },
                    new(9, 3) { CellColor = ConsoleColors.Aqua },

                    new(2, 4) { CellColor = ConsoleColors.Aqua },
                    new(3, 4) { CellColor = ConsoleColors.Aqua },
                    new(5, 4) { CellColor = ConsoleColors.Aqua },
                    new(6, 4) { CellColor = ConsoleColors.Aqua },
                    new(7, 4) { CellColor = ConsoleColors.Aqua },
                    new(9, 4) { CellColor = ConsoleColors.Aqua },
                    new(10, 4) { CellColor = ConsoleColors.Aqua },

                    new(1, 5) { CellColor = ConsoleColors.Aqua },
                    new(2, 5) { CellColor = ConsoleColors.Aqua },
                    new(3, 5) { CellColor = ConsoleColors.Aqua },
                    new(4, 5) { CellColor = ConsoleColors.Aqua },
                    new(5, 5) { CellColor = ConsoleColors.Aqua },
                    new(6, 5) { CellColor = ConsoleColors.Aqua },
                    new(7, 5) { CellColor = ConsoleColors.Aqua },
                    new(8, 5) { CellColor = ConsoleColors.Aqua },
                    new(9, 5) { CellColor = ConsoleColors.Aqua },
                    new(10, 5) { CellColor = ConsoleColors.Aqua },
                    new(11, 5) { CellColor = ConsoleColors.Aqua },

                    new(1, 6) { CellColor = ConsoleColors.Aqua },
                    new(3, 6) { CellColor = ConsoleColors.Aqua },
                    new(4, 6) { CellColor = ConsoleColors.Aqua },
                    new(5, 6) { CellColor = ConsoleColors.Aqua },
                    new(6, 6) { CellColor = ConsoleColors.Aqua },
                    new(7, 6) { CellColor = ConsoleColors.Aqua },
                    new(8, 6) { CellColor = ConsoleColors.Aqua },
                    new(9, 6) { CellColor = ConsoleColors.Aqua },
                    new(11, 6) { CellColor = ConsoleColors.Aqua },

                    new(1, 7) { CellColor = ConsoleColors.Aqua },
                    new(3, 7) { CellColor = ConsoleColors.Aqua },
                    new(9, 7) { CellColor = ConsoleColors.Aqua },
                    new(11, 7) { CellColor = ConsoleColors.Aqua },

                    new(4, 8) { CellColor = ConsoleColors.Aqua },
                    new(5, 8) { CellColor = ConsoleColors.Aqua },
                    new(7, 8) { CellColor = ConsoleColors.Aqua },
                    new(8, 8) { CellColor = ConsoleColors.Aqua },
                ]
            };
            return enemyShape.Render();
        }
    }
}
