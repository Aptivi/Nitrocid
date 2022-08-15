
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

Imports KS.Files.Querying
Imports KS.Misc.Screensaver.Customized
Imports KS.Misc.Screensaver

Namespace Shell.Shells.UESH.Commands
    Class SetSaverCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim modPath As String = GetKernelPath(KernelPathType.Mods)
            StringArgs = StringArgs.ToLower
            If Screensavers.ContainsKey(StringArgs) Or CustomSavers.ContainsKey(StringArgs) Then
                SetDefaultScreensaver(StringArgs)
                Write(DoTranslation("{0} is set to default screensaver."), True, ColTypes.Neutral, StringArgs)
            Else
                If FileExists($"{modPath}{StringArgs}") And Not SafeMode Then
                    SetDefaultScreensaver(StringArgs)
                    Write(DoTranslation("{0} is set to default screensaver."), True, ColTypes.Neutral, StringArgs)
                Else
                    Write(DoTranslation("Screensaver {0} not found."), True, ColTypes.Error, StringArgs)
                End If
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            If CustomSavers.Count > 0 Then
                Write(DoTranslation("where customsaver will be") + " {0}", True, ColTypes.Neutral, String.Join(", ", CustomSavers.Keys))
            End If
            Write(DoTranslation("where builtinsaver will be") + " {0}", True, ColTypes.Neutral, String.Join(", ", Screensavers.Keys))
        End Sub

    End Class
End Namespace
