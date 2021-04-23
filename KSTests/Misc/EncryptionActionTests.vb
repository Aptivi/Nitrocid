
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

Imports System.IO
Imports KS

<TestClass()> Public Class EncryptionActionTests

    ''' <summary>
    ''' Tests string encryption
    ''' </summary>
    <TestMethod()> <TestCategory("Action")> Public Sub TestGetEncryptedString()
        Dim TextHash As String = "Test hashing."
        Dim TextHashMD5 As String = GetEncryptedString(TextHash, Algorithms.MD5)
        Dim TextHashSHA1 As String = GetEncryptedString(TextHash, Algorithms.SHA1)
        Dim TextHashSHA256 As String = GetEncryptedString(TextHash, Algorithms.SHA256)
        Dim TextHashSHA512 As String = GetEncryptedString(TextHash, Algorithms.SHA512)
        Assert.IsTrue(TextHashMD5 = "C4C1867580D6D25B11210F84F935359A" And
                      TextHashSHA1 = "CFF9FDA895B0B638957E17CF952457D81ADD622F" And
                      TextHashSHA256 = "525514740C93C5442DBCB8FB92FB1B17B6F8B94B3C98E6F07CA8AEB093C2E79F" And
                      TextHashSHA512 = "0015CAF195A7248127F7E50C8D839935681A2234344387B5E9DF761E6D4F152CC4458ADCD45A19F59413EA6BC5E7C907A01A0B47B548CE0DAD04787CE416157D",
                      "Encrypted string hash isn't get properly. Got:" + vbNewLine + TextHashMD5 + vbNewLine +
                                                                                     TextHashSHA1 + vbNewLine +
                                                                                     TextHashSHA256 + vbNewLine +
                                                                                     TextHashSHA512)
    End Sub

    ''' <summary>
    ''' Tests file encryption
    ''' </summary>
    <TestMethod()> <TestCategory("Action")> Public Sub TestGetEncryptedFileUsingStream()
        InitPaths()
        Dim FileStreamHash As FileStream = File.Create(paths("Home") + "/TestSum.txt")
        FileStreamHash.Write(Text.Encoding.Default.GetBytes("Test hashing."), 0, 13)
        FileStreamHash.Flush()
        Dim FileHashMD5 As String = GetEncryptedFile(FileStreamHash, Algorithms.MD5)
        Dim FileHashSHA1 As String = GetEncryptedFile(FileStreamHash, Algorithms.SHA1)
        Dim FileHashSHA256 As String = GetEncryptedFile(FileStreamHash, Algorithms.SHA256)
        Dim FileHashSHA512 As String = GetEncryptedFile(FileStreamHash, Algorithms.SHA512)
        FileStreamHash.Close()
        File.Delete(paths("Home") + "/TestSum.txt")
        Assert.IsTrue(FileHashMD5 = "D41D8CD98F00B204E9800998ECF8427E" And
                      FileHashSHA1 = "DA39A3EE5E6B4B0D3255BFEF95601890AFD80709" And
                      FileHashSHA256 = "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855" And
                      FileHashSHA512 = "CF83E1357EEFB8BDF1542850D66D8007D620E4050B5715DC83F4A921D36CE9CE47D0D13C5D85F2B0FF8318D2877EEC2F63B931BD47417A81A538327AF927DA3E",
                      "Encrypted file hash isn't get properly. Got:" + vbNewLine + FileHashMD5 + vbNewLine +
                                                                                   FileHashSHA1 + vbNewLine +
                                                                                   FileHashSHA256 + vbNewLine +
                                                                                   FileHashSHA512)
    End Sub

    ''' <summary>
    ''' Tests file encryption
    ''' </summary>
    <TestMethod()> <TestCategory("Action")> Public Sub TestGetEncryptedFileUsingPath()
        InitPaths()
        Dim FileStreamHash As FileStream = File.Create(paths("Home") + "/TestSum.txt")
        FileStreamHash.Write(Text.Encoding.Default.GetBytes("Test hashing with path."), 0, 23)
        FileStreamHash.Flush()
        FileStreamHash.Close()
        Dim FileHashMD5 As String = GetEncryptedFile(paths("Home") + "/TestSum.txt", Algorithms.MD5)
        Dim FileHashSHA1 As String = GetEncryptedFile(paths("Home") + "/TestSum.txt", Algorithms.SHA1)
        Dim FileHashSHA256 As String = GetEncryptedFile(paths("Home") + "/TestSum.txt", Algorithms.SHA256)
        Dim FileHashSHA512 As String = GetEncryptedFile(paths("Home") + "/TestSum.txt", Algorithms.SHA512)
        File.Delete(paths("Home") + "/TestSum.txt")
        Assert.IsTrue(FileHashMD5 = "CD5578C85A4CF32E48D157746A90C7F6" And
                      FileHashSHA1 = "36EBF31AF7234D6C99CA65DC4EDA524161600657" And
                      FileHashSHA256 = "7E6857729A34755DE8C2C9E535A8765BDE241F593BE3588B8FA6D29D949EFADA" And
                      FileHashSHA512 = "6DF635C184D4B131B0243D4F2BD66925A61B82A5093F573920F42D7B8474D6332FD2886920F3CA36D9206C73DD59C8F1EEA18501E6FEF15FDDA664B1ABB0E361",
                      "Encrypted file hash isn't get properly. Got:" + vbNewLine + FileHashMD5 + vbNewLine +
                                                                                   FileHashSHA1 + vbNewLine +
                                                                                   FileHashSHA256 + vbNewLine +
                                                                                   FileHashSHA512)
    End Sub

    ''' <summary>
    ''' Tests hash verification
    ''' </summary>
    <TestMethod()> <TestCategory("Action")> Public Sub TestVerifyHashFromHash()
        InitPaths()
        Dim FileStreamHash As FileStream = File.Create(paths("Home") + "/TestSum.txt")
        FileStreamHash.Write(Text.Encoding.Default.GetBytes("Test hashing with path."), 0, 23)
        FileStreamHash.Flush()
        FileStreamHash.Close()
        Dim FileHashMD5 As String = GetEncryptedFile(paths("Home") + "/TestSum.txt", Algorithms.MD5)
        Dim FileHashSHA1 As String = GetEncryptedFile(paths("Home") + "/TestSum.txt", Algorithms.SHA1)
        Dim FileHashSHA256 As String = GetEncryptedFile(paths("Home") + "/TestSum.txt", Algorithms.SHA256)
        Dim FileHashSHA512 As String = GetEncryptedFile(paths("Home") + "/TestSum.txt", Algorithms.SHA512)
        Try
            Dim ResultMD5 As Boolean = VerifyHashFromHash(paths("Home") + "/TestSum.txt", Algorithms.MD5, "CD5578C85A4CF32E48D157746A90C7F6", FileHashMD5)
            Dim ResultSHA1 As Boolean = VerifyHashFromHash(paths("Home") + "/TestSum.txt", Algorithms.SHA1, "36EBF31AF7234D6C99CA65DC4EDA524161600657", FileHashSHA1)
            Dim ResultSHA256 As Boolean = VerifyHashFromHash(paths("Home") + "/TestSum.txt", Algorithms.SHA256, "7E6857729A34755DE8C2C9E535A8765BDE241F593BE3588B8FA6D29D949EFADA", FileHashSHA256)
            Dim ResultSHA512 As Boolean = VerifyHashFromHash(paths("Home") + "/TestSum.txt", Algorithms.SHA512, "6DF635C184D4B131B0243D4F2BD66925A61B82A5093F573920F42D7B8474D6332FD2886920F3CA36D9206C73DD59C8F1EEA18501E6FEF15FDDA664B1ABB0E361", FileHashSHA512)
            File.Delete(paths("Home") + "/TestSum.txt")
            Assert.IsTrue(ResultMD5, "Hash isn't verified propertly. Got {0}", ResultMD5)
            Assert.IsTrue(ResultSHA1, "Hash isn't verified propertly. Got {0}", ResultSHA1)
            Assert.IsTrue(ResultSHA256, "Hash isn't verified propertly. Got {0}", ResultSHA256)
            Assert.IsTrue(ResultSHA512, "Hash isn't verified propertly. Got {0}", ResultSHA512)
        Catch ex As Exception
            Assert.Fail(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Tests hash verification from hashes file
    ''' </summary>
    <TestMethod()> <TestCategory("Action")> Public Sub TestVerifyHashFromFileStdFormat()
        Dim FileHashMD5 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.MD5)
        Dim FileHashSHA1 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA1)
        Dim FileHashSHA256 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA256)
        Dim FileHashSHA512 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA512)
        Try
            Dim ResultMD5 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.MD5, Environment.CurrentDirectory + "/TestVerifyMD5.txt", FileHashMD5)
            Dim ResultSHA1 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA1, Environment.CurrentDirectory + "/TestVerifySHA1.txt", FileHashSHA1)
            Dim ResultSHA256 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA256, Environment.CurrentDirectory + "/TestVerifySHA256.txt", FileHashSHA256)
            Dim ResultSHA512 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA512, Environment.CurrentDirectory + "/TestVerifySHA512.txt", FileHashSHA512)
            Assert.IsTrue(ResultMD5, "Hash isn't verified propertly. Got {0}", ResultMD5)
            Assert.IsTrue(ResultSHA1, "Hash isn't verified propertly. Got {0}", ResultSHA1)
            Assert.IsTrue(ResultSHA256, "Hash isn't verified propertly. Got {0}", ResultSHA256)
            Assert.IsTrue(ResultSHA512, "Hash isn't verified propertly. Got {0}", ResultSHA512)
        Catch ex As Exception
            Assert.Fail(ex.Message)
        End Try
    End Sub

    <TestMethod> <TestCategory("Action")> Public Sub TestGetEmptyHash()
        Assert.IsNotNull(GetEmptyHash(Algorithms.MD5))
        Assert.IsNotNull(GetEmptyHash(Algorithms.SHA1))
        Assert.IsNotNull(GetEmptyHash(Algorithms.SHA256))
        Assert.IsNotNull(GetEmptyHash(Algorithms.SHA512))
    End Sub

End Class