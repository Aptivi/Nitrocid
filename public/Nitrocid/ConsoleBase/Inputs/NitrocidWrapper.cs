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

using Nitrocid.Drivers;
using System;
using Terminaux.Base.Structures;
using Terminaux.Base.Wrappers;

namespace Nitrocid.ConsoleBase.Inputs
{
    internal class NitrocidWrapper : BaseConsoleWrapper
    {
        public override bool IsDumb =>
            DriverHandler.CurrentConsoleDriverLocal.IsDumb;

        public override int CursorLeft
        {
            get => DriverHandler.CurrentConsoleDriverLocal.CursorLeft;
            set => DriverHandler.CurrentConsoleDriverLocal.SetCursorLeft(value);
        }

        public override int CursorTop
        {
            get => DriverHandler.CurrentConsoleDriverLocal.CursorTop;
            set => DriverHandler.CurrentConsoleDriverLocal.SetCursorTop(value);
        }

        public override Coordinate GetCursorPosition =>
            new(CursorLeft, CursorTop);

        public override int WindowWidth =>
            DriverHandler.CurrentConsoleDriverLocal.WindowWidth;

        public override int WindowHeight =>
            DriverHandler.CurrentConsoleDriverLocal.WindowHeight;

        public override int BufferWidth =>
            DriverHandler.CurrentConsoleDriverLocal.BufferWidth;

        public override int BufferHeight =>
            DriverHandler.CurrentConsoleDriverLocal.BufferHeight;

        public override bool CursorVisible
        {
            set => DriverHandler.CurrentConsoleDriverLocal.CursorVisible = value;
        }

        public override bool KeyAvailable =>
            DriverHandler.CurrentConsoleDriverLocal.KeyAvailable;

        public override bool TreatCtrlCAsInput
        {
            get => DriverHandler.CurrentConsoleDriverLocal.TreatCtrlCAsInput;
            set => DriverHandler.CurrentConsoleDriverLocal.TreatCtrlCAsInput = value;
        }

        public override void Beep() =>
            DriverHandler.CurrentConsoleDriverLocal.Beep();

        public override void Clear() =>
            DriverHandler.CurrentConsoleDriverLocal.Clear();

        public override void ClearLoadBack() =>
            DriverHandler.CurrentConsoleDriverLocal.Clear(true);

        public override ConsoleKeyInfo ReadKey(bool intercept = false) =>
            DriverHandler.CurrentConsoleDriverLocal.ReadKey(intercept);

        public override void SetBufferDimensions(int width, int height) =>
            DriverHandler.CurrentConsoleDriverLocal.SetBufferDimensions(width, height);

        public override void SetBufferHeight(int height) =>
            DriverHandler.CurrentConsoleDriverLocal.SetBufferHeight(height);

        public override void SetBufferWidth(int width) =>
            DriverHandler.CurrentConsoleDriverLocal.SetBufferWidth(width);

        public override void SetCursorLeft(int left) =>
            DriverHandler.CurrentConsoleDriverLocal.SetCursorLeft(left);

        public override void SetCursorPosition(int left, int top) =>
            DriverHandler.CurrentConsoleDriverLocal.SetCursorPosition(left, top);

        public override void SetCursorTop(int top) =>
            DriverHandler.CurrentConsoleDriverLocal.SetCursorTop(top);

        public override void SetWindowDimensions(int width, int height) =>
            DriverHandler.CurrentConsoleDriverLocal.SetWindowDimensions(width, height);

        public override void SetWindowHeight(int height) =>
            DriverHandler.CurrentConsoleDriverLocal.SetWindowHeight(height);

        public override void SetWindowWidth(int width) =>
            DriverHandler.CurrentConsoleDriverLocal.SetWindowWidth(width);

        public override void Write(char value) =>
            DriverHandler.CurrentConsoleDriverLocal.Write(value);

        public override void Write(string text) =>
            DriverHandler.CurrentConsoleDriverLocal.Write(text);

        public override void Write(string text, params object[] args) =>
            DriverHandler.CurrentConsoleDriverLocal.Write(text, args);

        public override void WriteLine() =>
            DriverHandler.CurrentConsoleDriverLocal.WriteLine();

        public override void WriteLine(string text) =>
            DriverHandler.CurrentConsoleDriverLocal.WriteLine(text);

        public override void WriteLine(string text, params object[] args) =>
            DriverHandler.CurrentConsoleDriverLocal.WriteLine(text, args);
    }
}
