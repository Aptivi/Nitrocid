
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

Imports System.Runtime.InteropServices
Imports Newtonsoft.Json.Linq

Namespace ConsoleBase
    Public Module Color255

        ''' <summary>
        ''' The 255 console colors data JSON token to get information about these colors
        ''' </summary>
        Public ReadOnly ColorDataJson As JToken = JToken.Parse(My.Resources.ConsoleColorsData)

        ''' <summary>
        ''' [Windows] Sets console mode
        ''' </summary>
        ''' <param name="hConsoleHandle">Console Handle</param>
        ''' <param name="mode">Mode</param>
        ''' <returns>True if succeeded, false if failed</returns>
        <DllImport("kernel32.dll", SetLastError:=True)>
        Private Function SetConsoleMode(hConsoleHandle As IntPtr, mode As Integer) As Boolean
        End Function

        ''' <summary>
        ''' [Windows] Gets console mode
        ''' </summary>
        ''' <param name="handle">Console handle</param>
        ''' <param name="mode">Mode</param>
        ''' <returns>True if succeeded, false if failed</returns>
        <DllImport("kernel32.dll", SetLastError:=True)>
        Private Function GetConsoleMode(handle As IntPtr, <Out()> ByRef mode As Integer) As Boolean
        End Function

        ''' <summary>
        ''' [Windows] Gets console handle
        ''' </summary>
        ''' <param name="handle">Handle number</param>
        ''' <returns>True if succeeded, false if failed</returns>
        <DllImport("kernel32.dll", SetLastError:=True)>
        Private Function GetStdHandle(handle As Integer) As IntPtr
        End Function

        ''' <summary>
        ''' All 255 console colors
        ''' </summary>
        Public Enum ConsoleColors As Integer
            Black
            DarkRed
            DarkGreen
            DarkYellow
            DarkBlue
            DarkMagenta
            DarkCyan
            Gray
            DarkGray
            Red
            Green
            Yellow
            Blue
            Magenta
            Cyan
            White
            Grey0
            NavyBlue
            DarkBlue_000087
            Blue3
            Blue3_d7
            Blue1
            DarkGreen_005f00
            DeepSkyBlue4_005f5f
            DeepSkyBlue4_005f87
            DeepSkyBlue4_005faf
            DodgerBlue3
            DodgerBlue2
            Green4
            SpringGreen4
            Turquoise4
            DeepSkyBlue3_0087af
            DeepSkyBlue3_0087d7
            DodgerBlue1
            Green3_af00
            SpringGreen3_00af5f
            DarkCyan_00af87
            LightSeaGreen
            DeepSkyBlue2
            DeepSkyBlue1
            Green3_00d700
            SpringGreen3_00d75f
            SpringGreen2_00d787
            Cyan3
            DarkTurquoise
            Turquoise2
            Green1
            SpringGreen2
            SpringGreen1
            MediumSpringGreen
            Cyan2
            Cyan1
            DarkRed_5f0000
            DeepPink4
            Purple4_5f0087
            Purple4_5f00af
            Purple3
            BlueViolet
            Orange4
            Grey37
            MediumPurple4
            SlateBlue3_5f5faf
            SlateBlue3_5f5fd7
            RoyalBlue1
            Chartreuse4
            DarkSeaGreen4
            PaleTurquoise4
            SteelBlue
            SteelBlue3
            CornflowerBlue
            Chartreuse3
            DarkSeaGreen4_5faf5f
            CadetBlue_5faf87
            CadetBlue_5fafaf
            SkyBlue3
            SteelBlue1_5fafff
            Chartreuse3_5fd700
            PaleGreen3
            SeaGreen3
            Aquamarine3
            MediumTurquoise
            SteelBlue1_5fd7ff
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
            def = 999
        End Enum

        ''' <summary>
        ''' [Windows] Initializes 255 color support
        ''' </summary>
        Sub Initialize255()
            Dim handle = GetStdHandle(-11)
            Wdbg(DebugLevel.I, "Integer pointer {0}", handle)
            Dim mode As Integer
            GetConsoleMode(handle, mode)
            Wdbg(DebugLevel.I, "Mode: {0}", mode)
            If Not mode = 7 Then
                SetConsoleMode(handle, mode Or &H4)
                Wdbg(DebugLevel.I, "Added support for VT escapes.")
            End If
        End Sub

        ''' <summary>
        ''' A simplification for <see cref="Convert.ToChar(Integer)"/> function to return the ESC character
        ''' </summary>
        ''' <returns>ESC</returns>
        Public Function GetEsc() As Char
            Return Convert.ToChar(&H1B)
        End Function

    End Module
End Namespace
