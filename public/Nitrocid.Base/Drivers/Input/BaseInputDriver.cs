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

using In = Nitrocid.Base.ConsoleBase.Inputs.InputTools;
using System;
using Terminaux.Reader;
using System.Threading;
using Terminaux.Base;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Drivers.Input
{
    /// <summary>
    /// Base input driver
    /// </summary>
    public abstract class BaseInputDriver : IInputDriver
    {
        /// <inheritdoc/>
        public virtual string DriverName =>
            "Default";

        /// <inheritdoc/>
        public virtual DriverTypes DriverType =>
            DriverTypes.Input;

        /// <inheritdoc/>
        public virtual bool DriverInternal =>
            false;

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        public virtual string ReadLine() =>
            ReadLine("", "", In.globalSettings);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        public virtual string ReadLine(string InputText) =>
            ReadLine(InputText, "", In.globalSettings);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        public virtual string ReadLine(string InputText, string DefaultValue) =>
            ReadLine(InputText, DefaultValue, In.globalSettings);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="settings">Reader settings</param>
        public virtual string ReadLine(string InputText, string DefaultValue, TermReaderSettings settings)
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
        public virtual string ReadLineWrapped() =>
            ReadLineWrapped("", "", In.globalSettings);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        public virtual string ReadLineWrapped(string InputText) =>
            ReadLineWrapped(InputText, "", In.globalSettings);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        public virtual string ReadLineWrapped(string InputText, string DefaultValue) =>
            ReadLineWrapped(InputText, DefaultValue, In.globalSettings);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="settings">Reader settings</param>
        public virtual string ReadLineWrapped(string InputText, string DefaultValue, TermReaderSettings settings)
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
        public virtual string ReadLineUnsafe(string InputText, string DefaultValue, bool OneLineWrap = false) =>
            ReadLineUnsafe(InputText, DefaultValue, OneLineWrap, In.globalSettings);

        /// <summary>
        /// Reads the line from the console unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="OneLineWrap">Whether to wrap the input to one line</param>
        /// <param name="settings">Reader settings</param>
        public virtual string ReadLineUnsafe(string InputText, string DefaultValue, bool OneLineWrap = false, TermReaderSettings? settings = null)
        {
            bool cursorState = ConsoleWrapper.CursorVisible;
            ConsoleWrapper.CursorVisible = true;
            TermReaderSettings finalSettings = settings is null ? In.globalSettings : settings;
            string Output = TermReader.Read(InputText, DefaultValue, finalSettings, false, OneLineWrap, true);
            ConsoleWrapper.CursorVisible = cursorState;

            // For some reason, Terminaux tends to forget to restore the below property to the state before the read.
            ConsoleWrapper.TreatCtrlCAsInput = false;
            return Output;
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        public virtual string ReadLineNoInput()
        {
            if (!string.IsNullOrEmpty(Config.MainConfig.CurrentMask))
                return ReadLineNoInput(Config.MainConfig.CurrentMask[0]);
            else
                return ReadLineNoInput(Convert.ToChar("\0"));
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        public virtual string ReadLineNoInput(TermReaderSettings settings)
        {
            if (!string.IsNullOrEmpty(Config.MainConfig.CurrentMask))
                return ReadLineNoInput(Config.MainConfig.CurrentMask[0], settings);
            else
                return ReadLineNoInput('\0', settings);
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        public virtual string ReadLineNoInput(char MaskChar)
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
        public virtual string ReadLineNoInput(char MaskChar, TermReaderSettings settings)
        {
            TermReaderSettings finalSettings = settings is null ? In.globalSettings : settings;
            string pass = ReadLineNoInputUnsafe(MaskChar, finalSettings);

            // If in lock mode, wait until release
            DebugWriter.WriteDebug(DebugLevel.I, "Waiting for lock mode to release...");
            SpinWait.SpinUntil(() => !ScreensaverManager.LockMode);
            return pass;
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        public virtual string ReadLineNoInputUnsafe()
        {
            if (!string.IsNullOrEmpty(Config.MainConfig.CurrentMask))
                return ReadLineNoInputUnsafe(Config.MainConfig.CurrentMask[0]);
            else
                return ReadLineNoInputUnsafe('\0');
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="settings">Reader settings</param>
        public virtual string ReadLineNoInputUnsafe(TermReaderSettings settings)
        {
            if (!string.IsNullOrEmpty(Config.MainConfig.CurrentMask))
                return ReadLineNoInputUnsafe(Config.MainConfig.CurrentMask[0], settings);
            else
                return ReadLineNoInputUnsafe('\0', settings);
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        public virtual string ReadLineNoInputUnsafe(char MaskChar) =>
            ReadLineNoInputUnsafe(MaskChar, In.globalSettings);

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        /// <param name="settings">Reader settings</param>
        public virtual string ReadLineNoInputUnsafe(char MaskChar, TermReaderSettings settings)
        {
            bool cursorState = ConsoleWrapper.CursorVisible;
            ConsoleWrapper.CursorVisible = true;
            TermReaderSettings finalSettings = settings is null ? In.globalSettings : settings;
            finalSettings.PasswordMaskChar = MaskChar;
            string pass = TermReader.Read("", "", finalSettings, true, false, true);
            ConsoleWrapper.CursorVisible = cursorState;

            // For some reason, Terminaux tends to forget to restore the below property to the state before the read.
            ConsoleWrapper.TreatCtrlCAsInput = false;
            return pass;
        }

        /// <summary>
        /// Reads the next key from the console input stream with the timeout
        /// </summary>
        /// <param name="Intercept">Whether to intercept an input</param>
        /// <param name="Timeout">Timeout</param>
        public virtual ConsoleKeyInfo ReadKeyTimeout(bool Intercept, TimeSpan Timeout)
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
        public virtual ConsoleKeyInfo ReadKeyTimeoutUnsafe(bool Intercept, TimeSpan Timeout)
        {
            SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable, Timeout);
            if (!ConsoleWrapper.KeyAvailable)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Timeout trying to read key.");
                throw new KernelException(KernelExceptionType.ConsoleReadTimeout, LanguageTools.GetLocalized("NKS_DRIVERS_INPUT_BASE_EXCEPTION_INPUTTIMEOUT"));
            }
            return ConsoleWrapper.ReadKey(Intercept);
        }

        /// <summary>
        /// Detects the keypress
        /// </summary>
        public virtual ConsoleKeyInfo DetectKeypress()
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
        public virtual ConsoleKeyInfo DetectKeypressUnsafe()
        {
            SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable);
            var key = ConsoleWrapper.ReadKey(true);
            DebugWriter.WriteDebug(DebugLevel.I, "Got key! {0} [{1}] {2}", vars: [key.Key.ToString(), (int)key.KeyChar, key.Modifiers.ToString()]);
            return key;
        }
    }
}
