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
using System.Data;
using System.Linq;
using System.Net;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit.Cryptography;
using Nettify.MailAddress;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Textify.Tools.Placeholder;
using Nitrocid.Base.ConsoleBase.Inputs;
using Nitrocid.Base.Network.Connections;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.ShellPacks.Tools.PGP;

namespace Nitrocid.ShellPacks.Tools
{
    internal static class MailLogin
    {
        // Variables
        public static ImapClient IMAP_Client = new();
        public static SmtpClient SMTP_Client = new();
        internal static NetworkCredential Authentication = new();

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
            SMTP
        }

        /// <summary>
        /// Prompts user to enter username or e-mail address
        /// </summary>
        public static NetworkConnection? PromptUser()
        {
            // Username or mail address
            if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.MailUserPromptStyle))
                TextWriterColor.Write(PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.MailUserPromptStyle), false, ThemeColorType.Input);
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_ADDRESSPROMPT"), false, ThemeColorType.Input);

            // Try to get the username or e-mail address from the input
            string InputMailAddress = InputTools.ReadLine();
            return PromptPassword(InputMailAddress);
        }

        /// <summary>
        /// Prompts user to enter password
        /// </summary>
        /// <param name="Username">Specified username</param>
        public static NetworkConnection? PromptPassword(string Username)
        {
            // Password
            DebugWriter.WriteDebug(DebugLevel.I, "Username: {0}", vars: [Username]);
            Authentication.UserName = Username;
            if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.MailPassPromptStyle))
            {
                TextWriterColor.Write(PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.MailPassPromptStyle), false, ThemeColorType.Input);
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_PASSWORDPROMPT"), false, ThemeColorType.Input);
            }
            Authentication.Password = InputTools.ReadLineNoInput();

            string DynamicAddressIMAP = ShellsInit.ShellsConfig.MailAutoDetectServer ? ServerDetect(Username, ServerType.IMAP) : "";
            string DynamicAddressSMTP = ShellsInit.ShellsConfig.MailAutoDetectServer ? ServerDetect(Username, ServerType.SMTP) : "";

            if (!string.IsNullOrEmpty(DynamicAddressIMAP) && !string.IsNullOrEmpty(DynamicAddressSMTP))
                return ParseAddresses(DynamicAddressIMAP, 0, DynamicAddressSMTP, 0);
            else
                return PromptServer();
        }

        /// <summary>
        /// Prompts for server
        /// </summary>
        public static NetworkConnection? PromptServer()
        {
            string IMAP_Address;
            var IMAP_Port = 0;
            int SMTP_Port;

            // IMAP server address and port
            if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.MailIMAPPromptStyle))
                TextWriterColor.Write(PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.MailIMAPPromptStyle), false, ThemeColorType.Input);
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_IMAPSERVERPROMPT"), false, ThemeColorType.Input);
            IMAP_Address = InputTools.ReadLine();
            DebugWriter.WriteDebug(DebugLevel.I, "IMAP Server: \"{0}\"", vars: [IMAP_Address]);

            // SMTP server address and port
            if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.MailSMTPPromptStyle))
                TextWriterColor.Write(PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.MailSMTPPromptStyle), false, ThemeColorType.Input);
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_SMTPSERVERPROMPT"), false, ThemeColorType.Input);
            string SMTP_Address = InputTools.ReadLine();
            SMTP_Port = 587;
            DebugWriter.WriteDebug(DebugLevel.I, "SMTP Server: \"{0}\"", vars: [SMTP_Address]);

            // Parse addresses to connect
            return ParseAddresses(IMAP_Address, IMAP_Port, SMTP_Address, SMTP_Port);
        }

        public static NetworkConnection? ParseAddresses(string IMAP_Address, int IMAP_Port, string SMTP_Address, int SMTP_Port)
        {
            // If the address is <address>:[port]
            if (IMAP_Address.Contains(':'))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Found colon in address. Separating...", vars: [Authentication.UserName]);
                IMAP_Port = Convert.ToInt32(IMAP_Address[(IMAP_Address.IndexOf(":") + 1)..]);
                IMAP_Address = IMAP_Address.Remove(IMAP_Address.IndexOf(":"));
                DebugWriter.WriteDebug(DebugLevel.I, "Final address: {0}, Final port: {1}", vars: [IMAP_Address, IMAP_Port]);
            }

            // If the address is <address>:[port]
            if (SMTP_Address.Contains(':'))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Found colon in address. Separating...", vars: [Authentication.UserName]);
                SMTP_Port = Convert.ToInt32(SMTP_Address[(SMTP_Address.IndexOf(":") + 1)..]);
                SMTP_Address = SMTP_Address.Remove(SMTP_Address.IndexOf(":"));
                DebugWriter.WriteDebug(DebugLevel.I, "Final address: {0}, Final port: {1}", vars: [SMTP_Address, SMTP_Port]);
            }

            // Try to connect
            Authentication.Domain = IMAP_Address;
            return ConnectShell(IMAP_Address, IMAP_Port, SMTP_Address, SMTP_Port);
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
        public static NetworkConnection? ConnectShell(string Address, int Port, string SmtpAddress, int SmtpPort)
        {
            try
            {
                // Register the context and initialize the loggers if debug mode is on
                if (KernelEntry.DebugMode & ShellsInit.ShellsConfig.MailDebug)
                {
                    IMAP_Client = new ImapClient(new ProtocolLogger(PathsManagement.HomePath + "/ImapDebug.log") { LogTimestamps = true, RedactSecrets = true, ClientPrefix = "KS:  ", ServerPrefix = "SRV: " });
                    SMTP_Client = new SmtpClient(new ProtocolLogger(PathsManagement.HomePath + "/SmtpDebug.log") { LogTimestamps = true, RedactSecrets = true, ClientPrefix = "KS:  ", ServerPrefix = "SRV: " });
                }
                CryptographyContext.Register(typeof(PGPContext));

                // IMAP Connection
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_CONNECTING"), Address);
                DebugWriter.WriteDebug(DebugLevel.I, "Connecting to IMAP Server {0}:{1} with SSL...", vars: [Address, Port]);
                IMAP_Client.Connect(Address, Port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                IMAP_Client.WebAlert += MailHandlers.HandleWebAlert;

                // SMTP Connection
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_CONNECTING"), SmtpAddress);
                DebugWriter.WriteDebug(DebugLevel.I, "Connecting to SMTP Server {0}:{1} with SSL...", vars: [SmtpAddress, SmtpPort]);
                SMTP_Client.Connect(SmtpAddress, SmtpPort, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);

                // IMAP Authentication
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_AUTHENTICATING"));
                DebugWriter.WriteDebug(DebugLevel.I, "Authenticating {0} to IMAP server {1}...", vars: [Authentication.UserName, Address]);
                IMAP_Client.Authenticate(Authentication);

                // SMTP Authentication
                DebugWriter.WriteDebug(DebugLevel.I, "Authenticating {0} to SMTP server {1}...", vars: [Authentication.UserName, SmtpAddress]);
                SMTP_Client.Authenticate(Authentication);
                IMAP_Client.WebAlert -= MailHandlers.HandleWebAlert;

                // Initialize shell
                DebugWriter.WriteDebug(DebugLevel.I, "Authentication succeeded. Opening shell...");
                var Client = NetworkConnectionTools.EstablishConnection("Mail client", $"mailto:{Authentication.UserName}", NetworkConnectionType.Mail, new object[] { IMAP_Client, SMTP_Client });
                return Client;
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_CONNECTIONFAILED"), true, ThemeColorType.Error, Address, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                IMAP_Client.Disconnect(true);
                SMTP_Client.Disconnect(true);
                return null;
            }
        }
    }
}
