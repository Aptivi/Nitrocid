
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
using System.Data;
using System.Linq;
using System.Net;
using Addresstigator;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Probers;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.Mail.PGP;
using KS.Shell.ShellBase.Shells;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit.Cryptography;

namespace KS.Network.Mail
{
    static class MailLogin
    {

        // Variables
        public static ImapClient IMAP_Client = new();
        public static SmtpClient SMTP_Client = new();
        internal static NetworkCredential Mail_Authentication = new();
        public static string Mail_UserPromptStyle = "";
        public static string Mail_PassPromptStyle = "";
        public static string Mail_IMAPPromptStyle = "";
        public static string Mail_SMTPPromptStyle = "";
        public static string Mail_GPGPromptStyle = "";
        public static bool Mail_Debug;
        public static bool Mail_AutoDetectServer = true;

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
        public static void PromptUser()
        {
            // Username or mail address
            if (!string.IsNullOrWhiteSpace(Mail_UserPromptStyle))
            {
                TextWriterColor.Write(PlaceParse.ProbePlaces(Mail_UserPromptStyle), false, KernelColorType.Input);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Enter username or mail address: "), false, KernelColorType.Input);
            }

            // Try to get the username or e-mail address from the input
            string InputMailAddress = Input.ReadLine();
            if (!string.IsNullOrWhiteSpace(Mail_UserPromptStyle))
            {
                PromptPassword(InputMailAddress);
            }
        }

        /// <summary>
        /// Prompts user to enter password
        /// </summary>
        /// <param name="Username">Specified username</param>
        public static void PromptPassword(string Username)
        {
            // Password
            DebugWriter.WriteDebug(DebugLevel.I, "Username: {0}", Username);
            Mail_Authentication.UserName = Username;
            if (!string.IsNullOrWhiteSpace(Mail_PassPromptStyle))
            {
                TextWriterColor.Write(PlaceParse.ProbePlaces(Mail_PassPromptStyle), false, KernelColorType.Input);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Enter password: "), false, KernelColorType.Input);
            }
            Mail_Authentication.Password = Input.ReadLineNoInput();

            string DynamicAddressIMAP = ServerDetect(Username, ServerType.IMAP);
            string DynamicAddressSMTP = ServerDetect(Username, ServerType.SMTP);

            if (!string.IsNullOrEmpty(DynamicAddressIMAP) & !string.IsNullOrEmpty(DynamicAddressSMTP) & Mail_AutoDetectServer)
            {
                ParseAddresses(DynamicAddressIMAP, 0, DynamicAddressSMTP, 0);
            }
            else
            {
                PromptServer();
            }
        }

        /// <summary>
        /// Prompts for server
        /// </summary>
        public static void PromptServer()
        {
            string IMAP_Address;
            var IMAP_Port = 0;
            string SMTP_Address = "";
            int SMTP_Port;
            // IMAP server address and port
            if (!string.IsNullOrWhiteSpace(Mail_IMAPPromptStyle))
            {
                TextWriterColor.Write(PlaceParse.ProbePlaces(Mail_IMAPPromptStyle), false, KernelColorType.Input);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Enter IMAP server address and port (<address> or <address>:[port]): "), false, KernelColorType.Input);
            }
            IMAP_Address = Input.ReadLine(false);
            DebugWriter.WriteDebug(DebugLevel.I, "IMAP Server: \"{0}\"", IMAP_Address);

            // SMTP server address and port
            if (!string.IsNullOrWhiteSpace(Mail_SMTPPromptStyle))
            {
                TextWriterColor.Write(PlaceParse.ProbePlaces(Mail_SMTPPromptStyle), false, KernelColorType.Input);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Enter SMTP server address and port (<address> or <address>:[port]): "), false, KernelColorType.Input);
            }
            SMTP_Address = Input.ReadLine(false);
            SMTP_Port = 587;
            DebugWriter.WriteDebug(DebugLevel.I, "SMTP Server: \"{0}\"", SMTP_Address);

            // Parse addresses to connect
            ParseAddresses(IMAP_Address, IMAP_Port, SMTP_Address, SMTP_Port);
        }

        public static void ParseAddresses(string IMAP_Address, int IMAP_Port, string SMTP_Address, int SMTP_Port)
        {
            // If the address is <address>:[port]
            if (IMAP_Address.Contains(":"))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Found colon in address. Separating...", Mail_Authentication.UserName);
                IMAP_Port = Convert.ToInt32(IMAP_Address.Substring(IMAP_Address.IndexOf(":") + 1));
                IMAP_Address = IMAP_Address.Remove(IMAP_Address.IndexOf(":"));
                DebugWriter.WriteDebug(DebugLevel.I, "Final address: {0}, Final port: {1}", IMAP_Address, IMAP_Port);
            }

            // If the address is <address>:[port]
            if (SMTP_Address.Contains(":"))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Found colon in address. Separating...", Mail_Authentication.UserName);
                SMTP_Port = Convert.ToInt32(SMTP_Address.Substring(SMTP_Address.IndexOf(":") + 1));
                SMTP_Address = SMTP_Address.Remove(SMTP_Address.IndexOf(":"));
                DebugWriter.WriteDebug(DebugLevel.I, "Final address: {0}, Final port: {1}", SMTP_Address, SMTP_Port);
            }

            // Try to connect
            Mail_Authentication.Domain = IMAP_Address;
            ConnectShell(IMAP_Address, IMAP_Port, SMTP_Address, SMTP_Port);
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
            var DynamicConfiguration = Tools.GetIspConfig(Address);
            string ReturnedMailAddress = "";
            var ReturnedMailPort = 0;
            switch (Type)
            {
                case ServerType.IMAP:
                    {
                        var ImapServers = DynamicConfiguration.EmailProvider.IncomingServer.Select(x => x).Where(x => x.Type == "imap");
                        if (ImapServers.Count() > 0)
                        {
                            var ImapServer = ImapServers.ElementAtOrDefault(0);
                            ReturnedMailAddress = ImapServer.Hostname;
                            ReturnedMailPort = ImapServer.Port;
                        }

                        break;
                    }
                case ServerType.SMTP:
                    {
                        var SmtpServer = DynamicConfiguration.EmailProvider.OutgoingServer;
                        ReturnedMailAddress = SmtpServer?.Hostname;
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
        public static void ConnectShell(string Address, int Port, string SmtpAddress, int SmtpPort)
        {
            try
            {
                // Register the context and initialize the loggers if debug mode is on
                if (Flags.DebugMode & Mail_Debug)
                {
                    IMAP_Client = new ImapClient(new ProtocolLogger(Paths.HomePath + "/ImapDebug.log") { LogTimestamps = true, RedactSecrets = true, ClientPrefix = "KS:  ", ServerPrefix = "SRV: " });
                    SMTP_Client = new SmtpClient(new ProtocolLogger(Paths.HomePath + "/SmtpDebug.log") { LogTimestamps = true, RedactSecrets = true, ClientPrefix = "KS:  ", ServerPrefix = "SRV: " });
                }
                CryptographyContext.Register(typeof(PGPContext));

                // IMAP Connection
                TextWriterColor.Write(Translate.DoTranslation("Connecting to {0}..."), Address);
                DebugWriter.WriteDebug(DebugLevel.I, "Connecting to IMAP Server {0}:{1} with SSL...", Address, Port);
                IMAP_Client.Connect(Address, Port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                IMAP_Client.WebAlert += MailHandlers.HandleWebAlert;

                // SMTP Connection
                TextWriterColor.Write(Translate.DoTranslation("Connecting to {0}..."), SmtpAddress);
                DebugWriter.WriteDebug(DebugLevel.I, "Connecting to SMTP Server {0}:{1} with SSL...", Address, Port);
                SMTP_Client.Connect(SmtpAddress, SmtpPort, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);

                // IMAP Authentication
                TextWriterColor.Write(Translate.DoTranslation("Authenticating..."));
                DebugWriter.WriteDebug(DebugLevel.I, "Authenticating {0} to IMAP server {1}...", Mail_Authentication.UserName, Address);
                IMAP_Client.Authenticate(Mail_Authentication);

                // SMTP Authentication
                DebugWriter.WriteDebug(DebugLevel.I, "Authenticating {0} to SMTP server {1}...", Mail_Authentication.UserName, SmtpAddress);
                SMTP_Client.Authenticate(Mail_Authentication);
                IMAP_Client.WebAlert -= MailHandlers.HandleWebAlert;

                // Initialize shell
                DebugWriter.WriteDebug(DebugLevel.I, "Authentication succeeded. Opening shell...");
                ShellStart.StartShell(ShellType.MailShell);
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(Translate.DoTranslation("Error while connecting to {0}: {1}"), true, KernelColorType.Error, Address, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                IMAP_Client.Disconnect(true);
                SMTP_Client.Disconnect(true);
            }
        }

    }
}
