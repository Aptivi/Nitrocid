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

using System;

namespace KS.ConsoleBase.Interactive
{
    /// <summary>
    /// Interactive TUI binding information class
    /// </summary>
    public class InteractiveTuiBinding
    {
        private readonly string _bindingName;
        private readonly ConsoleKey _bindingKeyName;
        private readonly ConsoleModifiers _bindingKeyModifiers;
        private readonly Action<object, int> _bindingAction;
        internal bool _localizable;

        /// <summary>
        /// Key binding name
        /// </summary>
        public string BindingName =>
            _bindingName;

        /// <summary>
        /// Which key is bound to the action?
        /// </summary>
        public ConsoleKey BindingKeyName =>
            _bindingKeyName;

        /// <summary>
        /// Which key is bound to the action?
        /// </summary>
        public ConsoleModifiers BindingKeyModifiers =>
            _bindingKeyModifiers;

        /// <summary>
        /// The action to execute.
        /// The integer argument denotes the currently selected data
        /// </summary>
        public Action<object, int> BindingAction =>
            _bindingAction;

        /// <summary>
        /// Makes a new instance of an interactive TUI key binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingKeyName">Which key is bound to the action?</param>
        /// <param name="bindingAction">The action to execute. The object argument denotes the currently selected item, and the integer argument denotes the currently selected data</param>
        public InteractiveTuiBinding(string bindingName, ConsoleKey bindingKeyName, Action<object, int> bindingAction) :
            this(bindingName, bindingKeyName, default, bindingAction, false)
        { }

        /// <summary>
        /// Makes a new instance of an interactive TUI key binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingKeyName">Which key is bound to the action?</param>
        /// <param name="bindingKeyModifiers">Which modifiers of the key is bound to the action?</param>
        /// <param name="bindingAction">The action to execute. The object argument denotes the currently selected item, and the integer argument denotes the currently selected data</param>
        public InteractiveTuiBinding(string bindingName, ConsoleKey bindingKeyName, ConsoleModifiers bindingKeyModifiers, Action<object, int> bindingAction) :
            this(bindingName, bindingKeyName, bindingKeyModifiers, bindingAction, false)
        { }

        internal InteractiveTuiBinding(string bindingName, ConsoleKey bindingKeyName, Action<object, int> bindingAction, bool localizable) :
            this(bindingName, bindingKeyName, default, bindingAction, localizable)
        { }

        internal InteractiveTuiBinding(string bindingName, ConsoleKey bindingKeyName, ConsoleModifiers bindingKeyModifiers, Action<object, int> bindingAction, bool localizable)
        {
            _bindingName = bindingName;
            _bindingKeyName = bindingKeyName;
            _bindingKeyModifiers = bindingKeyModifiers;
            _bindingAction = bindingAction;
            _localizable = localizable;
        }
    }
}
