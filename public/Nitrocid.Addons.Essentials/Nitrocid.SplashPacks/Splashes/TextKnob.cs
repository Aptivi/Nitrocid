//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Text;
using System.Threading;
using Colorimetry;
using Colorimetry.Data;
using Colorimetry.Transformation;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Splash;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Textify.Data.Figlet;
using Textify.General;
using static System.Net.Mime.MediaTypeNames;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashTextKnob : BaseSplash, ISplash
    {
        private readonly Color stagingColor = ThemeColorsTools.GetColor(ThemeColorType.Stage);
        private int currentStage = 0;
        private Timer? incrementor = null;

        // Standalone splash information
        public override string SplashName => "TextKnob";

        // Actual logic
        public override string Opening(SplashContext context)
        {
            incrementor ??= new((_) => Increment(), null, 0, 100);
            return base.Opening(context);
        }

        public override string Display(SplashContext context)
        {
            // Adjust color
            int factor = context == SplashContext.ShuttingDown || context == SplashContext.Rebooting ? currentStage : 100 - currentStage;
            Color currentColor = TransformationTools.BlendColor(stagingColor, ConsoleColors.Black, factor / 100d);

            // Write a glorious Welcome screen
            string text = "Nitrocid";
            var figFont = FigletTools.GetFigletFont(Config.MainConfig.DefaultFigletFontName);
            int figHeight = FigletTools.GetFigletHeight(text, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            var figText = new AlignedFigletText(figFont)
            {
                Top = consoleY,
                Text = text,
                ForegroundColor = currentColor,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            return figText.Render();
        }

        public override string Closing(SplashContext context, out bool delayRequired)
        {
            currentStage = 0;
            incrementor?.Dispose();
            incrementor = null;
            return base.Closing(context, out delayRequired);
        }

        private void Increment()
        {
            if (currentStage < 100)
                currentStage++;
        }
    }
}
