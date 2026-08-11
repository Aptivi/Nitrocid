//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Prompts;
using Nitrocid.Base.Shell.Shells.Hex.Commands;
using Nitrocid.Base.Shell.Shells.Hex.Presets;

namespace Nitrocid.Base.Shell.Shells.Hex
{
    /// <summary>
    /// Common hex shell class
    /// </summary>
    internal class HexShellInfo : BaseShellInfo<HexShell>, IShellInfo
    {

        /// <summary>
        /// Hex commands
        /// </summary>
        public override BaseCommand[] Commands =>
        [
            new AddByteCommand(),
            new AddBytesCommand(),
            new AddByteToCommand(),
            new ClearCommand(),
            new DelByteCommand(),
            new DelBytesCommand(),
            new ExitNoSaveCommand(),
            new PrintCommand(),
            new QueryByteCommand(),
            new ReplaceCommand(),
            new SaveCommand(),
            new TuiCommand(),
        ];

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new HexDefaultPreset() },
            { "PowerLine1", new HexPowerLine1Preset() },
            { "PowerLine2", new HexPowerLine2Preset() },
            { "PowerLine3", new HexPowerLine3Preset() },
            { "PowerLineBG1", new HexPowerLineBG1Preset() },
            { "PowerLineBG2", new HexPowerLineBG2Preset() },
            { "PowerLineBG3", new HexPowerLineBG3Preset() }
        };
    }
}
