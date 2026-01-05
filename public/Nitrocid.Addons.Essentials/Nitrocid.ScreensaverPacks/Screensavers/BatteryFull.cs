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

using System.Text;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Gradients;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for BatteryFull
    /// </summary>
    public class BatteryFullDisplay : BaseScreensaver, IScreensaver
    {
        private int batteryLevel = 100;
        private int batteryCycles = 0;
        private int batteryDegradedCycles = 0;
        private int batteryDrainSpeed = 500;
        private int batteryChargeSpeed = 250;
        private bool batteryCharging = false;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "BatteryFull";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            batteryLevel = 100;
            batteryCycles = 0;
            batteryDegradedCycles = 0;
            batteryDrainSpeed = 500;
            batteryChargeSpeed = 250;
            batteryCharging = false;
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Print battery life information
            int batteryLife = 100 - batteryCycles;
            var gradientsLevel = ColorGradients.StageLevelSmooth(ConsoleColors.Red, ConsoleColors.Yellow, ConsoleColors.Lime, batteryLevel / 100d);
            var gradientsLife = ColorGradients.StageLevelSmooth(ConsoleColors.Red, ConsoleColors.Yellow, ConsoleColors.Lime, batteryLife / 100d);
            var gradientsCycles = ColorGradients.StageLevelSmooth(ConsoleColors.Lime, ConsoleColors.Yellow, ConsoleColors.Red, batteryCycles, 0, 75, false, 1, 50);
            var gradientsDegradedCycles = batteryDegradedCycles == 0 ? ConsoleColors.Lime : ConsoleColors.Red;
            var gradientsDrainSpeed = ColorGradients.StageLevelSmooth(ConsoleColors.Red, ConsoleColors.Yellow, ConsoleColors.Lime, batteryDrainSpeed, 0, 500, false, 150, 250);
            var gradientsChargeSpeed = ColorGradients.StageLevelSmooth(ConsoleColors.Lime, ConsoleColors.Yellow, ConsoleColors.Red, batteryChargeSpeed, 0, 250, false, 125, 175);
            string batteryLifeDesc =
                $"Le: {ColorTools.RenderSetConsoleColor(gradientsLevel)}{batteryLevel}%{ColorTools.RenderRevertForeground()}, " +
                $"Li: {ColorTools.RenderSetConsoleColor(gradientsLife)}{batteryLife}%{ColorTools.RenderRevertForeground()}, " +
                $"Cy: {ColorTools.RenderSetConsoleColor(gradientsCycles)}{batteryCycles}{ColorTools.RenderRevertForeground()}, " +
                $"De: {ColorTools.RenderSetConsoleColor(gradientsDegradedCycles)}{batteryDegradedCycles}{ColorTools.RenderRevertForeground()}, " +
                $"Ds: {ColorTools.RenderSetConsoleColor(gradientsDrainSpeed)}{batteryDrainSpeed} ms{ColorTools.RenderRevertForeground()}, " +
                $"Ch: {ColorTools.RenderSetConsoleColor(gradientsChargeSpeed)}{batteryChargeSpeed} ms{ColorTools.RenderRevertForeground()}";
            var builder = new StringBuilder();
            var batteryLifeDescEraser = new Eraser()
            {
                Left = 0,
                Top = ConsoleWrapper.WindowHeight - 2,
                Width = ConsoleWrapper.WindowWidth,
                Height = 1,
            };
            var batteryLifeDescRenderer = new BoundedText()
            {
                Left = 0,
                Top = ConsoleWrapper.WindowHeight - 2,
                Width = ConsoleWrapper.WindowWidth,
                Height = 1,
                Text = batteryLifeDesc,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle
                }
            };
            builder.Append(batteryLifeDescEraser.Render() + batteryLifeDescRenderer.Render());

            // Render the battery icon itself
            int batteryBoxWidth = ConsoleWrapper.WindowWidth / 2;
            int batteryBoxHeight = ConsoleWrapper.WindowHeight / 4;
            int batteryBoxLeft = ConsoleWrapper.WindowWidth / 2 - batteryBoxWidth / 2;
            int batteryBoxTop = ConsoleWrapper.WindowHeight / 2 - batteryBoxHeight / 2 - 2;
            var batteryBoxEraser = new Eraser()
            {
                Left = batteryBoxLeft + 1,
                Top = batteryBoxTop + 1,
                Width = batteryBoxWidth,
                Height = batteryBoxHeight,
            };
            var batteryBox = new BoxFrame()
            {
                Left = batteryBoxLeft,
                Top = batteryBoxTop,
                Width = batteryBoxWidth,
                Height = batteryBoxHeight,
                Settings = new()
                {
                    BorderLeftFrameChar = '│',
                    BorderRightFrameChar = '│',
                    BorderUpperFrameChar = '─',
                    BorderLowerFrameChar = '─',
                    BorderUpperLeftCornerChar = '╭',
                    BorderUpperRightCornerChar = '╮',
                    BorderLowerLeftCornerChar = '╰',
                    BorderLowerRightCornerChar = '╯',
                }
            };
            var batteryIndicator = new Box()
            {
                Left = batteryBoxLeft + 1,
                Top = batteryBoxTop + 1,
                Width = (int)(batteryBoxWidth * (batteryLevel / 100d)),
                Height = batteryBoxHeight,
                Color = gradientsLevel,
            };
            builder.Append(
                batteryBoxEraser.Render() +
                batteryBox.Render() +

                // Render the battery terminal stem
                ConsolePositioning.RenderChangePosition(batteryBoxLeft + batteryBoxWidth + 1, batteryBoxTop + batteryBoxHeight / 2) +
                '├' +
                ConsolePositioning.RenderChangePosition(batteryBoxLeft + batteryBoxWidth + 1, batteryBoxTop + batteryBoxHeight / 2 + 2) +
                '├' +

                // Render the positive battery terminal
                ConsolePositioning.RenderChangePosition(batteryBoxLeft + batteryBoxWidth + 2, batteryBoxTop + batteryBoxHeight / 2) +
                '╮' +
                ConsolePositioning.RenderChangePosition(batteryBoxLeft + batteryBoxWidth + 2, batteryBoxTop + batteryBoxHeight / 2 + 1) +
                '│' +
                ConsolePositioning.RenderChangePosition(batteryBoxLeft + batteryBoxWidth + 2, batteryBoxTop + batteryBoxHeight / 2 + 2) +
                '╯' +

                // Render the indicator itself
                batteryIndicator.Render()
            );

            // Write to the console
            TextWriterRaw.WriteRaw(builder.ToString());

            // Decrease the battery level until charging is needed
            if (batteryCharging)
            {
                // Battery is charging. Once we reach 100%, add a cycle.
                if (++batteryLevel >= 100)
                {
                    batteryCharging = false;
                    if (batteryCycles < 75)
                    {
                        batteryDrainSpeed -= 5;
                        batteryChargeSpeed -= 2;
                        batteryCycles++;
                    }
                    else if (++batteryDegradedCycles >= 50)
                    {
                        batteryLifeDescRenderer.Text =
                            $"Le: {ColorTools.RenderSetConsoleColor(ConsoleColors.Red)}-%{ColorTools.RenderRevertForeground()}, " +
                            $"Li: {ColorTools.RenderSetConsoleColor(ConsoleColors.Red)}-%{ColorTools.RenderRevertForeground()}, " +
                            $"Cy: {ColorTools.RenderSetConsoleColor(ConsoleColors.Red)}-{ColorTools.RenderRevertForeground()}, " +
                            $"De: {ColorTools.RenderSetConsoleColor(ConsoleColors.Red)}-{ColorTools.RenderRevertForeground()}, " +
                            $"Ds: {ColorTools.RenderSetConsoleColor(ConsoleColors.Red)}- ms{ColorTools.RenderRevertForeground()}, " +
                            $"Ch: {ColorTools.RenderSetConsoleColor(ConsoleColors.Red)}- ms{ColorTools.RenderRevertForeground()}";
                        builder.Append(batteryLifeDescEraser.Render() + batteryLifeDescRenderer.Render());
                        TextWriterRaw.WriteRaw(builder.ToString());
                        ScreensaverManager.Delay(5000);
                        ScreensaverPreparation();
                    }
                }

                // Delay
                ScreensaverManager.Delay(batteryChargeSpeed);
            }
            else
            {
                // Battery is discharging.
                int minimumLevel = RandomDriver.Random(5, 20);
                if (--batteryLevel < minimumLevel)
                    batteryCharging = true;

                // Delay
                ScreensaverManager.Delay(batteryDrainSpeed);
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}
