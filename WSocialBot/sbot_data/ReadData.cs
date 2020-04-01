using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSocialBot.sbot_util;
using WSocialBot.app_view;

namespace WSocialBot.sbot_data
{
    /// <summary>
    /// Contains methods for scanning and using the contents of data files.
    /// </summary>
    sealed class ReadData
    {
        /// <summary>
        /// Scan a string for keywords of a particular type. The type must be declared in braces. Keywords must follow the declaration. A semicolon goes at the end of each keyword.
        /// Displays a warning if no keywords were found for the given type.
        /// </summary>
        /// <param name="file">The string to be scanned.</param>
        /// <param name="type">The type of keyword to search for.</param>
        public static string[] Keywords(string file, string type, string filename)
        {
            int mode = 0;
            int n = 0;
            int wi = 0;
            string word = String.Empty;
            string[] keywords = new string[8];
            for (int i = 0; i < 8; i++)
                keywords[i] = String.Empty;
            while (n < file.Length)
            {
                switch (mode)
                {
                    case 0: // Search for a type declaration
                        if (file[n] != '{') { n++; continue; }
                        else { n++; mode = 1; word = ""; continue; }
                    case 1: // Read type declaration
                        if (file[n] != '}') { word += file[n]; n++; continue; }
                        else
                        {
                            if (word == type) { n++; mode = 2; continue; }
                            else { n++; mode = 0; continue; }
                        }
                    case 2: // Search for a keyword
                        if (file[n] == '{') 
                        {
                            if (keywords[0] == String.Empty)
                                Error.NoKeywords(filename, type);
                            return Util.RefineLength(keywords); 
                        }
                        else if (!char.IsLetterOrDigit(file[n])) { n++; continue; }
                        else { word = ""; word += file[n]; n++; mode = 3; continue; }
                    case 3: // Read keyword
                        if (file[n] == ';')
                        {
                            n++; keywords[wi] = word; wi++;
                            if (wi >= keywords.Length)
                            {
                                string[] temp = new string[keywords.Length * 2];
                                for (int i = 0; i < keywords.Length; i++)
                                    temp[i] = keywords[i];
                                keywords = temp;
                            }
                            word = ""; mode = 2; continue;
                        }
                        else { word += file[n]; n++; continue; }
                }
            }
            if (keywords[0] == String.Empty)
                Error.NoKeywords(filename, type);
            return Util.RefineLength(keywords);
        }
        /// <summary>
        /// Scan a string for keywords of a particular type. The type must be declared in braces. Keywords must follow the declaration. A semicolon goes at the end of each keyword.
        /// Displays a warning if no keywords were found for the given type.
        /// </summary>
        /// <param name="file">The string to be scanned.</param>
        /// <param name="type">The type of keyword to search for.</param>
        public static string[] Keywords(string file, string type)
        {
            return Keywords(file, type, Data.keywordsPath);
        }
        /// <summary>
        /// Scan a string for boolean settings. The setting name must not contain spaces or = signs and is case sensitive. It must start with a letter or digit. 
        /// Excluding spaces, it must be followed by an = sign, and then 0 or 1.
        /// Displays an appropriate error message and returns false by default if the requested setting is not found.
        /// </summary>
        /// <param name="file">The string to be scanned.</param>
        /// <param name="setting">The name of the setting to find.</param>
        public static bool BooleanSetting(string file, string setting, string filename)
        {
            int mode = 0;
            int n = 0;
            string word = String.Empty;
            while (n < file.Length)
            {
                switch (mode)
                {
                    case 0: // Search for a setting name
                        if (file[n] != setting[0]) { n++; continue; }
                        else { n++; mode = 1; word = setting[0].ToString(); continue; }
                    case 1: // Read setting name
                        if (file[n] != ' ' && file[n] != '=') { word += file[n]; n++; continue; }
                        else
                        {
                            if (word == setting)
                            {
                                if (file[n] == ' ') { n++; mode = 2; continue; }
                                else { n++; mode = 3; continue; }
                            }
                            else { n++; mode = 0; continue; }
                        }
                    case 2: // Search for an = sign
                        if (file[n] == '=') { n++; mode = 3; continue; }
                        else if (char.IsLetterOrDigit(file[n])) { mode = 0; continue; }
                        else { n++; continue; }
                    case 3: // Search for the setting value
                        if (file[n] == '0') { return false; }
                        else if (file[n] == '1') { return true; }
                        else if (char.IsLetterOrDigit(file[n]))
                        {
                            Error.Unassigned(filename, setting);
                            return false;
                        }
                        else { n++; continue; }
                }
            }
            Error.Undefined(filename, setting); // Requested variable was not found
            return false;
        }
        /// <summary>
        /// Scan a string for boolean settings. The setting name must not contain spaces or = signs and is case sensitive. It must start with a letter or digit. 
        /// Excluding spaces, it must be followed by an = sign, and then 0 or 1.
        /// Displays an appropriate error message and returns false by default if the requested setting is not found.
        /// </summary>
        /// <param name="file">The string to be scanned.</param>
        /// <param name="setting">The name of the setting to find.</param>
        public static bool BooleanSetting(string file, string setting)
        {
            return BooleanSetting(file, setting, Data.botSettingsPath);
        }
    }
}
