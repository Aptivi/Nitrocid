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
using System.Collections.Generic;
using System.Net;
using System.Text;
using Aptivestigate.CrashHandler;
using Aptivestigate.Logging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Network.Types.RPC;
using Nitrocid.Core.Languages;
using Nitrocid.Rkm.Arguments;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Arguments.Base;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Rkm
{
    internal class EntryPoint
    {
        internal static BaseLogger? abstractLogger;
        internal static bool verbose = false;
        internal static string hostName = "127.0.0.1";
        internal static int hostPort = Config.MainConfig.RPCPort;
        internal static string command = "test";
        internal static string commandArguments = "";
        internal static readonly Dictionary<string, ArgumentInfo> arguments = new()
        {
            { "verbose", new("verbose", /* Localizable */ "NKS_RKM_ARGUMENTS_VERBOSE_DESC", new VerboseArgument()) },
            { "help", new("help", /* Localizable */ "NKS_RKM_ARGUMENTS_HELP_DESC",
                [
                    new([
                            new CommandArgumentPart(true, "argument", new()
                            {
                                ArgumentDescription = /* Localizable */ "NKS_RKM_ARGUMENTS_HELP_ARGUMENT_DESC"
                            })
                        ])
                ], new HelpArgument()) },
            { "host", new("host", /* Localizable */ "NKS_RKM_ARGUMENTS_HOST_DESC",
                [
                    new([
                            new CommandArgumentPart(true, "host", new()
                            {
                                ArgumentDescription = /* Localizable */ "NKS_RKM_ARGUMENTS_HOST_HOST_DESC"
                            })
                        ])
                ], new HostArgument()) },
            { "port", new("port", /* Localizable */ "NKS_RKM_ARGUMENTS_PORT_DESC",
                [
                    new([
                            new CommandArgumentPart(true, "port", new()
                            {
                                ArgumentDescription = /* Localizable */ "NKS_RKM_ARGUMENTS_PORT_PORT_DESC",
                                IsNumeric = true
                            })
                        ])
                ], new PortArgument()) },
            { "command", new("command", /* Localizable */ "NKS_RKM_ARGUMENTS_COMMAND_DESC",
                [
                    new([
                            new CommandArgumentPart(true, "command", new()
                            {
                                ArgumentDescription = /* Localizable */ "NKS_RKM_ARGUMENTS_COMMAND_COMMAND_DESC"
                            }),
                            new CommandArgumentPart(false, "arguments", new()
                            {
                                ArgumentDescription = /* Localizable */ "NKS_RKM_ARGUMENTS_COMMAND_ARGUMENTS_DESC"
                            }),
                        ], false, true)
                ], new CommandArgument()) },
        };

        static int Main(string[] args)
        {
            LanguageTools.AddCustomAction("Rkm", new("Nitrocid.Rkm.Resources.Languages.Output.Localizations", typeof(EntryPoint).Assembly));

            // Initialize logging and crash logging
            ArgumentParse.ParseArguments(args, arguments);
            CrashTools.InstallCrashHandler();

            // If verbose is enabled, write to the log
            if (abstractLogger is not null)
                LogTools.Debug(abstractLogger, "Passed to RKM program with: {0}, {1}, {2}, {3} {4}", verbose, hostName, hostPort, command, commandArguments);

            // Main code
            try
            {
                // Try to connect to the host
                if (abstractLogger is not null)
                    LogTools.Info(abstractLogger, "Connecting to Nitrocid RPC at {0}:{1}...", hostName, hostPort);
                if (verbose)
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_RKM_CONNECTING"), ThemeColorType.Progress);
                RPCCommands.SendCommand($"<Request:Acknowledge>({hostName})", hostName, hostPort, true);

                // Ensure that we got a reply
                var endPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] replyBytes = RemoteProcedure.rpcStandaloneClient.Receive(ref endPoint);
                string replyData = Encoding.UTF8.GetString(replyBytes);
                if (abstractLogger is not null)
                    LogTools.Debug(abstractLogger, "replyData = [{0} bytes] {1}", replyData.Length, replyData);
                if (verbose)
                    TextWriterColor.Write("replyData: " + replyData);
                if (replyData != "N-KS-ACK")
                {
                    // Not a Nitrocid instance
                    if (abstractLogger is not null)
                        LogTools.Error(abstractLogger, "replyData is not Nitrocid ack.");
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_RKM_INVALIDINSTANCE"), ThemeColorType.Error);
                    return -1;
                }

                // Now, we have a Nitrocid instance, so process commands
                bool processReply = false;
                switch (command)
                {
                    case "shutdown":
                        RPCCommands.SendCommand($"<Request:Shutdown>({hostName})", hostName, hostPort, true);
                        break;
                    case "reboot":
                        RPCCommands.SendCommand($"<Request:Reboot>({hostName})", hostName, hostPort, true);
                        break;
                    case "rebootsafe":
                        RPCCommands.SendCommand($"<Request:RebootSafe>({hostName})", hostName, hostPort, true);
                        break;
                    case "rebootmaintenance":
                        RPCCommands.SendCommand($"<Request:RebootMaintenance>({hostName})", hostName, hostPort, true);
                        break;
                    case "rebootdebug":
                        RPCCommands.SendCommand($"<Request:RebootDebug>({hostName})", hostName, hostPort, true);
                        break;
                    case "savescr":
                        RPCCommands.SendCommand($"<Request:SaveScr>({hostName})", hostName, hostPort, true);
                        break;
                    case "exec":
                        RPCCommands.SendCommand($"<Request:Exec>({arguments})", hostName, hostPort, true);
                        break;
                    case "test":
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_RKM_CONNECTIONSUCCESS"), ThemeColorType.Success);
                        break;
                    case "ping":
                        RPCCommands.SendCommand($"<Request:Ping>({hostName})", hostName, hostPort, true);
                        break;
                    case "version":
                        RPCCommands.SendCommand($"<Request:Version>({hostName})", hostName, hostPort, true);
                        processReply = true;
                        break;
                    case "versioncode":
                        RPCCommands.SendCommand($"<Request:VersionCode>({hostName})", hostName, hostPort, true);
                        processReply = true;
                        break;
                    case "apiversion":
                        RPCCommands.SendCommand($"<Request:ApiVersion>({hostName})", hostName, hostPort, true);
                        processReply = true;
                        break;
                    case "apiversioncode":
                        RPCCommands.SendCommand($"<Request:ApiVersionCode>({hostName})", hostName, hostPort, true);
                        processReply = true;
                        break;
                }

                // Determine whether we need to process reply
                if (abstractLogger is not null)
                    LogTools.Debug(abstractLogger, "Needs reply: {0}", processReply);
                if (processReply)
                {
                    // Write the reply
                    replyBytes = RemoteProcedure.rpcStandaloneClient.Receive(ref endPoint);
                    replyData = Encoding.UTF8.GetString(replyBytes);
                    if (abstractLogger is not null)
                        LogTools.Debug(abstractLogger, "{0} replyData = [{1} bytes] {2}", command, replyData.Length, replyData);
                    TextWriterColor.Write(replyData);
                }
                if (abstractLogger is not null)
                    LogTools.Info(abstractLogger, "Connection finished: {0}:{1}", hostName, hostPort);
            }
            catch (Exception ex)
            {
                // Connection failure or a bug
                if (abstractLogger is not null)
                    LogTools.Fatal(abstractLogger, ex, "Command {0} failed at {1}:{2} with error: {3}", command, hostName, hostPort, ex.Message);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_RKM_FAILED"), ThemeColorType.Error);
                if (verbose)
                    TextWriterColor.Write(ex.Message, ThemeColorType.Error);
                return ex is KernelException kex ? (int)kex.ExceptionType : ex.HResult;
            }
            return 0;
        }
    }
}
