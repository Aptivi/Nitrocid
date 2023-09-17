
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Instances;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Splash.Splashes;

namespace KS.Misc.Splash
{
    /// <summary>
    /// Splash management module
    /// </summary>
    public static class SplashManager
    {

        internal static KernelThread SplashThread = new("Kernel Splash Thread", false, (splash) => GetSplashFromName((string)splash).EntryPoint.Display()) { isCritical = true };
        internal readonly static Dictionary<string, SplashInfo> InstalledSplashes = new()
        {
            { "Simple", new SplashInfo("Simple", new SplashSimple()) },
            { "Progress", new SplashInfo("Progress", new SplashProgress()) },
            { "Blank", new SplashInfo("Blank", new SplashBlank(), false) },
            { "Fader", new SplashInfo("Fader", new SplashFader()) },
            { "FaderBack", new SplashInfo("FaderBack", new SplashFaderBack()) },
            { "BeatFader", new SplashInfo("BeatFader", new SplashBeatFader()) },
            { "systemd", new SplashInfo("systemd", new SplashSystemd()) },
            { "sysvinit", new SplashInfo("sysvinit", new SplashSysvinit()) },
            { "openrc", new SplashInfo("openrc", new SplashOpenRC()) },
            { "Pulse", new SplashInfo("Pulse", new SplashPulse()) },
            { "BeatPulse", new SplashInfo("BeatPulse", new SplashBeatPulse()) },
            { "EdgePulse", new SplashInfo("EdgePulse", new SplashEdgePulse()) },
            { "BeatEdgePulse", new SplashInfo("BeatEdgePulse", new SplashBeatEdgePulse()) },
            { "PowerLine", new SplashInfo("PowerLine", new SplashPowerLine()) },
            { "PowerLineProgress", new SplashInfo("PowerLine", new SplashPowerLineProgress()) },
            { "Spin", new SplashInfo("Spin", new SplashSpin()) },
            { "Dots", new SplashInfo("Dots", new SplashDots()) },
            { "Welcome", new SplashInfo("Welcome", new SplashWelcome()) },
            { "SquareCorner", new SplashInfo("SquareCorner", new SplashSquareCorner()) },
            { "TextBox", new SplashInfo("TextBox", new SplashTextBox()) },
        };

        /// <summary>
        /// Current splash name
        /// </summary>
        public static string SplashName =>
            Config.MainConfig.SplashName;

        /// <summary>
        /// Current splash screen
        /// </summary>
        public static ISplash CurrentSplash =>
            GetSplashFromName(SplashName).EntryPoint;

        /// <summary>
        /// Current splash screen info instance
        /// </summary>
        public static SplashInfo CurrentSplashInfo =>
            GetSplashFromName(SplashName);

        /// <summary>
        /// All the installed splashes either normal or custom
        /// </summary>
        public static Dictionary<string, SplashInfo> Splashes =>
            InstalledSplashes;

        /// <summary>
        /// Gets names of the installed splashes
        /// </summary>
        public static List<string> GetNamesOfSplashes() =>
            Splashes.Keys.ToList();

        /// <summary>
        /// Loads all the splashes from the KSSplashes folder
        /// </summary>
        public static void LoadSplashes()
        {
            string SplashPath = Paths.GetKernelPath(KernelPathType.CustomSplashes);
            if (!Checking.FolderExists(SplashPath))
                Making.MakeDirectory(SplashPath);
            var SplashFiles = Listing.CreateList(SplashPath);
            foreach (FileSystemEntry SplashFileInfo in SplashFiles)
            {
                string FilePath = SplashFileInfo.FilePath;
                string FileName = SplashFileInfo.BaseEntry.Name;

                // Try to parse the splash file
                if (SplashFileInfo.BaseEntry.Extension == ".dll")
                {
                    // We got a .dll file that may or may not contain splash file. Parse that to verify.
                    try
                    {
                        // Add splash dependencies folder (if any) to the private appdomain lookup folder
                        string SplashDepPath = SplashPath + "Deps/" + Path.GetFileNameWithoutExtension(FileName) + "-" + FileVersionInfo.GetVersionInfo(FilePath).FileVersion + "/";
                        AssemblyLookup.AddPathToAssemblySearchPath(SplashDepPath);

                        // Now, actually parse that.
                        DebugWriter.WriteDebug(DebugLevel.I, "Parsing splash file {0}...", FilePath);
                        var SplashAssembly = Assembly.LoadFrom(FilePath);
                        var SplashInstance = GetSplashInstance(SplashAssembly);
                        if (SplashInstance is not null)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Found valid splash! Getting information...");
                            string Name = SplashInstance.SplashName;
                            bool DisplaysProgress = SplashInstance.SplashDisplaysProgress;

                            // Install the values to the new instance
                            DebugWriter.WriteDebug(DebugLevel.I, "- Name: {0}", Name);
                            DebugWriter.WriteDebug(DebugLevel.I, "- Displays Progress: {0}", DisplaysProgress);
                            DebugWriter.WriteDebug(DebugLevel.I, "Installing splash...");
                            var InstalledSplash = new SplashInfo(Name, SplashInstance, DisplaysProgress);
                            InstalledSplashes.Remove(Name);
                            InstalledSplashes.Add(Name, InstalledSplash);
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Skipping incompatible splash file {0}...", FilePath);
                        }
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Could not handle splash file {0}! {1}", FilePath, ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                        foreach (Exception LoaderException in ex.LoaderExceptions)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Loader exception: {0}", LoaderException.Message);
                            DebugWriter.WriteDebugStackTrace(LoaderException);
                        }
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Skipping incompatible splash file {0} because file extension is not .dll ({1})...", FilePath, SplashFileInfo.BaseEntry.Extension);
                }
            }
        }

        /// <summary>
        /// Unloads all the splashes from the KSSplashes folder
        /// </summary>
        public static void UnloadSplashes()
        {
            var SplashFiles = Listing.CreateList(Paths.GetKernelPath(KernelPathType.CustomSplashes));
            foreach (FileSystemEntry SplashFileInfo in SplashFiles)
            {
                string FilePath = SplashFileInfo.FilePath;

                // Try to parse the splash file
                try
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Parsing splash file {0}...", FilePath);
                    var SplashAssembly = Assembly.LoadFrom(FilePath);
                    var SplashInstance = GetSplashInstance(SplashAssembly);
                    if (SplashInstance is not null)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Found valid splash! Getting information...");
                        string Name = SplashInstance.SplashName;

                        // Uninstall the splash
                        DebugWriter.WriteDebug(DebugLevel.I, "- Name: {0}", Name);
                        DebugWriter.WriteDebug(DebugLevel.I, "Uninstalling splash...");
                        InstalledSplashes.Remove(Name);
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Skipping incompatible splash file {0}...", FilePath);
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Could not handle splash file {0}! {1}", FilePath, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
        }

        /// <summary>
        /// Gets the splash instance from compiled assembly
        /// </summary>
        /// <param name="Assembly">An assembly</param>
        public static ISplash GetSplashInstance(Assembly Assembly)
        {
            foreach (Type t in Assembly.GetTypes())
            {
                if (t.GetInterface(typeof(ISplash).Name) is not null)
                    return (ISplash)Assembly.CreateInstance(t.FullName);
            }
            return null;
        }

        /// <summary>
        /// Gets the splash information from the name
        /// </summary>
        /// <param name="splashName">Splash name</param>
        /// <returns>Splash information</returns>
        public static SplashInfo GetSplashFromName(string splashName)
        {
            if (Splashes.ContainsKey(splashName))
            {
                return Splashes[splashName];
            }
            else
            {
                return Splashes["Simple"];
            }
        }

        /// <summary>
        /// Opens the splash screen
        /// </summary>
        public static void OpenSplash() => OpenSplash(CurrentSplash);

        /// <summary>
        /// Opens the splash screen
        /// </summary>
        /// <param name="splashName">Splash name</param>
        public static void OpenSplash(string splashName) => OpenSplash(GetSplashFromName(splashName).EntryPoint);

        /// <summary>
        /// Opens the splash screen
        /// </summary>
        /// <param name="splash">Splash interface to use</param>
        public static void OpenSplash(ISplash splash)
        {
            if (Flags.EnableSplash)
            {
                SplashReport._Progress = 0;
                ConsoleBase.ConsoleWrapper.CursorVisible = false;
                splash.Opening();
                SplashThread.Stop();
                SplashThread.Start(splash.SplashName);
                SplashReport._InSplash = true;
            }
        }

        /// <summary>
        /// Closes the splash screen
        /// </summary>
        public static void CloseSplash() => CloseSplash(CurrentSplash);

        /// <summary>
        /// Closes the splash screen
        /// </summary>
        /// <param name="splashName">Splash name</param>
        public static void CloseSplash(string splashName) => CloseSplash(GetSplashFromName(splashName).EntryPoint);

        /// <summary>
        /// Closes the splash screen
        /// </summary>
        /// <param name="splash">Splash interface to use</param>
        public static void CloseSplash(ISplash splash) => CloseSplash(splash, true);

        /// <summary>
        /// Closes the splash screen
        /// </summary>
        /// <param name="splash">Splash interface to use</param>
        /// <param name="showClosing">Shows the closing animation, or clears the screen</param>
        internal static void CloseSplash(ISplash splash, bool showClosing)
        {
            if (Flags.EnableSplash)
            {
                splash.SplashClosing = true;

                // We need to wait for the splash display thread to finish its work once SplashClosing is set, because some splashes, like PowerLine,
                // actually do some operations that take a few milliseconds to finish what it's doing, and if we didn't wait here until the operations
                // are done in the Display() function, we'd abruptly stop without waiting, causing race condition. If this happened, visual glitches
                // manifest, which is not good.
                SplashThread.Wait();
                SplashThread.Stop();
                if (showClosing)
                    splash.Closing();
                else
                    InstalledSplashes["Blank"].EntryPoint.Closing();
                ConsoleBase.ConsoleWrapper.CursorVisible = true;

                // Reset the SplashClosing variable in case it needs to be open again. Some splashes don't do anything if they detect that the splash
                // screen is closing.
                splash.SplashClosing = false;
                SplashReport._InSplash = false;
            }
        }

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        public static void PreviewSplash() =>
            PreviewSplash(CurrentSplash, false);

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="splashOut">Whether to test out the important messages on splash.</param>
        public static void PreviewSplash(bool splashOut) =>
            PreviewSplash(CurrentSplash, splashOut);

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="splashName">Splash name</param>
        public static void PreviewSplash(string splashName) =>
            PreviewSplash(GetSplashFromName(splashName).EntryPoint, false);

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="splashName">Splash name</param>
        /// <param name="splashOut">Whether to test out the important messages on splash.</param>
        public static void PreviewSplash(string splashName, bool splashOut) =>
            PreviewSplash(GetSplashFromName(splashName).EntryPoint, splashOut);

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="splash">Splash name</param>
        public static void PreviewSplash(ISplash splash) =>
            PreviewSplash(splash, false);

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="splash">Splash name</param>
        /// <param name="splashOut">Whether to test out the important messages on splash.</param>
        public static void PreviewSplash(ISplash splash, bool splashOut)
        {
            // Open the splash and reset the report progress to 0%
            OpenSplash(splash);

            // Report progress 5 times
            for (int i = 1; i <= 5; i++)
            {
                int prog = i * 20;
                SplashReport.ReportProgress($"{prog}%", 20, true, splash);
                Thread.Sleep(1000);
                if (splashOut)
                {
                    BeginSplashOut();
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("We've reached {0}%!"), vars: prog);
                    EndSplashOut();
                }
            }

            // Close
            CloseSplash(splash);
        }

        /// <summary>
        /// Clears the screen for important messages to show up during kernel booting
        /// </summary>
        public static void BeginSplashOut()
        {
            if (Flags.EnableSplash && SplashReport._InSplash)
                CloseSplash(CurrentSplash, false);
        }

        /// <summary>
        /// Declares that it's done showing important messages during kernel booting
        /// </summary>
        public static void EndSplashOut()
        {
            if (Flags.EnableSplash && !SplashReport._InSplash)
                OpenSplash();
        }

    }
}
