
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

#if !NETCOREAPP
using System;
using System.Diagnostics;
using System.Globalization;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Files.Folders;
using KS.Kernel;
using KS.Languages;
using KS.Shell.Shells.Text;
using KS.Misc.Screensaver;
using KS.Misc.Screensaver.Displays;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network;
using KS.Network.Mail.Directory;
using KS.Network.RPC;
using KS.Network.SSH;
using KS.Shell;
using MadMilkman.Ini;
using ManagedWeatherMap.Core;
using ColorSeq;
using KS.Kernel.Debugging;
using KS.Kernel.Debugging.RemoteDebug;

namespace KSConverter
{
    static class FivePointFive
    {
        /// <summary>
        /// Takes configuration values and installs them to appropriate variables.
        /// </summary>
        /// <param name="PathToConfig">Path to 0.0.5.5+ config (kernelConfig.ini)</param>
        public static bool ReadFivePointFiveConfig(string PathToConfig)
        {
            try
            {
                var ConfigReader = new IniFile();
                var ValidFormat = default(bool);
                Debug.WriteLine("Reading post-0.0.5.5 config...");
                ConfigReader.Load(PathToConfig);

                // Check for sections
                if (ConfigReader.Sections.Contains("General") & ConfigReader.Sections.Contains("Colors") & ConfigReader.Sections.Contains("Hardware") & ConfigReader.Sections.Contains("Login") & ConfigReader.Sections.Contains("Shell") & ConfigReader.Sections.Contains("Misc"))
                {
                    Debug.WriteLine("Valid config!");
                    ValidFormat = true;
                }

                // Now, install the values - General section
                if (ConfigReader.Sections["General"].Keys.Contains("Maintenance Mode"))
                {
                    if (ConfigReader.Sections["General"].Keys["Maintenance Mode"].Value == "True")
                        Flags.Maintenance = true;
                    else
                        Flags.Maintenance = false;
                }
                if (ConfigReader.Sections["General"].Keys.Contains("Prompt for Arguments on Boot"))
                {
                    if (ConfigReader.Sections["General"].Keys["Prompt for Arguments on Boot"].Value == "True")
                        Flags.ArgsOnBoot = true;
                    else
                        Flags.ArgsOnBoot = false;
                }
                if (ConfigReader.Sections["General"].Keys.Contains("Check for Updates on Startup"))
                {
                    if (ConfigReader.Sections["General"].Keys["Check for Updates on Startup"].Value == "True")
                        Flags.CheckUpdateStart = true;
                    else
                        Flags.CheckUpdateStart = false;
                }
                if (ConfigReader.Sections["General"].Keys.Contains("Change Culture when Switching Languages"))
                {
                    if (ConfigReader.Sections["General"].Keys["Change Culture when Switching Languages"].Value == "True")
                        Flags.LangChangeCulture = true;
                    else
                        Flags.LangChangeCulture = false;
                }
                if (ConfigReader.Sections["General"].Keys.Contains("Language"))
                {
                    string ConfiguredLang = ConfigReader.Sections["General"].Keys["Language"].Value;
                    LanguageManager.SetLang(string.IsNullOrWhiteSpace(ConfiguredLang) ? "eng" : ConfiguredLang);
                }
                if (ConfigReader.Sections["General"].Keys.Contains("Culture"))
                {
                    if (Flags.LangChangeCulture)
                        CultureManager.CurrentCult = new CultureInfo(ConfigReader.Sections["General"].Keys["Culture"].Value);
                }

                // Colors section
                // We use New Color() to parse entered color. This is to ensure that the kernel can use the correct VT sequence.
                if (ConfigReader.Sections["Colors"].Keys.Contains("User Name Shell Color"))
                    ColorTools.SetColor(ColorTools.ColTypes.UserNameShell, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), ConfigReader.Sections["Colors"].Keys["User Name Shell Color"].Value))));
                if (ConfigReader.Sections["Colors"].Keys.Contains("Host Name Shell Color"))
                    ColorTools.SetColor(ColorTools.ColTypes.HostNameShell, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), ConfigReader.Sections["Colors"].Keys["Host Name Shell Color"].Value))));
                if (ConfigReader.Sections["Colors"].Keys.Contains("Continuable Kernel Error Color"))
                    ColorTools.SetColor(ColorTools.ColTypes.ContKernelError, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), ConfigReader.Sections["Colors"].Keys["Continuable Kernel Error Color"].Value))));
                if (ConfigReader.Sections["Colors"].Keys.Contains("Uncontinuable Kernel Error Color"))
                    ColorTools.SetColor(ColorTools.ColTypes.UncontKernelError, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), ConfigReader.Sections["Colors"].Keys["Uncontinuable Kernel Error Color"].Value))));
                if (ConfigReader.Sections["Colors"].Keys.Contains("Text Color"))
                    ColorTools.SetColor(ColorTools.ColTypes.NeutralText, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), ConfigReader.Sections["Colors"].Keys["Text Color"].Value))));
                if (ConfigReader.Sections["Colors"].Keys.Contains("License Color"))
                    ColorTools.SetColor(ColorTools.ColTypes.License, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), ConfigReader.Sections["Colors"].Keys["License Color"].Value))));
                if (ConfigReader.Sections["Colors"].Keys.Contains("Background Color"))
                    ColorTools.SetColor(ColorTools.ColTypes.Background, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), ConfigReader.Sections["Colors"].Keys["Background Color"].Value))));
                if (ConfigReader.Sections["Colors"].Keys.Contains("Input Color"))
                    ColorTools.SetColor(ColorTools.ColTypes.Input, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), ConfigReader.Sections["Colors"].Keys["Input Color"].Value))));
                if (ConfigReader.Sections["Colors"].Keys.Contains("List Entry Color"))
                    ColorTools.SetColor(ColorTools.ColTypes.ListEntry, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), ConfigReader.Sections["Colors"].Keys["List Entry Color"].Value))));
                if (ConfigReader.Sections["Colors"].Keys.Contains("List Value Color"))
                    ColorTools.SetColor(ColorTools.ColTypes.ListValue, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), ConfigReader.Sections["Colors"].Keys["List Value Color"].Value))));
                if (ConfigReader.Sections["Colors"].Keys.Contains("Kernel Stage Color"))
                    ColorTools.SetColor(ColorTools.ColTypes.Stage, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), ConfigReader.Sections["Colors"].Keys["Kernel Stage Color"].Value))));
                if (ConfigReader.Sections["Colors"].Keys.Contains("Error Text Color"))
                    ColorTools.SetColor(ColorTools.ColTypes.Error, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), ConfigReader.Sections["Colors"].Keys["Error Text Color"].Value))));
                if (ConfigReader.Sections["Colors"].Keys.Contains("Warning Text Color"))
                    ColorTools.SetColor(ColorTools.ColTypes.Warning, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), ConfigReader.Sections["Colors"].Keys["Warning Text Color"].Value))));
                if (ConfigReader.Sections["Colors"].Keys.Contains("Option Color"))
                    ColorTools.SetColor(ColorTools.ColTypes.Option, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), ConfigReader.Sections["Colors"].Keys["Option Color"].Value))));
                if (ConfigReader.Sections["Colors"].Keys.Contains("Banner Color"))
                    ColorTools.SetColor(ColorTools.ColTypes.Banner, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), ConfigReader.Sections["Colors"].Keys["Banner Color"].Value))));

                // Login section
                if (ConfigReader.Sections["Login"].Keys.Contains("Clear Screen on Log-in"))
                {
                    if (ConfigReader.Sections["Login"].Keys["Clear Screen on Log-in"].Value == "True")
                        Flags.ClearOnLogin = true;
                    else
                        Flags.ClearOnLogin = false;
                }
                if (ConfigReader.Sections["Login"].Keys.Contains("Show MOTD on Log-in"))
                {
                    if (ConfigReader.Sections["Login"].Keys["Show MOTD on Log-in"].Value == "True")
                        Flags.ShowMOTD = true;
                    else
                        Flags.ShowMOTD = false;
                }
                if (ConfigReader.Sections["Login"].Keys.Contains("Show available usernames"))
                {
                    if (ConfigReader.Sections["Login"].Keys["Show available usernames"].Value == "True")
                        Flags.ShowAvailableUsers = true;
                    else
                        Flags.ShowAvailableUsers = false;
                }
                if (ConfigReader.Sections["Login"].Keys.Contains("Host Name"))
                {
                    if (!string.IsNullOrEmpty(ConfigReader.Sections["Login"].Keys["Host Name"].Value))
                    {
                        NetworkTools.HostName = ConfigReader.Sections["Login"].Keys["Host Name"].Value;
                    }
                    else
                    {
                        NetworkTools.HostName = "kernel";
                    }
                }

                // Shell section
                if (ConfigReader.Sections["Shell"].Keys.Contains("Simplified Help Command"))
                {
                    if (ConfigReader.Sections["Shell"].Keys["Simplified Help Command"].Value == "True")
                        Flags.SimHelp = true;
                    else
                        Flags.SimHelp = false;
                }
                if (ConfigReader.Sections["Shell"].Keys.Contains("Current Directory"))
                {
                    CurrentDirectory.CurrentDir = ConfigReader.Sections["Shell"].Keys["Current Directory"].Value;
                }
                if (ConfigReader.Sections["Shell"].Keys.Contains("Lookup Directories"))
                {
                    Shell.PathsToLookup = ConfigReader.Sections["Shell"].Keys["Lookup Directories"].Value.ReleaseDoubleQuotes();
                }

                // Hardware section
                if (ConfigReader.Sections["Hardware"].Keys.Contains("Quiet Probe"))
                {
                    if (ConfigReader.Sections["Hardware"].Keys["Quiet Probe"].Value == "True")
                        Flags.QuietHardwareProbe = true;
                    else
                        Flags.QuietHardwareProbe = false;
                }
                if (ConfigReader.Sections["Hardware"].Keys.Contains("Full Probe"))
                {
                    if (ConfigReader.Sections["Hardware"].Keys["Full Probe"].Value == "True")
                        Flags.FullHardwareProbe = true;
                    else
                        Flags.FullHardwareProbe = false;
                }

                // Network section
                if (ConfigReader.Sections["Network"]?.Keys?.Contains("Debug Port") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Network"].Keys["Debug Port"].Value, out int argresult))
                        RemoteDebugger.DebugPort = Convert.ToInt32(ConfigReader.Sections["Network"].Keys["Debug Port"].Value);
                }
                if (ConfigReader.Sections["Network"]?.Keys?.Contains("Download Retry Times") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Network"].Keys["Download Retry Times"].Value, out int argresult1))
                        NetworkTools.DownloadRetries = Convert.ToInt32(ConfigReader.Sections["Network"].Keys["Download Retry Times"].Value);
                }
                if (ConfigReader.Sections["Network"]?.Keys?.Contains("Upload Retry Times") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Network"].Keys["Upload Retry Times"].Value, out int argresult2))
                        NetworkTools.UploadRetries = Convert.ToInt32(ConfigReader.Sections["Network"].Keys["Upload Retry Times"].Value);
                }
                if (ConfigReader.Sections["Network"]?.Keys?.Contains("Show progress bar while downloading or uploading from \"get\" or \"put\" command") == true)
                {
                    Flags.ShowProgress = Convert.ToBoolean(ConfigReader.Sections["Network"].Keys["Show progress bar while downloading or uploading from \"get\" or \"put\" command"].Value);
                }
                if (ConfigReader.Sections["Network"]?.Keys?.Contains("Log FTP username") == true)
                {
                    Flags.FTPLoggerUsername = Convert.ToBoolean(ConfigReader.Sections["Network"].Keys["Log FTP username"].Value);
                }
                if (ConfigReader.Sections["Network"]?.Keys?.Contains("Log FTP IP address") == true)
                {
                    Flags.FTPLoggerIP = Convert.ToBoolean(ConfigReader.Sections["Network"].Keys["Log FTP IP address"].Value);
                }
                if (ConfigReader.Sections["Network"]?.Keys?.Contains("Return only first FTP profile") == true)
                {
                    Flags.FTPFirstProfileOnly = Convert.ToBoolean(ConfigReader.Sections["Network"].Keys["Return only first FTP profile"].Value);
                }
                if (ConfigReader.Sections["Network"]?.Keys?.Contains("Show mail message preview") == true)
                {
                    MailManager.ShowPreview = Convert.ToBoolean(ConfigReader.Sections["Network"].Keys["Show mail message preview"].Value);
                }
                if (ConfigReader.Sections["Network"]?.Keys?.Contains("Record chat to debug log") == true)
                {
                    Flags.RecordChatToDebugLog = Convert.ToBoolean(ConfigReader.Sections["Network"].Keys["Record chat to debug log"].Value);
                }
                if (ConfigReader.Sections["Network"]?.Keys?.Contains("Show SSH banner") == true)
                {
                    SSH.SSHBanner = Convert.ToBoolean(ConfigReader.Sections["Network"].Keys["Show SSH banner"].Value);
                }
                if (ConfigReader.Sections["Network"]?.Keys?.Contains("Enable RPC") == true)
                {
                    RemoteProcedure.RPCEnabled = Convert.ToBoolean(ConfigReader.Sections["Network"].Keys["Enable RPC"].Value);
                }
                if (ConfigReader.Sections["Network"]?.Keys?.Contains("RPC Port") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Network"].Keys["RPC Port"].Value, out int argresult3))
                        RemoteProcedure.RPCPort = Convert.ToInt32(ConfigReader.Sections["Network"].Keys["RPC Port"].Value);
                }

                // Screensaver section
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Screensaver") == true)
                {
                    Screensaver.DefSaverName = ConfigReader.Sections["Screensaver"].Keys["Screensaver"].Value;
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Screensaver Timeout in ms") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Screensaver Timeout in ms"].Value, out int argresult4))
                        Screensaver.ScrnTimeout = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Screensaver Timeout in ms"].Value);
                }

                // Screensaver: Colors
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("ColorMix - Activate True Color Mode") == true)
                {
                    ColorMixSettings.ColorMixTrueColor = Convert.ToBoolean(ConfigReader.Sections["Screensaver"].Keys["ColorMix - Activate True Color Mode"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Disco - Activate True Color Mode") == true)
                {
                    DiscoSettings.DiscoTrueColor = Convert.ToBoolean(ConfigReader.Sections["Screensaver"].Keys["Disco - Activate True Color Mode"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("GlitterColor - Activate True Color Mode") == true)
                {
                    GlitterColorSettings.GlitterColorTrueColor = Convert.ToBoolean(ConfigReader.Sections["Screensaver"].Keys["GlitterColor - Activate True Color Mode"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Lines - Activate True Color Mode") == true)
                {
                    LinesSettings.LinesTrueColor = Convert.ToBoolean(ConfigReader.Sections["Screensaver"].Keys["Lines - Activate True Color Mode"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Dissolve - Activate True Color Mode") == true)
                {
                    DissolveSettings.DissolveTrueColor = Convert.ToBoolean(ConfigReader.Sections["Screensaver"].Keys["Dissolve - Activate True Color Mode"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("BouncingBlock - Activate True Color Mode") == true)
                {
                    BouncingBlockSettings.BouncingBlockTrueColor = Convert.ToBoolean(ConfigReader.Sections["Screensaver"].Keys["BouncingBlock - Activate True Color Mode"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("ProgressClock - Activate True Color Mode") == true)
                {
                    ProgressClockSettings.ProgressClockTrueColor = Convert.ToBoolean(ConfigReader.Sections["Screensaver"].Keys["ProgressClock - Activate True Color Mode"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Lighter - Activate True Color Mode") == true)
                {
                    LighterSettings.LighterTrueColor = Convert.ToBoolean(ConfigReader.Sections["Screensaver"].Keys["Lighter - Activate True Color Mode"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Wipe - Activate True Color Mode") == true)
                {
                    WipeSettings.WipeTrueColor = Convert.ToBoolean(ConfigReader.Sections["Screensaver"].Keys["Wipe - Activate True Color Mode"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Disco - Cycle Colors") == true)
                {
                    DiscoSettings.DiscoCycleColors = Convert.ToBoolean(ConfigReader.Sections["Screensaver"].Keys["Disco - Cycle Colors"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("ProgressClock - Cycle Colors") == true)
                {
                    ProgressClockSettings.ProgressClockCycleColors = Convert.ToBoolean(ConfigReader.Sections["Screensaver"].Keys["ProgressClock - Cycle Colors"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("ProgressClock - Color of Seconds Bar") == true)
                {
                    ProgressClockSettings.ProgressClockSecondsProgressColor = ConfigReader.Sections["Screensaver"].Keys["ProgressClock - Color of Seconds Bar"].Value;
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("ProgressClock - Color of Minutes Bar") == true)
                {
                    ProgressClockSettings.ProgressClockMinutesProgressColor = ConfigReader.Sections["Screensaver"].Keys["ProgressClock - Color of Minutes Bar"].Value;
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("ProgressClock - Color of Hours Bar") == true)
                {
                    ProgressClockSettings.ProgressClockHoursProgressColor = ConfigReader.Sections["Screensaver"].Keys["ProgressClock - Color of Hours Bar"].Value;
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("ProgressClock - Color of Information") == true)
                {
                    ProgressClockSettings.ProgressClockProgressColor = ConfigReader.Sections["Screensaver"].Keys["ProgressClock - Color of Information"].Value;
                }

                // Screensaver: Delays
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("BouncingBlock - Delay in Milliseconds") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["BouncingBlock - Delay in Milliseconds"].Value, out int argresult5))
                        BouncingBlockSettings.BouncingBlockDelay = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["BouncingBlock - Delay in Milliseconds"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("BouncingText - Delay in Milliseconds") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["BouncingText - Delay in Milliseconds"].Value, out int argresult6))
                        BouncingTextSettings.BouncingTextDelay = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["BouncingText - Delay in Milliseconds"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("ColorMix - Delay in Milliseconds") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["ColorMix - Delay in Milliseconds"].Value, out int argresult7))
                        ColorMixSettings.ColorMixDelay = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["ColorMix - Delay in Milliseconds"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Disco - Delay in Milliseconds") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Disco - Delay in Milliseconds"].Value, out int argresult8))
                        DiscoSettings.DiscoDelay = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Disco - Delay in Milliseconds"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("GlitterColor - Delay in Milliseconds") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["GlitterColor - Delay in Milliseconds"].Value, out int argresult9))
                        GlitterColorSettings.GlitterColorDelay = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["GlitterColor - Delay in Milliseconds"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("GlitterMatrix - Delay in Milliseconds") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["GlitterMatrix - Delay in Milliseconds"].Value, out int argresult10))
                        GlitterMatrixSettings.GlitterMatrixDelay = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["GlitterMatrix - Delay in Milliseconds"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Lines - Delay in Milliseconds") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Lines - Delay in Milliseconds"].Value, out int argresult11))
                        LinesSettings.LinesDelay = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Lines - Delay in Milliseconds"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Matrix - Delay in Milliseconds") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Matrix - Delay in Milliseconds"].Value, out int argresult12))
                        MatrixSettings.MatrixDelay = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Matrix - Delay in Milliseconds"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Lighter - Delay in Milliseconds") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Lighter - Delay in Milliseconds"].Value, out int argresult13))
                        LighterSettings.LighterDelay = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Lighter - Delay in Milliseconds"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Fader - Delay in Milliseconds") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Fader - Delay in Milliseconds"].Value, out int argresult14))
                        FaderSettings.FaderDelay = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Fader - Delay in Milliseconds"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Fader - Fade Out Delay in Milliseconds") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Fader - Fade Out Delay in Milliseconds"].Value, out int argresult15))
                        FaderSettings.FaderFadeOutDelay = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Fader - Fade Out Delay in Milliseconds"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("ProgressClock - Ticks to change color") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["ProgressClock - Ticks to change color"].Value, out int argresult16))
                        ProgressClockSettings.ProgressClockCycleColorsTicks = Convert.ToInt64(ConfigReader.Sections["Screensaver"].Keys["ProgressClock - Ticks to change color"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Typo - Delay in Milliseconds") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Typo - Delay in Milliseconds"].Value, out int argresult17))
                        TypoSettings.TypoDelay = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Typo - Delay in Milliseconds"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Typo - Write Again Delay in Milliseconds") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Typo - Write Again Delay in Milliseconds"].Value, out int argresult18))
                        TypoSettings.TypoWriteAgainDelay = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Typo - Write Again Delay in Milliseconds"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Wipe - Delay in Milliseconds") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Wipe - Delay in Milliseconds"].Value, out int argresult19))
                        WipeSettings.WipeDelay = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Wipe - Delay in Milliseconds"].Value);
                }

                // Screensaver: Texts
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("BouncingText - Text Shown") == true)
                {
                    BouncingTextSettings.BouncingTextWrite = ConfigReader.Sections["Screensaver"].Keys["BouncingText - Text Shown"].Value;
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Fader - Text Shown") == true)
                {
                    FaderSettings.FaderWrite = ConfigReader.Sections["Screensaver"].Keys["Fader - Text Shown"].Value;
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Typo - Text Shown") == true)
                {
                    TypoSettings.TypoWrite = ConfigReader.Sections["Screensaver"].Keys["Typo - Text Shown"].Value;
                }

                // Screensaver: Misc
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Lighter - Max Positions Count") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Lighter - Max Positions Count"].Value, out int argresult20))
                        LighterSettings.LighterMaxPositions = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Lighter - Max Positions Count"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Fader - Max Fade Steps") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Fader - Max Fade Steps"].Value, out int argresult21))
                        FaderSettings.FaderMaxSteps = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Fader - Max Fade Steps"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Typo - Minimum writing speed in WPM") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Typo - Minimum writing speed in WPM"].Value, out int argresult22))
                        TypoSettings.TypoWritingSpeedMin = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Typo - Minimum writing speed in WPM"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Typo - Maximum writing speed in WPM") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Typo - Maximum writing speed in WPM"].Value, out int argresult23))
                        TypoSettings.TypoWritingSpeedMax = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Typo - Maximum writing speed in WPM"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Typo - Probability of typo in percent") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Typo - Probability of typo in percent"].Value, out int argresult24))
                        TypoSettings.TypoMissStrikePossibility = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Typo - Probability of typo in percent"].Value);
                }
                if (ConfigReader.Sections["Screensaver"]?.Keys?.Contains("Wipe - Wipes to change direction") == true)
                {
                    if (int.TryParse(ConfigReader.Sections["Screensaver"].Keys["Wipe - Wipes to change direction"].Value, out int argresult25))
                        WipeSettings.WipeWipesNeededToChangeDirection = Convert.ToInt32(ConfigReader.Sections["Screensaver"].Keys["Wipe - Wipes to change direction"].Value);
                }

                // Misc section
                if (ConfigReader.Sections["Misc"].Keys.Contains("Show Time/Date on Upper Right Corner"))
                {
                    if (ConfigReader.Sections["Misc"].Keys["Show Time/Date on Upper Right Corner"].Value == "True")
                        Flags.CornerTimeDate = true;
                    else
                        Flags.CornerTimeDate = false;
                }
                if (ConfigReader.Sections["Misc"].Keys.Contains("Debug Size Quota in Bytes"))
                {
                    if (int.TryParse(ConfigReader.Sections["Misc"].Keys["Debug Size Quota in Bytes"].Value, out int argresult26))
                        DebugManager.DebugQuota = Convert.ToDouble(ConfigReader.Sections["Misc"].Keys["Debug Size Quota in Bytes"].Value);
                }
                if (ConfigReader.Sections["Misc"].Keys.Contains("Size parse mode"))
                {
                    if (ConfigReader.Sections["Misc"].Keys["Size parse mode"].Value == "True")
                        Flags.FullParseMode = true;
                    else
                        Flags.FullParseMode = false;
                }
                if (ConfigReader.Sections["Misc"].Keys.Contains("Marquee on startup"))
                {
                    if (ConfigReader.Sections["Misc"].Keys["Marquee on startup"].Value == "True")
                        Flags.StartScroll = true;
                    else
                        Flags.StartScroll = false;
                }
                if (ConfigReader.Sections["Misc"].Keys.Contains("Long Time and Date"))
                {
                    if (ConfigReader.Sections["Misc"].Keys["Long Time and Date"].Value == "True")
                        Flags.LongTimeDate = true;
                    else
                        Flags.LongTimeDate = false;
                }
                if (ConfigReader.Sections["Misc"].Keys.Contains("Show Hidden Files"))
                {
                    if (ConfigReader.Sections["Misc"].Keys["Show Hidden Files"].Value == "True")
                        Flags.HiddenFiles = true;
                    else
                        Flags.HiddenFiles = false;
                }
                if (ConfigReader.Sections["Misc"].Keys.Contains("Preferred Unit for Temperature"))
                {
                    KS.Misc.Forecast.Forecast.PreferredUnit = (UnitMeasurement)Convert.ToInt32(Enum.Parse(typeof(UnitMeasurement), ConfigReader.Sections["Misc"].Keys["Preferred Unit for Temperature"].Value));
                }
                if (ConfigReader.Sections["Misc"].Keys.Contains("Enable text editor autosave"))
                {
                    if (ConfigReader.Sections["Misc"].Keys["Enable text editor autosave"].Value == "True")
                        TextEditShellCommon.TextEdit_AutoSaveFlag = true;
                    else
                        TextEditShellCommon.TextEdit_AutoSaveFlag = false;
                }
                if (ConfigReader.Sections["Misc"].Keys.Contains("Text editor autosave interval"))
                {
                    if (int.TryParse(ConfigReader.Sections["Misc"].Keys["Text editor autosave interval"].Value, out int argresult27))
                        TextEditShellCommon.TextEdit_AutoSaveInterval = Convert.ToInt32(ConfigReader.Sections["Misc"].Keys["Text editor autosave interval"].Value);
                }
                if (ConfigReader.Sections["Misc"].Keys.Contains("Wrap list outputs"))
                {
                    if (ConfigReader.Sections["Misc"].Keys["Wrap list outputs"].Value == "True")
                        Flags.WrapListOutputs = true;
                    else
                        Flags.WrapListOutputs = false;
                }
                if (ConfigReader.Sections["Misc"].Keys.Contains("Filesystem sort mode"))
                {
                    Listing.SortMode = (FilesystemSortOptions)Convert.ToInt32(Enum.Parse(typeof(FilesystemSortOptions), ConfigReader.Sections["Misc"].Keys["Filesystem sort mode"].Value));
                }
                if (ConfigReader.Sections["Misc"].Keys.Contains("Filesystem sort direction"))
                {
                    Listing.SortDirection = (FilesystemSortDirection)Convert.ToInt32(Enum.Parse(typeof(FilesystemSortDirection), ConfigReader.Sections["Misc"].Keys["Filesystem sort direction"].Value));
                }

                // Return valid format
                Debug.WriteLine($"Returning ValidFormat as {ValidFormat}...");
                return ValidFormat;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while converting config! {ex.Message}");
                TextWriterColor.Write("  - Warning: Failed to completely convert config. Some of the configurations might not be fully migrated.", true, ColorTools.ColTypes.Warning);
                return false;
            }
        }
    }
}
#endif
