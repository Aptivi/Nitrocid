//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using BassBoom.Basolia.Enumerations;
using BassBoom.Basolia.File;
using BassBoom.Basolia.Format;
using BassBoom.Basolia.Playback;
using BassBoom.Basolia.Radio;
using Nitrocid.Extras.BassBoom.Player.Tools;
using Nitrocid.Languages;
using System.Linq;
using System.Text;
using System.Threading;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Textify.General;

namespace Nitrocid.Extras.BassBoom.Player
{
    internal static class RadioControls
    {
        internal static void Play()
        {
            // In case we have no stations in the playlist...
            if (Common.cachedInfos.Count == 0)
                return;

            // There could be a chance that the music has fully stopped without any user interaction, but since we're on
            // a radio station, we should seek nothing; just drop.
            if (PlaybackTools.State == PlaybackState.Stopped)
                PlaybackPositioningTools.Drop();
            Common.advance = true;
            Radio.playerThread.Start();
            SpinWait.SpinUntil(() => PlaybackTools.Playing || Common.failedToPlay);
            Common.failedToPlay = false;
        }

        internal static void Pause()
        {
            Common.advance = false;
            Common.paused = true;
            PlaybackTools.Pause();
        }

        internal static void Stop(bool resetCurrentStation = true)
        {
            Common.advance = false;
            Common.paused = false;
            if (resetCurrentStation)
                Common.currentPos = 1;
            PlaybackTools.Stop();
        }

        internal static void NextStation()
        {
            // In case we have no stations in the playlist...
            if (Common.cachedInfos.Count == 0)
                return;

            PlaybackTools.Stop();
            Common.currentPos++;
            if (Common.currentPos > Common.cachedInfos.Count)
                Common.currentPos = 1;
        }

        internal static void PreviousStation()
        {
            // In case we have no stations in the playlist...
            if (Common.cachedInfos.Count == 0)
                return;

            PlaybackTools.Stop();
            Common.currentPos--;
            if (Common.currentPos <= 0)
                Common.currentPos = Common.cachedInfos.Count;
        }

        internal static void PromptForAddStation()
        {
            string path = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a path to the radio station. The URL to the station must provide an MPEG radio station. AAC ones are not supported yet."));
            ScreenTools.CurrentScreen.RequireRefresh();
            Common.populate = true;
            PopulateRadioStationInfo(path);
            Common.populate = true;
            PopulateRadioStationInfo(Common.CurrentCachedInfo.MusicPath);
        }

        internal static void PopulateRadioStationInfo(string musicPath)
        {
            // Try to open the file after loading the library
            if (PlaybackTools.Playing || !Common.populate)
                return;
            Common.populate = false;
            Common.Switch(musicPath);
            if (!Common.cachedInfos.Any((csi) => csi.MusicPath == musicPath))
            {
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Loading BassBoom to open {0}..."), false, musicPath);
                var formatInfo = FormatTools.GetFormatInfo();
                var frameInfo = AudioInfoTools.GetFrameInfo();

                // Try to open the lyrics
                var instance = new CachedSongInfo(musicPath, null, null, -1, formatInfo, frameInfo, null, FileTools.CurrentFile.StationName, true);
                Common.cachedInfos.Add(instance);
            }
        }

        internal static string RenderStationName()
        {
            // Render the station name
            string icy = PlaybackTools.RadioNowPlaying;

            // Print the music name
            return
                TextWriterWhereColor.RenderWhere(ConsoleClearing.GetClearLineToRightSequence(), 0, 1) +
                CenteredTextColor.RenderCentered(1, Translate.DoTranslation("Now playing") + ": {0}", ConsoleColors.White, ConsoleColors.Black, 0, 0, icy);
        }

        internal static void RemoveCurrentStation()
        {
            // In case we have no stations in the playlist...
            if (Common.cachedInfos.Count == 0)
                return;

            Common.cachedInfos.RemoveAt(Common.currentPos - 1);
            if (Common.cachedInfos.Count > 0)
            {
                Common.currentPos--;
                if (Common.currentPos == 0)
                    Common.currentPos = 1;
                Common.populate = true;
                PopulateRadioStationInfo(Common.CurrentCachedInfo.MusicPath);
            }
        }

        internal static void RemoveAllStations()
        {
            // In case we have no stations in the playlist...
            if (Common.cachedInfos.Count == 0)
                return;

            for (int i = Common.cachedInfos.Count; i > 0; i--)
                RemoveCurrentStation();
        }

        internal static void ShowStationInfo()
        {
            string section1 = Translate.DoTranslation("Station info");
            string section2 = Translate.DoTranslation("Layer info");
            string section3 = Translate.DoTranslation("Native State");
            InfoBoxColor.WriteInfoBox(
                $$"""
                {{section1}}
                {{new string('=', ConsoleChar.EstimateCellWidth(section1))}}

                {{Translate.DoTranslation("Radio station URL")}}: {{Common.CurrentCachedInfo.MusicPath}}
                {{Translate.DoTranslation("Radio station name")}}: {{Common.CurrentCachedInfo.StationName}}
                {{Translate.DoTranslation("Radio station current song")}}: {{PlaybackTools.RadioNowPlaying}}
                
                {{section2}}
                {{new string('=', ConsoleChar.EstimateCellWidth(section2))}}
                
                {{Translate.DoTranslation("Version")}}: {{Common.CurrentCachedInfo.FrameInfo.Version}}
                {{Translate.DoTranslation("Layer")}}: {{Common.CurrentCachedInfo.FrameInfo.Layer}}
                {{Translate.DoTranslation("Rate")}}: {{Common.CurrentCachedInfo.FrameInfo.Rate}}
                {{Translate.DoTranslation("Mode")}}: {{Common.CurrentCachedInfo.FrameInfo.Mode}}
                {{Translate.DoTranslation("Mode Ext")}}: {{Common.CurrentCachedInfo.FrameInfo.ModeExt}}
                {{Translate.DoTranslation("Frame Size")}}: {{Common.CurrentCachedInfo.FrameInfo.FrameSize}}
                {{Translate.DoTranslation("Flags")}}: {{Common.CurrentCachedInfo.FrameInfo.Flags}}
                {{Translate.DoTranslation("Emphasis")}}: {{Common.CurrentCachedInfo.FrameInfo.Emphasis}}
                {{Translate.DoTranslation("Bitrate")}}: {{Common.CurrentCachedInfo.FrameInfo.BitRate}}
                {{Translate.DoTranslation("ABR Rate")}}: {{Common.CurrentCachedInfo.FrameInfo.AbrRate}}
                {{Translate.DoTranslation("Variable bitrate")}}: {{Common.CurrentCachedInfo.FrameInfo.Vbr}}
                
                {{section3}}
                {{new string('=', ConsoleChar.EstimateCellWidth(section3))}}
                
                {{Translate.DoTranslation("Accurate rendering")}}: {{PlaybackTools.GetNativeState(PlaybackStateType.Accurate)}}
                {{Translate.DoTranslation("Buffer fill")}}: {{PlaybackTools.GetNativeState(PlaybackStateType.BufferFill)}}
                {{Translate.DoTranslation("Decoding delay")}}: {{PlaybackTools.GetNativeState(PlaybackStateType.DecodeDelay)}}
                {{Translate.DoTranslation("Encoding delay")}}: {{PlaybackTools.GetNativeState(PlaybackStateType.EncodeDelay)}}
                {{Translate.DoTranslation("Encoding padding")}}: {{PlaybackTools.GetNativeState(PlaybackStateType.EncodePadding)}}
                {{Translate.DoTranslation("Frankenstein stream")}}: {{PlaybackTools.GetNativeState(PlaybackStateType.Frankenstein)}}
                {{Translate.DoTranslation("Fresh decoder")}}: {{PlaybackTools.GetNativeState(PlaybackStateType.FreshDecoder)}}
                """
            );
        }

        internal static void ShowExtendedStationInfo()
        {
            var station = RadioTools.GetRadioInfo(Common.CurrentCachedInfo.MusicPath);
            var streamBuilder = new StringBuilder();
            foreach (var stream in station.Streams)
            {
                streamBuilder.AppendLine($"{Translate.DoTranslation("Name")}: {stream.StreamTitle}");
                streamBuilder.AppendLine($"{Translate.DoTranslation("Home page")}: {stream.StreamHomepage}");
                streamBuilder.AppendLine($"{Translate.DoTranslation("Genre")}: {stream.StreamGenre}");
                streamBuilder.AppendLine($"{Translate.DoTranslation("Now playing")}: {stream.SongTitle}");
                streamBuilder.AppendLine($"{Translate.DoTranslation("Stream path")}: {stream.StreamPath}");
                streamBuilder.AppendLine(Translate.DoTranslation("Listeners: {0} with {1} at peak").FormatString(stream.CurrentListeners, stream.PeakListeners));
                streamBuilder.AppendLine($"{Translate.DoTranslation("Bit rate")}: {stream.BitRate} kbps");
                streamBuilder.AppendLine($"{Translate.DoTranslation("Media type")}: {stream.MimeInfo}");
                streamBuilder.AppendLine("===============================");
            }
            string section1 = Translate.DoTranslation("Radio server info");
            string section2 = Translate.DoTranslation("Stream info");
            InfoBoxColor.WriteInfoBox(
                $$"""
                {{section1}}
                {{new string('=', ConsoleChar.EstimateCellWidth(section1))}}

                {{Translate.DoTranslation("Radio station URL")}}: {{station.ServerHostFull}}
                {{Translate.DoTranslation("Radio station uses HTTPS")}}: {{station.ServerHttps}}
                {{Translate.DoTranslation("Radio station server type")}}: {{station.ServerType}}
                {{Translate.DoTranslation("Radio station streams: {0} with {1} active").FormatString(station.TotalStreams, station.ActiveStreams)}}
                {{Translate.DoTranslation("Radio station listeners: {0} with {1} at peak").FormatString(station.CurrentListeners, station.PeakListeners)}}
                
                {{section2}}
                {{new string('=', ConsoleChar.EstimateCellWidth(section2))}}

                ===============================
                {{streamBuilder}}
                """
            );
        }
    }
}
