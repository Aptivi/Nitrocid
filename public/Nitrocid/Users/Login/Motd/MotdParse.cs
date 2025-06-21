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
using System.Collections.Generic;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;

namespace Nitrocid.Users.Login.Motd
{
    /// <summary>
    /// Message of the Day (MOTD) parsing module
    /// </summary>
    public static class MotdParse
    {
        private static string motdMessage = "";
        private static readonly List<Func<string>> motdDynamics = [];

        /// <summary>
        /// Current MOTD message
        /// </summary>
        public static string MotdMessage
        {
            get => motdMessage ?? LanguageTools.GetLocalized("NKS_USERS_LOGIN_MOTD_DEFAULTMOTD");
            set => motdMessage = value ?? LanguageTools.GetLocalized("NKS_USERS_LOGIN_MOTD_DEFAULTMOTD");
        }

        /// <summary>
        /// Initializes the MOTD if the file isn't found.
        /// </summary>
        public static void InitMotd()
        {
            if (!FilesystemTools.FileExists(PathsManagement.GetKernelPath(KernelPathType.MOTD)))
                SetMotd(LanguageTools.GetLocalized("NKS_USERS_LOGIN_MOTD_DEFAULTMOTD"));
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
                Config.MainConfig.MotdFilePath = FilesystemTools.NeutralizePath(Config.MainConfig.MotdFilePath);
                DebugWriter.WriteDebug(DebugLevel.I, "Path: {0}", vars: [Config.MainConfig.MotdFilePath]);

                // Set the message
                MotdMessage = Message;
                FilesystemTools.WriteContentsText(Config.MainConfig.MotdFilePath, Message);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.MOTD, LanguageTools.GetLocalized("NKS_USERS_LOGIN_MOTD_EXCEPTION_SETFAILED_MOTD"), ex.Message);
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
                Config.MainConfig.MotdFilePath = FilesystemTools.NeutralizePath(Config.MainConfig.MotdFilePath);
                DebugWriter.WriteDebug(DebugLevel.I, "Path: {0}", vars: [Config.MainConfig.MotdFilePath]);

                // Read the message
                InitMotd();
                MotdMessage = FilesystemTools.ReadContentsText(Config.MainConfig.MotdFilePath);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.MOTD, LanguageTools.GetLocalized("NKS_USERS_LOGIN_MOTD_EXCEPTION_GETFAILED_MOTD"), ex.Message);
            }
        }

        /// <summary>
        /// Registers a dynamic MOTD
        /// </summary>
        /// <param name="dynamicMotd">Dynamic MOTD function that returns a string containing text to be printed</param>
        /// <exception cref="KernelException"></exception>
        public static void RegisterDynamicMotd(Func<string> dynamicMotd)
        {
            if (dynamicMotd is null)
                throw new KernelException(KernelExceptionType.MOTD, LanguageTools.GetLocalized("NKS_USERS_LOGIN_MOTD_EXCEPTION_DYNAMICNOTNULL_MOTD"));

            // Now, register it.
            motdDynamics.Add(dynamicMotd);
        }

        /// <summary>
        /// Unregisters a dynamic MOTD
        /// </summary>
        /// <param name="dynamicMotd">Dynamic MOTD function that returns a string containing text to be printed</param>
        /// <exception cref="KernelException"></exception>
        public static void UnregisterDynamicMotd(Func<string> dynamicMotd)
        {
            if (dynamicMotd is null)
                throw new KernelException(KernelExceptionType.MOTD, LanguageTools.GetLocalized("NKS_USERS_LOGIN_MOTD_EXCEPTION_DYNAMICNOTNULL_MOTD"));

            // Now, unregister it.
            motdDynamics.Remove(dynamicMotd);
        }

        internal static void ProcessDynamicMotd()
        {
            try
            {
                foreach (var motdDynamic in motdDynamics)
                {
                    string result = motdDynamic();
                    TextWriters.Write(result, ThemeColorType.Banner);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.MOTD, LanguageTools.GetLocalized("NKS_USERS_LOGIN_MOTD_EXCEPTION_GETFAILED_MOTD"), ex.Message);
            }
        }

    }
}
