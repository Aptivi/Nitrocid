//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;

namespace KS.Shell.Prompts
{
    /// <summary>
    /// Base prompt preset
    /// </summary>
    public class PromptPresetBase : IPromptPreset
    {

        /// <inheritdoc/>
        public virtual string PresetName { get; } = "BasePreset";

        /// <inheritdoc/>
        public virtual string PresetPrompt { get; } = "> ";

        /// <inheritdoc/>
        public virtual string PresetPromptCompletion { get; } = "[+] > ";

        /// <inheritdoc/>
        public virtual string PresetPromptShowcase { get; } = "> ";

        /// <inheritdoc/>
        public virtual string PresetPromptCompletionShowcase { get; } = "[+] > ";

        /// <inheritdoc/>
        public virtual string PresetShellType { get; } = "Shell";

        internal virtual string PresetPromptBuilder()
        {
            DebugWriter.WriteDebug(DebugLevel.E, "Tried to call prompt builder on base.");
            throw new KernelException(KernelExceptionType.NotImplementedYet);
        }

        string IPromptPreset.PresetPromptBuilder() => PresetPromptBuilder();

        internal virtual string PresetPromptCompletionBuilder()
        {
            DebugWriter.WriteDebug(DebugLevel.E, "Tried to call prompt builder on base.");
            throw new KernelException(KernelExceptionType.NotImplementedYet);
        }

        string IPromptPreset.PresetPromptCompletionBuilder() => PresetPromptCompletionBuilder();

        internal virtual string PresetPromptBuilderShowcase()
        {
            DebugWriter.WriteDebug(DebugLevel.E, "Tried to call prompt builder on base.");
            throw new KernelException(KernelExceptionType.NotImplementedYet);
        }

        string IPromptPreset.PresetPromptBuilderShowcase() => PresetPromptBuilderShowcase();

        internal virtual string PresetPromptCompletionBuilderShowcase()
        {
            DebugWriter.WriteDebug(DebugLevel.E, "Tried to call prompt builder on base.");
            throw new KernelException(KernelExceptionType.NotImplementedYet);
        }

        string IPromptPreset.PresetPromptCompletionBuilderShowcase() => PresetPromptCompletionBuilderShowcase();

    }
}
