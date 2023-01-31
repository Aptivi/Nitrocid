
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Misc.Screensaver.Customized;
using KS.Misc.Writers.ConsoleWriters;
using System;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class GetCustomSaverSetting : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Gets custom saver settings");
        public override void Run()
        {
            string Screensaver = Input.ReadLine(Translate.DoTranslation("Write a custom screensaver name:") + " ", "");
            string SettingEntry = Input.ReadLine(Translate.DoTranslation("Write a setting entry name:") + " ", "");
            if (CustomSaverTools.CustomSavers.ContainsKey(Screensaver))
            {
                TextWriterColor.Write("- {0} -> {1}: ", false, KernelColorType.ListEntry, Screensaver, SettingEntry);
                TextWriterColor.Write(Convert.ToString(CustomSaverTools.GetCustomSaverSettings(Screensaver, SettingEntry)), true, KernelColorType.ListValue);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Screensaver {0} not found."), true, KernelColorType.Error, Screensaver);
            }
        }
    }
}
