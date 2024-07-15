//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Terminaux.Inputs.Pointer;

namespace Nitrocid.Shell.Homepage
{
    internal class HomepageBinding
    {
        private readonly string _bindingName;
        private readonly bool _bindingUsesMouse;
        private readonly ConsoleKey _bindingKeyName = (ConsoleKey)(-1);
        private readonly ConsoleModifiers _bindingKeyModifiers = (ConsoleModifiers)(-1);
        private readonly PointerButton _bindingPointerButton = (PointerButton)(-1);
        private readonly PointerButtonPress _bindingPointerButtonPress = (PointerButtonPress)(-1);
        private readonly PointerModifiers _bindingPointerModifiers = (PointerModifiers)(-1);

        /// <summary>
        /// Key binding name
        /// </summary>
        public string BindingName =>
            _bindingName;

        /// <summary>
        /// Whether the binding uses the mouse or the keyboard
        /// </summary>
        public bool BindingUsesMouse =>
            _bindingUsesMouse;

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
        /// Which pointer button is bound to the action?
        /// </summary>
        public PointerButton BindingPointerButton =>
            _bindingPointerButton;

        /// <summary>
        /// Which pointer button press mode is bound to the action?
        /// </summary>
        public PointerButtonPress BindingPointerButtonPress =>
            _bindingPointerButtonPress;

        /// <summary>
        /// Which pointer modifier is bound to the action?
        /// </summary>
        public PointerModifiers BindingPointerModifiers =>
            _bindingPointerModifiers;

        /// <summary>
        /// Makes a new instance of a key binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingKeyName">Which key is bound to the action?</param>
        public HomepageBinding(string bindingName, ConsoleKey bindingKeyName) :
            this(bindingName, bindingKeyName, default)
        { }

        /// <summary>
        /// Makes a new instance of a key binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingKeyName">Which key is bound to the action?</param>
        /// <param name="bindingKeyModifiers">Which modifiers of the key is bound to the action?</param>
        public HomepageBinding(string bindingName, ConsoleKey bindingKeyName, ConsoleModifiers bindingKeyModifiers)
        {
            _bindingName = bindingName;
            _bindingKeyName = bindingKeyName;
            _bindingKeyModifiers = bindingKeyModifiers;
        }

        /// <summary>
        /// Makes a new instance of a mouse pointer binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingPointerButton">Which key is bound to the action?</param>
        public HomepageBinding(string bindingName, PointerButton bindingPointerButton) :
            this(bindingName, bindingPointerButton, PointerButtonPress.Moved, PointerModifiers.None)
        { }

        /// <summary>
        /// Makes a new instance of a mouse pointer binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingPointerButton">Which button is bound to the action?</param>
        /// <param name="bindingPointerButtonPress">Which press mode of the button is bound to the action?</param>
        public HomepageBinding(string bindingName, PointerButton bindingPointerButton, PointerButtonPress bindingPointerButtonPress) :
            this(bindingName, bindingPointerButton, bindingPointerButtonPress, PointerModifiers.None)
        { }

        /// <summary>
        /// Makes a new instance of a mouse pointer binding
        /// </summary>
        /// <param name="bindingName">Key binding name</param>
        /// <param name="bindingPointerButton">Which button is bound to the action?</param>
        /// <param name="bindingPointerButtonPress">Which press mode of the button is bound to the action?</param>
        /// <param name="bindingButtonModifiers">Which modifiers of the button is bound to the action?</param>
        public HomepageBinding(string bindingName, PointerButton bindingPointerButton, PointerButtonPress bindingPointerButtonPress, PointerModifiers bindingButtonModifiers)
        {
            _bindingName = bindingName;
            _bindingUsesMouse = true;
            _bindingPointerButton = bindingPointerButton;
            _bindingPointerButtonPress = bindingPointerButtonPress;
            _bindingPointerModifiers = bindingButtonModifiers;
        }
    }
}
