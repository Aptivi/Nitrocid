
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

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Changes system language
    ''' </summary>
    ''' <remarks>
    ''' The system language can be changed either by manually editing configuration files or by using this command. Restart is not required, since printing text, viewing user manual, and updating help list relies on the current language field.
    ''' <br></br>
    ''' <list type="table">
    ''' <listheader>
    ''' <term>Switches</term>
    ''' <description>Description</description>
    ''' </listheader>
    ''' <item>
    ''' <term>-alwaystransliterated</term>
    ''' <description>Always use the transliterated version of the language. Must be transliterable.</description>
    ''' </item>
    ''' <item>
    ''' <term>-alwaystranslated</term>
    ''' <description>Always use the translated version of the language. Must be transliterable.</description>
    ''' </item>
    ''' <item>
    ''' <term>-force</term>
    ''' <description>Forces the language to be set.</description>
    ''' </item>
    ''' <item>
    ''' <term>-list</term>
    ''' <description>Lists the installed languages.</description>
    ''' </item>
    ''' </list>
    ''' <br></br>
    ''' The user must have at least the administrative privileges before they can run the below commands.
    ''' </remarks>
    Class ChLangCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListSwitchesOnly.Contains("-list") Then
                Write(DoTranslation("Available languages:"), True, ColTypes.ListTitle)
                For Each Language As String In Languages.Languages.Keys
                    Write("- {0}: ", False, ColTypes.ListEntry, Language)
                    Write(Languages.Languages(Language).FullLanguageName, True, ColTypes.ListValue)
                Next
            Else
                Dim AlwaysTransliterated, AlwaysTranslated, Force As Boolean
                If ListSwitchesOnly.Contains("-alwaystransliterated") Then AlwaysTransliterated = True
                If ListSwitchesOnly.Contains("-alwaystranslated") Then AlwaysTranslated = True '-alwaystransliterated has higher priority.
                If ListSwitchesOnly.Contains("-force") Then Force = True
                PromptForSetLang(ListArgsOnly(0), Force, AlwaysTransliterated, AlwaysTranslated)
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            Write("  -alwaystransliterated: ", False, ColTypes.ListEntry) : Write(DoTranslation("Always use the transliterated version"), True, ColTypes.ListValue)
            Write("  -alwaystranslated: ", False, ColTypes.ListEntry) : Write(DoTranslation("Always use the translated version"), True, ColTypes.ListValue)
            Write("  -force: ", False, ColTypes.ListEntry) : Write(DoTranslation("Force switching language"), True, ColTypes.ListValue)
            Write("  -list: ", False, ColTypes.ListEntry) : Write(DoTranslation("Lists available languages"), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace
