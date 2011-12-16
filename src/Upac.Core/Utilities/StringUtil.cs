namespace Upac.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using Upac.Core.Diagnostics;

    public static class StringUtil
    {
        #region Methods

        public static string Clip(string text, int maxLength, bool ellipsis)
        {
            text = RemoveMultipleSpaces(RemoveNewLines(NbspToSpace(text), true));
            if (text.Length > maxLength)
            {
                if (ellipsis)
                {
                    maxLength -= 3;
                }

                int num = text.LastIndexOf(" ", maxLength);
                if (num < 0)
                {
                    num = maxLength;
                }
                text = text.Substring(0, num);

                if (ellipsis)
                {
                    text = text + "...";
                }
            }
            return text;
        }

        public static string Left(string text, int length)
        {
            Assert.EnsureStringValue(text, "text");
            if (length <= 0)
            {
                return string.Empty;
            }
            if (text.Length <= length)
            {
                return text;
            }
            return text.Substring(0, length);
        }

        public static string Mid(string text, int start)
        {
            Assert.EnsureStringValue(text, "text");
            if ((start < text.Length) && (start >= 0))
            {
                return text.Substring(start);
            }
            return string.Empty;
        }

        /// <summary>
        /// Converts all HTML non-breaking spaces to normal space
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string</returns>
        public static string NbspToSpace(string input)
        {
            return input.Replace("&nbsp;", " ");
        }

        /// <summary>
        /// Converts new line(\n) and carriage return(\r) symbols to
        /// HTML line breaks.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string.</returns>
        public static string NewLineToBreak(string input)
        {
            Regex regEx = new Regex(@"[\n|\r]+");
            return regEx.Replace(input, "<br />");
        }

        /// <summary>
        /// Removes multiple spaces between words
        /// </summary>
        /// <param name="input">The string to trim.</param>
        /// <returns>A string.</returns>
        public static string RemoveMultipleSpaces(string input)
        {
            Regex regEx = new Regex(@"[\s]+");
            return regEx.Replace(input, " ");
        }

        /// <summary>
        /// Removes the new line (\n) and carriage return (\r) symbols.
        /// </summary>
        /// <param name="input">The string to search.</param>
        /// <returns>A string</returns>
        public static string RemoveNewLines(string input)
        {
            return RemoveNewLines(input, false);
        }

        /// <summary>
        /// Removes the new line (\n) and carriage return 
        /// (\r) symbols.
        /// </summary>
        /// <param name="input">The string to search.</param>
        /// <param name="addSpace">If true, adds a space 
        /// (" ") for each newline and carriage
        /// return found.</param>
        /// <returns>A string</returns>
        public static string RemoveNewLines(string input, bool addSpace)
        {
            string replace = (addSpace) ? " " : string.Empty;
            string pattern = @"[\r|\n]";
            Regex regEx = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regEx.Replace(input, replace);
        }

        /// <summary>
        /// Converts all spaces to HTML non-breaking spaces
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string</returns>
        public static string SpaceToNbsp(string input)
        {
            return input.Replace(" ", "&nbsp;");
        }

        public static string[] Split(string text, string delimiter, bool trim)
        {
            string[] strArray = text.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (trim)
            {
                for (int i = 0; i < strArray.Length; i++)
                {
                    strArray[i] = strArray[i].Trim();
                }
            }
            return strArray;
        }

        /// <summary>
        /// Wraps the passed string at the 
        /// at the next whitespace on or after the 
        /// total charCount has been reached
        /// for that line.  Uses the environment new line
        /// symbol for the break text.
        /// </summary>
        /// <param name="input">The string to wrap.</param>
        /// <param name="charCount">The number of characters 
        /// per line.</param>
        /// <returns>A string.</returns>
        public static string WordWrap(string input, int charCount)
        {
            return StringUtil.WordWrap(input, charCount, false, Environment.NewLine);
        }

        /// <summary>
        /// Wraps the passed string at the total 
        /// number of characters (if cuttOff is true)
        /// or at the next whitespace (if cutOff is false).
        /// Uses the environment new line
        /// symbol for the break text.
        /// </summary>
        /// <param name="input">The string to wrap.</param>
        /// <param name="charCount">The number of characters 
        /// per line.</param>
        /// <param name="cutOff">If true, will break in 
        /// the middle of a word.</param>
        /// <returns>A string.</returns>
        public static string WordWrap(string input, int charCount, bool cutOff)
        {
            return StringUtil.WordWrap(input, charCount, cutOff, Environment.NewLine);
        }

        // <summary>
        /// Wraps the passed string at the total number 
        /// of characters (if cuttOff is true)
        /// or at the next whitespace (if cutOff is false).
        /// Uses the passed breakText
        /// for lineBreaks.
        /// </summary>
        /// <param name="input">The string to wrap.</param>
        /// <param name="charCount">The number of 
        /// characters per line.</param>
        /// <param name="cutOff">If true, will break in 
        /// the middle of a word.</param>
        /// <param name="breakText">The line break text to use.</param>
        /// <returns>A string.</returns>
        public static string WordWrap(string input, int charCount, bool cutOff, string breakText)
        {
            StringBuilder sb = new StringBuilder(input.Length + 100);
             int counter = 0;

             if (cutOff)
             {
            while (counter < input.Length)
            {
               if (input.Length > counter + charCount)
               {
                  sb.Append(input.Substring(counter, charCount));
                  sb.Append(breakText);
               }
               else
               {
                  sb.Append(input.Substring(counter));
               }
               counter += charCount;
            }
             }
             else
             {
            string[] strings = input.Split(' ');
            for (int i = 0; i < strings.Length; i++)
            {
               // added one to represent the space.
               counter += strings[i].Length + 1;
               if (i != 0 && counter > charCount)
               {
                  sb.Append(breakText);
                  counter = 0;
               }

               sb.Append(strings[i] + ' ');
            }
             }
             // to get rid of the extra space at the end.
             return sb.ToString().TrimEnd();
        }

        #endregion Methods
    }
}