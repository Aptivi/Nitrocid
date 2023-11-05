//
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
using System.Text;
using KS.Drivers;
using KS.Files;
using KS.Files.Operations.Querying;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Kernel.Debugging.Trace;
using KS.Kernel.Exceptions;
using KS.Kernel.Time;
using KS.Kernel.Time.Renderers;
using KS.Misc.Text;

namespace KS.Kernel.Debugging
{
    /// <summary>
    /// Debug writing module
    /// </summary>
    public static class DebugWriter
    {

        internal static string DebugPath = "";
        internal static string lastRoutinePath = "";
        internal static StreamWriter DebugStreamWriter;
        internal static bool isDisposed;
        internal static object WriteLock = new();
        internal readonly static List<string> debugStackTraces = new();
        internal readonly static List<string> debugLines = new();

        /// <summary>
        /// Debug stack trace list
        /// </summary>
        public static string[] DebugStackTraces =>
            debugStackTraces.ToArray();

        /// <summary>
        /// Censor private information that may be printed to the debug logs.
        /// </summary>
        public static bool DebugCensorPrivateInfo =>
            Config.MainConfig.DebugCensorPrivateInfo;

        /// <summary>
        /// Enables event debugging
        /// </summary>
        public static bool EventDebug =>
            Config.MainConfig.EventDebug;

        /// <summary>
        /// Enables debug quota checks.
        /// </summary>
        public static bool DebugQuotaCheck =>
            Config.MainConfig.DebugQuotaCheck;

        /// <summary>
        /// How many lines to print to the debug buffer before reaching the quota limit?
        /// </summary>
        public static int DebugQuotaLines =>
            Config.MainConfig.DebugQuotaLines;

        /// <summary>
        /// Outputs the text into the debugger file, and sets the time stamp. Censors all secure arguments if <see cref="DebugCensorPrivateInfo"/> is on.
        /// </summary>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger file. Supports {0}, {1}, ...</param>
        /// <param name="SecureVarIndexes">Secure variable indexes to modify <paramref name="vars"/> to censor them when <see cref="DebugCensorPrivateInfo"/> is on</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebugPrivacy(DebugLevel Level, string text, int[] SecureVarIndexes, params object[] vars)
        {
            // First, iterate through all the provided secure indexes to convert these to censored strings
            foreach (int SecureVarIndex in SecureVarIndexes)
            {
                // Check the index value
                if (SecureVarIndex < 0)
                    continue;
                if (SecureVarIndex >= vars.Length)
                    continue;

                // Censor all the secure vars found
                if (DebugCensorPrivateInfo)
                    vars[SecureVarIndex] = "[removed for privacy]";
            }

            // Then, go ahead and write the message
            WriteDebug(Level, text, vars);
        }

        /// <summary>
        /// Outputs the text into the debugger file, and sets the time stamp.
        /// </summary>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger file. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebug(DebugLevel Level, string text, params object[] vars)
        {
            lock (WriteLock)
            {
                if (KernelEntry.DebugMode)
                {
                    WriteDebugLogOnly(Level, text, vars);
                    WriteDebugDevicesOnly(Level, text, false, vars);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the debugger file, and sets the time stamp.
        /// </summary>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger file. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebugLogOnly(DebugLevel Level, string text, params object[] vars)
        {
            lock (WriteLock)
            {
                if (KernelEntry.DebugMode)
                {
                    // Open debugging stream
                    string debugFilePath = DebugPath;
                    if (DebugStreamWriter is null || DebugStreamWriter?.BaseStream is null || isDisposed)
                    {
                        DebugStreamWriter = new StreamWriter(debugFilePath, true) { AutoFlush = true };
                        isDisposed = false;
                    }

                    // Try to debug...
                    try
                    {
                        // Check for quota
                        CheckDebugQuota();

                        // Populate the debug stack frame
                        var STrace = new DebugStackFrameBasic();
                        StringBuilder message = new();

                        // Descend a frame until we're out of this class
                        int unwound = 0;
                        while (STrace.RoutinePath.Contains(nameof(DebugWriter)))
                        {
                            STrace = new DebugStackFrameBasic(unwound);
                            unwound++;
                        }

                        // Remove the \r line endings from the text, since the debug file needs to have its line endings in the
                        // UNIX format anyways.
                        text = text.Replace(char.ToString((char)13), "");

                        // Handle the new lines
                        string[] texts = text.Split('\n');
                        foreach (string splitText in texts)
                        {
                            string routinePath = STrace.RoutinePath;
                            
                            // Check to see if source file name is not empty.
                            if (routinePath != lastRoutinePath)
                            {
                                message.Append('\n');
                                message.Append($"{TimeDateTools.KernelDateTime.ToShortDateString()} {TimeDateTools.KernelDateTime.ToShortTimeString()} ");
                                message.Append($"({routinePath})\n");
                                message.Append(new string('-', message.Length - 2));
                                message.Append($"\n\n");
                            }

                            // Show stack information
                            message.Append($"[{Level}] : {splitText}\n");
                            lastRoutinePath = routinePath;
                        }

                        // Debug to file and all connected debug devices (raw mode). The reason for the \r\n is that because
                        // Nitrocid on the Linux host tends to use \n only for new lines, and Windows considers \r\n as the
                        // new line. This causes the staircase effect on text written to the remote debugger, which messes up
                        // the output on Windows.
                        //
                        // However, we don't want to append the Windows new line character to the debug file, because we need
                        // it to have consistent line endings across platforms, like if you try to print the output of a text
                        // file that only has \n at the end of each line, we would inadvertently place the \r\n in each debug
                        // line, causing the file to have mixed line endings.
                        if (vars.Length > 0)
                            DriverHandler.CurrentDebugLoggerDriverLocal.Write(message.ToString(), vars);
                        else
                            DriverHandler.CurrentDebugLoggerDriverLocal.Write(message.ToString());

                        // If quota is enabled, add the line
                        if (DebugQuotaCheck)
                            debugLines.Add(message.ToString());
                    }
                    catch (Exception ex)
                    {
                        WriteDebug(DebugLevel.F, "Debugger error: {0}", ex.Message);
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
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebugConditional(bool Condition, DebugLevel Level, string text, params object[] vars)
        {
            lock (WriteLock)
            {
                if (Condition)
                    WriteDebug(Level, text, vars);
            }
        }

        /// <summary>
        /// Outputs the text into the debugger devices, and sets the time stamp. Note that it doesn't print where did the debugger debug in source files.
        /// </summary>
        /// <param name="Level">Debug level</param>
        /// <param name="text">A sentence that will be written to the the debugger devices. Supports {0}, {1}, ...</param>
        /// <param name="force">Force message to appear, regardless of mute settings</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebugDevicesOnly(DebugLevel Level, string text, bool force, params object[] vars)
        {
            lock (WriteLock)
            {
                if (KernelEntry.DebugMode)
                {
                    for (int i = 0; i <= RemoteDebugger.DebugDevices.Count - 1; i++)
                    {
                        var device = RemoteDebugger.DebugDevices[i];
                        if (!WriteDebugDeviceOnly(Level, text, force, device, vars) && i > 0)
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
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>True if successfully sent. False otherwise. Also true if the kernel runs on non-debug mode.</returns>
        public static bool WriteDebugDeviceOnly(DebugLevel Level, string text, bool force, RemoteDebugDevice device, params object[] vars)
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
                        foreach (string textStr in texts)
                        {
                            if (force || (!force && !device.DeviceInfo.MuteLogs))
                                device.ClientStreamWriter.Write($"{TimeDateTools.KernelDateTime.ToShortDateString()} {TimeDateTools.KernelDateTime.ToShortTimeString()} [{Level}] {textStr}\r\n", vars);
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
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteDebugChatsOnly(DebugLevel Level, string text, bool force, params object[] vars)
        {
            lock (WriteLock)
            {
                if (KernelEntry.DebugMode)
                {
                    for (int i = 0; i <= RemoteChat.DebugChatDevices.Count - 1; i++)
                    {
                        var device = RemoteChat.DebugChatDevices[i];
                        if (!WriteDebugDeviceOnly(Level, text, force, device, vars) && i > 0)
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
        public static void WriteDebugStackTrace(Exception Ex)
        {
            lock (WriteLock)
            {
                if (KernelEntry.DebugMode)
                {
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
            $"{ex.GetType().FullName}: {(ex is KernelException kex ? kex.OriginalExceptionMessage : ex.Message)}{CharManager.NewLine}{ex.StackTrace}{CharManager.NewLine}";

        internal static void CheckDebugQuota()
        {
            // Don't do anything if debug quota check is disabled.
            if (!DebugQuotaCheck)
                return;

            // Now, check how many lines we've written to the buffer
            if (debugLines.Count > DebugQuotaLines)
            {
                debugLines.Clear();
                InitializeDebugPath();
                isDisposed = true;
            }
        }

        internal static void InitializeDebugPath()
        {
            // Initialize debug path
            DebugPath = Getting.GetNumberedFileName(Path.GetDirectoryName(Paths.GetKernelPath(KernelPathType.Debugging)), Paths.GetKernelPath(KernelPathType.Debugging));
        }

    }
}
