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

using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Configuration.Instances;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Terminaux.Colors;

namespace Nitrocid.Tests.Kernel.Configuration.CustomConfigs
{
    internal class KernelCustomSettings : BaseKernelConfig
    {
        private readonly string _entriesJson =
            """
            [
                {
                    "Name": "CustomSection",
                    "Desc": "This is the custom section.",
                    "DisplayAs": "Custom section",
                    "Keys": [
                        {
                            "Name": "Custom switch",
                            "Type": "SBoolean",
                            "Variable": "CustomSwitch",
                            "Description": "Specifies the custom switch."
                        },
                        {
                            "Name": "Custom character",
                            "Type": "SChar",
                            "Variable": "CustomChar",
                            "Description": "Specifies the custom character."
                        },
                        {
                            "Name": "Custom color",
                            "Type": "SColor",
                            "Variable": "CustomColor",
                            "Description": "Specifies the custom color."
                        },
                        {
                            "Name": "Custom double",
                            "Type": "SDouble",
                            "Variable": "CustomDouble",
                            "Description": "Specifies the custom double."
                        },
                        {
                            "Name": "Custom figlet font",
                            "Type": "SFiglet",
                            "Variable": "CustomFigletFont",
                            "Description": "Specifies the custom figlet font."
                        },
                        {
                            "Name": "Custom integer",
                            "Type": "SInt",
                            "Variable": "CustomInt",
                            "Description": "Specifies the custom integer."
                        },
                        {
                            "Name": "Custom integer slider",
                            "Type": "SIntSlider",
                            "Variable": "CustomIntSlider",
                            "Description": "Specifies the custom integer slider.",
                            "MinimumValue": 0,
                            "MaximumValue": 255
                        },
                        {
                            "Name": "Custom list",
                            "Type": "SList",
                            "Variable": "CustomList",
                            "SelectionFunctionName": "GetPathList",
                            "SelectionFunctionType": "Filesystem",
                            "DelimiterVariable": "PathLookupDelimiter",
                            "IsValuePath": true,
                            "IsPathCurrentPath": true,
                            "Description": "Specifies the custom list."
                        },
                        {
                            "Name": "Custom preset",
                            "Type": "SPreset",
                            "Variable": "CustomPreset",
                            "ShellType": "Shell",
                            "Description": "Specifies the custom preset."
                        },
                        {
                            "Name": "Custom selection",
                            "Type": "SSelection",
                            "Variable": "CustomSelection",
                            "IsEnumeration": false,
                            "SelectionFunctionName": "ListAllLanguages",
                            "SelectionFunctionType": "LanguageManager",
                            "IsSelectionFunctionDict": true,
                            "SelectionFallback": [ "en-US" ],
                            "Description": "Specifies the custom FilesystemTools."
                        },
                        {
                            "Name": "Custom string",
                            "Type": "SString",
                            "Variable": "CustomString",
                            "Description": "Specifies the custom string."
                        }
                    ]
                }
            ]
            """;

        public override SettingsEntry[] SettingsEntries =>
            ConfigTools.GetSettingsEntries(_entriesJson);

        public char CustomChar { get; set; } = 'A';

        public string CustomColor { get; set; } = Color.Empty.PlainSequence;

        public double CustomDouble { get; set; } = 0.5d;

        public bool CustomSwitch { get; set; } = true;

        public string CustomFigletFont { get; set; } = "small";

        public int CustomInt { get; set; } = 1;

        public int CustomIntSlider { get; set; } = 4;

        public string CustomList { get; set; } = "";

        public string CustomPreset { get; set; } = "Default";

        public string CustomString { get; set; } = "Default";

        public string CustomSelection { get; set; } = "en-US";
    }
}
