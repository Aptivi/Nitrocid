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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using KS.ConsoleBase;
using KS.ConsoleBase.Buffered;
using KS.ConsoleBase.Inputs.Styles;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Instances;
using KS.Files.Operations;
using KS.Files.Operations.Querying;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Splash.Splashes;
using KS.Modifications;

namespace KS.Misc.Splash
{
    /// <summary>
    /// Splash management module
    /// </summary>
    public static class SplashManager
    {

        internal static Screen splashScreen = new();
        internal static SplashContext currentContext = SplashContext.StartingUp;
        internal static KernelThread SplashThread = new("Kernel Splash Thread", false, (splashParams) => SplashThreadHandler((SplashThreadParameters)splashParams));
        internal readonly static Dictionary<string, SplashInfo> InstalledSplashes = new()
        {
            // They are the base splashes. They shouldn't be moved to the splash addon pack as such movement breaks things.
            { "Welcome", new SplashInfo("Welcome", new SplashWelcome()) },
            { "Blank", new SplashInfo("Blank", new SplashBlank(), false) },
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
        /// Gets the current splash context
        /// </summary>
        public static SplashContext CurrentSplashContext =>
            currentContext;

        /// <summary>
        /// Enable the stylish splash screen in place of the regular verbose boot messages
        /// </summary>
        public static bool EnableSplash =>
            Config.MainConfig.EnableSplash;

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

                        // Check the public key
                        var modAsmName = new AssemblyName(SplashAssembly.FullName);
                        var modAsmPublicKey = modAsmName.GetPublicKeyToken();
                        if (modAsmPublicKey is null || modAsmPublicKey.Length == 0)
                        {
                            SplashReport.ReportProgressWarning(Translate.DoTranslation("The splash is not strongly signed. It may contain untrusted code."));
                            if (!ModManager.AllowUntrustedMods)
                                continue;
                        }

                        // Now, get the instance
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
            string SplashPath = Paths.GetKernelPath(KernelPathType.CustomSplashes);
            var SplashFiles = Listing.CreateList(SplashPath);
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

                        // Remove splash dependencies folder (if any) from the private appdomain lookup folder
                        string SplashDepPath = SplashPath + "Deps/" + Path.GetFileNameWithoutExtension(FilePath) + "-" + FileVersionInfo.GetVersionInfo(FilePath).FileVersion + "/";
                        AssemblyLookup.RemovePathFromAssemblySearchPath(SplashDepPath);
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
            if (Splashes.TryGetValue(splashName, out SplashInfo splashInfo))
                return splashInfo;
            else
                return Splashes["Welcome"];
        }

        /// <summary>
        /// Opens the splash screen
        /// </summary>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void OpenSplash(SplashContext context) =>
            OpenSplash(CurrentSplash, context);

        /// <summary>
        /// Opens the splash screen
        /// </summary>
        /// <param name="splashName">Splash name</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void OpenSplash(string splashName, SplashContext context) =>
            OpenSplash(GetSplashFromName(splashName).EntryPoint, context);

        /// <summary>
        /// Opens the splash screen
        /// </summary>
        /// <param name="splash">Splash interface to use</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void OpenSplash(ISplash splash, SplashContext context)
        {
            if (EnableSplash)
            {
                // Clean everything up
                var openingPart = new ScreenPart();
                splashScreen.RemoveBufferedParts();

                // Now, set the current context and prepare
                currentContext = context;
                SplashReport._Progress = 0;
                ConsoleWrapper.CursorVisible = false;

                // Add the opening function as dynamic text
                openingPart.AddDynamicText(() => splash.Opening(context));
                splashScreen.AddBufferedPart(openingPart);

                // Make it resize-aware
                ScreenTools.SetCurrent(splashScreen);

                // Finally, render it
                ScreenTools.Render(true);

                // Render the display
                SplashThread.Stop();
                SplashThread.Start(new SplashThreadParameters(splash.SplashName, context));

                // Inform the kernel that the splash has started
                SplashReport._InSplash = true;
            }
        }

        /// <summary>
        /// Closes the splash screen
        /// </summary>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void CloseSplash(SplashContext context) =>
            CloseSplash(CurrentSplash, context);

        /// <summary>
        /// Closes the splash screen
        /// </summary>
        /// <param name="splashName">Splash name</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void CloseSplash(string splashName, SplashContext context) =>
            CloseSplash(GetSplashFromName(splashName).EntryPoint, context);

        /// <summary>
        /// Closes the splash screen
        /// </summary>
        /// <param name="splash">Splash interface to use</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void CloseSplash(ISplash splash, SplashContext context) =>
            CloseSplash(splash, true, context);

        /// <summary>
        /// Closes the splash screen
        /// </summary>
        /// <param name="splash">Splash interface to use</param>
        /// <param name="showClosing">Shows the closing animation, or clears the screen</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        internal static void CloseSplash(ISplash splash, bool showClosing, SplashContext context)
        {
            if (EnableSplash)
            {
                var closingPart = new ScreenPart();
                bool delay = false;
                splashScreen.RemoveBufferedParts();
                currentContext = context;
                splash.SplashClosing = true;

                // We need to wait for the splash display thread to finish its work once SplashClosing is set, because some splashes, like PowerLine,
                // actually do some operations that take a few milliseconds to finish what it's doing, and if we didn't wait here until the operations
                // are done in the Display() function, we'd abruptly stop without waiting, causing race condition. If this happened, visual glitches
                // manifest, which is not good.
                SplashThread.Wait();
                SplashThread.Stop();
                if (showClosing)
                    closingPart.AddDynamicText(() => splash.Closing(context, out delay));
                else
                    closingPart.AddDynamicText(() => InstalledSplashes["Blank"].EntryPoint.Closing(context, out delay));
                ConsoleWrapper.CursorVisible = true;
                splashScreen.AddBufferedPart(closingPart);
                ScreenTools.Render();

                // Reset the SplashClosing variable in case it needs to be open again. Some splashes don't do anything if they detect that the splash
                // screen is closing.
                splash.SplashClosing = false;
                SplashReport._InSplash = false;
                ScreenTools.UnsetCurrent(splashScreen);

                // Wait for 3 seconds
                if (delay)
                    Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void PreviewSplash(SplashContext context) =>
            PreviewSplash(CurrentSplash, false, context);

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="splashOut">Whether to test out the important messages on splash.</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void PreviewSplash(bool splashOut, SplashContext context) =>
            PreviewSplash(CurrentSplash, splashOut, context);

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="splashName">Splash name</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void PreviewSplash(string splashName, SplashContext context) =>
            PreviewSplash(GetSplashFromName(splashName).EntryPoint, false, context);

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="splashName">Splash name</param>
        /// <param name="splashOut">Whether to test out the important messages on splash.</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void PreviewSplash(string splashName, bool splashOut, SplashContext context) =>
            PreviewSplash(GetSplashFromName(splashName).EntryPoint, splashOut, context);

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="splash">Splash name</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void PreviewSplash(ISplash splash, SplashContext context) =>
            PreviewSplash(splash, false, context);

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="splash">Splash name</param>
        /// <param name="splashOut">Whether to test out the important messages on splash.</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void PreviewSplash(ISplash splash, bool splashOut, SplashContext context)
        {
            // Open the splash and reset the report progress to 0%
            OpenSplash(splash, context);

            // Report progress 5 times
            for (int i = 1; i <= 5; i++)
            {
                int prog = i * 20;
                SplashReport.ReportProgress($"{prog}%", 20, true, splash);
                Thread.Sleep(1000);
                if (splashOut)
                {
                    BeginSplashOut(context);
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("We've reached {0}%!"), vars: prog);
                    EndSplashOut(context);
                }
            }

            // Close
            CloseSplash(splash, context);
        }

        /// <summary>
        /// Clears the screen for important messages to show up during kernel booting
        /// </summary>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void BeginSplashOut(SplashContext context)
        {
            if (EnableSplash && SplashReport._InSplash)
                CloseSplash(CurrentSplash, false, context);
        }

        /// <summary>
        /// Declares that it's done showing important messages during kernel booting
        /// </summary>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void EndSplashOut(SplashContext context)
        {
            if (EnableSplash && !SplashReport._InSplash)
                OpenSplash(context);
        }

        private static void SplashThreadHandler(SplashThreadParameters threadParameters)
        {
            var splash = GetSplashFromName(threadParameters.SplashName).EntryPoint;
            while (!splash.SplashClosing)
            {
                var displayPart = new ScreenPart();
                displayPart.AddDynamicText(() => splash.Display(threadParameters.SplashContext));
                if (splashScreen.ScreenParts.Length > 1)
                    splashScreen.EditBufferedPart(1, displayPart);
                else
                    splashScreen.AddBufferedPart(displayPart);
                ScreenTools.Render();
                Thread.Sleep(20);
            }
        }

    }
}
