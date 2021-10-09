
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

Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports Newtonsoft.Json.Linq

Public Module SettingsApp

    ''' <summary>
    ''' Key type for settings entry
    ''' </summary>
    Enum SettingsKeyType
        SUnknown
        SBoolean
        SInt
        SString
        SLongString
        SSelection
        SList
        SVariant
        SColor
        SMaskedString
    End Enum

    ''' <summary>
    ''' Main page
    ''' </summary>
    Sub OpenMainPage(Screensaver As Boolean)
        Dim PromptFinished As Boolean
        Dim AnswerString As String
        Dim AnswerInt As Integer
        Dim SettingsToken As JToken = JToken.Parse(If(Screensaver, My.Resources.ScreensaverSettingsEntries, My.Resources.SettingsEntries))
        Dim MaxSections As Integer = SettingsToken.Count

        While Not PromptFinished
            Console.Clear()
            'List sections
            WriteSeparator(DoTranslation("Welcome to Settings!"), True)
            W(vbNewLine + DoTranslation("Select section:") + vbNewLine, True, ColTypes.Neutral)
            For SectionIndex As Integer = 0 To MaxSections - 1
                Dim Section As JProperty = SettingsToken.ToList(SectionIndex)
                If Screensaver Then
                    W(" {0}) " + Section.Name + "...", True, ColTypes.Option, SectionIndex + 1)
                Else
                    W(" {0}) " + DoTranslation(Section.Name + " Settings..."), True, ColTypes.Option, SectionIndex + 1)
                End If
            Next
            Console.WriteLine()
            W(" {0}) " + DoTranslation("Save Settings"), True, ColTypes.BackOption, MaxSections + 1)
            W(" {0}) " + DoTranslation("Exit"), True, ColTypes.BackOption, MaxSections + 2)

            'Prompt user and check for input
            Console.WriteLine()
            W("> ", False, ColTypes.Input)
            AnswerString = Console.ReadLine
            Wdbg(DebugLevel.I, "User answered {0}", AnswerString)
            Console.WriteLine()

            Wdbg(DebugLevel.I, "Is the answer numeric? {0}", IsNumeric(AnswerString))
            If Integer.TryParse(AnswerString, AnswerInt) Then
                Wdbg(DebugLevel.I, "Succeeded. Checking the answer if it points to the right direction...")
                If AnswerInt >= 1 And AnswerInt <= MaxSections Then
                    Dim SelectedSection As JProperty = SettingsToken.ToList(AnswerInt - 1)
                    Wdbg(DebugLevel.I, "Opening section {0}...", SelectedSection.Name)
                    OpenSection(SelectedSection.Name, SettingsToken)
                ElseIf AnswerInt = MaxSections + 1 Then 'Save Settings
                    Wdbg(DebugLevel.I, "Saving settings...")
                    Try
                        CreateConfig()
                        SaveCustomSaverSettings()
                    Catch ex As Exception
                        W(ex.Message, True, ColTypes.Error)
                        WStkTrc(ex)
                        Console.ReadKey()
                    End Try
                ElseIf AnswerInt = MaxSections + 2 Then 'Exit
                    Wdbg(DebugLevel.W, "Exiting...")
                    PromptFinished = True
                    Console.Clear()
                Else
                    Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerInt)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            Else
                Wdbg(DebugLevel.W, "Answer is not numeric.")
                W(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
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
            Dim MaxOptions As Integer = SectionToken.Count - 1

            While Not SectionFinished
                Console.Clear()
                WriteSeparator(DoTranslation(Section + " Settings..."), True)
                W(vbNewLine + DoTranslation(SectionDescription) + vbNewLine, True, ColTypes.Neutral)

                'List options
                For SectionIndex As Integer = 0 To MaxOptions - 1
                    Dim Setting As JToken = SectionToken(SectionIndex)
                    Dim CurrentValue As Object
                    Dim Variable As String = Setting("Variable")
                    Dim VariableProperty As String = Setting("VariableProperty")

                    'Determine how to get the current value
                    If VariableProperty Is Nothing Then
                        CurrentValue = GetConfigValueField(Variable)
                    Else
                        CurrentValue = GetConfigPropertyValueInVariableField(Variable, VariableProperty)
                    End If
                    W(" {0}) " + DoTranslation(Setting("Name")) + " [{1}]", True, ColTypes.Option, SectionIndex + 1, CurrentValue)
                Next
                Console.WriteLine()
                W(" {0}) " + DoTranslation("Go Back...") + vbNewLine, True, ColTypes.BackOption, MaxOptions + 1)
                Wdbg(DebugLevel.W, "Section {0} has {1} selections.", Section, MaxOptions)

                'Prompt user and check for input
                W("> ", False, ColTypes.Input)
                AnswerString = Console.ReadLine
                Wdbg(DebugLevel.I, "User answered {0}", AnswerString)
                Console.WriteLine()

                Wdbg(DebugLevel.I, "Is the answer numeric? {0}", IsNumeric(AnswerString))
                If Integer.TryParse(AnswerString, AnswerInt) Then
                    Wdbg(DebugLevel.I, "Succeeded. Checking the answer if it points to the right direction...")
                    If AnswerInt >= 1 And AnswerInt <= MaxOptions Then
                        Wdbg(DebugLevel.I, "Opening key {0} from section {1}...", AnswerInt, Section)
                        OpenKey(Section, AnswerInt, SettingsToken)
                    ElseIf AnswerInt = MaxOptions + 1 Then 'Go Back...
                        Wdbg(DebugLevel.I, "User requested exit. Returning...")
                        SectionFinished = True
                    Else
                        Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                        W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerInt)
                        W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                        Console.ReadKey()
                    End If
                Else
                    Wdbg(DebugLevel.W, "Answer is not numeric.")
                    W(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            End While
        Catch ex As Exception
            Console.Clear()
            Wdbg(DebugLevel.I, "Error trying to open section: {0}", ex.Message)
            WriteSeparator("???", True)
            W(vbNewLine + "X) " + DoTranslation("Invalid section entered. Please go back."), True, ColTypes.Error)
            W("X) " + DoTranslation("If you're sure that you've opened the right section, check this message out:"), True, ColTypes.Error)
            W("X) " + ex.Message, True, ColTypes.Error)
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
            Dim SectionTokenGeneral As JToken = SettingsToken(Section)
            Dim SectionToken As JToken = SectionTokenGeneral("Keys")
            Dim KeyToken As JToken = SectionToken.ToList(KeyNumber - 1)
            Dim MaxKeyOptions As Integer = 0
            Dim KeyName As String = KeyToken("Name")
            Dim KeyDescription As String = KeyToken("Description")
            Dim KeyType As SettingsKeyType = [Enum].Parse(GetType(SettingsKeyType), KeyToken("Type"))
            Dim KeyVar As String = KeyToken("Variable")
            Dim KeyVarProperty As String = KeyToken("VariableProperty")
            Dim KeyValue As Object = ""
            Dim KeyDefaultValue As Object = ""
            Dim KeyFinished As Boolean
            Dim SelectionEnum As Boolean = If(KeyToken("IsEnumeration"), False)
            Dim SelectionEnumAssembly As String = KeyToken("EnumerationAssembly")
            Dim SelectionEnumInternal As Boolean = If(KeyToken("EnumerationInternal"), False)
            Dim SelectionEnumZeroBased As Boolean = If(KeyToken("EnumerationZeroBased"), False)
            Dim VariantValue As Object = ""
            Dim VariantValueFromExternalPrompt As Boolean
            Dim ColorValue As Object = ""
            Dim AnswerString As String = ""
            Dim AnswerInt As Integer
            Dim ListJoinString As String = KeyToken("Delimiter")
            Dim ListJoinStringVariable As String = KeyToken("DelimiterVariable")
            Dim ListFunctionName As String = KeyToken("SelectionFunctionName")
            Dim ListFunctionType As String = KeyToken("SelectionFunctionType")
            Dim TargetList As IEnumerable(Of Object)
            Dim SelectFrom As IEnumerable(Of Object)
            Dim Selections As Object
            Dim NeutralizePaths As Boolean
            Dim NeutralizeRootPath As String = CurrDir

            While Not KeyFinished
                Console.Clear()

                'Make an introductory banner
                WriteSeparator(DoTranslation(Section + " Settings...") + " > " + DoTranslation(KeyName), True)
                W(vbNewLine + DoTranslation(KeyDescription), True, ColTypes.Neutral)

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
                        KeyDefaultValue = GetConfigValueField(KeyVar)
                    Else
                        KeyDefaultValue = GetConfigPropertyValueInVariableField(KeyVar, KeyVarProperty)
                    End If
                End If

#Disable Warning BC42104
                'If the type is boolean, write the two options
                If KeyType = SettingsKeyType.SBoolean Then
                    Console.WriteLine()
                    MaxKeyOptions = 2
                    W(" 1) " + DoTranslation("Enable"), True, ColTypes.Option)
                    W(" 2) " + DoTranslation("Disable"), True, ColTypes.Option)
                End If
                Console.WriteLine()

                'If the type is a color, initialize the color wheel
                If KeyType = SettingsKeyType.SColor Then
                    ColorValue = ColorWheel(New Color(KeyDefaultValue).Type = ColorType.TrueColor, If(New Color(KeyDefaultValue).Type = ColorType._255Color, New Color(KeyDefaultValue).PlainSequence, ConsoleColors.White), New Color(KeyDefaultValue).R, New Color(KeyDefaultValue).G, New Color(KeyDefaultValue).B)
                End If

                If KeyType = SettingsKeyType.SSelection Then
                    W(DoTranslation("Current items:"), True, ColTypes.ListTitle)
                    WriteList(SelectFrom)
                    Console.WriteLine()
                ElseIf KeyType = SettingsKeyType.SList Then
                    W(DoTranslation("Current items:"), True, ColTypes.ListTitle)
                    WriteList(TargetList)
                    Console.WriteLine()
                End If

                'Add an option to go back.
                If Not KeyType = SettingsKeyType.SVariant And Not KeyType = SettingsKeyType.SInt And Not KeyType = SettingsKeyType.SLongString And Not KeyType = SettingsKeyType.SString And Not KeyType = SettingsKeyType.SList Then
                    W(" {0}) " + DoTranslation("Go Back...") + vbNewLine, True, ColTypes.BackOption, MaxKeyOptions + 1)
                ElseIf KeyType = SettingsKeyType.SList Then
                    W(vbNewLine + " q) " + DoTranslation("Save Changes...") + vbNewLine, True, ColTypes.Option, MaxKeyOptions + 1)
                End If

                'Print debugging info
                Wdbg(DebugLevel.W, "Key {0} in section {1} has {2} selections.", KeyNumber, Section, MaxKeyOptions)
                Wdbg(DebugLevel.W, "Target variable: {0}, Key Type: {1}, Key value: {2}, Variant Value: {3}", KeyVar, KeyType, KeyValue, VariantValue)

                'Prompt user
                If KeyType = SettingsKeyType.SVariant And Not VariantValueFromExternalPrompt Then
                    W("> ", False, ColTypes.Input)
                    VariantValue = Console.ReadLine
                    If NeutralizePaths Then AnswerString = NeutralizePath(AnswerString, NeutralizeRootPath)
                    Wdbg(DebugLevel.I, "User answered {0}", VariantValue)
                ElseIf Not KeyType = SettingsKeyType.SVariant And Not KeyType = SettingsKeyType.SColor Then
                    If KeyType = SettingsKeyType.SList Then
                        W("> ", False, ColTypes.Input)
                        Do Until AnswerString = "q"
                            AnswerString = Console.ReadLine
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
                                W("> ", False, ColTypes.Input)
                            End If
                        Loop
                    Else
                        W(If(KeyType = SettingsKeyType.SUnknown, "> ", "[{0}] > "), False, ColTypes.Input, KeyDefaultValue)
                        If KeyType = SettingsKeyType.SLongString Then
                            AnswerString = ReadLineLong()
                        ElseIf KeyType = SettingsKeyType.SMaskedString Then
                            AnswerString = ReadLineNoInput("*")
                        Else
                            AnswerString = Console.ReadLine
                        End If
                        If NeutralizePaths Then AnswerString = NeutralizePath(AnswerString, NeutralizeRootPath)
                        Wdbg(DebugLevel.I, "User answered {0}", AnswerString)
                    End If
                End If

                'Check for input
                Wdbg(DebugLevel.I, "Is the answer numeric? {0}", IsNumeric(AnswerString))
                If Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SBoolean Then
                    Wdbg(DebugLevel.I, "Answer is numeric and key is of the Boolean type.")
                    If AnswerInt >= 1 And AnswerInt <= MaxKeyOptions Then
                        Wdbg(DebugLevel.I, "Translating {0} to the boolean equivalent...", AnswerInt)
                        KeyFinished = True
                        Select Case AnswerInt
                            Case 1 'True
                                Wdbg(DebugLevel.I, "Setting to True...")
                                SetConfigValueField(KeyVar, True)
                            Case 2 'False
                                Wdbg(DebugLevel.I, "Setting to False...")
                                SetConfigValueField(KeyVar, False)
                        End Select
                    ElseIf AnswerInt = MaxKeyOptions + 1 Then 'Go Back...
                        Wdbg(DebugLevel.I, "User requested exit. Returning...")
                        KeyFinished = True
                    Else
                        Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                        W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerInt)
                        W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                        Console.ReadKey()
                    End If
                ElseIf (Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SInt) Or
                       (Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SSelection) Then
                    Wdbg(DebugLevel.I, "Answer is numeric and key is of the {0} type.", KeyType)
                    Dim AnswerIndex As Integer = AnswerInt - 1
                    If AnswerInt = MaxKeyOptions + 1 And KeyType = SettingsKeyType.SSelection Then 'Go Back...
                        Wdbg(DebugLevel.I, "User requested exit. Returning...")
                        KeyFinished = True
                    ElseIf KeyType = SettingsKeyType.SSelection And AnswerInt > 0 And Selections IsNot Nothing Then
                        Wdbg(DebugLevel.I, "Setting variable {0} to item index {1}...", KeyVar, AnswerInt)
                        KeyFinished = True
                        SetConfigValueField(KeyVar, Selections(AnswerIndex))
                    ElseIf (KeyType = SettingsKeyType.SSelection And AnswerInt > 0) Or
                           (KeyType = SettingsKeyType.SInt And AnswerInt >= 0) Then
                        If (KeyType = SettingsKeyType.SSelection And Not AnswerInt > MaxKeyOptions) Or KeyType = SettingsKeyType.SInt Then
                            If Not SelectionEnum Then
                                Wdbg(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt)
                                KeyFinished = True
                                SetConfigValueField(KeyVar, SelectFrom(AnswerInt - 1))
                            Else
                                Wdbg(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt)
                                KeyFinished = True
                                SetConfigValueField(KeyVar, AnswerInt)
                            End If
                        ElseIf KeyType = SettingsKeyType.SSelection Then
                            Wdbg(DebugLevel.W, "Answer is not valid.")
                            W(DoTranslation("The answer may not exceed the entries shown."), True, ColTypes.Error)
                            W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                            Console.ReadKey()
                        End If
                    ElseIf AnswerInt = 0 And Not SelectionEnumZeroBased Then
                        Wdbg(DebugLevel.W, "Zero is not allowed.")
                        W(DoTranslation("The answer may not be zero."), True, ColTypes.Error)
                        W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                        Console.ReadKey()
                    Else
                        Wdbg(DebugLevel.W, "Negative values are disallowed.")
                        W(DoTranslation("The answer may not be negative."), True, ColTypes.Error)
                        W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                        Console.ReadKey()
                    End If
                ElseIf KeyType = SettingsKeyType.SUnknown Then
                    Wdbg(DebugLevel.I, "User requested exit. Returning...")
                    KeyFinished = True
                ElseIf KeyType = SettingsKeyType.SString Or KeyType = SettingsKeyType.SLongString Or KeyType = SettingsKeyType.SMaskedString Then
                    Wdbg(DebugLevel.I, "Answer is not numeric and key is of the String type. Setting variable...")

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
                    SetConfigValueField(KeyVar, AnswerString)
                ElseIf KeyType = SettingsKeyType.SList Then
                    Dim FinalDelimiter As String
                    Wdbg(DebugLevel.I, "Answer is not numeric and key is of the List type. Adding answers to the list...")
                    KeyFinished = True
                    If ListJoinString Is Nothing Then
                        FinalDelimiter = GetConfigValueField(ListJoinStringVariable)
                    Else
                        FinalDelimiter = ListJoinString
                    End If
                    SetConfigValueField(KeyVar, String.Join(FinalDelimiter, TargetList))
                ElseIf KeyType = SettingsKeyType.SVariant Then
                    SetConfigValueField(KeyVar, VariantValue)
                    Wdbg(DebugLevel.I, "User requested exit. Returning...")
                    KeyFinished = True
                ElseIf KeyType = SettingsKeyType.SColor Then
                    SetConfigValueField(KeyVar, New Color(ColorValue).PlainSequence)
                    Wdbg(DebugLevel.I, "User requested exit. Returning...")
                    KeyFinished = True
                Else
                    Wdbg(DebugLevel.W, "Answer is not valid.")
                    W(DoTranslation("The answer is invalid. Check to make sure that the answer is numeric for config entries that need numbers as answers."), True, ColTypes.Error)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
#Enable Warning BC42104
            End While
        Catch ex As Exception
            Console.Clear()
            Wdbg(DebugLevel.I, "Error trying to open section: {0}", ex.Message)
            WriteSeparator(DoTranslation(Section + " Settings...") + " > ???", True)
            W(vbNewLine + "X) " + DoTranslation("Invalid section entered. Please go back."), True, ColTypes.Error)
            W("X) " + DoTranslation("If you're sure that you've opened the right section, check this message out:"), True, ColTypes.Error)
            W("X) " + ex.Message, True, ColTypes.Error)
            Console.ReadKey()
        End Try
    End Sub

    ''' <summary>
    ''' Sets the value of a variable to the new value dynamically
    ''' </summary>
    ''' <param name="Variable">Variable name. Use operator NameOf to get name.</param>
    ''' <param name="VariableValue">New value of variable</param>
    Public Sub SetConfigValueField(Variable As String, VariableValue As Object)
        'Get field for specified variable
        Dim TargetField As FieldInfo = GetField(Variable)

        'Set the variable if found
        If TargetField IsNot Nothing Then
            'The "obj" description says this: "The object whose field value will be set."
            'Apparently, SetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
            'Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
            Wdbg(DebugLevel.I, "Got field {0}. Setting to {1}...", TargetField.Name, VariableValue)
            TargetField.SetValue(Variable, VariableValue)
        Else
            'Variable not found on any of the "flag" modules.
            Wdbg(DebugLevel.I, "Field {0} not found.", Variable)
            W(DoTranslation("Variable {0} is not found on any of the modules."), True, ColTypes.Error, Variable)
        End If
    End Sub

    ''' <summary>
    ''' Gets the value of a variable dynamically 
    ''' </summary>
    ''' <param name="Variable">Variable name. Use operator NameOf to get name.</param>
    ''' <returns>Value of a variable</returns>
    Public Function GetConfigValueField(Variable As String) As Object
        'Get field for specified variable
        Dim TargetField As FieldInfo = GetField(Variable)

        'Get the variable if found
        If TargetField IsNot Nothing Then
            'The "obj" description says this: "The object whose field value will be returned."
            'Apparently, GetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
            'Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
            Wdbg(DebugLevel.I, "Got field {0}.", TargetField.Name)
            Return TargetField.GetValue(Variable)
        Else
            'Variable not found on any of the "flag" modules.
            Wdbg(DebugLevel.I, "Field {0} not found.", Variable)
            W(DoTranslation("Variable {0} is not found on any of the modules."), True, ColTypes.Error, Variable)
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Gets the value of a property in the type of a variable dynamically
    ''' </summary>
    ''' <param name="Variable">Variable name. Use operator NameOf to get name.</param>
    ''' <param name="Property">Property name from within the variable type</param>
    ''' <returns>Value of a property</returns>
    Public Function GetConfigPropertyValueInVariableField(Variable As String, [Property] As String) As Object
        'Get field for specified variable
        Dim TargetField As FieldInfo = GetField(Variable)

        'Get the variable if found
        If TargetField IsNot Nothing Then
            'Now, get the property
            Wdbg(DebugLevel.I, "Got field {0}.", TargetField.Name)
            Dim TargetProperty As PropertyInfo = TargetField.FieldType.GetProperty([Property])

            'Get the property value if found
            If TargetProperty IsNot Nothing Then
                Return TargetProperty.GetValue(GetConfigValueField(Variable))
            Else
                'Property not found on any of the "flag" modules.
                Wdbg(DebugLevel.I, "Property {0} not found.", [Property])
                W(DoTranslation("Property {0} is not found on any of the modules."), True, ColTypes.Error, [Property])
                Return Nothing
            End If
        Else
            'Variable not found on any of the "flag" modules.
            Wdbg(DebugLevel.I, "Field {0} not found.", Variable)
            W(DoTranslation("Variable {0} is not found on any of the modules."), True, ColTypes.Error, Variable)
            Return Nothing
        End If
    End Function

End Module
