
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
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

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class SetCustomSaverSetting : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Sets custom saver settings");
        public override void Run()
        {
            string Screensaver = Input.ReadLine(Translate.DoTranslation("Write a custom screensaver name:") + " ", "");
            string SettingEntry = Input.ReadLine(Translate.DoTranslation("Write a setting entry name:") + " ", "");
            string Value = Input.ReadLine(Translate.DoTranslation("Write a value:") + " ", "");
            if (CustomSaverTools.CustomSavers.ContainsKey(Screensaver))
            {
                if (CustomSaverTools.SetCustomSaverSettings(Screensaver, SettingEntry, Value))
                {
                    TextWriterColor.Write(Translate.DoTranslation("Settings set successfully for screensaver") + " {0}.", Screensaver);
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Failed to set a setting for screensaver") + " {0}.", true, KernelColorType.Error, Screensaver);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Screensaver {0} not found."), true, KernelColorType.Error, Screensaver);
            }
        }
    }
}
