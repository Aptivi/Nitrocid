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

using System.Linq;
using Nitrocid.Base.Misc.Audio;
using Terminaux.Inputs;
using Terminaux.Reader;

namespace Nitrocid.Base.Kernel.Configuration.Instances
{
    /// <summary>
    /// Main kernel configuration instance
    /// </summary>
    public partial class KernelMainConfig : BaseKernelConfig
    {
        private string audioCueThemeName = "the_mirage";
        private bool enableAudio = true;

        /// <summary>
        /// Enables the whole audio system for system cues
        /// </summary>
        public bool EnableAudio
        {
            get => enableAudio;
            set
            {
                enableAudio = value;
                if (!value)
                {
                    EnableKeyboardCues = value;
                    EnableStartupSounds = value;
                    EnableShutdownSounds = value;
                    EnableNavigationSounds = value;
                    EnableLowPriorityNotificationSounds = value;
                    EnableMediumPriorityNotificationSounds = value;
                    EnableHighPriorityNotificationSounds = value;
                    EnableAmbientSoundFx = value;
                }
            }
        }
        /// <summary>
        /// Whether to play keyboard cues for each keypress or not (<see cref="EnableNavigationSounds"/> must be enabled for this to take effect)
        /// </summary>
        public bool EnableKeyboardCues
        {
            get => TermReader.GlobalReaderSettings.KeyboardCues;
            set => TermReader.GlobalReaderSettings.KeyboardCues = value;
        }
        /// <summary>
        /// Whether to enable startup sounds or not
        /// </summary>
        public bool EnableStartupSounds { get; set; } = true;
        /// <summary>
        /// Whether to enable shutdown sounds or not
        /// </summary>
        public bool EnableShutdownSounds { get; set; } = true;
        /// <summary>
        /// Whether to enable navigation sounds or not
        /// </summary>
        public bool EnableNavigationSounds
        {
            get => Input.KeyboardCues;
            set => Input.KeyboardCues = value;
        }
        /// <summary>
        /// Whether to enable the notification sound for low-priority alerts or not
        /// </summary>
        public bool EnableLowPriorityNotificationSounds { get; set; } = true;
        /// <summary>
        /// Whether to enable the notification sound for medium-priority alerts or not
        /// </summary>
        public bool EnableMediumPriorityNotificationSounds { get; set; } = true;
        /// <summary>
        /// Whether to enable the notification sound for high-priority alerts or not
        /// </summary>
        public bool EnableHighPriorityNotificationSounds { get; set; } = true;
        /// <summary>
        /// Whether to play ambient screensaver sound effects or not
        /// </summary>
        public bool EnableAmbientSoundFx { get; set; }
        /// <summary>
        /// Selects the intensity of the ambient sound effect
        /// </summary>
        public AmbienceFxIntensity AmbientSoundFxIntensity { get; set; } = AmbienceFxIntensity.Medium;
        /// <summary>
        /// Audio cue volume
        /// </summary>
        public double AudioCueVolume
        {
            get => Input.CueVolume;
            set => Input.CueVolume = value;
        }
        /// <summary>
        /// Audio cue volume for the reader
        /// </summary>
        public double AudioCueVolumeReader
        {
            get => TermReader.GlobalReaderSettings.CueVolume;
            set => TermReader.GlobalReaderSettings.CueVolume = value;
        }
        /// <summary>
        /// Audio cue theme name
        /// </summary>
        public string AudioCueThemeName
        {
            get => audioCueThemeName;
            set
            {
                audioCueThemeName = AudioCuesTools.GetAudioThemeNames().Contains(value) ? value : "the_mirage";
                var cue = AudioCuesTools.GetAudioCue();
                TermReader.GlobalReaderSettings.CueWrite = cue.KeyboardCueTypeStream ?? TermReader.GlobalReaderSettings.CueWrite;
                TermReader.GlobalReaderSettings.CueRubout = cue.KeyboardCueBackspaceStream ?? TermReader.GlobalReaderSettings.CueRubout;
                TermReader.GlobalReaderSettings.CueEnter = cue.KeyboardCueEnterStream ?? TermReader.GlobalReaderSettings.CueEnter;
                Input.CueWrite = cue.KeyboardCueTypeStream ?? Input.CueWrite;
                Input.CueRubout = cue.KeyboardCueBackspaceStream ?? Input.CueRubout;
                Input.CueEnter = cue.KeyboardCueEnterStream ?? Input.CueEnter;
            }
        }
    }
}
