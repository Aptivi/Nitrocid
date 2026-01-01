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

using Nitrocid.Extras.Amusements.Amusements.Games;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;

namespace Nitrocid.Extras.Amusements.Screensavers
{

    /// <summary>
    /// Display code for ShipDuet
    /// </summary>
    public class ShipDuetDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "ShipDuet";

        /// <inheritdoc/>
        public override void ScreensaverPreparation() => ConsoleWrapper.Clear();

        /// <inheritdoc/>
        public override void ScreensaverLogic() => ShipDuetShooter.InitializeShipDuet(true);

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            ShipDuetShooter.ShipDuetDrawThread.Stop();
            ShipDuetShooter.ShipDuetDrawThread.Wait();
            ShipDuetShooter.GameEnded = false;
        }

    }
}
