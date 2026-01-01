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

using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Kernel.Threading;
using Nitrocid.Languages;
using Textify.General;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Misc.Screensaver
{
    /// <summary>
    /// Base screensaver class
    /// </summary>
    public abstract class BaseScreensaver : IScreensaver
    {

        /// <summary>
        /// Screensaver name
        /// </summary>
        public virtual string ScreensaverName { get; set; } = "BaseScreensaver";
        /// <summary>
        /// Whether the screensaver contains flashing images
        /// </summary>
        public virtual bool ScreensaverContainsFlashingImages { get; set; } = false;

        /// <summary>
        /// Shows the seizure warning before the preparation
        /// </summary>
        public virtual void ScreensaverSeizureWarning()
        {
            KernelColorTools.LoadBackground();
            InfoBoxNonModalColor.WriteInfoBoxColorBack(
                Translate.DoTranslation("Photosensitive seizure warning") + CharManager.NewLine + CharManager.NewLine +
                Translate.DoTranslation("This screensaver may contain flashing images and fast-paced animations that may cause seizures for the photosensitive. It's recommended to seek a medical specialist for more information about such seizure before continuing. If you want to get rid of this warning, you can turn this off from the screensaver settings."),
                ConsoleColors.White, ConsoleColors.Red);
            ConsoleWrapper.CursorVisible = false;
            ThreadManager.SleepUntilInput(10000);
        }

        /// <summary>
        /// Screensaver preparation logic
        /// </summary>
        public virtual void ScreensaverPreparation()
        {
            KernelColorTools.LoadBackground();
            ConsoleWrapper.CursorVisible = false;
        }

        /// <summary>
        /// Screensaver logic
        /// </summary>
        public virtual void ScreensaverLogic() =>
            ThreadManager.SleepNoBlock(10L, ScreensaverDisplayer.ScreensaverDisplayerThread);

        /// <summary>
        /// Screensaver outro
        /// </summary>
        public virtual void ScreensaverOutro() { }

        /// <summary>
        /// Screensaver resize sync
        /// </summary>
        public virtual void ScreensaverResizeSync() =>
            ScreensaverPreparation();
    }
}
