
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Namespace Misc.Animations.FaderBack
    Public Class FaderBackSettings

        Friend RandomDriver As Random
        Private _faderBackDelay As Integer = 50
        Private _faderBackFadeOutDelay As Integer = 3000
        Private _faderBackMaxSteps As Integer = 25
        Private _faderBackMinimumRedColorLevel As Integer = 0
        Private _faderBackMinimumGreenColorLevel As Integer = 0
        Private _faderBackMinimumBlueColorLevel As Integer = 0
        Private _faderBackMaximumRedColorLevel As Integer = 255
        Private _faderBackMaximumGreenColorLevel As Integer = 255
        Private _faderBackMaximumBlueColorLevel As Integer = 255

        ''' <summary>
        ''' [FaderBack] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property FaderBackDelay As Integer
            Get
                Return _faderBackDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 50
                _faderBackDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] How many milliseconds to wait before fading the text out?
        ''' </summary>
        Public Property FaderBackFadeOutDelay As Integer
            Get
                Return _faderBackFadeOutDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 3000
                _faderBackFadeOutDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] How many fade steps to do?
        ''' </summary>
        Public Property FaderBackMaxSteps As Integer
            Get
                Return _faderBackMaxSteps
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 25
                _faderBackMaxSteps = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The minimum red color level (true color)
        ''' </summary>
        Public Property FaderBackMinimumRedColorLevel As Integer
            Get
                Return _faderBackMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _faderBackMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The minimum green color level (true color)
        ''' </summary>
        Public Property FaderBackMinimumGreenColorLevel As Integer
            Get
                Return _faderBackMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _faderBackMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The minimum blue color level (true color)
        ''' </summary>
        Public Property FaderBackMinimumBlueColorLevel As Integer
            Get
                Return _faderBackMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _faderBackMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The maximum red color level (true color)
        ''' </summary>
        Public Property FaderBackMaximumRedColorLevel As Integer
            Get
                Return _faderBackMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _faderBackMinimumRedColorLevel Then value = _faderBackMinimumRedColorLevel
                If value > 255 Then value = 255
                _faderBackMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The maximum green color level (true color)
        ''' </summary>
        Public Property FaderBackMaximumGreenColorLevel As Integer
            Get
                Return _faderBackMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _faderBackMinimumGreenColorLevel Then value = _faderBackMinimumGreenColorLevel
                If value > 255 Then value = 255
                _faderBackMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The maximum blue color level (true color)
        ''' </summary>
        Public Property FaderBackMaximumBlueColorLevel As Integer
            Get
                Return _faderBackMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _faderBackMinimumBlueColorLevel Then value = _faderBackMinimumBlueColorLevel
                If value > 255 Then value = 255
                _faderBackMaximumBlueColorLevel = value
            End Set
        End Property

    End Class
End Namespace
