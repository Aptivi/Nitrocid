using System;
using System.IO;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Reflection;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Splash;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Screensaver.Customized
{
    public static class CustomSaverParser
    {

        /// <summary>
        /// Compiles the custom screensaver file and configures it so it can be viewed
        /// </summary>
        /// <param name="file">File name with .ss.vb</param>
        public static void ParseCustomSaver(string file)
        {
            // Initialize path
            string ModPath = Paths.GetKernelPath(KernelPathType.Mods);
            string FinalScreensaverPath = Filesystem.NeutralizePath(file, ModPath);
            string SaverFileName = Path.GetFileName(FinalScreensaverPath);

            // Start parsing screensaver
            if (Checking.FileExists(FinalScreensaverPath))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Parsing {0}...", SaverFileName);
                BaseScreensaver ScreensaverBase;
                if (Path.GetExtension(FinalScreensaverPath) == ".dll")
                {
                    // Try loading the screensaver
                    try
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "{0} is probably a valid screensaver. Generating...", SaverFileName);
                        ScreensaverBase = Screensaver.GetScreensaverInstance(Assembly.LoadFrom(FinalScreensaverPath));
                        if (ScreensaverBase is not null)
                        {
                            // This screensaver uses the modern BaseScreensaver and IScreensaver interfaces
                            DebugWriter.Wdbg(DebugLevel.I, "{0} is a valid screensaver!", SaverFileName);
                            SplashReport.ReportProgress(Translate.DoTranslation("{0} has been initialized properly."), 0, ColorTools.ColTypes.Neutral, SaverFileName);
                            string SaverName = ScreensaverBase.ScreensaverName;
                            CustomSaverInfo SaverInstance;
                            SaverInstance = new CustomSaverInfo(SaverName, SaverFileName, FinalScreensaverPath, ScreensaverBase);
                            CustomSaverTools.CustomSavers.Add(SaverName, SaverInstance);
                        }
                        else
                        {
                            DebugWriter.Wdbg(DebugLevel.E, "{0} is not a valid screensaver.", file);
                        }
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        DebugWriter.Wdbg(DebugLevel.E, "Error trying to load dynamic screensaver {0} because of reflection failure: {1}", file, ex.Message);
                        DebugWriter.WStkTrc(ex);
                        SplashReport.ReportProgress(Translate.DoTranslation("Screensaver can't be loaded because of the following: "), 0, ColorTools.ColTypes.Error);
                        foreach (Exception LoaderException in ex.LoaderExceptions)
                        {
                            DebugWriter.Wdbg(DebugLevel.E, "Loader exception: {0}", LoaderException.Message);
                            DebugWriter.WStkTrc(LoaderException);
                            SplashReport.ReportProgress(LoaderException.Message, 0, ColorTools.ColTypes.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.Wdbg(DebugLevel.E, "Error trying to load dynamic screensaver {0}: {1}", file, ex.Message);
                        DebugWriter.WStkTrc(ex);
                    }
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.W, "{0} is not a screensaver. A screensaver code should have \".ss.vb\" or \".dll\" at the end.", file);
                }
            }
            else
            {
                SplashReport.ReportProgress(Translate.DoTranslation("Screensaver {0} does not exist."), 0, ColorTools.ColTypes.Error, file);
                DebugWriter.Wdbg(DebugLevel.E, "The file {0} does not exist for compilation.", file);
            }
        }

    }
}