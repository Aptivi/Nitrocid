
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

Imports Newtonsoft.Json.Linq
Imports KS.Files
Imports KS.Network.Transfer
Imports KS.Misc.Splash
Imports System.IO

Namespace Kernel.Updates
    Public Module UpdateManager

        ''' <summary>
        ''' Fetches the GitHub repo to see if there are any updates
        ''' </summary>
        ''' <returns>A kernel update instance</returns>
        Public Function FetchKernelUpdates() As KernelUpdate
            Try
                'Because api.github.com requires the UserAgent header to be put, else, 403 error occurs. Fortunately for us, "Aptivi" is enough.
                WClient.DefaultRequestHeaders.Add("User-Agent", "Aptivi")

                'Populate the following variables with information
                Dim UpdateStr As String = DownloadString("https://api.github.com/repos/Aptivi/Kernel-Simulator/releases")
                Dim UpdateToken As JToken = JToken.Parse(UpdateStr)
                Dim UpdateInstance As New KernelUpdate(UpdateToken)

                'Return the update instance
                WClient.DefaultRequestHeaders.Remove("User-Agent")
                Return UpdateInstance
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to check for updates: {0}", ex.Message)
                WStkTrc(ex)
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' Prompt for checking for kernel updates
        ''' </summary>
        Sub CheckKernelUpdates()
            'Check to see if we're running from Ubuntu PPA
            If ExecPath.StartsWith("/usr/lib/ks") Then
                ReportProgress(DoTranslation("Use apt to update Kernel Simulator."), 10, ColTypes.Error)
                Exit Sub
            End If

            'Check for updates now
            ReportProgress(DoTranslation("Checking for system updates..."), 10, ColTypes.Neutral)
            Dim AvailableUpdate As KernelUpdate = FetchKernelUpdates()
            If AvailableUpdate IsNot Nothing Then
                If Not AvailableUpdate.Updated Then
                    ReportProgress(DoTranslation("Found new version: "), 10, ColTypes.ListEntry)
                    ReportProgress(AvailableUpdate.UpdateVersion.ToString, 10, ColTypes.ListValue)
                    If AutoDownloadUpdate Then
                        DownloadFile(AvailableUpdate.UpdateURL.ToString, Path.Combine(ExecPath, "update.rar"))
                        ReportProgress(DoTranslation("Downloaded the update successfully!"), 10, ColTypes.Success)
                    Else
                        ReportProgress(DoTranslation("You can download it at: "), 10, ColTypes.ListEntry)
                        ReportProgress(AvailableUpdate.UpdateURL.ToString, 10, ColTypes.ListValue)
                    End If
                Else
                    ReportProgress(DoTranslation("You're up to date!"), 10, ColTypes.Neutral)
                End If
            ElseIf AvailableUpdate Is Nothing Then
                ReportProgress(DoTranslation("Failed to check for updates."), 10, ColTypes.Error)
            End If
        End Sub

    End Module
End Namespace
