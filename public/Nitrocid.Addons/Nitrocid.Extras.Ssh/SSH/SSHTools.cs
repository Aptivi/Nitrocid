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
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Common;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Files;
using Nitrocid.Kernel.Events;
using Nitrocid.Kernel.Configuration;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel;
using Textify.General;
using Terminaux.Base;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.Network.Connections;
using Terminaux.Inputs;
using System.Reflection;
using Nitrocid.Kernel.Exceptions;

namespace Nitrocid.Extras.Ssh.SSH
{
    /// <summary>
    /// SSH module
    /// </summary>
    public static class SSHTools
    {

        /// <summary>
        /// Whether or not if disconnection is requested
        /// </summary>
        private static bool DisconnectionRequested;

        /// <summary>
        /// Specifies SSH connection type
        /// </summary>
        public enum ConnectionType : int
        {
            /// <summary>
            /// Connecting to SSH to use a shell
            /// </summary>
            Shell,
            /// <summary>
            /// Connecting to SSH to use a single command
            /// </summary>
            Command
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
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SSH_AUTHMETHOD_QUESTION") + CharManager.NewLine, true, KernelColorType.Question);
                TextWriters.Write("1) " + LanguageTools.GetLocalized("NKS_SSH_AUTHMETHOD_PRIVATEKEY"), true, KernelColorType.Option);
                TextWriters.Write("2) " + LanguageTools.GetLocalized("NKS_SSH_AUTHMETHOD_PASSWORD") + CharManager.NewLine, true, KernelColorType.Option);
                TextWriters.Write(">> ", false, KernelColorType.Input);
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
                            TextWriters.Write(LanguageTools.GetLocalized("NKS_SSH_INVALIDOPTION"), true, KernelColorType.Error, Answer);
                            TextWriters.Write(LanguageTools.GetLocalized("NKS_SSH_GOBACK"), true, KernelColorType.Error);
                            Input.ReadKey();
                            break;
                    }

                    if (exitWhile)
                        break;
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Answer is not numeric.");
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SSH_ANSWERNEEDSNUM"), true, KernelColorType.Error);
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SSH_GOBACK"), true, KernelColorType.Error);
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
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_SSH_PRIVKEYLOCATIONPROMPT"), false, KernelColorType.Input, Username);
                        PrivateKeyFile = InputTools.ReadLine();
                        PrivateKeyFile = FilesystemTools.NeutralizePath(PrivateKeyFile);
                        if (FilesystemTools.FileExists(PrivateKeyFile))
                        {
                            // Ask for passphrase
                            TextWriters.Write(LanguageTools.GetLocalized("NKS_SSH_PASSPHRASEPROMPT"), false, KernelColorType.Input, PrivateKeyFile);
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
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_SSH_CANTADDPRIVKEY") + " {0}", true, KernelColorType.Error, ex.Message);
                            }
                        }
                        else if (PrivateKeyFile.EndsWith("/q"))
                            break;
                        else
                            TextWriters.Write(LanguageTools.GetLocalized("NKS_SSH_KEYFILENOTFOUND"), true, KernelColorType.Error, PrivateKeyFile);
                    }

                    // Add authentication method
                    AuthenticationMethods.Add(new PrivateKeyAuthenticationMethod(Username, AuthFiles.ToArray()));
                    break;
                case 2:
                    // Password
                    string Pass;

                    // Ask for password
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SSH_PASSWORDPROMPT"), false, KernelColorType.Input, Username);
                    Pass = InputTools.ReadLineNoInput();

                    // Add authentication method
                    AuthenticationMethods.Add(new PasswordAuthenticationMethod(Username, Pass));
                    break;
            }
            return GetConnectionInfo(Address, Port, Username, AuthenticationMethods);
        }

        /// <summary>
        /// Gets connection info from the information that the user provided
        /// </summary>
        /// <param name="Address">An IP address or hostname</param>
        /// <param name="Port">A port of the SSH/SFTP server. It's usually 22</param>
        /// <param name="Username">A username to authenticate with</param>
        /// <param name="AuthMethods">Authentication methods list.</param>
        public static ConnectionInfo GetConnectionInfo(string Address, int Port, string Username, List<AuthenticationMethod> AuthMethods) =>
            new(Address, Port, Username, [.. AuthMethods]);

        /// <summary>
        /// Opens a session to specified address using the specified port and the username
        /// </summary>
        /// <param name="Address">An IP address or hostname</param>
        /// <param name="Port">A port of the SSH server. It's usually 22</param>
        /// <param name="Username">A username to authenticate with</param>
        /// <param name="Connection">Connection type</param>
        /// <param name="Command">Command to sent to remote system (usually a UNIX command)</param>
        public static void InitializeSSH(string Address, int Port, string Username, ConnectionType Connection, string Command = "")
        {
            try
            {
                // Connection
                var SSH = new SshClient(PromptConnectionInfo(Address, Port, Username));
                SSH.ConnectionInfo.Timeout = TimeSpan.FromSeconds(30d);
                if (Config.MainConfig.SSHBanner)
                    SSH.ConnectionInfo.AuthenticationBanner += ShowBanner;
                DebugWriter.WriteDebug(DebugLevel.I, "Connecting to {0}...", vars: [Address]);
                SSH.Connect();

                // Establish connection
                var sshConnection = NetworkConnectionTools.EstablishConnection("SSH client", Address, NetworkConnectionType.SSH, SSH);

                // Open SSH connection
                if (Connection == ConnectionType.Shell)
                    OpenShell(sshConnection);
                else
                    OpenCommand(sshConnection, Command);
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.SSHError, ex);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SSH_CONNECTFAILED"), true, KernelColorType.Error, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        /// <summary>
        /// Shows the SSH banner
        /// </summary>
        private static void ShowBanner(object? sender, AuthenticationBannerEventArgs e)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Banner language: {0}", vars: [e.Language]);
            DebugWriter.WriteDebug(DebugLevel.I, "Banner username: {0}", vars: [e.Username]);
            DebugWriter.WriteDebug(DebugLevel.I, "Banner length: {0}", vars: [e.BannerMessage.Length]);
            DebugWriter.WriteDebug(DebugLevel.I, "Banner:");
            var BannerMessageLines = e.BannerMessage.SplitNewLines();
            foreach (string BannerLine in BannerMessageLines)
            {
                DebugWriter.WriteDebug(DebugLevel.I, BannerLine);
                TextWriterColor.Write(BannerLine);
            }
        }

        /// <summary>
        /// Opens an SSH shell
        /// </summary>
        /// <param name="sshConnection">SSH connection</param>
        public static void OpenShell(NetworkConnection sshConnection)
        {
            OpenShell((SshClient?)sshConnection.ConnectionInstance);
            int connectionIndex = NetworkConnectionTools.GetConnectionIndex(sshConnection);
            NetworkConnectionTools.CloseConnection(connectionIndex);
        }

        /// <summary>
        /// Opens an SSH shell
        /// </summary>
        /// <param name="SSHClient">SSH client instance</param>
        public static void OpenShell(SshClient? SSHClient)
        {
            // Check the client
            if (SSHClient is null)
                throw new KernelException(KernelExceptionType.SFTPShell, LanguageTools.GetLocalized("NKS_SSH_EXCEPTION_CONNECTEDNOCLIENT"));

            try
            {
                // Add handler for SSH
                Console.CancelKeyPress += SSHDisconnect;
                Console.CancelKeyPress -= CancellationHandlers.CancelCommand;
                EventsManager.FireEvent(EventType.SSHConnected, SSHClient.ConnectionInfo.Host + ":" + SSHClient.ConnectionInfo.Port.ToString());

                // Shell creation. Note that $TERM is what kind of terminal being used (vt100, xterm, ...). Always vt100 on Windows.
                DebugWriter.WriteDebug(DebugLevel.I, "Opening shell...");
                var SSHS = SSHClient.CreateShell(
                    Console.OpenStandardInput(),
                    Console.OpenStandardOutput(),
                    Console.OpenStandardError(),
                    KernelPlatform.IsOnUnix() ? KernelPlatform.GetTerminalType() : "vt100",
                    (uint)ConsoleWrapper.WindowWidth,
                    (uint)ConsoleWrapper.WindowHeight,
                    (uint)Console.BufferWidth,
                    (uint)ConsoleWrapper.BufferHeight,
                    new Dictionary<TerminalModes, uint>()
                );
                SSHS.Start();

                // Wait until disconnection
                while (SSHClient.IsConnected)
                {
                    System.Threading.Thread.Sleep(1);
                    var reflectedInput = SSHS.GetType().GetField("_input", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (DisconnectionRequested || reflectedInput is null || reflectedInput.GetValue(SSHS) is null)
                    {
                        SSHS.Stop();
                        SSHClient.Disconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error on SSH shell in {0}: {1}", vars: [SSHClient.ConnectionInfo.Host, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SSH_SSHELLFAILED") + ": {0}", true, KernelColorType.Error, ex.Message);
            }
            finally
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Connected: {0}", vars: [SSHClient.IsConnected]);
                TextWriterColor.Write(CharManager.NewLine + LanguageTools.GetLocalized("NKS_SSH_DISCONNECTED"));
                DisconnectionRequested = false;

                // Remove handler for SSH
                Console.CancelKeyPress += CancellationHandlers.CancelCommand;
                Console.CancelKeyPress -= SSHDisconnect;
            }
        }

        /// <summary>
        /// Opens an SSH shell for a command
        /// </summary>
        /// <param name="sshConnection">SSH connection</param>
        /// <param name="Command">Command to sent to remote system (usually a UNIX command)</param>
        public static void OpenCommand(NetworkConnection sshConnection, string Command)
        {
            OpenCommand((SshClient?)sshConnection.ConnectionInstance, Command);
            int connectionIndex = NetworkConnectionTools.GetConnectionIndex(sshConnection);
            NetworkConnectionTools.CloseConnection(connectionIndex);
        }

        /// <summary>
        /// Opens an SSH shell for a command
        /// </summary>
        /// <param name="SSHClient">SSH client instance</param>
        /// <param name="Command">Command to sent to remote system (usually a UNIX command)</param>
        public static void OpenCommand(SshClient? SSHClient, string Command)
        {
            // Check the client
            if (SSHClient is null)
                throw new KernelException(KernelExceptionType.SFTPShell, LanguageTools.GetLocalized("NKS_SSH_EXCEPTION_CONNECTEDNOCLIENT"));

            try
            {
                // Add handler for SSH
                Console.CancelKeyPress += SSHDisconnect;
                Console.CancelKeyPress -= CancellationHandlers.CancelCommand;
                EventsManager.FireEvent(EventType.SSHConnected, SSHClient.ConnectionInfo.Host + ":" + SSHClient.ConnectionInfo.Port.ToString());

                // Shell creation
                DebugWriter.WriteDebug(DebugLevel.I, "Opening shell...");
                EventsManager.FireEvent(EventType.SSHPreExecuteCommand, SSHClient.ConnectionInfo.Host + ":" + SSHClient.ConnectionInfo.Port.ToString(), Command);
                var SSHC = SSHClient.CreateCommand(Command);
                var SSHCAsyncResult = SSHC.BeginExecute();
                var SSHCOutputReader = new StreamReader(SSHC.OutputStream);
                var SSHCErrorReader = new StreamReader(SSHC.ExtendedOutputStream);

                // Wait until disconnection
                while (!SSHCAsyncResult.IsCompleted)
                {
                    System.Threading.Thread.Sleep(1);
                    if (DisconnectionRequested)
                    {
                        SSHC.CancelAsync();
                        SSHClient.Disconnect();
                    }
                    while (!SSHCErrorReader.EndOfStream)
                    {
                        string? line = SSHCErrorReader.ReadLine();
                        if (line is not null)
                            TextWriterColor.Write(line);
                    }
                    while (!SSHCOutputReader.EndOfStream)
                    {
                        string? line = SSHCOutputReader.ReadLine();
                        if (line is not null)
                            TextWriterColor.Write(line);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to execute SSH command \"{0}\" to {1}: {2}", vars: [Command, SSHClient.ConnectionInfo.Host, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SSH_SSHCMDFAILED") + " {0}: {1}", true, KernelColorType.Error, Command, ex.Message);
                EventsManager.FireEvent(EventType.SSHCommandError, SSHClient.ConnectionInfo.Host + ":" + SSHClient.ConnectionInfo.Port.ToString(), Command, ex);
            }
            finally
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Connected: {0}", vars: [SSHClient.IsConnected]);
                TextWriterColor.Write(CharManager.NewLine + LanguageTools.GetLocalized("NKS_SSH_DISCONNECTED"));
                DisconnectionRequested = false;
                EventsManager.FireEvent(EventType.SSHPostExecuteCommand, SSHClient.ConnectionInfo.Host + ":" + SSHClient.ConnectionInfo.Port.ToString(), Command);

                // Remove handler for SSH
                Console.CancelKeyPress += CancellationHandlers.CancelCommand;
                Console.CancelKeyPress -= SSHDisconnect;
            }
        }

        private static void SSHDisconnect(object? sender, ConsoleCancelEventArgs e)
        {
            if (e.SpecialKey == ConsoleSpecialKey.ControlC)
            {
                e.Cancel = true;
                DisconnectionRequested = true;
                EventsManager.FireEvent(EventType.SSHDisconnected);
            }
        }

    }
}
