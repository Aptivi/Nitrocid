//
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
using KS.Drivers.RNG;
using KS.Languages;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase;
using KS.Kernel.Threading;
using KS.ConsoleBase.Writers.ConsoleWriters;
using Terminaux.Colors;
using KS.Misc.Screensaver;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    /// <summary>
    /// Meteor shooter game module
    /// </summary>
    public static class MeteorShooter
    {

        internal readonly static KernelThread MeteorDrawThread = new("Meteor Shooter Draw Thread", true, DrawGame);
        internal static bool GameEnded = false;
        internal static bool GameExiting = false;
        internal static int meteorSpeed = 10;
        private static int SpaceshipHeight = 0;
        private readonly static int MaxBullets = 10;
        private readonly static List<(int, int)> Bullets = new();
        private readonly static int MaxMeteors = 10;
        private readonly static List<(int, int)> Meteors = new();

        /// <summary>
        /// Use PowerLine characters for the spaceship?
        /// </summary>
        public static bool MeteorUsePowerLine =>
            AmusementsInit.AmusementsConfig.MeteorUsePowerLine;
        /// <summary>
        /// Meteor speed in milliseconds
        /// </summary>
        public static int MeteorSpeed
        {
            get => AmusementsInit.AmusementsConfig.MeteorSpeed;
            set => AmusementsInit.AmusementsConfig.MeteorSpeed = value < 0 ? 10 : value;
        }

        /// <summary>
        /// Initializes the Meteor game
        /// </summary>
        public static void InitializeMeteor(bool simulation = false)
        {
            // Clear screen
            ConsoleWrapper.Clear();

            // Clear all bullets and meteors
            Bullets.Clear();
            Meteors.Clear();

            // Make the spaceship height in the center
            SpaceshipHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);

            // Start the draw thread
            MeteorDrawThread.Stop();
            MeteorDrawThread.Start();

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
                        Keypress = Input.DetectKeypress();
                        HandleKeypress(Keypress.Key);
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
                    HandleKeypress(Keypress);
                    ThreadManager.SleepNoBlock(100, ScreensaverDisplayer.ScreensaverDisplayerThread);
                }
            }

            // Stop the draw thread since the game ended
            MeteorDrawThread.Wait();
            MeteorDrawThread.Stop();
            GameEnded = false;
            GameExiting = false;
        }

        private static void HandleKeypress(ConsoleKey Keypress)
        {
            switch (Keypress)
            {
                case ConsoleKey.UpArrow:
                    if (SpaceshipHeight > 0)
                        SpaceshipHeight -= 1;
                    break;
                case ConsoleKey.DownArrow:
                    if (SpaceshipHeight < ConsoleWrapper.WindowHeight - 1)
                        SpaceshipHeight += 1;
                    break;
                case ConsoleKey.Spacebar:
                    if (Bullets.Count < MaxBullets)
                        Bullets.Add((1, SpaceshipHeight));
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
                    // Clear only the relevant parts
                    for (int y = 0; y < ConsoleWrapper.WindowHeight; y++)
                    {
                        if (y != SpaceshipHeight)
                            TextWriterWhereColor.WriteWhere(" ", 0, y);
                    }

                    // Move the meteors left
                    for (int Meteor = 0; Meteor <= Meteors.Count - 1; Meteor++)
                    {
                        TextWriterWhereColor.WriteWhere(" ", Meteors[Meteor].Item1, Meteors[Meteor].Item2);
                        int MeteorX = Meteors[Meteor].Item1 - 1;
                        int MeteorY = Meteors[Meteor].Item2;
                        Meteors[Meteor] = (MeteorX, MeteorY);
                    }

                    // Move the bullets right
                    for (int Bullet = 0; Bullet <= Bullets.Count - 1; Bullet++)
                    {
                        TextWriterWhereColor.WriteWhere(" ", Bullets[Bullet].Item1, Bullets[Bullet].Item2);
                        int BulletX = Bullets[Bullet].Item1 + 1;
                        int BulletY = Bullets[Bullet].Item2;
                        Bullets[Bullet] = (BulletX, BulletY);
                    }

                    // If any bullet is out of X range, delete it
                    for (int BulletIndex = Bullets.Count - 1; BulletIndex >= 0; BulletIndex -= 1)
                    {
                        var Bullet = Bullets[BulletIndex];
                        if (Bullet.Item1 >= ConsoleWrapper.WindowWidth)
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
                        int MeteorX = ConsoleWrapper.WindowWidth - 1;
                        int MeteorY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight - 1);
                        Meteors.Add((MeteorX, MeteorY));
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
                                TextWriterWhereColor.WriteWhere(" ", Meteor.Item1, Meteor.Item2);
                                TextWriterWhereColor.WriteWhere(" ", Bullet.Item1, Bullet.Item2);
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
                GameExiting = true;
            }
            // Game is over. Move to the Finally block.
            catch (Exception ex)
            {
                // Game is over with an unexpected error.
                TextWriterWhereColor.WriteWhereColor(Translate.DoTranslation("Unexpected error") + ": {0}", 0, ConsoleWrapper.WindowHeight - 1, false, ConsoleColors.Red, vars: ex.Message);
                ThreadManager.SleepNoBlock(3000L, MeteorDrawThread);
                GameExiting = true;
                ConsoleWrapper.Clear();
            }
            finally
            {
                // Write game over if not exiting
                if (!GameExiting)
                {
                    TextWriterWhereColor.WriteWhereColor(Translate.DoTranslation("Game over"), 0, ConsoleWrapper.WindowHeight - 1, false, ConsoleColors.Red);
                    ThreadManager.SleepNoBlock(3000L, MeteorDrawThread);
                }
                ConsoleWrapper.Clear();
            }
        }

        private static void DrawSpaceship()
        {
            char PowerLineSpaceship = Convert.ToChar(0xE0B0);
            char SpaceshipSymbol = MeteorUsePowerLine ? PowerLineSpaceship : '>';
            TextWriterWhereColor.WriteWhereColor(Convert.ToString(SpaceshipSymbol), 0, SpaceshipHeight, false, ConsoleColors.Green);
        }

        private static void DrawMeteor(int MeteorX, int MeteorY)
        {
            char MeteorSymbol = '*';
            TextWriterWhereColor.WriteWhereColor(Convert.ToString(MeteorSymbol), MeteorX, MeteorY, false, ConsoleColors.Red);
        }

        private static void DrawBullet(int BulletX, int BulletY)
        {
            char BulletSymbol = '-';
            TextWriterWhereColor.WriteWhereColor(Convert.ToString(BulletSymbol), BulletX, BulletY, false, ConsoleColors.Cyan);
        }

    }
}
