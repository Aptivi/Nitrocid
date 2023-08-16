
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
        internal Dictionary<string, string> aliases = new();
        internal Dictionary<string, CommandInfo> modCommands = new();
        internal Dictionary<string, PromptPresetBase> customShellPresets = new();

        /// <inheritdoc/>
        public virtual object ShellLock => new();
        /// <inheritdoc/>
        public virtual Dictionary<string, string> Aliases => aliases;
        /// <inheritdoc/>
        public virtual Dictionary<string, CommandInfo> Commands => new();
        /// <inheritdoc/>
        public virtual Dictionary<string, CommandInfo> ModCommands => modCommands;
        /// <inheritdoc/>
        public virtual Dictionary<string, PromptPresetBase> ShellPresets => new();
        /// <inheritdoc/>
        public virtual Dictionary<string, PromptPresetBase> CustomShellPresets => customShellPresets;
        /// <inheritdoc/>
        public virtual BaseShell ShellBase => null;
        /// <inheritdoc/>
        public virtual PromptPresetBase CurrentPreset => null;
        /// <inheritdoc/>
        public virtual bool AcceptsNetworkConnection => false;
        /// <inheritdoc/>
        public virtual string NetworkConnectionType => "";
        /// <summary>
        /// Shell type. Taken from <see cref="ShellBase"/> for easier access
        /// </summary>
        public string ShellType => ShellBase.ShellType;
    }
}
