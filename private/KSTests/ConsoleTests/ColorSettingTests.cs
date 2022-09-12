
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

using ColorSeq;
using KS.ConsoleBase.Colors;
using NUnit.Framework;
using Shouldly;

namespace KSTests.ConsoleTests
{

    [TestFixture]
    public class ColorSettingTests
    {

        /// <summary>
        /// Tests setting colors
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestSetColors()
        {
            ColorTools.SetColors(
                ((int)ConsoleColors.White).ToString(),         // Input Color
                ((int)ConsoleColors.White).ToString(),         // License Color
                ((int)ConsoleColors.Yellow).ToString(),        // Continuable Kernel Error Color
                ((int)ConsoleColors.Red).ToString(),           // Uncontinuable Kernel Error Color
                ((int)ConsoleColors.DarkGreen).ToString(),     // Host name color
                ((int)ConsoleColors.Green).ToString(),         // User name color
                ((int)ConsoleColors.Black).ToString(),         // Background color
                ((int)ConsoleColors.Gray).ToString(),          // Neutral text
                ((int)ConsoleColors.DarkYellow).ToString(),    // List entry text
                ((int)ConsoleColors.DarkGray).ToString(),      // List value text
                ((int)ConsoleColors.Green).ToString(),         // Stage text
                ((int)ConsoleColors.Red).ToString(),           // Error text
                ((int)ConsoleColors.Yellow).ToString(),        // Warning text
                ((int)ConsoleColors.DarkYellow).ToString(),    // Option text
                ((int)ConsoleColors.Green).ToString(),         // Banner text
                ((int)ConsoleColors.White).ToString(),         // Notification title text
                ((int)ConsoleColors.Gray).ToString(),          // Notification description text
                ((int)ConsoleColors.DarkYellow).ToString(),    // Notification progress text
                ((int)ConsoleColors.Red).ToString(),           // Notification failure text
                ((int)ConsoleColors.Yellow).ToString(),        // Question text
                ((int)ConsoleColors.Green).ToString(),         // Success text
                ((int)ConsoleColors.Gray).ToString(),          // User dollar sign on shell text
                ((int)ConsoleColors.Gray).ToString(),          // Tip text
                ((int)ConsoleColors.White).ToString(),         // Separator text
                ((int)ConsoleColors.Gray).ToString(),          // Separator color
                ((int)ConsoleColors.White).ToString(),         // List title text
                ((int)ConsoleColors.Yellow).ToString(),        // Development warning text
                ((int)ConsoleColors.Gray).ToString(),          // Stage time text
                ((int)ConsoleColors.DarkYellow).ToString(),    // General progress text
                ((int)ConsoleColors.DarkRed).ToString(),       // Back option text
                ((int)ConsoleColors.White).ToString(),         // Low priority notification border color
                ((int)ConsoleColors.Yellow).ToString(),        // Medium priority notification border color
                ((int)ConsoleColors.Red).ToString(),           // High priority notification border color
                ((int)ConsoleColors.DarkGray).ToString(),      // Table separator
                ((int)ConsoleColors.White).ToString(),         // Table header
                ((int)ConsoleColors.Gray).ToString(),          // Table value
                ((int)ConsoleColors.Yellow).ToString(),        // Selected option
                ((int)ConsoleColors.DarkGreen).ToString()      // Alternative option
            );

            // Check for correctness
            ColorTools.InputColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.White));
            ColorTools.LicenseColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.White));
            ColorTools.ContKernelErrorColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Yellow));
            ColorTools.UncontKernelErrorColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Red));
            ColorTools.HostNameShellColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkGreen));
            ColorTools.UserNameShellColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Green));
            ColorTools.BackgroundColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Black));
            ColorTools.NeutralTextColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Gray));
            ColorTools.ListEntryColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkYellow));
            ColorTools.ListValueColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkGray));
            ColorTools.StageColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Green));
            ColorTools.ErrorColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Red));
            ColorTools.WarningColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Yellow));
            ColorTools.OptionColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkYellow));
            ColorTools.BannerColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Green));
            ColorTools.NotificationTitleColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.White));
            ColorTools.NotificationDescriptionColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Gray));
            ColorTools.NotificationProgressColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkYellow));
            ColorTools.NotificationFailureColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Red));
            ColorTools.QuestionColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Yellow));
            ColorTools.SuccessColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Green));
            ColorTools.UserDollarColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Gray));
            ColorTools.TipColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Gray));
            ColorTools.SeparatorTextColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.White));
            ColorTools.SeparatorColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Gray));
            ColorTools.ListTitleColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.White));
            ColorTools.DevelopmentWarningColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Yellow));
            ColorTools.StageTimeColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Gray));
            ColorTools.ProgressColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkYellow));
            ColorTools.LowPriorityBorderColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.White));
            ColorTools.MediumPriorityBorderColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Yellow));
            ColorTools.HighPriorityBorderColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Red));
            ColorTools.TableSeparatorColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkGray));
            ColorTools.TableHeaderColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.White));
            ColorTools.TableValueColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Gray));
            ColorTools.SelectedOptionColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.Yellow));
            ColorTools.AlternativeOptionColor.ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkGreen));
        }

    }
}
