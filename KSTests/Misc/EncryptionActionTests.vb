
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

Imports System.IO
Imports KS.Misc.Encryption

<TestFixture> Public Class EncryptionActionTests

    ''' <summary>
    ''' Tests string encryption
    ''' </summary>
    <Test, Description("Action")> Public Sub TestGetEncryptedString()
        Dim TextHash As String = "Test hashing."
        Dim TextHashCRC32 As String = GetEncryptedString(TextHash, Algorithms.CRC32)
        Dim TextHashMD5 As String = GetEncryptedString(TextHash, Algorithms.MD5)
        Dim TextHashSHA1 As String = GetEncryptedString(TextHash, Algorithms.SHA1)
        Dim TextHashSHA256 As String = GetEncryptedString(TextHash, Algorithms.SHA256)
        Dim TextHashSHA384 As String = GetEncryptedString(TextHash, Algorithms.SHA384)
        Dim TextHashSHA512 As String = GetEncryptedString(TextHash, Algorithms.SHA512)
        TextHashCRC32.ShouldBe("9413827E")
        TextHashMD5.ShouldBe("C4C1867580D6D25B11210F84F935359A")
        TextHashSHA1.ShouldBe("CFF9FDA895B0B638957E17CF952457D81ADD622F")
        TextHashSHA256.ShouldBe("525514740C93C5442DBCB8FB92FB1B17B6F8B94B3C98E6F07CA8AEB093C2E79F")
        TextHashSHA384.ShouldBe("B26ADFF6A6BDD59612F4B560E7D2A0240D7A611AF46BD4D2891181F46341E4886A8D74724877955AFC908F6B17A5B613")
        TextHashSHA512.ShouldBe("0015CAF195A7248127F7E50C8D839935681A2234344387B5E9DF761E6D4F152CC4458ADCD45A19F59413EA6BC5E7C907A01A0B47B548CE0DAD04787CE416157D")
    End Sub

    ''' <summary>
    ''' Tests file encryption
    ''' </summary>
    <Test, Description("Action")> Public Sub TestGetEncryptedFileUsingStream()
        Dim FileStreamHash As FileStream = File.Create(HomePath + "/TestSum.txt")
        FileStreamHash.Write(Text.Encoding.Default.GetBytes("Test hashing."), 0, 13)
        FileStreamHash.Flush()
        FileStreamHash.Position = 0
        Dim FileHashCRC32 As String = GetEncryptedFile(FileStreamHash, Algorithms.CRC32)
        Dim FileHashMD5 As String = GetEncryptedFile(FileStreamHash, Algorithms.MD5)
        Dim FileHashSHA1 As String = GetEncryptedFile(FileStreamHash, Algorithms.SHA1)
        Dim FileHashSHA256 As String = GetEncryptedFile(FileStreamHash, Algorithms.SHA256)
        Dim FileHashSHA384 As String = GetEncryptedFile(FileStreamHash, Algorithms.SHA384)
        Dim FileHashSHA512 As String = GetEncryptedFile(FileStreamHash, Algorithms.SHA512)
        FileStreamHash.Close()
        File.Delete(HomePath + "/TestSum.txt")
        FileHashCRC32.ShouldBe("9413827E")
        FileHashMD5.ShouldBe("D41D8CD98F00B204E9800998ECF8427E")
        FileHashSHA1.ShouldBe("DA39A3EE5E6B4B0D3255BFEF95601890AFD80709")
        FileHashSHA256.ShouldBe("E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855")
        FileHashSHA384.ShouldBe("38B060A751AC96384CD9327EB1B1E36A21FDB71114BE07434C0CC7BF63F6E1DA274EDEBFE76F65FBD51AD2F14898B95B")
        FileHashSHA512.ShouldBe("CF83E1357EEFB8BDF1542850D66D8007D620E4050B5715DC83F4A921D36CE9CE47D0D13C5D85F2B0FF8318D2877EEC2F63B931BD47417A81A538327AF927DA3E")
    End Sub

    ''' <summary>
    ''' Tests file encryption
    ''' </summary>
    <Test, Description("Action")> Public Sub TestGetEncryptedFileUsingPath()
        Dim FileStreamHash As FileStream = File.Create(HomePath + "/TestSum.txt")
        FileStreamHash.Write(Text.Encoding.Default.GetBytes("Test hashing with path."), 0, 23)
        FileStreamHash.Flush()
        FileStreamHash.Close()
        Dim FileHashCRC32 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.CRC32)
        Dim FileHashMD5 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.MD5)
        Dim FileHashSHA1 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.SHA1)
        Dim FileHashSHA256 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.SHA256)
        Dim FileHashSHA384 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.SHA384)
        Dim FileHashSHA512 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.SHA512)
        File.Delete(HomePath + "/TestSum.txt")
        FileHashCRC32.ShouldBe("D394D7F0")
        FileHashMD5.ShouldBe("CD5578C85A4CF32E48D157746A90C7F6")
        FileHashSHA1.ShouldBe("36EBF31AF7234D6C99CA65DC4EDA524161600657")
        FileHashSHA256.ShouldBe("7E6857729A34755DE8C2C9E535A8765BDE241F593BE3588B8FA6D29D949EFADA")
        FileHashSHA384.ShouldBe("92CBCB3F982C7EC24EED668175D4FE7C73D9BBCBECA659EDDE6D6E56B798D64C808F86C7E13FA6BE03464AE2D145BB60")
        FileHashSHA512.ShouldBe("6DF635C184D4B131B0243D4F2BD66925A61B82A5093F573920F42D7B8474D6332FD2886920F3CA36D9206C73DD59C8F1EEA18501E6FEF15FDDA664B1ABB0E361")
    End Sub

    ''' <summary>
    ''' Tests hash verification
    ''' </summary>
    <Test, Description("Action")> Public Sub TestVerifyHashFromHash()
        Dim FileStreamHash As FileStream = File.Create(HomePath + "/TestSum.txt")
        FileStreamHash.Write(Text.Encoding.Default.GetBytes("Test hashing with path."), 0, 23)
        FileStreamHash.Flush()
        FileStreamHash.Close()
        Dim FileHashCRC32 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.CRC32)
        Dim FileHashMD5 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.MD5)
        Dim FileHashSHA1 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.SHA1)
        Dim FileHashSHA256 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.SHA256)
        Dim FileHashSHA384 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.SHA384)
        Dim FileHashSHA512 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.SHA512)
        Dim ResultCRC32 As Boolean = VerifyHashFromHash(HomePath + "/TestSum.txt", Algorithms.CRC32, "D394D7F0", FileHashCRC32)
        Dim ResultMD5 As Boolean = VerifyHashFromHash(HomePath + "/TestSum.txt", Algorithms.MD5, "CD5578C85A4CF32E48D157746A90C7F6", FileHashMD5)
        Dim ResultSHA1 As Boolean = VerifyHashFromHash(HomePath + "/TestSum.txt", Algorithms.SHA1, "36EBF31AF7234D6C99CA65DC4EDA524161600657", FileHashSHA1)
        Dim ResultSHA256 As Boolean = VerifyHashFromHash(HomePath + "/TestSum.txt", Algorithms.SHA256, "7E6857729A34755DE8C2C9E535A8765BDE241F593BE3588B8FA6D29D949EFADA", FileHashSHA256)
        Dim ResultSHA384 As Boolean = VerifyHashFromHash(HomePath + "/TestSum.txt", Algorithms.SHA384, "92CBCB3F982C7EC24EED668175D4FE7C73D9BBCBECA659EDDE6D6E56B798D64C808F86C7E13FA6BE03464AE2D145BB60", FileHashSHA384)
        Dim ResultSHA512 As Boolean = VerifyHashFromHash(HomePath + "/TestSum.txt", Algorithms.SHA512, "6DF635C184D4B131B0243D4F2BD66925A61B82A5093F573920F42D7B8474D6332FD2886920F3CA36D9206C73DD59C8F1EEA18501E6FEF15FDDA664B1ABB0E361", FileHashSHA512)
        File.Delete(HomePath + "/TestSum.txt")
        ResultCRC32.ShouldBeTrue
        ResultMD5.ShouldBeTrue
        ResultSHA1.ShouldBeTrue
        ResultSHA256.ShouldBeTrue
        ResultSHA384.ShouldBeTrue
        ResultSHA512.ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests hash verification from hashes file
    ''' </summary>
    <Test, Description("Action")> Public Sub TestVerifyHashFromFileStdFormat()
        Dim FileHashCRC32 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.CRC32)
        Dim FileHashMD5 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.MD5)
        Dim FileHashSHA1 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA1)
        Dim FileHashSHA256 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA256)
        Dim FileHashSHA384 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA384)
        Dim FileHashSHA512 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA512)
        Dim ResultCRC32 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.CRC32, Environment.CurrentDirectory + "/TestVerifyCRC32.txt", FileHashCRC32)
        Dim ResultMD5 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.MD5, Environment.CurrentDirectory + "/TestVerifyMD5.txt", FileHashMD5)
        Dim ResultSHA1 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA1, Environment.CurrentDirectory + "/TestVerifySHA1.txt", FileHashSHA1)
        Dim ResultSHA256 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA256, Environment.CurrentDirectory + "/TestVerifySHA256.txt", FileHashSHA256)
        Dim ResultSHA384 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA384, Environment.CurrentDirectory + "/TestVerifySHA384.txt", FileHashSHA384)
        Dim ResultSHA512 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA512, Environment.CurrentDirectory + "/TestVerifySHA512.txt", FileHashSHA512)
        ResultCRC32.ShouldBeTrue
        ResultMD5.ShouldBeTrue
        ResultSHA1.ShouldBeTrue
        ResultSHA256.ShouldBeTrue
        ResultSHA384.ShouldBeTrue
        ResultSHA512.ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests hash verification from hashes file
    ''' </summary>
    <Test, Description("Action")> Public Sub TestVerifyHashFromFileKSFormat()
        Dim FileHashCRC32 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.CRC32)
        Dim FileHashMD5 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.MD5)
        Dim FileHashSHA1 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA1)
        Dim FileHashSHA256 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA256)
        Dim FileHashSHA384 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA384)
        Dim FileHashSHA512 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA512)
        Dim ResultCRC32 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.CRC32, Environment.CurrentDirectory + "/TestVerifyCRC32KS.txt", FileHashCRC32)
        Dim ResultMD5 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.MD5, Environment.CurrentDirectory + "/TestVerifyMD5KS.txt", FileHashMD5)
        Dim ResultSHA1 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA1, Environment.CurrentDirectory + "/TestVerifySHA1KS.txt", FileHashSHA1)
        Dim ResultSHA256 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA256, Environment.CurrentDirectory + "/TestVerifySHA256KS.txt", FileHashSHA256)
        Dim ResultSHA384 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA384, Environment.CurrentDirectory + "/TestVerifySHA384KS.txt", FileHashSHA384)
        Dim ResultSHA512 As Boolean = VerifyHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA512, Environment.CurrentDirectory + "/TestVerifySHA512KS.txt", FileHashSHA512)
        ResultCRC32.ShouldBeTrue
        ResultMD5.ShouldBeTrue
        ResultSHA1.ShouldBeTrue
        ResultSHA256.ShouldBeTrue
        ResultSHA384.ShouldBeTrue
        ResultSHA512.ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests hash verification for an uncalculated file
    ''' </summary>
    <Test, Description("Action")> Public Sub TestVerifyUncalculatedHashFromHash()
        Dim FileStreamHash As FileStream = File.Create(HomePath + "/TestSum.txt")
        FileStreamHash.Write(Text.Encoding.Default.GetBytes("Test hashing with path."), 0, 23)
        FileStreamHash.Flush()
        FileStreamHash.Close()
        Dim FileHashCRC32 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.CRC32)
        Dim FileHashMD5 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.MD5)
        Dim FileHashSHA1 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.SHA1)
        Dim FileHashSHA256 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.SHA256)
        Dim FileHashSHA384 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.SHA384)
        Dim FileHashSHA512 As String = GetEncryptedFile(HomePath + "/TestSum.txt", Algorithms.SHA512)
        Dim ResultCRC32 As Boolean = VerifyUncalculatedHashFromHash(HomePath + "/TestSum.txt", Algorithms.CRC32, "D394D7F0")
        Dim ResultMD5 As Boolean = VerifyUncalculatedHashFromHash(HomePath + "/TestSum.txt", Algorithms.MD5, "CD5578C85A4CF32E48D157746A90C7F6")
        Dim ResultSHA1 As Boolean = VerifyUncalculatedHashFromHash(HomePath + "/TestSum.txt", Algorithms.SHA1, "36EBF31AF7234D6C99CA65DC4EDA524161600657")
        Dim ResultSHA256 As Boolean = VerifyUncalculatedHashFromHash(HomePath + "/TestSum.txt", Algorithms.SHA256, "7E6857729A34755DE8C2C9E535A8765BDE241F593BE3588B8FA6D29D949EFADA")
        Dim ResultSHA384 As Boolean = VerifyUncalculatedHashFromHash(HomePath + "/TestSum.txt", Algorithms.SHA384, "92CBCB3F982C7EC24EED668175D4FE7C73D9BBCBECA659EDDE6D6E56B798D64C808F86C7E13FA6BE03464AE2D145BB60")
        Dim ResultSHA512 As Boolean = VerifyUncalculatedHashFromHash(HomePath + "/TestSum.txt", Algorithms.SHA512, "6DF635C184D4B131B0243D4F2BD66925A61B82A5093F573920F42D7B8474D6332FD2886920F3CA36D9206C73DD59C8F1EEA18501E6FEF15FDDA664B1ABB0E361")
        File.Delete(HomePath + "/TestSum.txt")
        ResultCRC32.ShouldBeTrue
        ResultMD5.ShouldBeTrue
        ResultSHA1.ShouldBeTrue
        ResultSHA256.ShouldBeTrue
        ResultSHA384.ShouldBeTrue
        ResultSHA512.ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests hash verification from hashes file for an uncalculated file
    ''' </summary>
    <Test, Description("Action")> Public Sub TestVerifyUncalculatedHashFromFileStdFormat()
        Dim FileHashCRC32 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.CRC32)
        Dim FileHashMD5 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.MD5)
        Dim FileHashSHA1 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA1)
        Dim FileHashSHA256 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA256)
        Dim FileHashSHA384 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA384)
        Dim FileHashSHA512 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA512)
        Dim ResultCRC32 As Boolean = VerifyUncalculatedHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.CRC32, Environment.CurrentDirectory + "/TestVerifyCRC32.txt")
        Dim ResultMD5 As Boolean = VerifyUncalculatedHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.MD5, Environment.CurrentDirectory + "/TestVerifyMD5.txt")
        Dim ResultSHA1 As Boolean = VerifyUncalculatedHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA1, Environment.CurrentDirectory + "/TestVerifySHA1.txt")
        Dim ResultSHA256 As Boolean = VerifyUncalculatedHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA256, Environment.CurrentDirectory + "/TestVerifySHA256.txt")
        Dim ResultSHA384 As Boolean = VerifyUncalculatedHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA384, Environment.CurrentDirectory + "/TestVerifySHA384.txt")
        Dim ResultSHA512 As Boolean = VerifyUncalculatedHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA512, Environment.CurrentDirectory + "/TestVerifySHA512.txt")
        ResultCRC32.ShouldBeTrue
        ResultMD5.ShouldBeTrue
        ResultSHA1.ShouldBeTrue
        ResultSHA256.ShouldBeTrue
        ResultSHA384.ShouldBeTrue
        ResultSHA512.ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests hash verification from hashes file for an uncalculated file
    ''' </summary>
    <Test, Description("Action")> Public Sub TestVerifyUncalculatedHashFromFileKSFormat()
        Dim FileHashCRC32 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.CRC32)
        Dim FileHashMD5 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.MD5)
        Dim FileHashSHA1 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA1)
        Dim FileHashSHA256 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA256)
        Dim FileHashSHA384 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA384)
        Dim FileHashSHA512 As String = GetEncryptedFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA512)
        Dim ResultCRC32 As Boolean = VerifyUncalculatedHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.CRC32, Environment.CurrentDirectory + "/TestVerifyCRC32KS.txt")
        Dim ResultMD5 As Boolean = VerifyUncalculatedHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.MD5, Environment.CurrentDirectory + "/TestVerifyMD5KS.txt")
        Dim ResultSHA1 As Boolean = VerifyUncalculatedHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA1, Environment.CurrentDirectory + "/TestVerifySHA1KS.txt")
        Dim ResultSHA256 As Boolean = VerifyUncalculatedHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA256, Environment.CurrentDirectory + "/TestVerifySHA256KS.txt")
        Dim ResultSHA384 As Boolean = VerifyUncalculatedHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA384, Environment.CurrentDirectory + "/TestVerifySHA384KS.txt")
        Dim ResultSHA512 As Boolean = VerifyUncalculatedHashFromHashesFile(Environment.CurrentDirectory + "/TestText.txt", Algorithms.SHA512, Environment.CurrentDirectory + "/TestVerifySHA512KS.txt")
        ResultCRC32.ShouldBeTrue
        ResultMD5.ShouldBeTrue
        ResultSHA1.ShouldBeTrue
        ResultSHA256.ShouldBeTrue
        ResultSHA384.ShouldBeTrue
        ResultSHA512.ShouldBeTrue
    End Sub

    <Test, Description("Action")> Public Sub TestGetEmptyHash()
        Dim EmptyCRC32 As String = GetEmptyHash(Algorithms.CRC32)
        Dim EmptyMD5 As String = GetEmptyHash(Algorithms.MD5)
        Dim EmptySHA1 As String = GetEmptyHash(Algorithms.SHA1)
        Dim EmptySHA256 As String = GetEmptyHash(Algorithms.SHA256)
        Dim EmptySHA384 As String = GetEmptyHash(Algorithms.SHA384)
        Dim EmptySHA512 As String = GetEmptyHash(Algorithms.SHA512)
        EmptyCRC32.ShouldNotBeNullOrEmpty
        EmptyCRC32.ShouldBe("00000000")
        EmptyMD5.ShouldNotBeNullOrEmpty
        EmptyMD5.ShouldBe("D41D8CD98F00B204E9800998ECF8427E")
        EmptySHA1.ShouldNotBeNullOrEmpty
        EmptySHA1.ShouldBe("DA39A3EE5E6B4B0D3255BFEF95601890AFD80709")
        EmptySHA256.ShouldNotBeNullOrEmpty
        EmptySHA256.ShouldBe("E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855")
        EmptySHA384.ShouldNotBeNullOrEmpty
        EmptySHA384.ShouldBe("38B060A751AC96384CD9327EB1B1E36A21FDB71114BE07434C0CC7BF63F6E1DA274EDEBFE76F65FBD51AD2F14898B95B")
        EmptySHA512.ShouldNotBeNullOrEmpty
        EmptySHA512.ShouldBe("CF83E1357EEFB8BDF1542850D66D8007D620E4050B5715DC83F4A921D36CE9CE47D0D13C5D85F2B0FF8318D2877EEC2F63B931BD47417A81A538327AF927DA3E")
    End Sub

End Class