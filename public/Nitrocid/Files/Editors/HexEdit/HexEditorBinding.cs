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

using KS.Kernel.Exceptions;
using KS.Languages;
using System;
using System.Collections.Generic;

namespace KS.Files.Editors.HexEdit
{
    /// <summary>
    /// Keybinding class for the interactive hex editor
    /// </summary>
    public class HexEditorBinding : IEquatable<HexEditorBinding>
    {
        internal bool _localizable;

        /// <summary>
        /// Name of the keybinding
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Key to assign this keybinding to
        /// </summary>
        public ConsoleKey Key { get; }
        /// <summary>
        /// Modifiers to add with the primary key
        /// </summary>
        public ConsoleModifiers KeyModifiers { get; }
        /// <summary>
        /// Action to bind with this keybinding
        /// </summary>
        public Action Action { get; }

        /// <summary>
        /// Makes a new instance of the hex editor binding
        /// </summary>
        /// <param name="name">Name of the keybinding</param>
        /// <param name="key">Key to assign this keybinding to</param>
        /// <param name="keyModifiers">Modifiers to add with the primary key</param>
        /// <param name="action">Action to bind with this keybinding</param>
        /// <param name="localizable">Is the binding localizable?</param>
        /// <exception cref="KernelException"></exception>
        internal HexEditorBinding(string name, ConsoleKey key, ConsoleModifiers keyModifiers, Action action, bool localizable = false)
        {
            Name = name;
            Key = key;
            KeyModifiers = keyModifiers;
            _localizable = localizable;
            Action = action ??
                throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("This keybinding contains no action."));
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals(obj as HexEditorBinding);

        /// <inheritdoc/>
        public bool Equals(HexEditorBinding other) =>
            other is not null &&
            Key == other.Key &&
            KeyModifiers == other.KeyModifiers;

        /// <inheritdoc/>
        public override int GetHashCode() =>
            HashCode.Combine(Key, KeyModifiers);

        /// <inheritdoc/>
        public static bool operator ==(HexEditorBinding left, HexEditorBinding right) =>
            EqualityComparer<HexEditorBinding>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(HexEditorBinding left, HexEditorBinding right) =>
            !(left == right);
    }
}
