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

using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Modifications;
using Nitrocid.ScreensaverPacks.Screensavers;
using Nitrocid.ScreensaverPacks.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Nitrocid.ScreensaverPacks
{
    internal class ScreensaverPackInit : IAddon
    {
        internal static Dictionary<string, BaseScreensaver> Screensavers = new()
        {
            { "aberration", new AberrationDisplay() },
            { "analogclock", new AnalogClockDisplay() },
            { "aurora", new AuroraDisplay() },
            { "barrot", new BarRotDisplay() },
            { "barwave", new BarWaveDisplay() },
            { "beatfader", new BeatFaderDisplay() },
            { "beatpulse", new BeatPulseDisplay() },
            { "beatedgepulse", new BeatEdgePulseDisplay() },
            { "bigletter", new BigLetterDisplay() },
            { "bloom", new BloomDisplay() },
            { "bouncingblock", new BouncingBlockDisplay() },
            { "bouncingtext", new BouncingTextDisplay() },
            { "boxgrid", new BoxGridDisplay() },
            { "bsod", new BSODDisplay() },
            { "clochroma", new ClochromaDisplay() },
            { "colorbleed", new ColorBleedDisplay() },
            { "colormix", new ColorMixDisplay() },
            { "commitmilestone", new CommitMilestoneDisplay() },
            { "dancelines", new DanceLinesDisplay() },
            { "dancenumbers", new DanceNumbersDisplay() },
            { "dateandtime", new DateAndTimeDisplay() },
            { "diamond", new DiamondDisplay() },
            { "disco", new DiscoDisplay() },
            { "dissolve", new DissolveDisplay() },
            { "doorshift", new DoorShiftDisplay() },
            { "edgepulse", new EdgePulseDisplay() },
            { "equalizer", new EqualizerDisplay() },
            { "excalibeats", new ExcaliBeatsDisplay() },
            { "fader", new FaderDisplay() },
            { "faderback", new FaderBackDisplay() },
            { "fallingline", new FallingLineDisplay() },
            { "figlet", new FigletDisplay() },
            { "fillfade", new FillFadeDisplay() },
            { "fireworks", new FireworksDisplay() },
            { "flashcolor", new FlashColorDisplay() },
            { "flashtext", new FlashTextDisplay() },
            { "following", new FollowingDisplay() },
            { "glitch", new GlitchDisplay() },
            { "glitterchar", new GlitterCharDisplay() },
            { "glittercolor", new GlitterColorDisplay() },
            { "glittermatrix", new GlitterMatrixDisplay() },
            { "gradient", new GradientDisplay() },
            { "gradientbloom", new GradientBloomDisplay() },
            { "gradientrot", new GradientRotDisplay() },
            { "hueback", new HueBackDisplay() },
            { "huebackgradient", new HueBackGradientDisplay() },
            { "indeterminate", new IndeterminateDisplay() },
            { "ksx", new KSXDisplay() },
            { "ksx2", new KSX2Display() },
            { "ksx3", new KSX3Display() },
            { "ksxtheend", new KSXTheEndDisplay() },
            { "laserbeams", new LaserBeamsDisplay() },
            { "letterscatter", new LetterScatterDisplay() },
            { "lighter", new LighterDisplay() },
            { "lightning", new LightningDisplay() },
            { "lines", new LinesDisplay() },
            { "linotypo", new LinotypoDisplay() },
            { "marquee", new MarqueeDisplay() },
            { "matrix", new MatrixDisplay() },
            { "mazer", new MazerDisplay() },
            { "memdump", new MemdumpDisplay() },
            { "mesmerize", new MesmerizeDisplay() },
            { "multilines", new MultiLinesDisplay() },
            { "neons", new NeonsDisplay() },
            { "newyear", new NewYearDisplay() },
            { "noise", new NoiseDisplay() },
            { "numberscatter", new NumberScatterDisplay() },
            { "omen", new OmenDisplay() },
            { "particles", new ParticlesDisplay() },
            { "pointtrack", new PointTrackDisplay() },
            { "progressclock", new ProgressClockDisplay() },
            { "progresses", new ProgressesDisplay() },
            { "pulse", new PulseDisplay() },
            { "ramp", new RampDisplay() },
            { "simplematrix", new SimpleMatrixDisplay() },
            { "siren", new SirenDisplay() },
            { "sirentheme", new SirenThemeDisplay() },
            { "skycomet", new SkyCometDisplay() },
            { "snakefill", new SnakeFillDisplay() },
            { "speckles", new SpecklesDisplay() },
            { "spin", new SpinDisplay() },
            { "spotwrite", new SpotWriteDisplay() },
            { "spray", new SprayDisplay() },
            { "squarecorner", new SquareCornerDisplay() },
            { "stackbox", new StackBoxDisplay() },
            { "starfield", new StarfieldDisplay() },
            { "starfieldwarp", new StarfieldWarpDisplay() },
            { "swivel", new SwivelDisplay() },
            { "text", new TextDisplay() },
            { "textbox", new TextBoxDisplay() },
            { "textwander", new TextWanderDisplay() },
            { "trails", new TrailsDisplay() },
            { "twospins", new TwoSpinsDisplay() },
            { "typewriter", new TypewriterDisplay() },
            { "typo", new TypoDisplay() },
            { "wave", new WaveDisplay() },
            { "windowslogo", new WindowsLogoDisplay() },
            { "wipe", new WipeDisplay() },
            { "wordhasher", new WordHasherDisplay() },
            { "wordhasherwrite", new WordHasherWriteDisplay() },
            { "wordslot", new WordSlotDisplay() },
            { "worldclock", new WorldClockDisplay() },
            { "zebrashift", new ZebraShiftDisplay() },
        };

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.AddonScreensaverPacks);

        internal static ExtraSaversConfig SaversConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(ExtraSaversConfig)) ? (ExtraSaversConfig)Config.baseConfigurations[nameof(ExtraSaversConfig)] : new ExtraSaversConfig();

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

        void IAddon.StartAddon()
        {
            // First, initialize screensavers
            foreach (var saver in Screensavers.Keys)
                ScreensaverManager.AddonSavers.Add(saver, Screensavers[saver]);

            // Then, initialize configuration in a way that no mod can play with them
            var saversConfig = new ExtraSaversConfig();
            ConfigTools.RegisterBaseSetting(saversConfig);
        }

        void IAddon.StopAddon()
        {
            // First, unload screensavers
            foreach (var saver in Screensavers.Keys)
                ScreensaverManager.AddonSavers.Remove(saver);

            // Then, unload the configuration
            ConfigTools.UnregisterBaseSetting(nameof(ExtraSaversConfig));
        }

        void IAddon.FinalizeAddon()
        { }
    }
}
