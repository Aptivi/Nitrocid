
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

Imports System.Threading
Imports KS.TimeDate

Namespace Misc.Screensaver.Displays
    Public Module PersonLookupDisplay

        Friend PersonLookup As New KernelThread("PersonLookup screensaver thread", True, AddressOf PersonLookup_DoWork)
        Private _personLookupDelay As Integer = 75
        Private _personLookupLookedUpDelay As Integer = 10000
        Private _personLookupMinimumNames As Integer = 10
        Private _personLookupMaximumNames As Integer = 100
        Private _personLookupMinimumAgeYears As Integer = 18
        Private _personLookupMaximumAgeYears As Integer = 100

        ''' <summary>
        ''' [PersonLookup] How many milliseconds to wait before getting the new name?
        ''' </summary>
        Public Property PersonLookupDelay As Integer
            Get
                Return _personLookupDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 75
                _personLookupDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [PersonLookup] How many milliseconds to show the looked up name?
        ''' </summary>
        Public Property PersonLookupLookedUpDelay As Integer
            Get
                Return _personLookupLookedUpDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10000
                _personLookupLookedUpDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [PersonLookup] Minimum names count
        ''' </summary>
        Public Property PersonLookupMinimumNames As Integer
            Get
                Return _personLookupMinimumNames
            End Get
            Set(value As Integer)
                If value <= 10 Then value = 10
                If value > 1000 Then value = 1000
                _personLookupMinimumNames = value
            End Set
        End Property
        ''' <summary>
        ''' [PersonLookup] Maximum names count
        ''' </summary>
        Public Property PersonLookupMaximumNames As Integer
            Get
                Return _personLookupMaximumNames
            End Get
            Set(value As Integer)
                If value <= _personLookupMinimumNames Then value = _personLookupMinimumNames
                If value > 1000 Then value = 1000
                _personLookupMaximumNames = value
            End Set
        End Property
        ''' <summary>
        ''' [PersonLookup] Minimum age years
        ''' </summary>
        Public Property PersonLookupMinimumAgeYears As Integer
            Get
                Return _personLookupMinimumAgeYears
            End Get
            Set(value As Integer)
                If value <= 18 Then value = 18
                If value > 100 Then value = 100
                _personLookupMinimumAgeYears = value
            End Set
        End Property
        ''' <summary>
        ''' [PersonLookup] Maximum age years
        ''' </summary>
        Public Property PersonLookupMaximumAgeYears As Integer
            Get
                Return _personLookupMaximumAgeYears
            End Get
            Set(value As Integer)
                If value <= _personLookupMinimumAgeYears Then value = _personLookupMinimumAgeYears
                If value > 100 Then value = 100
                _personLookupMaximumAgeYears = value
            End Set
        End Property

        ''' <summary>
        ''' Handles the code of PersonLookup
        ''' </summary>
        Sub PersonLookup_DoWork()
            Try
                'Variables
                Dim random As New Random()

                'Preparations
                PopulateNames()

                'Screensaver logic
                Do While True
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.ForegroundColor = ConsoleColor.Green
                    Console.Clear()
                    Console.CursorVisible = False

                    'Generate names
                    Dim NumberOfPeople As Integer = random.Next(PersonLookupMinimumNames, PersonLookupMaximumNames)
                    Dim NamesToLookup As List(Of String) = GenerateNames(NumberOfPeople)

                    'Loop through names
                    For Each GeneratedName As String In NamesToLookup
                        Dim Age As Integer = random.Next(PersonLookupMinimumAgeYears, PersonLookupMaximumAgeYears)
                        Dim AgeMonth As Integer = random.Next(-12, 12)
                        Dim AgeDay As Integer = random.Next(-31, 31)
                        Dim Birthdate As Date = Date.Now.AddYears(-Age).AddMonths(AgeMonth).AddDays(AgeDay)
                        Dim FinalAge As Integer = New DateTime((Date.Now - Birthdate).Ticks).Year
                        Dim FirstName As String = GeneratedName.Substring(0, GeneratedName.IndexOf(" "))
                        Dim LastName As String = GeneratedName.Substring(GeneratedName.IndexOf(" ") + 1)

                        'Print all information
                        Console.Clear()
                        WriteWherePlain("  - Name:                {0}", 0, 1, False, GeneratedName)
                        WriteWherePlain("  - First Name:          {0}", 0, 2, False, FirstName)
                        WriteWherePlain("  - Last Name / Surname: {0}", 0, 3, False, LastName)
                        WriteWherePlain("  - Age:                 {0} years old", 0, 4, False, FinalAge)
                        WriteWherePlain("  - Birth date:          {0}", 0, 5, False, Render(Birthdate))

                        'Lookup delay
                        SleepNoBlock(PersonLookupDelay, PersonLookup)
                    Next

                    'Wait until we run the lookup again
                    SleepNoBlock(PersonLookupLookedUpDelay, PersonLookup)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
