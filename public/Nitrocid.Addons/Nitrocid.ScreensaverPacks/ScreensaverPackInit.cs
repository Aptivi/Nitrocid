//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Kernel.Configuration;
using KS.Kernel.Extensions;
using KS.Misc.Screensaver;
using KS.Misc.Splash;
using Nitrocid.ScreensaverPacks.Screensavers;
using Nitrocid.ScreensaverPacks.Settings;
using Nitrocid.ScreensaverPacks.Splashes;
using System.Collections.Generic;

namespace Nitrocid.ScreensaverPacks
{
    internal class ScreensaverPackInit : IAddon
    {
        internal static Dictionary<string, BaseScreensaver> Screensavers = new()
        {
            { "aurora", new AuroraDisplay() },
            { "barrot", new BarRotDisplay() },
            { "barwave", new BarWaveDisplay() },
            { "beatfader", new BeatFaderDisplay() },
            { "beatpulse", new BeatPulseDisplay() },
            { "beatedgepulse", new BeatEdgePulseDisplay() },
            { "bloom", new BloomDisplay() },
            { "bouncingblock", new BouncingBlockDisplay() },
            { "bouncingtext", new BouncingTextDisplay() },
            { "boxgrid", new BoxGridDisplay() },
            { "bsod", new BSODDisplay() },
            { "colorbleed", new ColorBleedDisplay() },
            { "colormix", new ColorMixDisplay() },
            { "dancelines", new DanceLinesDisplay() },
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
            { "glitch", new GlitchDisplay() },
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
            { "laserbeams", new LaserBeamsDisplay() },
            { "letterscatter", new LetterScatterDisplay() },
            { "lighter", new LighterDisplay() },
            { "lightning", new LightningDisplay() },
            { "lines", new LinesDisplay() },
            { "linotypo", new LinotypoDisplay() },
            { "marquee", new MarqueeDisplay() },
            { "mazer", new MazerDisplay() },
            { "memdump", new MemdumpDisplay() },
            { "mesmerize", new MesmerizeDisplay() },
            { "multilines", new MultiLinesDisplay() },
            { "newyear", new NewYearDisplay() },
            { "noise", new NoiseDisplay() },
            { "numberscatter", new NumberScatterDisplay() },
            { "particles", new ParticlesDisplay() },
            { "progressclock", new ProgressClockDisplay() },
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
            { "squarecorner", new SquareCornerDisplay() },
            { "stackbox", new StackBoxDisplay() },
            { "starfield", new StarfieldDisplay() },
            { "starfieldwarp", new StarfieldWarpDisplay() },
            { "swivel", new SwivelDisplay() },
            { "text", new TextDisplay() },
            { "textbox", new TextBoxDisplay() },
            { "textwander", new TextWanderDisplay() },
            { "typewriter", new TypewriterDisplay() },
            { "typo", new TypoDisplay() },
            { "wave", new WaveDisplay() },
            { "windowslogo", new WindowsLogoDisplay() },
            { "wipe", new WipeDisplay() },
            { "wordhasher", new WordHasherDisplay() },
            { "wordhasherwrite", new WordHasherWriteDisplay() },
            { "worldclock", new WorldClockDisplay() },
        };

        internal readonly static Dictionary<string, SplashInfo> Splashes = new()
        {
            { "Fader", new SplashInfo("Fader", new SplashFader()) },
            { "FaderBack", new SplashInfo("FaderBack", new SplashFaderBack()) },
            { "BeatFader", new SplashInfo("BeatFader", new SplashBeatFader()) },
            { "Pulse", new SplashInfo("Pulse", new SplashPulse()) },
            { "BeatPulse", new SplashInfo("BeatPulse", new SplashBeatPulse()) },
            { "EdgePulse", new SplashInfo("EdgePulse", new SplashEdgePulse()) },
            { "BeatEdgePulse", new SplashInfo("BeatEdgePulse", new SplashBeatEdgePulse()) },
            { "Spin", new SplashInfo("Spin", new SplashSpin()) },
            { "SquareCorner", new SplashInfo("SquareCorner", new SplashSquareCorner()) },
        };

        string IAddon.AddonName => "Extra Screensavers Pack";

        AddonType IAddon.AddonType => AddonType.Important;

        internal static ExtraSaversConfig SaversConfig =>
            (ExtraSaversConfig)Config.baseConfigurations[nameof(ExtraSaversConfig)];

        void IAddon.StartAddon()
        {
            // First, initialize screensavers and splashes
            foreach (var saver in Screensavers.Keys)
                ScreensaverManager.AddonSavers.Add(saver, Screensavers[saver]);
            foreach (var splash in Splashes.Keys)
                SplashManager.InstalledSplashes.Add(splash, Splashes[splash]);

            // Then, initialize configuration in a way that no mod can play with them
            var saversConfig = new ExtraSaversConfig();
            ConfigTools.RegisterBaseSetting(saversConfig);
        }

        void IAddon.StopAddon()
        {
            // First, unload screensavers and splashes
            foreach (var saver in Screensavers.Keys)
                ScreensaverManager.AddonSavers.Remove(saver);
            foreach (var splash in Splashes.Keys)
                SplashManager.InstalledSplashes.Remove(splash);

            // Then, unload the configuration
            ConfigTools.UnregisterBaseSetting(nameof(ExtraSaversConfig));
        }

        void IAddon.FinalizeAddon()
        { }
    }
}
