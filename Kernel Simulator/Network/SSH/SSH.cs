using System;
using System.Collections.Generic;
using System.IO;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;

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

using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Platform;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace KS.Network.SSH
{
    public static class SSH
    {

        /// <summary>
        /// Whether or not if disconnection is requested
        /// </summary>
        private static bool DisconnectionRequested;
        /// <summary>
        /// Whether or not to show the SSH banner on connection
        /// </summary>
        public static bool SSHBanner;

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
            DebugWriter.Wdbg(DebugLevel.I, "Address: {0}:{1}, Username: {2}", Address, Port, Username);
            var AuthenticationMethods = new List<AuthenticationMethod>();
            int Answer;
            while (true)
            {
                // Ask for authentication method
                TextWriterColor.Write(Translate.DoTranslation("How do you want to authenticate?") + Kernel.Kernel.NewLine, true, ColorTools.ColTypes.Question);
                TextWriterColor.Write("1) " + Translate.DoTranslation("Private key file"), true, ColorTools.ColTypes.Option);
                TextWriterColor.Write("2) " + Translate.DoTranslation("Password") + Kernel.Kernel.NewLine, true, ColorTools.ColTypes.Option);
                TextWriterColor.Write(">> ", false, ColorTools.ColTypes.Input);
                if (int.TryParse(Input.ReadLine(false), out Answer))
                {
                    // Check for answer
                    bool exitWhile = false;
                    switch (Answer)
                    {
                        case 1:
                        case 2:
                            {
                                exitWhile = true;
                                break;
                            }

                        default:
                            {
                                DebugWriter.Wdbg(DebugLevel.W, "Option is not valid. Returning...");
                                TextWriterColor.Write(Translate.DoTranslation("Specified option {0} is invalid."), true, ColorTools.ColTypes.Error, Answer);
                                TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                                Console.ReadKey();
                                break;
                            }
                    }

                    if (exitWhile)
                    {
                        break;
                    }
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Answer is not numeric.");
                    TextWriterColor.Write(Translate.DoTranslation("The answer must be numeric."), true, ColorTools.ColTypes.Error);
                    TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                    Console.ReadKey();
                }
            }

            switch (Answer)
            {
                case 1: // Private key file
                    {
                        var AuthFiles = new List<PrivateKeyFile>();

                        // Prompt user
                        while (true)
                        {
                            string PrivateKeyFile, PrivateKeyPassphrase;
                            PrivateKeyFile PrivateKeyAuth;

                            // Ask for location
                            TextWriterColor.Write(Translate.DoTranslation("Enter the location of the private key for {0}. Write \"q\" to finish adding keys: "), false, ColorTools.ColTypes.Input, Username);
                            PrivateKeyFile = Input.ReadLine(false);
                            PrivateKeyFile = Filesystem.NeutralizePath(PrivateKeyFile);
                            if (Checking.FileExists(PrivateKeyFile))
                            {
                                // Ask for passphrase
                                TextWriterColor.Write(Translate.DoTranslation("Enter the passphrase for key {0}: "), false, ColorTools.ColTypes.Input, PrivateKeyFile);
                                PrivateKeyPassphrase = Input.ReadLineNoInput();

                                // Add authentication method
                                try
                                {
                                    if (string.IsNullOrEmpty(PrivateKeyPassphrase))
                                    {
                                        PrivateKeyAuth = new PrivateKeyFile(PrivateKeyFile);
                                    }
                                    else
                                    {
                                        PrivateKeyAuth = new PrivateKeyFile(PrivateKeyFile, PrivateKeyPassphrase);
                                    }
                                    AuthFiles.Add(PrivateKeyAuth);
                                }
                                catch (Exception ex)
                                {
                                    DebugWriter.WStkTrc(ex);
                                    DebugWriter.Wdbg(DebugLevel.E, "Error trying to add private key authentication method: {0}", ex.Message);
                                    TextWriterColor.Write(Translate.DoTranslation("Error trying to add private key:") + " {0}", true, ColorTools.ColTypes.Error, ex.Message);
                                }
                            }
                            else if (PrivateKeyFile.EndsWith("/q"))
                            {
                                break;
                            }
                            else
                            {
                                TextWriterColor.Write(Translate.DoTranslation("Key file {0} doesn't exist."), true, ColorTools.ColTypes.Error, PrivateKeyFile);
                            }
                        }

                        // Add authentication method
                        AuthenticationMethods.Add(new PrivateKeyAuthenticationMethod(Username, AuthFiles.ToArray()));
                        break;
                    }
                case 2: // Password
                    {
                        string Pass;

                        // Ask for password
                        TextWriterColor.Write(Translate.DoTranslation("Enter the password for {0}: "), false, ColorTools.ColTypes.Input, Username);
                        Pass = Input.ReadLineNoInput();

                        // Add authentication method
                        AuthenticationMethods.Add(new PasswordAuthenticationMethod(Username, Pass));
                        break;
                    }
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
        public static ConnectionInfo GetConnectionInfo(string Address, int Port, string Username, List<AuthenticationMethod> AuthMethods)
        {
            return new ConnectionInfo(Address, Port, Username, AuthMethods.ToArray());
        }

        /// <summary>
        /// Opens a session to specified address using the specified port and the username
        /// </summary>
        /// <param name="Address">An IP address or hostname</param>
        /// <param name="Port">A port of the SSH server. It's usually 22</param>
        /// <param name="Username">A username to authenticate with</param>
        public static void InitializeSSH(string Address, int Port, string Username, ConnectionType Connection, string Command = "")
        {
            try
            {
                // Connection
                var SSH = new SshClient(PromptConnectionInfo(Address, Port, Username));
                SSH.ConnectionInfo.Timeout = TimeSpan.FromSeconds(30d);
                if (SSHBanner)
                    SSH.ConnectionInfo.AuthenticationBanner += ShowBanner;
                DebugWriter.Wdbg(DebugLevel.I, "Connecting to {0}...", Address);
                SSH.Connect();

                // Open SSH connection
                if (Connection == ConnectionType.Shell)
                {
                    OpenShell(SSH);
                }
                else
                {
                    OpenCommand(SSH, Command);
                }
            }
            catch (Exception ex)
            {
                Kernel.Kernel.KernelEventManager.RaiseSSHError(ex);
                TextWriterColor.Write(Translate.DoTranslation("Error trying to connect to SSH server: {0}"), true, ColorTools.ColTypes.Error, ex.Message);
                DebugWriter.WStkTrc(ex);
            }
        }

        /// <summary>
        /// Shows the SSH banner
        /// </summary>
        private static void ShowBanner(object sender, AuthenticationBannerEventArgs e)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Banner language: {0}", e.Language);
            DebugWriter.Wdbg(DebugLevel.I, "Banner username: {0}", e.Username);
            DebugWriter.Wdbg(DebugLevel.I, "Banner length: {0}", e.BannerMessage.Length);
            DebugWriter.Wdbg(DebugLevel.I, "Banner:");
            var BannerMessageLines = e.BannerMessage.SplitNewLines();
            foreach (string BannerLine in BannerMessageLines)
            {
                DebugWriter.Wdbg(DebugLevel.I, BannerLine);
                TextWriterColor.Write(BannerLine, true, ColorTools.ColTypes.Neutral);
            }
        }

        /// <summary>
        /// Opens an SSH shell
        /// </summary>
        /// <param name="SSHClient">SSH client instance</param>
        public static void OpenShell(SshClient SSHClient)
        {
            try
            {
                // Add handler for SSH
                Console.CancelKeyPress += SSHDisconnect;
                Console.CancelKeyPress -= CancellationHandlers.CancelCommand;
                Kernel.Kernel.KernelEventManager.RaiseSSHConnected(SSHClient.ConnectionInfo.Host + ":" + SSHClient.ConnectionInfo.Port.ToString());

                // Shell creation. Note that $TERM is what kind of terminal being used (vt100, xterm, ...). Always vt100 on Windows.
                DebugWriter.Wdbg(DebugLevel.I, "Opening shell...");
                var SSHS = SSHClient.CreateShell(Console.OpenStandardInput(), Console.OpenStandardOutput(), Console.OpenStandardError(), KernelPlatform.IsOnUnix() ? KernelPlatform.GetTerminalType() : "vt100", (uint)Console.WindowWidth, (uint)Console.WindowHeight, (uint)Console.BufferWidth, (uint)Console.BufferHeight, new Dictionary<TerminalModes, uint>());
                SSHS.Start();

                // Wait until disconnection
                while (SSHClient.IsConnected)
                {
                    System.Threading.Thread.Sleep(1);
                    if (DisconnectionRequested | SSHS.GetType().GetField("_input", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(SSHS) is null)
                    {
                        SSHS.Stop();
                        SSHClient.Disconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Error on SSH shell in {0}: {1}", SSHClient.ConnectionInfo.Host, ex.Message);
                DebugWriter.WStkTrc(ex);
                TextWriterColor.Write(Translate.DoTranslation("Error on SSH shell") + ": {0}", true, ColorTools.ColTypes.Error, ex.Message);
            }
            finally
            {
                DebugWriter.Wdbg(DebugLevel.I, "Connected: {0}", SSHClient.IsConnected);
                TextWriterColor.Write(Kernel.Kernel.NewLine + Translate.DoTranslation("SSH Disconnected."), true, ColorTools.ColTypes.Neutral);
                DisconnectionRequested = false;

                // Remove handler for SSH
                Console.CancelKeyPress += CancellationHandlers.CancelCommand;
                Console.CancelKeyPress -= SSHDisconnect;
            }
        }

        /// <summary>
        /// Opens an SSH shell for a command
        /// </summary>
        /// <param name="SSHClient">SSH client instance</param>
        public static void OpenCommand(SshClient SSHClient, string Command)
        {
            try
            {
                // Add handler for SSH
                Console.CancelKeyPress += SSHDisconnect;
                Console.CancelKeyPress -= CancellationHandlers.CancelCommand;
                Kernel.Kernel.KernelEventManager.RaiseSSHConnected(SSHClient.ConnectionInfo.Host + ":" + SSHClient.ConnectionInfo.Port.ToString());

                // Shell creation
                DebugWriter.Wdbg(DebugLevel.I, "Opening shell...");
                Kernel.Kernel.KernelEventManager.RaiseSSHPreExecuteCommand(SSHClient.ConnectionInfo.Host + ":" + SSHClient.ConnectionInfo.Port.ToString(), Command);
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
                        TextWriterColor.Write(SSHCErrorReader.ReadLine(), true, ColorTools.ColTypes.Neutral);
                    while (!SSHCOutputReader.EndOfStream)
                        TextWriterColor.Write(SSHCOutputReader.ReadLine(), true, ColorTools.ColTypes.Neutral);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Error trying to execute SSH command \"{0}\" to {1}: {2}", Command, SSHClient.ConnectionInfo.Host, ex.Message);
                DebugWriter.WStkTrc(ex);
                TextWriterColor.Write(Translate.DoTranslation("Error executing SSH command") + " {0}: {1}", true, ColorTools.ColTypes.Error, Command, ex.Message);
                Kernel.Kernel.KernelEventManager.RaiseSSHCommandError(SSHClient.ConnectionInfo.Host + ":" + SSHClient.ConnectionInfo.Port.ToString(), Command, ex);
            }
            finally
            {
                DebugWriter.Wdbg(DebugLevel.I, "Connected: {0}", SSHClient.IsConnected);
                TextWriterColor.Write(Kernel.Kernel.NewLine + Translate.DoTranslation("SSH Disconnected."), true, ColorTools.ColTypes.Neutral);
                DisconnectionRequested = false;
                Kernel.Kernel.KernelEventManager.RaiseSSHPostExecuteCommand(SSHClient.ConnectionInfo.Host + ":" + SSHClient.ConnectionInfo.Port.ToString(), Command);

                // Remove handler for SSH
                Console.CancelKeyPress += CancellationHandlers.CancelCommand;
                Console.CancelKeyPress -= SSHDisconnect;
            }
        }

        private static void SSHDisconnect(object sender, ConsoleCancelEventArgs e)
        {
            if (e.SpecialKey == ConsoleSpecialKey.ControlC)
            {
                e.Cancel = true;
                DisconnectionRequested = true;
                Kernel.Kernel.KernelEventManager.RaiseSSHDisconnected();
            }
        }

    }
}