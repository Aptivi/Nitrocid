using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Misc.Writers.ConsoleWriters;

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

namespace KS.Misc.Splash
{
    public static class SplashReport
    {

        internal static int _Progress = 0;
        internal static string _ProgressText = "";
        internal static bool _KernelBooted = false;

        /// <summary>
        /// The progress indicator of the kernel 
        /// </summary>
        public static int Progress
        {
            get
            {
                return _Progress;
            }
        }

        /// <summary>
        /// The progress text to indicate how did the kernel progress
        /// </summary>
        public static string ProgressText
        {
            get
            {
                return _ProgressText;
            }
        }

        /// <summary>
        /// Did the kernel boot successfully?
        /// </summary>
        public static bool KernelBooted
        {
            get
            {
                return _KernelBooted;
            }
        }

        /// <summary>
        /// Reports the progress for the splash screen while the kernel is booting.
        /// </summary>
        /// <param name="Text">The progress text to indicate how did the kernel progress</param>
        /// <param name="Progress">The progress indicator of the kernel</param>
        /// <remarks>
        /// If the kernel has booted successfully, it will act like the normal printing command. If this routine was called during boot,<br></br>
        /// it will report the progress to the splash system.
        /// </remarks>
        internal static void ReportProgress(string Text, int Progress, ColorTools.ColTypes ColTypes, params string[] Vars)
        {
            if (!KernelBooted)
            {
                _Progress += Progress;
                _ProgressText = Text;
                if (_Progress >= 100)
                    _Progress = 100;
                if (SplashManager.CurrentSplashInfo.DisplaysProgress)
                {
                    if (Flags.EnableSplash)
                    {
                        SplashManager.CurrentSplash.Report(_Progress, Text, Vars);
                    }
                    else if (!Flags.QuietKernel)
                    {
                        TextWriterColor.Write(Text, true, ColTypes, Vars);
                    }
                }
            }
            else
            {
                TextWriterColor.Write(Text, true, ColTypes, Vars);
            }
        }

    }
}