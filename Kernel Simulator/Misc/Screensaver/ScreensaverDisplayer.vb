
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

Imports System.Threading

Namespace Misc.Screensaver
    Public Module ScreensaverDisplayer

        Public ReadOnly ScreensaverDisplayerThread As New KernelThread("Screensaver display thread", False, AddressOf DisplayScreensaver)
        Friend OutOfRandom As Boolean

        ''' <summary>
        ''' Displays the screensaver from the screensaver base
        ''' </summary>
        ''' <param name="Screensaver">Screensaver base containing information about the screensaver</param>
        Public Sub DisplayScreensaver(Screensaver As BaseScreensaver)
            Try
                'Preparations
                OutOfRandom = False
                Screensaver.ScreensaverPreparation()

                'Execute the actual screensaver logic
                Do While Not OutOfRandom
                    Screensaver.ScreensaverLogic()
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
                OutOfRandom = True
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
