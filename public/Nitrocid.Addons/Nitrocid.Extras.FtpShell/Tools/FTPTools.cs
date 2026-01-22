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
using System.Net;
using System.Net.Security;
using FluentFTP;
using FluentFTP.Client.BaseClient;
using Nitrocid.Extras.FtpShell.FTP;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Textify.Tools.Placeholder;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;
using Terminaux.Colors;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.Network.Connections;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles.Choice;
using System.Collections.Generic;
using Terminaux.Inputs.Styles;
using Terminaux.Base.Extensions;

namespace Nitrocid.Extras.FtpShell.Tools
{
    /// <summary>
    /// FTP tools class
    /// </summary>
    public static class FTPTools
    {

        /// <summary>
        /// Log username for FTP
        /// </summary>
        public static bool FtpLoggerUsername =>
            FtpShellInit.FtpConfig.FtpLoggerUsername;
        /// <summary>
        /// Log IP address for FTP
        /// </summary>
        public static bool FtpLoggerIP =>
            FtpShellInit.FtpConfig.FtpLoggerIP;
        /// <summary>
        /// Only first profile will be returned
        /// </summary>
        public static bool FtpFirstProfileOnly =>
            FtpShellInit.FtpConfig.FtpFirstProfileOnly;

        /// <summary>
        /// Prompts user for a password
        /// </summary>
        /// <param name="clientFTP">FTP client</param>
        /// <param name="user">A user name</param>
        /// <param name="Address">A host address</param>
        /// <param name="Port">A port for the address</param>
        /// <param name="EncryptionMode">FTP encryption mode</param>
        public static NetworkConnection? PromptForPassword(FtpClient? clientFTP, string user, string Address = "", int Port = 0, FtpEncryptionMode EncryptionMode = FtpEncryptionMode.Explicit)
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
                TextWriters.Write(PlaceParse.ProbePlaces(FTPShellCommon.FtpPassPromptStyle), false, KernelColorType.Input, user);
            else
                TextWriters.Write(Translate.DoTranslation("Password for {0}: "), false, KernelColorType.Input, user);

            // Get input
            FTPShellCommon.FtpPass = InputTools.ReadLineNoInput();

            // Set up credentials
            clientFTP.Credentials = new NetworkCredential(user, FTPShellCommon.FtpPass);

            // Connect to FTP
            return ConnectFTP(clientFTP);
        }

        /// <summary>
        /// Tries to connect to the FTP server
        /// </summary>
        /// <param name="address">An FTP server. You may specify it like "[address]" or "[address]:[port]"</param>
        public static NetworkConnection? TryToConnect(string address)
        {
            try
            {
                // Create an FTP stream to connect to
                int indexOfPort = address.LastIndexOf(":");
                string FtpHost = address.Replace("ftpes://", "").Replace("ftps://", "").Replace("ftp://", "");
                FtpHost = indexOfPort < 0 ? FtpHost : FtpHost.Replace(FtpHost[FtpHost.LastIndexOf(":")..], "");
                string FtpPortString = address.Replace("ftpes://", "").Replace("ftps://", "").Replace("ftp://", "").Replace(FtpHost + ":", "");
                DebugWriter.WriteDebug(DebugLevel.W, "Host: {0}, Port: {1}", vars: [FtpHost, FtpPortString]);
                bool portParsed = int.TryParse(FtpHost == FtpPortString ? "0" : FtpPortString, out int FtpPort);
                if (!portParsed)
                {
                    TextWriters.Write(Translate.DoTranslation("Make sure that you specify the port correctly."), true, KernelColorType.Error);
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
                    TextWriters.Write(PlaceParse.ProbePlaces(FTPShellCommon.FtpUserPromptStyle), false, KernelColorType.Input, address);
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Username for {0}: "), false, KernelColorType.Input, address);
                }
                FTPShellCommon.FtpUser = InputTools.ReadLine();
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
                DebugWriter.WriteDebug(DebugLevel.W, "Error connecting to {0}: {1}", vars: [address, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriters.Write(Translate.DoTranslation("Error when trying to connect to {0}: {1}"), true, KernelColorType.Error, address, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Tries to connect to the FTP server.
        /// </summary>
        private static NetworkConnection? ConnectFTP(FtpClient clientFTP)
        {
            // Prepare profiles
            TextWriterColor.Write(Translate.DoTranslation("Preparing profiles... It could take several minutes..."));
            var profiles = clientFTP.AutoDetect(FtpFirstProfileOnly);
            var profsel = new FtpProfile();
            DebugWriter.WriteDebug(DebugLevel.I, "Profile count: {0}", vars: [profiles.Count]);
            if (profiles.Count > 1)
            {
                // More than one profile
                if (FTPShellCommon.FtpUseFirstProfile)
                    profsel = profiles[0];
                else
                {
                    string profanswer;
                    var profanswered = false;
                    List<InputChoiceInfo> choices = [];
                    for (int i = 0; i <= profiles.Count - 1; i++)
                    {
                        var profile = profiles[i];
                        choices.Add(
                            new InputChoiceInfo($"{i + 1}", $"{profile.Host}, {profile.Credentials.UserName}, {profile.DataConnection}, {profile.Encoding.EncodingName}, {profile.Encryption}, {profile.Protocols}")
                        );
                    }
                    while (!profanswered)
                    {
                        profanswer = ChoiceStyle.PromptChoice(
                            Translate.DoTranslation("More than one profile found. Select one:") +
                            "\n###: {0}, {1}, {2}, {3}, {4}, {5}".FormatString(
                                Translate.DoTranslation("Host Name"),
                                Translate.DoTranslation("Username"),
                                Translate.DoTranslation("Data Type"),
                                Translate.DoTranslation("Encoding"),
                                Translate.DoTranslation("Encryption"),
                                Translate.DoTranslation("Protocols")
                            ), [.. choices], new()
                            {
                                OutputType = ChoiceOutputType.Modern
                            });
                        DebugWriter.WriteDebug(DebugLevel.I, "Selection: {0}", vars: [profanswer]);
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
                                TextWriters.Write(Translate.DoTranslation("Invalid profile FilesystemTools.") + CharManager.NewLine, true, KernelColorType.Error);
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
                TextWriters.Write(Translate.DoTranslation("Error when trying to connect to {0}: Connection timeout or lost connection"), true, KernelColorType.Error, clientFTP.Host);
                return null;
            }

            // Connect
            TextWriterColor.Write(Translate.DoTranslation("Trying to connect to {0} with profile {1}..."), clientFTP.Host, profiles.IndexOf(profsel));
            DebugWriter.WriteDebug(DebugLevel.I, "Connecting to {0} with {1}...", vars: [clientFTP.Host, profiles.IndexOf(profsel)]);
            clientFTP.Connect(profsel);
            var ftpConnection = NetworkConnectionTools.EstablishConnection("FTP connection", clientFTP.Host, NetworkConnectionType.FTP, clientFTP);

            // Show that it's connected
            TextWriters.Write(Translate.DoTranslation("Connected to {0}"), true, KernelColorType.Success, clientFTP.Host);
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
                TextWriters.Write(Translate.DoTranslation("During certificate validation, there are certificate errors. It might be the first time you've connected to the server or the certificate might have been expired. Here's an error:"), true, KernelColorType.Error);
                TextWriters.Write("- {0}", true, KernelColorType.Error, e.PolicyErrors.ToString());
                if (FTPShellCommon.FtpAlwaysAcceptInvalidCerts)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Certificate accepted, although there are errors.");
                    DebugWriter.WriteDebug(DebugLevel.I, e.Certificate.GetRawCertDataString());
                    e.Accept = true;
                }
                else
                {
                    string Answer = "";
                    while (!Answer.Equals("y", StringComparison.OrdinalIgnoreCase) || !Answer.Equals("n", StringComparison.OrdinalIgnoreCase))
                    {
                        TextWriters.Write(Translate.DoTranslation("Are you sure that you want to connect?") + " (y/n) ", false, KernelColorType.Question);
                        ConsoleColoring.SetConsoleColor(KernelColorTools.GetColor(KernelColorType.Input));
                        Answer = Convert.ToString(Input.ReadKey().KeyChar);
                        TextWriterRaw.Write();
                        DebugWriter.WriteDebug(DebugLevel.I, $"Answer is {Answer}");
                        if (Answer.Equals("y", StringComparison.OrdinalIgnoreCase))
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Certificate accepted, although there are errors.");
                            DebugWriter.WriteDebug(DebugLevel.I, e.Certificate.GetRawCertDataString());
                            e.Accept = true;
                        }
                        else if (!Answer.Equals("n", StringComparison.OrdinalIgnoreCase))
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Invalid answer.");
                            TextWriters.Write(Translate.DoTranslation("Invalid answer. Please try again."), true, KernelColorType.Error);
                        }
                    }
                }
            }
        }

    }
}
