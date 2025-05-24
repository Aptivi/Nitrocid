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

using FluentFTP;
using Newtonsoft.Json;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection.Internal;
using Nitrocid.Shell.Prompts;
using Nitrocid.ShellPacks.Shells.FTP;

namespace Nitrocid.ShellPacks.Settings
{
    /// <summary>
    /// Configuration instance for FTP
    /// </summary>
    public class FtpConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries
        {
            get
            {
                var dataStream = ResourcesManager.GetData("FtpSettings.json", ResourcesType.Misc, typeof(FtpConfig).Assembly) ??
                    throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Failed to obtain settings entries."));
                string dataString = ResourcesManager.ConvertToString(dataStream);
                return ConfigTools.GetSettingsEntries(dataString);
            }
        }

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
    }
}
