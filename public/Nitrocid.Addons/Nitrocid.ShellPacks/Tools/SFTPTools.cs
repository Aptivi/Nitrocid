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
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Textify.Tools.Placeholder;
using Renci.SshNet;
using Nitrocid.Base.ConsoleBase.Inputs;
using Nitrocid.Base.Network.Connections;
using System.Collections.Generic;
using Textify.General;
using Nitrocid.Base.Files;
using Terminaux.Inputs;
using Nitrocid.ShellPacks.Shells.SFTP;

namespace Nitrocid.ShellPacks.Tools
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
        public static NetworkConnection? SFTPTryToConnect(string address)
        {
            try
            {
                // Create an SFTP stream to connect to
                int indexOfPort = address.LastIndexOf(":");
                string SftpHost = address.Replace("sftp://", "");
                SftpHost = indexOfPort < 0 ? SftpHost : SftpHost.Replace(SftpHost[SftpHost.LastIndexOf(":")..], "");
                string SftpPortString = address.Replace("sftp://", "").Replace(SftpHost + ":", "");
                DebugWriter.WriteDebug(DebugLevel.W, "Host: {0}, Port: {1}", vars: [SftpHost, SftpPortString]);
                bool portParsed = int.TryParse(SftpHost == SftpPortString ? "22" : SftpPortString, out int SftpPort);
                if (!portParsed)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_CORRECTPORTREQUIRED"), true, ThemeColorType.Error);
                    return null;
                }

                // Prompt for username
                if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.SFTPUserPromptStyle))
                    TextWriterColor.Write(PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.SFTPUserPromptStyle), false, ThemeColorType.Input, address);
                else
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_PROMPTUSERNAME"), false, ThemeColorType.Input, address);
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
                DebugWriter.WriteDebug(DebugLevel.W, "Error connecting to {0}: {1}", vars: [address, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_ERRORCONNECTING"), true, ThemeColorType.Error, address, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Prompts the user for the connection info
        /// </summary>
        /// <param name="Address">An IP address or hostname</param>
        /// <param name="Port">A port of the SSH/SFTP server. It's usually 22</param>
        /// <param name="Username">A username to authenticate with</param>
        public static ConnectionInfo PromptConnectionInfo(string Address, int Port, string Username)
        {
            // Authentication
            DebugWriter.WriteDebug(DebugLevel.I, "Address: {0}:{1}, Username: {2}", vars: [Address, Port, Username]);
            var AuthenticationMethods = new List<AuthenticationMethod>();
            int Answer;
            while (true)
            {
                // Ask for authentication method
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_AUTHMETHOD") + CharManager.NewLine, true, ThemeColorType.Question);
                TextWriterColor.Write("1) " + LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_AUTHMETHOD_PRIVATEKEY"), true, ThemeColorType.Option);
                TextWriterColor.Write("2) " + LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_AUTHMETHOD_PASSWORD") + CharManager.NewLine, true, ThemeColorType.Option);
                TextWriterColor.Write(">> ", false, ThemeColorType.Input);
                if (int.TryParse(InputTools.ReadLine(), out Answer))
                {
                    // Check for answer
                    bool exitWhile = false;
                    switch (Answer)
                    {
                        case 1:
                        case 2:
                            exitWhile = true;
                            break;
                        default:
                            DebugWriter.WriteDebug(DebugLevel.W, "Option is not valid. Returning...");
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_INVALIDOPTION"), true, ThemeColorType.Error, Answer);
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_GOBACK"), true, ThemeColorType.Error);
                            Input.ReadKey();
                            break;
                    }

                    if (exitWhile)
                        break;
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Answer is not numeric.");
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_OPTIONNUMERIC"), true, ThemeColorType.Error);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_GOBACK"), true, ThemeColorType.Error);
                    Input.ReadKey();
                }
            }

            switch (Answer)
            {
                case 1:
                    // Private key file
                    var AuthFiles = new List<PrivateKeyFile>();

                    // Prompt user
                    while (true)
                    {
                        string PrivateKeyFile, PrivateKeyPassphrase;
                        PrivateKeyFile PrivateKeyAuth;

                        // Ask for location
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_AUTHMETHOD_LOCATIONSPROMPT"), false, ThemeColorType.Input, Username);
                        PrivateKeyFile = InputTools.ReadLine();
                        PrivateKeyFile = FilesystemTools.NeutralizePath(PrivateKeyFile);
                        if (FilesystemTools.FileExists(PrivateKeyFile))
                        {
                            // Ask for passphrase
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_AUTHMETHOD_KEYPASSPHRASE"), false, ThemeColorType.Input, PrivateKeyFile);
                            PrivateKeyPassphrase = InputTools.ReadLineNoInput();

                            // Add authentication method
                            try
                            {
                                if (string.IsNullOrEmpty(PrivateKeyPassphrase))
                                    PrivateKeyAuth = new PrivateKeyFile(PrivateKeyFile);
                                else
                                    PrivateKeyAuth = new PrivateKeyFile(PrivateKeyFile, PrivateKeyPassphrase);
                                AuthFiles.Add(PrivateKeyAuth);
                            }
                            catch (Exception ex)
                            {
                                DebugWriter.WriteDebugStackTrace(ex);
                                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to add private key authentication method: {0}", vars: [ex.Message]);
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_AUTHMETHOD_KEYADDFAILED") + " {0}", true, ThemeColorType.Error, ex.Message);
                            }
                        }
                        else if (PrivateKeyFile.EndsWith("/q"))
                            break;
                        else
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_AUTHMETHOD_KEYNOTFOUND"), true, ThemeColorType.Error, PrivateKeyFile);
                    }

                    // Add authentication method
                    AuthenticationMethods.Add(new PrivateKeyAuthenticationMethod(Username, AuthFiles.ToArray()));
                    break;
                case 2:
                    // Password
                    string Pass;

                    // Ask for password
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_PASSWORDPROMPT"), false, ThemeColorType.Input, Username);
                    Pass = InputTools.ReadLineNoInput();

                    // Add authentication method
                    AuthenticationMethods.Add(new PasswordAuthenticationMethod(Username, Pass));
                    break;
            }
            return new(Address, Port, Username, [.. AuthenticationMethods]);
        }

        internal static SftpClient GetConnectionInfo(string SftpHost, int SftpPort, string SftpUser) =>
            new(PromptConnectionInfo(SftpHost, Convert.ToInt32(SftpPort), SftpUser));

        /// <summary>
        /// Tries to connect to the SFTP server.
        /// </summary>
        internal static NetworkConnection ConnectSFTP(SftpClient client)
        {
            // Connect
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTING"), client.ConnectionInfo.Host);
            DebugWriter.WriteDebug(DebugLevel.I, "Connecting to {0} with {1}...", vars: [client.ConnectionInfo.Host]);
            client.Connect();
            var sftpConnection = NetworkConnectionTools.EstablishConnection("SFTP client", client.ConnectionInfo.Host, NetworkConnectionType.SFTP, client);

            // Show that it's connected
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_CONNECTEDTO"), client.ConnectionInfo.Host);
            DebugWriter.WriteDebug(DebugLevel.I, "Connected.");
            return sftpConnection;
        }

    }
}
