//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Languages;
using System.Text;
using Terminaux.Base;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Nitrocid.Users.Login.Widgets.Implementations
{
    internal class Emoji : BaseWidget, IWidget
    {
        private string cachedIcon = "";

        public override string Cleanup(int left, int top, int width, int height) =>
            "";

        public override string Initialize(int left, int top, int width, int height)
        {
            // Get the dimensions
            int iconHeight = height;
            int iconWidth = height * 2;
            int iconLeft = left + (width / 2) - height;
            int iconTop = top;

            // Render the icon, caching it in the process
            if (AddonTools.GetAddon(InterAddonTranslations.GetAddonName(KnownAddons.ExtrasImagesIcons)) is null)
            {
                var message = new AlignedText()
                {
                    Text = Translate.DoTranslation("The icons addon needs to be installed before being able to display this emoji."),
                    Top = iconTop,
                    Left = left,
                    Width = width,
                    ForegroundColor = KernelColorTools.GetColor(KernelColorType.Error),
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    },
                };
                return message.Render();
            }
            var type = InterAddonTools.GetTypeFromAddon(KnownAddons.ExtrasImagesIcons, "Nitrocid.Extras.Images.Icons.Tools.IconsTools");
            string[] emojiList = (string[]?)InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasImagesIcons, "GetIconNames", type) ?? [];
            string finalEmojiName = Config.WidgetConfig.EmojiWidgetCycleEmoticons ? emojiList[RandomDriver.RandomIdx(emojiList.Length)] : Config.WidgetConfig.EmojiWidgetEmoticonName;
            cachedIcon = (string?)InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasImagesIcons, "RenderIcon", type, finalEmojiName, iconWidth, iconHeight, iconLeft, iconTop) ?? "";
            return "";
        }

        public override string Render(int left, int top, int width, int height)
        {
            var display = new StringBuilder();

            // Print everything
            display.Append(cachedIcon);
            return display.ToString();
        }
    }
}
