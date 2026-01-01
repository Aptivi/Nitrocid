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
using System.Threading;
using Terminaux.Colors;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Configuration;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using System.Text;
using Terminaux.Sequences.Builder.Types;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    static class Pong
    {

        public static void InitializePong()
        {
            // Clear the screen
            ConsoleWrapper.CursorVisible = false;
            KernelColorTools.LoadBackground();

            // Variables
            var Direction = Enum.Parse<BallDirection>(RandomDriver.Random(3).ToString());
            int p1Score = 0;
            int p2Score = 0;

            // Add a buffer
            Screen screen = new();
            ScreenPart part = new();

            // Get the field color ready
            int RedColorNum = RandomDriver.Random(0, 255);
            int GreenColorNum = RandomDriver.Random(0, 255);
            int BlueColorNum = RandomDriver.Random(0, 255);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
            var fieldColor = new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}");

            // Draw the field
            part.AddDynamicText(() =>
            {
                StringBuilder field = new(ColorTools.RenderSetConsoleColor(fieldColor, true));

                int FieldTopLeftEdge = 3;
                int FieldBottomLeftEdge = 3;
                DebugWriter.WriteDebug(DebugLevel.I, "Top left edge: {0}, Bottom left edge: {1}", vars: [FieldTopLeftEdge, FieldBottomLeftEdge]);

                int FieldTopRightEdge = ConsoleWrapper.WindowWidth - 3;
                int FieldBottomRightEdge = ConsoleWrapper.WindowWidth - 3;
                DebugWriter.WriteDebug(DebugLevel.I, "Top right edge: {0}, Bottom right edge: {1}", vars: [FieldTopRightEdge, FieldBottomRightEdge]);

                int FieldTopEdge = 3;
                int FieldBottomEdge = ConsoleWrapper.WindowHeight - 2;
                DebugWriter.WriteDebug(DebugLevel.I, "Top edge: {0}, Bottom edge: {1}", vars: [FieldTopEdge, FieldBottomEdge]);

                int FieldLeftEdge = 2;
                int FieldRightEdge = ConsoleWrapper.WindowWidth - 4;
                DebugWriter.WriteDebug(DebugLevel.I, "Left edge: {0}, Right edge: {1}", vars: [FieldLeftEdge, FieldRightEdge]);

                // First, draw the field top edge
                for (int x = FieldTopLeftEdge; x <= FieldTopRightEdge; x++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Drawing field top edge ({0}, {1})", vars: [x, 1]);
                    field.Append(
                        CsiSequences.GenerateCsiCursorPosition(x + 1, FieldTopEdge + 1) +
                        " "
                    );
                }

                // Second, draw the field bottom edge
                for (int x = FieldBottomLeftEdge; x <= FieldBottomRightEdge; x++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Drawing field bottom edge ({0}, {1})", vars: [x, FieldBottomEdge]);
                    field.Append(
                        CsiSequences.GenerateCsiCursorPosition(x + 1, FieldBottomEdge + 1) +
                        " "
                    );
                }

                // Third, draw the field left edge
                for (int y = FieldTopEdge; y <= FieldBottomEdge; y++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Drawing field left edge ({0}, {1})", vars: [FieldLeftEdge, y]);
                    field.Append(
                        CsiSequences.GenerateCsiCursorPosition(FieldLeftEdge + 1, y + 1) +
                        "  "
                    );
                }

                // Finally, draw the field right edge
                for (int y = FieldTopEdge; y <= FieldBottomEdge; y++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Drawing field right edge ({0}, {1})", vars: [FieldRightEdge, y]);
                    field.Append(
                        CsiSequences.GenerateCsiCursorPosition(FieldRightEdge + 2, y + 1) +
                        "  "
                    );
                }

                // Now, print the scores
                var scores = new AlignedText()
                {
                    Text =
                        Translate.DoTranslation("Score") + $": {p1Score,3}" +
                        "     |     " +
                        Translate.DoTranslation("Score") + $": {p2Score,3}",
                    Top = 1,
                    ForegroundColor = KernelColorTools.GetColor(KernelColorType.NeutralText),
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    }
                };
                field.Append(
                    ColorTools.RenderRevertBackground() +
                    scores.Render()
                );

                // Render the result
                return field.ToString();
            });

            var ballColor = new Color(ConsoleColors.White);

            // The ball usually starts in the middle.
            int FloorTopEdge = 3;
            int FloorBottomEdge = ConsoleWrapper.WindowHeight - 2;
            int FloorLeftEdge = 3;
            int FloorRightEdge = ConsoleWrapper.WindowWidth - 3;
            DebugWriter.WriteDebug(DebugLevel.I, "Floor top edge {0}", vars: [FloorTopEdge]);
            DebugWriter.WriteDebug(DebugLevel.I, "Floor bottom edge {0}", vars: [FloorBottomEdge]);
            DebugWriter.WriteDebug(DebugLevel.I, "Floor left edge {0}", vars: [FloorLeftEdge]);
            DebugWriter.WriteDebug(DebugLevel.I, "Floor right edge {0}", vars: [FloorRightEdge]);

            int BallCurrentX = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            int BallCurrentY = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            DebugWriter.WriteDebug(DebugLevel.I, "Initial ball position ({0}, {1})", vars: [BallCurrentX, BallCurrentY]);

            bool Dead = false;
            bool bailing = false;
            int BallPreviousX = BallCurrentX;
            int BallPreviousY = BallCurrentY;
            var BallWipeX = 0;
            var BallWipeY = 0;
            int padLength = 3;
            int leftPadX = 5;
            int leftPadY = ConsoleWrapper.WindowHeight / 2;
            int rightPadX = FloorRightEdge - 2;
            int rightPadY = ConsoleWrapper.WindowHeight / 2;
            part.AddDynamicText(() =>
            {
                StringBuilder buffer = new();
                FloorBottomEdge = ConsoleWrapper.WindowHeight - 2;
                FloorRightEdge = ConsoleWrapper.WindowWidth - 3;

                // Remove positions
                buffer.Append(
                    CsiSequences.GenerateCsiCursorPosition(BallWipeX + 1, BallWipeY + 1) +
                    " "
                );
                for (int y = FloorTopEdge + 1; y <= FloorBottomEdge - 1; y++)
                {
                    buffer.Append(
                        CsiSequences.GenerateCsiCursorPosition(leftPadX + 1, y + 1) +
                        " " +
                        CsiSequences.GenerateCsiCursorPosition(rightPadX + 1, y + 1) +
                        " "
                    );
                }

                // Make a ball
                buffer.Append(
                    ColorTools.RenderSetConsoleColor(ballColor, true) +
                    CsiSequences.GenerateCsiCursorPosition(BallCurrentX + 1, BallCurrentY + 1) +
                    " "
                );
                DebugWriter.WriteDebug(DebugLevel.I, "Drawn ball at ({0}, {1})", vars: [BallCurrentX, BallCurrentY]);

                // Draw two pads
                for (int y = leftPadY - padLength; y <= leftPadY + padLength; y++)
                {
                    buffer.Append(
                        CsiSequences.GenerateCsiCursorPosition(leftPadX + 1, y + 1) +
                        " "
                    );
                }
                DebugWriter.WriteDebug(DebugLevel.I, "Drawn left pad at ({0}, {1})", vars: [leftPadX, leftPadY]);
                for (int y = rightPadY - padLength; y <= rightPadY + padLength; y++)
                {
                    buffer.Append(
                        CsiSequences.GenerateCsiCursorPosition(rightPadX + 1, y + 1) +
                        " "
                    );
                }
                DebugWriter.WriteDebug(DebugLevel.I, "Drawn right pad at ({0}, {1})", vars: [leftPadX, leftPadY]);

                // Return the result
                return buffer.ToString();
            });
            screen.AddBufferedPart("Pong - UI", part);
            ScreenTools.SetCurrent(screen);

            // Main loop
            double factor = 1.0;
            while (!Dead && !bailing)
            {
                // Delay
                SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable, (int)(100 * factor));
                ScreenTools.Render();

                // Move the ball according to the mode
                if (ConsoleWrapper.KeyAvailable)
                {
                    var Pressed = Input.ReadKey().Key;
                    switch (Pressed)
                    {
                        case ConsoleKey.DownArrow:
                            if (leftPadY + padLength + 1 >= FloorBottomEdge)
                                break;
                            leftPadY += 1;
                            break;
                        case ConsoleKey.UpArrow:
                            if (leftPadY - padLength - 1 <= FloorTopEdge)
                                break;
                            leftPadY -= 1;
                            break;
                        case ConsoleKey.S:
                            if (rightPadY + padLength + 1 >= FloorBottomEdge)
                                break;
                            rightPadY += 1;
                            break;
                        case ConsoleKey.W:
                            if (rightPadY - padLength - 1 <= FloorTopEdge)
                                break;
                            rightPadY -= 1;
                            break;
                        case ConsoleKey.Escape:
                            bailing = true;
                            break;
                    }
                }

                // Increase or decrease the ball current position
                BallWipeX = BallCurrentX;
                BallWipeY = BallCurrentY;
                switch (Direction)
                {
                    case BallDirection.LeftUp:
                        BallCurrentX -= 1;
                        BallCurrentY -= 1;
                        DebugWriter.WriteDebug(DebugLevel.I, "Increased vertical ball position from {0} to {1}", vars: [BallPreviousY, BallCurrentY]);
                        break;
                    case BallDirection.RightUp:
                        BallCurrentX += 1;
                        BallCurrentY -= 1;
                        DebugWriter.WriteDebug(DebugLevel.I, "Decreased vertical ball position from {0} to {1}", vars: [BallPreviousY, BallCurrentY]);
                        break;
                    case BallDirection.LeftDown:
                        BallCurrentX -= 1;
                        BallCurrentY += 1;
                        DebugWriter.WriteDebug(DebugLevel.I, "Decreased horizontal ball position from {0} to {1}", vars: [BallPreviousX, BallCurrentX]);
                        break;
                    case BallDirection.RightDown:
                        BallCurrentX += 1;
                        BallCurrentY += 1;
                        DebugWriter.WriteDebug(DebugLevel.I, "Increased horizontal ball position from {0} to {1}", vars: [BallPreviousX, BallCurrentX]);
                        break;
                }
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Ball is facing {0}.", vars: [Direction.ToString()]);
                
                // Determine whether the ball has hit the pad or the top and bottom edges
                if (BallCurrentY <= FloorTopEdge + 1)
                {
                    if (Direction == BallDirection.LeftUp)
                        Direction = BallDirection.LeftDown;
                    else
                        Direction = BallDirection.RightDown;
                }
                if (BallCurrentY >= FloorBottomEdge - 1)
                {
                    if (Direction == BallDirection.LeftDown)
                        Direction = BallDirection.LeftUp;
                    else
                        Direction = BallDirection.RightUp;
                }
                if (BallCurrentX == leftPadX + 1 && BallCurrentY <= leftPadY + padLength && BallCurrentY >= leftPadY - padLength)
                {
                    if (Direction == BallDirection.LeftDown)
                        Direction = BallDirection.RightDown;
                    else
                        Direction = BallDirection.RightUp;

                    // Decrease the factor if needed
                    factor -= 0.01;
                    if (factor < 0.25)
                        factor = 0.25;

                    // Increase score
                    p1Score++;
                }
                if (BallCurrentX == rightPadX - 1 && BallCurrentY <= rightPadY + padLength && BallCurrentY >= rightPadY - padLength)
                {
                    if (Direction == BallDirection.RightDown)
                        Direction = BallDirection.LeftDown;
                    else
                        Direction = BallDirection.LeftUp;

                    // Decrease the factor if needed
                    factor -= 0.01;
                    if (factor < 0.25)
                        factor = 0.25;

                    // Increase score
                    p2Score++;
                }

                // If the ball reached the left or the right edge without hitting the pad, the game is over.
                if (BallCurrentX <= FloorLeftEdge || BallCurrentX >= FloorRightEdge)
                    Dead = true;

                // Set the previous positions
                BallPreviousX = BallCurrentX;
                BallPreviousY = BallCurrentY;
            }
            ScreenTools.Render();

            // Show the stage for few seconds before wiping
            if (Dead)
                Thread.Sleep(5000);

            // Reset console display and screen
            ScreenTools.UnsetCurrent(screen);
            KernelColorTools.LoadBackground();
            ConsoleResizeHandler.WasResized();
        }

        public enum BallDirection
        {
            LeftUp,
            RightUp,
            LeftDown,
            RightDown
        }

    }
}
