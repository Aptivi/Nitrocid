using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using System.Collections.Generic;

namespace KS.Shell.ShellBase.Shells
{
    /// <summary>
    /// Shell information interface for both the KS shells and the custom shells made by mods
    /// </summary>
    public interface IShellInfo
    {
        /// <summary>
        /// Shell sync lock object
        /// </summary>
        object ShellLock { get; }
        /// <summary>
        /// Shell command aliases
        /// </summary>
        Dictionary<string, string> Aliases { get; }
        /// <summary>
        /// Built-in shell commands
        /// </summary>
        Dictionary<string, CommandInfo> Commands { get; }
        /// <summary>
        /// Mod commands
        /// </summary>
        Dictionary<string, CommandInfo> ModCommands { get; }
        /// <summary>
        /// Built-in shell presets
        /// </summary>
        Dictionary<string, PromptPresetBase> ShellPresets { get; }
        /// <summary>
        /// Mod shell presets
        /// </summary>
        Dictionary<string, PromptPresetBase> CustomShellPresets { get; }
        /// <summary>
        /// Gets the shell base
        /// </summary>
        BaseShell ShellBase { get; }
        /// <summary>
        /// Gets the current preset
        /// </summary>
        PromptPresetBase CurrentPreset { get; }
        /// <summary>
        /// Whether the shell accepts network connection
        /// </summary>
        bool AcceptsNetworkConnection { get; }
        /// <summary>
        /// Network connection type defined for the shell (valid only on shells that have <see cref="AcceptsNetworkConnection"/> set to true)
        /// </summary>
        string NetworkConnectionType { get; }
    }
}
