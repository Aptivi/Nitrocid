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

using System.Threading;
using System;
using Nitrocid.Base.Kernel.Starting;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Themes.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Aptivestigate.CrashHandler;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Shell.Arguments.Base;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Arguments;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Users.Windows;
using Nitrocid.Base.Kernel.Power;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.ConsoleBase.Inputs;
using Nitrocid.Core.Environment;
using Nitrocid.Core.Languages;

namespace Nitrocid
{
    /// <summary>
    /// Kernel main class
    /// </summary>
    public static class KernelMain
    {
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

                // Show help / version prior to starting the kernel if help / version is passed
                ArgumentParse.ParseArguments(Args, KernelArguments.outArgs);

                // Show development notice
                if (!PowerManager.KernelShutdown)
                {
                    // Show the message
#if !SPECIFIERREL
                    string message =
#if SPECIFIERDEV
                        LanguageTools.GetLocalized("NKS_KERNEL_STARTING_DEVMESSAGE")
#elif SPECIFIERRC
                        LanguageTools.GetLocalized("NKS_KERNEL_STARTING_RCMESSAGE")
#elif SPECIFIERALPHA
                        LanguageTools.GetLocalized("NKS_KERNEL_STARTING_ALPHAMESSAGE")
#elif SPECIFIERBETA
                        LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BETAMESSAGE")
#else
                        LanguageTools.GetLocalized("NKS_KERNEL_STARTING_UNSUPPORTED")
#endif
                    ;
                    TextWriterColor.Write(message, true, ThemeColorType.Warning);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_COMMON_ANYKEY"), true, ThemeColorType.Warning);
                    InputTools.DetectKeypress();
#endif
                }

                // This is a kernel entry point
                EnvironmentTools.kernelArguments = Args;
                EnvironmentTools.ResetEnvironment();
                while (!PowerManager.KernelShutdown)
                {
                    try
                    {
                        EnvironmentTools.ExecuteEnvironment();
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
                            ThemeColorsTools.LoadBackground();
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
                        ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Success)
                    });
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_FATALERROR") + $" {ex.Message}", new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                });
            }
            finally
            {
                // Load main buffer
                if (!KernelPlatform.IsOnWindows() && KernelEntry.UseAltBuffer && ConsoleMisc.IsOnAltBuffer && !PowerManager.hardShutdown)
                    ConsoleMisc.ShowMainBuffer();

                // Reset colors and clear the console
                if (!PowerManager.hardShutdown)
                    ConsoleClearing.ResetAll();
                else
                    ThemeColorsTools.ResetColors();

                // Reset cursor state
                ConsoleWrapper.CursorVisible = true;

                // Check to see if we're restarting Nitrocid with elevated permissions
                if (PowerManager.elevating && KernelPlatform.IsOnWindows() && !WindowsUserTools.IsAdministrator())
                    PowerManager.ElevateSelf();
            }
        }
    }
}
