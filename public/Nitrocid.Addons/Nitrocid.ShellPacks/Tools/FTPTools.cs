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
using System.Net;
using System.Net.Security;
using FluentFTP;
using FluentFTP.Client.BaseClient;
using Nitrocid.Base.Kernel.Debugging;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Languages;
using Textify.Tools.Placeholder;
using Terminaux.Colors.Themes.Colors;
using Textify.General;
using Terminaux.Colors;
using Nitrocid.Base.ConsoleBase.Inputs;
using Nitrocid.Base.Network.Connections;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles.Choice;
using System.Collections.Generic;
using Terminaux.Inputs.Styles;
using Nitrocid.ShellPacks.Shells.FTP;

namespace Nitrocid.ShellPacks.Tools
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
        public static NetworkConnection? PromptForPassword(FtpClient? clientFTP, string user, string Address = "", int Port = 0, FtpEncryptionMode EncryptionMode = FtpEncryptionMode.Explicit)
        {
            // Make a new FTP client object instance (Used in case logging in using speed dial)
            if (clientFTP is null)
            {
                var ftpConfig = new FtpConfig()
                {
                    RetryAttempts = ShellsInit.ShellsConfig.FtpVerifyRetryAttempts,
                    ConnectTimeout = ShellsInit.ShellsConfig.FtpConnectTimeout,
                    DataConnectionConnectTimeout = ShellsInit.ShellsConfig.FtpDataConnectTimeout,
                    EncryptionMode = EncryptionMode,
                    InternetProtocolVersions = (FtpIpVersion)ShellsInit.ShellsConfig.FtpProtocolVersions
                };
                clientFTP = new FtpClient()
                {
                    Host = Address,
                    Port = Port,
                    Config = ftpConfig
                };
            }

            // Prompt for password
            if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.FtpPassPromptStyle))
                TextWriterColor.Write(PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.FtpPassPromptStyle), false, ThemeColorType.Input, user);
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_PROMPTPASSWORD"), false, ThemeColorType.Input, user);

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
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_CORRECTPORTREQUIRED"), true, ThemeColorType.Error);
                    return null;
                }

                // Make a new FTP client object instance
                FtpConfig ftpConfig = new()
                {
                    RetryAttempts = ShellsInit.ShellsConfig.FtpVerifyRetryAttempts,
                    ConnectTimeout = ShellsInit.ShellsConfig.FtpConnectTimeout,
                    DataConnectionConnectTimeout = ShellsInit.ShellsConfig.FtpDataConnectTimeout,
                    EncryptionMode = FtpEncryptionMode.Auto,
                    InternetProtocolVersions = (FtpIpVersion)ShellsInit.ShellsConfig.FtpProtocolVersions
                };
                FtpClient _clientFTP = new()
                {
                    Host = FtpHost,
                    Port = FtpPort,
                    Config = ftpConfig
                };

                // Add handler for SSL validation
                if (ShellsInit.ShellsConfig.FtpTryToValidateCertificate)
                    _clientFTP.ValidateCertificate += new FtpSslValidation(TryToValidate);

                // Prompt for username
                if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.FtpUserPromptStyle))
                    TextWriterColor.Write(PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.FtpUserPromptStyle), false, ThemeColorType.Input, address);
                else
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_PROMPTUSERNAME"), false, ThemeColorType.Input, address);
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
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_ERRORCONNECTING"), true, ThemeColorType.Error, address, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Tries to connect to the FTP server.
        /// </summary>
        private static NetworkConnection? ConnectFTP(FtpClient clientFTP)
        {
            // Prepare profiles
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_PREPARINGPROFILES"));
            var profiles = clientFTP.AutoDetect(ShellsInit.ShellsConfig.FtpFirstProfileOnly);
            var profsel = new FtpProfile();
            DebugWriter.WriteDebug(DebugLevel.I, "Profile count: {0}", vars: [profiles.Count]);
            if (profiles.Count > 1)
            {
                // More than one profile
                if (ShellsInit.ShellsConfig.FtpUseFirstProfile)
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
                            LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECT_LISTPROFILE_PROMPT") +
                            "\n###: {0}, {1}, {2}, {3}, {4}, {5}".FormatString(
                                LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECT_LISTPROFILE_HOSTNAME"),
                                LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECT_LISTPROFILE_USERNAME"),
                                LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECT_LISTPROFILE_DATATYPE"),
                                LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECT_LISTPROFILE_ENCODING"),
                                LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECT_LISTPROFILE_ENCRYPTION"),
                                LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECT_LISTPROFILE_PROTOCOLS")
                            ), [.. choices], new()
                            {
                                OutputType = ChoiceOutputType.Modern
                            });
                        DebugWriter.WriteDebug(DebugLevel.I, "Selection: {0}", vars: [profanswer]);
                        if (profanswer.IsStringNumeric())
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
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_INVALIDPROFILE") + CharManager.NewLine, true, ThemeColorType.Error);
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
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_NOPROFILESORTIMEOUT"), true, ThemeColorType.Error, clientFTP.Host);
                return null;
            }

            // Connect
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECTING"), clientFTP.Host, profiles.IndexOf(profsel));
            DebugWriter.WriteDebug(DebugLevel.I, "Connecting to {0} with {1}...", vars: [clientFTP.Host, profiles.IndexOf(profsel)]);
            clientFTP.Connect(profsel);
            var ftpConnection = NetworkConnectionTools.EstablishConnection("FTP connection", clientFTP.Host, NetworkConnectionType.FTP, clientFTP);

            // Show that it's connected
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_CONNECTEDTO"), true, ThemeColorType.Success, clientFTP.Host);
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
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_VALIDATIONFAILED_MESSAGE"), true, ThemeColorType.Error);
                TextWriterColor.Write("- {0}", true, ThemeColorType.Error, e.PolicyErrors.ToString());
                if (ShellsInit.ShellsConfig.FtpAlwaysAcceptInvalidCerts)
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
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_VALIDATIONFAILED_PROMPT") + " (y/n) ", false, ThemeColorType.Question);
                        ColorTools.SetConsoleColor(ThemeColorsTools.GetColor(ThemeColorType.Input));
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
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_VALIDATIONFAILED_INVALID"), true, ThemeColorType.Error);
                        }
                    }
                }
            }
        }
    }
}
