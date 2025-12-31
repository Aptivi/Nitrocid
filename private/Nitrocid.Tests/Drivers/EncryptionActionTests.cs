//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.IO;
using Nitrocid.Base.Drivers.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using SpecProbe.Software.Platform;
using System;

namespace Nitrocid.Tests.Drivers
{

    [TestClass]
    public class EncryptionActionTests
    {

        /// <summary>
        /// Tests string encryption
        /// </summary>
        [TestMethod]
        [DataRow("MD5", "C4C1867580D6D25B11210F84F935359A")]
        [DataRow("SHA1", "CFF9FDA895B0B638957E17CF952457D81ADD622F")]
        [DataRow("SHA256", "525514740C93C5442DBCB8FB92FB1B17B6F8B94B3C98E6F07CA8AEB093C2E79F")]
        [DataRow("SHA384", "B26ADFF6A6BDD59612F4B560E7D2A0240D7A611AF46BD4D2891181F46341E4886A8D74724877955AFC908F6B17A5B613")]
        [DataRow("SHA512", "0015CAF195A7248127F7E50C8D839935681A2234344387B5E9DF761E6D4F152CC4458ADCD45A19F59413EA6BC5E7C907A01A0B47B548CE0DAD04787CE416157D")]
        [DataRow("SHA256Enhanced", "8136836181082CB424CDDF914D4906484AC9D5A957CBC557CC2461568F0FFF6F")]
        [DataRow("SHA384Enhanced", "3C8C2D1BEB552DC3D0C07976F9E00FD1108FD3472CFE9013CD9A593144AD2CEAA165F704E972A8679B9C52BBC612077E")]
        [DataRow("SHA512Enhanced", "1C8C419D1081F06B359B5F1D5F333CC9F46E8E807F4F11D5C4F593CE844BEA83686D9A6053EA4C484D052E11F57C64E4D580BA9DC9B50327A626E545E2E39731")]
        [Description("Action")]
        public void TestGetEncryptedString(string Algorithm, string expected)
        {
            if (Algorithm.EndsWith("Enhanced"))
            {
                if (PlatformHelper.IsOnWindows() && !OperatingSystem.IsWindowsVersionAtLeast(10, 0, 25324))
                    Assert.Inconclusive("Your version of Windows doesn't support SHA-3 yet.");
                if (PlatformHelper.IsOnMacOS())
                    Assert.Inconclusive("macOS doesn't support SHA-3 yet.");
            }
            string TextHash = "Test hashing.";
            string actual = Encryption.GetEncryptedString(TextHash, Algorithm);
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Tests file encryption
        /// </summary>
        [TestMethod]
        [DataRow("MD5", "C4C1867580D6D25B11210F84F935359A")]
        [DataRow("SHA1", "CFF9FDA895B0B638957E17CF952457D81ADD622F")]
        [DataRow("SHA256", "525514740C93C5442DBCB8FB92FB1B17B6F8B94B3C98E6F07CA8AEB093C2E79F")]
        [DataRow("SHA384", "B26ADFF6A6BDD59612F4B560E7D2A0240D7A611AF46BD4D2891181F46341E4886A8D74724877955AFC908F6B17A5B613")]
        [DataRow("SHA512", "0015CAF195A7248127F7E50C8D839935681A2234344387B5E9DF761E6D4F152CC4458ADCD45A19F59413EA6BC5E7C907A01A0B47B548CE0DAD04787CE416157D")]
        [Description("Action")]
        public void TestGetEncryptedFileUsingStream(string Algorithm, string expected)
        {
            var FileStreamHash = File.Create(InitTest.PathToTestSlotFolder + "/TestSum.txt");
            FileStreamHash.Write(System.Text.Encoding.Default.GetBytes("Test hashing."), 0, 13);
            FileStreamHash.Flush();
            FileStreamHash.Position = 0L;
            string ResultHash = Encryption.GetEncryptedFile(FileStreamHash, Algorithm);
            FileStreamHash.Close();
            File.Delete(InitTest.PathToTestSlotFolder + "/TestSum.txt");
            ResultHash.ShouldBe(expected);
        }

        /// <summary>
        /// Tests file encryption
        /// </summary>
        [TestMethod]
        [DataRow("MD5", "CD5578C85A4CF32E48D157746A90C7F6")]
        [DataRow("SHA1", "36EBF31AF7234D6C99CA65DC4EDA524161600657")]
        [DataRow("SHA256", "7E6857729A34755DE8C2C9E535A8765BDE241F593BE3588B8FA6D29D949EFADA")]
        [DataRow("SHA384", "92CBCB3F982C7EC24EED668175D4FE7C73D9BBCBECA659EDDE6D6E56B798D64C808F86C7E13FA6BE03464AE2D145BB60")]
        [DataRow("SHA512", "6DF635C184D4B131B0243D4F2BD66925A61B82A5093F573920F42D7B8474D6332FD2886920F3CA36D9206C73DD59C8F1EEA18501E6FEF15FDDA664B1ABB0E361")]
        [Description("Action")]
        public void TestGetEncryptedFileUsingPath(string Algorithm, string expected)
        {
            var FileStreamHash = File.Create(InitTest.PathToTestSlotFolder + "/TestSum.txt");
            FileStreamHash.Write(System.Text.Encoding.Default.GetBytes("Test hashing with path."), 0, 23);
            FileStreamHash.Flush();
            FileStreamHash.Close();
            string FileHash = Encryption.GetEncryptedFile(InitTest.PathToTestSlotFolder + "/TestSum.txt", Algorithm);
            File.Delete(InitTest.PathToTestSlotFolder + "/TestSum.txt");
            FileHash.ShouldBe(expected);
        }

        /// <summary>
        /// Tests hash verification
        /// </summary>
        [TestMethod]
        [DataRow("MD5", "CD5578C85A4CF32E48D157746A90C7F6", true)]
        [DataRow("SHA1", "36EBF31AF7234D6C99CA65DC4EDA524161600657", true)]
        [DataRow("SHA256", "7E6857729A34755DE8C2C9E535A8765BDE241F593BE3588B8FA6D29D949EFADA", true)]
        [DataRow("SHA384", "92CBCB3F982C7EC24EED668175D4FE7C73D9BBCBECA659EDDE6D6E56B798D64C808F86C7E13FA6BE03464AE2D145BB60", true)]
        [DataRow("SHA512", "6DF635C184D4B131B0243D4F2BD66925A61B82A5093F573920F42D7B8474D6332FD2886920F3CA36D9206C73DD59C8F1EEA18501E6FEF15FDDA664B1ABB0E361", true)]
        [Description("Action")]
        public void TestVerifyHashFromHash(string Algorithm, string ExpectedHash, bool expected)
        {
            var FileStreamHash = File.Create(InitTest.PathToTestSlotFolder + "/TestSum.txt");
            FileStreamHash.Write(System.Text.Encoding.Default.GetBytes("Test hashing with path."), 0, 23);
            FileStreamHash.Flush();
            FileStreamHash.Close();
            string FileHash = Encryption.GetEncryptedFile(InitTest.PathToTestSlotFolder + "/TestSum.txt", Algorithm);
            bool Result = HashVerifier.VerifyHashFromHash(InitTest.PathToTestSlotFolder + "/TestSum.txt", Algorithm, ExpectedHash, FileHash);
            File.Delete(InitTest.PathToTestSlotFolder + "/TestSum.txt");
            Result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests hash verification for an uncalculated file
        /// </summary>
        [TestMethod]
        [DataRow("MD5", "CD5578C85A4CF32E48D157746A90C7F6", true)]
        [DataRow("SHA1", "36EBF31AF7234D6C99CA65DC4EDA524161600657", true)]
        [DataRow("SHA256", "7E6857729A34755DE8C2C9E535A8765BDE241F593BE3588B8FA6D29D949EFADA", true)]
        [DataRow("SHA384", "92CBCB3F982C7EC24EED668175D4FE7C73D9BBCBECA659EDDE6D6E56B798D64C808F86C7E13FA6BE03464AE2D145BB60", true)]
        [DataRow("SHA512", "6DF635C184D4B131B0243D4F2BD66925A61B82A5093F573920F42D7B8474D6332FD2886920F3CA36D9206C73DD59C8F1EEA18501E6FEF15FDDA664B1ABB0E361", true)]
        [Description("Action")]
        public void TestVerifyUncalculatedHashFromHash(string Algorithm, string ExpectedHash, bool expected)
        {
            var FileStreamHash = File.Create(InitTest.PathToTestSlotFolder + "/TestSum.txt");
            FileStreamHash.Write(System.Text.Encoding.Default.GetBytes("Test hashing with path."), 0, 23);
            FileStreamHash.Flush();
            FileStreamHash.Close();
            bool Result = HashVerifier.VerifyUncalculatedHashFromHash(InitTest.PathToTestSlotFolder + "/TestSum.txt", Algorithm, ExpectedHash);
            File.Delete(InitTest.PathToTestSlotFolder + "/TestSum.txt");
            Result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting empty hash
        /// </summary>
        [TestMethod]
        [DataRow("MD5", "D41D8CD98F00B204E9800998ECF8427E")]
        [DataRow("SHA1", "DA39A3EE5E6B4B0D3255BFEF95601890AFD80709")]
        [DataRow("SHA256", "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855")]
        [DataRow("SHA384", "38B060A751AC96384CD9327EB1B1E36A21FDB71114BE07434C0CC7BF63F6E1DA274EDEBFE76F65FBD51AD2F14898B95B")]
        [DataRow("SHA512", "CF83E1357EEFB8BDF1542850D66D8007D620E4050B5715DC83F4A921D36CE9CE47D0D13C5D85F2B0FF8318D2877EEC2F63B931BD47417A81A538327AF927DA3E")]
        [Description("Action")]
        public void TestGetEmptyHash(string Algorithm, string expected)
        {
            string Empty = Encryption.GetEmptyHash(Algorithm);
            Empty.ShouldNotBeNullOrEmpty();
            Empty.ShouldBe(expected);
        }

    }
}
