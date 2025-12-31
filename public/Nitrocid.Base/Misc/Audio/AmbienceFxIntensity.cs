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

namespace Nitrocid.Base.Misc.Audio
{
    /// <summary>
    /// Ambience sound effect intensity
    /// </summary>
    public enum AmbienceFxIntensity
    {
        /// <summary>
        /// Calm (implies <see cref="AudioCueType.AmbienceIdle"/>)
        /// </summary>
        Calm,
        /// <summary>
        /// Normal (implies <see cref="AudioCueType.AlarmIdle"/>)
        /// </summary>
        Normal,
        /// <summary>
        /// Medium (implies <see cref="AudioCueType.Ambience"/>)
        /// </summary>
        Medium,
        /// <summary>
        /// Intense (implies <see cref="AudioCueType.Alarm"/>)
        /// </summary>
        Intense,
    }
}
