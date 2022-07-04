
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

Imports System.IO
Imports System.Net.NetworkInformation
Imports System.Text
Imports Newtonsoft.Json.Linq
Imports KS.Misc.Configuration

Namespace Network
    Public Module NetworkTools

        'Variables
        Public DownloadRetries As Integer = 3
        Public UploadRetries As Integer = 3
        Public PingTimeout As Integer = 60000
        Friend TransferFinished As Boolean

        Public Enum SpeedDialType
            ''' <summary>
            ''' FTP speed dial
            ''' </summary>
            FTP
            ''' <summary>
            ''' SFTP speed dial
            ''' </summary>
            SFTP
        End Enum

        ''' <summary>
        ''' Checks to see if the network is available
        ''' </summary>
        Public ReadOnly Property NetworkAvailable As Boolean
            Get
                Return NetworkInterface.GetIsNetworkAvailable()
            End Get
        End Property

        ''' <summary>
        ''' Pings an address
        ''' </summary>
        ''' <param name="Address">Target address</param>
        ''' <returns>A ping reply status</returns>
        Public Function PingAddress(Address As String) As PingReply
            Dim Pinger As New Ping
            Dim PingerOpts As New PingOptions With {.DontFragment = True}
            Dim PingBuffer() As Byte = Encoding.ASCII.GetBytes("Kernel Simulator")
            Dim Timeout As Integer = PingTimeout '60 seconds = 1 minute. timeout of Pinger.Send() takes milliseconds.
            Return PingAddress(Address, Timeout, PingBuffer)
        End Function

        ''' <summary>
        ''' Pings an address
        ''' </summary>
        ''' <param name="Address">Target address</param>
        ''' <returns>A ping reply status</returns>
        Public Function PingAddress(Address As String, Timeout As Integer) As PingReply
            Dim Pinger As New Ping
            Dim PingerOpts As New PingOptions With {.DontFragment = True}
            Dim PingBuffer() As Byte = Encoding.ASCII.GetBytes("Kernel Simulator")
            Return PingAddress(Address, Timeout, PingBuffer)
        End Function

        ''' <summary>
        ''' Pings an address
        ''' </summary>
        ''' <param name="Address">Target address</param>
        ''' <returns>A ping reply status</returns>
        Public Function PingAddress(Address As String, Timeout As Integer, Buffer() As Byte) As PingReply
            Dim Pinger As New Ping
            Dim PingerOpts As New PingOptions With {.DontFragment = True}
            Return Pinger.Send(Address, Timeout, Buffer, PingerOpts)
        End Function

        ''' <summary>
        ''' Changes host name
        ''' </summary>
        ''' <param name="NewHost">New host name</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function ChangeHostname(NewHost As String) As Boolean
            Try
                HostName = NewHost
                Dim Token As JToken = GetConfigCategory(ConfigCategory.Login)
                SetConfigValue(ConfigCategory.Login, Token, "Host Name", HostName)
                Return True
            Catch ex As Exception
                WStkTrc(ex)
                Wdbg(DebugLevel.E, "Failed to change hostname: {0}", ex.Message)
                Throw New Exceptions.HostnameException(DoTranslation("Failed to change host name: {0}"), ex, ex.Message)
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Adds an entry to speed dial
        ''' </summary>
        ''' <param name="Address">A speed dial address</param>
        ''' <param name="Port">A speed dial port</param>
        ''' <param name="User">A speed dial username</param>
        ''' <param name="EncryptionMode">A speed dial encryption mode</param>
        ''' <param name="SpeedDialType">Speed dial type</param>
        ''' <param name="ThrowException">Optionally throw exception</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function AddEntryToSpeedDial(Address As String, Port As Integer, User As String, SpeedDialType As SpeedDialType, Optional EncryptionMode As FtpEncryptionMode = FtpEncryptionMode.None, Optional ThrowException As Boolean = True) As Boolean
            Dim PathName As String = If(SpeedDialType = SpeedDialType.SFTP, "SFTPSpeedDial", "FTPSpeedDial")
            Dim SpeedDialEnum As KernelPathType = [Enum].Parse(GetType(KernelPathType), PathName)
            MakeFile(GetKernelPath(SpeedDialEnum), False)
            Dim SpeedDialJsonContent As String = File.ReadAllText(GetKernelPath(SpeedDialEnum))
            If SpeedDialJsonContent.StartsWith("[") Then
                ConvertSpeedDialEntries(SpeedDialType)
                SpeedDialJsonContent = File.ReadAllText(GetKernelPath(SpeedDialEnum))
            End If
            Dim SpeedDialToken As JObject = JObject.Parse(If(Not String.IsNullOrEmpty(SpeedDialJsonContent), SpeedDialJsonContent, "{}"))
            If SpeedDialToken(Address) Is Nothing Then
                Dim NewSpeedDial As New JObject(New JProperty("Address", Address),
                                                New JProperty("Port", Port),
                                                New JProperty("User", User),
                                                New JProperty("Type", SpeedDialType),
                                                New JProperty("FTP Encryption Mode", EncryptionMode))
                SpeedDialToken.Add(Address, NewSpeedDial)
                File.WriteAllText(GetKernelPath(SpeedDialEnum), JsonConvert.SerializeObject(SpeedDialToken, Formatting.Indented))
                Return True
            Else
                If ThrowException Then
                    If SpeedDialType = SpeedDialType.FTP Then
                        Throw New Exceptions.FTPNetworkException(DoTranslation("Entry already exists."))
                    ElseIf SpeedDialType = SpeedDialType.SFTP Then
                        Throw New Exceptions.SFTPNetworkException(DoTranslation("Entry already exists."))
                    End If
                End If
                Return False
            End If
        End Function

        ''' <summary>
        ''' Lists all speed dial entries
        ''' </summary>
        ''' <param name="SpeedDialType">Speed dial type</param>
        ''' <returns>A list</returns>
        Public Function ListSpeedDialEntries(SpeedDialType As SpeedDialType) As Dictionary(Of String, JToken)
            Dim PathName As String = If(SpeedDialType = SpeedDialType.SFTP, "SFTPSpeedDial", "FTPSpeedDial")
            Dim SpeedDialEnum As KernelPathType = [Enum].Parse(GetType(KernelPathType), PathName)
            MakeFile(GetKernelPath(SpeedDialEnum), False)
            Dim SpeedDialJsonContent As String = File.ReadAllText(GetKernelPath(SpeedDialEnum))
            If SpeedDialJsonContent.StartsWith("[") Then
                ConvertSpeedDialEntries(SpeedDialType)
                SpeedDialJsonContent = File.ReadAllText(GetKernelPath(SpeedDialEnum))
            End If
            Dim SpeedDialToken As JObject = JObject.Parse(If(Not String.IsNullOrEmpty(SpeedDialJsonContent), SpeedDialJsonContent, "{}"))
            Dim SpeedDialEntries As New Dictionary(Of String, JToken)
            For Each SpeedDialAddress In SpeedDialToken.Properties
                SpeedDialEntries.Add(SpeedDialAddress.Name, SpeedDialAddress.Value)
            Next
            Return SpeedDialEntries
        End Function

        ''' <summary>
        ''' Convert speed dial entries from the old jsonified version (pre-0.0.16 RC1) to the new jsonified version
        ''' </summary>
        ''' <param name="SpeedDialType">Speed dial type</param>
        Public Sub ConvertSpeedDialEntries(SpeedDialType As SpeedDialType)
            Dim PathName As String = If(SpeedDialType = SpeedDialType.SFTP, "SFTPSpeedDial", "FTPSpeedDial")
            Dim SpeedDialEnum As KernelPathType = [Enum].Parse(GetType(KernelPathType), PathName)
            Dim SpeedDialJsonContent As String = File.ReadAllText(GetKernelPath(SpeedDialEnum))
            Dim SpeedDialToken As JArray = JArray.Parse(If(Not String.IsNullOrEmpty(SpeedDialJsonContent), SpeedDialJsonContent, "[]"))
            File.Delete(GetKernelPath(SpeedDialEnum))
            For Each SpeedDialEntry As String In SpeedDialToken
                Dim ChosenLineSeparation As String() = SpeedDialEntry.Split(",")
                Dim Address As String = ChosenLineSeparation(0)
                Dim Port As String = ChosenLineSeparation(1)
                Dim Username As String = ChosenLineSeparation(2)
                Dim Encryption As FtpEncryptionMode = If(SpeedDialType = SpeedDialType.FTP, [Enum].Parse(GetType(FtpEncryptionMode), ChosenLineSeparation(3)), FtpEncryptionMode.None)
                AddEntryToSpeedDial(Address, Port, Username, SpeedDialType, Encryption, False)
            Next
        End Sub

        ''' <summary>
        ''' Gets the filename from the URL
        ''' </summary>
        ''' <param name="Url">The target URL that contains the filename</param>
        Public Function GetFilenameFromUrl(Url As String) As String
            Dim FileName As String = Url.Split("/").Last()
            Wdbg(DebugLevel.I, "Prototype Filename: {0}", FileName)
            If FileName.Contains("?"c) Then
                FileName = FileName.Remove(FileName.IndexOf("?"c))
            End If
            Wdbg(DebugLevel.I, "Finished Filename: {0}", FileName)
            Return FileName
        End Function

    End Module
End Namespace
