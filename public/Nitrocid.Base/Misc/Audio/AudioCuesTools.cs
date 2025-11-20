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

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using BassBoom.Basolia.Independent;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Threading;

namespace Nitrocid.Base.Misc.Audio
{
    /// <summary>
    /// Audio cues tools
    /// </summary>
    public static class AudioCuesTools
    {
        private static readonly Dictionary<string, AudioCueContainer> cueThemes = PopulateCues();
        private static bool isThemeMusicPlaying = false;
        private static readonly KernelThread themeMusicThread = new("Theme Music Thread", true, HandleThemeMusic);

        /// <summary>
        /// Gets the audio theme names
        /// </summary>
        /// <returns>An array of available audio cues</returns>
        public static string[] GetAudioThemeNames() =>
            [.. cueThemes.Keys];

        /// <summary>
        /// Gets an audio cue from the current audio cue theme name
        /// </summary>
        /// <returns>A container for all audio cues for the current cue theme, or all audio cues from the default if not found</returns>
        public static AudioCueContainer GetAudioCue() =>
            GetAudioCue(Config.MainConfig.AudioCueThemeName);

        /// <summary>
        /// Gets an audio cue from the name
        /// </summary>
        /// <param name="name">Audio cue name</param>
        /// <returns>A container for all audio cues for the specified cue theme, or all audio cues from the default if not found</returns>
        public static AudioCueContainer GetAudioCue(string name)
        {
            if (cueThemes.TryGetValue(name, out var container))
                return container;
            return cueThemes["the_mirage"];
        }

        /// <summary>
        /// Plays the audio cue
        /// </summary>
        /// <param name="cueType">Cue type</param>
        /// <param name="async">Whether the audio plays in a non-blocking or a blocking way</param>
        public static void PlayAudioCue(AudioCueType cueType, bool async = true) =>
            PlayAudioCue(cueType, GetAudioCue(), async);

        /// <summary>
        /// Plays the audio cue
        /// </summary>
        /// <param name="cueType">Cue type</param>
        /// <param name="cueName">Audio cue theme name</param>
        /// <param name="async">Whether the audio plays in a non-blocking or a blocking way</param>
        public static void PlayAudioCue(AudioCueType cueType, string cueName, bool async = true) =>
            PlayAudioCue(cueType, GetAudioCue(cueName), async);

        /// <summary>
        /// Plays the audio cue
        /// </summary>
        /// <param name="cueType">Cue type</param>
        /// <param name="cueContainer">Audio cue container instance</param>
        /// <param name="async">Whether the audio plays in a non-blocking or a blocking way</param>
        public static void PlayAudioCue(AudioCueType cueType, AudioCueContainer cueContainer, bool async = true)
        {
            if (!Config.MainConfig.EnableAudio)
                return;
            try
            {
                // Get the audio cue stream
                var stream = cueContainer.GetStream(cueType);
                if (stream == null)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, $"There is no audio cue for {cueType} on {cueContainer.Name}. Ignoring...");
                    return;
                }
                DebugWriter.WriteDebug(DebugLevel.I, $"Stream of {cueType} on {cueContainer.Name} is {stream.Length} bytes.");

                // Then, seek this stream to the beginning and play it using the play-n-forget technique
                var settings = new PlayForgetSettings()
                {
                    Volume = Config.MainConfig.AudioCueVolume,
                };
                stream.Seek(0, SeekOrigin.Begin);
                if (async)
                    PlayForget.PlayStreamAsync(stream, settings);
                else
                    PlayForget.PlayStream(stream, settings);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Audio cue playing failed: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.W, "Turning off audio cue support...");
                Config.MainConfig.EnableAudio = false;
            }
        }

        /// <summary>
        /// Maps the ambience intensity to the appropriate cue type
        /// </summary>
        /// <param name="intensity">Intensity of the ambient sound</param>
        /// <returns>Cue type that represents the selected ambience intensity</returns>
        public static AudioCueType MapAmbienceToCueType(AmbienceFxIntensity intensity) =>
            intensity switch
            {
                AmbienceFxIntensity.Calm => AudioCueType.AmbienceIdle,
                AmbienceFxIntensity.Normal => AudioCueType.AlarmIdle,
                AmbienceFxIntensity.Medium => AudioCueType.Ambience,
                AmbienceFxIntensity.Intense => AudioCueType.Alarm,
                _ => AudioCueType.AmbienceIdle
            };

        /// <summary>
        /// Maps the cue type to the appropriate ambience intensity
        /// </summary>
        /// <param name="cueType">Cue type that represents the selected ambience intensity</param>
        /// <returns>Intensity of the ambient sound</returns>
        public static AmbienceFxIntensity MapCueTypeToAmbience(AudioCueType cueType) =>
            cueType switch
            {
                AudioCueType.AmbienceIdle => AmbienceFxIntensity.Calm,
                AudioCueType.AlarmIdle => AmbienceFxIntensity.Normal,
                AudioCueType.Ambience => AmbienceFxIntensity.Medium,
                AudioCueType.Alarm => AmbienceFxIntensity.Intense,
                _ => AmbienceFxIntensity.Calm
            };

        internal static void PlayThemeMusic()
        {
            if (!isThemeMusicPlaying)
            {
                if (themeMusicThread.BaseThread.ThreadState == ThreadState.Stopped)
                    themeMusicThread.Regen();
                themeMusicThread.Start();
            }
        }

        private static Dictionary<string, AudioCueContainer> PopulateCues()
        {
            Dictionary<string, string> cueDescriptors = new()
            {
                { "the_mirage", "The Mirage" },
                { "big_loss", "Big Loss" },
                { "great_moments", "Great Moments" },
                { "thousands_nights", "00's Nights" },
                { "the_night", "The Night" },
                { "the_mirage_urban", "The Mirage (Urban)" },
            };
            Dictionary<string, AudioCueContainer> containers = [];

            // Populate the cue containers
            foreach (string descriptor in cueDescriptors.Keys)
            {
                DebugWriter.WriteDebug(DebugLevel.I, $"Adding cues for descriptor {descriptor} [{cueDescriptors[descriptor]}] from the resources...");
                containers.Add(descriptor, new AudioCueContainer(descriptor, cueDescriptors[descriptor]));
            }
            return containers;
        }

        private static void HandleThemeMusic()
        {
            try
            {
                if (isThemeMusicPlaying)
                    return;
                isThemeMusicPlaying = true;
                PlayAudioCue(AudioCueType.Full, false);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Error trying to play theme music: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
            }
            finally
            {
                isThemeMusicPlaying = false;
            }
        }
    }
}
