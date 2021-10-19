
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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
Imports Newtonsoft.Json.Linq

Public Module ConfigTools

    ''' <summary>
    ''' Reloads config
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function ReloadConfig() As Boolean
        Try
            EventManager.RaisePreReloadConfig()
            InitializeConfig()
            EventManager.RaisePostReloadConfig()
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

        'Check for missing sections
        If ConfigToken.Count <> 9 Then
            Wdbg(DebugLevel.W, "Missing sections. Config fix needed set to true.")
            FixesNeeded = True
        End If
        If ConfigToken("Screensaver") IsNot Nothing Then
            If ConfigToken("Screensaver").Count <> 22 + 4 Then 'Screensavers + Keys
                Wdbg(DebugLevel.W, "Missing sections and/or keys in Screensaver. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If

        'Now, check for missing keys in each section that ARE available.
        If ConfigToken("General") IsNot Nothing Then
            If ConfigToken("General").Count <> 15 Then
                Wdbg(DebugLevel.W, "Missing keys in General. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If
        If ConfigToken("Colors") IsNot Nothing Then
            If ConfigToken("Colors").Count <> 36 Then
                Wdbg(DebugLevel.W, "Missing keys in Colors. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If
        If ConfigToken("Hardware") IsNot Nothing Then
            If ConfigToken("Hardware").Count <> 4 Then
                Wdbg(DebugLevel.W, "Missing keys in Hardware. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If
        If ConfigToken("Login") IsNot Nothing Then
            If ConfigToken("Login").Count <> 9 Then
                Wdbg(DebugLevel.W, "Missing keys in Login. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If
        If ConfigToken("Shell") IsNot Nothing Then
            If ConfigToken("Shell").Count <> 16 Then
                Wdbg(DebugLevel.W, "Missing keys in Shell. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If
        If ConfigToken("Filesystem") IsNot Nothing Then
            If ConfigToken("Filesystem").Count <> 9 Then
                Wdbg(DebugLevel.W, "Missing keys in Filesystem. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If
        If ConfigToken("Network") IsNot Nothing Then
            If ConfigToken("Network").Count <> 54 Then
                Wdbg(DebugLevel.W, "Missing keys in Network. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If
        If ConfigToken("Screensaver") IsNot Nothing Then
            If ConfigToken("Screensaver")("ColorMix") IsNot Nothing Then
                If ConfigToken("Screensaver")("ColorMix").Count <> 12 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > ColorMix. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Disco") IsNot Nothing Then
                If ConfigToken("Screensaver")("Disco").Count <> 13 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > Disco. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("GlitterColor") IsNot Nothing Then
                If ConfigToken("Screensaver")("GlitterColor").Count <> 11 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > GlitterColor. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Lines") IsNot Nothing Then
                If ConfigToken("Screensaver")("Lines").Count <> 13 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > Lines. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Dissolve") IsNot Nothing Then
                If ConfigToken("Screensaver")("Dissolve").Count <> 11 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > Dissolve. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("BouncingBlock") IsNot Nothing Then
                If ConfigToken("Screensaver")("BouncingBlock").Count <> 13 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > BouncingBlock. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("BouncingText") IsNot Nothing Then
                If ConfigToken("Screensaver")("BouncingText").Count <> 14 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > BouncingText. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("ProgressClock") IsNot Nothing Then
                If ConfigToken("Screensaver")("ProgressClock").Count <> 68 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > ProgressClock. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Lighter") IsNot Nothing Then
                If ConfigToken("Screensaver")("Lighter").Count <> 13 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > Lighter. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Wipe") IsNot Nothing Then
                If ConfigToken("Screensaver")("Wipe").Count <> 13 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > Wipe. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Matrix") IsNot Nothing Then
                If ConfigToken("Screensaver")("Matrix").Count <> 1 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > Matrix. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("GlitterMatrix") IsNot Nothing Then
                If ConfigToken("Screensaver")("GlitterMatrix").Count <> 3 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > GlitterMatrix. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Fader") IsNot Nothing Then
                If ConfigToken("Screensaver")("Fader").Count <> 11 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > Fader. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("FaderBack") IsNot Nothing Then
                If ConfigToken("Screensaver")("FaderBack").Count <> 9 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > FaderBack. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("BeatFader") IsNot Nothing Then
                If ConfigToken("Screensaver")("BeatFader").Count <> 14 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > BeatFader. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Typo") IsNot Nothing Then
                If ConfigToken("Screensaver")("Typo").Count <> 8 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > Typo. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Marquee") IsNot Nothing Then
                If ConfigToken("Screensaver")("Marquee").Count <> 15 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > Marquee. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Linotypo") IsNot Nothing Then
                If ConfigToken("Screensaver")("Linotypo").Count <> 12 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > Linotypo. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Typewriter") IsNot Nothing Then
                If ConfigToken("Screensaver")("Typewriter").Count <> 6 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > Typewriter. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("FlashColor") IsNot Nothing Then
                If ConfigToken("Screensaver")("FlashColor").Count <> 12 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > FlashColor. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("SpotWrite") IsNot Nothing Then
                If ConfigToken("Screensaver")("SpotWrite").Count <> 4 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > SpotWrite. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Ramp") IsNot Nothing Then
                If ConfigToken("Screensaver")("Ramp").Count <> 37 Then
                    Wdbg(DebugLevel.W, "Missing keys in Screensaver > Ramp. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
        End If
        If ConfigToken("Misc") IsNot Nothing Then
            If ConfigToken("Misc").Count <> 23 Then
                Wdbg(DebugLevel.W, "Missing keys in Misc. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If

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

End Module
