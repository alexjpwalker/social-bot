using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WSocialBot.app_view;

namespace WSocialBot.sbot_data
{
    sealed class Data
    {
        // The list of filenames recognised by this application.
        public const string keywordsPath = "keywords_default.txt";
        public const string rTablePath = "rtable_default.txt";
        public const string botSettingsPath = "settings_sbot.txt";

        /// <summary>
        /// Attempts to load a text file from the current directory, \data directory or project path. Displays an error if the file is not found.
        /// </summary>
        /// <param name="filename">The name of the file to be loaded.</param>
        /// <returns>The full path of the file, or an empty string if the file is not found.</returns>
        public static string Load(string filename)
        {
            string path = String.Concat(Environment.CurrentDirectory, "\\", filename);
            if (!File.Exists(path))
                path = String.Concat(Environment.CurrentDirectory, "\\data\\", filename);
            if (!File.Exists(path))
                path = String.Concat("C:\\Users\\User\\Documents\\Visual Studio 2010\\Projects\\WSocialBot\\WSocialBot\\", filename);
            if (!File.Exists(path))
            {
                Error.FileNotFound(filename);
                path = String.Empty;
            }
            return path;
        }
    }
}
