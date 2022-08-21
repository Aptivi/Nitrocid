
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

Imports KS.Misc.Reflection
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.IO
Imports Newtonsoft.Json.Linq

Namespace Kernel.Configuration
    Public Module ConfigTools

        ''' <summary>
        ''' Reloads config
        ''' </summary>
        Public Sub ReloadConfig()
            KernelEventManager.RaisePreReloadConfig()
            InitializeConfig()
            KernelEventManager.RaisePostReloadConfig()
        End Sub

        ''' <summary>
        ''' Reloads config
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function TryReloadConfig() As Boolean
            Try
                ReloadConfig()
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to reload config: {0}", ex.Message)
                WStkTrc(ex)
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Checks to see if the config needs repair and repairs it as necessary.
        ''' </summary>
        ''' <returns>True if the config is repaired; False if no repairs done; Throws exceptions if unsuccessful.</returns>
        Public Function RepairConfig() As Boolean
            'Variables
            Dim FixesNeeded As Boolean

            'General sections
            Dim ExpectedSections As Integer = PristineConfigToken.Count

            'Check for missing sections
            If ConfigToken.Count <> ExpectedSections Then
                Wdbg(DebugLevel.W, "Missing sections. Config fix needed set to true.")
                FixesNeeded = True
            End If

            'Go through sections
            Try
                For Each Section In PristineConfigToken
                    If ConfigToken(Section.Key) IsNot Nothing Then
                        'Check the normal keys
                        If ConfigToken(Section.Key).Count <> PristineConfigToken(Section.Key).Count Then
                            Wdbg(DebugLevel.W, "Missing sections and/or keys in {0}. Config fix needed set to true.", Section.Key)
                            FixesNeeded = True
                        End If

                        'Check the screensaver keys
                        If Section.Key = "Screensaver" Then
                            For Each ScreensaverSection As JProperty In PristineConfigToken("Screensaver").Where(Function(x) x.First.Type = JTokenType.Object)
                                If ConfigToken("Screensaver")(ScreensaverSection.Name).Count() <> PristineConfigToken("Screensaver")(ScreensaverSection.Name).Count() Then
                                    Wdbg(DebugLevel.W, "Missing sections and/or keys in Screensaver > {0}. Config fix needed set to true.", ScreensaverSection.Name)
                                    FixesNeeded = True
                                End If
                            Next
                        End If

                        'Check the splash keys
                        If Section.Key = "Splash" Then
                            For Each SplashSection As JProperty In PristineConfigToken("Splash").Where(Function(x) x.First.Type = JTokenType.Object)
                                If ConfigToken("Splash")(SplashSection.Name).Count() <> PristineConfigToken("Splash")(SplashSection.Name).Count() Then
                                    Wdbg(DebugLevel.W, "Missing sections and/or keys in Splash > {0}. Config fix needed set to true.", SplashSection.Name)
                                    FixesNeeded = True
                                End If
                            Next
                        End If
                    End If
                Next
            Catch ex As Exception
                'Somehow, the config is corrupt or something. Fix it.
                Wdbg(DebugLevel.E, "Found a serious error in configuration: {0}", ex.Message)
                WStkTrc(ex)
                FixesNeeded = True
            End Try

            'If the fixes are needed, try to remake config with parsed values
            If FixesNeeded Then CreateConfig()
            Return FixesNeeded
        End Function

        ''' <summary>
        ''' Gets the JSON token for specific configuration category with an optional sub-category
        ''' </summary>
        ''' <param name="ConfigCategory">Config category</param>
        ''' <param name="ConfigSubCategoryName">Sub-category name. Should be an Object. Currently used for screensavers</param>
        Public Function GetConfigCategory(ConfigCategory As ConfigCategory, Optional ConfigSubCategoryName As String = "") As JToken
            'Try to parse the config category
            Wdbg(DebugLevel.I, "Parsing config category {0}...", ConfigCategory)
            If [Enum].TryParse(ConfigCategory, ConfigCategory) Then
                'We got a valid category. Now, get the token for the specific category
                Wdbg(DebugLevel.I, "Category {0} found! Parsing sub-category {1} ({2})...", ConfigCategory, ConfigSubCategoryName, ConfigSubCategoryName.Length)
                Dim CategoryToken As JToken = ConfigToken(ConfigCategory.ToString)

                'Try to get the sub-category token and check to see if it's found or not
                Dim SubCategoryToken As JToken = CategoryToken(ConfigSubCategoryName)
                If Not String.IsNullOrWhiteSpace(ConfigSubCategoryName) And SubCategoryToken IsNot Nothing Then
                    'We got the subcategory! Check to see if it's really a sub-category (Object) or not
                    Wdbg(DebugLevel.I, "Sub-category {0} found! Is it really the sub-category? Type = {1}", ConfigSubCategoryName, SubCategoryToken.Type)
                    If SubCategoryToken.Type = JTokenType.Object Then
                        Wdbg(DebugLevel.I, "It is really a sub-category!")
                        Return SubCategoryToken
                    Else
                        Wdbg(DebugLevel.W, "It is not really a sub-category. Returning master category...")
                        Return CategoryToken
                    End If
                Else
                    'We only got the full category.
                    Wdbg(DebugLevel.I, "Returning master category...")
                    Return CategoryToken
                End If
            Else
                'We didn't get a category.
                Wdbg(DebugLevel.E, "Category {0} not found!", ConfigCategory)
                Throw New Exceptions.ConfigException(DoTranslation("Config category {0} not found."), ConfigCategory)
            End If
        End Function

        ''' <summary>
        ''' Sets the value of an entry in a category.
        ''' </summary>
        ''' <param name="ConfigCategory">Config category</param>
        ''' <param name="ConfigEntryName">Config entry name.</param>
        ''' <param name="ConfigValue">Config entry value to install</param>
        Public Sub SetConfigValue(ConfigCategory As ConfigCategory, ConfigEntryName As String, ConfigValue As JToken)
            SetConfigValue(ConfigCategory, GetConfigCategory(ConfigCategory), ConfigEntryName, ConfigValue)
        End Sub

        ''' <summary>
        ''' Sets the value of an entry in a category.
        ''' </summary>
        ''' <param name="ConfigCategory">Config category</param>
        ''' <param name="ConfigEntryName">Config entry name.</param>
        ''' <param name="ConfigValue">Config entry value to install</param>
        Public Sub SetConfigValue(ConfigCategory As ConfigCategory, ConfigSubCategoryName As String, ConfigEntryName As String, ConfigValue As JToken)
            SetConfigValue(ConfigCategory, GetConfigCategory(ConfigCategory, ConfigSubCategoryName), ConfigEntryName, ConfigValue)
        End Sub

        ''' <summary>
        ''' Sets the value of an entry in a category.
        ''' </summary>
        ''' <param name="ConfigCategory">Config category</param>
        ''' <param name="ConfigCategoryToken">Config category or sub-category token (You can get it from <see cref="GetConfigCategory(ConfigCategory, String)"/></param>
        ''' <param name="ConfigEntryName">Config entry name.</param>
        ''' <param name="ConfigValue">Config entry value to install</param>
        Public Sub SetConfigValue(ConfigCategory As ConfigCategory, ConfigCategoryToken As JToken, ConfigEntryName As String, ConfigValue As JToken)
            'Try to parse the config category
            Wdbg(DebugLevel.I, "Parsing config category {0}...", ConfigCategory)
            If [Enum].TryParse(ConfigCategory, ConfigCategory) Then
                'We have a valid category. Now, find the config entry property in the token
                Wdbg(DebugLevel.I, "Parsing config entry {0}...", ConfigEntryName)
                Dim CategoryToken As JToken = ConfigToken(ConfigCategory.ToString)
                If ConfigCategoryToken(ConfigEntryName) IsNot Nothing Then
                    'Assign the new value to it and write the changes to the token and the config file. Don't worry, debuggers, when you set the value like below,
                    'it will automatically save the changes to ConfigToken as in three lines above
                    Wdbg(DebugLevel.E, "Entry {0} found! Setting value...", ConfigEntryName)
                    ConfigCategoryToken(ConfigEntryName) = ConfigValue

                    'Write the changes to the config file
                    File.WriteAllText(GetKernelPath(KernelPathType.Configuration), JsonConvert.SerializeObject(ConfigToken, Formatting.Indented))
                Else
                    'We didn't get an entry.
                    Wdbg(DebugLevel.E, "Entry {0} not found!", ConfigEntryName)
                    Throw New Exceptions.ConfigException(DoTranslation("Config entry {0} not found."), ConfigEntryName)
                End If
            Else
                'We didn't get a category.
                Wdbg(DebugLevel.E, "Category {0} not found!", ConfigCategory)
                Throw New Exceptions.ConfigException(DoTranslation("Config category {0} not found."), ConfigCategory)
            End If
        End Sub

        ''' <summary>
        ''' Gets the value of an entry in a category.
        ''' </summary>
        ''' <param name="ConfigCategory">Config category</param>
        ''' <param name="ConfigEntryName">Config entry name.</param>
        Public Function GetConfigValue(ConfigCategory As ConfigCategory, ConfigEntryName As String) As JToken
            Return GetConfigValue(ConfigCategory, GetConfigCategory(ConfigCategory), ConfigEntryName)
        End Function

        ''' <summary>
        ''' Gets the value of an entry in a category.
        ''' </summary>
        ''' <param name="ConfigCategory">Config category</param>
        ''' <param name="ConfigEntryName">Config entry name.</param>
        Public Function GetConfigValue(ConfigCategory As ConfigCategory, ConfigSubCategoryName As String, ConfigEntryName As String) As JToken
            Return GetConfigValue(ConfigCategory, GetConfigCategory(ConfigCategory, ConfigSubCategoryName), ConfigEntryName)
        End Function

        ''' <summary>
        ''' Gets the value of an entry in a category.
        ''' </summary>
        ''' <param name="ConfigCategory">Config category</param>
        ''' <param name="ConfigCategoryToken">Config category or sub-category token (You can get it from <see cref="GetConfigCategory(ConfigCategory, String)"/></param>
        ''' <param name="ConfigEntryName">Config entry name.</param>
        Public Function GetConfigValue(ConfigCategory As ConfigCategory, ConfigCategoryToken As JToken, ConfigEntryName As String) As JToken
            'Try to parse the config category
            Wdbg(DebugLevel.I, "Parsing config category {0}...", ConfigCategory)
            If [Enum].TryParse(ConfigCategory, ConfigCategory) Then
                'We have a valid category. Now, find the config entry property in the token
                Wdbg(DebugLevel.I, "Parsing config entry {0}...", ConfigEntryName)
                Dim CategoryToken As JToken = ConfigToken(ConfigCategory.ToString)
                If ConfigCategoryToken(ConfigEntryName) IsNot Nothing Then
                    'We got the appropriate value! Return it.
                    Wdbg(DebugLevel.E, "Entry {0} found! Getting value...", ConfigEntryName)
                    Return ConfigCategoryToken(ConfigEntryName)
                Else
                    'We didn't get an entry.
                    Wdbg(DebugLevel.E, "Entry {0} not found!", ConfigEntryName)
                    Throw New Exceptions.ConfigException(DoTranslation("Config entry {0} not found."), ConfigEntryName)
                End If
            Else
                'We didn't get a category.
                Wdbg(DebugLevel.E, "Category {0} not found!", ConfigCategory)
                Throw New Exceptions.ConfigException(DoTranslation("Config category {0} not found."), ConfigCategory)
            End If
        End Function

        ''' <summary>
        ''' Finds a setting with the matching pattern
        ''' </summary>
        Public Function FindSetting(Pattern As String, SettingsToken As JToken) As List(Of String)
            Dim Results As New List(Of String)

            'Search the settings for the given pattern
            Try
                For SectionIndex As Integer = 0 To SettingsToken.Count - 1
                    Dim SectionToken As JToken = SettingsToken.ToList(SectionIndex)
                    For SettingIndex As Integer = 0 To SectionToken.Count - 1
                        Dim SettingToken As JToken = SectionToken.ToList(SettingIndex)("Keys")
                        For KeyIndex As Integer = 0 To SettingToken.Count - 1
                            Dim KeyName As String = DoTranslation(SettingToken.ToList(KeyIndex)("Name"))
                            If Regex.IsMatch(KeyName, Pattern, RegexOptions.IgnoreCase) Then
                                Results.Add($"[{SectionIndex + 1}/{KeyIndex + 1}] {KeyName}")
                            End If
                        Next
                    Next
                Next
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to find setting {0}: {1}", Pattern, ex.Message)
                WStkTrc(ex)
            End Try

            'Return the results
            Return Results
        End Function

        ''' <summary>
        ''' Checks all the config variables to see if they can be parsed
        ''' </summary>
        Public Function CheckConfigVariables() As Dictionary(Of String, Boolean)
            Dim SettingsToken As JToken = JToken.Parse(Properties.Resources.SettingsEntries)
            Dim SaverSettingsToken As JToken = JToken.Parse(Properties.Resources.ScreensaverSettingsEntries)
            Dim SplashSettingsToken As JToken = JToken.Parse(Properties.Resources.SplashSettingsEntries)
            Dim Tokens As JToken() = {SettingsToken, SaverSettingsToken, SplashSettingsToken}
            Dim Results As New Dictionary(Of String, Boolean)

            'Parse all the settings
            For Each Token As JToken In Tokens
                For Each Section As JProperty In Token
                    Dim SectionToken As JToken = Token(Section.Name)
                    For Each Key As JToken In SectionToken("Keys")
                        Dim KeyName As String = Key("Name")
                        Dim KeyVariable As String = Key("Variable")
                        Dim KeyEnumeration As String = Key("Enumeration")
                        Dim KeyEnumerationInternal As Boolean = If(Key("EnumerationInternal"), False)
                        Dim KeyEnumerationAssembly As String = Key("EnumerationAssembly")
                        Dim KeyFound As Boolean

                        'Check the variable
                        KeyFound = CheckField(KeyVariable) Or CheckProperty(KeyVariable)
                        Results.Add($"{KeyName}, {KeyVariable}", KeyFound)

                        'Check the enumeration
                        If KeyEnumeration IsNot Nothing Then
                            Dim Result As Boolean
                            If KeyEnumerationInternal Then
                                'Apparently, we need to have a full assembly name for getting types.
                                Result = Type.GetType("KS." + KeyEnumeration + ", " + Assembly.GetExecutingAssembly.FullName) IsNot Nothing
                            Else
                                Result = Type.GetType(KeyEnumeration + ", " + KeyEnumerationAssembly) IsNot Nothing
                            End If
                            Results.Add($"{KeyName}, {KeyVariable}, {KeyEnumeration}", Result)
                        End If
                    Next
                Next
            Next

            'Return the results
            Return Results
        End Function

    End Module
End Namespace
