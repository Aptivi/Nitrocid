
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

Imports System.IO

Public Module PlaceParse

    'Placeholders (strings)
    Private ReadOnly userplace As String = "<user>"
    Private ReadOnly sdateplace As String = "<shortdate>"
    Private ReadOnly ldateplace As String = "<longdate>"
    Private ReadOnly stimeplace As String = "<shorttime>"
    Private ReadOnly ltimeplace As String = "<longtime>"
    Private ReadOnly dateplace As String = "<date>"
    Private ReadOnly timeplace As String = "<time>"
    Private ReadOnly tzplace As String = "<timezone>"
    Private ReadOnly stzplace As String = "<summertimezone>"
    Private ReadOnly sysplace As String = "<system>"

    ''' <summary>
    ''' Probes the placeholders found in string
    ''' </summary>
    ''' <param name="text">Specified string</param>
    ''' <returns>A string that has the parsed placeholders</returns>
    Public Function ProbePlaces(ByVal text As String) As String

        EventManager.RaisePlaceholderParsing(text)
        Try
            Wdbg("I", "Parsing text for placeholders...")
            If text.Contains(userplace) Then
                Wdbg("I", "Username placeholder found.")
                text = text.Replace(userplace, signedinusrnm)
            End If
            If text.Contains(sdateplace) Then
                Wdbg("I", "Short Date placeholder found.")
                text = text.Replace(sdateplace, KernelDateTime.ToShortDateString)
            End If
            If text.Contains(ldateplace) Then
                Wdbg("I", "Long Date placeholder found.")
                text = text.Replace(ldateplace, KernelDateTime.ToLongDateString)
            End If
            If text.Contains(stimeplace) Then
                Wdbg("I", "Short Time placeholder found.")
                text = text.Replace(stimeplace, KernelDateTime.ToShortTimeString)
            End If
            If text.Contains(ltimeplace) Then
                Wdbg("I", "Long Time placeholder found.")
                text = text.Replace(ltimeplace, KernelDateTime.ToShortDateString)
            End If
            If text.Contains(dateplace) Then
                Wdbg("I", "Rendered Date placeholder found.")
                text = text.Replace(dateplace, RenderDate)
            End If
            If text.Contains(timeplace) Then
                Wdbg("I", "Rendered Time placeholder found.")
                text = text.Replace(timeplace, RenderTime)
            End If
            If text.Contains(tzplace) Then
                Wdbg("I", "Standard Time Zone placeholder found.")
                text = text.Replace(tzplace, TimeZone.CurrentTimeZone.StandardName)
            End If
            If text.Contains(stzplace) Then
                Wdbg("I", "Summer Time Zone placeholder found.")
                text = text.Replace(stzplace, TimeZone.CurrentTimeZone.DaylightName)
            End If
            If text.Contains(sysplace) Then
                Wdbg("I", "System placeholder found.")
                text = text.Replace(sysplace, EnvironmentOSType)
            End If
            EventManager.RaisePlaceholderParsed(text)
        Catch nre As NullReferenceException
            Dim STrace As New StackTrace(True)
            Dim Source As String = Path.GetFileName(STrace.GetFrame(1).GetFileName)
            Dim LineNum As String = STrace.GetFrame(1).GetFileLineNumber
            WStkTrc(nre)
            If DebugMode = True Then
                W(DoTranslation("There is a null reference exception on {0}:{1} - Stack trace:", currentLang) + vbNewLine + nre.StackTrace, True, ColTypes.Err, Source, LineNum)
            Else
                W(DoTranslation("There is a null reference exception on {0}:{1}", currentLang), True, ColTypes.Err, Source, LineNum)
            End If
        Catch ex As Exception
            WStkTrc(ex)
            If DebugMode = True Then
                W(DoTranslation("Error trying to parse placeholders. {0} - Stack trace:", currentLang) + vbNewLine + ex.StackTrace, True, ColTypes.Err, ex.Message)
            Else
                W(DoTranslation("Error trying to parse placeholders. {0}", currentLang), True, ColTypes.Err, ex.Message)
            End If
        End Try
        Return text

    End Function

End Module
