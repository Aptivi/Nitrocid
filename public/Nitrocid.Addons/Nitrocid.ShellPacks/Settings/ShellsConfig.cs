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

using FluentFTP;
using MimeKit.Text;
using Newtonsoft.Json;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Configuration.Instances;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection.Internal;
using Terminaux.Shell.Prompts;
using Nitrocid.ShellPacks.Shells.FTP;
using Nitrocid.ShellPacks.Shells.Json;
using Nitrocid.ShellPacks.Shells.Mail;
using Nitrocid.ShellPacks.Shells.RSS;

namespace Nitrocid.ShellPacks.Settings
{
    /// <summary>
    /// Configuration instance for all shells
    /// </summary>
    public class ShellsConfig : BaseKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries
        {
            get
            {
                var dataStream = ResourcesManager.GetData("ShellsSettings.json", ResourcesType.Misc, typeof(ShellsConfig).Assembly) ??
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_SETTINGS_EXCEPTION_ENTRIESFAILED"));
                string dataString = ResourcesManager.ConvertToString(dataStream);
                return ConfigTools.GetSettingsEntries(dataString);
            }
        }

        #region Archive shell
        /// <summary>
        /// Prompt Preset
        /// </summary>
        public string ArchivePromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("ArchiveShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "ArchiveShell", false);
        }
        #endregion

        #region FTP shell
        /// <summary>
        /// FTP Prompt Preset
        /// </summary>
        public string FtpPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("FTPShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "FTPShell", false);
        }
        /// <summary>
        /// How many times to verify the upload and download and retry if the verification fails before the download fails as a whole?
        /// </summary>
        public int FtpVerifyRetryAttempts
        {
            get => FTPShellCommon.verifyRetryAttempts;
            set => FTPShellCommon.verifyRetryAttempts = value < 0 ? 3 : value;
        }
        /// <summary>
        /// How many milliseconds to wait before the FTP connection timeout?
        /// </summary>
        public int FtpConnectTimeout
        {
            get => FTPShellCommon.connectTimeout;
            set => FTPShellCommon.connectTimeout = value < 0 ? 15000 : value;
        }
        /// <summary>
        /// How many milliseconds to wait before the FTP data connection timeout?
        /// </summary>
        public int FtpDataConnectTimeout
        {
            get => FTPShellCommon.dataConnectTimeout;
            set => FTPShellCommon.dataConnectTimeout = value < 0 ? 15000 : value;
        }
        /// <summary>
        /// Choose the version of Internet Protocol that the FTP server supports and that the FTP client uses
        /// </summary>
        public int FtpProtocolVersions { get; set; } = (int)FtpIpVersion.ANY;
        /// <summary>
        /// Whether or not to log FTP username
        /// </summary>
        public bool FtpLoggerUsername { get; set; }
        /// <summary>
        /// Whether or not to log FTP IP address
        /// </summary>
        public bool FtpLoggerIP { get; set; }
        /// <summary>
        /// Pick the first profile only when connecting
        /// </summary>
        public bool FtpFirstProfileOnly { get; set; }
        /// <summary>
        /// Shows the FTP file details while listing remote directories
        /// </summary>
        public bool FtpShowDetailsInList { get; set; } = true;
        /// <summary>
        /// Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string FtpUserPromptStyle { get; set; } = "";
        /// <summary>
        /// Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string FtpPassPromptStyle { get; set; } = "";
        /// <summary>
        /// Uses the first FTP profile to connect to FTP
        /// </summary>
        public bool FtpUseFirstProfile { get; set; }
        /// <summary>
        /// If enabled, adds a new connection to the FTP speed dial
        /// </summary>
        public bool FtpNewConnectionsToSpeedDial { get; set; } = true;
        /// <summary>
        /// Tries to validate the FTP certificates. Turning it off is not recommended
        /// </summary>
        public bool FtpTryToValidateCertificate { get; set; } = true;
        /// <summary>
        /// Shows the FTP message of the day on login
        /// </summary>
        public bool FtpShowMotd { get; set; } = true;
        /// <summary>
        /// Always accept invalid FTP certificates. Turning it on is not recommended as it may pose security risks
        /// </summary>
        public bool FtpAlwaysAcceptInvalidCerts { get; set; }
        /// <summary>
        /// Whether to recursively hash a directory. Please note that not all the FTP servers support that
        /// </summary>
        public bool FtpRecursiveHashing { get; set; }
        #endregion

        #region Git shell
        /// <summary>
        /// Prompt Preset
        /// </summary>
        public string GitPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("GitShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "GitShell", false);
        }
        #endregion

        #region HTTP shell
        /// <summary>
        /// HTTP Shell Prompt Preset
        /// </summary>
        public string HttpShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("HTTPShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "HTTPShell", false);
        }
        #endregion

        #region JSON shell
        /// <summary>
        /// Prompt Preset
        /// </summary>
        public string JsonShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("JsonShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "JsonShell", false);
        }
        /// <summary>
        /// Turns on or off the hex editor autosave feature
        /// </summary>
        public bool JsonEditAutoSaveFlag { get; set; } = true;
        /// <summary>
        /// If autosave is enabled, the binary file will be saved for each "n" seconds
        /// </summary>
        public int JsonEditAutoSaveInterval
        {
            get => JsonShellCommon.autoSaveInterval;
            set => JsonShellCommon.autoSaveInterval = value < 0 ? 60 : value;
        }
        /// <summary>
        /// Selects the default JSON formatting (beautified or minified) for the JSON shell to save
        /// </summary>
        public int JsonShellFormatting { get; set; } = (int)Formatting.Indented;
        #endregion

        #region Mail shell
        /// <summary>
        /// Mail Shell Prompt Preset
        /// </summary>
        public string MailPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("MailShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "MailShell", false);
        }
        /// <summary>
        /// When listing mail messages, show body preview
        /// </summary>
        public bool ShowPreview { get; set; }
        /// <summary>
        /// Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string MailUserPromptStyle { get; set; } = "";
        /// <summary>
        /// Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string MailPassPromptStyle { get; set; } = "";
        /// <summary>
        /// Write how you want your IMAP server prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string MailIMAPPromptStyle { get; set; } = "";
        /// <summary>
        /// Write how you want your SMTP server prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string MailSMTPPromptStyle { get; set; } = "";
        /// <summary>
        /// Automatically detect the mail server based on the given address
        /// </summary>
        public bool MailAutoDetectServer { get; set; } = true;
        /// <summary>
        /// Enables mail server debug
        /// </summary>
        public bool MailDebug { get; set; }
        /// <summary>
        /// Notifies you for any new mail messages
        /// </summary>
        public bool MailNotifyNewMail { get; set; } = true;
        /// <summary>
        /// Write how you want your GPG password prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string MailGPGPromptStyle { get; set; } = "";
        /// <summary>
        /// How many milliseconds to send the IMAP ping?
        /// </summary>
        public int MailImapPingInterval
        {
            get => MailShellCommon.imapPingInterval;
            set => MailShellCommon.imapPingInterval = value < 0 ? 30000 : value;
        }
        /// <summary>
        /// How many milliseconds to send the SMTP ping?
        /// </summary>
        public int MailSmtpPingInterval
        {
            get => MailShellCommon.smtpPingInterval;
            set => MailShellCommon.smtpPingInterval = value < 0 ? 30000 : value;
        }
        /// <summary>
        /// Controls how the mail text will be shown
        /// </summary>
        public int MailTextFormat { get; set; } = (int)TextFormat.Plain;
        /// <summary>
        /// How many e-mail messages to display in one page?
        /// </summary>
        public int MailMaxMessagesInPage
        {
            get => MailShellCommon.maxMessagesInPage;
            set => MailShellCommon.maxMessagesInPage = value < 0 ? 10 : value;
        }
        /// <summary>
        /// If enabled, the mail shell will show how many bytes transmitted when downloading mail.
        /// </summary>
        public bool MailShowProgress { get; set; } = true;
        /// <summary>
        /// Write how you want your mail transfer progress style to be. Leave blank to use default style. Placeholders are parsed. {0} for transferred size and {1} for total size.
        /// </summary>
        public string MailProgressStyle { get; set; } = "";
        /// <summary>
        /// Write how you want your mail transfer progress style to be. Leave blank to use default style. Placeholders are parsed. {0} for transferred size.
        /// </summary>
        public string MailProgressStyleSingle { get; set; } = "";
        #endregion

        #region RSS shell
        /// <summary>
        /// RSS Prompt Preset
        /// </summary>
        public string RSSPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("RSSShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "RSSShell", false);
        }
        /// <summary>
        /// Write how you want your RSS feed server prompt to be. Leave blank to use default style. Placeholders are parsed.
        /// </summary>
        public string RSSFeedUrlPromptStyle { get; set; } = "";
        /// <summary>
        /// Auto refresh RSS feed
        /// </summary>
        public bool RSSRefreshFeeds { get; set; } = true;
        /// <summary>
        /// How many milliseconds to refresh the RSS feed?
        /// </summary>
        public int RSSRefreshInterval
        {
            get => RSSShellCommon.refreshInterval;
            set => RSSShellCommon.refreshInterval = value < 0 ? 60000 : value;
        }
        /// <summary>
        /// How many milliseconds to wait before RSS feed fetch timeout?
        /// </summary>
        public int RSSFetchTimeout
        {
            get => RSSShellCommon.fetchTimeout;
            set => RSSShellCommon.fetchTimeout = value < 0 ? 60000 : value;
        }
        #endregion

        #region SFTP shell
        /// <summary>
        /// SFTP Prompt Preset
        /// </summary>
        public string SftpPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("SFTPShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "SFTPShell", false);
        }
        /// <summary>
        /// Shows the SFTP file details while listing remote directories
        /// </summary>
        public bool SFTPShowDetailsInList { get; set; } = true;
        /// <summary>
        /// Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string SFTPUserPromptStyle { get; set; } = "";
        /// <summary>
        /// If enabled, adds a new connection to the SFTP speed dial
        /// </summary>
        public bool SFTPNewConnectionsToSpeedDial { get; set; } = true;
        #endregion

        #region SQL shell
        /// <summary>
        /// Prompt Preset
        /// </summary>
        public string SqlShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("SqlShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "SqlShell", false);
        }
        #endregion
    }
}
