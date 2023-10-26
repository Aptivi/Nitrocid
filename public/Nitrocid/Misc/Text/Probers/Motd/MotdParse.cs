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
using KS.Files;
using KS.Files.Operations;
using KS.Files.Operations.Querying;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;

namespace KS.Misc.Text.Probers.Motd
{
    /// <summary>
    /// Message of the Day (MOTD) parsing module
    /// </summary>
    public static class MotdParse
    {
        private static string motdMessage;

        /// <summary>
        /// MOTD file path
        /// </summary>
        public static string MotdFilePath =>
            Config.MainConfig.MotdFilePath;

        /// <summary>
        /// Current MOTD message
        /// </summary>
        public static string MotdMessage
        {
            get => motdMessage ?? Translate.DoTranslation("Welcome to Nitrocid Kernel!");
            set => motdMessage = value ?? Translate.DoTranslation("Welcome to Nitrocid Kernel!");
        }

        /// <summary>
        /// Initializes the MOTD if the file isn't found.
        /// </summary>
        public static void InitMotd()
        {
            if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.MOTD)))
                SetMotd(Translate.DoTranslation("Welcome to Nitrocid Kernel!"));
        }

        /// <summary>
        /// Sets the Message of the Day
        /// </summary>
        /// <param name="Message">A message of the day</param>
        public static void SetMotd(string Message)
        {
            try
            {
                // Get the MOTD file path
                Config.MainConfig.MotdFilePath = FilesystemTools.NeutralizePath(MotdFilePath);
                DebugWriter.WriteDebug(DebugLevel.I, "Path: {0}", MotdFilePath);

                // Set the message
                MotdMessage = Message;
                Writing.WriteContentsText(MotdFilePath, Message);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.MOTD, Translate.DoTranslation("Error when trying to set MOTD: {0}"), ex.Message);
            }
        }

        /// <summary>
        /// Reads the message of the day
        /// </summary>
        public static void ReadMotd()
        {
            try
            {
                // Get the MAL file path
                Config.MainConfig.MotdFilePath = FilesystemTools.NeutralizePath(MotdFilePath);
                DebugWriter.WriteDebug(DebugLevel.I, "Path: {0}", MotdFilePath);

                // Read the message
                InitMotd();
                MotdMessage = Reading.ReadContentsText(MotdFilePath);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.MOTD, Translate.DoTranslation("Error when trying to get MOTD: {0}"), ex.Message);
            }
        }

    }
}
