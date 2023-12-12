//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using BassBoom.Basolia.File;
using BassBoom.Basolia.Format;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs.Styles.Infobox;
using KS.Files.Operations.Querying;
using KS.Languages;
using System.IO;

namespace Nitrocid.Extras.BassBoom.Player
{
    internal static class PlayerHandler
    {
        public static void Handle(string path)
        {
            if (!Checking.FileExists(path))
            {
                InfoBoxColor.WriteInfoBoxKernelColor(Translate.DoTranslation("Can't open music file '{0}' because it's not found."), KernelColorType.Error, path);
                return;
            }
            if (!PlayerTui.musicFiles.Contains(path))
                PlayerTui.musicFiles.Add(path);
            PlayerControls.PopulateMusicFileInfo(path);
            PlayerTui.PlayerLoop();
        }

        public static string InfoHandle(string path)
        {
            // Get the ID3 metadata
            FileTools.OpenFile(path);
            AudioInfoTools.GetId3Metadata(out var metadatav1, out var metadatav2);
            FileTools.CloseFile();

            // Now, populate the necessary variables
            string musicName =
                !string.IsNullOrEmpty(metadatav2.Title) ? metadatav2.Title :
                !string.IsNullOrEmpty(metadatav1.Title) ? metadatav1.Title :
                Path.GetFileNameWithoutExtension(path);
            string musicArtist =
                !string.IsNullOrEmpty(metadatav2.Artist) ? metadatav2.Artist :
                !string.IsNullOrEmpty(metadatav1.Artist) ? metadatav1.Artist :
                Translate.DoTranslation("Unknown Artist");
            string musicGenre =
                !string.IsNullOrEmpty(metadatav2.Genre) ? metadatav2.Genre :
                metadatav1.GenreIndex >= 0 ? $"{metadatav1.Genre} [{metadatav1.GenreIndex}]" :
                Translate.DoTranslation("Unknown Genre");
            return
                $"{Translate.DoTranslation("Song name")}: {musicName}\n" +
                $"{Translate.DoTranslation("Song artist")}: {musicArtist}\n" +
                $"{Translate.DoTranslation("Song genre")}: {musicGenre}";
        }
    }
}
