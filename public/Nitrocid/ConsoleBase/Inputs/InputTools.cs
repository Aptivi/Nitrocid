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

using System;
using Nitrocid.Drivers;
using Terminaux.Base;
using Terminaux.Reader;

namespace Nitrocid.ConsoleBase.Inputs
{
    /// <summary>
    /// Console input module
    /// </summary>
    public static class InputTools
    {
        internal static TermReaderSettings globalSettings = new();
        internal static string currentMask = "*";
        internal static bool isWrapperInitialized;

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        public static string ReadLine() =>
            ReadLine("", "", globalSettings);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        public static string ReadLine(string InputText) =>
            ReadLine(InputText, "", globalSettings);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        public static string ReadLine(string InputText, string DefaultValue) =>
            ReadLine(InputText, DefaultValue, globalSettings);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="settings">Reader settings</param>
        public static string ReadLine(string InputText, string DefaultValue, TermReaderSettings settings) =>
            DriverHandler.CurrentInputDriverLocal.ReadLine(InputText, DefaultValue, settings);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        public static string ReadLineWrapped() =>
            ReadLineWrapped("", "", globalSettings);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        public static string ReadLineWrapped(string InputText) =>
            ReadLineWrapped(InputText, "", globalSettings);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        public static string ReadLineWrapped(string InputText, string DefaultValue) =>
            ReadLineWrapped(InputText, DefaultValue, globalSettings);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="settings">Reader settings</param>
        public static string ReadLineWrapped(string InputText, string DefaultValue, TermReaderSettings settings) =>
            DriverHandler.CurrentInputDriverLocal.ReadLineWrapped(InputText, DefaultValue, settings);

        /// <summary>
        /// Reads the line from the console unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="OneLineWrap">Whether to wrap the input to one line</param>
        public static string ReadLineUnsafe(string InputText, string DefaultValue, bool OneLineWrap = false) =>
            ReadLineUnsafe(InputText, DefaultValue, OneLineWrap, globalSettings);

        /// <summary>
        /// Reads the line from the console unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="OneLineWrap">Whether to wrap the input to one line</param>
        /// <param name="settings">Reader settings</param>
        public static string ReadLineUnsafe(string InputText, string DefaultValue, bool OneLineWrap = false, TermReaderSettings? settings = null) =>
            DriverHandler.CurrentInputDriverLocal.ReadLineUnsafe(InputText, DefaultValue, OneLineWrap, settings);

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        public static string ReadLineNoInput() =>
            DriverHandler.CurrentInputDriverLocal.ReadLineNoInput();

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        public static string ReadLineNoInput(TermReaderSettings settings) =>
            DriverHandler.CurrentInputDriverLocal.ReadLineNoInput(settings);

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        public static string ReadLineNoInput(char MaskChar) =>
            DriverHandler.CurrentInputDriverLocal.ReadLineNoInput(MaskChar);

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        /// <param name="settings">Reader settings</param>
        public static string ReadLineNoInput(char MaskChar, TermReaderSettings settings) =>
            DriverHandler.CurrentInputDriverLocal.ReadLineNoInput(MaskChar, settings);

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        public static string ReadLineNoInputUnsafe() =>
            DriverHandler.CurrentInputDriverLocal.ReadLineNoInputUnsafe();

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="settings">Reader settings</param>
        public static string ReadLineNoInputUnsafe(TermReaderSettings settings) =>
            DriverHandler.CurrentInputDriverLocal.ReadLineNoInputUnsafe(settings);

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        public static string ReadLineNoInputUnsafe(char MaskChar) =>
            DriverHandler.CurrentInputDriverLocal.ReadLineNoInputUnsafe(MaskChar);

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        /// <param name="settings">Reader settings</param>
        public static string ReadLineNoInputUnsafe(char MaskChar, TermReaderSettings settings) =>
            DriverHandler.CurrentInputDriverLocal.ReadLineNoInputUnsafe(MaskChar, settings);

        /// <summary>
        /// Reads the next key from the console input stream with the timeout
        /// </summary>
        /// <param name="Intercept">Whether to intercept an input</param>
        /// <param name="Timeout">Timeout</param>
        public static ConsoleKeyInfo ReadKeyTimeout(bool Intercept, TimeSpan Timeout) =>
            DriverHandler.CurrentInputDriverLocal.ReadKeyTimeout(Intercept, Timeout);

        /// <summary>
        /// Reads the next key from the console input stream with the timeout
        /// </summary>
        /// <param name="Intercept">Whether to intercept an input</param>
        /// <param name="Timeout">Timeout</param>
        public static ConsoleKeyInfo ReadKeyTimeoutUnsafe(bool Intercept, TimeSpan Timeout) =>
            DriverHandler.CurrentInputDriverLocal.ReadKeyTimeoutUnsafe(Intercept, Timeout);

        /// <summary>
        /// Detects the keypress
        /// </summary>
        public static ConsoleKeyInfo DetectKeypress() =>
            DriverHandler.CurrentInputDriverLocal.DetectKeypress();

        /// <summary>
        /// Detects the keypress
        /// </summary>
        public static ConsoleKeyInfo DetectKeypressUnsafe() =>
            DriverHandler.CurrentInputDriverLocal.DetectKeypressUnsafe();

        internal static void InitializeTerminauxWrappers()
        {
            if (isWrapperInitialized)
                return;

            // Initialize console wrappers for Terminaux
            ConsoleWrapperTools.ActionBeep = DriverHandler.CurrentConsoleDriverLocal.Beep;
            ConsoleWrapperTools.ActionBufferHeight = () => DriverHandler.CurrentConsoleDriverLocal.BufferHeight;
            ConsoleWrapperTools.ActionClear = () => DriverHandler.CurrentConsoleDriverLocal.Clear();
            ConsoleWrapperTools.ActionClearLoadBack = () => DriverHandler.CurrentConsoleDriverLocal.Clear(true);
            ConsoleWrapperTools.ActionCursorLeft = () => DriverHandler.CurrentConsoleDriverLocal.CursorLeft;
            ConsoleWrapperTools.ActionCursorTop = () => DriverHandler.CurrentConsoleDriverLocal.CursorTop;
            ConsoleWrapperTools.ActionCursorVisible = (value) => DriverHandler.CurrentConsoleDriverLocal.CursorVisible = value;
            ConsoleWrapperTools.ActionIsDumb = () => DriverHandler.CurrentConsoleDriverLocal.IsDumb;
            ConsoleWrapperTools.ActionKeyAvailable = () => DriverHandler.CurrentConsoleDriverLocal.KeyAvailable;
            ConsoleWrapperTools.ActionReadKey = DriverHandler.CurrentConsoleDriverLocal.ReadKey;
            ConsoleWrapperTools.ActionSetCursorPosition = DriverHandler.CurrentConsoleDriverLocal.SetCursorPosition;
            ConsoleWrapperTools.ActionSetWindowDimensions = DriverHandler.CurrentConsoleDriverLocal.SetWindowDimensions;
            ConsoleWrapperTools.ActionSetBufferDimensions = DriverHandler.CurrentConsoleDriverLocal.SetBufferDimensions;
            ConsoleWrapperTools.ActionTreatCtrlCAsInput = (value) => DriverHandler.CurrentConsoleDriverLocal.TreatCtrlCAsInput = value;
            ConsoleWrapperTools.ActionGetTreatCtrlCAsInput = () => DriverHandler.CurrentConsoleDriverLocal.TreatCtrlCAsInput;
            ConsoleWrapperTools.ActionSetWindowHeight = DriverHandler.CurrentConsoleDriverLocal.SetWindowHeight;
            ConsoleWrapperTools.ActionWindowHeight = () => DriverHandler.CurrentConsoleDriverLocal.WindowHeight;
            ConsoleWrapperTools.ActionSetWindowWidth = DriverHandler.CurrentConsoleDriverLocal.SetWindowWidth;
            ConsoleWrapperTools.ActionWindowWidth = () => DriverHandler.CurrentConsoleDriverLocal.WindowWidth;
            ConsoleWrapperTools.ActionWriteChar = DriverHandler.CurrentConsoleDriverLocal.Write;
            ConsoleWrapperTools.ActionWriteLine = DriverHandler.CurrentConsoleDriverLocal.WriteLine;
            ConsoleWrapperTools.ActionWriteLineParameterized = DriverHandler.CurrentConsoleDriverLocal.WriteLine;
            ConsoleWrapperTools.ActionWriteLineString = DriverHandler.CurrentConsoleDriverLocal.WriteLine;
            ConsoleWrapperTools.ActionWriteParameterized = DriverHandler.CurrentConsoleDriverLocal.Write;
            ConsoleWrapperTools.ActionWriteString = DriverHandler.CurrentConsoleDriverLocal.Write;
            isWrapperInitialized = true;
        }

    }
}
