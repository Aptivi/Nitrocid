
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

Public Module PlaceParse

    'Placeholders (strings)
    Private ReadOnly userplace As String = "<user>"
    Private ReadOnly sdateplace As String = "<shortdate>"
    Private ReadOnly ldateplace As String = "<longdate>"
    Private ReadOnly stimeplace As String = "<shorttime>"
    Private ReadOnly ltimeplace As String = "<longtime>"
    Private ReadOnly tzplace As String = "<timezone>"
    Private ReadOnly stzplace As String = "<summertimezone>"
    Private ReadOnly sysplace As String = "<system>"
    Private ReadOnly nlnplace As String = "<eol>"

    'Probing code
    Public Function ProbePlaces(ByVal text As String) As String

        EventManager.RaisePlaceholderParsing()
        Try
            If text.Contains(userplace) Then text = text.Replace(userplace, signedinusrnm)
            If text.Contains(sdateplace) Then text = text.Replace(sdateplace, FormatDateTime(strKernelTimeDate, DateFormat.ShortDate))
            If text.Contains(ldateplace) Then text = text.Replace(ldateplace, FormatDateTime(strKernelTimeDate, DateFormat.LongDate))
            If text.Contains(stimeplace) Then text = text.Replace(stimeplace, FormatDateTime(strKernelTimeDate, DateFormat.ShortTime))
            If text.Contains(ltimeplace) Then text = text.Replace(ltimeplace, FormatDateTime(strKernelTimeDate, DateFormat.LongTime))
            If text.Contains(tzplace) Then text = text.Replace(tzplace, TimeZone.CurrentTimeZone.StandardName)
            If text.Contains(stzplace) Then text = text.Replace(stzplace, TimeZone.CurrentTimeZone.DaylightName)
            If text.Contains(sysplace) Then text = text.Replace(sysplace, EnvironmentOSType)
            If text.Contains(nlnplace) Then text = text.Replace(nlnplace, vbNewLine)
            EventManager.RaisePlaceholderParsed()
        Catch ex As NullReferenceException
            WStkTrc(ex)
            If DebugMode = True Then
                W(DoTranslation("Error trying to parse placeholders. {0} - Stack trace:", currentLang) + vbNewLine + ex.StackTrace, True, "neutralText", ex.Message)
            Else
                W(DoTranslation("Error trying to parse placeholders. {0}", currentLang), True, "neutralText", ex.Message)
            End If
        End Try
        Return text

    End Function

End Module
