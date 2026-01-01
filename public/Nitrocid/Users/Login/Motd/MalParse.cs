//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using System.Collections.Generic;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files;
using Nitrocid.Files.Operations;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;

namespace Nitrocid.Users.Login.Motd
{
    /// <summary>
    /// Message of the Day After Login (MAL) parsing module
    /// </summary>
    public static class MalParse
    {
        private static string malMessage = "";
        private static readonly List<Func<string>> malDynamics = [];

        /// <summary>
        /// Current MAL message
        /// </summary>
        public static string MalMessage
        {
            get => malMessage ?? Translate.DoTranslation("Enjoy your day") + ", <user>!";
            set => malMessage = value ?? Translate.DoTranslation("Enjoy your day") + ", <user>!";
        }

        /// <summary>
        /// Initializes the MAL if the file isn't found.
        /// </summary>
        public static void InitMal()
        {
            if (!Checking.FileExists(PathsManagement.GetKernelPath(KernelPathType.MAL)))
                SetMal(Translate.DoTranslation("Enjoy your day") + ", <user>!");
        }

        /// <summary>
        /// Sets the MAL
        /// </summary>
        /// <param name="Message">A message of the day after login</param>
        public static void SetMal(string Message)
        {
            try
            {
                // Get the MOTD file path
                Config.MainConfig.MalFilePath = FilesystemTools.NeutralizePath(Config.MainConfig.MalFilePath);
                DebugWriter.WriteDebug(DebugLevel.I, "Path: {0}", Config.MainConfig.MalFilePath);

                // Set the message
                MalMessage = Message;
                Writing.WriteContentsText(Config.MainConfig.MalFilePath, Message);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.MOTD, Translate.DoTranslation("Error when trying to set MAL: {0}"), ex.Message);
            }
        }

        /// <summary>
        /// Reads the message of the day before/after login
        /// </summary>
        public static void ReadMal()
        {
            try
            {
                // Get the MAL file path
                Config.MainConfig.MalFilePath = FilesystemTools.NeutralizePath(Config.MainConfig.MalFilePath);
                DebugWriter.WriteDebug(DebugLevel.I, "Path: {0}", Config.MainConfig.MalFilePath);

                // Read the message
                InitMal();
                MalMessage = Reading.ReadContentsText(Config.MainConfig.MalFilePath);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.MOTD, Translate.DoTranslation("Error when trying to get MAL: {0}"), ex.Message);
            }
        }

        /// <summary>
        /// Registers a dynamic MAL
        /// </summary>
        /// <param name="dynamicMal">Dynamic MAL function that returns a string containing text to be printed</param>
        /// <exception cref="KernelException"></exception>
        public static void RegisterDynamicMal(Func<string> dynamicMal)
        {
            if (dynamicMal is null)
                throw new KernelException(KernelExceptionType.MOTD, Translate.DoTranslation("The message of the day after login may not be null."));

            // Now, register it.
            malDynamics.Add(dynamicMal);
        }

        /// <summary>
        /// Unregisters a dynamic MAL
        /// </summary>
        /// <param name="dynamicMal">Dynamic MAL function that returns a string containing text to be printed</param>
        /// <exception cref="KernelException"></exception>
        public static void UnregisterDynamicMal(Func<string> dynamicMal)
        {
            if (dynamicMal is null)
                throw new KernelException(KernelExceptionType.MOTD, Translate.DoTranslation("The message of the day after login may not be null."));

            // Now, unregister it.
            malDynamics.Remove(dynamicMal);
        }

        internal static void ProcessDynamicMal()
        {
            try
            {
                foreach (var malDynamic in malDynamics)
                {
                    string result = malDynamic();
                    TextWriters.Write(result, KernelColorType.Banner);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.MOTD, Translate.DoTranslation("Error when trying to get MAL: {0}"), ex.Message);
            }
        }

    }
}
