
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports KS.ConsoleBase

<TestFixture> Public Class ColorSettingTests

    ''' <summary>
    ''' Tests setting colors
    ''' </summary>
    <Test, Description("Setting")> Public Sub TestSetColors()
        SetColors(ConsoleColors.White,       'Input Color
                  ConsoleColors.White,       'License Color
                  ConsoleColors.Yellow,      'Continuable Kernel Error Color
                  ConsoleColors.Red,         'Uncontinuable Kernel Error Color
                  ConsoleColors.DarkGreen,   'Host name color
                  ConsoleColors.Green,       'User name color
                  ConsoleColors.Black,       'Background color
                  ConsoleColors.Gray,        'Neutral text
                  ConsoleColors.DarkYellow,  'List entry text
                  ConsoleColors.DarkGray,    'List value text
                  ConsoleColors.Green,       'Stage text
                  ConsoleColors.Red,         'Error text
                  ConsoleColors.Yellow,      'Warning text
                  ConsoleColors.DarkYellow,  'Option text
                  ConsoleColors.Green,       'Banner text
                  ConsoleColors.White,       'Notification title text
                  ConsoleColors.Gray,        'Notification description text
                  ConsoleColors.DarkYellow,  'Notification progress text
                  ConsoleColors.Red,         'Notification failure text
                  ConsoleColors.Yellow,      'Question text
                  ConsoleColors.Green,       'Success text
                  ConsoleColors.Gray,        'User dollar sign on shell text
                  ConsoleColors.Gray,        'Tip text
                  ConsoleColors.White,       'Separator text
                  ConsoleColors.Gray,        'Separator color
                  ConsoleColors.White,       'List title text
                  ConsoleColors.Yellow,      'Development warning text
                  ConsoleColors.Gray,        'Stage time text
                  ConsoleColors.DarkYellow,  'General progress text
                  ConsoleColors.DarkRed,     'Back option text
                  ConsoleColors.White,       'Low priority notification border color
                  ConsoleColors.Yellow,      'Medium priority notification border color
                  ConsoleColors.Red,         'High priority notification border color
                  ConsoleColors.DarkGray,    'Table separator
                  ConsoleColors.White,       'Table header
                  ConsoleColors.Gray,        'Table value
                  ConsoleColors.Yellow,      'Selected option
                  ConsoleColors.DarkGreen    'Alternative option
                 )
        InputColor.ShouldBeEquivalentTo(New Color(ConsoleColors.White))
        LicenseColor.ShouldBeEquivalentTo(New Color(ConsoleColors.White))
        ContKernelErrorColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Yellow))
        UncontKernelErrorColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Red))
        HostNameShellColor.ShouldBeEquivalentTo(New Color(ConsoleColors.DarkGreen))
        UserNameShellColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Green))
        BackgroundColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Black))
        NeutralTextColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Gray))
        ListEntryColor.ShouldBeEquivalentTo(New Color(ConsoleColors.DarkYellow))
        ListValueColor.ShouldBeEquivalentTo(New Color(ConsoleColors.DarkGray))
        StageColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Green))
        ErrorColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Red))
        WarningColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Yellow))
        OptionColor.ShouldBeEquivalentTo(New Color(ConsoleColors.DarkYellow))
        BannerColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Green))
        NotificationTitleColor.ShouldBeEquivalentTo(New Color(ConsoleColors.White))
        NotificationDescriptionColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Gray))
        NotificationProgressColor.ShouldBeEquivalentTo(New Color(ConsoleColors.DarkYellow))
        NotificationFailureColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Red))
        QuestionColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Yellow))
        SuccessColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Green))
        UserDollarColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Gray))
        TipColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Gray))
        SeparatorTextColor.ShouldBeEquivalentTo(New Color(ConsoleColors.White))
        SeparatorColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Gray))
        ListTitleColor.ShouldBeEquivalentTo(New Color(ConsoleColors.White))
        DevelopmentWarningColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Yellow))
        StageTimeColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Gray))
        ProgressColor.ShouldBeEquivalentTo(New Color(ConsoleColors.DarkYellow))
        LowPriorityBorderColor.ShouldBeEquivalentTo(New Color(ConsoleColors.White))
        MediumPriorityBorderColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Yellow))
        HighPriorityBorderColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Red))
        TableSeparatorColor.ShouldBeEquivalentTo(New Color(ConsoleColors.DarkGray))
        TableHeaderColor.ShouldBeEquivalentTo(New Color(ConsoleColors.White))
        TableValueColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Gray))
        SelectedOptionColor.ShouldBeEquivalentTo(New Color(ConsoleColors.Yellow))
        AlternativeOptionColor.ShouldBeEquivalentTo(New Color(ConsoleColors.DarkGreen))
    End Sub

End Class