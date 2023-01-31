
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

using System;
using System.IO;

namespace KS.Files.Interactive
{
    /// <summary>
    /// File manager key binding information class
    /// </summary>
    internal class FileManagerBinding
    {
        private string _bindingName;
        private ConsoleKey _bindingKeyName;
        private Action<string, FileSystemInfo> _bindingAction;

        /// <summary>
        /// Key binding name
        /// </summary>
        public string BindingName { get => _bindingName; }

        /// <summary>
        /// Which key is bound to the action?
        /// </summary>
        public ConsoleKey BindingKeyName { get => _bindingKeyName; }

        /// <summary>
        /// The action to execute.
        /// The string argument denotes the current directory in the current pane, and
        /// the <see cref="FileSystemInfo"/> argument represents the selected file in the current pane.
        /// </summary>
        public Action<string, FileSystemInfo> BindingAction { get => _bindingAction; }

        internal FileManagerBinding(string bindingName, ConsoleKey bindingKeyName, Action<string, FileSystemInfo> bindingAction)
        {
            _bindingName = bindingName;
            _bindingKeyName = bindingKeyName;
            _bindingAction = bindingAction;
        }
    }
}
