using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSocialBot.sbot_util
{
    /// <summary>
    /// Utility methods for social bots.
    /// </summary>
    static class Util
    {
        static Random r = new Random();
        /// <summary>
        /// Picks a string at random from a list, all choices being equally likely.
        /// </summary>
        public static string RandomPick(string[] strings)
        {
            int n = strings.Length;
            int rn = r.Next(n);
            return strings[rn];
        }
        /// <summary>
        /// Picks a string at random from a list. The chance of getting each string is determined by its Chance value.
        /// <para>This method is crucial to the operation of the default bot.</para>
        /// <para>It is important that the Chance values of the PStrings sum to exactly 100. Please ensure that rounding errors are correctly compensated for when using RandomPick.
        /// Total values less than 100 give a chance of returning the empty string, while values greater than 100 will reduce the chance of seeing certain strings, potentially to zero.</para>
        /// </summary>
        /// <param name="pstrings">An array of probabilistic strings.</param>
        /// <returns>A string at random from the array. The chance of getting each string is determined by its Chance value.</returns>
        public static string RandomPick(PString[] pstrings)
        {
            int x = pstrings.Length;
            int rn = r.Next(100);
            int[] cumul = new int[x];
            cumul[0] = pstrings[0].Chance;
            for (int i = 1; i < x; i++)
                cumul[i] = cumul[i-1] + pstrings[i].Chance;
            for (int i = 0; i < x; i++)
            {
                if (rn < cumul[i])
                    return pstrings[i].String;
            }
            return String.Empty;
        }

        /// <summary>
        /// Returns true with the given percentage chance.
        /// </summary>
        public static Boolean RandomBoolean(int percentChance)
        {
            int rn = r.Next(100);
            return (rn < percentChance);
        }

        /// <summary>
        /// Generates a random ID for a robot consisting of capital letters and numerals.
        /// </summary>
        /// <param name="length">The length of the ID.</param>
        public static string RandomID(int length)
        {
            string s = String.Empty;
            if (length == 0) { return s; }
            char randomLetter = (char)r.Next((int)'A', (int)'Z');
            char randomNumber;
            s += randomLetter;
            for (int i = 1; i < length; i++)
            {
                randomLetter = (char)r.Next((int)'A', (int)'Z');
                randomNumber = (r.Next(10)).ToString()[0];
                if (r.Next(36) > 10)
                    s += randomLetter;
                else
                    s += randomNumber;
            }
            return s;
        }
        /// <summary>
        /// Generates a random ID for a robot consisting of capital letters and numerals. The length of the ID is randomly chosen from 6 to 10.
        /// </summary>
        public static string RandomID()
        {
            return RandomID(r.Next(6, 11));
        }

        /// <summary>
        /// Has a percentage chance to append one string to another. If it fails, it will return the original string.
        /// </summary>
        public static string AppendChance(string original, string append, uint percentChance)
        {
            int rn = r.Next(100);
            if (rn < percentChance)
                return String.Concat(original, append);
            else
                return original;
        }

        /// <summary>
        /// Has a percentage chance to precede one string with another. If it fails, it will return the original string.
        /// </summary>
        public static string PrecedeChance(string original, string precede, uint percentChance)
        {
            int rn = r.Next(100);
            if (rn < percentChance)
                return String.Concat(precede, original);
            else
                return original;
        }

        /// <summary>
        /// Checks if the given message contains any of the given search terms, and returns its index if so; returns -1 otherwise.
        /// </summary>
        public static int IndexOfAny(string message, string[] searchTerms)
        {
            for (int i = 0; i < searchTerms.Length; i++)
            {
                if (message.Contains(searchTerms[i]))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Refines the length of a string array, deleting all elements after and including the first null/empty element.
        /// </summary>
        public static string[] RefineLength(string[] stringArray)
        {
            int i = 0;
            for (i = 0; i < stringArray.Length; i++)
            {
                if (String.IsNullOrEmpty(stringArray[i]))
                    break;
            }
            string[] refinedArray = new string[i];
            for (int j = 0; j < i; j++)
                refinedArray[j] = stringArray[j];
            return refinedArray;
        }

        /// <summary>
        /// Quickly converts the character at the specified index of a given string to uppercase.
        /// </summary>
        public static string ToUppercase(string original, int index)
        {
            string s = original; int i = index;
            char c = char.ToUpper(s[i]);
            s = s.Remove(i, 1);
            s = s.Insert(i, c.ToString());
            return s;
        }
        /// <summary>
        /// Quickly converts the character at the specified index of a given string to lowercase.
        /// </summary>
        public static string ToLowercase(string original, int index)
        {
            string s = original; int i = index;
            char c = char.ToLower(s[i]);
            s = s.Remove(i, 1);
            s = s.Insert(i, c.ToString());
            return s;
        }

        private static string EndSentence = ".!?\n";
        /// <summary>
        /// Improves the grammatical correctness of a given string by capitalizing where appropriate. Converts " i " to " I ".
        /// Capitalize(Capitalize(s)) must be equivalent to Capitalize(s).
        /// </summary>
        public static string Capitalize(string s)
        {
            if (s == string.Empty)
                return s;
            if (char.IsLower(s[0]))
                s = ToUppercase(s, 0);
            for (int i = 2; i < s.Length; i++)
            {
                if (EndSentence.Contains(s[i-2]) && char.IsLower(s[i]))
                    s = ToUppercase(s, i);
                if (s[i-2] == ' ' && s[i-1] == 'i' && (s[i] == ' ' || s[i] == '\''))
                    s = ToUppercase(s, i-1);
            }
            return s;
        }
        /// <summary>
        /// Capitalizes all words in a given string.
        /// </summary>
        public static string CapitalizeWords(string s)
        {
            if (char.IsLower(s[0]))
                s = ToUppercase(s, 0);
            for (int i = 2; i < s.Length; i++)
            {
                if (s[i - 1] == ' ' && char.IsLower(s[i]))
                    s = ToUppercase(s, i);
            }
            return s;
        }

        /// <summary>
        /// Combines the elements of two arrays into a single array.
        /// </summary>
        /// <typeparam name="T">The type of element in each array.</typeparam>
        public static T[] Combine<T>(this T[] array1, T[] array2)
        {
            int oldLen = array1.Length;
            Array.Resize<T>(ref array1, array1.Length + array2.Length);
            Array.Copy(array2, 0, array1, oldLen, array2.Length);
            return array1;
        }
        /// <summary>
        /// Adds a single element to the end of an existing array.
        /// </summary>
        /// <typeparam name="T">The type of element in the array.</typeparam>
        public static T[] Combine<T>(this T[] array, T newElement)
        {
            int oldLen = array.Length;
            Array.Resize<T>(ref array, array.Length + 1);
            array[oldLen] = newElement;
            return array;
        }
        /// <summary>
        /// Splits a string into an array of substrings, delimited by any of the specified characters. Non-whitespace separators are included as substrings if includePunctuation is set to true.
        /// </summary>
        /// <param name="message">The string to be split.</param>
        /// <param name="separators">The separators to use when splitting.</param>
        /// <param name="includePunctuation">If this is set to true, non-whitespace separators will be included as substrings.</param>
        /// <returns></returns>
        public static string[] Split(string message, char[] separators, bool includePunctuation)
        {
            string[] words = new string[256];
            int wi = 0;
            for (int i = 0; i < message.Length; i++)
            {
                if (!separators.Contains(message[i]))
                    words[wi] += message[i];
                else if (message[i] == ' ') 
                {
                    if (string.IsNullOrEmpty(words[wi]))
                        continue;
                    else { wi++; continue; }
                }
                else if (message[i] != ' ')
                {
                    if (!string.IsNullOrEmpty(words[wi])) { wi++; } // wi will not change if a punctuation mark follows another punctuation mark and whitespaces.
                    if (includePunctuation)
                    {
                        words[wi] += message[i];
                        wi++;
                    }
                }
            }
            words = RefineLength(words);
            for (int i = 0; i < words.Length; i++)
                words[i] = words[i].Trim();
            return words;
        }
    }
}
