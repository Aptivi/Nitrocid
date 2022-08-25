
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
using System.Data;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Probers;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Shell.Shells.SFTP;
using Renci.SshNet;

namespace KS.Network.SFTP
{
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
                TextWriterColor.Write(Translate.DoTranslation("You should disconnect from server before connecting to another server"), true, ColorTools.ColTypes.Error);
            }
            else
            {
                try
                {
                    // Create an SFTP stream to connect to
                    string SftpHost = address.Replace("sftp://", "").Replace(address.Substring(address.LastIndexOf(":")), "");
                    string SftpPort = address.Replace("sftp://", "").Replace(SftpHost + ":", "");

                    // Check to see if no port is provided by client
                    if ((SftpHost ?? "") == (SftpPort ?? ""))
                    {
                        SftpPort = 22.ToString();
                    }

                    // Prompt for username
                    if (!string.IsNullOrWhiteSpace(SFTPShellCommon.SFTPUserPromptStyle))
                    {
                        TextWriterColor.Write(PlaceParse.ProbePlaces(SFTPShellCommon.SFTPUserPromptStyle), false, ColorTools.ColTypes.Input, address);
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Username for {0}: "), false, ColorTools.ColTypes.Input, address);
                    }
                    SFTPShellCommon.SFTPUser = Input.ReadLine();
                    if (string.IsNullOrEmpty(SFTPShellCommon.SFTPUser))
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "User is not provided. Fallback to \"anonymous\"");
                        SFTPShellCommon.SFTPUser = "anonymous";
                    }

                    // Check to see if we're aborting or not
                    if (ReadLineReboot.ReadLine.ReadRanToCompletion)
                    {
                        SFTPShellCommon._clientSFTP = new SftpClient(SSH.SSH.PromptConnectionInfo(SftpHost, Convert.ToInt32(SftpPort), SFTPShellCommon.SFTPUser));

                        // Connect to SFTP
                        ConnectSFTP();
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Error connecting to {0}: {1}", address, ex.Message);
                    DebugWriter.WStkTrc(ex);
                    TextWriterColor.Write(Translate.DoTranslation("Error when trying to connect to {0}: {1}"), true, ColorTools.ColTypes.Error, address, ex.Message);
                }
            }
        }

        /// <summary>
        /// Tries to connect to the SFTP server.
        /// </summary>
        private static void ConnectSFTP()
        {
            // Connect
            TextWriterColor.Write(Translate.DoTranslation("Trying to connect to {0}..."), true, ColorTools.ColTypes.Neutral, SFTPShellCommon.ClientSFTP.ConnectionInfo.Host);
            DebugWriter.Wdbg(DebugLevel.I, "Connecting to {0} with {1}...", SFTPShellCommon.ClientSFTP.ConnectionInfo.Host);
            SFTPShellCommon.ClientSFTP.Connect();

            // Show that it's connected
            TextWriterColor.Write(Translate.DoTranslation("Connected to {0}"), true, ColorTools.ColTypes.Neutral, SFTPShellCommon.ClientSFTP.ConnectionInfo.Host);
            DebugWriter.Wdbg(DebugLevel.I, "Connected.");
            SFTPShellCommon.SFTPConnected = true;

            // Prepare to print current SFTP directory
            SFTPShellCommon.SFTPCurrentRemoteDir = SFTPShellCommon.ClientSFTP.WorkingDirectory;
            DebugWriter.Wdbg(DebugLevel.I, "Working directory: {0}", SFTPShellCommon.SFTPCurrentRemoteDir);
            SFTPShellCommon.SFTPSite = SFTPShellCommon.ClientSFTP.ConnectionInfo.Host;
            SFTPShellCommon.SFTPUser = SFTPShellCommon.ClientSFTP.ConnectionInfo.Username;

            // Write connection information to Speed Dial file if it doesn't exist there
            var SpeedDialEntries = NetworkTools.ListSpeedDialEntries(NetworkTools.SpeedDialType.SFTP);
            DebugWriter.Wdbg(DebugLevel.I, "Speed dial length: {0}", SpeedDialEntries.Count);
            if (SpeedDialEntries.ContainsKey(SFTPShellCommon.SFTPSite))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Site already there.");
                return;
            }
            // Speed dial format is below:
            // Site,Port,Username
            else if (SFTPShellCommon.SFTPNewConnectionsToSpeedDial)
                NetworkTools.AddEntryToSpeedDial(SFTPShellCommon.SFTPSite, SFTPShellCommon.ClientSFTP.ConnectionInfo.Port, SFTPShellCommon.SFTPUser, NetworkTools.SpeedDialType.SFTP);
        }

        /// <summary>
        /// Opens speed dial prompt
        /// </summary>
        public static void SFTPQuickConnect()
        {
            if (Checking.FileExists(Paths.GetKernelPath(KernelPathType.SFTPSpeedDial)))
            {
                var SpeedDialLines = NetworkTools.ListSpeedDialEntries(NetworkTools.SpeedDialType.SFTP);
                DebugWriter.Wdbg(DebugLevel.I, "Speed dial length: {0}", SpeedDialLines.Count);
                string Answer;
                bool Answering = true;
                var SpeedDialHeaders = new[] { "#", Translate.DoTranslation("Host Name"), Translate.DoTranslation("Host Port"), Translate.DoTranslation("Username") };
                var SpeedDialData = new string[SpeedDialLines.Count, 4];
                if (!(SpeedDialLines.Count == 0))
                {
                    TextWriterColor.Write(Translate.DoTranslation("Select an address to connect to:"), true, ColorTools.ColTypes.Neutral);
                    for (int i = 0, loopTo = SpeedDialLines.Count - 1; i <= loopTo; i++)
                    {
                        string SpeedDialAddress = SpeedDialLines.Keys.ElementAtOrDefault(i);
                        DebugWriter.Wdbg(DebugLevel.I, "Speed dial address: {0}", SpeedDialAddress);
                        SpeedDialData[i, 0] = (i + 1).ToString();
                        SpeedDialData[i, 1] = SpeedDialAddress;
                        SpeedDialData[i, 2] = (string)SpeedDialLines[SpeedDialAddress]["Port"];
                        SpeedDialData[i, 3] = (string)SpeedDialLines[SpeedDialAddress]["User"];
                    }
                    TableColor.WriteTable(SpeedDialHeaders, SpeedDialData, 2, ColorTools.ColTypes.Option);
                    ConsoleBase.ConsoleWrapper.WriteLine();
                    while (Answering)
                    {
                        TextWriterColor.Write(">> ", false, ColorTools.ColTypes.Input);
                        Answer = Input.ReadLine();
                        DebugWriter.Wdbg(DebugLevel.I, "Response: {0}", Answer);
                        if (StringQuery.IsStringNumeric(Answer))
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Response is numeric. IsStringNumeric(Answer) returned true. Checking to see if in-bounds...");
                            int AnswerInt = Convert.ToInt32(Answer);
                            if (AnswerInt <= SpeedDialLines.Count)
                            {
                                Answering = false;
                                DebugWriter.Wdbg(DebugLevel.I, "Response is in-bounds. Connecting...");
                                string ChosenSpeedDialAddress = SpeedDialLines.Keys.ElementAtOrDefault(AnswerInt - 1);
                                DebugWriter.Wdbg(DebugLevel.I, "Chosen connection: {0}", ChosenSpeedDialAddress);
                                string Address = ChosenSpeedDialAddress;
                                string Port = (string)SpeedDialLines[ChosenSpeedDialAddress]["Port"];
                                string Username = (string)SpeedDialLines[ChosenSpeedDialAddress]["User"];
                                DebugWriter.Wdbg(DebugLevel.I, "Address: {0}, Port: {1}, Username: {2}", Address, Port, Username);
                                SFTPShellCommon._clientSFTP = new SftpClient(SSH.SSH.PromptConnectionInfo(Address, Convert.ToInt32(Port), Username));
                                ConnectSFTP();
                            }
                            else
                            {
                                DebugWriter.Wdbg(DebugLevel.I, "Response is out-of-bounds. Retrying...");
                                TextWriterColor.Write(Translate.DoTranslation("The selection is out of range. Select between 1-{0}. Try again."), true, ColorTools.ColTypes.Error, SpeedDialLines.Count);
                            }
                        }
                        else if (ReadLineReboot.ReadLine.ReadRanToCompletion)
                        {
                            DebugWriter.Wdbg(DebugLevel.W, "Response isn't numeric. IsStringNumeric(Answer) returned false.");
                            TextWriterColor.Write(Translate.DoTranslation("The selection is not a number. Try again."), true, ColorTools.ColTypes.Error);
                        }
                        else
                        {
                            Answering = false;
                        }
                    }
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Speed dial is empty. Lines count is 0.");
                    TextWriterColor.Write(Translate.DoTranslation("Speed dial is empty. Connect to a server to add an address to it."), true, ColorTools.ColTypes.Error);
                }
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.E, "File doesn't exist.");
                TextWriterColor.Write(Translate.DoTranslation("Speed dial doesn't exist. Connect to a server to add an address to it."), true, ColorTools.ColTypes.Error);
            }
        }

    }
}