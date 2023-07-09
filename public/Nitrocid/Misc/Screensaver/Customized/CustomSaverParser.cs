
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
using System.IO;
using System.Linq;
using System.Reflection;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Splash;

namespace KS.Misc.Screensaver.Customized
{
    /// <summary>
    /// Custom screensaver parser
    /// </summary>
    public static class CustomSaverParser
    {

        /// <summary>
        /// Parses all the custom screensavers found in KSScreensavers
        /// </summary>
        public static void ParseCustomSavers()
        {
            string SaversPath = Paths.GetKernelPath(KernelPathType.Screensavers);
            DebugWriter.WriteDebug(DebugLevel.I, "Safe mode: {0}", Flags.SafeMode);
            if (!Flags.SafeMode)
            {
                // We're not in safe mode. We're good now.
                if (!Checking.FolderExists(SaversPath))
                    Directory.CreateDirectory(SaversPath);
                int count = Directory.EnumerateFiles(SaversPath).Count();
                DebugWriter.WriteDebug(DebugLevel.I, "Files count: {0}", count);

                // Check to see if we have screensavers
                if (count != 0)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("Loading custom screensavers..."), 0);
                    DebugWriter.WriteDebug(DebugLevel.I, "Screensavers are being loaded. Total mods = {0}", count);
                    foreach (string saverFilePath in Directory.EnumerateFiles(SaversPath))
                    {
                        string saverFile = Path.GetFileName(saverFilePath);
                        DebugWriter.WriteDebug(DebugLevel.I, "Starting screensaver {0}...", saverFile);
                        SplashReport.ReportProgress(Translate.DoTranslation("Loading screensaver") + " {0}...", 0, saverFile);
                        ParseCustomSaver(saverFilePath);
                    }
                }
                else
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("No screensavers detected."), 0);
                }
            }
            else
            {
                SplashReport.ReportProgressError(Translate.DoTranslation("Parsing screensavers not allowed on safe mode."));
            }
        }

        /// <summary>
        /// Compiles the custom screensaver file and configures it so it can be viewed
        /// </summary>
        /// <param name="file">File name with .dll</param>
        public static void ParseCustomSaver(string file)
        {
            // Initialize path
            string ModPath = Paths.GetKernelPath(KernelPathType.Mods);
            string FinalScreensaverPath = Filesystem.NeutralizePath(file, ModPath);
            string SaverFileName = Path.GetFileName(FinalScreensaverPath);

            // Start parsing screensaver
            if (Checking.FileExists(FinalScreensaverPath))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Parsing {0}...", SaverFileName);
                BaseScreensaver ScreensaverBase;
                if (Path.GetExtension(FinalScreensaverPath) == ".dll")
                {
                    // Try loading the screensaver
                    try
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "{0} is probably a valid screensaver. Generating...", SaverFileName);
                        ScreensaverBase = Screensaver.GetScreensaverInstance(Assembly.LoadFrom(FinalScreensaverPath));
                        if (ScreensaverBase is not null)
                        {
                            // This screensaver uses the modern BaseScreensaver and IScreensaver interfaces
                            DebugWriter.WriteDebug(DebugLevel.I, "{0} is a valid screensaver!", SaverFileName);
                            SplashReport.ReportProgress(Translate.DoTranslation("{0} has been initialized properly."), 0, SaverFileName);
                            string SaverName = ScreensaverBase.ScreensaverName;
                            bool updating = CustomSaverTools.CustomSavers.ContainsKey(SaverName);
                            CustomSaverInfo SaverInstance;
                            SaverInstance = new CustomSaverInfo(SaverName, SaverFileName, FinalScreensaverPath, ScreensaverBase);
                            if (updating)
                            {
                                SplashReport.ReportProgress(Translate.DoTranslation("{0} has already been initialized. Updating screensaver..."), 0, SaverFileName);
                                CustomSaverTools.CustomSavers.Remove(SaverName);
                            }
                            CustomSaverTools.CustomSavers.Add(SaverName, SaverInstance);
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "{0} is not a valid screensaver.", file);
                        }
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Error trying to load dynamic screensaver {0} because of reflection failure: {1}", file, ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                        SplashReport.ReportProgressError(Translate.DoTranslation("Screensaver can't be loaded because of the following: "));
                        foreach (Exception LoaderException in ex.LoaderExceptions)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Loader exception: {0}", LoaderException.Message);
                            DebugWriter.WriteDebugStackTrace(LoaderException);
                            SplashReport.ReportProgressError(LoaderException.Message);
                            if (LoaderException.GetType() == typeof(FileLoadException) && ((FileLoadException)LoaderException).FileName.Contains("Kernel Simulator"))
                                SplashReport.ReportProgressError(Translate.DoTranslation("When the kernel tried to load the specified screensaver, it requested loading \"Kernel Simulator\". Since the main application is renamed to Nitrocid KS, this screensaver can't be run safely. We advice you to upgrade the screensaver."));
                        }
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Error trying to load dynamic screensaver {0}: {1}", file, ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "{0} is not a screensaver. A screensaver code should have \".dll\" at the end.", file);
                }
            }
            else
            {
                SplashReport.ReportProgressError(Translate.DoTranslation("Screensaver {0} does not exist."), file);
                DebugWriter.WriteDebug(DebugLevel.E, "The file {0} does not exist for compilation.", file);
            }
        }

    }
}
