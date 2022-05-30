
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
    Module PersonLookupDisplay

        Public PersonLookup As New KernelThread("PersonLookup screensaver thread", True, AddressOf PersonLookup_DoWork)

        ''' <summary>
        ''' Handles the code of PersonLookup
        ''' </summary>
        Sub PersonLookup_DoWork()
            Try
                'Variables
                Dim random As New Random()
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

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
            Catch taex As ThreadAbortException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
