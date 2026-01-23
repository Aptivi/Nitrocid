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
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using Aptivestigate.CrashHandler;
using Nitrocid.Core.Environment;
using Nitrocid.Core.Languages;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid
{
    /// <summary>
    /// Kernel main class
    /// </summary>
    public static class KernelMain
    {
        private static bool kernelShutdown = false;

        /// <summary>
        /// Entry point
        /// </summary>
        internal static void Main(string[] Args)
        {
            try
            {
                // Prepare language
                LanguageTools.AddCustomAction("Nitrocid", new("Nitrocid.Resources.Languages.Output.Localizations", typeof(KernelMain).Assembly));

                // Set main thread name
                Thread.CurrentThread.Name = "Main Nitrocid Kernel Thread";

                // Run unhandled crash handler
                CrashTools.InstallCrashHandler();

                // This is a kernel entry point
                int faultLevel = 0;
                var uptime = new Stopwatch();
                EnvironmentTools.kernelArguments = Args;
                EnvironmentTools.ResetEnvironment();
                uptime.Start();
                while (!kernelShutdown)
                {
                    try
                    {
                        // Execute initialization point
                        EnvironmentTools.ExecuteInitEnvironment();

                        // Execute the environment
                        EnvironmentTools.ExecuteEnvironment();
                    }
                    catch (Exception ex)
                    {
                        if (uptime.ElapsedMilliseconds <= 3000)
                            faultLevel++;
                        if (faultLevel == 3)
                            Environment.FailFast(LanguageTools.GetLocalized("NKS_KERNEL_ENVERROR") + $" {ex.Message}", ex);
                        else
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_ENVERROR") + $" {ex.Message}", ThemeColorType.Error);
                    }
                    finally
                    {
                        // Execute exit point
                        EnvironmentTools.ExecuteExitEnvironment();

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

                        // Check to see if we're demanding a shutdown
                        string typeName = typeof(EnvironmentTools).Assembly?.FullName?.Replace(".Core", ".Base") ?? "";
                        var shutdownVar = Type.GetType($"Nitrocid.Base.Kernel.Power.PowerManager, {typeName}")?.GetField("KernelShutdown", BindingFlags.NonPublic | BindingFlags.Static);
                        if ((bool)(shutdownVar?.GetValue(null) ?? false))
                            kernelShutdown = true;
                        uptime.Restart();
                    }
                }
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_FATALERROR") + $" {ex}", ThemeColorType.Error);
            }
        }
    }
}
