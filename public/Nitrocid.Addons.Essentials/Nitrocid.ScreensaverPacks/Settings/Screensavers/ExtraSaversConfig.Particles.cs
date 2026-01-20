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

namespace Nitrocid.ScreensaverPacks.Settings
{
    /// <summary>
    /// Screensaver kernel configuration instance
    /// </summary>
    public partial class ExtraSaversConfig : BaseKernelConfig
    {
        private bool particlesTrueColor = true;
        private int particlesDelay = 1;
        private int particlesDensity = 25;
        private int particlesMinimumRedColorLevel = 0;
        private int particlesMinimumGreenColorLevel = 0;
        private int particlesMinimumBlueColorLevel = 0;
        private int particlesMinimumColorLevel = 0;
        private int particlesMaximumRedColorLevel = 255;
        private int particlesMaximumGreenColorLevel = 255;
        private int particlesMaximumBlueColorLevel = 255;
        private int particlesMaximumColorLevel = 255;

        /// <summary>
        /// [Particles] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public bool ParticlesTrueColor
        {
            get
            {
                return particlesTrueColor;
            }
            set
            {
                particlesTrueColor = value;
            }
        }
        /// <summary>
        /// [Particles] How many milliseconds to wait before making the next write?
        /// </summary>
        public int ParticlesDelay
        {
            get
            {
                return particlesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                particlesDelay = value;
            }
        }
        /// <summary>
        /// [Particles] How dense are the particles?
        /// </summary>
        public int ParticlesDensity
        {
            get
            {
                return particlesDensity;
            }
            set
            {
                if (value <= 0)
                    value = 25;
                particlesDensity = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum red color level (true color)
        /// </summary>
        public int ParticlesMinimumRedColorLevel
        {
            get
            {
                return particlesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                particlesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum green color level (true color)
        /// </summary>
        public int ParticlesMinimumGreenColorLevel
        {
            get
            {
                return particlesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                particlesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum blue color level (true color)
        /// </summary>
        public int ParticlesMinimumBlueColorLevel
        {
            get
            {
                return particlesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                particlesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public int ParticlesMinimumColorLevel
        {
            get
            {
                return particlesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                particlesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum red color level (true color)
        /// </summary>
        public int ParticlesMaximumRedColorLevel
        {
            get
            {
                return particlesMaximumRedColorLevel;
            }
            set
            {
                if (value <= particlesMinimumRedColorLevel)
                    value = particlesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                particlesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum green color level (true color)
        /// </summary>
        public int ParticlesMaximumGreenColorLevel
        {
            get
            {
                return particlesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= particlesMinimumGreenColorLevel)
                    value = particlesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                particlesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum blue color level (true color)
        /// </summary>
        public int ParticlesMaximumBlueColorLevel
        {
            get
            {
                return particlesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= particlesMinimumBlueColorLevel)
                    value = particlesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                particlesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Particles] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public int ParticlesMaximumColorLevel
        {
            get
            {
                return particlesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= particlesMinimumColorLevel)
                    value = particlesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                particlesMaximumColorLevel = value;
            }
        }
    }
}
