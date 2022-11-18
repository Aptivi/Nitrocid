
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
using System.IO;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell;
using ColorSeq;
using KS.Misc.Probers.Motd;
using KS.Network.Base;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

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
                        if (line.Contains("User Name Shell , "))
                        {
                            ColorTools.SetColor(ColorTools.ColTypes.UserNameShell, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("User Name Shell , ", "")))));
                        }
                        else if (line.Contains("Host Name Shell , "))
                        {
                            ColorTools.SetColor(ColorTools.ColTypes.HostNameShell, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Host Name Shell , ", "")))));
                        }
                        else if (line.Contains("Continuable Kernel Error , "))
                        {
                            ColorTools.SetColor(ColorTools.ColTypes.ContKernelError, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Continuable Kernel Error , ", "")))));
                        }
                        else if (line.Contains("Uncontinuable Kernel Error , "))
                        {
                            ColorTools.SetColor(ColorTools.ColTypes.UncontKernelError, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Uncontinuable Kernel Error , ", "")))));
                        }
                        else if (line.Contains("Text , "))
                        {
                            ColorTools.SetColor(ColorTools.ColTypes.NeutralText, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Text , ", "")))));
                        }
                        else if (line.Contains("License , "))
                        {
                            ColorTools.SetColor(ColorTools.ColTypes.License, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("License , ", "")))));
                        }
                        else if (line.Contains("Background , "))
                        {
                            ColorTools.SetColor(ColorTools.ColTypes.Background, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Background , ", "")))));
                        }
                        else if (line.Contains("Input , "))
                        {
                            ColorTools.SetColor(ColorTools.ColTypes.Input, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Input , ", "")))));
                        }
                        else if (line.Contains("Listed command in Help , "))
                        {
                            ColorTools.SetColor(ColorTools.ColTypes.ListEntry, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Listed command in Help , ", "")))));
                        }
                        else if (line.Contains("Definition of command in Help , "))
                        {
                            ColorTools.SetColor(ColorTools.ColTypes.ListValue, new Color(Convert.ToInt32(Enum.Parse(typeof(ConsoleColors), line.Replace("Definition of command in Help , ", "")))));
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
                            MotdParse.MOTDMessage = line.Replace("MOTD = ", "");
                        }
                        else if (line.Contains("Host Name = "))
                        {
                            NetworkTools.HostName = line.Replace("Host Name = ", "");
                        }
                        else if (line.Contains("MOTD After Login = "))
                        {
                            MalParse.MAL = line.Replace("MOTD After Login = ", "");
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
