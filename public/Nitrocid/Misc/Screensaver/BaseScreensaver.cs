//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using Terminaux.Inputs.Styles.Infobox.Tools;

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
        public virtual string ScreensaverName =>
            "BaseScreensaver";
        /// <summary>
        /// Whether the screensaver contains flashing images
        /// </summary>
        public virtual bool ScreensaverContainsFlashingImages =>
            false;

        /// <summary>
        /// Shows the seizure warning before the preparation
        /// </summary>
        public virtual void ScreensaverSeizureWarning()
        {
            KernelColorTools.LoadBackground();
            InfoBoxNonModalColor.WriteInfoBox(
                LanguageTools.GetLocalized("NKS_MISC_SCREENSAVER_PSWARNING_TITLE") + CharManager.NewLine + CharManager.NewLine +
                LanguageTools.GetLocalized("NKS_MISC_SCREENSAVER_PSWARNING_DESC"), new InfoBoxSettings()
                {
                    ForegroundColor = ConsoleColors.White,
                    BackgroundColor = ConsoleColors.Red
                });
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
            ScreensaverManager.Delay(10);

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
