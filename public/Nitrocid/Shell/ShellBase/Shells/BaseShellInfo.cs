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

using Nitrocid.Languages;
using Nitrocid.Shell.Prompts;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using System.Collections.Generic;

namespace Nitrocid.Shell.ShellBase.Shells
{
    /// <summary>
    /// Shell information for both the KS shells and the custom shells made by mods
    /// </summary>
    public abstract class BaseShellInfo : IShellInfo
    {
        internal List<CommandInfo> modCommands = [];
        internal List<CommandInfo> addonCommands = [];
        internal Dictionary<string, PromptPresetBase> customShellPresets = [];

        /// <inheritdoc/>
        public virtual object ShellLock => new();
        /// <inheritdoc/>
        public virtual List<CommandInfo> Commands => [];
        /// <inheritdoc/>
        public virtual List<CommandInfo> ModCommands => modCommands;
        /// <inheritdoc/>
        public virtual Dictionary<string, PromptPresetBase> ShellPresets => [];
        /// <inheritdoc/>
        public virtual Dictionary<string, PromptPresetBase> CustomShellPresets => customShellPresets;
        /// <inheritdoc/>
        public virtual bool AcceptsNetworkConnection => false;
        /// <inheritdoc/>
        public virtual string NetworkConnectionType => "";
        /// <inheritdoc/>
        public virtual bool OneLineWrap => false;
        /// <inheritdoc/>
        public virtual bool SlashCommand => false;
        /// <inheritdoc/>
        public virtual CommandInfo NonSlashCommandInfo =>
            new("slashreminder", LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMAND_SLASHREMINDER_DESC"), new SlashReminderCommand());
        /// <inheritdoc/>
        public virtual BaseShell? ShellBase =>
            Activator.CreateInstance<BaseShell>();
        /// <inheritdoc/>
        public virtual PromptPresetBase CurrentPreset =>
            new();
        /// <summary>
        /// Shell type. Taken from <see cref="ShellBase"/> for easier access
        /// </summary>
        public string ShellType =>
            ShellBase?.ShellType ?? "";
    }

    /// <summary>
    /// Shell information for both the KS shells and the custom shells made by mods
    /// </summary>
    public abstract class BaseShellInfo<TShell> : BaseShellInfo, IShellInfo
        where TShell : BaseShell, IShell
    {
        /// <inheritdoc/>
        public override TShell? ShellBase =>
            Activator.CreateInstance<TShell>();

        /// <inheritdoc/>
        public override PromptPresetBase CurrentPreset =>
            PromptPresetManager.GetAllPresetsFromShell(ShellType)[PromptPresetManager.CurrentPresets[ShellType]];
    }
}
