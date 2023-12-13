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

using KS.Drivers;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Textify.Sequences.Tools;

namespace KS.Misc.Text
{
    /// <summary>
    /// Tools for text manipulation
    /// </summary>
    public static class TextTools
    {
        internal static string defaultFigletFontName = "speed";
        private static readonly string regexMatchEnclosedStrings = /* lang=regex */
            @"(""(.+?)(?<![^\\]\\)"")|('(.+?)(?<![^\\]\\)')|(`(.+?)(?<![^\\]\\)`)|(?:[^\\\s]|\\.)+|\S+";

        /// <summary>
        /// Default figlet font name
        /// </summary>
        public static string DefaultFigletFontName =>
            Config.MainConfig.DefaultFigletFontName ?? "speed";

        /// <summary>
        /// Gets the wrapped sentences for text wrapping for console
        /// </summary>
        /// <param name="text">Text to be wrapped</param>
        /// <param name="maximumLength">Maximum length of text before wrapping</param>
        public static string[] GetWrappedSentences(string text, int maximumLength) =>
            GetWrappedSentences(text, maximumLength, 0);

        /// <summary>
        /// Gets the wrapped sentences for text wrapping for console
        /// </summary>
        /// <param name="text">Text to be wrapped</param>
        /// <param name="maximumLength">Maximum length of text before wrapping</param>
        /// <param name="indentLength">Indentation length</param>
        public static string[] GetWrappedSentences(string text, int maximumLength, int indentLength)
        {
            if (string.IsNullOrEmpty(text))
                return [""];

            // Split the paragraph into sentences that have the length of maximum characters that can be printed in various terminal
            // sizes.
            var IncompleteSentences = new List<string>();
            var IncompleteSentenceBuilder = new StringBuilder();

            // Make the text look like it came from Linux
            text = text.Replace(Convert.ToString(Convert.ToChar(13)), "");

            // Convert tabs to four spaces
            text = text.Replace("\t", "    ");

            // This indent length count tells us how many spaces are used for indenting the paragraph. This is only set for
            // the first time and will be reverted back to zero after the incomplete sentence is formed.
            foreach (string splitText in text.SplitNewLines())
            {
                var sequencesCollections = VtSequenceTools.MatchVTSequences(splitText);
                foreach (var sequences in sequencesCollections)
                {
                    int vtSeqIdx = 0;
                    int vtSeqCompensate = 0;
                    if (splitText.Length == 0)
                        IncompleteSentences.Add(splitText);
                    for (int i = 0; i < splitText.Length; i++)
                    {
                        // Check the character to see if we're at the VT sequence
                        bool explicitNewLine = splitText[^1] == '\n';
                        char ParagraphChar = splitText[i];
                        bool isNewLine = splitText[i] == '\n';
                        string seq = "";
                        if (sequences.Count > 0 && sequences[vtSeqIdx].Index == i)
                        {
                            // We're at an index which is the same as the captured VT sequence. Get the sequence
                            seq = sequences[vtSeqIdx].Value;

                            // Raise the index in case we have the next sequence, but only if we're sure that we have another
                            if (vtSeqIdx + 1 < sequences.Count)
                                vtSeqIdx++;

                            // Raise the paragraph index by the length of the sequence
                            i += seq.Length - 1;
                            vtSeqCompensate += seq.Length;
                        }

                        // Append the character into the incomplete sentence builder.
                        if (!isNewLine)
                            IncompleteSentenceBuilder.Append(!string.IsNullOrEmpty(seq) ? seq : ParagraphChar.ToString());

                        // Also, compensate the \0 characters
                        if (splitText[i] == '\0')
                            vtSeqCompensate++;

                        // Check to see if we're at the maximum character number or at the new line
                        if (IncompleteSentenceBuilder.Length == maximumLength - indentLength + vtSeqCompensate |
                            i == splitText.Length - 1 |
                            isNewLine)
                        {
                            // We're at the character number of maximum character. Add the sentence to the list for "wrapping" in columns.
                            DebugWriter.WriteDebug(DebugLevel.I, "Adding {0} to the list... Incomplete sentences: {1}", IncompleteSentenceBuilder.ToString(), IncompleteSentences.Count);
                            IncompleteSentences.Add(IncompleteSentenceBuilder.ToString());
                            if (explicitNewLine)
                                IncompleteSentences.Add("");

                            // Clean everything up
                            IncompleteSentenceBuilder.Clear();
                            indentLength = 0;
                            vtSeqCompensate = 0;
                        }
                    }
                }
            }

            DebugWriter.WriteDebug(DebugLevel.I, "{0} incomplete sentences...", IncompleteSentences.Count);
            return [.. IncompleteSentences];
        }

        /// <summary>
        /// Splits the string enclosed in double quotes delimited by spaces using regular expression formula
        /// </summary>
        /// <param name="target">Target string</param>
        public static string[] SplitEncloseDoubleQuotes(this string target)
        {
            if (target is null)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The target may not be null"));

            return DriverHandler.CurrentRegexpDriverLocal
                .Matches(target, regexMatchEnclosedStrings)
                .Select((m) => m.Value.ReleaseDoubleQuotes())
                .ToArray();
        }

        /// <summary>
        /// Splits the string enclosed in double quotes delimited by spaces using regular expression formula without releasing double quotes
        /// </summary>
        /// <param name="target">Target string</param>
        public static string[] SplitEncloseDoubleQuotesNoRelease(this string target)
        {
            if (target is null)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The target may not be null"));

            return DriverHandler.CurrentRegexpDriverLocal
                .Matches(target, regexMatchEnclosedStrings)
                .Select((m) => m.Value)
                .ToArray();
        }

        /// <summary>
        /// Releases a string from double quotations
        /// </summary>
        /// <param name="target">Target string</param>
        /// <returns>A string that doesn't contain double quotation marks at the start and at the end of the string</returns>
        public static string ReleaseDoubleQuotes(this string target)
        {
            if (target is null)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The target may not be null"));

            string ReleasedString = target;
            if (target.StartsWith('\"') && target.EndsWith('\"') && target != "\"" ||
                target.StartsWith('\'') && target.EndsWith('\'') && target != "'" ||
                target.StartsWith('`') && target.EndsWith('`') && target != "`")
            {
                ReleasedString = ReleasedString.Remove(0, 1);
                ReleasedString = ReleasedString.Remove(ReleasedString.Length - 1);
            }
            return ReleasedString;
        }

        /// <summary>
        /// Gets the enclosed double quotes type from the string
        /// </summary>
        /// <param name="target">Target string to query</param>
        /// <returns><see cref="EnclosedDoubleQuotesType"/> containing information about the current string enclosure</returns>
        public static EnclosedDoubleQuotesType GetEnclosedDoubleQuotesType(this string target)
        {
            if (target is null)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The target may not be null"));

            var type = EnclosedDoubleQuotesType.None;
            if (target.StartsWith('\"') && target.EndsWith('\"') && target != "\"")
                type = EnclosedDoubleQuotesType.DoubleQuotes;
            else if (target.StartsWith('\'') && target.EndsWith('\'') && target != "'")
                type = EnclosedDoubleQuotesType.SingleQuotes;
            else if (target.StartsWith('`') && target.EndsWith('`') && target != "`")
                type = EnclosedDoubleQuotesType.Backticks;
            return type;
        }

        /// <summary>
        /// Truncates the string if the string is larger than the threshold, otherwise, returns an unchanged string
        /// </summary>
        /// <param name="target">Source string to truncate</param>
        /// <param name="threshold">Max number of string characters</param>
        /// <returns>Truncated string</returns>
        public static string Truncate(this string target, int threshold)
        {
            if (target is null)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The target may not be null"));

            // Try to truncate string. If the string length is bigger than the threshold, it'll be truncated to the length of
            // the threshold, putting three dots next to it. We don't use ellipsis marks here because we're dealing with the
            // terminal, and some terminals and some monospace fonts may not support that character, so we mimick it by putting
            // the three dots.
            //
            // In case VT sequences are inserted to the target string, it can mess with the actual length returned, so only
            // truncate the non-VT sequences.
            int newLength = VtSequenceTools.FilterVTSequences(target).Length;
            if (newLength > threshold)
                return target[..(threshold - 1)] + "...";
            else
                return target;
        }

        /// <summary>
        /// Makes a string array with new line as delimiter
        /// </summary>
        /// <param name="target">Target string</param>
        /// <returns>List of words that are separated by the new lines</returns>
        public static string[] SplitNewLines(this string target)
        {
            if (target is null)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The target may not be null"));

            return target
                .Replace($"{Convert.ToChar(13)}{Convert.ToChar(10)}", $"{Convert.ToChar(10)}")
                .Replace($"{Convert.ToChar(13)}", $"{Convert.ToChar(10)}")
                .Split(Convert.ToChar(10));
        }

        /// <summary>
        /// Makes a string array with new line as delimiter (the old way)
        /// </summary>
        /// <param name="target">Target string</param>
        /// <returns>List of words that are separated by the new lines</returns>
        public static string[] SplitNewLinesOld(this string target)
        {
            if (target is null)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The target may not be null"));

            return target
                .Replace(Convert.ToChar(13).ToString(), "")
                .Split(Convert.ToChar(10));
        }

        /// <summary>
        /// Checks to see if the string starts with any of the values
        /// </summary>
        /// <param name="target">Target string</param>
        /// <param name="values">Values</param>
        /// <returns>True if the string starts with any of the values specified in the array. Otherwise, false.</returns>
        public static bool StartsWithAnyOf(this string target, string[] values)
        {
            if (target is null)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The target may not be null"));

            bool started = false;
            foreach (string value in values)
                if (target.StartsWith(value))
                    started = true;
            return started;
        }

        /// <summary>
        /// Checks to see if the string contains any of the target strings.
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="targets">Target strings</param>
        /// <returns>True if one of them is found; otherwise, false.</returns>
        public static bool ContainsAnyOf(this string source, string[] targets)
        {
            if (source is null)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The source may not be null"));

            foreach (string target in targets)
                if (source.Contains(target))
                    return true;
            return false;
        }

        /// <summary>
        /// Replaces all the instances of strings with a string
        /// </summary>
        /// <param name="target">Target string</param>
        /// <param name="toBeReplaced">Strings to be replaced</param>
        /// <param name="toReplace">String to replace with</param>
        /// <returns>Modified string</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ReplaceAll(this string target, string[] toBeReplaced, string toReplace)
        {
            if (target is null)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The target may not be null"));
            if (toBeReplaced is null || toBeReplaced.Length == 0)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("Array of to be replaced strings may not be null"));

            foreach (string ReplaceTarget in toBeReplaced)
                target = target.Replace(ReplaceTarget, toReplace);
            return target;
        }

        /// <summary>
        /// Replaces all the instances of strings with a string assigned to each entry
        /// </summary>
        /// <param name="target">Target string</param>
        /// <param name="toBeReplaced">Strings to be replaced</param>
        /// <param name="toReplace">Strings to replace with</param>
        /// <returns>Modified string</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string ReplaceAllRange(this string target, string[] toBeReplaced, string[] toReplace)
        {
            if (target is null)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The target may not be null"));
            if (toBeReplaced is null || toBeReplaced.Length == 0)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("Array of to be replaced strings may not be null"));
            if (toReplace is null || toReplace.Length == 0)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("Array of to be replacement strings may not be null"));
            if (toBeReplaced.Length != toReplace.Length)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("Array length of which strings to be replaced doesn't equal the array length of which strings to replace."));

            for (int i = 0, loopTo = toBeReplaced.Length - 1; i <= loopTo; i++)
                target = target.Replace(toBeReplaced[i], toReplace[i]);
            return target;
        }

        /// <summary>
        /// Replaces last occurrence of a text in source string with the replacement
        /// </summary>
        /// <param name="source">A string which has the specified text to replace</param>
        /// <param name="searchText">A string to be replaced</param>
        /// <param name="replace">A string to replace</param>
        /// <returns>String that has its last occurrence of text replaced</returns>
        public static string ReplaceLastOccurrence(this string source, string searchText, string replace)
        {
            if (source is null)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The source may not be null"));
            if (searchText is null)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The search text may not be null"));

            int position = source.LastIndexOf(searchText);
            if (position == -1)
                return source;
            string result = source.Remove(position, searchText.Length).Insert(position, replace);
            return result;
        }

        /// <summary>
        /// Get all indexes of a value in string
        /// </summary>
        /// <param name="target">Source string</param>
        /// <param name="value">A value</param>
        /// <returns>Indexes of strings</returns>
        public static IEnumerable<int> AllIndexesOf(this string target, string value)
        {
            if (target is null)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The target may not be null"));
            if (string.IsNullOrEmpty(value))
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("Empty string value specified"));

            int index = 0;
            while (true)
            {
                index = target.IndexOf(value, index);
                if (index == -1)
                    break;
                yield return index;
                index += value.Length;
            }
        }

        /// <summary>
        /// Formats the string
        /// </summary>
        /// <param name="Format">The string to format</param>
        /// <param name="Vars">The variables used</param>
        /// <returns>A formatted string if successful, or the unformatted one if failed.</returns>
        public static string FormatString(string Format, params object[] Vars)
        {
            if (Format is null)
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The target format may not be null"));

            string FormattedString = Format;
            try
            {
                if (Vars.Length > 0)
                    FormattedString = string.Format(Format, Vars);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to format string: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return FormattedString;
        }

        /// <summary>
        /// Is the string numeric?
        /// </summary>
        /// <param name="Expression">The expression</param>
        public static bool IsStringNumeric(string Expression)
        {
            if (string.IsNullOrWhiteSpace(Expression))
                throw new KernelException(KernelExceptionType.Text, Translate.DoTranslation("The target expression may not be null"));

            return double.TryParse(Expression, out double _);
        }

        /// <summary>
		/// Removes a prefix from the text
		/// </summary>
		public static string RemovePrefix(this string text, string prefix)
        {
            if (text.StartsWith(prefix))
                return text[prefix.Length..].Trim();
            return text;
        }

        /// <summary>
        /// Removes a suffix from the text
        /// </summary>
        public static string RemoveSuffix(this string text, string suffix)
        {
            if (text.EndsWith(suffix))
                return text[..^suffix.Length];
            return text;
        }

        /// <summary>
        /// Gets a BASE64-encoded string from the text
        /// </summary>
        /// <param name="text">Text to encode to BASE64</param>
        /// <returns>A BASE64-encoded string from the text</returns>
        public static string GetBase64Encoded(this string text)
        {
            byte[] textBytes = Encoding.Default.GetBytes(text);
            return Convert.ToBase64String(textBytes);
        }

        /// <summary>
        /// Gets a BASE64-decoded string from the BASE64-encoded string
        /// </summary>
        /// <param name="text">Text to encode to BASE64</param>
        /// <returns>A BASE64-encoded string from the text</returns>
        public static string GetBase64Decoded(this string text)
        {
            byte[] textBytes = Convert.FromBase64String(text);
            return Encoding.Default.GetString(textBytes);
        }
    }
}
