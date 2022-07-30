
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

Imports System.Globalization

Namespace TimeDate
    Public Module TimeDateRenderers

        ''' <summary>
        ''' Renders the current time based on kernel config (long or short) and current culture
        ''' </summary>
        ''' <returns>A long or short time</returns>
        Public Function RenderTime() As String
            If LongTimeDate Then
                Return KernelDateTime.ToString(CurrentCult.DateTimeFormat.LongTimePattern, CurrentCult)
            Else
                Return KernelDateTime.ToString(CurrentCult.DateTimeFormat.ShortTimePattern, CurrentCult)
            End If
        End Function

        ''' <summary>
        ''' Renders the current time based on kernel config (long or short) and current culture
        ''' </summary>
        ''' <param name="FormatType">Date/time format type</param>
        ''' <returns>A long or short time</returns>
        Public Function RenderTime(FormatType As FormatType) As String
            If FormatType = FormatType.Long Then
                Return KernelDateTime.ToString(CurrentCult.DateTimeFormat.LongTimePattern, CurrentCult)
            Else
                Return KernelDateTime.ToString(CurrentCult.DateTimeFormat.ShortTimePattern, CurrentCult)
            End If
        End Function

        ''' <summary>
        ''' Renders the current time based on specified culture
        ''' </summary>
        ''' <param name="Cult">A culture.</param>
        ''' <returns>A time</returns>
        Public Function RenderTime(Cult As CultureInfo) As String
            If LongTimeDate Then
                Return KernelDateTime.ToString(Cult.DateTimeFormat.LongTimePattern, Cult)
            Else
                Return KernelDateTime.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult)
            End If
        End Function

        ''' <summary>
        ''' Renders the current time based on specified culture
        ''' </summary>
        ''' <param name="Cult">A culture.</param>
        ''' <param name="FormatType">Date/time format type</param>
        ''' <returns>A time</returns>
        Public Function RenderTime(Cult As CultureInfo, FormatType As FormatType) As String
            If FormatType = FormatType.Long Then
                Return KernelDateTime.ToString(Cult.DateTimeFormat.LongTimePattern, Cult)
            Else
                Return KernelDateTime.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult)
            End If
        End Function

        ''' <summary>
        ''' Renders the time based on specified time using the kernel config (long or short) and current culture
        ''' </summary>
        ''' <param name="DT">Specified time</param>
        ''' <returns>A long or short time</returns>
        Public Function RenderTime(DT As Date) As String
            If LongTimeDate Then
                Return DT.ToString(CurrentCult.DateTimeFormat.LongTimePattern, CurrentCult)
            Else
                Return DT.ToString(CurrentCult.DateTimeFormat.ShortTimePattern, CurrentCult)
            End If
        End Function

        ''' <summary>
        ''' Renders the time based on specified time using the kernel config (long or short) and current culture
        ''' </summary>
        ''' <param name="DT">Specified time</param>
        ''' <param name="FormatType">Date/time format type</param>
        ''' <returns>A long or short time</returns>
        Public Function RenderTime(DT As Date, FormatType As FormatType) As String
            If FormatType = FormatType.Long Then
                Return DT.ToString(CurrentCult.DateTimeFormat.LongTimePattern, CurrentCult)
            Else
                Return DT.ToString(CurrentCult.DateTimeFormat.ShortTimePattern, CurrentCult)
            End If
        End Function

        ''' <summary>
        ''' Renders the time based on specified date and culture using the kernel config (long or short)
        ''' </summary>
        ''' <param name="DT">Specified time</param>
        ''' <param name="Cult">A culture</param>
        ''' <returns>A time</returns>
        Public Function RenderTime(DT As Date, Cult As CultureInfo) As String
            If LongTimeDate Then
                Return DT.ToString(Cult.DateTimeFormat.LongTimePattern, Cult)
            Else
                Return DT.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult)
            End If
        End Function

        ''' <summary>
        ''' Renders the time based on specified date and culture using the kernel config (long or short)
        ''' </summary>
        ''' <param name="DT">Specified time</param>
        ''' <param name="Cult">A culture</param>
        ''' <param name="FormatType">Date/time format type</param>
        ''' <returns>A time</returns>
        Public Function RenderTime(DT As Date, Cult As CultureInfo, FormatType As FormatType) As String
            If FormatType = FormatType.Long Then
                Return DT.ToString(Cult.DateTimeFormat.LongTimePattern, Cult)
            Else
                Return DT.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult)
            End If
        End Function

        ''' <summary>
        ''' Renders the current date based on kernel config (long or short) and current culture
        ''' </summary>
        ''' <returns>A long or short date</returns>
        Public Function RenderDate() As String
            If LongTimeDate Then
                Return KernelDateTime.ToString(CurrentCult.DateTimeFormat.LongDatePattern, CurrentCult)
            Else
                Return KernelDateTime.ToString(CurrentCult.DateTimeFormat.ShortDatePattern, CurrentCult)
            End If
        End Function

        ''' <summary>
        ''' Renders the current date based on kernel config (long or short) and current culture
        ''' </summary>
        ''' <param name="FormatType">Date/time format type</param>
        ''' <returns>A long or short date</returns>
        Public Function RenderDate(FormatType As FormatType) As String
            If FormatType = FormatType.Long Then
                Return KernelDateTime.ToString(CurrentCult.DateTimeFormat.LongDatePattern, CurrentCult)
            Else
                Return KernelDateTime.ToString(CurrentCult.DateTimeFormat.ShortDatePattern, CurrentCult)
            End If
        End Function

        ''' <summary>
        ''' Renders the current date based on specified culture
        ''' </summary>
        ''' <param name="Cult">A culture.</param>
        ''' <returns>A date</returns>
        Public Function RenderDate(Cult As CultureInfo) As String
            If LongTimeDate Then
                Return KernelDateTime.ToString(Cult.DateTimeFormat.LongDatePattern, Cult)
            Else
                Return KernelDateTime.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult)
            End If
        End Function

        ''' <summary>
        ''' Renders the current date based on specified culture
        ''' </summary>
        ''' <param name="Cult">A culture.</param>
        ''' <param name="FormatType">Date/time format type</param>
        ''' <returns>A date</returns>
        Public Function RenderDate(Cult As CultureInfo, FormatType As FormatType) As String
            If FormatType = FormatType.Long Then
                Return KernelDateTime.ToString(Cult.DateTimeFormat.LongDatePattern, Cult)
            Else
                Return KernelDateTime.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult)
            End If
        End Function

        ''' <summary>
        ''' Renders the date based on specified date using the kernel config (long or short) and current culture
        ''' </summary>
        ''' <param name="DT">Specified date</param>
        ''' <returns>A long or short date</returns>
        Public Function RenderDate(DT As Date) As String
            If LongTimeDate Then
                Return DT.ToString(CurrentCult.DateTimeFormat.LongDatePattern, CurrentCult)
            Else
                Return DT.ToString(CurrentCult.DateTimeFormat.ShortDatePattern, CurrentCult)
            End If
        End Function

        ''' <summary>
        ''' Renders the date based on specified date using the kernel config (long or short) and current culture
        ''' </summary>
        ''' <param name="DT">Specified date</param>
        ''' <param name="FormatType">Date/time format type</param>
        ''' <returns>A long or short date</returns>
        Public Function RenderDate(DT As Date, FormatType As FormatType) As String
            If FormatType = FormatType.Long Then
                Return DT.ToString(CurrentCult.DateTimeFormat.LongDatePattern, CurrentCult)
            Else
                Return DT.ToString(CurrentCult.DateTimeFormat.ShortDatePattern, CurrentCult)
            End If
        End Function

        ''' <summary>
        ''' Renders the date based on specified date and culture using the kernel config (long or short)
        ''' </summary>
        ''' <param name="DT">Specified date</param>
        ''' <param name="Cult">A culture</param>
        ''' <returns>A date</returns>
        Public Function RenderDate(DT As Date, Cult As CultureInfo) As String
            If LongTimeDate Then
                Return DT.ToString(Cult.DateTimeFormat.LongDatePattern, Cult)
            Else
                Return DT.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult)
            End If
        End Function

        ''' <summary>
        ''' Renders the date based on specified date and culture using the kernel config (long or short)
        ''' </summary>
        ''' <param name="DT">Specified date</param>
        ''' <param name="Cult">A culture</param>
        ''' <param name="FormatType">Date/time format type</param>
        ''' <returns>A date</returns>
        Public Function RenderDate(DT As Date, Cult As CultureInfo, FormatType As FormatType) As String
            If FormatType = FormatType.Long Then
                Return DT.ToString(Cult.DateTimeFormat.LongDatePattern, Cult)
            Else
                Return DT.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult)
            End If
        End Function

        ''' <summary>
        ''' Renders the current time and date based on kernel config (long or short) and current culture
        ''' </summary>
        ''' <returns>A long or short time and date</returns>
        Public Function Render() As String
            If LongTimeDate Then
                Return KernelDateTime.ToString(CurrentCult.DateTimeFormat.FullDateTimePattern, CurrentCult)
            Else
                Return KernelDateTime.ToString(CurrentCult.DateTimeFormat.ShortDatePattern, CurrentCult) + " - " + KernelDateTime.ToString(CurrentCult.DateTimeFormat.ShortTimePattern, CurrentCult)
            End If
        End Function

        ''' <summary>
        ''' Renders the current time and date based on kernel config (long or short) and current culture
        ''' </summary>
        ''' <param name="FormatType">Date/time format type</param>
        ''' <returns>A long or short time and date</returns>
        Public Function Render(FormatType As FormatType) As String
            If FormatType = FormatType.Long Then
                Return KernelDateTime.ToString(CurrentCult.DateTimeFormat.FullDateTimePattern, CurrentCult)
            Else
                Return KernelDateTime.ToString(CurrentCult.DateTimeFormat.ShortDatePattern, CurrentCult) + " - " + KernelDateTime.ToString(CurrentCult.DateTimeFormat.ShortTimePattern, CurrentCult)
            End If
        End Function

        ''' <summary>
        ''' Renders the current time and date based on specified culture
        ''' </summary>
        ''' <param name="Cult">A culture.</param>
        ''' <returns>A time and date</returns>
        Public Function Render(Cult As CultureInfo) As String
            If LongTimeDate Then
                Return KernelDateTime.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult)
            Else
                Return KernelDateTime.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + KernelDateTime.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult)
            End If
        End Function

        ''' <summary>
        ''' Renders the current time and date based on specified culture
        ''' </summary>
        ''' <param name="Cult">A culture.</param>
        ''' <param name="FormatType">Date/time format type</param>
        ''' <returns>A time and date</returns>
        Public Function Render(Cult As CultureInfo, FormatType As FormatType) As String
            If FormatType = FormatType.Long Then
                Return KernelDateTime.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult)
            Else
                Return KernelDateTime.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + KernelDateTime.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult)
            End If
        End Function

        ''' <summary>
        ''' Renders the time and date based on specified time using the kernel config (long or short) and current culture
        ''' </summary>
        ''' <param name="DT">Specified time and date</param>
        ''' <returns>A long or short time and date</returns>
        Public Function Render(DT As Date) As String
            If LongTimeDate Then
                Return DT.ToString(CurrentCult.DateTimeFormat.FullDateTimePattern, CurrentCult)
            Else
                Return DT.ToString(CurrentCult.DateTimeFormat.ShortDatePattern, CurrentCult) + " - " + DT.ToString(CurrentCult.DateTimeFormat.ShortTimePattern, CurrentCult)
            End If
        End Function

        ''' <summary>
        ''' Renders the time and date based on specified time using the kernel config (long or short) and current culture
        ''' </summary>
        ''' <param name="DT">Specified time and date</param>
        ''' <param name="FormatType">Date/time format type</param>
        ''' <returns>A long or short time and date</returns>
        Public Function Render(DT As Date, FormatType As FormatType) As String
            If FormatType = FormatType.Long Then
                Return DT.ToString(CurrentCult.DateTimeFormat.FullDateTimePattern, CurrentCult)
            Else
                Return DT.ToString(CurrentCult.DateTimeFormat.ShortDatePattern, CurrentCult) + " - " + DT.ToString(CurrentCult.DateTimeFormat.ShortTimePattern, CurrentCult)
            End If
        End Function

        ''' <summary>
        ''' Renders the time and date based on specified date and culture using the kernel config (long or short)
        ''' </summary>
        ''' <param name="DT">Specified time and date</param>
        ''' <param name="Cult">A culture</param>
        ''' <returns>A time and date</returns>
        Public Function Render(DT As Date, Cult As CultureInfo) As String
            If LongTimeDate Then
                Return DT.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult)
            Else
                Return DT.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + DT.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult)
            End If
        End Function

        ''' <summary>
        ''' Renders the time and date based on specified date and culture using the kernel config (long or short)
        ''' </summary>
        ''' <param name="DT">Specified time and date</param>
        ''' <param name="Cult">A culture</param>
        ''' <param name="FormatType">Date/time format type</param>
        ''' <returns>A time and date</returns>
        Public Function Render(DT As Date, Cult As CultureInfo, FormatType As FormatType) As String
            If FormatType = FormatType.Long Then
                Return DT.ToString(Cult.DateTimeFormat.FullDateTimePattern, Cult)
            Else
                Return DT.ToString(Cult.DateTimeFormat.ShortDatePattern, Cult) + " - " + DT.ToString(Cult.DateTimeFormat.ShortTimePattern, Cult)
            End If
        End Function

    End Module
End Namespace
