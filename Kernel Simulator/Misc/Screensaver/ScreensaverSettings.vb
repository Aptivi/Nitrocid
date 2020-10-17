
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Public Module ScreensaverSettings

    Public ColorMix255Colors As Boolean                         'Enable 255 color support for ColorMix. Has a higher priority than 16 color support.
    Public Disco255Colors As Boolean                            'Enable 255 color support for Disco. Has a higher priority than 16 color support.
    Public GlitterColor255Colors As Boolean                     'Enable 255 color support for GlitterColor. Has a higher priority than 16 color support.
    Public Lines255Colors As Boolean                            'Enable 255 color support for Lines. Has a higher priority than 16 color support.
    Public Dissolve255Colors As Boolean                         'Enable 255 color support for Dissolve. Has a higher priority than 16 color support.
    Public ColorMixTrueColor As Boolean                         'Enable truecolor support for ColorMix. Has a higher priority than 255 color support.
    Public DiscoTrueColor As Boolean                            'Enable truecolor support for Disco. Has a higher priority than 255 color support.
    Public GlitterColorTrueColor As Boolean                     'Enable truecolor support for GlitterColor. Has a higher priority than 255 color support.
    Public LinesTrueColor As Boolean                            'Enable truecolor support for Lines. Has a higher priority than 255 color support.
    Public DissolveTrueColor As Boolean                         'Enable truecolor support for Dissolve. Has a higher priority than 255 color support.
    Public DiscoCycleColors As Boolean                          'Enable color cycling for Disco
    Public BouncingTextWrite As String = "Kernel Simulator"     'Text for Bouncing Text

End Module
