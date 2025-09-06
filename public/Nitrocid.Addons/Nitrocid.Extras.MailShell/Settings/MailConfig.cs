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

using MimeKit.Text;
using Newtonsoft.Json;
using Nitrocid.Extras.MailShell.Mail;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection.Internal;
using Terminaux.Shell.Prompts;

namespace Nitrocid.Extras.MailShell.Settings
{
    /// <summary>
    /// Configuration instance for HTTP
    /// </summary>
    public class MailConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries
        {
            get
            {
                var dataStream = ResourcesManager.GetData("MailSettings.json", ResourcesType.Misc, typeof(MailConfig).Assembly) ??
                    throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Failed to obtain settings entries."));
                string dataString = ResourcesManager.ConvertToString(dataStream);
                return ConfigTools.GetSettingsEntries(dataString);
            }
        }


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
    }
}
