//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

namespace Nitrocid.Shell.Prompts
{
    /// <summary>
    /// Prompt preset interface
    /// </summary>
    public interface IPromptPreset
    {

        /// <summary>
        /// Preset name
        /// </summary>
        string PresetName { get; }
        /// <summary>
        /// Preset prompt style
        /// </summary>
        string PresetPrompt { get; }
        /// <summary>
        /// Preset prompt completion style
        /// </summary>
        string PresetPromptCompletion { get; }
        /// <summary>
        /// Preset prompt style (showcase)
        /// </summary>
        string PresetPromptShowcase { get; }
        /// <summary>
        /// Preset prompt completion style (showcase)
        /// </summary>
        string PresetPromptCompletionShowcase { get; }
        /// <summary>
        /// Preset shell type
        /// </summary>
        string PresetShellType { get; }

        /// <summary>
        /// Preset prompt builder logic for advanced prompts, like PowerLine, ...
        /// </summary>
        string PresetPromptBuilder();
        /// <summary>
        /// Preset prompt completion builder logic for advanced prompts, like PowerLine, ...
        /// </summary>
        string PresetPromptCompletionBuilder();
        /// <summary>
        /// Preset prompt builder logic for advanced prompts, like PowerLine, ... (showcase)
        /// </summary>
        string PresetPromptBuilderShowcase();
        /// <summary>
        /// Preset prompt completion builder logic for advanced prompts, like PowerLine, ... (showcase)
        /// </summary>
        string PresetPromptCompletionBuilderShowcase();

    }
}
