
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

Imports KS.Files.Operations
Imports System.IO
Imports Newtonsoft.Json.Linq

Namespace Misc.Screensaver.Customized
    Public Module CustomSaverTools

        Public CustomSavers As New Dictionary(Of String, CustomSaverInfo)
        Public CustomSaverSettingsToken As JObject

        ''' <summary>
        ''' Initializes and reads the custom saver settings
        ''' </summary>
        Public Sub InitializeCustomSaverSettings()
            MakeFile(GetKernelPath(KernelPathType.CustomSaverSettings), False)
            Dim CustomSaverJsonContent As String = File.ReadAllText(GetKernelPath(KernelPathType.CustomSaverSettings))
            Dim CustomSaverToken As JObject = JObject.Parse(If(Not String.IsNullOrEmpty(CustomSaverJsonContent), CustomSaverJsonContent, "{}"))
            For Each Saver As String In CustomSavers.Keys
                Dim CustomSaverSettings As JObject = TryCast(CustomSaverToken(Saver), JObject)
                If CustomSaverSettings IsNot Nothing Then
                    For Each Setting In CustomSaverSettings
                        CustomSavers(Saver).ScreensaverBase.ScreensaverSettings(Setting.Key) = Setting.Value.ToString
                    Next
                End If
            Next
            CustomSaverSettingsToken = CustomSaverToken
        End Sub

        ''' <summary>
        ''' Saves the custom saver settings
        ''' </summary>
        Public Sub SaveCustomSaverSettings()
            For Each Saver As String In CustomSavers.Keys
                If CustomSavers(Saver).ScreensaverBase IsNot Nothing Then
                    If CustomSavers(Saver).ScreensaverBase.ScreensaverSettings IsNot Nothing Then
                        For Each Setting As String In CustomSavers(Saver).ScreensaverBase.ScreensaverSettings.Keys
                            If Not TryCast(CustomSaverSettingsToken(Saver), JObject).ContainsKey(Setting) Then
                                TryCast(CustomSaverSettingsToken(Saver), JObject).Add(Setting, CustomSavers(Saver).ScreensaverBase.ScreensaverSettings(Setting).ToString)
                            Else
                                CustomSaverSettingsToken(Saver)(Setting) = CustomSavers(Saver).ScreensaverBase.ScreensaverSettings(Setting).ToString
                            End If
                        Next
                    End If
                End If
            Next
            If CustomSaverSettingsToken IsNot Nothing Then File.WriteAllText(GetKernelPath(KernelPathType.CustomSaverSettings), JsonConvert.SerializeObject(CustomSaverSettingsToken, Formatting.Indented))
        End Sub

        ''' <summary>
        ''' Adds a custom screensaver to settings
        ''' </summary>
        ''' <param name="CustomSaver">A custom saver</param>
        ''' <exception cref="Exceptions.NoSuchScreensaverException"></exception>
        Public Sub AddCustomSaverToSettings(CustomSaver As String)
            If Not CustomSavers.ContainsKey(CustomSaver) Then Throw New Exceptions.NoSuchScreensaverException(DoTranslation("Screensaver {0} not found."), CustomSaver)
            If Not CustomSaverSettingsToken.ContainsKey(CustomSaver) Then
                Dim NewCustomSaver As New JObject
                If CustomSavers(CustomSaver).ScreensaverBase IsNot Nothing Then
                    If CustomSavers(CustomSaver).ScreensaverBase.ScreensaverSettings IsNot Nothing Then
                        For Each Setting As String In CustomSavers(CustomSaver).ScreensaverBase.ScreensaverSettings.Keys
                            NewCustomSaver.Add(Setting, CustomSavers(CustomSaver).ScreensaverBase.ScreensaverSettings(Setting).ToString)
                        Next
                        CustomSaverSettingsToken.Add(CustomSaver, NewCustomSaver)
                        If CustomSaverSettingsToken IsNot Nothing Then File.WriteAllText(GetKernelPath(KernelPathType.CustomSaverSettings), JsonConvert.SerializeObject(CustomSaverSettingsToken, Formatting.Indented))
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Removes a custom screensaver from settings
        ''' </summary>
        ''' <param name="CustomSaver">A custom saver</param>
        ''' <exception cref="Exceptions.NoSuchScreensaverException"></exception>
        ''' <exception cref="Exceptions.ScreensaverManagementException"></exception>
        Public Sub RemoveCustomSaverFromSettings(CustomSaver As String)
            If Not CustomSavers.ContainsKey(CustomSaver) Then Throw New Exceptions.NoSuchScreensaverException(DoTranslation("Screensaver {0} not found."), CustomSaver)
            If Not CustomSaverSettingsToken.Remove(CustomSaver) Then Throw New Exceptions.ScreensaverManagementException(DoTranslation("Failed to remove screensaver {0} from config."), CustomSaver)
            If CustomSaverSettingsToken IsNot Nothing Then File.WriteAllText(GetKernelPath(KernelPathType.CustomSaverSettings), JsonConvert.SerializeObject(CustomSaverSettingsToken, Formatting.Indented))
        End Sub

        ''' <summary>
        ''' Gets custom saver settings
        ''' </summary>
        ''' <param name="CustomSaver">A custom saver</param>
        ''' <param name="SaverSetting">A saver setting</param>
        ''' <returns>Saver setting value if successful; nothing if unsuccessful.</returns>
        ''' <exception cref="Exceptions.NoSuchScreensaverException"></exception>
        Public Function GetCustomSaverSettings(CustomSaver As String, SaverSetting As String) As Object
            If Not CustomSaverSettingsToken.ContainsKey(CustomSaver) Then Throw New Exceptions.NoSuchScreensaverException(DoTranslation("Screensaver {0} not found."), CustomSaver)
            For Each Setting As JProperty In CustomSaverSettingsToken(CustomSaver)
                If Setting.Name = SaverSetting Then
                    Return Setting.Value.ToObject(GetType(Object))
                End If
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' Sets custom saver settings
        ''' </summary>
        ''' <param name="CustomSaver">A custom saver</param>
        ''' <param name="SaverSetting">A saver setting</param>
        ''' <param name="Value">Value</param>
        ''' <returns>True if successful; False if unsuccessful.</returns>
        ''' <exception cref="Exceptions.NoSuchScreensaverException"></exception>
        Public Function SetCustomSaverSettings(CustomSaver As String, SaverSetting As String, Value As Object) As Boolean
            If Not CustomSaverSettingsToken.ContainsKey(CustomSaver) Then Throw New Exceptions.NoSuchScreensaverException(DoTranslation("Screensaver {0} not found."), CustomSaver)
            Dim SettingFound As Boolean
            For Each Setting As JProperty In CustomSaverSettingsToken(CustomSaver)
                If Setting.Name = SaverSetting Then
                    SettingFound = True
                    CustomSaverSettingsToken(CustomSaver)(SaverSetting) = Value.ToString
                End If
            Next
            If CustomSaverSettingsToken IsNot Nothing Then File.WriteAllText(GetKernelPath(KernelPathType.CustomSaverSettings), JsonConvert.SerializeObject(CustomSaverSettingsToken, Formatting.Indented))
            Return SettingFound
        End Function

    End Module
End Namespace
