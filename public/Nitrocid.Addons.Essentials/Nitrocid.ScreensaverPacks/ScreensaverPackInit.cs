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

using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Core.Languages;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.ScreensaverPacks.Screensavers;
using Nitrocid.ScreensaverPacks.Settings;
using System.Collections.Generic;

namespace Nitrocid.ScreensaverPacks
{
    internal class ScreensaverPackInit : IAddon
    {
        internal static Dictionary<string, BaseScreensaver> Screensavers = new()
        {
            { "aberration", new AberrationDisplay() },
            { "analogclock", new AnalogClockDisplay() },
            { "aurora", new AuroraDisplay() },
            { "backimplosion", new BackImplosionDisplay() },
            { "barrot", new BarRotDisplay() },
            { "barwave", new BarWaveDisplay() },
            { "beatfader", new BeatFaderDisplay() },
            { "beatpulse", new BeatPulseDisplay() },
            { "beatedgepulse", new BeatEdgePulseDisplay() },
            { "bigletter", new BigLetterDisplay() },
            { "blockclock", new BlockClockDisplay() },
            { "bloom", new BloomDisplay() },
            { "bouncingblock", new BouncingBlockDisplay() },
            { "bouncingtext", new BouncingTextDisplay() },
            { "boxgrid", new BoxGridDisplay() },
            { "bsod", new BSODDisplay() },
            { "clochroma", new ClochromaDisplay() },
            { "clochromaanalog", new ClochromaAnalogDisplay() },
            { "colorbleed", new ColorBleedDisplay() },
            { "colormix", new ColorMixDisplay() },
            { "colorspread", new ColorSpreadDisplay() },
            { "commitmilestone", new CommitMilestoneDisplay() },
            { "dancelines", new DanceLinesDisplay() },
            { "dancenumbers", new DanceNumbersDisplay() },
            { "dateandtime", new DateAndTimeDisplay() },
            { "diamond", new DiamondDisplay() },
            { "digitalclock", new DigitalClockDisplay() },
            { "disco", new DiscoDisplay() },
            { "dissolve", new DissolveDisplay() },
            { "doorshift", new DoorShiftDisplay() },
            { "edgepulse", new EdgePulseDisplay() },
            { "equalizer", new EqualizerDisplay() },
            { "evaporate", new EvaporateDisplay() },
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
            { "lettercycle", new LetterCycleDisplay() },
            { "letterscatter", new LetterScatterDisplay() },
            { "lighter", new LighterDisplay() },
            { "lightning", new LightningDisplay() },
            { "lines", new LinesDisplay() },
            { "lineup", new LineUpDisplay() },
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
            { "pi", new PiDisplay() },
            { "pointtrack", new PointTrackDisplay() },
            { "primus", new PrimusDisplay() },
            { "progressclock", new ProgressClockDisplay() },
            { "progresses", new ProgressesDisplay() },
            { "pulse", new PulseDisplay() },
            { "raining", new RainingDisplay() },
            { "ramp", new RampDisplay() },
            { "ripple", new RippleDisplay() },
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
            { "swivelreveal", new SwivelRevealDisplay() },
            { "text", new TextDisplay() },
            { "textbox", new TextBoxDisplay() },
            { "textwander", new TextWanderDisplay() },
            { "trails", new TrailsDisplay() },
            { "twospins", new TwoSpinsDisplay() },
            { "typewriter", new TypewriterDisplay() },
            { "typo", new TypoDisplay() },
            { "wave", new WaveDisplay() },
            { "waverun", new WaveRunDisplay() },
            { "windowslogo", new WindowsLogoDisplay() },
            { "wipe", new WipeDisplay() },
            { "wordhasher", new WordHasherDisplay() },
            { "wordhasherwrite", new WordHasherWriteDisplay() },
            { "wordslot", new WordSlotDisplay() },
            { "worldclock", new WorldClockDisplay() },
            { "zebrashift", new ZebraShiftDisplay() },
        };

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.AddonScreensaverPacks);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.AddonScreensaverPacks);

        internal static ExtraSaversConfig SaversConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(ExtraSaversConfig)) ? (ExtraSaversConfig)Config.baseConfigurations[nameof(ExtraSaversConfig)] : Config.GetFallbackKernelConfig<ExtraSaversConfig>();

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.ScreensaverPacks.Resources.Languages.Output.Localizations", typeof(ScreensaverPackInit).Assembly));

            // First, initialize screensavers
            foreach (var saver in Screensavers.Keys)
                ScreensaverManager.AddonSavers.Add(saver, Screensavers[saver]);

            // Then, initialize configuration in a way that no mod can play with them
            var saversConfig = new ExtraSaversConfig();
            ConfigTools.RegisterBaseSetting(saversConfig);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);

            // First, unload screensavers
            foreach (var saver in Screensavers.Keys)
                ScreensaverManager.AddonSavers.Remove(saver);

            // Then, unload the configuration
            ConfigTools.UnregisterBaseSetting(nameof(ExtraSaversConfig));
        }

        public void FinalizeAddon()
        { }
    }
}
