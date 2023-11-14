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

namespace KS.Files.Editors.TextEdit
{
    /// <summary>
    /// Keybinding class for the interactive text editor
    /// </summary>
    public class TextEditorBinding : IEquatable<TextEditorBinding>
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
        public Func<List<string>, List<string>> Action { get; }

        /// <summary>
        /// Makes a new instance of the text editor binding
        /// </summary>
        /// <param name="name">Name of the keybinding</param>
        /// <param name="key">Key to assign this keybinding to</param>
        /// <param name="keyModifiers">Modifiers to add with the primary key</param>
        /// <param name="action">Action to bind with this keybinding</param>
        /// <param name="localizable">Is the binding localizable?</param>
        /// <exception cref="KernelException"></exception>
        internal TextEditorBinding(string name, ConsoleKey key, ConsoleModifiers keyModifiers, Func<List<string>, List<string>> action, bool localizable = false)
        {
            Name = name;
            Key = key;
            KeyModifiers = keyModifiers;
            _localizable = localizable;
            Action = action ??
                throw new KernelException(KernelExceptionType.TextEditor, Translate.DoTranslation("This keybinding contains no action."));
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals(obj as TextEditorBinding);

        /// <inheritdoc/>
        public bool Equals(TextEditorBinding other) =>
            other is not null &&
            Key == other.Key &&
            KeyModifiers == other.KeyModifiers;

        /// <inheritdoc/>
        public override int GetHashCode() =>
            HashCode.Combine(Key, KeyModifiers);

        /// <inheritdoc/>
        public static bool operator ==(TextEditorBinding left, TextEditorBinding right) =>
            EqualityComparer<TextEditorBinding>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(TextEditorBinding left, TextEditorBinding right) =>
            !(left == right);
    }
}
