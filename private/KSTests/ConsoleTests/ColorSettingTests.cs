
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
            ColorTools.GetColor(ColorTools.ColTypes.Input).ShouldBeEquivalentTo(new Color((int)ConsoleColors.White));
            ColorTools.GetColor(ColorTools.ColTypes.License).ShouldBeEquivalentTo(new Color((int)ConsoleColors.White));
            ColorTools.GetColor(ColorTools.ColTypes.ContKernelError).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Yellow));
            ColorTools.GetColor(ColorTools.ColTypes.UncontKernelError).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Red));
            ColorTools.GetColor(ColorTools.ColTypes.HostName).ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkGreen));
            ColorTools.GetColor(ColorTools.ColTypes.UserNameShell).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Green));
            ColorTools.GetColor(ColorTools.ColTypes.Background).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Black));
            ColorTools.GetColor(ColorTools.ColTypes.NeutralText).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Gray));
            ColorTools.GetColor(ColorTools.ColTypes.ListEntry).ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkYellow));
            ColorTools.GetColor(ColorTools.ColTypes.ListValue).ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkGray));
            ColorTools.GetColor(ColorTools.ColTypes.Stage).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Green));
            ColorTools.GetColor(ColorTools.ColTypes.Error).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Red));
            ColorTools.GetColor(ColorTools.ColTypes.Warning).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Yellow));
            ColorTools.GetColor(ColorTools.ColTypes.Option).ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkYellow));
            ColorTools.GetColor(ColorTools.ColTypes.Banner).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Green));
            ColorTools.GetColor(ColorTools.ColTypes.NotificationTitle).ShouldBeEquivalentTo(new Color((int)ConsoleColors.White));
            ColorTools.GetColor(ColorTools.ColTypes.NotificationDescription).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Gray));
            ColorTools.GetColor(ColorTools.ColTypes.NotificationProgress).ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkYellow));
            ColorTools.GetColor(ColorTools.ColTypes.NotificationFailure).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Red));
            ColorTools.GetColor(ColorTools.ColTypes.Question).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Yellow));
            ColorTools.GetColor(ColorTools.ColTypes.Success).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Green));
            ColorTools.GetColor(ColorTools.ColTypes.UserDollar).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Gray));
            ColorTools.GetColor(ColorTools.ColTypes.Tip).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Gray));
            ColorTools.GetColor(ColorTools.ColTypes.SeparatorText).ShouldBeEquivalentTo(new Color((int)ConsoleColors.White));
            ColorTools.GetColor(ColorTools.ColTypes.Separator).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Gray));
            ColorTools.GetColor(ColorTools.ColTypes.ListTitle).ShouldBeEquivalentTo(new Color((int)ConsoleColors.White));
            ColorTools.GetColor(ColorTools.ColTypes.DevelopmentWarning).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Yellow));
            ColorTools.GetColor(ColorTools.ColTypes.StageTime).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Gray));
            ColorTools.GetColor(ColorTools.ColTypes.Progress).ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkYellow));
            ColorTools.GetColor(ColorTools.ColTypes.LowPriorityBorder).ShouldBeEquivalentTo(new Color((int)ConsoleColors.White));
            ColorTools.GetColor(ColorTools.ColTypes.MediumPriorityBorder).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Yellow));
            ColorTools.GetColor(ColorTools.ColTypes.HighPriorityBorder).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Red));
            ColorTools.GetColor(ColorTools.ColTypes.TableSeparator).ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkGray));
            ColorTools.GetColor(ColorTools.ColTypes.TableHeader).ShouldBeEquivalentTo(new Color((int)ConsoleColors.White));
            ColorTools.GetColor(ColorTools.ColTypes.TableValue).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Gray));
            ColorTools.GetColor(ColorTools.ColTypes.SelectedOption).ShouldBeEquivalentTo(new Color((int)ConsoleColors.Yellow));
            ColorTools.GetColor(ColorTools.ColTypes.AlternativeOption).ShouldBeEquivalentTo(new Color((int)ConsoleColors.DarkGreen));
        }

    }
}
