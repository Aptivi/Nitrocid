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
using System.Threading;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Themes.Colors;
using BassBoom.Basolia;
using BassBoom.Basolia.File;
using BassBoom.Basolia.Playback;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Misc.Audio;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Events;
using Terminaux.Base.Extensions;
using Terminaux.Inputs;

namespace Nitrocid.Base.Misc.Screensaver
{
    /// <summary>
    /// Screensaver display module
    /// </summary>
    internal static class ScreensaverDisplayer
    {

        internal readonly static KernelThread ScreensaverDisplayerThread = new("Screensaver display thread", false, (objArray) => DisplayScreensaver((object[]?)objArray ?? []));
        internal readonly static KernelThread ScreensaverAmbienceThread = new("Screensaver ambience thread", false, ScreensaverAmbience);

        internal static BaseScreensaver? displayingSaver;

        /// <summary>
        /// Displays the screensaver from the screensaver base
        /// </summary>
        /// <param name="threadParams">
        /// Thread parameters: <br></br><br></br>
        /// - Screensaver base containing information about the screensaver<br></br>
        /// - Boolean variable that describes whether to show the seizure warning
        /// </param>
        internal static void DisplayScreensaver(object[] threadParams)
        {
            if (threadParams.Length == 0)
                throw new KernelException(KernelExceptionType.ScreensaverManagement, LanguageTools.GetLocalized("NKS_MISC_SCREENSAVER_EXCEPTION_NEEDSSCREENSAVER"));
            BaseScreensaver? Screensaver = (BaseScreensaver?)threadParams[0] ??
                throw new KernelException(KernelExceptionType.ScreensaverManagement, LanguageTools.GetLocalized("NKS_MISC_SCREENSAVER_EXCEPTION_NEEDSSCREENSAVER"));
            bool displaySeizureWarning = (bool?)threadParams[1] ?? false;
            bool initialVisible = ConsoleWrapper.CursorVisible;
            bool initialBack = ConsoleColoring.AllowBackground;
            bool initialPalette = ColorTools.GlobalSettings.UseTerminalPalette;
            try
            {
                // Show the seizure warning if required
                if (displaySeizureWarning)
                {
                    Screensaver.ScreensaverSeizureWarning();
                    bool result = SpinWait.SpinUntil(() => ScreensaverDisplayerThread.IsStopping || ScreensaverManager.bailing, new TimeSpan(0, 0, 10));
                    if (result)
                        return;
                }

                // Preparations
                displayingSaver = Screensaver;
                ConsoleColoring.AllowBackground = true;
                ColorTools.GlobalSettings.UseTerminalPalette = false;
                Screensaver.ScreensaverPreparation();

                // Execute the actual screensaver logic
                while (!ScreensaverDisplayerThread.IsStopping && !ScreensaverManager.bailing)
                {
                    if (ConsoleWrapper.CursorVisible)
                        ConsoleWrapper.CursorVisible = false;
                    Screensaver.ScreensaverLogic();
                    ConsoleResizeHandler.WasResized();
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Screensaver is stopping due to user request...");
            }
            catch (Exception ex)
            {
                ScreensaverManager.HandleSaverError(ex, initialVisible);
            }
            finally
            {
                Screensaver.ScreensaverOutro();
                ScreensaverManager.HandleSaverCancel(initialVisible);
                ConsoleColoring.AllowBackground = initialBack;
                ColorTools.GlobalSettings.UseTerminalPalette = initialPalette;
                displayingSaver = null;
                ThemeColorsTools.LoadBackground();
            }
        }

        internal static void BailFromScreensaver()
        {
            if (ScreensaverManager.InSaver)
            {
                ScreensaverManager.bailing = true;
                ScreensaverDisplayerThread.Stop(false);
                ScreensaverAmbienceThread.Stop(false);
                ScreensaverManager.SaverAutoReset.WaitOne();

                // Raise event
                DebugWriter.WriteDebug(DebugLevel.I, "Screensaver really stopped.");
                EventsManager.FireEvent(EventType.PostShowScreensaver);
                ScreensaverManager.bailing = false;
                ScreensaverManager.inSaver = false;
                ScreensaverManager.ScrnTimeReached = false;
                ScreensaverDisplayerThread.Regen();
                ScreensaverAmbienceThread.Regen();
            }
        }

        internal static void ScreensaverAmbience()
        {
            try
            {
                var basoliaMedia = new BasoliaMedia();
                DebugWriter.WriteDebug(DebugLevel.I, $"Screensaver ambience starting with theme {Config.MainConfig.AudioCueThemeName}");
                
                // Open the ambient SFX stream
                var ambientFxType = AudioCuesTools.MapAmbienceToCueType(Config.MainConfig.AmbientSoundFxIntensity);
                var cue = AudioCuesTools.GetAudioCue();

                // Repeatedly play it
                while (!ScreensaverAmbienceThread.IsStopping)
                {
                    var ambientStream = cue.GetStream(ambientFxType) ??
                        throw new KernelException(KernelExceptionType.AudioCue, LanguageTools.GetLocalized("NKS_MISC_SCREENSAVER_EXCEPTION_NOCUE"));

                    try
                    {
                        FileTools.OpenFrom(basoliaMedia, ambientStream);
                        DebugWriter.WriteDebug(DebugLevel.I, $"Restarting screensaver ambience {ambientFxType} from {Config.MainConfig.AudioCueThemeName}...");
                        PlaybackTools.PlayAsync(basoliaMedia);
                        if (!SpinWait.SpinUntil(() => PlaybackTools.GetState(basoliaMedia) == PlaybackState.Playing, 15000))
                            throw new KernelException(KernelExceptionType.AudioCue, LanguageTools.GetLocalized("NKS_MISC_SCREENSAVER_AMBIENT_EXCEPTION_SOUNDPLAYTIMEOUT"));
                        while (PlaybackTools.GetState(basoliaMedia) == PlaybackState.Playing && !ScreensaverDisplayerThread.IsStopping) ;
                        ambientStream.Seek(0, System.IO.SeekOrigin.Begin);
                        FileTools.CloseFile(basoliaMedia);
                    }
                    catch (ThreadInterruptedException)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Screensaver ambience is stopping due to user request...");
                        break;
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, $"Screensaver ambience is stopping due to failure. {ex.Message}");
                        DebugWriter.WriteDebugStackTrace(ex);
                        break;
                    }
                }
                if (PlaybackTools.GetState(basoliaMedia) != PlaybackState.Stopped)
                    PlaybackTools.Stop(basoliaMedia);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Screensaver ambience is stopping due to failure. {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }
    }
}
