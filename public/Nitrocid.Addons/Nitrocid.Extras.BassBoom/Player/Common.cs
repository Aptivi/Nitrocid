//
// BassBoom  Copyright (C) 2023  Aptivi
//
// This file is part of BassBoom
//
// BassBoom is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BassBoom is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using BassBoom.Basolia;
using BassBoom.Basolia.Devices;
using BassBoom.Basolia.File;
using BassBoom.Basolia.Format;
using BassBoom.Basolia.Playback;
using Nitrocid.Extras.BassBoom.Player.Tools;
using Nitrocid.Languages;
using SpecProbe.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles.Infobox;

namespace Nitrocid.Extras.BassBoom.Player
{
    internal static class Common
    {
        internal static double volume = 1.0;
        internal static bool enableDisco = false;
        internal static int currentPos = 1;
        internal static bool exiting = false;
        internal static bool advance = false;
        internal static bool populate = true;
        internal static bool paused = false;
        internal static bool failedToPlay = false;
        internal static bool isRadioMode = false;
        internal static readonly List<CachedSongInfo> cachedInfos = [];

        internal static CachedSongInfo CurrentCachedInfo =>
            cachedInfos.Count > 0 ? cachedInfos[currentPos - 1] : null;

        internal static void RaiseVolume()
        {
            volume += 0.05;
            if (volume > 1)
                volume = 1;
            PlaybackTools.SetVolume(volume);
        }

        internal static void LowerVolume()
        {
            volume -= 0.05;
            if (volume < 0)
                volume = 0;
            PlaybackTools.SetVolume(volume);
        }

        internal static void Exit()
        {
            exiting = true;
            advance = false;
            if (FileTools.IsOpened)
                PlaybackTools.Stop();
        }

        internal static void Switch(string musicPath)
        {
            if (FileTools.IsOpened)
                FileTools.CloseFile();
            if (isRadioMode)
                FileTools.OpenUrl(musicPath);
            else
                FileTools.OpenFile(musicPath);
        }

        internal static void ShowDeviceDriver()
        {
            var builder = new StringBuilder();
            var currentBuilder = new StringBuilder();
            if (PlaybackTools.Playing)
            {
                var (driver, device) = DeviceTools.GetCurrent();
                var cached = DeviceTools.GetCurrentCached();
                currentBuilder.AppendLine(
                    $$"""
                    {{Translate.DoTranslation("Device")}}: {{device}}
                    {{Translate.DoTranslation("Driver")}}: {{driver}}
                    {{Translate.DoTranslation("Device (cached")}}: {{cached.device}}
                    {{Translate.DoTranslation("Driver (cached")}}: {{cached.driver}}
                    """
                );
            }
            else
                currentBuilder.AppendLine(Translate.DoTranslation("Can't query current devices while not playing."));
            var drivers = DeviceTools.GetDrivers();
            string activeDevice = "";
            foreach (var driver in drivers)
            {
                try
                {
                    builder.AppendLine($"- {driver.Key}: {driver.Value}");
                    var devices = DeviceTools.GetDevices(driver.Key, ref activeDevice);
                    foreach (var device in devices)
                        builder.AppendLine($"  - {device.Key}: {device.Value}");
                }
                catch
                {
                    continue;
                }
            }
            string section1 = Translate.DoTranslation("Device and Driver");
            string section2 = Translate.DoTranslation("Available devices and drivers");
            InfoBoxColor.WriteInfoBox(
                $$"""
                {{section1}}
                {{new string('=', ConsoleChar.EstimateCellWidth(section1))}}

                {{currentBuilder}}

                {{section2}}
                {{new string('=', ConsoleChar.EstimateCellWidth(section2))}}

                {{builder}}
                """
            );
        }

        internal static void ShowSpecs()
        {
            string section1 = Translate.DoTranslation("BassBoom specifications");
            string section2 = Translate.DoTranslation("Decoders");
            string section3 = Translate.DoTranslation("System specifications");
            InfoBoxColor.WriteInfoBox(
                $$"""
                {{section1}}
                {{new string('=', ConsoleChar.EstimateCellWidth(section1))}}

                {{Translate.DoTranslation("Basolia version")}}: {{InitBasolia.BasoliaVersion}}
                {{Translate.DoTranslation("MPG123 version")}}: {{InitBasolia.MpgLibVersion}}
                {{Translate.DoTranslation("OUT123 version")}}: {{InitBasolia.OutLibVersion}}

                {{section2}}
                {{new string('=', ConsoleChar.EstimateCellWidth(section2))}}

                {{Translate.DoTranslation("Supported decoders")}}:
                  - {{string.Join("\n  - ", DecodeTools.GetDecoders(true))}}

                {{Translate.DoTranslation("All decoders")}}:
                  - {{string.Join("\n  - ", DecodeTools.GetDecoders(false))}}

                {{section3}}
                {{new string('=', ConsoleChar.EstimateCellWidth(section3))}}

                {{Translate.DoTranslation("System")}}: {{(PlatformHelper.IsOnWindows() ? "Windows" : PlatformHelper.IsOnMacOS() ? "macOS" : "Unix/Linux")}}
                {{Translate.DoTranslation("System Architecture")}}: {{RuntimeInformation.OSArchitecture}}
                {{Translate.DoTranslation("Process Architecture")}}: {{RuntimeInformation.ProcessArchitecture}}
                {{Translate.DoTranslation("System description")}}: {{RuntimeInformation.OSDescription}}
                {{Translate.DoTranslation(".NET description")}}: {{RuntimeInformation.FrameworkDescription}}
                """
            );
        }

        internal static void ShowHelp()
        {
            string section1 = Translate.DoTranslation("Available keystrokes");
            InfoBoxColor.WriteInfoBox(
                $$"""
                {{section1}}
                {{new string('=', ConsoleChar.EstimateCellWidth(section1))}}

                [SPACE]             {{Translate.DoTranslation("Play/Pause")}}
                [ESC]               {{Translate.DoTranslation("Stop")}}
                [Q]                 {{Translate.DoTranslation("Exit")}}
                [UP/DOWN]           {{Translate.DoTranslation("Volume control")}}
                [<-/->]             {{Translate.DoTranslation("Seek control")}}
                [CTRL] + [<-/->]    {{Translate.DoTranslation("Seek duration control")}}
                [I]                 {{Translate.DoTranslation("Song info")}}
                [A]                 {{Translate.DoTranslation("Add a music file")}}
                [S]                 {{Translate.DoTranslation("(when idle) Add a music directory to the playlist")}}
                [B]                 {{Translate.DoTranslation("Previous song")}}
                [N]                 {{Translate.DoTranslation("Next song")}}
                [R]                 {{Translate.DoTranslation("Remove current song")}}
                [CTRL] + [R]        {{Translate.DoTranslation("Remove all songs")}}
                [S]                 {{Translate.DoTranslation("(when playing) Selectively seek")}}
                [F]                 {{Translate.DoTranslation("(when playing) Seek to previous lyric")}}
                [G]                 {{Translate.DoTranslation("(when playing) Seek to next lyric")}}
                [J]                 {{Translate.DoTranslation("(when playing) Seek to current lyric")}}
                [K]                 {{Translate.DoTranslation("(when playing) Seek to which lyric")}}
                [C]                 {{Translate.DoTranslation("Set repeat checkpoint")}}
                [SHIFT] + [C]       {{Translate.DoTranslation("Seek to repeat checkpoint")}}
                [E]                 {{Translate.DoTranslation("Opens the equalizer")}}
                [D]                 {{Translate.DoTranslation("Device and driver info")}}
                [CTRL] + [D]        {{Translate.DoTranslation("Set device and driver")}}
                [SHIFT] + [D]       {{Translate.DoTranslation("Reset device and driver")}}
                [Z]                 {{Translate.DoTranslation("System info")}}
                """
            );
        }

        internal static void ShowHelpRadio()
        {
            string section1 = Translate.DoTranslation("Available keystrokes");
            InfoBoxColor.WriteInfoBox(
                $$"""
                {{section1}}
                {{new string('=', ConsoleChar.EstimateCellWidth(section1))}}
                
                [SPACE]             {{Translate.DoTranslation("Play/Pause")}}
                [ESC]               {{Translate.DoTranslation("Stop")}}
                [Q]                 {{Translate.DoTranslation("Exit")}}
                [UP/DOWN]           {{Translate.DoTranslation("Volume control")}}
                [I]                 {{Translate.DoTranslation("Radio station info")}}
                [CTRL] + [I]        {{Translate.DoTranslation("Radio station extended info")}}
                [A]                 {{Translate.DoTranslation("Add a radio station")}}
                [B]                 {{Translate.DoTranslation("Previous radio station")}}
                [N]                 {{Translate.DoTranslation("Next radio station")}}
                [R]                 {{Translate.DoTranslation("Remove current radio station")}}
                [CTRL] + [R]        {{Translate.DoTranslation("Remove all radio stations")}}
                [E]                 {{Translate.DoTranslation("Opens the equalizer")}}
                [D]                 {{Translate.DoTranslation("Device and driver info")}}
                [CTRL] + [D]        {{Translate.DoTranslation("Set device and driver")}}
                [SHIFT] + [D]       {{Translate.DoTranslation("Reset device and driver")}}
                [Z]                 {{Translate.DoTranslation("System info")}}
                """
            );
        }

        internal static void HandleKeypressCommon(ConsoleKeyInfo keystroke, Screen playerScreen, bool radio)
        {
            switch (keystroke.Key)
            {
                case ConsoleKey.UpArrow:
                    RaiseVolume();
                    break;
                case ConsoleKey.DownArrow:
                    LowerVolume();
                    break;
                case ConsoleKey.H:
                    if (radio)
                        ShowHelpRadio();
                    else
                        ShowHelp();
                    playerScreen.RequireRefresh();
                    break;
                case ConsoleKey.E:
                    Equalizer.OpenEqualizer(playerScreen);
                    playerScreen.RequireRefresh();
                    break;
                case ConsoleKey.Z:
                    ShowSpecs();
                    playerScreen.RequireRefresh();
                    break;
                case ConsoleKey.L:
                    enableDisco = !enableDisco;
                    break;
                case ConsoleKey.D:
                    if (keystroke.Modifiers == ConsoleModifiers.Control)
                    {
                        var drivers = DeviceTools.GetDrivers().Select((kvp) => new InputChoiceInfo(kvp.Key, kvp.Value)).ToArray();
                        int driverIdx = InfoBoxSelectionColor.WriteInfoBoxSelection(drivers, Translate.DoTranslation("Select a driver. ESC to quit."));
                        playerScreen.RequireRefresh();
                        if (driverIdx < 0)
                            return;
                        var driver = drivers[driverIdx];
                        string active = "";
                        var devices = DeviceTools.GetDevices(driver.ChoiceName, ref active).Select((kvp) => new InputChoiceInfo(kvp.Key, kvp.Value)).ToArray();
                        int deviceIdx = InfoBoxSelectionColor.WriteInfoBoxSelection(devices, Translate.DoTranslation("Select a device. Current driver is {0}. ESC to quit."), active);
                        playerScreen.RequireRefresh();
                        if (deviceIdx < 0)
                            return;
                        var device = devices[deviceIdx];
                        DeviceTools.SetActiveDriver(driver.ChoiceName);
                        DeviceTools.SetActiveDevice(driver.ChoiceName, device.ChoiceName);
                    }
                    else if (keystroke.Modifiers == ConsoleModifiers.Shift)
                        DeviceTools.Reset();
                    else
                        ShowDeviceDriver();
                    playerScreen.RequireRefresh();
                    break;
                case ConsoleKey.Q:
                    Exit();
                    break;
            }
        }
    }
}
