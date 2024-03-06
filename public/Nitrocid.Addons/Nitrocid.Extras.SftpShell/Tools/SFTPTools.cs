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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.SftpShell.SFTP;
using Nitrocid.Extras.SftpShell.SSH;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Misc.Text.Probers.Placeholder;
using Renci.SshNet;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.Network.Connections;

namespace Nitrocid.Extras.SftpShell.Tools
{
    /// <summary>
    /// SFTP tools module
    /// </summary>
    public static class SFTPTools
    {

        /// <summary>
        /// Tries to connect to the FTP server
        /// </summary>
        /// <param name="address">An FTP server. You may specify it like "[address]" or "[address]:[port]"</param>
        public static NetworkConnection SFTPTryToConnect(string address)
        {
            try
            {
                // Create an SFTP stream to connect to
                int indexOfPort = address.LastIndexOf(":");
                string SftpHost = address.Replace("sftp://", "");
                SftpHost = indexOfPort < 0 ? SftpHost : SftpHost.Replace(SftpHost[SftpHost.LastIndexOf(":")..], "");
                string SftpPortString = address.Replace("sftp://", "").Replace(SftpHost + ":", "");
                DebugWriter.WriteDebug(DebugLevel.W, "Host: {0}, Port: {1}", SftpHost, SftpPortString);
                bool portParsed = int.TryParse(SftpHost == SftpPortString ? "22" : SftpPortString, out int SftpPort);
                if (!portParsed)
                {
                    TextWriters.Write(Translate.DoTranslation("Make sure that you specify the port correctly."), true, KernelColorType.Error);
                    return null;
                }

                // Prompt for username
                if (!string.IsNullOrWhiteSpace(SFTPShellCommon.SFTPUserPromptStyle))
                {
                    TextWriters.Write(PlaceParse.ProbePlaces(SFTPShellCommon.SFTPUserPromptStyle), false, KernelColorType.Input, address);
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Username for {0}: "), false, KernelColorType.Input, address);
                }
                SFTPShellCommon.SFTPUser = InputTools.ReadLine();
                if (string.IsNullOrEmpty(SFTPShellCommon.SFTPUser))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "User is not provided. Fallback to \"anonymous\"");
                    SFTPShellCommon.SFTPUser = "anonymous";
                }

                // Check to see if we're aborting or not
                var client = GetConnectionInfo(SftpHost, Convert.ToInt32(SftpPort), SFTPShellCommon.SFTPUser);

                // Connect to SFTP
                return ConnectSFTP(client);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Error connecting to {0}: {1}", address, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriters.Write(Translate.DoTranslation("Error when trying to connect to {0}: {1}"), true, KernelColorType.Error, address, ex.Message);
                return null;
            }
        }

        internal static SftpClient GetConnectionInfo(string SftpHost, int SftpPort, string SftpUser) =>
            new(SSHTools.PromptConnectionInfo(SftpHost, Convert.ToInt32(SftpPort), SftpUser));

        /// <summary>
        /// Tries to connect to the SFTP server.
        /// </summary>
        internal static NetworkConnection ConnectSFTP(SftpClient client)
        {
            // Connect
            TextWriterColor.Write(Translate.DoTranslation("Trying to connect to {0}..."), client.ConnectionInfo.Host);
            DebugWriter.WriteDebug(DebugLevel.I, "Connecting to {0} with {1}...", client.ConnectionInfo.Host);
            client.Connect();
            var sftpConnection = NetworkConnectionTools.EstablishConnection("SFTP client", client.ConnectionInfo.Host, NetworkConnectionType.SFTP, client);

            // Show that it's connected
            TextWriterColor.Write(Translate.DoTranslation("Connected to {0}"), client.ConnectionInfo.Host);
            DebugWriter.WriteDebug(DebugLevel.I, "Connected.");
            return sftpConnection;
        }

    }
}
