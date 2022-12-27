
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
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Probers;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.SpeedDial;
using KS.Shell.Shells.SFTP;
using Renci.SshNet;

namespace KS.Network.SFTP
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
        public static void SFTPTryToConnect(string address)
        {
            if (SFTPShellCommon.SFTPConnected == true)
            {
                TextWriterColor.Write(Translate.DoTranslation("You should disconnect from server before connecting to another server"), true, KernelColorType.Error);
            }
            else
            {
                try
                {
                    // Create an SFTP stream to connect to
                    string SftpHost = address.Replace("sftp://", "").Replace(address.Substring(address.LastIndexOf(":")), "");
                    string SftpPort = address.Replace("sftp://", "").Replace(SftpHost + ":", "");

                    // Check to see if no port is provided by client
                    if (SftpHost == SftpPort)
                    {
                        SftpPort = 22.ToString();
                    }

                    // Prompt for username
                    if (!string.IsNullOrWhiteSpace(SFTPShellCommon.SFTPUserPromptStyle))
                    {
                        TextWriterColor.Write(PlaceParse.ProbePlaces(SFTPShellCommon.SFTPUserPromptStyle), false, KernelColorType.Input, address);
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Username for {0}: "), false, KernelColorType.Input, address);
                    }
                    SFTPShellCommon.SFTPUser = Input.ReadLine();
                    if (string.IsNullOrEmpty(SFTPShellCommon.SFTPUser))
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "User is not provided. Fallback to \"anonymous\"");
                        SFTPShellCommon.SFTPUser = "anonymous";
                    }

                    // Check to see if we're aborting or not
                    SFTPShellCommon._clientSFTP = new SftpClient(SSH.SSH.PromptConnectionInfo(SftpHost, Convert.ToInt32(SftpPort), SFTPShellCommon.SFTPUser));

                    // Connect to SFTP
                    ConnectSFTP();
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Error connecting to {0}: {1}", address, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriterColor.Write(Translate.DoTranslation("Error when trying to connect to {0}: {1}"), true, KernelColorType.Error, address, ex.Message);
                }
            }
        }

        /// <summary>
        /// Tries to connect to the SFTP server.
        /// </summary>
        private static void ConnectSFTP()
        {
            // Connect
            TextWriterColor.Write(Translate.DoTranslation("Trying to connect to {0}..."), SFTPShellCommon.ClientSFTP.ConnectionInfo.Host);
            DebugWriter.WriteDebug(DebugLevel.I, "Connecting to {0} with {1}...", SFTPShellCommon.ClientSFTP.ConnectionInfo.Host);
            SFTPShellCommon.ClientSFTP.Connect();

            // Show that it's connected
            TextWriterColor.Write(Translate.DoTranslation("Connected to {0}"), SFTPShellCommon.ClientSFTP.ConnectionInfo.Host);
            DebugWriter.WriteDebug(DebugLevel.I, "Connected.");
            SFTPShellCommon.SFTPConnected = true;

            // Prepare to print current SFTP directory
            SFTPShellCommon.SFTPCurrentRemoteDir = SFTPShellCommon.ClientSFTP.WorkingDirectory;
            DebugWriter.WriteDebug(DebugLevel.I, "Working directory: {0}", SFTPShellCommon.SFTPCurrentRemoteDir);
            SFTPShellCommon.SFTPSite = SFTPShellCommon.ClientSFTP.ConnectionInfo.Host;
            SFTPShellCommon.SFTPUser = SFTPShellCommon.ClientSFTP.ConnectionInfo.Username;

            // Write connection information to Speed Dial file if it doesn't exist there
            SpeedDialTools.TryAddEntryToSpeedDial(SFTPShellCommon.SFTPSite, SFTPShellCommon.ClientSFTP.ConnectionInfo.Port, SpeedDialType.SFTP, true, SFTPShellCommon.SFTPUser);
        }

        /// <summary>
        /// Opens speed dial prompt
        /// </summary>
        public static void SFTPQuickConnect()
        {
            var quickConnectInfo = SpeedDialTools.GetQuickConnectInfo(SpeedDialType.SFTP);
            string Address = (string)quickConnectInfo["Address"];
            string Port = (string)quickConnectInfo["Port"];
            string Username = (string)quickConnectInfo["Options"][0];
            DebugWriter.WriteDebug(DebugLevel.I, "Address: {0}, Port: {1}, Username: {2}", Address, Port, Username);
            SFTPShellCommon._clientSFTP = new SftpClient(SSH.SSH.PromptConnectionInfo(Address, Convert.ToInt32(Port), Username));
            ConnectSFTP();
        }

    }
}
