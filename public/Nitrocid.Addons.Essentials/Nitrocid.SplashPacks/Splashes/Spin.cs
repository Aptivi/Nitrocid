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
using System.Threading;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Splash;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.SplashPacks.Splashes
{
    class SplashSpin : BaseSplash, ISplash
    {

        private readonly int _spinDelay = 10;
        private static int currentSpinStep = 0;
        private static readonly char[] spinSteps = ['/', '|', '\\', '-'];

        // Standalone splash information
        public override string SplashName => "Spin";

        public override string Display(SplashContext context)
        {
            var builder = new StringBuilder();
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash displaying.");
                ConsoleWrapper.CursorVisible = false;

                // Get spin character from current index
                char spinStep = spinSteps[currentSpinStep];

                // Make a spin buffer
                builder.Append(
                    new Color(ConsoleColors.White).VTSequenceForeground +
                    new Color(ConsoleColors.Black).VTSequenceBackground
                );
                for (int x = 0; x < ConsoleWrapper.WindowWidth; x++)
                {
                    for (int y = 0; y < ConsoleWrapper.WindowHeight; y++)
                    {
                        builder.Append(spinStep);
                    }
                }
                ThreadManager.SleepNoBlock(_spinDelay);

                // Step the current spin step forward
                currentSpinStep++;
                if (currentSpinStep >= spinSteps.Length)
                    currentSpinStep = 0;
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Splash done.");
            }
            return builder.ToString();
        }

    }
}
