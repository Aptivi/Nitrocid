
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FluentFTP;
using KS.ConsoleBase;
using KS.Files.Querying;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Shells;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Users.Groups;
using KS.Users;
using KS.Network.SpeedDial;
using KS.ConsoleBase.Colors;

namespace KSConverter
{
    static class Converter
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        public static void Main()
        {
            // Check console
            ConsoleChecker.CheckConsole();

            try
            {
                // Warning message
                TextWriterColor.Write("Warning: this converter will stop being shipped with Kernel Simulator starting from 0.1.0 Beta 1. It's no longer able to convert kernel configuration files.", true, KernelColorType.Warning);

                // Initialize all needed variables
                var ListOfOldPaths = ConverterTools.GetOldPaths("");
                var ListOfBackups = ConverterTools.GetOldPaths("KSBackup");

                // Load user token
                UserManagement.LoadUserToken();

                // Make backup directory
                SeparatorWriterColor.WriteSeparator("[1/6] Making backup directory", true, KernelColorType.Stage);
                Debug.WriteLine($"Backup directory: {ConverterTools.GetHomeDirectory() + "/KSBackup"}");
                Debug.WriteLine($"FolderExists = {Checking.FolderExists(ConverterTools.GetHomeDirectory() + "/KSBackup")}");
                if (!Checking.FolderExists(ConverterTools.GetHomeDirectory() + "/KSBackup"))
                {
                    // Just make it!
                    TextWriterColor.Write("  - Backup directory not found. Creating directory...", true, KernelColorType.Progress);
                    Debug.WriteLine("Creating directory...");
                    Directory.CreateDirectory(ConverterTools.GetHomeDirectory() + "/KSBackup");
                }
                else
                {
                    // Directory found. Skip the creation.
                    Debug.WriteLine("Already found.");
                    TextWriterColor.Write("  - Warning: backup directory is already found.", true, KernelColorType.Warning);
                }
                Console.WriteLine();

                // Make backup of old configuration files in case something goes wrong during conversion.
                SeparatorWriterColor.WriteSeparator("[2/6] Making backup of old configuration files", true, KernelColorType.Stage);
                foreach (string ConfigEntry in ListOfOldPaths.Keys)
                {
                    Debug.WriteLine($"Old path config entry: {ConfigEntry}");
                    Debug.WriteLine($"Old path exists: {Checking.FileExists(ListOfOldPaths[ConfigEntry])}");
                    Debug.WriteLine($"Backup exists: {Checking.FileExists(ListOfBackups[ConfigEntry])}");
                    if (Checking.FileExists(ListOfOldPaths[ConfigEntry]) & !Checking.FileExists(ListOfBackups[ConfigEntry]))
                    {
                        // Move the old config file to backup
                        Debug.WriteLine($"Moving {ConfigEntry} from {ListOfOldPaths[ConfigEntry]} to {ListOfBackups[ConfigEntry]}...");
                        TextWriterColor.Write("  - {0}: {1} -> {2}", true, KernelColorType.NeutralText, ConfigEntry, ListOfOldPaths[ConfigEntry], ListOfBackups[ConfigEntry]);
                        File.Move(ListOfOldPaths[ConfigEntry], ListOfBackups[ConfigEntry]);
                    }
                    else if (Checking.FileExists(ListOfBackups[ConfigEntry]))
                    {
                        Debug.WriteLine("We already have backup!");
                        TextWriterColor.Write("  - Warning: {0} already exists", true, KernelColorType.Warning, ListOfBackups[ConfigEntry]);
                    }
                    else
                    {
                        // File not found. Skip it.
                        Debug.WriteLine("We don't have config.");
                        TextWriterColor.Write("  - Warning: {0} not found in home directory.", true, KernelColorType.Warning, ListOfOldPaths[ConfigEntry]);
                    }
                }
                Console.WriteLine();

                // Import all blocked devices to DebugDeviceNames.json
                SeparatorWriterColor.WriteSeparator("[3/6] Importing all blocked devices to DebugDeviceNames.json", true, KernelColorType.Stage);
                Debug.WriteLine($"Blocked device backup exists = {Checking.FileExists(ListOfBackups["BlockedDevices"])}");
                if (Checking.FileExists(ListOfBackups["BlockedDevices"]))
                {
                    // Read blocked devices from old file
                    TextWriterColor.Write("  - Reading blocked devices from blocked_devices.csv...", true, KernelColorType.Progress);
                    Debug.WriteLine($"Calling File.ReadAllLines on {ListOfBackups["BlockedDevices"]}...");
                    var BlockedDevices = File.ReadAllLines(ListOfBackups["BlockedDevices"]).ToList();
                    Debug.WriteLine($"We have {BlockedDevices.Count} devices.");
                    TextWriterColor.Write("  - {0} devices found.", true, KernelColorType.NeutralText, BlockedDevices.Count);

                    // Add blocked devices to new format
                    Debug.WriteLine($"Iterating {BlockedDevices.Count} blocked devices...");
                    foreach (string BlockedDevice in BlockedDevices)
                    {
                        Debug.WriteLine($"Adding blocked device {BlockedDevice} to the new config format...");
                        TextWriterColor.Write("  - Adding {0} to DebugDeviceNames.json...", true, KernelColorType.Progress, BlockedDevice);
                        RemoteDebugTools.AddDeviceToJson(BlockedDevice, false);
                        RemoteDebugTools.SetDeviceProperty(BlockedDevice, RemoteDebugTools.DeviceProperty.Blocked, true);
                    }
                }
                else
                {
                    // File not found. Skip stage.
                    Debug.WriteLine("We don't have file.");
                    TextWriterColor.Write("  - Warning: blocked_devices.csv not found in home directory.", true, KernelColorType.Warning);
                }
                Console.WriteLine();

                // Import all FTP speed dial settings to JSON
                SeparatorWriterColor.WriteSeparator("[4/6] Importing all FTP speed dial addresses to FTP_SpeedDial.json", true, KernelColorType.Stage);
                Debug.WriteLine($"Speed dial addresses exists = {Checking.FileExists(ListOfBackups["FTPSpeedDial"])}");
                if (Checking.FileExists(ListOfBackups["FTPSpeedDial"]))
                {
                    // Read FTP speed dial addresses from old file
                    TextWriterColor.Write("  - Reading FTP speed dial addresses from ftp_speeddial.csv...", true, KernelColorType.Progress);
                    Debug.WriteLine($"Calling File.ReadAllLines on {ListOfBackups["FTPSpeedDial"]}...");
                    var SpeedDialLines = File.ReadAllLines(ListOfBackups["FTPSpeedDial"]);
                    Debug.WriteLine($"We have {SpeedDialLines.Length} addresses.");
                    TextWriterColor.Write("  - {0} addresses found.", true, KernelColorType.NeutralText, SpeedDialLines.Length);

                    // Add addresses to new format
                    foreach (string SpeedDialLine in SpeedDialLines)
                    {
                        // Populate variables
                        var ChosenLineSeparation = SpeedDialLine.Split(',');
                        Debug.WriteLine($"Separation count = {ChosenLineSeparation.Length}");
                        string Address = ChosenLineSeparation[0];
                        Debug.WriteLine($"Address = {Address}");
                        string Port = ChosenLineSeparation[1];
                        Debug.WriteLine($"Port = {Port}");
                        string Username = ChosenLineSeparation[2];
                        Debug.WriteLine($"Username = {Username}");
                        FtpEncryptionMode Encryption = (FtpEncryptionMode)Enum.Parse(typeof(FtpEncryptionMode), ChosenLineSeparation[3]);
                        Debug.WriteLine($"Encryption = {Encryption}");

                        // Add the entry!
                        TextWriterColor.Write("  - Adding {0} to FTP_SpeedDial.json...", true, KernelColorType.Progress, Address);
                        SpeedDialTools.AddEntryToSpeedDial(Address, Convert.ToInt32(Port), SpeedDialType.FTP, true, Username, Encryption);
                    }
                }
                else
                {
                    // File not found. Skip stage.
                    Debug.WriteLine("We don't have file.");
                    TextWriterColor.Write("  - Warning: ftp_speeddial.csv not found in home directory.", true, KernelColorType.Warning);
                }
                Console.WriteLine();

                // Import all users to JSON
                SeparatorWriterColor.WriteSeparator("[5/6] Importing all users to Users.json", true, KernelColorType.Stage);
                Debug.WriteLine($"Users file exists = {Checking.FileExists(ListOfBackups["Users"])}");
                if (Checking.FileExists(ListOfBackups["Users"]))
                {
                    // Read all users from old file
                    TextWriterColor.Write("  - Reading users from users.csv...", true, KernelColorType.Progress);
                    Debug.WriteLine($"Calling File.ReadAllLines on {ListOfBackups["Users"]}...");
                    var UsersLines = File.ReadAllLines(ListOfBackups["Users"]);
                    Debug.WriteLine($"We have {UsersLines.Length} addresses.");
                    TextWriterColor.Write("  - {0} users found.", true, KernelColorType.NeutralText, UsersLines.Length);

                    // Add users to new format
                    foreach (string UsersLine in UsersLines)
                    {
                        // Populate variables
                        Debug.WriteLine($"Parsing line {UsersLine}...");
                        var ChosenLineSeparation = UsersLine.Split(',');
                        Debug.WriteLine($"Separation count = {ChosenLineSeparation.Length}");
                        string Username = ChosenLineSeparation[0];
                        Debug.WriteLine($"Username = {Username}");
                        string Password = ChosenLineSeparation[1];
                        Debug.WriteLine($"Password = {Password}");
                        string Administrator = ChosenLineSeparation.Length >= 3 ? ChosenLineSeparation[2] : "False";
                        Debug.WriteLine($"Administrator = {Administrator}");
                        string Disabled = ChosenLineSeparation.Length >= 4 ? ChosenLineSeparation[3] : "False";
                        Debug.WriteLine($"Disabled = {Disabled}");
                        string Anonymous = ChosenLineSeparation.Length >= 5 ? ChosenLineSeparation[4] : "False";
                        Debug.WriteLine($"Anonymous = {Anonymous}");

                        // Add the entry!
                        TextWriterColor.Write("  - Adding {0} to Users.json...", true, KernelColorType.Progress, Username);
                        UserManagement.InitializeUser(Username, Password, false);
                        if (Administrator == "True")
                        {
                            Debug.WriteLine($"Adding the Administrator permission to {Username}...");
                            GroupManagement.AddGroup(GroupManagement.GroupType.Administrator, Username);
                        }
                        if (Disabled == "True")
                        {
                            Debug.WriteLine($"Adding the Disabled permission to {Username}...");
                            GroupManagement.AddGroup(GroupManagement.GroupType.Disabled, Username);
                        }
                        if (Anonymous == "True")
                        {
                            Debug.WriteLine($"Adding the Anonymous permission to {Username}...");
                            GroupManagement.AddGroup(GroupManagement.GroupType.Anonymous, Username);
                        }
                    }
                }
                else
                {
                    // File not found. Skip stage.
                    Debug.WriteLine("We don't have file.");
                    TextWriterColor.Write("  - Warning: users.csv not found in home directory.", true, KernelColorType.Warning);
                }
                Console.WriteLine();

                // Import all aliases to JSON
                SeparatorWriterColor.WriteSeparator("[6/6] Importing all aliases to Aliases.json", true, KernelColorType.Stage);
                Debug.WriteLine($"Aliases file exists = {Checking.FileExists(ListOfBackups["Aliases"])}");
                if (Checking.FileExists(ListOfBackups["Aliases"]))
                {
                    // Read all aliases from old file
                    TextWriterColor.Write("  - Reading users from aliases.csv...", true, KernelColorType.Progress);
                    Debug.WriteLine($"Calling File.ReadAllLines on {ListOfBackups["Aliases"]}...");
                    var AliasesLines = File.ReadAllLines(ListOfBackups["Aliases"]);
                    Debug.WriteLine($"We have {AliasesLines.Length} aliases.");
                    TextWriterColor.Write("  - {0} aliases found.", true, KernelColorType.NeutralText, AliasesLines.Length);

                    // Add aliases to new format
                    foreach (string AliasLine in AliasesLines)
                    {
                        // POpulate variables
                        var AliasLineSplit = AliasLine.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        Debug.WriteLine($"Separation count = {AliasLineSplit.Length}");
                        string AliasCommand = AliasLineSplit[1];
                        Debug.WriteLine($"AliasCommand = {AliasCommand}");
                        string ActualCommand = AliasLineSplit[2];
                        Debug.WriteLine($"ActualCommand = {ActualCommand}");
                        string AliasType = AliasLineSplit[0];
                        Debug.WriteLine($"AliasType = {AliasType}");

                        // Add the entry!
                        TextWriterColor.Write("  - Adding {0} to Aliases.json...", true, KernelColorType.Progress, AliasCommand);
                        switch (AliasType)
                        {
                            case "Shell":
                                {
                                    Dictionary<string, string> aliases = AliasManager.GetAliasesListFromType(ShellType.Shell);
                                    if (!aliases.ContainsKey(AliasCommand))
                                        AliasManager.AddAlias(AliasCommand, ActualCommand, ShellType.Shell);
                                    break;
                                }
                            case "FTPShell":
                                {
                                    Dictionary<string, string> aliases = AliasManager.GetAliasesListFromType(ShellType.FTPShell);
                                    if (!aliases.ContainsKey(AliasCommand))
                                        AliasManager.AddAlias(AliasCommand, ActualCommand, ShellType.FTPShell);
                                    break;
                                }
                            case "SFTPShell":
                                {
                                    Dictionary<string, string> aliases = AliasManager.GetAliasesListFromType(ShellType.SFTPShell);
                                    if (!aliases.ContainsKey(AliasCommand))
                                        AliasManager.AddAlias(AliasCommand, ActualCommand, ShellType.SFTPShell);
                                    break;
                                }
                            case "Mail":
                                {
                                    Dictionary<string, string> aliases = AliasManager.GetAliasesListFromType(ShellType.MailShell);
                                    if (!aliases.ContainsKey(AliasCommand))
                                        AliasManager.AddAlias(AliasCommand, ActualCommand, ShellType.MailShell);
                                    break;
                                }
                            default:
                                {
                                    TextWriterColor.Write("  - Invalid type {0}", true, KernelColorType.Error, AliasType);
                                    break;
                                }
                        }
                    }

                    // Save the changes
                    Debug.WriteLine("Saving...");
                    TextWriterColor.Write("  - Saving aliases to Aliases.json...", true, KernelColorType.Progress);
                    AliasManager.SaveAliases();
                }
                else
                {
                    // File not found. Skip stage.
                    Debug.WriteLine("We don't have file.");
                    TextWriterColor.Write("  - Warning: aliases.csv not found in home directory.", true, KernelColorType.Warning);
                }
                Console.WriteLine();

                // Print this message:
                TextWriterColor.Write("- Successfully converted all settings to new format! Enjoy!", true, KernelColorType.Success);
                TextWriterColor.Write("- Press any key to exit.", true, KernelColorType.Success);
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                TextWriterColor.Write("- Error converting settings: {0}", true, KernelColorType.Error, ex.Message);
                TextWriterColor.Write("- Press any key to exit. Stack trace below:", true, KernelColorType.Error);
                TextWriterColor.Write(ex.StackTrace, true, KernelColorType.NeutralText);
                Console.ReadKey(true);
            }
        }

    }
}
