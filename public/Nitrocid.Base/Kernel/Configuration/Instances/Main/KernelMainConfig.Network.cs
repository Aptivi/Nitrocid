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

using Nitrocid.Base.Kernel.Debugging.RemoteDebug;
using Nitrocid.Base.Kernel.Debugging.RemoteDebug.RemoteChat;
using Nitrocid.Base.Network;
using Nitrocid.Base.Network.Types.RPC;

namespace Nitrocid.Base.Kernel.Configuration.Instances
{
    /// <summary>
    /// Main kernel configuration instance
    /// </summary>
    public partial class KernelMainConfig : BaseKernelConfig
    {
        /// <summary>
        /// Write a remote debugger port. It must be numeric, and must not be already used. Otherwise, remote debugger will fail to open the port
        /// </summary>
        public int DebugPort
        {
            get => RemoteDebugger.debugPort;
            set => RemoteDebugger.debugPort = value < 0 ? 3014 : value;
        }
        /// <summary>
        /// Write a remote debugger chat port. It must be numeric, and must not be already used. Otherwise, remote debugger chat will fail to open the port
        /// </summary>
        public int DebugChatPort
        {
            get => RemoteChatTools.debugChatPort;
            set => RemoteChatTools.debugChatPort = value < 0 ? 3015 : value;
        }
        /// <summary>
        /// Write how many times the "get" command should retry failed downloads. It must be numeric.
        /// </summary>
        public int DownloadRetries
        {
            get => NetworkTools.downloadRetries;
            set => NetworkTools.downloadRetries = value < 0 ? 3 : value;
        }
        /// <summary>
        /// Write how many times the "put" command should retry failed uploads. It must be numeric.
        /// </summary>
        public int UploadRetries
        {
            get => NetworkTools.uploadRetries;
            set => NetworkTools.uploadRetries = value < 0 ? 3 : value;
        }
        /// <summary>
        /// If true, it makes "get" or "put" show the progress bar while downloading or uploading.
        /// </summary>
        public bool ShowProgress { get; set; } = true;
        /// <summary>
        /// Records remote debug chat to debug log
        /// </summary>
        public bool RecordChatToDebugLog { get; set; } = true;
        /// <summary>
        /// Shows the SSH server banner on connection
        /// </summary>
        public bool SSHBanner { get; set; }
        /// <summary>
        /// Whether or not to enable RPC
        /// </summary>
        public bool RPCEnabled { get; set; }
        /// <summary>
        /// Write an RPC port. It must be numeric, and must not be already used. Otherwise, RPC will fail to open the port.
        /// </summary>
        public int RPCPort
        {
            get => RemoteProcedure.rpcPort;
            set => RemoteProcedure.rpcPort = value < 0 ? 12345 : value;
        }
        /// <summary>
        /// If you want remote debug to start on boot, enable this
        /// </summary>
        public bool RDebugAutoStart { get; set; } = true;
        /// <summary>
        /// Specifies the remote debug message format. {0} for name, {1} for message
        /// </summary>
        public string RDebugMessageFormat { get; set; } = "";
        /// <summary>
        /// How many milliseconds to wait before declaring timeout?
        /// </summary>
        public int PingTimeout
        {
            get => NetworkTools.pingTimeout;
            set => NetworkTools.pingTimeout = value < 0 ? 60000 : value;
        }
        /// <summary>
        /// Write how you want your download percentage text to be. Leave blank to use default style. Placeholders are parsed. {0} for downloaded size, {1} for target size, {2} for percentage.
        /// </summary>
        public string DownloadPercentagePrint { get; set; } = "";
        /// <summary>
        /// Write how you want your upload percentage text to be. Leave blank to use default style. Placeholders are parsed. {0} for uploaded size, {1} for target size, {2} for percentage.
        /// </summary>
        public string UploadPercentagePrint { get; set; } = "";
        /// <summary>
        /// Shows the notification showing the download progress
        /// </summary>
        public bool DownloadNotificationProvoke { get; set; }
        /// <summary>
        /// Shows the notification showing the upload progress
        /// </summary>
        public bool UploadNotificationProvoke { get; set; }
        /// <summary>
        /// If enabled, will use the notification system to notify the host of remote debug connection error. Otherwise, will use the default console writing.
        /// </summary>
        public bool NotifyOnRemoteDebugConnectionError { get; set; } = true;
        /// <summary>
        /// If enabled, will ignore certificate errors when making a connection. Otherwise, verifies the server certificate. This should, normally, be not enabled unless you're sure that you're trusting the server.
        /// </summary>
        public bool IgnoreCertificateErrors { get; set; }
    }
}
