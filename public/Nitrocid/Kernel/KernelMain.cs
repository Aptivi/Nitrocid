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

using System.Threading;
using KS.ConsoleBase;
using KS.Languages;
using KS.Kernel.Power;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Starting.Environment;
using KS.ConsoleBase.Colors;
using System;
using KS.Kernel.Exceptions;
using KS.Kernel.Debugging;
using KS.Kernel.Starting;
using KS.Arguments;
using KS.Arguments.Help;
using KS.Users.Windows;
using System.Diagnostics;
using System.Reflection;
using SemanVer.Instance;
using KS.ConsoleBase.Inputs.Styles.Infobox;

namespace KS.Kernel
{
    /// <summary>
    /// Kernel main class
    /// </summary>
    public static class KernelMain
    {
        private static readonly Version kernelVersion =
            Assembly.GetExecutingAssembly().GetName().Version;
        private static readonly SemVer kernelVersionFull =
            SemVer.ParseWithRev($"{kernelVersion}-b2");
        private static readonly Version kernelApiVersion =
            new(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);

        /// <summary>
        /// Kernel version
        /// </summary>
        public static Version Version =>
            kernelVersion;
        /// <summary>
        /// Kernel API version
        /// </summary>
        // Refer to NitrocidModAPIVersion in the project file.
        public static Version ApiVersion =>
            kernelApiVersion;
        /// <summary>
        /// Kernel version (full)
        /// </summary>
        public static SemVer VersionFull =>
            kernelVersionFull;
        /// <summary>
        /// Kernel version (full)
        /// </summary>
        public static string VersionFullStr =>
            kernelVersionFull.ToString();

        /// <summary>
        /// Entry point
        /// </summary>
        internal static void Main(string[] Args)
        {
            // Set main thread name
            Thread.CurrentThread.Name = "Main Nitrocid Kernel Thread";

            // Show help prior to starting the kernel if help is passed
            if (ArgumentParse.IsArgumentPassed(Args, "help"))
            {
                // Kernel arguments
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Available kernel arguments:"), true, KernelColorType.ListTitle);
                ArgumentHelpPrint.ShowArgsHelp();
                ConsoleExtensions.ResetColors();
                return;
            }

            // This is a kernel entry point
            while (!PowerManager.KernelShutdown)
            {
                try
                {
                    EnvironmentTools.ExecuteEnvironment(Args);
                }
                catch (KernelException icde) when (icde.ExceptionType == KernelExceptionType.InsaneConsoleDetected)
                {
                    ConsoleWrapper.WriteLine(icde.Message);
                    PowerManager.KernelShutdown = true;
                }
                catch (KernelErrorException kee)
                {
                    DebugWriter.WriteDebugStackTrace(kee);
                    KernelEntry.SafeMode = false;
                }
                catch (Exception ex)
                {
                    KernelPanic.KernelError(KernelErrorLevel.U, true, 5, Translate.DoTranslation("Kernel environment error:") + $" {ex.Message}", ex);
                }
                finally
                {
                    // Reset everything to their initial state
                    if (!PowerManager.hardShutdown)
                    {
                        KernelInitializers.ResetEverything();

                        // Clear the console
                        KernelColorTools.LoadBack();
                    }

                    // Always switch back to the main environment
                    if (EnvironmentTools.resetEnvironment)
                    {
                        EnvironmentTools.resetEnvironment = false;
                        EnvironmentTools.ResetEnvironment();
                    }
                }
            }

            // If "No APM" is enabled, simply print the text
            if (PowerManager.SimulateNoAPM)
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("It's now safe to turn off your computer."));

            // Load main buffer
            if (!KernelPlatform.IsOnWindows() && ConsoleExtensions.UseAltBuffer && ConsoleExtensions.HasSetAltBuffer && !PowerManager.hardShutdown)
                ConsoleExtensions.ShowMainBuffer();

            // Reset colors and clear the console
            if (!PowerManager.hardShutdown)
                ConsoleExtensions.ResetAll();
            else
                ConsoleExtensions.ResetColors();

            // Reset cursor state and dispose handlers
            ConsoleWrapper.CursorVisible = true;
            PowerSignalHandlers.DisposeHandlers();

            // Check to see if we're restarting Nitrocid with elevated permissions
            if (PowerManager.elevating && KernelPlatform.IsOnWindows() && !WindowsUserTools.IsAdministrator())
                PowerManager.ElevateSelf();
        }
    }
}
