
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
Imports KS.Files.Folders

Namespace Misc.Execution
    Public Module ProcessExecutor

        Friend ProcessData As String = ""
        Friend NewDataSpotted As Boolean
        Friend NewDataDetector As New KernelThread("New data detection for process", False, AddressOf DetectNewData)

        ''' <summary>
        ''' Thread parameters for ExecuteProcess()
        ''' </summary>
        Friend Class ExecuteProcessThreadParameters
            ''' <summary>
            ''' Full path to file
            ''' </summary>
            Friend File As String
            ''' <summary>
            ''' Arguments, if any
            ''' </summary>
            Friend Args As String

            Friend Sub New(File As String, Args As String)
                Me.File = File
                Me.Args = Args
            End Sub
        End Class

        ''' <summary>
        ''' Executes a file with specified arguments
        ''' </summary>
        Friend Sub ExecuteProcess(ThreadParams As ExecuteProcessThreadParameters)
            ExecuteProcess(ThreadParams.File, ThreadParams.Args)
        End Sub

        ''' <summary>
        ''' Executes a file with specified arguments
        ''' </summary>
        ''' <param name="File">Full path to file</param>
        ''' <param name="Args">Arguments, if any</param>
        ''' <returns>Application exit code. -1 if internal error occurred.</returns>
        Public Function ExecuteProcess(File As String, Args As String) As Integer
            Return ExecuteProcess(File, Args, CurrentDir)
        End Function

        ''' <summary>
        ''' Executes a file with specified arguments
        ''' </summary>
        ''' <param name="File">Full path to file</param>
        ''' <param name="Args">Arguments, if any</param>
        ''' <returns>Application exit code. -1 if internal error occurred.</returns>
        Public Function ExecuteProcess(File As String, Args As String, WorkingDirectory As String) As Integer
            Try
                Dim CommandProcess As New Process
                Dim CommandProcessStart As New ProcessStartInfo With {.RedirectStandardInput = True,
                                                                  .RedirectStandardOutput = True,
                                                                  .RedirectStandardError = True,
                                                                  .FileName = File,
                                                                  .Arguments = Args,
                                                                  .WorkingDirectory = WorkingDirectory,
                                                                  .CreateNoWindow = True,
                                                                  .WindowStyle = ProcessWindowStyle.Hidden,
                                                                  .UseShellExecute = False}
                CommandProcess.StartInfo = CommandProcessStart
                AddHandler CommandProcess.OutputDataReceived, AddressOf ExecutableOutput
                AddHandler CommandProcess.ErrorDataReceived, AddressOf ExecutableOutput

                'Start the process
                Wdbg(DebugLevel.I, "Starting...")
                CommandProcess.Start()
                CommandProcess.BeginOutputReadLine()
                CommandProcess.BeginErrorReadLine()
                NewDataDetector.Start()

                'Wait for process exit
                While Not CommandProcess.HasExited Or Not CancelRequested
                    If CommandProcess.HasExited Then
                        Exit While
                    ElseIf CancelRequested Then
                        CommandProcess.Kill()
                        Exit While
                    End If
                End While

                'Wait until no more data is entered
                While NewDataSpotted
                End While

                'Stop the new data detector
                NewDataDetector.Stop()
                ProcessData = ""

                'Assume that we've spotted new data. This is to avoid race conditions happening sometimes if the processes are exited while output is still going.
                'This is a workaround for some commands like netstat.exe that don't work with normal workarounds shown below.
                NewDataSpotted = True
                Return CommandProcess.ExitCode
            Catch taex As ThreadInterruptedException
                CancelRequested = False
                Return -1
                Exit Function
            Catch ex As Exception
                KernelEventManager.RaiseProcessError(File + Args, ex)
                WStkTrc(ex)
                Write(DoTranslation("Error trying to execute command") + " {2}." + NewLine + DoTranslation("Error {0}: {1}"), True, ColTypes.Error, ex.GetType.FullName, ex.Message, File)
            End Try
            Return -1
        End Function

        ''' <summary>
        ''' Handles executable output
        ''' </summary>
        ''' <param name="sendingProcess">Sender</param>
        ''' <param name="outLine">Output</param>
        Private Sub ExecutableOutput(sendingProcess As Object, outLine As DataReceivedEventArgs)
            NewDataSpotted = True
            Wdbg(DebugLevel.I, outLine.Data)
            Write(outLine.Data, True, ColTypes.Neutral)
            ProcessData += outLine.Data
        End Sub

        ''' <summary>
        ''' Detects new data
        ''' </summary>
        Private Sub DetectNewData()
            While Thread.CurrentThread.IsAlive
                If NewDataSpotted Then
                    Dim OldLength As Long = ProcessData.LongCount
                    Thread.Sleep(50)
                    If OldLength = ProcessData.LongCount Then NewDataSpotted = False
                End If
            End While
        End Sub

    End Module
End Namespace