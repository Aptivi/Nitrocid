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

using System.Threading;
using System;
using System.Diagnostics;
using System.Reflection;
using Textify.Versioning;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ConsoleBase;
using Nitrocid.Arguments;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Starting;
using Nitrocid.Kernel.Starting.Environment;
using Nitrocid.Users.Windows;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Power;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Nitrocid.Kernel.Configuration;
using Aptivestigate.CrashHandler;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Nitrocid.Kernel
{
    /// <summary>
    /// Kernel main class
    /// </summary>
    public static class KernelMain
    {
        internal static readonly string rootNameSpace =
            (typeof(KernelMain).Namespace?.Split('.')[0]) ?? "";
        private static readonly Version? kernelVersion =
            Assembly.GetExecutingAssembly().GetName().Version;
        private static readonly SemVer? kernelVersionFull =
            SemVer.ParseWithRev($"{kernelVersion}");

        // Refer to NitrocidModAPIVersion in the project file.
        private static readonly Version kernelApiVersion =
            new(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion ?? "0.0.0.0");

        /// <summary>
        /// Kernel version
        /// </summary>
        public static Version? Version =>
            kernelVersion;
        /// <summary>
        /// Kernel API version
        /// </summary>
        public static Version ApiVersion =>
            kernelApiVersion;
        /// <summary>
        /// Kernel version (full)
        /// </summary>
        public static SemVer? VersionFull =>
            kernelVersionFull;
        /// <summary>
        /// Kernel version (full)
        /// </summary>
        public static string VersionFullStr =>
            kernelVersionFull?.ToString() ?? "0.0.0.0";

        /// <summary>
        /// Entry point
        /// </summary>
        internal static void Main(string[] Args)
        {
            try
            {
                // Set main thread name
                Thread.CurrentThread.Name = "Main Nitrocid Kernel Thread";

                // Run unhandled crash handler
                CrashTools.InstallCrashHandler();

                // Show help / version prior to starting the kernel if help / version is passed
                ArgumentParse.ParseArguments(Args, true);

                // This is a kernel entry point
                EnvironmentTools.kernelArguments = Args;
                EnvironmentTools.ResetEnvironment();
                while (!PowerManager.KernelShutdown)
                {
                    try
                    {
                        EnvironmentTools.ExecuteEnvironment();
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
                        KernelPanic.KernelError(KernelErrorLevel.U, true, 5, LanguageTools.GetLocalized("NKS_KERNEL_ENVERROR") + $" {ex.Message}", ex);
                    }
                    finally
                    {
                        // Reset everything to their initial state
                        if (!PowerManager.hardShutdown)
                        {
                            KernelInitializers.ResetEverything();
                            PowerSignalHandlers.DisposeHandlers();

                            // Clear the console
                            KernelColorTools.LoadBackground();
                        }

                        // Always switch back to the main environment
                        if (EnvironmentTools.anotherEnvPending)
                        {
                            if (EnvironmentTools.resetEnvironment)
                            {
                                EnvironmentTools.anotherEnvPending = false;
                                EnvironmentTools.resetEnvironment = false;
                                EnvironmentTools.ResetEnvironment();
                            }
                            else
                                EnvironmentTools.resetEnvironment = true;
                        }
                    }
                }

                // If "No APM" is enabled, simply print the text
                if (Config.MainConfig.SimulateNoAPM)
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_NOAPMSIMULATION"), new InfoBoxSettings()
                    {
                        ForegroundColor = KernelColorTools.GetColor(KernelColorType.Success)
                    });
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_FATALERROR") + $" {ex.Message}", new InfoBoxSettings()
                {
                    ForegroundColor = KernelColorTools.GetColor(KernelColorType.Error)
                });
            }
            finally
            {
                // Load main buffer
                if (!KernelPlatform.IsOnWindows() && ConsoleTools.UseAltBuffer && ConsoleMisc.IsOnAltBuffer && !PowerManager.hardShutdown)
                    ConsoleMisc.ShowMainBuffer();

                // Reset colors and clear the console
                if (!PowerManager.hardShutdown)
                    ConsoleClearing.ResetAll();
                else
                    KernelColorTools.ResetColors();

                // Reset cursor state
                ConsoleWrapper.CursorVisible = true;

                // Check to see if we're restarting Nitrocid with elevated permissions
                if (PowerManager.elevating && KernelPlatform.IsOnWindows() && !WindowsUserTools.IsAdministrator())
                    PowerManager.ElevateSelf();
            }
        }
    }
}
