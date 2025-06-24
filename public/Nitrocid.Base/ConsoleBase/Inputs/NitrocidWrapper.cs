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

using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Misc.Audio;
using System;
using Terminaux.Base.Wrappers;
using Terminaux.Reader;

namespace Nitrocid.Base.ConsoleBase.Inputs
{
    internal class NitrocidWrapper : BaseConsoleWrapper
    {
        public override ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            var keyInfo = base.ReadKey(intercept);
            var cueType =
                keyInfo.Key == ConsoleKey.Enter ? AudioCueType.KeyboardCueEnter :
                keyInfo.Key == ConsoleKey.Backspace ? AudioCueType.KeyboardCueBackspace :
                AudioCueType.KeyboardCueType;
            if (Config.MainConfig.EnableNavigationSounds && !TermReader.IsReaderBusy)
                AudioCuesTools.PlayAudioCue(cueType);
            return keyInfo;
        }
    }
}
