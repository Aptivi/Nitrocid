
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Threading;
using ColorSeq;
using KS.Drivers.RNG;
using KS.Languages;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.ConsoleBase.Inputs;
using KS.Kernel.Configuration;

namespace KS.Misc.Games
{
    /// <summary>
    /// Meteor shooter game module
    /// </summary>
    public static class MeteorShooter
    {

        internal readonly static KernelThread MeteorDrawThread = new("Meteor Shooter Draw Thread", true, DrawGame);
        internal static bool GameEnded = false;
        internal static int meteorSpeed = 10;
        private static int SpaceshipHeight = 0;
        private readonly static int MaxBullets = 10;
        private readonly static List<Tuple<int, int>> Bullets = new();
        private readonly static int MaxMeteors = 10;
        private readonly static List<Tuple<int, int>> Meteors = new();

        /// <summary>
        /// Use PowerLine characters for the spaceship?
        /// </summary>
        public static bool MeteorUsePowerLine =>
            Config.MainConfig.MeteorUsePowerLine;
        /// <summary>
        /// Meteor speed in milliseconds
        /// </summary>
        public static int MeteorSpeed =>
            Config.MainConfig.MeteorSpeed;

        /// <summary>
        /// Initializes the Meteor game
        /// </summary>
        public static void InitializeMeteor(bool simulation = false)
        {
            // Clear all bullets and meteors
            Bullets.Clear();
            Meteors.Clear();

            // Make the spaceship height in the center
            SpaceshipHeight = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowHeight / 2d);

            // Start the draw thread
            MeteorDrawThread.Stop();
            MeteorDrawThread.Start();

            // Remove the cursor
            ConsoleBase.ConsoleWrapper.CursorVisible = false;

            // Now, handle input or simulate keypresses
            if (!simulation)
            {
                // Player mode
                ConsoleKeyInfo Keypress;
                while (!GameEnded)
                {
                    if (ConsoleBase.ConsoleWrapper.KeyAvailable)
                    {
                        // Read the key
                        Keypress = Input.DetectKeypress();

                        // Select command based on key value
                        switch (Keypress.Key)
                        {
                            case ConsoleKey.UpArrow:
                                {
                                    if (SpaceshipHeight > 0)
                                        SpaceshipHeight -= 1;
                                    break;
                                }
                            case ConsoleKey.DownArrow:
                                {
                                    if (SpaceshipHeight < ConsoleBase.ConsoleWrapper.WindowHeight)
                                        SpaceshipHeight += 1;
                                    break;
                                }
                            case ConsoleKey.Spacebar:
                                {
                                    if (Bullets.Count < MaxBullets)
                                        Bullets.Add(new Tuple<int, int>(1, SpaceshipHeight));
                                    break;
                                }
                            case ConsoleKey.Escape:
                                {
                                    GameEnded = true;
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                // Simulation mode
                ConsoleKey Keypress = 0;
                ConsoleKey[] possibleKeys = new[] { ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.Spacebar };
                while (!GameEnded)
                {
                    float PossibilityToChange = (float)RandomDriver.RandomDouble();
                    if ((int)Math.Round(PossibilityToChange) == 1)
                        Keypress = possibleKeys[RandomDriver.RandomIdx(possibleKeys.Length)];

                    // Select command based on key value
                    switch (Keypress)
                    {
                        case ConsoleKey.UpArrow:
                            {
                                if (SpaceshipHeight > 0)
                                    SpaceshipHeight -= 1;
                                break;
                            }
                        case ConsoleKey.DownArrow:
                            {
                                if (SpaceshipHeight < ConsoleBase.ConsoleWrapper.WindowHeight)
                                    SpaceshipHeight += 1;
                                break;
                            }
                        case ConsoleKey.Spacebar:
                            {
                                if (Bullets.Count < MaxBullets)
                                    Bullets.Add(new Tuple<int, int>(1, SpaceshipHeight));
                                break;
                            }
                    }
                    ThreadManager.SleepNoBlock(100, Screensaver.ScreensaverDisplayer.ScreensaverDisplayerThread);
                }
            }

            // Stop the draw thread since the game ended
            MeteorDrawThread.Wait();
            MeteorDrawThread.Stop();
            GameEnded = false;
        }

        private static void DrawGame()
        {
            try
            {
                while (!GameEnded)
                {
                    // Clear screen
                    ConsoleBase.ConsoleWrapper.Clear();

                    // Move the meteors left
                    for (int Meteor = 0; Meteor <= Meteors.Count - 1; Meteor++)
                    {
                        int MeteorX = Meteors[Meteor].Item1 - 1;
                        int MeteorY = Meteors[Meteor].Item2;
                        Meteors[Meteor] = new Tuple<int, int>(MeteorX, MeteorY);
                    }

                    // Move the bullets right
                    for (int Bullet = 0; Bullet <= Bullets.Count - 1; Bullet++)
                    {
                        int BulletX = Bullets[Bullet].Item1 + 1;
                        int BulletY = Bullets[Bullet].Item2;
                        Bullets[Bullet] = new Tuple<int, int>(BulletX, BulletY);
                    }

                    // If any bullet is out of X range, delete it
                    for (int BulletIndex = Bullets.Count - 1; BulletIndex >= 0; BulletIndex -= 1)
                    {
                        var Bullet = Bullets[BulletIndex];
                        if (Bullet.Item1 >= ConsoleBase.ConsoleWrapper.WindowWidth)
                        {
                            // The bullet went beyond. Remove it.
                            Bullets.RemoveAt(BulletIndex);
                        }
                    }

                    // If any meteor is out of X range, delete it
                    for (int MeteorIndex = Meteors.Count - 1; MeteorIndex >= 0; MeteorIndex -= 1)
                    {
                        var Meteor = Meteors[MeteorIndex];
                        if (Meteor.Item1 < 0)
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
                        int MeteorX = ConsoleBase.ConsoleWrapper.WindowWidth - 1;
                        int MeteorY = RandomDriver.RandomIdx(ConsoleBase.ConsoleWrapper.WindowHeight - 1);
                        Meteors.Add(new Tuple<int, int>(MeteorX, MeteorY));
                    }

                    // Draw the meteor, the bullet, and the spaceship if any of them are updated
                    DrawSpaceship();
                    for (int MeteorIndex = Meteors.Count - 1; MeteorIndex >= 0; MeteorIndex -= 1)
                    {
                        var Meteor = Meteors[MeteorIndex];
                        DrawMeteor(Meteor.Item1, Meteor.Item2);
                    }
                    for (int BulletIndex = Bullets.Count - 1; BulletIndex >= 0; BulletIndex -= 1)
                    {
                        var Bullet = Bullets[BulletIndex];
                        DrawBullet(Bullet.Item1, Bullet.Item2);
                    }

                    // Check to see if the spaceship is blown up
                    for (int MeteorIndex = Meteors.Count - 1; MeteorIndex >= 0; MeteorIndex -= 1)
                    {
                        var Meteor = Meteors[MeteorIndex];
                        if (Meteor.Item1 == 0 & Meteor.Item2 == SpaceshipHeight)
                        {
                            // The spaceship crashed! Game ended.
                            GameEnded = true;
                        }
                    }

                    // Check to see if the meteor is blown up
                    for (int MeteorIndex = Meteors.Count - 1; MeteorIndex >= 0; MeteorIndex -= 1)
                    {
                        var Meteor = Meteors[MeteorIndex];
                        for (int BulletIndex = Bullets.Count - 1; BulletIndex >= 0; BulletIndex -= 1)
                        {
                            var Bullet = Bullets[BulletIndex];
                            if (Meteor.Item1 <= Bullet.Item1 & Meteor.Item2 == Bullet.Item2)
                            {
                                // The meteor crashed! Remove both the bullet and the meteor
                                Bullets.RemoveAt(BulletIndex);
                                Meteors.RemoveAt(MeteorIndex);
                            }
                        }
                    }

                    // Wait for a few milliseconds
                    ThreadManager.SleepNoBlock(MeteorSpeed, MeteorDrawThread);
                }
            }
            catch (ThreadInterruptedException)
            {
            }
            // Game is over. Move to the Finally block.
            catch (Exception ex)
            {
                // Game is over with an unexpected error.
                TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Unexpected error") + ": {0}", 0, ConsoleBase.ConsoleWrapper.WindowHeight - 1, false, ConsoleColors.Red, ex.Message);
                ThreadManager.SleepNoBlock(3000L, MeteorDrawThread);
                ConsoleBase.ConsoleWrapper.Clear();
            }
            finally
            {
                // Write game over
                TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Game over"), 0, ConsoleBase.ConsoleWrapper.WindowHeight - 1, false, ConsoleColors.Red);
                ThreadManager.SleepNoBlock(3000L, MeteorDrawThread);
                ConsoleBase.ConsoleWrapper.Clear();
            }
        }

        private static void DrawSpaceship()
        {
            char PowerLineSpaceship = Convert.ToChar(0xE0B0);
            char SpaceshipSymbol = MeteorUsePowerLine ? PowerLineSpaceship : '>';
            TextWriterWhereColor.WriteWhere(Convert.ToString(SpaceshipSymbol), 0, SpaceshipHeight, false, ConsoleColors.Green);
        }

        private static void DrawMeteor(int MeteorX, int MeteorY)
        {
            char MeteorSymbol = '*';
            TextWriterWhereColor.WriteWhere(Convert.ToString(MeteorSymbol), MeteorX, MeteorY, false, ConsoleColors.Red);
        }

        private static void DrawBullet(int BulletX, int BulletY)
        {
            char BulletSymbol = '-';
            TextWriterWhereColor.WriteWhere(Convert.ToString(BulletSymbol), BulletX, BulletY, false, ConsoleColors.Cyan);
        }

    }
}
