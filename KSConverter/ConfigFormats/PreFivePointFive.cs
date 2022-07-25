
// Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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
using System.IO;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell;

namespace KSConverter
{
    static class PreFivePointFive
    {
        // Pre-0.0.5.5 config format has "Kernel Version" as the first line.
        /// <summary>
        /// Takes configuration values and installs them to appropriate variables. Taken from config.vb version 0.0.5.2 with some removals that reflect this version
        /// </summary>
        /// <param name="PathToConfig">Path to pre-0.0.5.5 config (kernelConfig.ini)</param>
        public static bool ReadPreFivePointFiveConfig(string PathToConfig)
        {
            try
            {
                var OldConfigReader = new StreamReader(PathToConfig);
                string line = OldConfigReader.ReadLine();
                var ValidFormat = default(bool);
                Debug.WriteLine("Reading pre-0.0.5.5 config...");
                while (!string.IsNullOrEmpty(line))
                {
                    Debug.WriteLine($"Parsing line {line}...");
                    if (line.Contains("Kernel Version = "))
                    {
                        Debug.WriteLine("Valid config!");
                        ValidFormat = true;
                    }
                    if (ValidFormat)
                    {
                        if (line.Contains("Colored Shell = "))
                        {
                            if (line.Replace("Colored Shell = ", "") == "False")
                            {
                                Shell.ColoredShell = false;
                            }
                        }
                        if (line.Contains("User Name Shell Color = "))
                        {
                            if (Shell.ColoredShell == true)
                                ColorTools.UserNameShellColor = new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("User Name Shell Color = ", ""))));
                        }
                        else if (line.Contains("Host Name Shell Color = "))
                        {
                            if (Shell.ColoredShell == true)
                                ColorTools.HostNameShellColor = new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Host Name Shell Color = ", ""))));
                        }
                        else if (line.Contains("Continuable Kernel Error Color = "))
                        {
                            if (Shell.ColoredShell == true)
                                ColorTools.ContKernelErrorColor = new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Continuable Kernel Error Color = ", ""))));
                        }
                        else if (line.Contains("Uncontinuable Kernel Error Color = "))
                        {
                            if (Shell.ColoredShell == true)
                                ColorTools.UncontKernelErrorColor = new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Uncontinuable Kernel Error Color = ", ""))));
                        }
                        else if (line.Contains("Text Color = "))
                        {
                            if (Shell.ColoredShell == true)
                                ColorTools.NeutralTextColor = new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Text Color = ", ""))));
                        }
                        else if (line.Contains("License Color = "))
                        {
                            if (Shell.ColoredShell == true)
                                ColorTools.LicenseColor = new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("License Color = ", ""))));
                        }
                        else if (line.Contains("Background Color = "))
                        {
                            if (Shell.ColoredShell == true)
                            {
                                ColorTools.BackgroundColor = new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Background Color = ", ""))));
                            }
                        }
                        else if (line.Contains("Input Color = "))
                        {
                            if (Shell.ColoredShell == true)
                                ColorTools.InputColor = new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Input Color = ", ""))));
                        }
                        else if (line.Contains("Listed command in Help Color = "))
                        {
                            if (Shell.ColoredShell == true)
                                ColorTools.ListEntryColor = new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Listed command in Help Color = ", ""))));
                        }
                        else if (line.Contains("Definition of command in Help Color = "))
                        {
                            if (Shell.ColoredShell == true)
                                ColorTools.ListValueColor = new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Definition of command in Help Color = ", ""))));
                        }
                        else if (line.Contains("Maintenance Mode = "))
                        {
                            if (line.Replace("Maintenance Mode = ", "") == "True")
                            {
                                Flags.Maintenance = true;
                            }
                            else if (line.Replace("Maintenance Mode = ", "") == "False")
                            {
                                Flags.Maintenance = false;
                            }
                        }
                        else if (line.Contains("Prompt for Arguments on Boot = "))
                        {
                            if (line.Replace("Prompt for Arguments on Boot = ", "") == "True")
                            {
                                Flags.ArgsOnBoot = true;
                            }
                            else if (line.Replace("Prompt for Arguments on Boot = ", "") == "False")
                            {
                                Flags.ArgsOnBoot = false;
                            }
                        }
                        else if (line.Contains("Clear Screen on Log-in = "))
                        {
                            if (line.Replace("Clear Screen on Log-in = ", "") == "True")
                            {
                                Flags.ClearOnLogin = true;
                            }
                            else if (line.Replace("Clear Screen on Log-in = ", "") == "False")
                            {
                                Flags.ClearOnLogin = false;
                            }
                        }
                        else if (line.Contains("Show MOTD on Log-in = "))
                        {
                            if (line.Replace("Show MOTD on Log-in = ", "") == "True")
                            {
                                Flags.ShowMOTD = true;
                            }
                            else if (line.Replace("Show MOTD on Log-in = ", "") == "False")
                            {
                                Flags.ShowMOTD = false;
                            }
                        }
                        else if (line.Contains("Simplified Help Command = "))
                        {
                            if (line.Replace("Simplified Help Command = ", "") == "True")
                            {
                                Flags.SimHelp = true;
                            }
                            else if (line.Replace("Simplified Help Command = ", "") == "False")
                            {
                                Flags.SimHelp = false;
                            }
                        }
                        else if (line.Contains("Quiet Probe = "))
                        {
                            if (line.Replace("Quiet Probe = ", "") == "True")
                            {
                                Flags.QuietHardwareProbe = true;
                            }
                            else if (line.Replace("Quiet Probe = ", "") == "False")
                            {
                                Flags.QuietHardwareProbe = false;
                            }
                        }
                        else if (line.Contains("Show Time/Date on Corner = "))
                        {
                            if (line.Replace("Show Time/Date on Corner = ", "") == "True")
                            {
                                Flags.CornerTimeDate = true;
                            }
                            else if (line.Replace("Show Time/Date on Corner = ", "") == "False")
                            {
                                Flags.CornerTimeDate = false;
                            }
                        }
                        else if (line.Contains("MOTD = "))
                        {
                            Kernel.MOTDMessage = line.Replace("MOTD = ", "");
                        }
                        else if (line.Contains("Host Name = "))
                        {
                            Kernel.HostName = line.Replace("Host Name = ", "");
                        }
                        else if (line.Contains("MOTD After Login = "))
                        {
                            Kernel.MAL = line.Replace("MOTD After Login = ", "");
                        }
                    }
                    line = OldConfigReader.ReadLine();
                }
                OldConfigReader.Close();
                OldConfigReader.Dispose();
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