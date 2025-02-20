﻿//
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
using System.Linq;
using System.Threading;
using Terminaux.Base.Buffered;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Threading;
using Nitrocid.Languages;
using Nitrocid.Misc.Splash.Splashes;
using Terminaux.Base;

namespace Nitrocid.Misc.Splash
{
    /// <summary>
    /// Splash management module
    /// </summary>
    public static class SplashManager
    {

        internal static Screen splashScreen = new();
        internal static SplashContext currentContext = SplashContext.StartingUp;
        internal static KernelThread SplashThread = new("Kernel Splash Thread", false, (splashParams) => SplashThreadHandler((SplashThreadParameters?)splashParams));
        internal static SplashInfo fallbackSplash = new("Welcome", new SplashWelcome());
        internal static SplashInfo blankSplash = new("Blank", new SplashBlank(), false);
        internal readonly static List<SplashInfo> builtinSplashes =
        [
            // They are the base splashes. They shouldn't be moved to the splash addon pack as such movement breaks things.
            fallbackSplash,
            blankSplash,
        ];
        internal readonly static List<SplashInfo> customSplashes = [];

        /// <summary>
        /// Current splash screen
        /// </summary>
        public static ISplash CurrentSplash =>
            GetSplashFromName(Config.MainConfig.SplashName).EntryPoint;

        /// <summary>
        /// Current splash screen info instance
        /// </summary>
        public static SplashInfo CurrentSplashInfo =>
            GetSplashFromName(Config.MainConfig.SplashName);

        /// <summary>
        /// All the installed splashes either normal or custom
        /// </summary>
        public static SplashInfo[] Splashes =>
            builtinSplashes
                .Union(customSplashes)
                .ToArray();

        /// <summary>
        /// Gets the current splash context
        /// </summary>
        public static SplashContext CurrentSplashContext =>
            currentContext;

        /// <summary>
        /// Gets names of the installed splashes
        /// </summary>
        public static string[] GetNamesOfSplashes() =>
            Splashes
                .Select((info) => info.SplashName)
                .ToArray();

        /// <summary>
        /// Registers a custom splash to the list
        /// </summary>
        /// <param name="splashInfo">A custom splash information</param>
        /// <exception cref="KernelException"></exception>
        public static void RegisterSplash(SplashInfo splashInfo)
        {
            // Check for splash info sanity
            if (splashInfo is null)
                throw new KernelException(KernelExceptionType.Splash, Translate.DoTranslation("Can't register empty splash"));
            if (string.IsNullOrEmpty(splashInfo.SplashName))
                throw new KernelException(KernelExceptionType.Splash, Translate.DoTranslation("Can't register splash with no name"));
            if (splashInfo.EntryPoint is null)
                throw new KernelException(KernelExceptionType.Splash, Translate.DoTranslation("Can't register splash with no entry point"));

            // Now, register the splash to the custom splash list
            if (IsSplashRegistered(splashInfo.SplashName))
                throw new KernelException(KernelExceptionType.Splash, Translate.DoTranslation("Can't re-register splash. You need to unregister that splash first."));
            customSplashes.Add(splashInfo);
        }

        /// <summary>
        /// Unregisters a custom splash to the list
        /// </summary>
        /// <param name="splashName">A custom splash name</param>
        /// <exception cref="KernelException"></exception>
        public static void UnregisterSplash(string splashName)
        {
            // Check to see if we have the splash
            if (string.IsNullOrEmpty(splashName))
                throw new KernelException(KernelExceptionType.Splash, Translate.DoTranslation("Can't unregister splash with no name"));
            if (IsSplashRegistered(splashName))
                throw new KernelException(KernelExceptionType.Splash, Translate.DoTranslation("Can't unregister non-existent splash"));

            // Now, unregister the splash
            var splash = GetSplashFromName(splashName);
            customSplashes.Remove(splash);
        }

        /// <summary>
        /// Checks to see if a splash is registered
        /// </summary>
        /// <param name="splashName">A splash name</param>
        /// <exception cref="KernelException"></exception>
        public static bool IsSplashRegistered(string splashName)
        {
            // Check to see if we have the splash
            var names = GetNamesOfSplashes();
            return names.Contains(splashName);
        }

        /// <summary>
        /// Gets the splash information from the name
        /// </summary>
        /// <param name="splashName">Splash name</param>
        /// <returns>Splash information</returns>
        public static SplashInfo GetSplashFromName(string splashName)
        {
            if (IsSplashRegistered(splashName))
                return Splashes.First((info) => info.SplashName == splashName);
            else
                return Splashes.First((info) => info.SplashName == "Welcome");
        }

        /// <summary>
        /// Opens the splash screen
        /// </summary>
        public static void OpenSplash() =>
            OpenSplash(CurrentSplash, currentContext);

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
        public static void OpenSplash(string splashName) =>
            OpenSplash(GetSplashFromName(splashName).EntryPoint, currentContext);

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
        public static void OpenSplash(ISplash splash) =>
            OpenSplash(splash, currentContext);

        /// <summary>
        /// Opens the splash screen
        /// </summary>
        /// <param name="splash">Splash interface to use</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void OpenSplash(ISplash splash, SplashContext context)
        {
            if (Config.MainConfig.EnableSplash)
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
                splashScreen.AddBufferedPart("Opening splash", openingPart);

                // Make it resize-aware
                ScreenTools.SetCurrent(splashScreen);

                // Finally, render it
                KernelColorTools.LoadBackground();
                ScreenTools.Render();

                // Render the display
                SplashThread.Stop();
                SplashThread.Start(new SplashThreadParameters(splash.SplashName, context));

                // Inform the kernel that the splash has started
                SplashReport._InSplash = true;
                SplashReport.ResetProgressReportArea();
            }
        }

        /// <summary>
        /// Closes the splash screen
        /// </summary>
        public static void CloseSplash() =>
            CloseSplash(CurrentSplash, currentContext);

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
        public static void CloseSplash(string splashName) =>
            CloseSplash(GetSplashFromName(splashName).EntryPoint, currentContext);

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
        public static void CloseSplash(ISplash splash) =>
            CloseSplash(splash, true, currentContext);

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
            if (Config.MainConfig.EnableSplash)
            {
                bool delay = false;
                try
                {
                    var closingPart = new ScreenPart();
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
                        closingPart.AddDynamicText(() => blankSplash.EntryPoint.Closing(context, out delay));
                    splashScreen.AddBufferedPart("Closing splash", closingPart);
                    if (ScreenTools.CurrentScreen is not null)
                        ScreenTools.Render();
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"Splash closing failed to display: {ex.Message}");
                    DebugWriter.WriteDebugStackTrace(ex);
                    InfoBoxModalColor.WriteInfoBoxModalColor(Translate.DoTranslation("The closing splash has failed to display") + $".\n  - {ex.Message}", KernelColorTools.GetColor(KernelColorType.Error));
                }
                finally
                {
                    // Reset the SplashClosing variable in case it needs to be open again. Some splashes don't do anything if they detect that the splash
                    // screen is closing.
                    splash.SplashClosing = false;

                    // Wait for 3 seconds
                    if (delay)
                        Thread.Sleep(3000);

                    // Reset the state
                    SplashReport._InSplash = false;
                    ScreenTools.UnsetCurrent(splashScreen);

                    // Reset the cursor visibility
                    ConsoleWrapper.CursorVisible = true;
                }
            }
        }

        /// <summary>
        /// Clears the screen for important messages to show up during kernel booting
        /// </summary>
        public static void BeginSplashOut() =>
            BeginSplashOut(currentContext);

        /// <summary>
        /// Clears the screen for important messages to show up during kernel booting
        /// </summary>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void BeginSplashOut(SplashContext context)
        {
            if (Config.MainConfig.EnableSplash && SplashReport._InSplash)
                CloseSplash(CurrentSplash, false, context);
        }

        /// <summary>
        /// Declares that it's done showing important messages during kernel booting
        /// </summary>
        public static void EndSplashOut() =>
            EndSplashOut(currentContext);

        /// <summary>
        /// Declares that it's done showing important messages during kernel booting
        /// </summary>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        public static void EndSplashOut(SplashContext context)
        {
            if (Config.MainConfig.EnableSplash && !SplashReport._InSplash)
                OpenSplash(context);
        }

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        internal static void PreviewSplash(SplashContext context) =>
            PreviewSplash(CurrentSplash, false, context);

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="splashOut">Whether to test out the important messages on splash.</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        internal static void PreviewSplash(bool splashOut, SplashContext context) =>
            PreviewSplash(CurrentSplash, splashOut, context);

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="splashName">Splash name</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        internal static void PreviewSplash(string splashName, SplashContext context) =>
            PreviewSplash(GetSplashFromName(splashName).EntryPoint, false, context);

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="splashName">Splash name</param>
        /// <param name="splashOut">Whether to test out the important messages on splash.</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        internal static void PreviewSplash(string splashName, bool splashOut, SplashContext context) =>
            PreviewSplash(GetSplashFromName(splashName).EntryPoint, splashOut, context);

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="splash">Splash name</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        internal static void PreviewSplash(ISplash splash, SplashContext context) =>
            PreviewSplash(splash, false, context);

        /// <summary>
        /// Previews the splash by name
        /// </summary>
        /// <param name="splash">Splash name</param>
        /// <param name="splashOut">Whether to test out the important messages on splash.</param>
        /// <param name="context">Context of the splash screen (can be used as a reason as to why do you want to display the splash)</param>
        internal static void PreviewSplash(ISplash splash, bool splashOut, SplashContext context)
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
                    InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("We've reached {0}%!"), vars: prog);
                    EndSplashOut(context);
                }
            }

            // Close
            CloseSplash(splash, context);
        }

        private static void SplashThreadHandler(SplashThreadParameters? threadParameters)
        {
            try
            {
                if (threadParameters is null)
                    throw new KernelException(KernelExceptionType.Splash, Translate.DoTranslation("Splash thread parameters are not specified"));
                var splash = GetSplashFromName(threadParameters.SplashName).EntryPoint;
                while (!splash.SplashClosing)
                {
                    var displayPart = new ScreenPart();
                    displayPart.AddDynamicText(() => splash.Display(threadParameters.SplashContext));
                    if (splashScreen.CheckBufferedPart("Display"))
                        splashScreen.EditBufferedPart("Display", displayPart);
                    else
                        splashScreen.AddBufferedPart("Display", displayPart);
                    ScreenTools.Render();
                    Thread.Sleep(20);
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.I, $"Splash exiting because {nameof(CloseSplash)}() is called.");
            }
        }

    }
}
