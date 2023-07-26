
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Net;
using System.Net.Security;
using FluentFTP;
using FluentFTP.Client.BaseClient;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Probers.Placeholder;
using KS.Misc.Text;
using KS.Network.Base.Connections;
using KS.Network.SpeedDial;
using KS.Shell.Shells.FTP;

namespace KS.Network.FTP
{
    /// <summary>
    /// FTP tools class
    /// </summary>
    public static class FTPTools
    {

        /// <summary>
        /// Prompts user for a password
        /// </summary>
        /// <param name="clientFTP">FTP client</param>
        /// <param name="user">A user name</param>
        /// <param name="Address">A host address</param>
        /// <param name="Port">A port for the address</param>
        /// <param name="EncryptionMode">FTP encryption mode</param>
        public static NetworkConnection PromptForPassword(FtpClient clientFTP, string user, string Address = "", int Port = 0, FtpEncryptionMode EncryptionMode = FtpEncryptionMode.Explicit)
        {
            // Make a new FTP client object instance (Used in case logging in using speed dial)
            if (clientFTP is null)
            {
                var ftpConfig = new FtpConfig()
                {
                    RetryAttempts = FTPShellCommon.FtpVerifyRetryAttempts,
                    ConnectTimeout = FTPShellCommon.FtpConnectTimeout,
                    DataConnectionConnectTimeout = FTPShellCommon.FtpDataConnectTimeout,
                    EncryptionMode = EncryptionMode,
                    InternetProtocolVersions = FTPShellCommon.FtpProtocolVersions
                };
                clientFTP = new FtpClient()
                {
                    Host = Address,
                    Port = Port,
                    Config = ftpConfig
                };
            }

            // Prompt for password
            if (!string.IsNullOrWhiteSpace(FTPShellCommon.FtpPassPromptStyle))
                TextWriterColor.Write(PlaceParse.ProbePlaces(FTPShellCommon.FtpPassPromptStyle), false, KernelColorType.Input, user);
            else
                TextWriterColor.Write(Translate.DoTranslation("Password for {0}: "), false, KernelColorType.Input, user);

            // Get input
            FTPShellCommon.FtpPass = Input.ReadLineNoInput();

            // Set up credentials
            clientFTP.Credentials = new NetworkCredential(user, FTPShellCommon.FtpPass);

            // Connect to FTP
            return ConnectFTP(clientFTP);
        }

        /// <summary>
        /// Tries to connect to the FTP server
        /// </summary>
        /// <param name="address">An FTP server. You may specify it like "[address]" or "[address]:[port]"</param>
        public static NetworkConnection TryToConnect(string address)
        {
            try
            {
                // Create an FTP stream to connect to
                int indexOfPort = address.LastIndexOf(":");
                string FtpHost       = address.Replace("ftpes://", "").Replace("ftps://", "").Replace("ftp://", "");
                FtpHost = indexOfPort < 0 ? FtpHost : FtpHost.Replace(FtpHost[FtpHost.LastIndexOf(":")..], "");
                string FtpPortString = address.Replace("ftpes://", "").Replace("ftps://", "").Replace("ftp://", "").Replace(FtpHost + ":", "");
                DebugWriter.WriteDebug(DebugLevel.W, "Host: {0}, Port: {1}", FtpHost, FtpPortString);
                bool portParsed = int.TryParse(FtpHost == FtpPortString ? "0" : FtpPortString, out int FtpPort);
                if (!portParsed)
                {
                    TextWriterColor.Write(Translate.DoTranslation("Make sure that you specify the port correctly."), true, KernelColorType.Error);
                    return null;
                }

                // Make a new FTP client object instance
                FtpConfig ftpConfig = new()
                {
                    RetryAttempts = FTPShellCommon.FtpVerifyRetryAttempts,
                    ConnectTimeout = FTPShellCommon.FtpConnectTimeout,
                    DataConnectionConnectTimeout = FTPShellCommon.FtpDataConnectTimeout,
                    EncryptionMode = FtpEncryptionMode.Auto,
                    InternetProtocolVersions = FTPShellCommon.FtpProtocolVersions
                };
                FtpClient _clientFTP = new()
                {
                    Host = FtpHost,
                    Port = FtpPort,
                    Config = ftpConfig
                };

                // Add handler for SSL validation
                if (FTPShellCommon.FtpTryToValidateCertificate)
                    _clientFTP.ValidateCertificate += new FtpSslValidation(TryToValidate);

                // Prompt for username
                if (!string.IsNullOrWhiteSpace(FTPShellCommon.FtpUserPromptStyle))
                {
                    TextWriterColor.Write(PlaceParse.ProbePlaces(FTPShellCommon.FtpUserPromptStyle), false, KernelColorType.Input, address);
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Username for {0}: "), false, KernelColorType.Input, address);
                }
                FTPShellCommon.FtpUser = Input.ReadLine();
                if (string.IsNullOrEmpty(FTPShellCommon.FtpUser))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "User is not provided. Fallback to \"anonymous\"");
                    FTPShellCommon.FtpUser = "anonymous";
                }

                // If we didn't abort, prompt for password
                return PromptForPassword(_clientFTP, FTPShellCommon.FtpUser);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Error connecting to {0}: {1}", address, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("Error when trying to connect to {0}: {1}"), true, KernelColorType.Error, address, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Tries to connect to the FTP server.
        /// </summary>
        private static NetworkConnection ConnectFTP(FtpClient clientFTP)
        {
            // Prepare profiles
            TextWriterColor.Write(Translate.DoTranslation("Preparing profiles... It could take several minutes..."));
            var profiles = clientFTP.AutoDetect(Flags.FTPFirstProfileOnly);
            var profsel = new FtpProfile();
            DebugWriter.WriteDebug(DebugLevel.I, "Profile count: {0}", profiles.Count);
            if (profiles.Count > 1)
            {
                // More than one profile
                if (FTPShellCommon.FtpUseFirstProfile)
                    profsel = profiles[0];
                else
                {
                    string profanswer;
                    var profanswered = false;
                    var ProfHeaders = new[] { "#", Translate.DoTranslation("Host Name"), Translate.DoTranslation("Username"), Translate.DoTranslation("Data Type"), Translate.DoTranslation("Encoding"), Translate.DoTranslation("Encryption"), Translate.DoTranslation("Protocols") };
                    var ProfData = new string[profiles.Count, 7];
                    TextWriterColor.Write(Translate.DoTranslation("More than one profile found. Select one:"));
                    for (int i = 0; i <= profiles.Count - 1; i++)
                    {
                        ProfData[i, 0] = (i + 1).ToString();
                        ProfData[i, 1] = profiles[i].Host;
                        ProfData[i, 2] = profiles[i].Credentials.UserName;
                        ProfData[i, 3] = profiles[i].DataConnection.ToString();
                        ProfData[i, 4] = profiles[i].Encoding.EncodingName;
                        ProfData[i, 5] = profiles[i].Encryption.ToString();
                        ProfData[i, 6] = profiles[i].Protocols.ToString();
                    }
                    TableColor.WriteTable(ProfHeaders, ProfData, 2);
                    while (!profanswered)
                    {
                        TextWriterColor.Write(CharManager.NewLine + ">> ", false, KernelColorType.Input);
                        profanswer = Input.ReadLine();
                        DebugWriter.WriteDebug(DebugLevel.I, "Selection: {0}", profanswer);
                        if (TextTools.IsStringNumeric(profanswer))
                        {
                            try
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Profile selected");
                                int AnswerNumber = Convert.ToInt32(profanswer);
                                profsel = profiles[AnswerNumber - 1];
                                profanswered = true;
                            }
                            catch (Exception ex)
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Profile invalid");
                                TextWriterColor.Write(Translate.DoTranslation("Invalid profile selection.") + CharManager.NewLine, true, KernelColorType.Error);
                                DebugWriter.WriteDebugStackTrace(ex);
                            }
                        }
                    }
                }
            }
            else if (profiles.Count == 1)
                // Select first profile
                profsel = profiles[0];
            else
            {
                // Failed trying to get profiles
                TextWriterColor.Write(Translate.DoTranslation("Error when trying to connect to {0}: Connection timeout or lost connection"), true, KernelColorType.Error, clientFTP.Host);
                return null;
            }

            // Connect
            TextWriterColor.Write(Translate.DoTranslation("Trying to connect to {0} with profile {1}..."), clientFTP.Host, profiles.IndexOf(profsel));
            DebugWriter.WriteDebug(DebugLevel.I, "Connecting to {0} with {1}...", clientFTP.Host, profiles.IndexOf(profsel));
            clientFTP.Connect(profsel);
            var ftpConnection = NetworkConnectionTools.EstablishConnection("FTP connection", clientFTP.Host, NetworkConnectionType.FTP, clientFTP);

            // Show that it's connected
            TextWriterColor.Write(Translate.DoTranslation("Connected to {0}"), true, KernelColorType.Success, clientFTP.Host);
            DebugWriter.WriteDebug(DebugLevel.I, "Connected.");
            return ftpConnection;
        }

        /// <summary>
        /// Tries to validate certificate
        /// </summary>
        public static void TryToValidate(BaseFtpClient control, FtpSslValidationEventArgs e)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Certificate checks");
            if (e.PolicyErrors == SslPolicyErrors.None)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Certificate accepted.");
                DebugWriter.WriteDebug(DebugLevel.I, e.Certificate.GetRawCertDataString());
                e.Accept = true;
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, $"Certificate error is {e.PolicyErrors}");
                TextWriterColor.Write(Translate.DoTranslation("During certificate validation, there are certificate errors. It might be the first time you've connected to the server or the certificate might have been expired. Here's an error:"), true, KernelColorType.Error);
                TextWriterColor.Write("- {0}", true, KernelColorType.Error, e.PolicyErrors.ToString());
                if (FTPShellCommon.FtpAlwaysAcceptInvalidCerts)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Certificate accepted, although there are errors.");
                    DebugWriter.WriteDebug(DebugLevel.I, e.Certificate.GetRawCertDataString());
                    e.Accept = true;
                }
                else
                {
                    string Answer = "";
                    while (!(Answer.ToLower() == "y" | Answer.ToLower() == "n"))
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Are you sure that you want to connect?") + " (y/n) ", false, KernelColorType.Question);
                        KernelColorTools.SetConsoleColor(KernelColorTools.GetColor(KernelColorType.Input));
                        Answer = Convert.ToString(Input.DetectKeypress().KeyChar);
                        TextWriterColor.Write();
                        DebugWriter.WriteDebug(DebugLevel.I, $"Answer is {Answer}");
                        if (Answer.ToLower() == "y")
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Certificate accepted, although there are errors.");
                            DebugWriter.WriteDebug(DebugLevel.I, e.Certificate.GetRawCertDataString());
                            e.Accept = true;
                        }
                        else if (Answer.ToLower() != "n")
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Invalid answer.");
                            TextWriterColor.Write(Translate.DoTranslation("Invalid answer. Please try again."), true, KernelColorType.Error);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Opens speed dial prompt
        /// </summary>
        [Obsolete("NetworkConnection will be able to handle this. It'll be removed in Nitrocid 0.1.0 Beta 3.")]
        public static void QuickConnect()
        {
            var quickConnectInfo = SpeedDialTools.GetQuickConnectInfo();
            string Address = (string)quickConnectInfo["Address"];
            string Port = (string)quickConnectInfo["Port"];
            string Username = (string)quickConnectInfo["Options"][0];
            FtpEncryptionMode Encryption = (FtpEncryptionMode)Convert.ToInt32(Enum.Parse(typeof(FtpEncryptionMode), (string)quickConnectInfo["Options"][1]));
            DebugWriter.WriteDebug(DebugLevel.I, "Address: {0}, Port: {1}, Username: {2}, Encryption: {3}", Address, Port, Username, Encryption);
            PromptForPassword(null, Username, Address, Convert.ToInt32(Port), Encryption);
        }

    }
}
