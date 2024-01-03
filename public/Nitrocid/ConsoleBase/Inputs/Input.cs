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
using Nitrocid.Drivers;
using Nitrocid.Kernel.Configuration;
using Terminaux.Base;
using Terminaux.Reader;

namespace Nitrocid.ConsoleBase.Inputs
{
    /// <summary>
    /// Console input module
    /// </summary>
    public static class Input
    {
        internal static TermReaderSettings globalSettings = new();
        internal static string currentMask = "*";
        private static bool isWrapperInitialized;

        /// <summary>
        /// Current mask character
        /// </summary>
        public static string CurrentMask =>
            Config.MainConfig.CurrentMask;

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
        public static string ReadLineUnsafe(string InputText, string DefaultValue, bool OneLineWrap = false, TermReaderSettings settings = null) =>
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
            ConsoleWrapperTools.ActionBeep = ConsoleWrapper.Beep;
            ConsoleWrapperTools.ActionBufferHeight = () => ConsoleWrapper.BufferHeight;
            ConsoleWrapperTools.ActionCursorLeft = () => ConsoleWrapper.CursorLeft;
            ConsoleWrapperTools.ActionCursorTop = () => ConsoleWrapper.CursorTop;
            ConsoleWrapperTools.ActionCursorVisible = (value) => ConsoleWrapper.CursorVisible = value;
            ConsoleWrapperTools.ActionIsDumb = () => DriverHandler.CurrentConsoleDriverLocal.IsDumb;
            ConsoleWrapperTools.ActionKeyAvailable = () => ConsoleWrapper.KeyAvailable;
            ConsoleWrapperTools.ActionReadKey = ConsoleWrapper.ReadKey;
            ConsoleWrapperTools.ActionSetCursorPosition = ConsoleWrapper.SetCursorPosition;
            ConsoleWrapperTools.ActionTreatCtrlCAsInput = (value) => ConsoleWrapper.TreatCtrlCAsInput = value;
            ConsoleWrapperTools.ActionGetTreatCtrlCAsInput = () => ConsoleWrapper.TreatCtrlCAsInput;
            ConsoleWrapperTools.ActionWindowHeight = () => ConsoleWrapper.WindowHeight;
            ConsoleWrapperTools.ActionWindowWidth = () => ConsoleWrapper.WindowWidth;
            ConsoleWrapperTools.ActionWriteChar = ConsoleWrapper.Write;
            ConsoleWrapperTools.ActionWriteLine = ConsoleWrapper.WriteLine;
            ConsoleWrapperTools.ActionWriteLineParameterized = ConsoleWrapper.WriteLine;
            ConsoleWrapperTools.ActionWriteLineString = ConsoleWrapper.WriteLine;
            ConsoleWrapperTools.ActionWriteParameterized = ConsoleWrapper.Write;
            ConsoleWrapperTools.ActionWriteString = ConsoleWrapper.Write;
            isWrapperInitialized = true;
        }

    }
}
