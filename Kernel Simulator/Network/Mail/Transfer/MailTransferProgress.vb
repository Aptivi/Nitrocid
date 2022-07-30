
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

Imports MailKit

Namespace Network.Mail.Transfer
    Public Class MailTransferProgress
        Implements ITransferProgress

        Public Sub Report(bytesTransferred As Long, totalSize As Long) Implements ITransferProgress.Report
            If Mail_ShowProgress Then
                If Not String.IsNullOrWhiteSpace(Mail_ProgressStyle) Then
                    WriteWhere(ProbePlaces(Mail_ProgressStyle) + GetEsc() + "[0K", 0, Console.CursorTop, True, ColTypes.Progress, bytesTransferred.FileSizeToString, totalSize.FileSizeToString)
                Else
                    WriteWhere("{0}/{1} " + DoTranslation("of mail transferred...") + GetEsc() + "[0K", 0, Console.CursorTop, True, ColTypes.Progress, bytesTransferred.FileSizeToString, totalSize.FileSizeToString)
                End If
            End If
        End Sub

        Public Sub Report(bytesTransferred As Long) Implements ITransferProgress.Report
            If Mail_ShowProgress Then
                If Not String.IsNullOrWhiteSpace(Mail_ProgressStyleSingle) Then
                    WriteWhere(ProbePlaces(Mail_ProgressStyleSingle) + GetEsc() + "[0K", 0, Console.CursorTop, True, ColTypes.Progress, bytesTransferred.FileSizeToString)
                Else
                    WriteWhere("{0} " + DoTranslation("of mail transferred...") + GetEsc() + "[0K", 0, Console.CursorTop, True, ColTypes.Progress, bytesTransferred.FileSizeToString)
                End If
            End If
        End Sub

    End Class
End Namespace
