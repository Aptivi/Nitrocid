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
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Aptivestigate.Logging;
using Aptivestigate.Serilog;
using Nitrocid.Drivers;
using Nitrocid.Files;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging.RemoteDebug;
using Nitrocid.Kernel.Debugging.RemoteDebug.RemoteChat;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Renderers;
using Serilog;
using Textify.General;

namespace Nitrocid.Kernel.Debugging
{
    /// <summary>
    /// Debug writing module
    /// </summary>
    public static class DebugWriter
    {
        internal static BaseLogger? debugLogger;
        internal static object WriteLock = new();
        internal readonly static List<string> debugStackTraces = [];

        /// <summary>
        /// Debug stack trace list
        /// </summary>
        public static string[] DebugStackTraces =>
            [.. debugStackTraces];

        /// <summary>
        /// Outputs the text into the debugger file, and sets the time stamp. Censors all secure arguments if <see cref="Config.MainConfig"/>.DebugCensorPrivateInfo is on.
        /// </summary>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger file. Supports {0}, {1}, ...</param>
        /// <param name="SecureVarIndexes">Secure variable indexes to modify <paramref name="vars"/> to censor them when <see cref="Config.MainConfig"/>.DebugCensorPrivateInfo is on</param>
        /// <param name="memberName">Member name. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberLine">Member line number. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberPath">Member path. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebugPrivacy(DebugLevel Level, string text, int[] SecureVarIndexes, [CallerMemberName] string memberName = "", [CallerLineNumber] int memberLine = 0, [CallerFilePath] string memberPath = "", object?[]? vars = null)
        {
            // First, iterate through all the provided secure indexes to convert these to censored strings
            foreach (int SecureVarIndex in SecureVarIndexes)
            {
                // Check the index value
                if (vars is null)
                    continue;
                if (SecureVarIndex < 0)
                    continue;
                if (SecureVarIndex >= vars.Length)
                    continue;

                // Censor all the secure vars found
                if (Config.MainConfig.DebugCensorPrivateInfo)
                    vars[SecureVarIndex] = "[removed for privacy]";
            }

            // Then, go ahead and write the message
            WriteDebug(Level, text, memberName, memberLine, memberPath, vars);
        }

        /// <summary>
        /// Outputs the text into the debugger file, and sets the time stamp.
        /// </summary>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger file. Supports {0}, {1}, ...</param>
        /// <param name="memberName">Member name. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberLine">Member line number. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberPath">Member path. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebug(DebugLevel Level, string text, [CallerMemberName] string memberName = "", [CallerLineNumber] int memberLine = 0, [CallerFilePath] string memberPath = "", object?[]? vars = null)
        {
            lock (WriteLock)
            {
                if (KernelEntry.DebugMode)
                {
                    WriteDebugLogOnly(Level, text, memberName, memberLine, memberPath, vars);
                    WriteDebugDevicesOnly(Level, text, false, memberName, memberLine, memberPath, vars);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the debugger file, and sets the time stamp.
        /// </summary>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger file. Supports {0}, {1}, ...</param>
        /// <param name="memberName">Member name. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberLine">Member line number. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberPath">Member path. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebugLogOnly(DebugLevel Level, string text, [CallerMemberName] string memberName = "", [CallerLineNumber] int memberLine = 0, [CallerFilePath] string memberPath = "", object?[]? vars = null)
        {
            lock (WriteLock)
            {
                if (KernelEntry.DebugMode)
                {
                    // Try to debug...
                    try
                    {
                        // Populate the debug stack frame
                        string fileName = Path.GetFileName(memberPath);
                        StringBuilder message = new();

                        // Handle new lines and write each line to the debugger
                        string result =
                            vars is not null && vars.Length > 0 ?
                            text.ToString().FormatString(vars) :
                            text.ToString();
                        string[] split = result.SplitNewLines();
                        foreach (string splitText in split)
                        {
                            // Check to see if source file name is not empty.
                            message.Clear();
                            if (fileName is not null && memberLine != 0)
                                message.Append($"({memberName} - {fileName}:{memberLine}): ");
                            if (Level == DebugLevel.D || Level == DebugLevel.T)
                                message.Append($"[{Level}] ");
                            message.Append($"{splitText}");

                            // Write the result
                            DriverHandler.CurrentDebugLoggerDriverLocal.Write(message.ToString(), Level);
#if VSDEBUG
                            Debug.Write($"[{Level}] {result}");
#endif
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteDebug(DebugLevel.F, "Debugger error: {0}", vars: [ex.Message]);
                        WriteDebugStackTrace(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Conditionally outputs the text into the debugger file, and sets the time stamp.
        /// </summary>
        /// <param name="Condition">The condition that must be satisfied</param>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger file. Supports {0}, {1}, ...</param>
        /// <param name="memberName">Member name. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberLine">Member line number. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberPath">Member path. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebugConditional(bool Condition, DebugLevel Level, string text, [CallerMemberName] string memberName = "", [CallerLineNumber] int memberLine = 0, [CallerFilePath] string memberPath = "", object?[]? vars = null)
        {
            lock (WriteLock)
            {
                if (Condition)
                    WriteDebug(Level, text, memberName, memberLine, memberPath, vars);
            }
        }

        /// <summary>
        /// Outputs the text into the debugger devices, and sets the time stamp. Note that it doesn't print where did the debugger debug in source files.
        /// </summary>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger devices. Supports {0}, {1}, ...</param>
        /// <param name="force">Force message to appear, regardless of mute settings</param>
        /// <param name="memberName">Member name. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberLine">Member line number. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberPath">Member path. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebugDevicesOnly(DebugLevel Level, string text, bool force, [CallerMemberName] string memberName = "", [CallerLineNumber] int memberLine = 0, [CallerFilePath] string memberPath = "", object?[]? vars = null)
        {
            lock (WriteLock)
            {
                if (KernelEntry.DebugMode)
                {
                    for (int i = 0; i <= RemoteDebugger.DebugDevices.Count - 1; i++)
                    {
                        var device = RemoteDebugger.DebugDevices[i];
                        if (!WriteDebugDeviceOnly(Level, text, force, device, memberName, memberLine, memberPath, vars) && i > 0)
                            i--;
                    }
                }
            }
        }

        /// <summary>
        /// Outputs the text into a debugger device, and sets the time stamp. Note that it doesn't print where did the debugger debug in source files.
        /// </summary>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger devices. Supports {0}, {1}, ...</param>
        /// <param name="force">Force message to appear, regardless of mute settings</param>
        /// <param name="device">Device to contact</param>
        /// <param name="memberName">Member name. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberLine">Member line number. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberPath">Member path. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>True if successfully sent. False otherwise. Also true if the kernel runs on non-debug mode.</returns>
        public static bool WriteDebugDeviceOnly(DebugLevel Level, string text, bool force, RemoteDebugDevice device, [CallerMemberName] string memberName = "", [CallerLineNumber] int memberLine = 0, [CallerFilePath] string memberPath = "", object?[]? vars = null)
        {
            lock (WriteLock)
            {
                if (KernelEntry.DebugMode)
                {
                    try
                    {
                        // Remove the \r line endings from the text, since the debug file needs to have its line endings in the
                        // UNIX format anyways.
                        text = text.Replace(char.ToString((char)13), "");

                        // Handle the new lines
                        string[] texts = text.Split("\n");
                        string fileName = Path.GetFileName(memberPath);
                        foreach (string textStr in texts)
                        {
                            if (force || !force && !device.DeviceInfo.MuteLogs)
                            {
                                if (vars is null)
                                    device.ClientStreamWriter.Write($"{TimeDateTools.KernelDateTime.ToShortDateString()} {TimeDateTools.KernelDateTime.ToShortTimeString()} [{Level}] ({memberName} - {fileName}:{memberLine}): {textStr}\r\n");
                                else
                                    device.ClientStreamWriter.Write($"{TimeDateTools.KernelDateTime.ToShortDateString()} {TimeDateTools.KernelDateTime.ToShortTimeString()} [{Level}] ({memberName} - {fileName}:{memberLine}):  {textStr}\r\n", vars);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        RemoteDebugTools.DisconnectDependingOnException(ex, device);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Outputs the text into the debugger devices for chat, and sets the time stamp. Note that it doesn't print where did the debugger debug in source files.
        /// </summary>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger devices. Supports {0}, {1}, ...</param>
        /// <param name="force">Force message to appear, regardless of mute settings</param>
        /// <param name="memberName">Member name. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberLine">Member line number. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="memberPath">Member path. Do not set unless you know what you're doing. Usually, using <c>vars: [...]</c> directly before the <paramref name="memberName"/> parameter is enough.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebugChatsOnly(DebugLevel Level, string text, bool force, [CallerMemberName] string memberName = "", [CallerLineNumber] int memberLine = 0, [CallerFilePath] string memberPath = "", object?[]? vars = null)
        {
            lock (WriteLock)
            {
                if (KernelEntry.DebugMode)
                {
                    for (int i = 0; i <= RemoteChatTools.DebugChatDevices.Count - 1; i++)
                    {
                        var device = RemoteChatTools.DebugChatDevices[i];
                        if (!WriteDebugDeviceOnly(Level, text, force, device, memberName, memberLine, memberPath, vars) && i > 0)
                            i--;
                    }
                }
            }
        }

        /// <summary>
        /// Conditionally writes the exception's stack trace to the debugger
        /// </summary>
        /// <param name="Condition">The condition that must be satisfied</param>
        /// <param name="Ex">An exception</param>
        public static void WriteDebugStackTraceConditional(bool Condition, Exception Ex)
        {
            lock (WriteLock)
            {
                if (Condition)
                    WriteDebugStackTrace(Ex);
            }
        }

        /// <summary>
        /// Writes the exception's stack trace to the debugger
        /// </summary>
        /// <param name="Ex">An exception</param>
        public static void WriteDebugStackTrace(Exception? Ex)
        {
            lock (WriteLock)
            {
                if (KernelEntry.DebugMode)
                {
                    if (Ex is null)
                    {
                        WriteDebug(DebugLevel.T, "No stack trace!");
                        WriteDebug(DebugLevel.T, $"Event of incident: {TimeDateRenderers.Render()}");
                        return;
                    }

                    // These two NewLines are padding for accurate stack tracing.
                    var Inner = Ex.InnerException;
                    int InnerNumber = 1;
                    var NewStackTraces = new List<string>()
                    {
                        $"{CharManager.NewLine}{GetExceptionTraceString(Ex)}"
                    };

                    // Get all the inner exceptions
                    while (Inner is not null)
                    {
                        NewStackTraces.Add($"[{InnerNumber}] {GetExceptionTraceString(Inner)}");
                        InnerNumber += 1;
                        Inner = Inner.InnerException;
                    }

                    // Print stack trace to debugger
                    var StkTrcs = new List<string>();
                    for (int i = 0; i <= NewStackTraces.Count - 1; i++)
                        StkTrcs.AddRange(NewStackTraces[i].SplitNewLines());
                    for (int i = 0; i <= StkTrcs.Count - 1; i++)
                        WriteDebug(DebugLevel.T, StkTrcs[i]);
                    WriteDebug(DebugLevel.T, $"Event of incident: {TimeDateRenderers.Render()}");
                    debugStackTraces.AddRange(NewStackTraces);
                }
            }
        }

        internal static string GetExceptionTraceString(Exception ex) =>
            $"{ex.GetType().FullName}: " +
            $"{(ex is KernelException kex ? kex.OriginalExceptionMessage : ex.Message)}{CharManager.NewLine}" +
            $"{ex.StackTrace}{CharManager.NewLine}";

        internal static void InitializeDebug() =>
            InitializeDebug(LogTools.GenerateLogFilePath(out _));

        internal static void InitializeDebug(string loggerPath)
        {
            // Initialize debug logger
            debugLogger = new SerilogLogger(
                new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(loggerPath, rollOnFileSizeLimit: true));
        }

        internal static void DeterministicDebug(string text, DebugLevel level, object?[]? vars = null)
        {
            switch (level)
            {
                case DebugLevel.T:
                case DebugLevel.D:
                    if (vars is null)
                        debugLogger?.Debug(text);
                    else
                        debugLogger?.Debug(text, vars);
                    break;
                case DebugLevel.I:
                    if (vars is null)
                        debugLogger?.Info(text);
                    else
                        debugLogger?.Info(text, vars);
                    break;
                case DebugLevel.W:
                    if (vars is null)
                        debugLogger?.Warning(text);
                    else
                        debugLogger?.Warning(text, vars);
                    break;
                case DebugLevel.E:
                    if (vars is null)
                        debugLogger?.Error(text);
                    else
                        debugLogger?.Error(text, vars);
                    break;
                case DebugLevel.F:
                    if (vars is null)
                        debugLogger?.Fatal(text);
                    else
                        debugLogger?.Fatal(text, vars);
                    break;
            }
        }

        internal static void RemoveDebugLogs()
        {
            var files = FilesystemTools.GetFilesystemEntries(FilesystemTools.NeutralizePath(PathsManagement.AppDataPath + "/../Aptivi/Logs/") + "log_Nitrocid_*.txt");
            foreach (var file in files)
            {
                if (FilesystemTools.FileExists(file))
                    FilesystemTools.RemoveFile(file);
            }
        }
    }
}
