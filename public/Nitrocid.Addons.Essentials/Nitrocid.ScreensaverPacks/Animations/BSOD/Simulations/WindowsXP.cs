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

using System;
using System.Collections.Generic;
using Terminaux.Colors;
using Nitrocid.Drivers.RNG;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Data;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations
{
    internal class WindowsXP : BaseBSOD
    {
        // Refer to https://learn.microsoft.com/en-us/windows-hardware/drivers/debugger/bug-check-code-reference2 for more info
        public enum BugCheckCodes
        {
            IRQL_NOT_LESS_OR_EQUAL,
            DRIVER_IRQL_NOT_LESS_OR_EQUAL,
            CRITICAL_OBJECT_TERMINATION,
            BAD_POOL_HEADER,
            MEMORY_MANAGEMENT,
            KMODE_EXCEPTION_NOT_HANDLED,
            PHASE0_INITIALIZATION_FAILED,
            PHASE1_INITIALIZATION_FAILED,
            SYSTEM_SERVICE_EXCEPTION,
            PAGE_FAULT_IN_NONPAGED_AREA,
            HAL_INITIALIZATION_FAILED,
            UNSUPPORTED_PROCESSOR,
            BAD_SYSTEM_CONFIG_INFO,
            INACCESSIBLE_BOOT_DEVICE,
            SYSTEM_THREAD_EXCEPTION_NOT_HANDLED,
            UNEXPECTED_KERNEL_MODE_TRAP,
            KERNEL_MODE_EXCEPTION_NOT_HANDLED,
            MACHINE_CHECK_EXCEPTION,
            DRIVER_POWER_STATE_FAILURE,
            BAD_POOL_CALLER,
            THREAD_STUCK_IN_DEVICE_DRIVER,
            UNMOUNTABLE_BOOT_VOLUME,
            MAXIMUM_WAIT_OBJECTS_EXCEEDED,
            WHEA_UNCORRECTABLE_ERROR,
            UNEXPECTED_STORE_EXCEPTION,
            MANUALLY_INITIATED_CRASH,
        }

        public class BugCheckParams
        {
            public int WindowsBugCheckCode;
            public bool DisplayMessage;
            public string Message = "";
        }

        private static readonly Dictionary<BugCheckCodes, BugCheckParams> BugChecks = new()
        {
            { BugCheckCodes.IRQL_NOT_LESS_OR_EQUAL,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0xA,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.MAXIMUM_WAIT_OBJECTS_EXCEEDED,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0xC,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.DRIVER_IRQL_NOT_LESS_OR_EQUAL,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0xD1,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.CRITICAL_OBJECT_TERMINATION,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0xF4,
                    DisplayMessage = true,
                    Message =
                    """

                    A process or thread crucial to system operation has unexpectedly exited or been terminated.
                    """
                }
            },

            { BugCheckCodes.BAD_POOL_HEADER,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x19,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.MEMORY_MANAGEMENT,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x1A,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.KMODE_EXCEPTION_NOT_HANDLED,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x1E,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.PHASE0_INITIALIZATION_FAILED,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x31,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.PHASE1_INITIALIZATION_FAILED,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x32,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.SYSTEM_SERVICE_EXCEPTION,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x3B,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.PAGE_FAULT_IN_NONPAGED_AREA,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x50,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.HAL_INITIALIZATION_FAILED,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x5C,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.UNSUPPORTED_PROCESSOR,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x5D,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.BAD_SYSTEM_CONFIG_INFO,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x74,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.INACCESSIBLE_BOOT_DEVICE,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x7B,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.SYSTEM_THREAD_EXCEPTION_NOT_HANDLED,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x7E,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.UNEXPECTED_KERNEL_MODE_TRAP,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x7F,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.KERNEL_MODE_EXCEPTION_NOT_HANDLED,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x8E,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.MACHINE_CHECK_EXCEPTION,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x9C,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.DRIVER_POWER_STATE_FAILURE,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x9F,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.BAD_POOL_CALLER,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0xC2,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.MANUALLY_INITIATED_CRASH,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0xE2,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.THREAD_STUCK_IN_DEVICE_DRIVER,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0xEA,
                    DisplayMessage = true,
                    Message =
                        """

                        The device driver got stuck in an infinite loop. This usually indicates
                        problem with the device itself or with the device driver programming the
                        hardware incorrectly.

                        Please check with your hardware device vendor for any driver updates.
                        """
                }
            },

            { BugCheckCodes.UNMOUNTABLE_BOOT_VOLUME,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0xED,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.WHEA_UNCORRECTABLE_ERROR,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x124,
                    DisplayMessage = false
                }
            },

            { BugCheckCodes.UNEXPECTED_STORE_EXCEPTION,
                new BugCheckParams()
                {
                    WindowsBugCheckCode = 0x154,
                    DisplayMessage = false
                }
            },
        };

        public override void Simulate()
        {
            // Select a random bugcheck
            int bugCheckEnumLength = BugChecks.Count;
            int bugCheckIdx = RandomDriver.RandomIdx(bugCheckEnumLength);
            var bugCheck = (BugCheckCodes)Enum.Parse(typeof(BugCheckCodes), bugCheckIdx.ToString());

            // Now, display that bugcheck
            DisplayBugCheck(bugCheck);
        }

        public void DisplayBugCheck(BugCheckCodes BugCheckCode)
        {
            // Windows 7's BSOD is the same as Windows XP's and Windows Vista's BSOD.
            var bugParams = BugChecks[BugCheckCode];
            ConsoleColoring.LoadBackDry(new Color(ConsoleColors.DarkBlue));
            ConsoleColoring.SetConsoleColor(new Color(ConsoleColors.White));

            // First, write the introduction
            TextWriterRaw.WritePlain("\nA problem has been detected and Windows has been shut down to prevent damage\n" +
                                       "to your computer.\n", true);

            // Then, get the message
            bool displayCodeName = RandomDriver.RandomRussianRoulette();
            string bugCheckMessage = bugParams.DisplayMessage ? bugParams.Message :
                                     // We're not displaying message, but display code if Russian Roulette returned true.
                                     displayCodeName ? BugCheckCode.ToString() : "";
            if (!string.IsNullOrEmpty(bugCheckMessage))
                TextWriterRaw.WritePlain($"{bugCheckMessage}\n", true);

            // If this is the first time...
            TextWriterRaw.WritePlain("If this is the first time you've seen this Stop error screen,\n" +
                                       "restart your computer. If this screen appears again, follow\n" +
                                       "these steps:\n", true);

            // Display some steps as to how to update your software and hardware drivers through Windows Update
            TextWriterRaw.WritePlain("Check to make sure any new hardware or software is properly installed.\n" +
                                       "If this is a new installation, ask your hardware or software manufacturer\n" +
                                       "for any Windows updates you might need.\n", true);

            // Display an unhelpful step that only applies to 2001-era computers or older
            TextWriterRaw.WritePlain("If problems continue, disable or remove any newly installed hardware\n" +
                                       "or software. Disable BIOS memory options such as caching or shadowing.", true);

            // Safe mode...
            TextWriterRaw.WritePlain("If you need to use Safe Mode to remove or disable components, restart\n" +
                                       "your computer, press F8 to select Advanced Startup Options, and then\n" +
                                       "select Safe Mode.\n", true);

            // Display technical information
            TextWriterRaw.WritePlain("Technical information:\n\n" +
                                      $"*** STOP: 0x{bugParams.WindowsBugCheckCode:X8} (0x{RandomDriver.Random():X8}, 0x{RandomDriver.Random():X8}, 0x{RandomDriver.Random():X8}, 0x{RandomDriver.Random():X8})\n", true);

            // Display dumping message and stop here
            TextWriterRaw.WritePlain("Collecting data for crash dump...\n" +
                                       "Initializing disk for crash dump...\n" +
                                       "Beginning dump of physical memory.", true);
            TextWriterRaw.WritePlain("Dumping physical memory to disk:  ", false);
        }
    }
}
