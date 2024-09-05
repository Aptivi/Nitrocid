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

using BassBoom.Basolia.Format;
using BassBoom.Basolia.Lyrics;
using System;

namespace Nitrocid.Extras.BassBoom.Player.Tools
{
    /// <summary>
    /// Cached song info
    /// </summary>
    internal class CachedSongInfo
    {
        /// <summary>
        /// A full path to the music file
        /// </summary>
        public string MusicPath { get; private set; }
        /// <summary>
        /// ID3v1 metadata
        /// </summary>
        public Id3V1Metadata? MetadataV1 { get; private set; }
        /// <summary>
        /// ID3v2 metadata
        /// </summary>
        public Id3V2Metadata? MetadataV2 { get; private set; }
        /// <summary>
        /// Radio station name
        /// </summary>
        public string StationName { get; private set; }
        /// <summary>
        /// Music duration in samples
        /// </summary>
        public int Duration { get; private set; }
        /// <summary>
        /// Music duration in a string representation of the time span
        /// </summary>
        public string DurationSpan =>
            AudioInfoTools.GetDurationSpanFromSamples(Duration, FormatInfo.rate).ToString();
        /// <summary>
        /// Format information (rate, channels, and encoding)
        /// </summary>
        public (long rate, int channels, int encoding) FormatInfo { get; private set; }
        /// <summary>
        /// MPEG frame info
        /// </summary>
        public FrameInfo FrameInfo { get; private set; }
        /// <summary>
        /// An instance of the music lyrics (if any)
        /// </summary>
        public Lyric? LyricInstance { get; private set; }
        /// <summary>
        /// Checks to see if this cached song info instance is a radio station or not
        /// </summary>
        public bool IsRadio { get; private set; }
        /// <summary>
        /// Repeat checkpoint (not for radio stations)
        /// </summary>
        public TimeSpan RepeatCheckpoint { get; internal set; } = new();

        /// <summary>
        /// A cached song information
        /// </summary>
        /// <param name="musicPath">A full path to the music file</param>
        /// <param name="metadataV1">ID3v1 metadata</param>
        /// <param name="metadataV2">ID3v2 metadata</param>
        /// <param name="duration">Music duration in samples</param>
        /// <param name="formatInfo">Format information (rate, channels, and encoding)</param>
        /// <param name="frameInfo">MPEG frame info</param>
        /// <param name="lyricInstance">An instance of the music lyrics (if any)</param>
        /// <param name="stationName">Radio station name</param>
        /// <param name="isRadioStation">Is this cached song info instance is a radio station or not?</param>
        public CachedSongInfo(string musicPath, Id3V1Metadata? metadataV1, Id3V2Metadata? metadataV2, int duration, (long rate, int channels, int encoding) formatInfo, FrameInfo frameInfo, Lyric? lyricInstance, string stationName, bool isRadioStation)
        {
            MusicPath = musicPath;
            MetadataV1 = metadataV1;
            MetadataV2 = metadataV2;
            Duration = duration;
            FormatInfo = formatInfo;
            FrameInfo = frameInfo;
            LyricInstance = lyricInstance;
            StationName = stationName;
            IsRadio = isRadioStation;
        }
    }
}
