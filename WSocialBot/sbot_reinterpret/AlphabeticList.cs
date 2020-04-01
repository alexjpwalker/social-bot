using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WSocialBot.app_view;
using WSocialBot.sbot_data;

namespace WSocialBot.sbot_reinterpret
{
    /// <summary>
    /// A ReinterpretTable that accepts original words beginning with lowercase letters. Original words are sorted according to their initial letter for efficiency.
    /// Each set of original words maps to precisely one result phrase, which may consist of multiple words.
    /// </summary>
    class AlphabeticList : ReinterpretTable
    {
        StringMap[] mapA; StringMap[] mapB; StringMap[] mapC; StringMap[] mapD;
        StringMap[] mapE; StringMap[] mapF; StringMap[] mapG; StringMap[] mapH;
        StringMap[] mapI; StringMap[] mapJ; StringMap[] mapK; StringMap[] mapL;
        StringMap[] mapM; StringMap[] mapN; StringMap[] mapO; StringMap[] mapP;
        StringMap[] mapQ; StringMap[] mapR; StringMap[] mapS; StringMap[] mapT;
        StringMap[] mapU; StringMap[] mapV; StringMap[] mapW; StringMap[] mapX;
        StringMap[] mapY; StringMap[] mapZ;

        public int[] MapLength { get; private set; }

        public AlphabeticList()
        {
            int x = 8;
            mapA = new StringMap[x]; mapB = new StringMap[x]; mapC = new StringMap[x]; mapD = new StringMap[x];
            mapE = new StringMap[x]; mapF = new StringMap[x]; mapG = new StringMap[x]; mapH = new StringMap[x];
            mapI = new StringMap[x]; mapJ = new StringMap[x]; mapK = new StringMap[x]; mapL = new StringMap[x];
            mapM = new StringMap[x]; mapN = new StringMap[x]; mapO = new StringMap[x]; mapP = new StringMap[x];
            mapQ = new StringMap[x]; mapR = new StringMap[x]; mapS = new StringMap[x]; mapT = new StringMap[x];
            mapU = new StringMap[x]; mapV = new StringMap[x]; mapW = new StringMap[x]; mapX = new StringMap[x];
            mapY = new StringMap[x]; mapZ = new StringMap[x];

            MapLength = new int[26];
            for (int i = 0; i < 26; i++)
                MapLength[i] = 0;
        }

        public StringMap[] GetMap(char initialLetter)
        {
            switch (initialLetter)
            {
                case 'a': return mapA; case 'b': return mapB; case 'c': return mapC; case 'd': return mapD;
                case 'e': return mapE; case 'f': return mapF; case 'g': return mapG; case 'h': return mapH; 
                case 'i': return mapI; case 'j': return mapJ; case 'k': return mapK; case 'l': return mapL;
                case 'm': return mapM; case 'n': return mapN; case 'o': return mapO; case 'p': return mapP;
                case 'q': return mapQ; case 'r': return mapR; case 's': return mapS; case 't': return mapT;
                case 'u': return mapU; case 'v': return mapV; case 'w': return mapW; case 'x': return mapX;
                case 'y': return mapY; case 'z': return mapZ;
                default: return null;
            }
        }

        /// <summary>
        /// Populates the reinterpret table by scanning a file. The file must consist of lists of original words enclosed in braces {...}, 
        /// each list being followed by a result phrase enclosed in square brackets [...]. Each original word must start with a lowercase letter.
        /// </summary>
        public static AlphabeticList FromFile(string filename)
        {
            AlphabeticList aList = new AlphabeticList();
            string path = Data.Load(filename);
            if (string.IsNullOrEmpty(path))
                return aList;
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string file = sr.ReadToEnd();
            sr.Close();
            int mode = 0; 
            int n = 0;
            char tableRow = (char)0;
            char rowLetter = (char)0;
            for (int i = 0; i < 26; i++)
                aList.MapLength[i] = 0;
            int len = 0;
            string word = String.Empty;
            while (n < file.Length)
            {
                switch (mode)
                {
                    case 0: // Searching for a list of original words
                        if (file[n] != '{') { n++; continue; }
                        else { n++; mode = 1; word = ""; continue; }
                    case 1: // Allocating a string map
                        if (file[n] < 'a' || file[n] > 'z')
                        {
                            Error.BadFormat(filename, "Found an original word that does not start with a letter!");
                            return aList;
                        }
                        else // Found an original word, allocate map and start reading
                        { 
                            mode = 2; 
                            tableRow = (char)(file[n] - 'a');
                            rowLetter = file[n];
                            len = aList.MapLength[tableRow];
                            aList.GetMap(rowLetter)[len] = new StringMap();
                            continue; 
                        }
                    case 2: // Reading an original word
                        if (file[n] != ' ' && file[n] != '}') { word += file[n]; n++; continue; }
                        else // Copy the original word to the string map
                        {
                            aList.GetMap(rowLetter)[len].Add(word);
                            word = "";
                            if (file[n] == ' ')
                                mode = 3;
                            else
                                mode = 4; // List of original words is finished, search for result word
                            n++;
                            continue;
                        }
                    case 3: // Searching for the next original word
                        if (file[n] == ' ') { n++; continue; }
                        else if (file[n] == '}') { mode = 4; n++; continue; }
                        else { mode = 2; continue; }
                    case 4: // Searching for result phrase
                        if (file[n] == '{')
                        {
                            Error.BadFormat(filename, "A list of original words has no result phrase associated with it!");
                            return aList;
                        }
                        else if (file[n] != '[') { n++; continue; }
                        else { n++; mode = 5; continue; }
                    case 5: // Reading a result phrase
                        if (file[n] != ']') { word += file[n]; n++; continue; }
                        else // Result phrase is finished, mark the string map as complete and search for a new list of original words
                        {
                            aList.GetMap(rowLetter)[len].Result = word;
                            word = "";
                            aList.MapLength[tableRow]++;
                            n++;
                            mode = 0;
                            continue;
                        }
                }
            }
            return aList;
        }

        public string GetResult(string original)
        {
            char init = original[0];
            if (init < 'a' || init > 'z')
                return original;
            char tableRow = (char)(init - 'a');
            StringMap[] alphabeticMap = GetMap(init);
            for (int i = 0; i < MapLength[tableRow]; i++)
            {
                StringMap currentMap = alphabeticMap[i];
                if (currentMap.Originals.Contains(original))
                    return currentMap.Result;
            }
            return original;
        }
    }
}
