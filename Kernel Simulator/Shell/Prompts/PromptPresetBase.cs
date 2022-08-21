
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

using System;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Prompts
{
    public abstract class PromptPresetBase : IPromptPreset
    {

        public virtual string PresetName { get; } = "BasePreset";

        public virtual string PresetPrompt { get; } = "> ";

        public virtual ShellType PresetShellType { get; } = ShellType.Shell;

        internal virtual string PresetPromptBuilder()
        {
            DebugWriter.Wdbg(DebugLevel.E, "Tried to call prompt builder on base.");
            throw new NotImplementedException();
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

    }
}
