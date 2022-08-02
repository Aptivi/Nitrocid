
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
Imports KS.Files.Operations
Imports KS.Network.Transfer
Imports Newtonsoft.Json.Linq
Imports SharpCompress.Archives
Imports SharpCompress.Archives.Rar
Imports SharpCompress.Common
Imports System.Reflection

Namespace Shell.Commands
    Class RetroKSCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Write(DoTranslation("Checking for updates..."), True, ColTypes.Neutral)

            'Because api.github.com requires the UserAgent header to be put, else, 403 error occurs. Fortunately for us, "Aptivi" is enough.
            WClient.DefaultRequestHeaders.Add("User-Agent", "Aptivi")

            'Populate the following variables with information
            Dim RetroKSStr As String = DownloadString("https://api.github.com/repos/Aptivi/RetroKS/releases")
            Dim RetroKSToken As JToken = JToken.Parse(RetroKSStr)
            Dim SortedVersions As New List(Of KernelUpdateInfo)
            For Each RetroKS As JToken In RetroKSToken
                Dim RetroKSVer As New Version(RetroKS.SelectToken("tag_name").ToString)
                Dim RetroKSURL As String = RetroKS.SelectToken("assets")(0)("browser_download_url")
                Dim RetroKSInfo As New KernelUpdateInfo(RetroKSVer, RetroKSURL)
                SortedVersions.Add(RetroKSInfo)
            Next
            SortedVersions = SortedVersions.OrderByDescending(Function(x) x.UpdateVersion).ToList
            WClient.DefaultRequestHeaders.Remove("User-Agent")

            'Populate paths
            Dim RetroKSPath As String = NeutralizePath("retroks.rar", RetroKSDownloadPath)
            Dim RetroExecKSPath As String = NeutralizePath("RetroKS.exe", RetroKSDownloadPath)

            'Make the directory for RetroKS
            MakeDirectory(RetroKSDownloadPath, False)

            'Check to see if we already have RetroKS installed and up-to-date
            Dim RetroKSBin() As Byte = IO.File.ReadAllBytes(RetroExecKSPath)
            If (FileExists(RetroExecKSPath) AndAlso Assembly.Load(RetroKSBin).GetName.Version < SortedVersions(0).UpdateVersion) Or
                Not FileExists(RetroExecKSPath) Then
                Write(DoTranslation("Downloading version") + " {0}...", True, ColTypes.Neutral, SortedVersions(0).UpdateVersion.ToString)

                'Download RetroKS
                Dim RetroKSURI As Uri = SortedVersions(0).UpdateURL
                DownloadFile(RetroKSURI.ToString, RetroKSPath)

                'Extract it
                Write(DoTranslation("Installing version") + " {0}...", True, ColTypes.Neutral, SortedVersions(0).UpdateVersion.ToString)
                Using archive = RarArchive.Open(RetroKSPath)
                    For Each entry In archive.Entries.Where(Function(e) Not e.IsDirectory)
                        entry.WriteToDirectory(RetroKSDownloadPath, New ExtractionOptions() With {
                            .ExtractFullPath = True,
                            .Overwrite = True
                        })
                    Next entry
                End Using
            End If

            'Now, run the assembly
#If NET48 Then
            Write(DoTranslation("Going back to 2018..."), True, ColTypes.Neutral)
            Assembly.LoadFrom(RetroExecKSPath).EntryPoint.Invoke("", Array.Empty(Of Object))

            'Clear the console
            SetConsoleColor(BackgroundColor, True)
            Console.Clear()
#Else
            Write(DoTranslation("You can't go back to 2018 on .NET 6.0!"), True, ColTypes.Neutral)
#End If
        End Sub

    End Class
End Namespace
