
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Imports System.Runtime.InteropServices

Public Module Color255

    <DllImport("kernel32.dll", SetLastError:=True)>
    Public Function SetConsoleMode(ByVal hConsoleHandle As IntPtr, ByVal mode As Integer) As Boolean
    End Function
    <DllImport("kernel32.dll", SetLastError:=True)>
    Public Function GetConsoleMode(ByVal handle As IntPtr, <Out()> ByRef mode As Integer) As Boolean
    End Function
    <DllImport("kernel32.dll", SetLastError:=True)>
    Public Function GetStdHandle(ByVal handle As Integer) As IntPtr
    End Function

    Public Enum ConsoleColors As Integer
        Black = 0
        DarkRed = 1
        DarkGreen = 2
        DarkYellow = 3
        DarkBlue = 4
        DarkMagenta = 5
        DarkCyan = 6
        Gray = 7
        DarkGray = 8
        Red = 9
        Green = 10
        Yellow = 11
        Blue = 12
        Magenta = 13
        Cyan = 14
        White = 15
        Grey0 = 16
        NavyBlue = 17
        DarkBlue_000087 = 18
        Blue3 = 19
        Blue3_d7 = 20
        Blue1 = 21
        DarkGreen_005f00 = 22
        DeepSkyBlue4_005f5f = 23
        DeepSkyBlue4_005f87 = 24
        DeepSkyBlue4_005faf = 25
        DodgerBlue3 = 26
        DodgerBlue2 = 27
        Green4 = 28
        SpringGreen4 = 29
        Turquoise4 = 30
        DeepSkyBlue3_0087af = 31
        DeepSkyBlue3_0087d7 = 32
        DodgerBlue1 = 33
        Green3_af00 = 34
        SpringGreen3_00af5f = 35
        DarkCyan_00af87 = 36
        LightSeaGreen = 37
        DeepSkyBlue2 = 38
        DeepSkyBlue1 = 39
        Green3_00d700 = 40
        SpringGreen3_00d75f = 41
        SpringGreen2_00d787 = 42
        Cyan3 = 43
        DarkTurquoise = 44
        Turquoise2 = 45
        Green1 = 46
        SpringGreen2 = 47
        SpringGreen1 = 48
        MediumSpringGreen = 49
        Cyan2 = 50
        Cyan1 = 51
        DarkRed_5f0000 = 52
        DeepPink4 = 53
        Purple4_5f0087 = 54
        Purple4_5f00af = 55
        Purple3 = 56
        BlueViolet = 57
        Orange4 = 58
        Grey37 = 59
        MediumPurple4 = 60
        SlateBlue3_5f5faf = 61
        SlateBlue3_5f5fd7 = 62
        RoyalBlue1 = 63
        Chartreuse4 = 64
        DarkSeaGreen4 = 65
        PaleTurquoise4 = 66
        SteelBlue = 67
        SteelBlue3 = 68
        CornflowerBlue = 69
        Chartreuse3 = 70
        DarkSeaGreen4_5faf5f = 71
        CadetBlue_5faf87 = 72
        CadetBlue_5fafaf = 73
        SkyBlue3 = 74
        SteelBlue1_5fafff = 75
        Chartreuse3_5fd700 = 76
        PaleGreen3 = 77
        SeaGreen3 = 78
        Aquamarine3 = 79
        MediumTurquoise = 80
        SteelBlue1_5fd7ff = 81
        Chartreuse2
        SeaGreen2
        SeaGreen1
        SeaGreen1_5fffaf
        Aquamarine1
        DarkSlateGray2
        DarkRed_870000
        DeepPink4_87005f
        DarkMagenta_870087
        DarkMagenta_8700af
        DarkViolet
        Purple
        Orange4_875f00
        LightPink4
        Plum4
        MediumPurple3_875faf
        MediumPurple3_875fd7
        SlateBlue1
        Yellow4
        Wheat4
        Grey53
        LightSlateGrey
        MediumPurple
        LightSlateBlue
        Yellow4_87af00
        DarkOliveGreen3
        DarkSeaGreen
        LightSkyBlue3
        LightSkyBlue3_87afd7
        SkyBlue2
        Chartreuse2_87d700
        DarkOliveGreen3_87d75f
        PaleGreen3_87d787
        DarkSeaGreen3
        DarkSlateGray3
        SkyBlue1
        Chartreuse1
        LightGreen
        LightGreen_87ff87
        PaleGreen1
        Aquamarine1_87ffd7
        DarkSlateGray1
        Red3
        DeepPink4_af005f
        MediumVioletRed
        Magenta3
        DarkViolet_af00d7
        Purple_af00ff
        DarkOrange3
        IndianRed
        HotPink3
        MediumOrchid3
        MediumOrchid
        MediumPurple2
        DarkGoldenrod
        LightSalmon3
        RosyBrown
        Grey63
        MediumPurple2_af87d7
        MediumPurple1
        Gold3
        DarkKhaki
        NavajoWhite3
        Grey69
        LightSteelBlue3
        LightSteelBlue
        Yellow3
        DarkOliveGreen3_afd75f
        DarkSeaGreen3_afd787
        DarkSeaGreen2
        LightCyan3
        LightSkyBlue1
        GreenYellow
        DarkOliveGreen2
        PaleGreen1_afff87
        DarkSeaGreen2_afffaf
        DarkSeaGreen1
        PaleTurquoise1
        Red3_d70000
        DeepPink3
        DeepPink3_d70087
        Magenta3_d700af
        Magenta3_d700d7
        Magenta2
        DarkOrange3_d75f00
        IndianRed_d75f5f
        HotPink3_d75f87
        HotPink2
        Orchid
        MediumOrchid1
        Orange3
        LightSalmon3_d7875f
        LightPink3
        Pink3
        Plum3
        Violet
        Gold3_d7af00
        LightGoldenrod3
        Tan
        MistyRose3
        Thistle3
        Plum2
        Yellow3_d7d700
        Khaki3
        LightGoldenrod2
        LightYellow3
        Grey84
        LightSteelBlue1
        Yellow2
        DarkOliveGreen1
        DarkOliveGreen1_d7ff87
        DarkSeaGreen1_d7ffaf
        Honeydew2
        LightCyan1
        Red1
        DeepPink2
        DeepPink1_ff0087
        DeepPink1_ff00af
        Magenta2_ff00d7
        Magenta1
        OrangeRed1
        IndianRed1_ff5f5f
        IndianRed1_ff5f87
        HotPink_ff5faf
        HotPink_ff5fd7
        MediumOrchid1_ff5fff
        DarkOrange
        Salmon1
        LightCoral
        PaleVioletRed1
        Orchid2
        Orchid1
        Orange1
        SandyBrown
        LightSalmon1
        LightPink1
        Pink1
        Plum1
        Gold1
        LightGoldenrod2_ffd75f
        LightGoldenrod2_ffd787
        NavajoWhite1
        MistyRose1
        Thistle1
        Yellow1
        LightGoldenrod1
        Khaki1
        Wheat1
        Cornsilk1
        Grey100
        Grey3
        Grey7
        Grey11
        Grey15
        Grey19
        Grey23
        Grey27
        Grey30
        Grey35
        Grey39
        Grey42
        Grey46
        Grey50
        Grey54
        Grey58
        Grey62
        Grey66
        Grey70
        Grey74
        Grey78
        Grey82
        Grey85
        Grey89
        Grey93
    End Enum

    Sub Initialize255() 'Windows only
        Dim handle = GetStdHandle(-11)
        Wdbg("Integer pointer {0}", handle)
        Dim mode As Integer
        GetConsoleMode(handle, mode)
        Wdbg("Mode: {0}", mode)
        If Not mode = 7 Then
            SetConsoleMode(handle, mode Or &H4)
            Wdbg("Added support for VT escapes.")
        End If
    End Sub
    Function GetEsc() As Char
        Return ChrW(&H1B) 'ESC
    End Function

End Module
