
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.IO;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.Encryption;
using KS.Files;
using KS.Files.Operations.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Verifies the file
    /// </summary>
    /// <remarks>
    /// If you've previously calculated the hash sum of a file using the supported algorithms, and you have the expected hash or file that contains the hashes list, you can use this command to verify the sanity of the file.
    /// <br></br>
    /// It can handle three types:
    /// <br></br>
    /// <list type="bullet">
    /// <item>
    /// <term>First</term>
    /// <description>It can verify files by comparing expected hash with the actual hash.</description>
    /// </item>
    /// <item>
    /// <term>Second</term>
    /// <description>It can verify files by opening the hashes file that sumfiles generated and finding the hash of the file.</description>
    /// </item>
    /// <item>
    /// <term>Third</term>
    /// <description>It can verify files by opening the hashes file that some other tool generated and finding the hash of the file, assuming that it's in this format: <c>&lt;expected hash&gt; &lt;file name&gt;</c></description>
    /// </item>
    /// </list>
    /// <br></br>
    /// If the hashes match, that means that the file is not corrupted. However, if they don't match, the file is corrupted and must be redownloaded.
    /// <br></br>
    /// If you run across a hash file that verify can't parse, feel free to post an issue or make a PR.
    /// </remarks>
    class VerifyCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            try
            {
                string HashFile = FilesystemTools.NeutralizePath(parameters.ArgumentsList[2]);
                if (Checking.FileExists(HashFile))
                {
                    if (HashVerifier.VerifyHashFromHashesFile(parameters.ArgumentsList[3], parameters.ArgumentsList[0], parameters.ArgumentsList[2], parameters.ArgumentsList[1]))
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Hashes match."));
                        return 0;
                    }
                    else
                    {
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Hashes don't match."), true, KernelColorType.Warning);
                        return 4;
                    }
                }
                else if (HashVerifier.VerifyHashFromHash(parameters.ArgumentsList[3], parameters.ArgumentsList[0], parameters.ArgumentsList[2], parameters.ArgumentsList[1]))
                {
                    TextWriterColor.Write(Translate.DoTranslation("Hashes match."));
                    return 0;
                }
                else
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Hashes don't match."), true, KernelColorType.Warning);
                    return 4;
                }
            }
            catch (KernelException ihae) when (ihae.ExceptionType == KernelExceptionType.InvalidHashAlgorithm)
            {
                DebugWriter.WriteDebugStackTrace(ihae);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Invalid encryption algorithm."), true, KernelColorType.Error);
                return 10000 + (int)ihae.ExceptionType;
            }
            catch (KernelException ihe) when (ihe.ExceptionType == KernelExceptionType.InvalidHash)
            {
                DebugWriter.WriteDebugStackTrace(ihe);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Hashes are malformed."), true, KernelColorType.Error);
                return 10000 + (int)ihe.ExceptionType;
            }
            catch (FileNotFoundException fnfe)
            {
                DebugWriter.WriteDebugStackTrace(fnfe);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("{0} is not found."), true, KernelColorType.Error, parameters.ArgumentsList[3]);
                return 10000 + (int)KernelExceptionType.Encryption;
            }
        }

    }
}
