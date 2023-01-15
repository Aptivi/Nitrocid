
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using System.Collections.Generic;

namespace KS.Shell.ShellBase.Shells
{
    /// <summary>
    /// Shell information for both the KS shells and the custom shells made by mods
    /// </summary>
    public abstract class BaseShellInfo : IShellInfo
    {
        /// <inheritdoc/>
        public virtual object ShellLock => new();
        /// <inheritdoc/>
        public virtual Dictionary<string, string> Aliases => new();
        /// <inheritdoc/>
        public virtual Dictionary<string, CommandInfo> Commands => new();
        /// <inheritdoc/>
        public virtual Dictionary<string, CommandInfo> ModCommands => new();
        /// <inheritdoc/>
        public virtual Dictionary<string, PromptPresetBase> ShellPresets => new();
        /// <inheritdoc/>
        public virtual Dictionary<string, PromptPresetBase> CustomShellPresets => new();
        /// <inheritdoc/>
        public virtual BaseShell ShellBase => null;
        /// <inheritdoc/>
        public virtual PromptPresetBase CurrentPreset => null;
        /// <summary>
        /// Shell type. Taken from <see cref="ShellBase"/> for easier access
        /// </summary>
        public string ShellType => ShellBase.ShellType;
    }
}
