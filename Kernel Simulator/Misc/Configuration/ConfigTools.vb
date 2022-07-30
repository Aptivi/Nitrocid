
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

Imports System.IO
Imports Newtonsoft.Json.Linq

Namespace Misc.Configuration
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
            Dim PristineConfigObject As JObject = GetNewConfigObject()

            'General sections
            Dim ExpectedSections As Integer = PristineConfigObject.Count
            Dim ExpectedGeneralKeys As Integer = PristineConfigObject("General").Count
            Dim ExpectedColorsKeys As Integer = PristineConfigObject("Colors").Count
            Dim ExpectedHardwareKeys As Integer = PristineConfigObject("Hardware").Count
            Dim ExpectedLoginKeys As Integer = PristineConfigObject("Login").Count
            Dim ExpectedShellKeys As Integer = PristineConfigObject("Shell").Count
            Dim ExpectedFilesystemKeys As Integer = PristineConfigObject("Filesystem").Count
            Dim ExpectedNetworkKeys As Integer = PristineConfigObject("Network").Count
            Dim ExpectedScreensaverKeys As Integer = PristineConfigObject("Screensaver").Count
            Dim ExpectedSplashKeys As Integer = PristineConfigObject("Splash").Count
            Dim ExpectedMiscKeys As Integer = PristineConfigObject("Misc").Count

            'Individual screensaver keys
            Dim ExpectedScreensaverColorMixKeys As Integer = PristineConfigObject("Screensaver")("ColorMix").Count
            Dim ExpectedScreensaverDiscoKeys As Integer = PristineConfigObject("Screensaver")("Disco").Count
            Dim ExpectedScreensaverGlitterColorKeys As Integer = PristineConfigObject("Screensaver")("GlitterColor").Count
            Dim ExpectedScreensaverLinesKeys As Integer = PristineConfigObject("Screensaver")("Lines").Count
            Dim ExpectedScreensaverDissolveKeys As Integer = PristineConfigObject("Screensaver")("Dissolve").Count
            Dim ExpectedScreensaverBouncingBlockKeys As Integer = PristineConfigObject("Screensaver")("BouncingBlock").Count
            Dim ExpectedScreensaverBouncingTextKeys As Integer = PristineConfigObject("Screensaver")("BouncingText").Count
            Dim ExpectedScreensaverProgressClockKeys As Integer = PristineConfigObject("Screensaver")("ProgressClock").Count
            Dim ExpectedScreensaverLighterKeys As Integer = PristineConfigObject("Screensaver")("Lighter").Count
            Dim ExpectedScreensaverWipeKeys As Integer = PristineConfigObject("Screensaver")("Wipe").Count
            Dim ExpectedScreensaverMatrixKeys As Integer = PristineConfigObject("Screensaver")("Matrix").Count
            Dim ExpectedScreensaverGlitterMatrixKeys As Integer = PristineConfigObject("Screensaver")("GlitterMatrix").Count
            Dim ExpectedScreensaverFaderKeys As Integer = PristineConfigObject("Screensaver")("Fader").Count
            Dim ExpectedScreensaverFaderBackKeys As Integer = PristineConfigObject("Screensaver")("FaderBack").Count
            Dim ExpectedScreensaverBeatFaderKeys As Integer = PristineConfigObject("Screensaver")("BeatFader").Count
            Dim ExpectedScreensaverTypoKeys As Integer = PristineConfigObject("Screensaver")("Typo").Count
            Dim ExpectedScreensaverMarqueeKeys As Integer = PristineConfigObject("Screensaver")("Marquee").Count
            Dim ExpectedScreensaverLinotypoKeys As Integer = PristineConfigObject("Screensaver")("Linotypo").Count
            Dim ExpectedScreensaverTypewriterKeys As Integer = PristineConfigObject("Screensaver")("Typewriter").Count
            Dim ExpectedScreensaverFlashColorKeys As Integer = PristineConfigObject("Screensaver")("FlashColor").Count
            Dim ExpectedScreensaverSpotWriteKeys As Integer = PristineConfigObject("Screensaver")("SpotWrite").Count
            Dim ExpectedScreensaverRampKeys As Integer = PristineConfigObject("Screensaver")("Ramp").Count
            Dim ExpectedScreensaverStackBoxKeys As Integer = PristineConfigObject("Screensaver")("StackBox").Count
            Dim ExpectedScreensaverSnakerKeys As Integer = PristineConfigObject("Screensaver")("Snaker").Count
            Dim ExpectedScreensaverBarRotKeys As Integer = PristineConfigObject("Screensaver")("BarRot").Count
            Dim ExpectedScreensaverFireworksKeys As Integer = PristineConfigObject("Screensaver")("Fireworks").Count
            Dim ExpectedScreensaverFigletKeys As Integer = PristineConfigObject("Screensaver")("Figlet").Count

            'Individual splash keys
            Dim ExpectedSplashSimpleKeys As Integer = PristineConfigObject("Splash")("Simple").Count
            Dim ExpectedSplashProgressKeys As Integer = PristineConfigObject("Splash")("Progress").Count

            'Check for missing sections
            If ConfigToken.Count <> ExpectedSections Then
                Wdbg(DebugLevel.W, "Missing sections. Config fix needed set to true.")
                FixesNeeded = True
            End If
            If ConfigToken("Screensaver") IsNot Nothing Then
                If ConfigToken("Screensaver").Count <> ExpectedScreensaverKeys Then
                    Wdbg(DebugLevel.W, "Missing sections and/or keys in Screensaver. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Splash") IsNot Nothing Then
                If ConfigToken("Splash").Count <> ExpectedSplashKeys Then
                    Wdbg(DebugLevel.W, "Missing sections and/or keys in Splash. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If

            'Now, check for missing keys in each section that ARE available.
            If ConfigToken("General") IsNot Nothing Then
                If ConfigToken("General").Count <> ExpectedGeneralKeys Then
                    Wdbg(DebugLevel.W, "Missing keys in General. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Colors") IsNot Nothing Then
                If ConfigToken("Colors").Count <> ExpectedColorsKeys Then
                    Wdbg(DebugLevel.W, "Missing keys in Colors. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Hardware") IsNot Nothing Then
                If ConfigToken("Hardware").Count <> ExpectedHardwareKeys Then
                    Wdbg(DebugLevel.W, "Missing keys in Hardware. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Login") IsNot Nothing Then
                If ConfigToken("Login").Count <> ExpectedLoginKeys Then
                    Wdbg(DebugLevel.W, "Missing keys in Login. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Shell") IsNot Nothing Then
                If ConfigToken("Shell").Count <> ExpectedShellKeys Then
                    Wdbg(DebugLevel.W, "Missing keys in Shell. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Filesystem") IsNot Nothing Then
                If ConfigToken("Filesystem").Count <> ExpectedFilesystemKeys Then
                    Wdbg(DebugLevel.W, "Missing keys in Filesystem. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Network") IsNot Nothing Then
                If ConfigToken("Network").Count <> ExpectedNetworkKeys Then
                    Wdbg(DebugLevel.W, "Missing keys in Network. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver") IsNot Nothing Then
                If ConfigToken("Screensaver")("ColorMix") IsNot Nothing Then
                    If ConfigToken("Screensaver")("ColorMix").Count <> ExpectedScreensaverColorMixKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > ColorMix. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("Disco") IsNot Nothing Then
                    If ConfigToken("Screensaver")("Disco").Count <> ExpectedScreensaverDiscoKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > Disco. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("GlitterColor") IsNot Nothing Then
                    If ConfigToken("Screensaver")("GlitterColor").Count <> ExpectedScreensaverGlitterColorKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > GlitterColor. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("Lines") IsNot Nothing Then
                    If ConfigToken("Screensaver")("Lines").Count <> ExpectedScreensaverLinesKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > Lines. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("Dissolve") IsNot Nothing Then
                    If ConfigToken("Screensaver")("Dissolve").Count <> ExpectedScreensaverDissolveKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > Dissolve. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("BouncingBlock") IsNot Nothing Then
                    If ConfigToken("Screensaver")("BouncingBlock").Count <> ExpectedScreensaverBouncingBlockKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > BouncingBlock. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("BouncingText") IsNot Nothing Then
                    If ConfigToken("Screensaver")("BouncingText").Count <> ExpectedScreensaverBouncingTextKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > BouncingText. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("ProgressClock") IsNot Nothing Then
                    If ConfigToken("Screensaver")("ProgressClock").Count <> ExpectedScreensaverProgressClockKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > ProgressClock. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("Lighter") IsNot Nothing Then
                    If ConfigToken("Screensaver")("Lighter").Count <> ExpectedScreensaverLighterKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > Lighter. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("Wipe") IsNot Nothing Then
                    If ConfigToken("Screensaver")("Wipe").Count <> ExpectedScreensaverWipeKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > Wipe. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("Matrix") IsNot Nothing Then
                    If ConfigToken("Screensaver")("Matrix").Count <> ExpectedScreensaverMatrixKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > Matrix. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("GlitterMatrix") IsNot Nothing Then
                    If ConfigToken("Screensaver")("GlitterMatrix").Count <> ExpectedScreensaverGlitterMatrixKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > GlitterMatrix. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("Fader") IsNot Nothing Then
                    If ConfigToken("Screensaver")("Fader").Count <> ExpectedScreensaverFaderKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > Fader. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("FaderBack") IsNot Nothing Then
                    If ConfigToken("Screensaver")("FaderBack").Count <> ExpectedScreensaverFaderBackKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > FaderBack. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("BeatFader") IsNot Nothing Then
                    If ConfigToken("Screensaver")("BeatFader").Count <> ExpectedScreensaverBeatFaderKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > BeatFader. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("Typo") IsNot Nothing Then
                    If ConfigToken("Screensaver")("Typo").Count <> ExpectedScreensaverTypoKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > Typo. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("Marquee") IsNot Nothing Then
                    If ConfigToken("Screensaver")("Marquee").Count <> ExpectedScreensaverMarqueeKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > Marquee. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("Linotypo") IsNot Nothing Then
                    If ConfigToken("Screensaver")("Linotypo").Count <> ExpectedScreensaverLinotypoKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > Linotypo. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("Typewriter") IsNot Nothing Then
                    If ConfigToken("Screensaver")("Typewriter").Count <> ExpectedScreensaverTypewriterKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > Typewriter. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("FlashColor") IsNot Nothing Then
                    If ConfigToken("Screensaver")("FlashColor").Count <> ExpectedScreensaverFlashColorKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > FlashColor. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("SpotWrite") IsNot Nothing Then
                    If ConfigToken("Screensaver")("SpotWrite").Count <> ExpectedScreensaverSpotWriteKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > SpotWrite. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("Ramp") IsNot Nothing Then
                    If ConfigToken("Screensaver")("Ramp").Count <> ExpectedScreensaverRampKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > Ramp. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("StackBox") IsNot Nothing Then
                    If ConfigToken("Screensaver")("StackBox").Count <> ExpectedScreensaverStackBoxKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > StackBox. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("Snaker") IsNot Nothing Then
                    If ConfigToken("Screensaver")("Snaker").Count <> ExpectedScreensaverSnakerKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > Snaker. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("BarRot") IsNot Nothing Then
                    If ConfigToken("Screensaver")("BarRot").Count <> ExpectedScreensaverBarRotKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > BarRot. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("Fireworks") IsNot Nothing Then
                    If ConfigToken("Screensaver")("Fireworks").Count <> ExpectedScreensaverFireworksKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > Fireworks. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Screensaver")("Figlet") IsNot Nothing Then
                    If ConfigToken("Screensaver")("Figlet").Count <> ExpectedScreensaverFigletKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Screensaver > Figlet. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
            End If
            If ConfigToken("Splash") IsNot Nothing Then
                If ConfigToken("Splash")("Simple") IsNot Nothing Then
                    If ConfigToken("Splash")("Simple").Count <> ExpectedSplashSimpleKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Splash > Simple. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
                If ConfigToken("Splash")("Progress") IsNot Nothing Then
                    If ConfigToken("Splash")("Progress").Count <> ExpectedSplashProgressKeys Then
                        Wdbg(DebugLevel.W, "Missing keys in Splash > Progress. Config fix needed set to true.")
                        FixesNeeded = True
                    End If
                End If
            End If
            If ConfigToken("Misc") IsNot Nothing Then
                If ConfigToken("Misc").Count <> ExpectedMiscKeys Then
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

    End Module
End Namespace
