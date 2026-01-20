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

using Nitrocid.Base.Kernel.Configuration.Instances;
using System;

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private int mazerNewMazeDelay = 10000;
        private int mazerGenerationSpeed = 20;
        private bool mazerHighlightUncovered = false;
        private bool mazerUseSchwartzian = true;

        /// <summary>
        /// [Mazer] How many milliseconds to wait before generating a new maze?
        /// </summary>
        public int MazerNewMazeDelay
        {
            get
            {
                return mazerNewMazeDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                mazerNewMazeDelay = value;
            }
        }
        /// <summary>
        /// [Mazer] Maze generation speed in milliseconds
        /// </summary>
        public int MazerGenerationSpeed
        {
            get
            {
                return mazerGenerationSpeed;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                mazerGenerationSpeed = value;
            }
        }
        /// <summary>
        /// [Mazer] If enabled, highlights the non-covered positions with the gray background color. Otherwise, they render as boxes.
        /// </summary>
        public bool MazerHighlightUncovered
        {
            get => mazerHighlightUncovered;
            set => mazerHighlightUncovered = value;
        }
        /// <summary>
        /// [Mazer] Specifies whether to choose the <seealso href="http://en.wikipedia.org/wiki/Schwartzian_transform">Schwartzian transform</seealso> or to use <see cref="Random.Shuffle{T}(T[])"/>
        /// </summary>
        public bool MazerUseSchwartzian
        {
            get => mazerUseSchwartzian;
            set => mazerUseSchwartzian = value;
        }
    }
}
