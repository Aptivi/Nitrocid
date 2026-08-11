//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Data;
using System.Linq;
using System.Net;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit.Cryptography;
using Nettify.MailAddress;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Network.Connections;
using Nitrocid.Base.Network.SpeedDial;
using Nitrocid.ShellPacks.Shells.Mail.Tools.PGP;
using SpecProbe.Software.Platform;
using Terminaux.Reader;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.Tools.Placeholder;

namespace Nitrocid.ShellPacks.Shells.Mail.Tools
{
    internal static class MailLogin
    {
        /// <summary>
        /// Mail server type
        /// </summary>
        public enum ServerType
        {
            /// <summary>
            /// The IMAP server
            /// </summary>
            IMAP,
            /// <summary>
            /// The SMTP server
            /// </summary>
            SMTP,
            /// <summary>
            /// The POP3 server
            /// </summary>
            POP3,
        }

        /// <summary>
        /// Prompts user to enter username or e-mail address
        /// </summary>
        public static NetworkConnection? PromptUser(MailProtocolType protocolType)
        {
            // Username or mail address
            if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.MailUserPromptStyle))
                TextWriterColor.Write(PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.MailUserPromptStyle), false, ThemeColorType.Input);
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_ADDRESSPROMPT"), false, ThemeColorType.Input);

            // Try to get the username or e-mail address from the input
            string InputMailAddress = TermReader.Read();
            return PromptPassword(InputMailAddress, protocolType);
        }

        /// <summary>
        /// Prompts user to enter password
        /// </summary>
        /// <param name="Username">Specified username</param>
        /// <param name="protocolType">Protocol type</param>
        public static NetworkConnection? PromptPassword(string Username, MailProtocolType protocolType)
        {
            NetworkCredential Authentication = new();

            // Password
            DebugWriter.WriteDebug(DebugLevel.I, "Username: {0}", vars: [Username]);
            Authentication.UserName = Username;
            if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.MailPassPromptStyle))
                TextWriterColor.Write(PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.MailPassPromptStyle), false, ThemeColorType.Input);
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_PASSWORDPROMPT"), false, ThemeColorType.Input);
            Authentication.Password = TermReader.Read(password: true);

            string DynamicAddressIMAP = ShellsInit.ShellsConfig.MailAutoDetectServer ? ServerDetect(Username, protocolType == MailProtocolType.POP3 ? ServerType.POP3 : ServerType.IMAP) : "";
            string DynamicAddressSMTP = ShellsInit.ShellsConfig.MailAutoDetectServer ? ServerDetect(Username, ServerType.SMTP) : "";

            if (!string.IsNullOrEmpty(DynamicAddressIMAP) && !string.IsNullOrEmpty(DynamicAddressSMTP))
                return ParseAddresses(DynamicAddressIMAP, 0, DynamicAddressSMTP, 0, Authentication, protocolType);
            else
                return PromptServer(Authentication, protocolType);
        }

        /// <summary>
        /// Prompts for server
        /// </summary>
        public static NetworkConnection? PromptServer(NetworkCredential authentication, MailProtocolType protocolType)
        {
            string IMAP_Address;
            var IMAP_Port = 0;
            int SMTP_Port;

            // IMAP or POP3 server address and port
            string finalPrompt = "";
            if (protocolType == MailProtocolType.POP3)
            {
                // TODO: NKS_SHELLPACKS_MAIL_POP3SERVERPROMPT -> "Enter IMAP server address and port (<address> or <address>:[port]): "
                finalPrompt = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_POP3SERVERPROMPT");
                if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.MailPOP3PromptStyle))
                    finalPrompt = PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.MailPOP3PromptStyle);
            }
            else
            {
                finalPrompt = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_IMAPSERVERPROMPT");
                if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.MailIMAPPromptStyle))
                    finalPrompt = PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.MailIMAPPromptStyle);
            }
            IMAP_Address = TermReader.Read(finalPrompt);
            DebugWriter.WriteDebug(DebugLevel.I, "IMAP/POP3 Server: \"{0}\"", vars: [IMAP_Address]);

            // SMTP server address and port
            string finalSmtpPrompt = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_SMTPSERVERPROMPT");
            if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.MailSMTPPromptStyle))
                finalSmtpPrompt = PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.MailSMTPPromptStyle);
            string SMTP_Address = TermReader.Read(finalSmtpPrompt);
            SMTP_Port = 587;
            DebugWriter.WriteDebug(DebugLevel.I, "SMTP Server: \"{0}\"", vars: [SMTP_Address]);

            // Parse addresses to connect
            return ParseAddresses(IMAP_Address, IMAP_Port, SMTP_Address, SMTP_Port, authentication, protocolType);
        }

        public static NetworkConnection? ParseAddresses(string IMAP_Address, int IMAP_Port, string SMTP_Address, int SMTP_Port, NetworkCredential authentication, MailProtocolType protocolType)
        {
            // If the address is <address>:[port]
            if (IMAP_Address.Contains(':'))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Found colon in address. Separating...");
                IMAP_Port = Convert.ToInt32(IMAP_Address[(IMAP_Address.IndexOf(":") + 1)..]);
                IMAP_Address = IMAP_Address.Remove(IMAP_Address.IndexOf(":"));
                DebugWriter.WriteDebug(DebugLevel.I, "Final address: {0}, Final port: {1}", vars: [IMAP_Address, IMAP_Port]);
            }

            // If the address is <address>:[port]
            if (SMTP_Address.Contains(':'))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Found colon in address. Separating...");
                SMTP_Port = Convert.ToInt32(SMTP_Address[(SMTP_Address.IndexOf(":") + 1)..]);
                SMTP_Address = SMTP_Address.Remove(SMTP_Address.IndexOf(":"));
                DebugWriter.WriteDebug(DebugLevel.I, "Final address: {0}, Final port: {1}", vars: [SMTP_Address, SMTP_Port]);
            }

            // Try to connect
            authentication.Domain = IMAP_Address;
            return ConnectShell(IMAP_Address, IMAP_Port, SMTP_Address, SMTP_Port, authentication, protocolType);
        }

        /// <summary>
        /// Detects servers based on dictionary
        /// </summary>
        /// <param name="Address">E-mail address</param>
        /// <param name="Type">Server type</param>
        /// <returns>Server address. Otherwise, null.</returns>
        public static string ServerDetect(string Address, ServerType Type)
        {
            // Get the mail server dynamically
            var DynamicConfiguration = IspTools.GetIspConfig(Address);
            string ReturnedMailAddress = "";
            var ReturnedMailPort = 0;
            switch (Type)
            {
                case ServerType.IMAP:
                    {
                        var ImapServers = DynamicConfiguration.EmailProvider?.IncomingServer?.Select(x => x).Where(x => x.Type == "imap");
                        if (ImapServers is not null && ImapServers.Any())
                        {
                            var ImapServer = ImapServers.ElementAtOrDefault(0) ??
                                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_NOIMAP"));
                            ReturnedMailAddress = ImapServer.Hostname;
                            ReturnedMailPort = ImapServer.Port;
                        }

                        break;
                    }
                case ServerType.POP3:
                    {
                        var Pop3Servers = DynamicConfiguration.EmailProvider?.IncomingServer?.Select(x => x).Where(x => x.Type == "pop3");
                        if (Pop3Servers is not null && Pop3Servers.Any())
                        {
                            // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_NOPOP3 -> Can't get POP3 server configuration
                            var Pop3Server = Pop3Servers.ElementAtOrDefault(0) ??
                                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_NOPOP3"));
                            ReturnedMailAddress = Pop3Server.Hostname;
                            ReturnedMailPort = Pop3Server.Port;
                        }

                        break;
                    }
                case ServerType.SMTP:
                    {
                        var SmtpServer = DynamicConfiguration.EmailProvider?.OutgoingServer ??
                            throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_NOSMTP"));
                        ReturnedMailAddress = SmtpServer.Hostname;
                        ReturnedMailPort = SmtpServer.Port;
                        break;
                    }

                default:
                    {
                        return "";
                    }
            }
            return $"{ReturnedMailAddress}:{ReturnedMailPort}";
        }

        /// <summary>
        /// Tries to connect to specified address and port with specified credentials
        /// </summary>
        /// <param name="Address">An IP address of the IMAP server</param>
        /// <param name="Port">A port of the IMAP server</param>
        /// <param name="SmtpAddress">An IP address of the SMTP server</param>
        /// <param name="SmtpPort">A port of the SMTP server</param>
        /// <param name="authentication">Authentication credentials</param>
        /// <param name="protocolType">Protocol type</param>
        public static NetworkConnection? ConnectShell(string Address, int Port, string SmtpAddress, int SmtpPort, NetworkCredential authentication, MailProtocolType protocolType)
        {
            // Make new clients
            ImapClient? IMAP_Client = protocolType == MailProtocolType.IMAP ? new() : null;
            Pop3Client? POP3_Client = protocolType == MailProtocolType.POP3 ? new() : null;
            SmtpClient SMTP_Client = new();

            // Initialize the loggers if debug mode is on
            if (KernelEntry.DebugMode & ShellsInit.ShellsConfig.MailDebug)
            {
                if (protocolType == MailProtocolType.IMAP)
                    IMAP_Client = new ImapClient(new ProtocolLogger(PathsManagement.HomePath + "/ImapDebug.log") { LogTimestamps = true, RedactSecrets = true, ClientPrefix = "KS:  ", ServerPrefix = "SRV: " });
                if (protocolType == MailProtocolType.POP3)
                    POP3_Client = new Pop3Client(new ProtocolLogger(PathsManagement.HomePath + "/Pop3Debug.log") { LogTimestamps = true, RedactSecrets = true, ClientPrefix = "KS:  ", ServerPrefix = "SRV: " });
                SMTP_Client = new SmtpClient(new ProtocolLogger(PathsManagement.HomePath + "/SmtpDebug.log") { LogTimestamps = true, RedactSecrets = true, ClientPrefix = "KS:  ", ServerPrefix = "SRV: " });
            }

            try
            {
                // Register the PGP context
                CryptographyContext.Register(typeof(PGPContext));

                // IMAP or POP3
                if (POP3_Client is not null)
                {
                    // POP3 Connection
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_CONNECTING"), Address);
                    DebugWriter.WriteDebug(DebugLevel.I, "Connecting to POP3 Server {0}:{1} with SSL...", vars: [Address, Port]);
                    POP3_Client.Connect(Address, Port, SecureSocketOptions.Auto);

                    // POP3 Authentication
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_AUTHENTICATING"));
                    DebugWriter.WriteDebug(DebugLevel.I, "Authenticating {0} to POP3 server {1}...", vars: [authentication.UserName, Address]);
                    POP3_Client.Authenticate(authentication);
                }
                else if (IMAP_Client is not null)
                {
                    // Handle web alert
                    IMAP_Client.WebAlert += HandleWebAlert;

                    // IMAP Connection
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_CONNECTING"), Address);
                    DebugWriter.WriteDebug(DebugLevel.I, "Connecting to IMAP Server {0}:{1} with SSL...", vars: [Address, Port]);
                    IMAP_Client.Connect(Address, Port, SecureSocketOptions.Auto);

                    // IMAP Authentication
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_AUTHENTICATING"));
                    DebugWriter.WriteDebug(DebugLevel.I, "Authenticating {0} to IMAP server {1}...", vars: [authentication.UserName, Address]);
                    IMAP_Client.Authenticate(authentication);

                    // Remove web alert handler
                    IMAP_Client.WebAlert -= HandleWebAlert;
                }

                // SMTP Connection
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_CONNECTING"), SmtpAddress);
                DebugWriter.WriteDebug(DebugLevel.I, "Connecting to SMTP Server {0}:{1} with SSL...", vars: [SmtpAddress, SmtpPort]);
                SMTP_Client.Connect(SmtpAddress, SmtpPort, SecureSocketOptions.Auto);

                // SMTP Authentication
                DebugWriter.WriteDebug(DebugLevel.I, "Authenticating {0} to SMTP server {1}...", vars: [authentication.UserName, SmtpAddress]);
                SMTP_Client.Authenticate(authentication);

                // Initialize shell
                DebugWriter.WriteDebug(DebugLevel.I, "Authentication succeeded. Opening shell...");
                var Client = NetworkConnectionTools.EstablishConnection("Mail client", $"mailto:{authentication.UserName}", NetworkConnectionType.Mail, new object?[] { IMAP_Client, SMTP_Client, POP3_Client, authentication, protocolType });
                SpeedDialTools.TryAddEntryToSpeedDial(Client.ConnectionUri.AbsoluteUri, Client.ConnectionUri.Port, NetworkConnectionType.Mail, authentication.UserName, authentication.Password, false);
                return Client;
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_CONNECTIONFAILED"), true, ThemeColorType.Error, Address, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                IMAP_Client?.Disconnect(true);
                POP3_Client?.Disconnect(true);
                SMTP_Client.Disconnect(true);
                return null;
            }
        }

        /// <summary>
        /// Handles WebAlert sent by Gmail
        /// </summary>
        public static void HandleWebAlert(object? sender, WebAlertEventArgs e)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "WebAlert URI: {0}", vars: [e.WebUri.AbsoluteUri]);
            TextWriterColor.Write(e.Message, true, ThemeColorType.Warning);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_WEBALERT_OPENING"));
            PlatformHelper.PlatformOpen(e.WebUri.AbsoluteUri);
        }
    }
}
