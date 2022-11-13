
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Threading;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Screensaver;
using KS.Misc.Text;

#if NETCOREAPP
using System.Threading;
#endif

namespace KS.ConsoleBase.Inputs
{
    /// <summary>
    /// Console input module
    /// </summary>
    public static class Input
    {

        /// <summary>
        /// Current mask character
        /// </summary>
        public static string CurrentMask = Convert.ToString('*');

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        public static string ReadLine() => ReadLine("", "", true);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="UseCtrlCAsInput">Whether to treat CTRL + C as input</param>
        public static string ReadLine(bool UseCtrlCAsInput) => ReadLine("", "", UseCtrlCAsInput);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        public static string ReadLine(string InputText, string DefaultValue) => ReadLine(InputText, DefaultValue, true);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="UseCtrlCAsInput">Whether to treat CTRL + C as input</param>
        public static string ReadLine(string InputText, string DefaultValue, bool UseCtrlCAsInput)
        {
            string Output = ReadLineUnsafe(InputText, DefaultValue, UseCtrlCAsInput);

            // If in lock mode, wait until release
            while (Screensaver.LockMode)
                Thread.Sleep(1);
            return Output;
        }

        /// <summary>
        /// Reads the line from the console unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="UseCtrlCAsInput">Whether to treat CTRL + C as input</param>
        public static string ReadLineUnsafe(string InputText, string DefaultValue, bool UseCtrlCAsInput)
        {
            // Store the initial CtrlCEnabled value. This is so we can restore the state of CTRL + C being enabled.
            bool CtrlCEnabled = ReadLineReboot.ReadLine.CtrlCEnabled;
            ReadLineReboot.ReadLine.CtrlCEnabled = UseCtrlCAsInput;
            string Output = ReadLineReboot.ReadLine.Read(InputText, DefaultValue);
            ReadLineReboot.ReadLine.CtrlCEnabled = CtrlCEnabled;
            ScreensaverDisplayer.BailFromScreensaver();
            return Output;
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        public static string ReadLineNoInput()
        {
            if (!string.IsNullOrEmpty(CurrentMask))
            {
                return ReadLineNoInput(CurrentMask[0]);
            }
            else
            {
                return ReadLineNoInput(Convert.ToChar(""));
            }
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        public static string ReadLineNoInput(char MaskChar)
        {
            string pass = ReadLineNoInputUnsafe(MaskChar);

            // If in lock mode, wait until release
            while (Screensaver.LockMode)
                Thread.Sleep(1);
            return pass;
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        public static string ReadLineNoInputUnsafe()
        {
            if (!string.IsNullOrEmpty(CurrentMask))
            {
                return ReadLineNoInputUnsafe(CurrentMask[0]);
            }
            else
            {
                return ReadLineNoInputUnsafe(Convert.ToChar(""));
            }
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        public static string ReadLineNoInputUnsafe(char MaskChar)
        {
            string pass = ReadLineReboot.ReadLine.ReadPassword("", MaskChar);
            ScreensaverDisplayer.BailFromScreensaver();
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
            while (Screensaver.LockMode)
                Thread.Sleep(1);
            return Output;
        }

        /// <summary>
        /// Reads the next key from the console input stream with the timeout
        /// </summary>
        /// <param name="Intercept">Whether to intercept an input</param>
        /// <param name="Timeout">Timeout</param>
        public static ConsoleKeyInfo ReadKeyTimeoutUnsafe(bool Intercept, TimeSpan Timeout)
        {
            var CurrentMilliseconds = default(double);
            while (!ConsoleWrapper.KeyAvailable)
            {
                if (!(CurrentMilliseconds == Timeout.TotalMilliseconds))
                {
                    CurrentMilliseconds += 1d;
                }
                else
                {
                    ScreensaverDisplayer.BailFromScreensaver();
                    throw new KernelException(KernelExceptionType.ConsoleReadTimeout, Translate.DoTranslation("User didn't provide any input in a timely fashion."));
                }
                Thread.Sleep(1);
            }
            ScreensaverDisplayer.BailFromScreensaver();
            return ConsoleWrapper.ReadKey(Intercept);
        }

        /// <summary>
        /// Reads the next line of characters until the condition is met or the user pressed ENTER
        /// </summary>
        /// <param name="Condition">The condition to be met</param>
        [Obsolete("It'll make its return on a new generation of ReadLine.Reboot")]
        public static string ReadLineUntil(ref bool Condition)
        {
            string Final = "";
            var Finished = default(bool);
            while (!Finished)
            {
                ConsoleKeyInfo KeyInfo;
                char KeyCharacter;
                while (!ConsoleWrapper.KeyAvailable)
                {
                    if (Condition)
                        Finished = true;
                    System.Threading.Thread.Sleep(1);
                    if (Finished)
                        break;
                }
                if (!Finished)
                {
                    KeyInfo = ConsoleWrapper.ReadKey(true);
                    KeyCharacter = KeyInfo.KeyChar;
                    if (KeyCharacter == Convert.ToChar(13) | KeyCharacter == Convert.ToChar(10))
                    {
                        Finished = true;
                    }
                    else if (KeyInfo.Key == ConsoleKey.Backspace)
                    {
                        if (!(Final.Length == 0))
                        {
                            Final = Final.Remove(Final.Length - 1);
                            ConsoleWrapper.Write(Convert.ToString(CharManager.GetEsc()) + "D"); // Cursor backwards by one character
                            ConsoleWrapper.Write(Convert.ToString(CharManager.GetEsc()) + "[1X"); // Remove a character
                        }
                    }
                    else
                    {
                        Final += Convert.ToString(KeyCharacter);
                        ConsoleWrapper.Write(KeyCharacter);
                    }
                }
            }
            return Final;
        }

        /// <summary>
        /// Detects the keypress
        /// </summary>
        public static void DetectKeypress()
        {
#if NETCOREAPP
            while (!KS.ConsoleBase.ConsoleWrapper.KeyAvailable)
                Thread.Sleep(1);
            KS.ConsoleBase.ConsoleWrapper.ReadKey(true);
#else
            ConsoleWrapper.ReadKey();
#endif
        }

    }
}
