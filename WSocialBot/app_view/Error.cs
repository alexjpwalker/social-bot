using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSocialBot.app_view
{
    /// <summary>
    /// Displays a variety of error messages to the user.
    /// </summary>
    sealed class Error
    {
        /// <summary>
        /// Displays the specified error message in a suitable error message box.
        /// </summary>
        public static void Display(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); 
        }
        /// <summary>The specified data file was not found</summary>
        public static void FileNotFound(string filename)
        {
            Display("Could not find data file: " + filename);
        }
        /// <summary>The specified data file is formatted incorrectly</summary>
        public static void BadFormat(string filename)
        {
            Display("Bad format in data file: " + filename);
        }
        /// <summary>The specified data file is formatted incorrectly - the error is as follows</summary>
        public static void BadFormat(string filename, string details)
        {
            Display("Bad format in data file: " + filename + "\nError details: " + details);
        }
        /// <summary>The given variable is undefined in the specified data file</summary>
        public static void Undefined(string filename, string variableName)
        {
            Display("Variable " + variableName + " is undefined in data file: " + filename);
        }
        /// <summary>The given variable is declared in the specified data file, but its value is not suitably assigned</summary>
        public static void Unassigned(string filename, string variableName)
        {
            Display("Variable " + variableName + " is declared in data file \"" + filename + "\", but has no appropriate value assigned to it!");
        }
        /// <summary>Could not find any keywords associated with the given category in the specified data file</summary>
        public static void NoKeywords(string filename, string categoryName)
        {
            Display("The category {" + categoryName + "} has no keywords associated with it in data file: " + filename);
        }
        /// <summary>
        /// Notifies the user that a fatal error has occurred while loading, and the program cannot continue. Does not include code to terminate the application!
        /// </summary>
        public static void Fatal()
        {
            Display("A fatal error occurred while loading WSocialBot. The program will now terminate.");
        }
    }
}
