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
using Terminaux.Base.Extensions;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    /// <summary>
    /// Meteor shooter game module
    /// </summary>
    public static class MeteorShooter
    {

        internal readonly static KernelThread MeteorDrawThread = new("Meteor Shooter Draw Thread", true, (dodge) => DrawGame((bool?)dodge ?? true));
        internal static bool GameEnded = false;
        internal static bool GameExiting = false;
        internal static int meteorSpeed = 10;
        private static int SpaceshipHeight = 0;
        private static int score = 0;
        private readonly static int MaxBullets = 10;
        private readonly static List<(int, int)> Bullets = [];
        private readonly static int MaxMeteors = 10;
        private readonly static List<(int, int)> Meteors = [];

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
        /// <param name="simulation">Whether to make the player a CPU or not</param>
        /// <param name="dodge">Whether to disable shooting to enable dodge mode or not</param>
        public static void InitializeMeteor(bool simulation = false, bool dodge = false)
        {
            // Clear screen
            ConsoleColoring.LoadBackDry(0);

            // Clear all bullets and meteors
            Bullets.Clear();
            Meteors.Clear();

            // Reset the score
            score = 0;

            // Make the spaceship height in the center
            SpaceshipHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);

            // Start the draw thread
            MeteorDrawThread.Stop();
            MeteorDrawThread.Start(dodge);

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
                        HandleKeypress(Keypress.Key, dodge);
                    }
                }
            }
            else
            {
                // Simulation mode
                ConsoleKey Keypress = 0;
                ConsoleKey[] possibleKeys =
                    dodge ?
                    [ConsoleKey.UpArrow, ConsoleKey.DownArrow] :
                    [ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.Spacebar];
                while (!GameEnded)
                {
                    float PossibilityToChange = (float)RandomDriver.RandomDouble();
                    if ((int)Math.Round(PossibilityToChange) == 1)
                        Keypress = possibleKeys[RandomDriver.RandomIdx(possibleKeys.Length)];

                    // Select command based on key value
                    HandleKeypress(Keypress, dodge);
                    ScreensaverManager.Delay(100);
                }
            }

            // Stop the draw thread since the game ended
            MeteorDrawThread.Wait();
            MeteorDrawThread.Stop();
            GameEnded = false;
            GameExiting = false;
        }

        private static void HandleKeypress(ConsoleKey Keypress, bool dodge)
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
                    if (dodge)
                        return;

                    // Shoot the bullet!
                    if (Bullets.Count < MaxBullets)
                        Bullets.Add((1, SpaceshipHeight));
                    break;
                case ConsoleKey.Escape:
                    GameEnded = true;
                    GameExiting = true;
                    break;
            }
        }

        private static void DrawGame(bool dodge)
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
                        if (y != SpaceshipHeight)
                            buffer.Append(
                                ConsoleColoring.RenderSetConsoleColor(new Color(ConsoleColors.Black), true) +
                                TextWriterWhereColor.RenderWhere(" ", 0, y)
                            );
                    }

                    // Show the score
                    buffer.Append(
                        new Color(ConsoleColors.Green).VTSequenceForeground() +
                        TextWriterWhereColor.RenderWhere($"{score}", ConsoleWrapper.WindowWidth - $"{score}".Length, 0)
                    );

                    // Move the meteors left
                    for (int Meteor = 0; Meteor <= Meteors.Count - 1; Meteor++)
                    {
                        buffer.Append(ConsoleColoring.RenderSetConsoleColor(new Color(ConsoleColors.Black), true));
                        buffer.Append(TextWriterWhereColor.RenderWhere(" ", Meteors[Meteor].Item1, Meteors[Meteor].Item2));
                        int MeteorX = Meteors[Meteor].Item1 - 1;
                        int MeteorY = Meteors[Meteor].Item2;
                        Meteors[Meteor] = (MeteorX, MeteorY);
                    }

                    // Move the bullets right
                    for (int Bullet = 0; Bullet <= Bullets.Count - 1; Bullet++)
                    {
                        buffer.Append(ConsoleColoring.RenderSetConsoleColor(new Color(ConsoleColors.Black), true));
                        buffer.Append(TextWriterWhereColor.RenderWhere(" ", Bullets[Bullet].Item1, Bullets[Bullet].Item2));
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

                        // If on dodge mode, increase score for every draw
                        if (dodge)
                            score++;
                    }

                    // Draw the meteor, the bullet, and the spaceship if any of them are updated
                    buffer.Append(DrawSpaceship());
                    for (int MeteorIndex = Meteors.Count - 1; MeteorIndex >= 0; MeteorIndex -= 1)
                    {
                        var Meteor = Meteors[MeteorIndex];
                        buffer.Append(DrawMeteor(Meteor.Item1, Meteor.Item2));
                    }
                    for (int BulletIndex = Bullets.Count - 1; BulletIndex >= 0; BulletIndex -= 1)
                    {
                        var Bullet = Bullets[BulletIndex];
                        buffer.Append(DrawBullet(Bullet.Item1, Bullet.Item2));
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
                                buffer.Append(ConsoleColoring.RenderSetConsoleColor(new Color(ConsoleColors.Black), true));
                                buffer.Append(TextWriterWhereColor.RenderWhere(" ", Meteor.Item1, Meteor.Item2));
                                buffer.Append(TextWriterWhereColor.RenderWhere(" ", Bullet.Item1, Bullet.Item2));
                                Bullets.RemoveAt(BulletIndex);
                                Meteors.RemoveAt(MeteorIndex);
                                score++;
                            }
                        }
                    }

                    // Wait for a few milliseconds
                    TextWriterRaw.WritePlain(buffer.ToString(), false);
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
                try
                {
                    TextWriterWhereColor.WriteWhereColor(LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMON_UNEXPECTEDERROR") + ": {0}", 0, ConsoleWrapper.WindowHeight - 1, false, ConsoleColors.Red, vars: ex.Message);
                    ThreadManager.SleepNoBlock(3000L, MeteorDrawThread);
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
                // Write game over if not exiting
                if (!GameExiting)
                {
                    try
                    {
                        TextWriterWhereColor.WriteWhereColor(LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMON_GAMEOVER"), 0, ConsoleWrapper.WindowHeight - 1, false, ConsoleColors.Red);
                        ThreadManager.SleepNoBlock(3000L, MeteorDrawThread);
                    }
                    catch
                    {
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.E, "Can't display game over on meteor shooter.");
                    }
                }
                ConsoleWrapper.Clear();
            }
        }

        private static string DrawSpaceship()
        {
            char PowerLineSpaceship = Convert.ToChar(0xE0B0);
            char SpaceshipSymbol = MeteorUsePowerLine ? PowerLineSpaceship : '>';
            return TextWriterWhereColor.RenderWhereColorBack(Convert.ToString(SpaceshipSymbol), 0, SpaceshipHeight, false, ConsoleColors.Green, ConsoleColors.Black);
        }

        private static string DrawMeteor(int MeteorX, int MeteorY)
        {
            char MeteorSymbol = '*';
            return TextWriterWhereColor.RenderWhereColorBack(Convert.ToString(MeteorSymbol), MeteorX, MeteorY, false, ConsoleColors.Red, ConsoleColors.Black);
        }

        private static string DrawBullet(int BulletX, int BulletY)
        {
            char BulletSymbol = '-';
            return TextWriterWhereColor.RenderWhereColorBack(Convert.ToString(BulletSymbol), BulletX, BulletY, false, ConsoleColors.Aqua, ConsoleColors.Black);
        }

    }
}
