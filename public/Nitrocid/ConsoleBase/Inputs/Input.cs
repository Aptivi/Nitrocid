﻿//
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
using System.Threading;
using KS.Drivers;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Reader;

namespace KS.ConsoleBase.Inputs
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
        public static string ReadLine(string InputText, string DefaultValue, TermReaderSettings settings)
        {
            string Output = ReadLineUnsafe(InputText, DefaultValue, false, settings);

            // If in lock mode, wait until release
            DebugWriter.WriteDebug(DebugLevel.I, "Waiting for lock mode to release...");
            SpinWait.SpinUntil(() => !ScreensaverManager.LockMode);
            return Output;
        }

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
        public static string ReadLineWrapped(string InputText, string DefaultValue, TermReaderSettings settings)
        {
            string Output = ReadLineUnsafe(InputText, DefaultValue, true, settings);

            // If in lock mode, wait until release
            DebugWriter.WriteDebug(DebugLevel.I, "Waiting for lock mode to release...");
            SpinWait.SpinUntil(() => !ScreensaverManager.LockMode);
            return Output;
        }

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
        public static string ReadLineUnsafe(string InputText, string DefaultValue, bool OneLineWrap = false, TermReaderSettings settings = null)
        {
            bool cursorState = ConsoleWrapper.CursorVisible;
            ConsoleWrapper.CursorVisible = true;
            TermReaderSettings finalSettings = settings is null ? globalSettings : settings;
            string Output = TermReader.Read(InputText, DefaultValue, finalSettings, false, OneLineWrap, true);
            ConsoleWrapper.CursorVisible = cursorState;

            // For some reason, Terminaux tends to forget to restore the below property to the state before the read.
            Console.TreatControlCAsInput = false;
            return Output;
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        public static string ReadLineNoInput()
        {
            if (!string.IsNullOrEmpty(CurrentMask))
                return ReadLineNoInput(CurrentMask[0]);
            else
                return ReadLineNoInput(Convert.ToChar("\0"));
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        internal static string ReadLineNoInput(TermReaderSettings settings)
        {
            if (!string.IsNullOrEmpty(CurrentMask))
                return ReadLineNoInput(CurrentMask[0], settings);
            else
                return ReadLineNoInput('\0', settings);
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        public static string ReadLineNoInput(char MaskChar)
        {
            string pass = ReadLineNoInputUnsafe(MaskChar);

            // If in lock mode, wait until release
            DebugWriter.WriteDebug(DebugLevel.I, "Waiting for lock mode to release...");
            SpinWait.SpinUntil(() => !ScreensaverManager.LockMode);
            return pass;
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        /// <param name="settings">Reader settings</param>
        public static string ReadLineNoInput(char MaskChar, TermReaderSettings settings)
        {
            TermReaderSettings finalSettings = settings is null ? globalSettings : settings;
            string pass = ReadLineNoInputUnsafe(MaskChar, finalSettings);

            // If in lock mode, wait until release
            DebugWriter.WriteDebug(DebugLevel.I, "Waiting for lock mode to release...");
            SpinWait.SpinUntil(() => !ScreensaverManager.LockMode);
            return pass;
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        public static string ReadLineNoInputUnsafe()
        {
            if (!string.IsNullOrEmpty(CurrentMask))
                return ReadLineNoInputUnsafe(CurrentMask[0]);
            else
                return ReadLineNoInputUnsafe('\0');
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="settings">Reader settings</param>
        public static string ReadLineNoInputUnsafe(TermReaderSettings settings)
        {
            if (!string.IsNullOrEmpty(CurrentMask))
                return ReadLineNoInputUnsafe(CurrentMask[0], settings);
            else
                return ReadLineNoInputUnsafe('\0', settings);
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        public static string ReadLineNoInputUnsafe(char MaskChar) =>
            ReadLineNoInputUnsafe(MaskChar, globalSettings);

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        /// <param name="settings">Reader settings</param>
        public static string ReadLineNoInputUnsafe(char MaskChar, TermReaderSettings settings)
        {
            bool cursorState = ConsoleWrapper.CursorVisible;
            ConsoleWrapper.CursorVisible = true;
            TermReaderSettings finalSettings = settings is null ? globalSettings : settings;
            finalSettings.PasswordMaskChar = MaskChar;
            string pass = TermReader.Read("", "", settings, true, false, true);
            ConsoleWrapper.CursorVisible = cursorState;

            // For some reason, Terminaux tends to forget to restore the below property to the state before the read.
            Console.TreatControlCAsInput = false;
            return pass;
        }

        /// <summary>
        /// Reads the next key from the console input stream with the timeout
        /// </summary>
        /// <param name="Intercept">Whether to intercept an input</param>
        /// <param name="Timeout">Timeout</param>
        public static ConsoleKeyInfo ReadKeyTimeout(bool Intercept, TimeSpan Timeout)
        {
            var Output = ReadKeyTimeoutUnsafe(Intercept, Timeout);

            // If in lock mode, wait until release
            DebugWriter.WriteDebug(DebugLevel.I, "Waiting for lock mode to release...");
            SpinWait.SpinUntil(() => !ScreensaverManager.LockMode);
            return Output;
        }

        /// <summary>
        /// Reads the next key from the console input stream with the timeout
        /// </summary>
        /// <param name="Intercept">Whether to intercept an input</param>
        /// <param name="Timeout">Timeout</param>
        public static ConsoleKeyInfo ReadKeyTimeoutUnsafe(bool Intercept, TimeSpan Timeout)
        {
            SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable, Timeout);
            if (!ConsoleWrapper.KeyAvailable)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Timeout trying to read key.");
                throw new KernelException(KernelExceptionType.ConsoleReadTimeout, Translate.DoTranslation("User didn't provide any input in a timely fashion."));
            }
            return ConsoleWrapper.ReadKey(Intercept);
        }

        /// <summary>
        /// Detects the keypress
        /// </summary>
        public static ConsoleKeyInfo DetectKeypress()
        {
            var key = DetectKeypressUnsafe();

            // If in lock mode, wait until release
            DebugWriter.WriteDebug(DebugLevel.I, "Waiting for lock mode to release...");
            SpinWait.SpinUntil(() => !ScreensaverManager.LockMode);
            return key;
        }

        /// <summary>
        /// Detects the keypress
        /// </summary>
        public static ConsoleKeyInfo DetectKeypressUnsafe()
        {
            SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable);
            var key = ConsoleWrapper.ReadKey(true);
            DebugWriter.WriteDebug(DebugLevel.I, "Got key! {0} [{1}] {2}", key.Key.ToString(), (int)key.KeyChar, key.Modifiers.ToString());
            return key;
        }

        internal static void InitializeTerminauxWrappers()
        {
            if (isWrapperInitialized)
                return;

            // Initialize console wrappers for Terminaux
            ConsoleWrappers.ActionBeep = ConsoleWrapper.Beep;
            ConsoleWrappers.ActionBufferHeight = () => ConsoleWrapper.BufferHeight;
            ConsoleWrappers.ActionCursorLeft = () => ConsoleWrapper.CursorLeft;
            ConsoleWrappers.ActionCursorTop = () => ConsoleWrapper.CursorTop;
            ConsoleWrappers.ActionCursorVisible = (value) => ConsoleWrapper.CursorVisible = value;
            ConsoleWrappers.ActionIsDumb = () => DriverHandler.CurrentConsoleDriverLocal.IsDumb;
            ConsoleWrappers.ActionKeyAvailable = () => ConsoleWrapper.KeyAvailable;
            ConsoleWrappers.ActionReadKey = ConsoleWrapper.ReadKey;
            ConsoleWrappers.ActionSetCursorPosition = ConsoleWrapper.SetCursorPosition;
            ConsoleWrappers.ActionWindowHeight = () => ConsoleWrapper.WindowHeight;
            ConsoleWrappers.ActionWindowWidth = () => ConsoleWrapper.WindowWidth;
            ConsoleWrappers.ActionWriteChar = ConsoleWrapper.Write;
            ConsoleWrappers.ActionWriteLine = ConsoleWrapper.WriteLine;
            ConsoleWrappers.ActionWriteLineParameterized = ConsoleWrapper.WriteLine;
            ConsoleWrappers.ActionWriteLineString = ConsoleWrapper.WriteLine;
            ConsoleWrappers.ActionWriteParameterized = ConsoleWrapper.Write;
            ConsoleWrappers.ActionWriteString = ConsoleWrapper.Write;
            isWrapperInitialized = true;
        }

    }
}