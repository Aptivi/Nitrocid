
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

Imports MimeKit.Cryptography
Imports Org.BouncyCastle.Bcpg.OpenPgp

Namespace Network.Mail.PGP
    Public Class PGPContext
        Inherits GnuPGContext

        ''' <summary>
        ''' Gets password for secret key.
        ''' </summary>
        ''' <param name="key">Target key</param>
        ''' <returns>Entered Password</returns>
        Protected Overrides Function GetPasswordForKey(key As PgpSecretKey) As String
            If Not String.IsNullOrWhiteSpace(Mail_GPGPromptStyle) Then
                Write(ProbePlaces(Mail_GPGPromptStyle), False, ColTypes.Input, key.KeyId)
            Else
                Write(DoTranslation("Write password for key ID {0}") + ": ", False, ColTypes.Input, key.KeyId)
            End If
            Dim Password As String = ReadLineNoInput()
            Return Password
        End Function
    End Class
End Namespace
