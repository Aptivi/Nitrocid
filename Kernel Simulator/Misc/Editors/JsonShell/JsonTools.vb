
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
Imports System.Text
Imports Newtonsoft.Json.Linq
Imports Extensification.StreamReaderExts
Imports System.Threading

Namespace Misc.JsonShell
    Public Module JsonTools

        ''' <summary>
        ''' Opens the JSON file
        ''' </summary>
        ''' <param name="File">Target file. We recommend you to use <see cref="NeutralizePath(String, Boolean)"></see> to neutralize path.</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function JsonShell_OpenJsonFile(File As String) As Boolean
            Try
                Wdbg(DebugLevel.I, "Trying to open file {0}...", File)
                JsonShell_FileStream = New FileStream(File, FileMode.Open)
                Dim JsonFileReader As New StreamReader(JsonShell_FileStream)
                Dim JsonFileContents As String = JsonFileReader.ReadToEndAndSeek
                JsonShell_FileToken = JToken.Parse(If(Not String.IsNullOrWhiteSpace(JsonFileContents), JsonFileContents, "{}"))
                JsonShell_FileTokenOrig = JToken.Parse(If(Not String.IsNullOrWhiteSpace(JsonFileContents), JsonFileContents, "{}"))
                Wdbg(DebugLevel.I, "File {0} is open. Length: {1}, Pos: {2}", File, JsonShell_FileStream.Length, JsonShell_FileStream.Position)
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Open file {0} failed: {1}", File, ex.Message)
                WStkTrc(ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Closes text file
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function JsonShell_CloseTextFile() As Boolean
            Try
                Wdbg(DebugLevel.I, "Trying to close file...")
                JsonShell_FileStream.Close()
                JsonShell_FileStream = Nothing
                Wdbg(DebugLevel.I, "File is no longer open.")
                JsonShell_FileToken = JToken.Parse("{}")
                JsonShell_FileTokenOrig = JToken.Parse("{}")
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Closing file failed: {0}", ex.Message)
                WStkTrc(ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Saves JSON file
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function JsonShell_SaveFile(ClearJson As Boolean) As Boolean
            Return JsonShell_SaveFile(ClearJson, JsonShell_Formatting)
        End Function

        ''' <summary>
        ''' Saves JSON file
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function JsonShell_SaveFile(ClearJson As Boolean, Formatting As Formatting) As Boolean
            Try
                Wdbg(DebugLevel.I, "Trying to save file...")
                JsonShell_FileStream.SetLength(0)
                Wdbg(DebugLevel.I, "Length set to 0.")
                Dim FileLinesByte() As Byte = Encoding.Default.GetBytes(JsonConvert.SerializeObject(JsonShell_FileToken, Formatting))
                Wdbg(DebugLevel.I, "Converted lines to bytes. Length: {0}", FileLinesByte.Length)
                JsonShell_FileStream.Write(FileLinesByte, 0, FileLinesByte.Length)
                JsonShell_FileStream.Flush()
                Wdbg(DebugLevel.I, "File is saved.")
                If ClearJson Then
                    JsonShell_FileToken = JToken.Parse("{}")
                End If
                JsonShell_FileTokenOrig = JToken.Parse("{}")
                JsonShell_FileTokenOrig = JsonShell_FileToken
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Saving file failed: {0}", ex.Message)
                WStkTrc(ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Handles autosave
        ''' </summary>
        Public Sub JsonShell_HandleAutoSaveJsonFile()
            If JsonShell_AutoSaveFlag Then
                Try
                    Thread.Sleep(JsonShell_AutoSaveInterval * 1000)
                    If JsonShell_FileStream IsNot Nothing Then
                        JsonShell_SaveFile(False)
                    End If
                Catch ex As Exception
                    WStkTrc(ex)
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Was JSON edited?
        ''' </summary>
        Function JsonShell_WasJsonEdited() As Boolean
            Return Not JToken.DeepEquals(JsonShell_FileToken, JsonShell_FileTokenOrig)
        End Function

        ''' <summary>
        ''' Gets a property in the JSON file
        ''' </summary>
        ''' <param name="[Property]">The property. You can use JSONPath.</param>
        Public Function JsonShell_GetProperty([Property] As String) As JToken
            If JsonShell_FileStream IsNot Nothing Then
                Dim TargetToken As JToken = JsonShell_FileToken.SelectToken([Property])
                If TargetToken IsNot Nothing Then
                    Return TargetToken
                Else
                    Throw New ArgumentOutOfRangeException(NameOf([Property]), [Property], DoTranslation("The property inside the JSON file isn't found."))
                End If
            Else
                Throw New InvalidOperationException(DoTranslation("The JSON editor hasn't opened a file stream yet."))
            End If
        End Function

        ''' <summary>
        ''' Adds a new property to the current JSON file
        ''' </summary>
        ''' <param name="ParentProperty">Where to place the new property?</param>
        ''' <param name="Key">New property</param>
        ''' <param name="Value">The value for the new property</param>
        Public Sub JsonShell_AddNewProperty(ParentProperty As String, Key As String, Value As JToken)
            Wdbg(DebugLevel.I, "Old file lines: {0}", JsonShell_FileToken.Count)
            Dim TargetToken As JToken = JsonShell_GetProperty(ParentProperty)
            Dim TokenObject As JObject = TargetToken
            TokenObject.Add(Key, Value)
            Wdbg(DebugLevel.I, "New file lines: {0}", JsonShell_FileToken.Count)
        End Sub

        ''' <summary>
        ''' Removes a property from the current JSON file
        ''' </summary>
        ''' <param name="[Property]">The property. You can use JSONPath.</param>
        Public Sub JsonShell_RemoveProperty([Property] As String)
            Wdbg(DebugLevel.I, "Old file lines: {0}", JsonShell_FileToken.Count)
            Dim TargetToken As JToken = JsonShell_GetProperty([Property])
            TargetToken.Parent.Remove()
            Wdbg(DebugLevel.I, "New file lines: {0}", JsonShell_FileToken.Count)
        End Sub

        ''' <summary>
        ''' Serializes the property to the string
        ''' </summary>
        ''' <param name="[Property]">The property. You can use JSONPath.</param>
        Public Function JsonShell_SerializeToString([Property] As String) As String
            Dim TargetToken As JToken = JsonShell_GetProperty([Property])
            Return JsonConvert.SerializeObject(TargetToken, Formatting.Indented)
        End Function

    End Module
End Namespace