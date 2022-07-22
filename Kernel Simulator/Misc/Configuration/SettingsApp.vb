
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

Imports System.Reflection
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json.Linq
Imports KS.Files.Folders
Imports KS.Files.Querying
Imports KS.Misc.Reflection
Imports KS.Misc.Screensaver.Customized
Imports KS.Misc.Screensaver

Namespace Misc.Configuration
    Public Module SettingsApp

        Private CurrentSettingsType As SettingsType = SettingsType.Normal

        ''' <summary>
        ''' Main page
        ''' </summary>
        Sub OpenMainPage(SettingsType As SettingsType)
            Dim PromptFinished As Boolean
            Dim AnswerString As String
            Dim AnswerInt As Integer
            Dim SettingsToken As JToken = OpenSettingsResource(SettingsType)
            Dim MaxSections As Integer = SettingsToken.Count
            CurrentSettingsType = SettingsType

            While Not PromptFinished
                Console.Clear()

                'List sections
                WriteSeparator(DoTranslation("Welcome to Settings!"), True)
                Write(NewLine + DoTranslation("Select section:") + NewLine, True, ColTypes.Neutral)
                For SectionIndex As Integer = 0 To MaxSections - 1
                    Dim Section As JProperty = SettingsToken.ToList(SectionIndex)
                    If SettingsType <> SettingsType.Normal Then
                        Write(" {0}) " + Section.Name + "...", True, ColTypes.Option, SectionIndex + 1)
                    Else
                        Write(" {0}) " + DoTranslation(Section.Name + " Settings..."), True, ColTypes.Option, SectionIndex + 1)
                    End If
                Next
                Console.WriteLine()
                Write(" {0}) " + DoTranslation("Find a Setting"), True, ColTypes.AlternativeOption, MaxSections + 1)
                Write(" {0}) " + DoTranslation("Save Settings"), True, ColTypes.AlternativeOption, MaxSections + 2)
                Write(" {0}) " + DoTranslation("Save Settings As"), True, ColTypes.AlternativeOption, MaxSections + 3)
                Write(" {0}) " + DoTranslation("Load Settings From"), True, ColTypes.AlternativeOption, MaxSections + 4)
                Write(" {0}) " + DoTranslation("Exit"), True, ColTypes.AlternativeOption, MaxSections + 5)

                'Prompt user
                Console.WriteLine()
                Write("> ", False, ColTypes.Input)
                AnswerString = ReadLine()
                Wdbg(DebugLevel.I, "User answered {0}", AnswerString)
                Console.WriteLine()

                'Check for input
                Wdbg(DebugLevel.I, "Is the answer numeric? {0}", IsStringNumeric(AnswerString))
                If Integer.TryParse(AnswerString, AnswerInt) Then
                    Wdbg(DebugLevel.I, "Succeeded. Checking the answer if it points to the right direction...")
                    If AnswerInt >= 1 And AnswerInt <= MaxSections Then
                        'The selected answer is a section
                        Dim SelectedSection As JProperty = SettingsToken.ToList(AnswerInt - 1)
                        Wdbg(DebugLevel.I, "Opening section {0}...", SelectedSection.Name)
                        OpenSection(SelectedSection.Name, SettingsToken)
                    ElseIf AnswerInt = MaxSections + 1 Then
                        'The selected answer is "Find a Setting"
                        VariableFinder(SettingsToken)
                    ElseIf AnswerInt = MaxSections + 2 Then
                        'The selected answer is "Save Settings"
                        Wdbg(DebugLevel.I, "Saving settings...")
                        Try
                            CreateConfig()
                            SaveCustomSaverSettings()
                        Catch ex As Exception
                            Write(ex.Message, True, ColTypes.Error)
                            WStkTrc(ex)
                            Console.ReadKey()
                        End Try
                    ElseIf AnswerInt = MaxSections + 3 Then
                        'The selected answer is "Save Settings As"
                        Write(DoTranslation("Where do you want to save the current kernel settings?"), True, ColTypes.Question)
                        Dim Location As String = NeutralizePath(ReadLine())
                        If Not FileExists(Location) Then
                            Try
                                CreateConfig(Location)
                            Catch ex As Exception
                                Write(ex.Message, True, ColTypes.Error)
                                WStkTrc(ex)
                                Console.ReadKey()
                            End Try
                        Else
                            Write(DoTranslation("Can't save kernel settings on top of existing file."), True, ColTypes.Error)
                            Console.ReadKey()
                        End If
                    ElseIf AnswerInt = MaxSections + 4 Then
                        'The selected answer is "Load Settings From"
                        Write(DoTranslation("Where do you want to load the current kernel settings from?"), True, ColTypes.Question)
                        Dim Location As String = NeutralizePath(ReadLine())
                        If FileExists(Location) Then
                            Try
                                ReadConfig(Location)
                                CreateConfig()
                            Catch ex As Exception
                                Write(ex.Message, True, ColTypes.Error)
                                WStkTrc(ex)
                                Console.ReadKey()
                            End Try
                        Else
                            Write(DoTranslation("File not found."), True, ColTypes.Error)
                            Console.ReadKey()
                        End If
                    ElseIf AnswerInt = MaxSections + 5 Then
                        'The selected answer is "Exit"
                        Wdbg(DebugLevel.W, "Exiting...")
                        PromptFinished = True
                        Console.Clear()
                    Else
                        'Invalid selection
                        Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                        Write(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerInt)
                        Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                        Console.ReadKey()
                    End If
                ElseIf ReadLineReboot.ReadLine.ReadRanToCompletion Then
                    Wdbg(DebugLevel.W, "Answer is not numeric.")
                    Write(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                    Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            End While
        End Sub

        ''' <summary>
        ''' Open section
        ''' </summary>
        ''' <param name="Section">Section name</param>
        ''' <param name="SettingsToken">Settings token</param>
        Sub OpenSection(Section As String, SettingsToken As JToken)
            Try
                'General variables
                Dim SectionFinished As Boolean
                Dim AnswerString As String
                Dim AnswerInt As Integer
                Dim SectionTokenGeneral As JToken = SettingsToken(Section)
                Dim SectionToken As JToken = SectionTokenGeneral("Keys")
                Dim SectionDescription = SectionTokenGeneral("Desc")
                Dim MaxOptions As Integer = SectionToken.Count

                While Not SectionFinished
                    Console.Clear()
                    WriteSeparator(DoTranslation(Section + " Settings..."), True)
                    Write(NewLine + DoTranslation(SectionDescription) + NewLine, True, ColTypes.Neutral)

                    'List options
                    For SectionIndex As Integer = 0 To MaxOptions - 1
                        Dim Setting As JToken = SectionToken(SectionIndex)
                        Dim CurrentValue As Object = "Unknown"
                        Dim Variable As String = Setting("Variable")
                        Dim VariableProperty As String = Setting("VariableProperty")
                        Dim VariableType As SettingsKeyType = [Enum].Parse(GetType(SettingsKeyType), Setting("Type"))

                        'Print the option
                        If VariableType = SettingsKeyType.SMaskedString Then
                            'Don't print the default value! We don't want to reveal passwords.
                            Write(" {0}) " + DoTranslation(Setting("Name")), True, ColTypes.Option, SectionIndex + 1)
                        Else
                            'Determine how to get the current value
                            If VariableProperty Is Nothing Then
                                If CheckField(Variable) Then
                                    'We're dealing with the field, get the value from it
                                    CurrentValue = GetValue(Variable)
                                ElseIf CheckProperty(Variable) Then
                                    'We're dealing with the property, get the value from it
                                    CurrentValue = GetPropertyValue(Variable)
                                End If

                                'Get the plain sequence from the color
                                If TypeOf CurrentValue Is Color Then
                                    CurrentValue = CurrentValue.PlainSequence
                                End If
                            Else
                                'Get the property value from variable
                                CurrentValue = GetPropertyValueInVariable(Variable, VariableProperty)
                            End If
                            Write(" {0}) " + DoTranslation(Setting("Name")) + " [{1}]", True, ColTypes.Option, SectionIndex + 1, CurrentValue)
                        End If
                    Next
                    Console.WriteLine()
                    If CurrentSettingsType = SettingsType.Screensaver Then
                        Write(" {0}) " + DoTranslation("Preview screensaver"), True, ColTypes.BackOption, MaxOptions + 1)
                        Write(" {0}) " + DoTranslation("Go Back...") + NewLine, True, ColTypes.BackOption, MaxOptions + 2)
                    Else
                        Write(" {0}) " + DoTranslation("Go Back...") + NewLine, True, ColTypes.BackOption, MaxOptions + 1)
                    End If
                    Wdbg(DebugLevel.W, "Section {0} has {1} selections.", Section, MaxOptions)

                    'Prompt user and check for input
                    Write("> ", False, ColTypes.Input)
                    AnswerString = ReadLine()
                    Wdbg(DebugLevel.I, "User answered {0}", AnswerString)
                    Console.WriteLine()

                    Wdbg(DebugLevel.I, "Is the answer numeric? {0}", IsStringNumeric(AnswerString))
                    If Integer.TryParse(AnswerString, AnswerInt) Then
                        Wdbg(DebugLevel.I, "Succeeded. Checking the answer if it points to the right direction...")
                        If AnswerInt >= 1 And AnswerInt <= MaxOptions Then
                            Wdbg(DebugLevel.I, "Opening key {0} from section {1}...", AnswerInt, Section)
                            OpenKey(Section, AnswerInt, SettingsToken)
                        ElseIf AnswerInt = MaxOptions + 1 And CurrentSettingsType = SettingsType.Screensaver Then
                            'Preview screensaver
                            Wdbg(DebugLevel.I, "User requested screensaver preview.")
                            ShowSavers(Section)
                        ElseIf AnswerInt = MaxOptions + 1 Or (AnswerInt = MaxOptions + 2 And CurrentSettingsType = SettingsType.Screensaver) Then
                            'Go Back...
                            Wdbg(DebugLevel.I, "User requested exit. Returning...")
                            SectionFinished = True
                        Else
                            Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                            Write(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerInt)
                            Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                            Console.ReadKey()
                        End If
                    ElseIf ReadLineReboot.ReadLine.ReadRanToCompletion Then
                        Wdbg(DebugLevel.W, "Answer is not numeric.")
                        Write(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                        Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                        Console.ReadKey()
                    End If
                End While
            Catch ex As Exception
                Console.Clear()
                Wdbg(DebugLevel.I, "Error trying to open section: {0}", ex.Message)
                WriteSeparator("???", True)
                Write(NewLine + "X) " + DoTranslation("Invalid section entered. Please go back."), True, ColTypes.Error)
                Write("X) " + DoTranslation("If you're sure that you've opened the right section, check this message out:"), True, ColTypes.Error)
                Write("X) " + ex.Message, True, ColTypes.Error)
                Console.ReadKey()
            End Try
        End Sub

        ''' <summary>
        ''' Open a key.
        ''' </summary>
        ''' <param name="Section">Section</param>
        ''' <param name="KeyNumber">Key number</param>
        ''' <param name="SettingsToken">Settings token</param>
        Sub OpenKey(Section As String, KeyNumber As Integer, SettingsToken As JToken)
            Try
                'Section and key
                Dim SectionTokenGeneral As JToken = SettingsToken(Section)
                Dim SectionToken As JToken = SectionTokenGeneral("Keys")
                Dim KeyToken As JToken = SectionToken.ToList(KeyNumber - 1)
                Dim MaxKeyOptions As Integer = 0

                'Key properties
                Dim KeyName As String = KeyToken("Name")
                Dim KeyDescription As String = KeyToken("Description")
                Dim KeyType As SettingsKeyType = [Enum].Parse(GetType(SettingsKeyType), KeyToken("Type"))
                Dim KeyVar As String = KeyToken("Variable")
                Dim KeyValue As Object = ""
                Dim KeyDefaultValue As Object = ""
                Dim KeyFinished As Boolean

                'Integer slider properties
                Dim IntSliderMinimumValue As Integer = If(KeyToken("MinimumValue"), 0)
                Dim IntSliderMaximumValue As Integer = If(KeyToken("MaximumValue"), 100)

                'Selection properties
                Dim KeyVarProperty As String = KeyToken("VariableProperty")
                Dim SelectionEnum As Boolean = If(KeyToken("IsEnumeration"), False)
                Dim SelectionEnumAssembly As String = KeyToken("EnumerationAssembly")
                Dim SelectionEnumInternal As Boolean = If(KeyToken("EnumerationInternal"), False)
                Dim SelectionEnumZeroBased As Boolean = If(KeyToken("EnumerationZeroBased"), False)

                'Variant properties
                Dim VariantValue As Object = ""
                Dim VariantValueFromExternalPrompt As Boolean

                'Color properties
                Dim ColorValue As Object = ""

                'List properties
                Dim ListJoinString As String = KeyToken("Delimiter")
                Dim ListJoinStringVariable As String = KeyToken("DelimiterVariable")
                Dim ListFunctionName As String = KeyToken("SelectionFunctionName")
                Dim ListFunctionType As String = KeyToken("SelectionFunctionType")
                Dim ListIsPathCurrentPath As Boolean = If(KeyToken("IsPathCurrentPath"), False)
                Dim ListValuePathType As KernelPathType = If(KeyToken("ValuePathType") IsNot Nothing, [Enum].Parse(GetType(KernelPathType), KeyToken("ValuePathType")), KernelPathType.Mods)
                Dim TargetList As IEnumerable(Of Object)
                Dim SelectFrom As IEnumerable(Of Object)
                Dim Selections As Object
                Dim NeutralizePaths As Boolean = If(KeyToken("IsValuePath"), False)
                Dim NeutralizeRootPath As String = If(ListIsPathCurrentPath, CurrentDir, GetKernelPath(ListValuePathType))

                'Inputs
                Dim AnswerString As String = ""
                Dim AnswerInt As Integer

                While Not KeyFinished
                    Console.Clear()

                    'Make an introductory banner
                    WriteSeparator(DoTranslation(Section + " Settings...") + " > " + DoTranslation(KeyName), True)
                    Write(NewLine + DoTranslation(KeyDescription), True, ColTypes.Neutral)

                    'See how to get the value
                    If Not KeyType = SettingsKeyType.SUnknown Then
                        If KeyType = SettingsKeyType.SSelection Then
                            If SelectionEnum Then
                                If SelectionEnumInternal Then
                                    'Apparently, we need to have a full assembly name for getting types.
                                    SelectFrom = Type.GetType("KS." + KeyToken("Enumeration").ToString + ", " + Assembly.GetExecutingAssembly.FullName).GetEnumNames
                                    Selections = Type.GetType("KS." + KeyToken("Enumeration").ToString + ", " + Assembly.GetExecutingAssembly.FullName).GetEnumValues
                                    MaxKeyOptions = SelectFrom.Count
                                Else
                                    SelectFrom = Type.GetType(KeyToken("Enumeration").ToString + ", " + SelectionEnumAssembly).GetEnumNames
                                    Selections = Type.GetType(KeyToken("Enumeration").ToString + ", " + SelectionEnumAssembly).GetEnumValues
                                    MaxKeyOptions = SelectFrom.Count
                                End If
                            Else
                                SelectFrom = GetMethod(KeyToken("SelectionFunctionName")).Invoke(KeyToken("SelectionFunctionType"), Nothing)
                                MaxKeyOptions = SelectFrom.Count
                            End If
                        ElseIf KeyType = SettingsKeyType.SList Then
                            TargetList = GetMethod(ListFunctionName).Invoke(ListFunctionType, Nothing)
                        End If
                        If KeyVarProperty Is Nothing Then
                            If CheckField(KeyVar) Then
                                'We're dealing with the field, get the value from it
                                KeyDefaultValue = GetValue(KeyVar)
                            ElseIf CheckProperty(KeyVar) Then
                                'We're dealing with the property, get the value from it
                                KeyDefaultValue = GetPropertyValue(KeyVar)
                            End If

                            'Get the plain sequence from the color
                            If TypeOf KeyDefaultValue Is Color Then
                                KeyDefaultValue = KeyDefaultValue.PlainSequence
                            End If
                        Else
                            'Get the property value from variable
                            KeyDefaultValue = GetPropertyValueInVariable(KeyVar, KeyVarProperty)
                        End If
                    End If

#Disable Warning BC42104
                    'If the type is boolean, write the two options
                    If KeyType = SettingsKeyType.SBoolean Then
                        Console.WriteLine()
                        MaxKeyOptions = 2
                        Write(" 1) " + DoTranslation("Enable"), True, ColTypes.Option)
                        Write(" 2) " + DoTranslation("Disable"), True, ColTypes.Option)
                    End If
                    Console.WriteLine()

                    'If the type is a color, initialize the color wheel
                    If KeyType = SettingsKeyType.SColor Then
                        ColorValue = ColorWheel(New Color(KeyDefaultValue.ToString).Type = ColorType.TrueColor, If(New Color(KeyDefaultValue.ToString).Type = ColorType._255Color, New Color(KeyDefaultValue.ToString).PlainSequence, ConsoleColors.White), New Color(KeyDefaultValue.ToString).R, New Color(KeyDefaultValue.ToString).G, New Color(KeyDefaultValue.ToString).B)
                    End If

                    'Write the list from the current items
                    If KeyType = SettingsKeyType.SSelection Then
                        Write(DoTranslation("Current items:"), True, ColTypes.ListTitle)
                        WriteList(SelectFrom)
                        Console.WriteLine()
                    ElseIf KeyType = SettingsKeyType.SList Then
                        Write(DoTranslation("Current items:"), True, ColTypes.ListTitle)
                        WriteList(TargetList)
                        Console.WriteLine()
                    End If

                    'Add an option to go back.
                    If Not KeyType = SettingsKeyType.SVariant And Not KeyType = SettingsKeyType.SInt And Not KeyType = SettingsKeyType.SString And
                       Not KeyType = SettingsKeyType.SList And Not KeyType = SettingsKeyType.SMaskedString And Not KeyType = SettingsKeyType.SChar And
                       Not KeyType = SettingsKeyType.SIntSlider Then
                        Write(" {0}) " + DoTranslation("Go Back...") + NewLine, True, ColTypes.BackOption, MaxKeyOptions + 1)
                    ElseIf KeyType = SettingsKeyType.SList Then
                        Write(NewLine + " q) " + DoTranslation("Save Changes...") + NewLine, True, ColTypes.Option, MaxKeyOptions + 1)
                    End If

                    'Print debugging info
                    Wdbg(DebugLevel.W, "Key {0} in section {1} has {2} selections.", KeyNumber, Section, MaxKeyOptions)
                    Wdbg(DebugLevel.W, "Target variable: {0}, Key Type: {1}, Key value: {2}, Variant Value: {3}", KeyVar, KeyType, KeyValue, VariantValue)

                    'Prompt user
                    If KeyType = SettingsKeyType.SVariant And Not VariantValueFromExternalPrompt Then
                        Write("> ", False, ColTypes.Input)
                        VariantValue = ReadLine()
                        If NeutralizePaths Then VariantValue = NeutralizePath(VariantValue, NeutralizeRootPath)
                        Wdbg(DebugLevel.I, "User answered {0}", VariantValue)
                    ElseIf Not KeyType = SettingsKeyType.SVariant And Not KeyType = SettingsKeyType.SColor Then
                        If KeyType = SettingsKeyType.SList Then
                            Write("> ", False, ColTypes.Input)
                            Do Until AnswerString = "q"
                                AnswerString = ReadLine()
                                If Not AnswerString = "q" Then
                                    If NeutralizePaths Then AnswerString = NeutralizePath(AnswerString, NeutralizeRootPath)
                                    If Not AnswerString.StartsWith("-") Then
                                        'We're not removing an item!
                                        TargetList = Enumerable.Append(TargetList, AnswerString)
                                    Else
                                        'We're removing an item.
                                        Dim DeletedItems As IEnumerable(Of Object) = Enumerable.Empty(Of Object)
                                        DeletedItems = Enumerable.Append(DeletedItems, AnswerString.Substring(1))
                                        TargetList = Enumerable.Except(TargetList, DeletedItems)
                                    End If
                                    Wdbg(DebugLevel.I, "Added answer {0} to list.", AnswerString)
                                    Write("> ", False, ColTypes.Input)
                                End If
                            Loop
                        Else
                            'Make a prompt
                            If KeyType = SettingsKeyType.SUnknown Or KeyType = SettingsKeyType.SMaskedString Then
                                Write("> ", False, ColTypes.Input)
                            ElseIf Not KeyType = SettingsKeyType.SIntSlider Then
                                Write("[{0}] > ", False, ColTypes.Input, KeyDefaultValue)
                            End If

                            'Select how to present input
                            If KeyType = SettingsKeyType.SMaskedString Then
                                AnswerString = ReadLineNoInput()
                            ElseIf KeyType = SettingsKeyType.SChar Then
                                AnswerString = Console.ReadKey().KeyChar
                            ElseIf KeyType = SettingsKeyType.SIntSlider Then
                                Dim PressedKey As ConsoleKey
                                Dim CurrentValue As Integer = KeyDefaultValue
                                Console.CursorVisible = False
                                Do Until PressedKey = ConsoleKey.Enter
                                    'Draw the progress bar
                                    WriteProgress(100 * (CurrentValue / IntSliderMaximumValue), 4, Console.WindowHeight - 4)

                                    'Show the current value
                                    WriteWhere(DoTranslation("Current value:") + " {0} / {1} - {2}" + GetEsc() + "[0K", 5, Console.WindowHeight - 5, False, ColTypes.Neutral, CurrentValue, IntSliderMinimumValue, IntSliderMaximumValue)

                                    'Parse the user input
                                    PressedKey = Console.ReadKey().Key
                                    Select Case PressedKey
                                        Case ConsoleKey.LeftArrow
                                            If CurrentValue > IntSliderMinimumValue Then CurrentValue -= 1
                                        Case ConsoleKey.RightArrow
                                            If CurrentValue < IntSliderMaximumValue Then CurrentValue += 1
                                        Case ConsoleKey.Enter
                                            AnswerString = CurrentValue
                                            Console.CursorVisible = True
                                    End Select
                                Loop
                            Else
                                AnswerString = ReadLine()
                            End If

                            'Neutralize answer path if required
                            If NeutralizePaths Then AnswerString = NeutralizePath(AnswerString, NeutralizeRootPath)
                            Wdbg(DebugLevel.I, "User answered {0}", AnswerString)
                        End If
                    End If

                    'Check for input
                    Wdbg(DebugLevel.I, "Is the answer numeric? {0}", IsStringNumeric(AnswerString))
                    If Integer.TryParse(AnswerString, AnswerInt) Then
                        'The answer is numeric! Now, check for types
                        Select Case KeyType
                            Case SettingsKeyType.SBoolean
                                'We're dealing with boolean
                                Wdbg(DebugLevel.I, "Answer is numeric and key is of the Boolean type.")
                                If AnswerInt >= 1 And AnswerInt <= MaxKeyOptions Then
                                    Dim FinalBool As Boolean
                                    Wdbg(DebugLevel.I, "Translating {0} to the boolean equivalent...", AnswerInt)
                                    KeyFinished = True

                                    'Set boolean
                                    Select Case AnswerInt
                                        Case 1 'True
                                            Wdbg(DebugLevel.I, "Setting to True...")
                                            FinalBool = True
                                        Case 2 'False
                                            Wdbg(DebugLevel.I, "Setting to False...")
                                            FinalBool = False
                                    End Select

                                    'Now, set the value
                                    If CheckField(KeyVar) Then
                                        'We're dealing with the field
                                        SetValue(KeyVar, FinalBool)
                                    ElseIf CheckProperty(KeyVar) Then
                                        'We're dealing with the property
                                        SetPropertyValue(KeyVar, FinalBool)
                                    End If
                                ElseIf AnswerInt = MaxKeyOptions + 1 Then 'Go Back...
                                    Wdbg(DebugLevel.I, "User requested exit. Returning...")
                                    KeyFinished = True
                                Else
                                    Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                                    Write(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerInt)
                                    Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                                    Console.ReadKey()
                                End If
                            Case SettingsKeyType.SSelection
                                'We're dealing with selection
                                Wdbg(DebugLevel.I, "Answer is numeric and key is of the selection type.")
                                Dim AnswerIndex As Integer = AnswerInt - 1
                                If AnswerInt = MaxKeyOptions + 1 Then 'Go Back...
                                    Wdbg(DebugLevel.I, "User requested exit. Returning...")
                                    KeyFinished = True
                                ElseIf AnswerInt > 0 Then
                                    If Selections IsNot Nothing Then
                                        Wdbg(DebugLevel.I, "Setting variable {0} to item index {1}...", KeyVar, AnswerInt)
                                        KeyFinished = True

                                        'Now, set the value
                                        If CheckField(KeyVar) Then
                                            'We're dealing with the field
                                            SetValue(KeyVar, Selections(AnswerIndex))
                                        ElseIf CheckProperty(KeyVar) Then
                                            'We're dealing with the property
                                            SetPropertyValue(KeyVar, Selections(AnswerIndex))
                                        End If
                                    ElseIf Not AnswerInt > MaxKeyOptions Then
                                        Dim FinalValue As Object
                                        If Not SelectionEnum Then
                                            Wdbg(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt)
                                            KeyFinished = True
                                            FinalValue = SelectFrom(AnswerInt - 1)
                                        Else
                                            Wdbg(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt)
                                            KeyFinished = True
                                            FinalValue = AnswerInt
                                        End If

                                        'Now, set the value
                                        If CheckField(KeyVar) Then
                                            'We're dealing with the field
                                            SetValue(KeyVar, FinalValue)
                                        ElseIf CheckProperty(KeyVar) Then
                                            'We're dealing with the property
                                            SetPropertyValue(KeyVar, FinalValue)
                                        End If
                                    Else
                                        Wdbg(DebugLevel.W, "Answer is not valid.")
                                        Write(DoTranslation("The answer may not exceed the entries shown."), True, ColTypes.Error)
                                        Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                                        Console.ReadKey()
                                    End If
                                ElseIf AnswerInt = 0 And Not SelectionEnumZeroBased Then
                                    Wdbg(DebugLevel.W, "Zero is not allowed.")
                                    Write(DoTranslation("The answer may not be zero."), True, ColTypes.Error)
                                    Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                                    Console.ReadKey()
                                Else
                                    Wdbg(DebugLevel.W, "Negative values are disallowed.")
                                    Write(DoTranslation("The answer may not be negative."), True, ColTypes.Error)
                                    Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                                    Console.ReadKey()
                                End If
                            Case SettingsKeyType.SInt
                                'We're dealing with integers
                                Wdbg(DebugLevel.I, "Answer is numeric and key is of the integer type.")
                                Dim AnswerIndex As Integer = AnswerInt - 1
                                If AnswerInt >= 0 Then
                                    Wdbg(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt)
                                    KeyFinished = True

                                    'Now, set the value
                                    If CheckField(KeyVar) Then
                                        'We're dealing with the field
                                        SetValue(KeyVar, AnswerInt)
                                    ElseIf CheckProperty(KeyVar) Then
                                        'We're dealing with the property
                                        SetPropertyValue(KeyVar, AnswerInt)
                                    End If
                                Else
                                    Wdbg(DebugLevel.W, "Negative values are disallowed.")
                                    Write(DoTranslation("The answer may not be negative."), True, ColTypes.Error)
                                    Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                                    Console.ReadKey()
                                End If
                            Case SettingsKeyType.SIntSlider
                                'We're dealing with integers with limits
                                Wdbg(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt)
                                KeyFinished = True

                                'Now, set the value
                                If CheckField(KeyVar) Then
                                    'We're dealing with the field
                                    SetValue(KeyVar, AnswerInt)
                                ElseIf CheckProperty(KeyVar) Then
                                    'We're dealing with the property
                                    SetPropertyValue(KeyVar, AnswerInt)
                                End If
                        End Select
                    ElseIf ReadLineReboot.ReadLine.ReadRanToCompletion Then
                        Select Case KeyType
                            Case SettingsKeyType.SString, SettingsKeyType.SMaskedString, SettingsKeyType.SChar
                                Wdbg(DebugLevel.I, "Answer is not numeric and key is of the String or Char (inferred from keytype {0}) type. Setting variable...", KeyType.ToString)

                                'Check to see if written answer is empty
                                If String.IsNullOrWhiteSpace(AnswerString) Then
                                    Wdbg(DebugLevel.I, "Answer is nothing. Setting to {0}...", KeyValue)
                                    AnswerString = KeyValue
                                End If

                                'Check to see if the user intended to clear the variable to make it consist of nothing
                                If AnswerString.ToLower = "/clear" Then
                                    Wdbg(DebugLevel.I, "User requested clear.")
                                    AnswerString = ""
                                End If

                                'Set the value
                                KeyFinished = True
                                If CheckField(KeyVar) Then
                                    'We're dealing with the field
                                    SetValue(KeyVar, AnswerString)
                                ElseIf CheckProperty(KeyVar) Then
                                    'We're dealing with the property
                                    SetPropertyValue(KeyVar, AnswerString)
                                End If
                            Case SettingsKeyType.SList
                                Dim FinalDelimiter As String
                                Wdbg(DebugLevel.I, "Answer is not numeric and key is of the List type. Adding answers to the list...")
                                KeyFinished = True

                                'Get the delimiter
                                If ListJoinString Is Nothing Then
                                    FinalDelimiter = GetValue(ListJoinStringVariable)
                                Else
                                    FinalDelimiter = ListJoinString
                                End If

                                'Now, set the value
                                Dim JoinedString As String = String.Join(FinalDelimiter, TargetList)
                                If CheckField(KeyVar) Then
                                    'We're dealing with the field
                                    SetValue(KeyVar, JoinedString)
                                ElseIf CheckProperty(KeyVar) Then
                                    'We're dealing with the property
                                    SetPropertyValue(KeyVar, JoinedString)
                                End If
                            Case SettingsKeyType.SVariant
                                'Now, set the value
                                If CheckField(KeyVar) Then
                                    'We're dealing with the field
                                    SetValue(KeyVar, VariantValue)
                                ElseIf CheckProperty(KeyVar) Then
                                    'We're dealing with the property
                                    SetPropertyValue(KeyVar, VariantValue)
                                End If
                                KeyFinished = True
                            Case SettingsKeyType.SColor
                                Dim FinalColor As Object
                                If GetField(KeyVar).FieldType = GetType(Color) Then
                                    FinalColor = New Color(ColorValue.ToString)
                                Else
                                    FinalColor = ColorValue.ToString
                                End If

                                'Now, set the value
                                If CheckField(KeyVar) Then
                                    'We're dealing with the field
                                    SetValue(KeyVar, FinalColor)
                                ElseIf CheckProperty(KeyVar) Then
                                    'We're dealing with the property
                                    SetPropertyValue(KeyVar, FinalColor)
                                End If
                                KeyFinished = True
                            Case SettingsKeyType.SUnknown
                                Wdbg(DebugLevel.I, "User requested exit. Returning...")
                                KeyFinished = True
                            Case Else
                                Wdbg(DebugLevel.W, "Answer is not valid.")
                                Write(DoTranslation("The answer is invalid. Check to make sure that the answer is numeric for config entries that need numbers as answers."), True, ColTypes.Error)
                                Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                                Console.ReadKey()
                        End Select
                    End If
#Enable Warning BC42104
                End While
            Catch ex As Exception
                Console.Clear()
                Wdbg(DebugLevel.I, "Error trying to open section: {0}", ex.Message)
                WStkTrc(ex)
                WriteSeparator(DoTranslation(Section + " Settings...") + " > ???", True)
                Write(NewLine + "X) " + DoTranslation("Invalid section entered. Please go back."), True, ColTypes.Error)
                Write("X) " + DoTranslation("If you're sure that you've opened the right section, check this message out:"), True, ColTypes.Error)
                Write("X) " + ex.Message, True, ColTypes.Error)
                Console.ReadKey()
            End Try
        End Sub

        ''' <summary>
        ''' A sub for variable finding prompt
        ''' </summary>
        Sub VariableFinder(SettingsToken As JToken)
            Dim SearchFor As String
            Dim SettingsNumber As String
            Dim Results As List(Of String)

            'Prompt the user
            Write(DoTranslation("Write what do you want to search for."), True, ColTypes.Neutral)
            Wdbg(DebugLevel.I, "Prompting user for searching...")
            Write(">> ", False, ColTypes.Input)
            SearchFor = ReadLine()

            'Search for the setting
            Results = FindSetting(SearchFor, SettingsToken)

            'Write the settings
            If Not Results.Count = 0 Then
                WriteList(Results)

                'Prompt for the number of setting to go to
                Write(DoTranslation("Write the number of the setting to go to. Any other character means go back."), True, ColTypes.Neutral)
                Wdbg(DebugLevel.I, "Prompting user for writing...")
                Write(">> ", False, ColTypes.Input)
                SettingsNumber = ReadLine()

                'Parse the input and go to setting
                If IsStringNumeric(SettingsNumber) Then
                    Dim ChosenSettingIndex As Integer = CInt(SettingsNumber) - 1
                    Dim ChosenSetting As String = Results(ChosenSettingIndex)
                    Dim SectionIndex As Integer = CInt(ChosenSetting.AsSpan.Slice(1, ChosenSetting.IndexOf("/") - 1).ToString) - 1
                    Dim KeyNumber As Integer = ChosenSetting.AsSpan.Slice(ChosenSetting.IndexOf("/") + 1, ChosenSetting.IndexOf("]") - (ChosenSetting.IndexOf("/") + 1)).ToString
                    Dim Section As JProperty = SettingsToken.ToList()(SectionIndex)
                    Dim SectionName As String = Section.Name
                    OpenKey(SectionName, KeyNumber, SettingsToken)
                End If
            Else
                Write(DoTranslation("Nothing is found. Make sure that you've written the setting correctly."), True, ColTypes.Error)
                Console.ReadKey()
            End If
        End Sub

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
        ''' Checks all the settings variables to see if they can be parsed
        ''' </summary>
        Public Function CheckSettingsVariables() As Dictionary(Of String, Boolean)
            Dim SettingsToken As JToken = JToken.Parse(My.Resources.SettingsEntries)
            Dim SaverSettingsToken As JToken = JToken.Parse(My.Resources.ScreensaverSettingsEntries)
            Dim SplashSettingsToken As JToken = JToken.Parse(My.Resources.SplashSettingsEntries)
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

        ''' <summary>
        ''' Open the settings resource
        ''' </summary>
        ''' <param name="SettingsType">The settings type</param>
        Private Function OpenSettingsResource(SettingsType As SettingsType) As JToken
            Select Case SettingsType
                Case SettingsType.Normal
                    Return JToken.Parse(My.Resources.SettingsEntries)
                Case SettingsType.Screensaver
                    Return JToken.Parse(My.Resources.ScreensaverSettingsEntries)
                Case SettingsType.Splash
                    Return JToken.Parse(My.Resources.SplashSettingsEntries)
                Case Else
                    Return JToken.Parse(My.Resources.SettingsEntries)
            End Select
        End Function

    End Module
End Namespace