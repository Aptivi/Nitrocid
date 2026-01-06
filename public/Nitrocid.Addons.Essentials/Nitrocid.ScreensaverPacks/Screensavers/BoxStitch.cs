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

using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Nitrocid.Base.Drivers.RNG;
using System.Collections.Generic;
using Terminaux.Writer.CyclicWriters.Graphical.Rulers;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for BoxStitch
    /// </summary>
    public class BoxStitchDisplay : BaseScreensaver, IScreensaver
    {
        private Color boxColor1 = Color.Empty;
        private Color boxColor2 = Color.Empty;
        private Color boxColor3 = Color.Empty;
        private Color boxColor4 = Color.Empty;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "BoxStitch";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Set the box colors
            boxColor1 = ColorTools.GetRandomColor(ColorType.TrueColor,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumColorLevel,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumRedColorLevel,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumGreenColorLevel,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumBlueColorLevel);
            boxColor2 = ColorTools.GetRandomColor(ColorType.TrueColor,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumColorLevel,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumRedColorLevel,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumGreenColorLevel,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumBlueColorLevel);
            boxColor3 = ColorTools.GetRandomColor(ColorType.TrueColor,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumColorLevel,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumRedColorLevel,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumGreenColorLevel,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumBlueColorLevel);
            boxColor4 = ColorTools.GetRandomColor(ColorType.TrueColor,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumColorLevel,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumRedColorLevel,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumGreenColorLevel,
                ScreensaverPackInit.SaversConfig.BoxStitchMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.BoxStitchMaximumBlueColorLevel);
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get the dimensions for four boxes
            int width = ConsoleWrapper.WindowWidth / 2 - 2;
            int height = ConsoleWrapper.WindowHeight / 2 - 2;
            var box1 = new Border()
            {
                Color = boxColor1,
                Left = 0,
                Top = 0,
                Width = width,
                Height = height,
            };
            var box2 = new Border()
            {
                Color = boxColor2,
                Left = width + 2,
                Top = 0,
                Width = width,
                Height = height,
            };
            var box3 = new Border()
            {
                Color = boxColor3,
                Left = 0,
                Top = height + 2,
                Width = width,
                Height = height,
            };
            var box4 = new Border()
            {
                Color = boxColor4,
                Left = width + 2,
                Top = height + 2,
                Width = width,
                Height = height,
            };
            TextWriterRaw.WriteRaw(box1.Render());
            TextWriterRaw.WriteRaw(box2.Render());
            TextWriterRaw.WriteRaw(box3.Render());
            TextWriterRaw.WriteRaw(box4.Render());

            // Add lines
            bool box1Done = false;
            bool box2Done = false;
            bool box3Done = false;
            bool box4Done = false;
            List<RulerInfo> box1Rulers = [];
            List<RulerInfo> box2Rulers = [];
            List<RulerInfo> box3Rulers = [];
            List<RulerInfo> box4Rulers = [];
            while (!box1Done && !box2Done && !box3Done && !box4Done)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;
                bool makeLine1 = RandomDriver.RandomChance(RandomDriver.Random(25));
                bool makeLine2 = RandomDriver.RandomChance(RandomDriver.Random(25));
                bool makeLine3 = RandomDriver.RandomChance(RandomDriver.Random(25));
                bool makeLine4 = RandomDriver.RandomChance(RandomDriver.Random(25));
                if (makeLine1)
                {
                    bool lineVertical = RandomDriver.RandomChance(RandomDriver.Random(25));
                    int position = RandomDriver.Random(lineVertical ? width : height);
                    var rulerInfo = new RulerInfo(position, lineVertical ? RulerOrientation.Vertical : RulerOrientation.Horizontal);
                    if (!box1Rulers.Contains(rulerInfo))
                        box1Rulers.Add(rulerInfo);
                    box1.Rulers = [.. box1Rulers];
                    if (box1.Rulers.Length == width + height)
                        box1Done = true;
                }
                if (makeLine2)
                {
                    bool lineVertical = RandomDriver.RandomChance(RandomDriver.Random(25));
                    int position = RandomDriver.Random(lineVertical ? width : height);
                    var rulerInfo = new RulerInfo(position, lineVertical ? RulerOrientation.Vertical : RulerOrientation.Horizontal);
                    if (!box2Rulers.Contains(rulerInfo))
                        box2Rulers.Add(rulerInfo);
                    box2.Rulers = [.. box2Rulers];
                    if (box2.Rulers.Length == width + height)
                        box2Done = true;
                }
                if (makeLine3)
                {
                    bool lineVertical = RandomDriver.RandomChance(RandomDriver.Random(25));
                    int position = RandomDriver.Random(lineVertical ? width : height);
                    var rulerInfo = new RulerInfo(position, lineVertical ? RulerOrientation.Vertical : RulerOrientation.Horizontal);
                    if (!box3Rulers.Contains(rulerInfo))
                        box3Rulers.Add(rulerInfo);
                    box3.Rulers = [.. box3Rulers];
                    if (box3.Rulers.Length == width + height)
                        box3Done = true;
                }
                if (makeLine4)
                {
                    bool lineVertical = RandomDriver.RandomChance(RandomDriver.Random(25));
                    int position = RandomDriver.Random(lineVertical ? width : height);
                    var rulerInfo = new RulerInfo(position, lineVertical ? RulerOrientation.Vertical : RulerOrientation.Horizontal);
                    if (!box4Rulers.Contains(rulerInfo))
                        box4Rulers.Add(rulerInfo);
                    box4.Rulers = [.. box4Rulers];
                    if (box4.Rulers.Length == width + height)
                        box4Done = true;
                }
                TextWriterRaw.WriteRaw(box1.Render());
                TextWriterRaw.WriteRaw(box2.Render());
                TextWriterRaw.WriteRaw(box3.Render());
                TextWriterRaw.WriteRaw(box4.Render());
                ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.BoxStitchLineDelay);
            }
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.BoxStitchDelay);
        }
    }
}
