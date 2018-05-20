
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Module KernelTools

    ''' <summary>
    ''' Indicates that there's something wrong with the kernel.
    ''' </summary>
    ''' <param name="ErrorType">Specifies whether the error is serious, fatal, unrecoverable, or double panic. C/S/D/F/U</param>
    ''' <param name="Reboot">Optional. Specifies whether to reboot on panic or to show the message to press any key to shut down</param>
    ''' <param name="RebootTime">Optional. Specifies seconds before reboot. 0 is instant. Negative numbers not allowed.</param>
    ''' <param name="Description">Optional. Explanation of what happened when it errored.</param>
    ''' <remarks></remarks>
    Sub KernelError(ByVal ErrorType As Char, Optional ByVal Reboot As Boolean = True, Optional ByVal RebootTime As Long = 30, Optional ByVal Description As String = "General kernel error.")
        Try
            'Check error types and its capabilities
            If (ErrorType = "S" Or ErrorType = "F" Or ErrorType = "U" Or ErrorType = "D" Or ErrorType = "C") Then
                If (ErrorType = "U" And RebootTime > 5 Or ErrorType = "D" And RebootTime > 5) Then
                    'If the error type is unrecoverable, or double, and the reboot time exceeds 5 seconds, then
                    'generate a second kernel error stating that there is something wrong with the reboot time.
                    KernelError(CChar("D"), True, 5, "DOUBLE PANIC: Reboot Time exceeds maximum allowed " + CStr(ErrorType) + " error reboot time. You found a kernel bug.")
                    StopPanicAndGoToDoublePanic = True
                ElseIf (ErrorType = "U" And Reboot = False Or ErrorType = "D" And Reboot = False) Then
                    'If the error type is unrecoverable, or double, and the rebooting is false where it should
                    'not be false, then it can deal with this issue by enabling reboot.
                    Wln("[{0}] panic: Reboot enabled due to error level being {0}.", "uncontError", ErrorType)
                    Reboot = True
                End If
                If (RebootTime > 3600) Then
                    'If the reboot time exceeds 1 hour, then it will set the time to 1 minute.
                    Wln("[{0}] panic: Time to reboot: {1} seconds, exceeds 1 hour. It is set to 1 minute.", "uncontError", ErrorType, CStr(RebootTime))
                    RebootTime = 60
                End If
            Else
                'If the error type is other than D/F/C/U/S, then it will generate a second error.
                KernelError(CChar("D"), True, 5, "DOUBLE PANIC: Error Type " + CStr(ErrorType) + " invalid.")
                StopPanicAndGoToDoublePanic = True
            End If

            'Check error capabilities
            If (Description.Contains("DOUBLE PANIC: ") And ErrorType = "D") Then
                'If the description has a double panic tag and the error type is Double
                Wln("[{0}] dpanic: {1} -- Rebooting in {2} seconds...", "uncontError", ErrorType, CStr(Description), CStr(RebootTime))
                Sleep(CInt(RebootTime * 1000))
                System.Console.Clear()
                ResetEverything()
                Main()
            ElseIf (StopPanicAndGoToDoublePanic = True) Then
                'Switch to Double Panic
                Exit Sub
            ElseIf (ErrorType = "C" And Reboot = True) Then
                'Check if error is Continuable and reboot is enabled
                Reboot = False
                Wln("[{0}] panic: Reboot disabled due to error level being {0}." + vbNewLine + "[{0}] panic: {1} -- Press any key to continue using the kernel.", "contError", ErrorType, CStr(Description))
                Dim answercontpanic = System.Console.ReadKey.KeyChar
            ElseIf (ErrorType = "C" And Reboot = False) Then
                'Check if error is Continuable and reboot is disabled
                Wln("[{0}] panic: {1} -- Press any key to continue using the kernel.", "contError", ErrorType, CStr(Description))
                Dim answercontpanic = System.Console.ReadKey.KeyChar
            ElseIf ((Reboot = False And ErrorType <> "D") Or (Reboot = False And ErrorType <> "C")) Then
                'If rebooting is disabled and the error type does not equal Double or Continuable
                Wln("[{0}] panic: {1} -- Press any key to shutdown.", "uncontError", ErrorType, CStr(Description))
                Dim answerpanic = System.Console.ReadKey.KeyChar
                Environment.Exit(0)
            Else
                'Everything else.
                Wln("[{0}] panic: {1} -- Rebooting in {2} seconds...", "uncontError", ErrorType, CStr(Description), CStr(RebootTime))
                Sleep(CInt(RebootTime * 1000))
                System.Console.Clear()
                ResetEverything()
                Main()
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
                KernelError(CChar("D"), True, 5, "DOUBLE PANIC: Kernel bug: " + Err.Description)
            Else
                KernelError(CChar("D"), True, 5, "DOUBLE PANIC: Kernel bug: " + Err.Description)
            End If
        End Try

    End Sub

    Sub ResetEverything()

        'Reset every variable that is resettable
        If (argsInjected = False) Then
            answerargs = Nothing
        End If
        Erase BootArgs
        answerbeep = Nothing
        answerbeepms = Nothing
        answerecho = Nothing
        argsFlag = False
        Computers = Nothing
        ProbeFlag = True
        GPUProbeFlag = False
        Quiet = False
        StopPanicAndGoToDoublePanic = False
        strcommand = Nothing
        slotsUsedName = Nothing
        slotsUsedNum = 0
        totalSlots = 0
        Wdbg("General variables reset", True)

        'Reset users
        UserManagement.resetUsers()
        Wdbg("User variables reset", True)

        'Release RAM used
        DisposeExit.DisposeAll()
        Wdbg("Garbage collector finished", True)

        'Disable Debugger
        If (DebugMode = True) Then
            DebugMode = False
        End If

    End Sub

End Module
