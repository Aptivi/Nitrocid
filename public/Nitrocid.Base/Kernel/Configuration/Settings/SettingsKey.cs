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

using Newtonsoft.Json;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Configuration.Settings.KeyInputs;
using System;
using System.Diagnostics;

namespace Nitrocid.Base.Kernel.Configuration.Settings
{
    /// <summary>
    /// Settings key entry
    /// </summary>
    [DebuggerDisplay("Name = {Name}, Type = {Type}, Variable = {Variable}, Description = {Description}")]
    public class SettingsKey
    {
        // General
        [JsonProperty(nameof(Name))]
        internal string name = "";
        [JsonProperty(nameof(Type))]
        internal string type = "";
        [JsonProperty(nameof(Variable))]
        internal string variable = "";
        [JsonProperty(nameof(Description))]
        internal string description = "";

        // String
        [JsonProperty(nameof(Masked))]
        internal bool masked;

        // Selection
        [JsonProperty(nameof(SelectionFunctionName))]
        internal string selectionFunctionName = "";
        [JsonProperty(nameof(SelectionFunctionType))]
        internal string selectionFunctionType = "";
        [JsonProperty(nameof(SelectionFallback))]
        internal string[]? selectionFallback;
        [JsonProperty(nameof(IsSelectionFunctionDict))]
        internal bool isSelectionFunctionDict;
        [JsonProperty(nameof(SelectionFunctionArgs))]
        internal SettingsFunctionArgs[] selectionFunctionArgs = [];

        // Enumeration
        [JsonProperty(nameof(IsEnumeration))]
        internal bool isEnumeration;
        [JsonProperty(nameof(EnumerationInternal))]
        internal bool enumerationInternal;
        [JsonProperty(nameof(Enumeration))]
        internal string enumeration = "";
        [JsonProperty(nameof(EnumerationAssembly))]
        internal string enumerationAssembly = "";
        [JsonProperty(nameof(EnumerationZeroBased))]
        internal bool enumerationZeroBased;
        [JsonProperty(nameof(EnumerableIndex))]
        internal int enumerableIndex;

        // Path
        [JsonProperty(nameof(IsValuePath))]
        internal bool isValuePath;
        [JsonProperty(nameof(IsPathCurrentPath))]
        internal bool isPathCurrentPath;
        [JsonProperty(nameof(ValuePathType))]
        internal string valuePathType = "";

        // List
        [JsonProperty(nameof(DelimiterVariable))]
        internal string delimiterVariable = "";
        [JsonProperty(nameof(DelimiterVariableType))]
        internal string delimiterVariableType = "";
        [JsonProperty(nameof(Delimiter))]
        internal string delimiter = "";

        // Int slider
        [JsonProperty(nameof(MinimumValue))]
        internal int minimumValue;
        [JsonProperty(nameof(MaximumValue))]
        internal int maximumValue;

        // Misc
        [JsonProperty(nameof(UnsupportedPlatforms))]
        internal string[]? unsupportedPlatforms;
        [JsonProperty(nameof(ShellType))]
        internal string shellType = "";
        [JsonProperty(nameof(Tip))]
        internal string tip = "";
        [JsonProperty(nameof(Variables))]
        internal SettingsKey[] variables = [];

        // Internal
        [JsonIgnore]
        internal ISettingsKeyInput? keyInput;

        /// <summary>
        /// Settings key name
        /// </summary>
        [JsonIgnore]
        public string Name =>
            name;

        /// <summary>
        /// Settings key type
        /// </summary>
        [JsonIgnore]
        public SettingsKeyType Type
        {
            get
            {
                bool enumerated = Enum.TryParse(type, out SettingsKeyType keyType);
                if (enumerated)
                    return keyType;
                return default;
            }
        }

        /// <summary>
        /// Target variable to work on
        /// </summary>
        [JsonIgnore]
        public string Variable =>
            variable;

        /// <summary>
        /// Settings description
        /// </summary>
        [JsonIgnore]
        public string Description =>
            description;

        /// <summary>
        /// Is this string masked? (useful for passwords)
        /// </summary>
        [JsonIgnore]
        public bool Masked =>
            masked;

        /// <summary>
        /// Is this variable an enumeration?
        /// </summary>
        [JsonIgnore]
        public bool IsEnumeration =>
            isEnumeration;

        /// <summary>
        /// Function name to use for getting a list
        /// </summary>
        [JsonIgnore]
        public string SelectionFunctionName =>
            selectionFunctionName;

        /// <summary>
        /// Function type that contains the function name
        /// </summary>
        [JsonIgnore]
        public string SelectionFunctionType =>
            selectionFunctionType;

        /// <summary>
        /// Does the specified function name returns a dictionary?
        /// </summary>
        [JsonIgnore]
        public bool IsSelectionFunctionDict =>
            isSelectionFunctionDict;

        /// <summary>
        /// Specifies function arguments for selection
        /// </summary>
        [JsonIgnore]
        public SettingsFunctionArgs[] SelectionFunctionArgs =>
            selectionFunctionArgs;

        /// <summary>
        /// Fallback values for selection
        /// </summary>
        [JsonIgnore]
        public string[] SelectionFallback =>
            selectionFallback ?? [];

        /// <summary>
        /// Is the enumeration found within Nitrocid?
        /// </summary>
        [JsonIgnore]
        public bool EnumerationInternal =>
            enumerationInternal;

        /// <summary>
        /// Enumeration type name
        /// </summary>
        [JsonIgnore]
        public string Enumeration =>
            enumeration;

        /// <summary>
        /// Assembly name in which it contains the selected enumeration (should be external)
        /// </summary>
        [JsonIgnore]
        public string EnumerationAssembly =>
            enumerationAssembly;

        /// <summary>
        /// Is the enumeration zero based? If not, it's assumed to be one-based.
        /// </summary>
        [JsonIgnore]
        public bool EnumerationZeroBased =>
            enumerationZeroBased;

        /// <summary>
        /// Enumerable index
        /// </summary>
        [JsonIgnore]
        public int EnumerableIndex =>
            enumerableIndex;

        /// <summary>
        /// List of unsupported platforms: windows, unix, macos.
        /// </summary>
        [JsonIgnore]
        public string[] UnsupportedPlatforms =>
            unsupportedPlatforms ?? [];

        /// <summary>
        /// Is the value a path that will be neutralized by <see cref="FilesystemTools.NeutralizePath(string, bool)"/>?
        /// </summary>
        [JsonIgnore]
        public bool IsValuePath =>
            isValuePath;

        /// <summary>
        /// Is the path the current path?
        /// </summary>
        [JsonIgnore]
        public bool IsPathCurrentPath =>
            isPathCurrentPath;

        /// <summary>
        /// The variable containing the delimiter character
        /// </summary>
        [JsonIgnore]
        public string DelimiterVariable =>
            delimiterVariable;

        /// <summary>
        /// The variable type that contains the <see cref="DelimiterVariable"/>
        /// </summary>
        [JsonIgnore]
        public string DelimiterVariableType =>
            delimiterVariableType;

        /// <summary>
        /// The shell type to work on (for shell-related settings)
        /// </summary>
        [JsonIgnore]
        public string ShellType =>
            shellType;

        /// <summary>
        /// Minimum value that the slider can go to
        /// </summary>
        [JsonIgnore]
        public int MinimumValue =>
            minimumValue;

        /// <summary>
        /// Maximum value that the slider can go to
        /// </summary>
        [JsonIgnore]
        public int MaximumValue =>
            maximumValue;

        /// <summary>
        /// The delimiter character
        /// </summary>
        [JsonIgnore]
        public string Delimiter =>
            delimiter;

        /// <summary>
        /// The value path type
        /// </summary>
        [JsonIgnore]
        public KernelPathType ValuePathType
        {
            get
            {
                bool enumerated = Enum.TryParse(valuePathType, out KernelPathType keyType);
                if (enumerated)
                    return keyType;
                return default;
            }
        }

        /// <summary>
        /// Extra information about using this settings entry
        /// </summary>
        [JsonIgnore]
        public string Tip =>
            tip;

        /// <summary>
        /// List of needed variables (for <see cref="SettingsKeyType.SMultivar"/>)
        /// </summary>
        [JsonIgnore]
        public SettingsKey[] Variables =>
            variables;

        [JsonIgnore]
        internal bool Unsupported =>
            SettingsAppTools.IsUnsupported(this);

        internal ISettingsKeyInput KeyInput
        {
            get
            {
                // Get cached key input
                if (keyInput is not null)
                    return keyInput;

                keyInput = Type switch
                {
                    SettingsKeyType.SBoolean => new BooleanSettingsKeyInput(),
                    SettingsKeyType.SInt => new IntSettingsKeyInput(),
                    SettingsKeyType.SString => new StringSettingsKeyInput(),
                    SettingsKeyType.SSelection => new SelectionSettingsKeyInput(),
                    SettingsKeyType.SList => new ListSettingsKeyInput(),
                    SettingsKeyType.SColor => new ColorSettingsKeyInput(),
                    SettingsKeyType.SChar => new CharSettingsKeyInput(),
                    SettingsKeyType.SIntSlider => new IntSliderSettingsKeyInput(),
                    SettingsKeyType.SDouble => new DoubleSettingsKeyInput(),
                    SettingsKeyType.SPreset => new PresetSettingsKeyInput(),
                    SettingsKeyType.SFiglet => new FigletSettingsKeyInput(),
                    SettingsKeyType.SIcon => new IconSettingsKeyInput(),
                    SettingsKeyType.SMultivar => new MultivarSettingsKeyInput(),
                    SettingsKeyType.SUnknown => new UnknownSettingsKeyInput(),
                    _ => new UnknownSettingsKeyInput(),
                };
                return keyInput;
            }
        }

        [JsonConstructor]
        internal SettingsKey()
        { }
    }
}
