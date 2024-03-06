//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

// This file was taken from BassBoom. License notes below:

//
// BassBoom  Copyright (C) 2023  Aptivi
//
// This file is part of Nitrocid KS
//
// BassBoom is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BassBoom is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using BassBoom.Basolia.Playback;
using BassBoom.Native.Interop.Play;

namespace Nitrocid.Extras.BassBoom.Player
{
    internal static class EqualizerControls
    {
        private const int numBands = 32;
        private static readonly bool equalizerInited = false;
        private static readonly double[] bands = new double[32];

        internal static double GetCachedEqualizer(int band) =>
            bands[band];

        internal static double GetEqualizer(int band) =>
            PlaybackTools.GetEqualizer(mpg123_channels.MPG123_LR, band);

        internal static void SetEqualizer(int band, double value)
        {
            PlaybackTools.SetEqualizer(mpg123_channels.MPG123_LR, band, value);
            UpdateEqualizers();
        }

        internal static void ResetEqualizers()
        {
            PlaybackTools.ResetEqualizer();
            UpdateEqualizers();
        }

        private static void UpdateEqualizers()
        {
            for (int i = 0; i < numBands; i++)
                bands[i] = GetEqualizer(i);
        }

        static EqualizerControls()
        {
            if (equalizerInited)
                return;
            UpdateEqualizers();
            equalizerInited = true;
        }
    }
}
