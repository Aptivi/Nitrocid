
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

using KS.Languages;

namespace KS.Kernel.Debugging.RemoteDebug.Command.BaseCommands
{
    internal class RegisterCommand : RemoteDebugBaseCommand
    {
        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, RemoteDebugDeviceInfo Address)
        {
            string Name = ListArgsOnly[0];
            Address.name = Name;
            RemoteDebugTools.SaveAllDevices();
            DebugWriter.WriteDebugDevicesOnly(DebugLevel.I, Translate.DoTranslation("Successfully registered! Hi, {0}!"), true, Name);
        }
    }
}
