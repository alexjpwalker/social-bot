using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WSocialBot.sbot_data;
using WSocialBot.sbot_write;
using WSocialBot.sbot_reinterpret;
using WSocialBot.sbot_util;
using WSocialBot.app_view;

namespace WSocialBot.sbot_main
{
    /// <summary>
    /// The default implementation of SocialBot and the one used by the main application.
    /// </summary>
    class DefaultBot : SocialBot
    {
        // Predefined constants
        const int NO_MOOD = 249;
        const int PRIVATE = -4;

        // Reinterpret table for this bot
        private ReinterpretTable rTable;

        // List of phrases to be interpreted in a given way (imported from keywords_default)
        private string[] ReadGreetings;     // Greetings
        private string[] ReadGivenName;     // Acquiring user's name
        private string[] ReadGivenPlace;    // Acquiring user's location
        private string[] ReadPosition;      // Prepositions expressing location
        private string[] ReadArticles;      // Articles
        private string[] ReadOther;         // 3rd-person pronouns
        private string[] ReadPositive;      // Positive emotions
        private string[] ReadNegative;      // Negative emotions
        private string[] ReadNeutral;       // Indifferent emotions
        private string[] ReadEmphasis;      // Words of emphasis
        private string[] ReadAssertion;     // Words of assertion
        private string[] ReadPrivate;       // Saying a detail is private
        private string[] ReadThanks;        // Expressing gratitude

        // List of phrases to be interpreted in a given way if the bot is actively seeking to do so (extensions of Read phrases)
        private string[] ForceReadGivenName;    // Acquiring user's name
        private string[] ForceReadGivenPlace;   // Acquiring user's location

        // List of phrases to be interpreted in a given way (predefined)
        private static string ReadMe = "i";                             // Refers to the user
        private static string ReadYou = "you";                          // Refers to the bot
        private static string ReadIs = "is";                            // Verb of being
        private static string[] ReadNegate = { "no", "not" };           // Negates an expression
        private static string EndMarks = ";:.,!?~\n";                   // Ends a phrase
        private static string Separators = EndMarks + " ()[]{}-\"&/";   // Separates words

        // List of parameters for the bot to "learn" over its lifetime
        public string userName { get; private set; }
        public string userPlace { get; private set; }
        public string placePosition { get; private set; }

        // Message categories to consider replying to; multiple categories may be used to compute the reply
        public enum ReplyType { General, Statement, Question, Opinion };
        public enum ReplyContent { NA, None, Self, Bot, Other, Greeting, Name, Mood, Person, Place, Object };
        public sealed class ContentMap : Dictionary<string, int> { };
        public ContentMap contentMap { get; private set; }
        public ContentMap askContentMap { get; private set; }
        public int[] replyContent; private int contentCount;
        private ReplyContent mainContent;
        private ReplyContent dominantCategory;
        public void CreateContentMap()
        {
            contentMap = new ContentMap(); askContentMap = new ContentMap();
            contentMap.Add("N/A", (int)ReplyContent.NA);
            contentMap.Add("None", (int)ReplyContent.None); askContentMap.Add("None", (int)ReplyContent.None);
            contentMap.Add("Self", (int)ReplyContent.Self);
            contentMap.Add("Bot", (int)ReplyContent.Bot); askContentMap.Add("Bot", (int)ReplyContent.Bot);
            contentMap.Add("Other", (int)ReplyContent.Other);
            contentMap.Add("Greeting", (int)ReplyContent.Greeting);
            contentMap.Add("Name", (int)ReplyContent.Name); askContentMap.Add("Name", (int)ReplyContent.Name);
            contentMap.Add("Mood", (int)ReplyContent.Mood); askContentMap.Add("Mood", (int)ReplyContent.Mood);
            contentMap.Add("Person", (int)ReplyContent.Person);
            contentMap.Add("Place", (int)ReplyContent.Place); askContentMap.Add("Place", (int)ReplyContent.Place);
            contentMap.Add("Object", (int)ReplyContent.Object); askContentMap.Add("Object", (int)ReplyContent.Object);
            contentCount = contentMap.Count;
            replyContent = new int[contentCount];
        }

        // Data about the current state of the bot
        public enum RequestState { None, Name, Mood, Place };
        public RequestState requestState { get; private set; }
        public ReplyType lastReply { get; private set; }
        public bool lastGreet, usedGreet, genericReply;
        public Boolean knowsName { get; private set; }
        public Boolean knowsPlace { get; private set; }
        public Boolean knowsMood { get; private set; }
        private int openReply;
        
        // Variables controlling operation settings
        public bool block_e1, block_e2;

        /// <summary>
        /// Creates a new DefaultBot with the given name.
        /// </summary>
        /// <param name="name">The bot's name.</param>
        public DefaultBot(string name)
        {
            this.LoadSuccess = false;
            CreateContentMap();
            this.Name = name;
            this.rTable = AlphabeticList.FromFile(Data.rTablePath);
            if (ImportReadStrings(Data.keywordsPath) != 0) { return; }
            ImportSettings(Data.botSettingsPath);
            this.knowsName = false;
            this.userName = "";
            this.requestState = RequestState.None;
            this.lastReply = ReplyType.General;
            this.usedGreet = false;
            this.openReply = 0;
            this.LoadSuccess = true; // If this line is not reached, the program will terminate with a fatal error.
        }

        /// <summary>
        /// Import the bot's read strings from a file.
        /// </summary>
        /// <param name="filename">The file to be opened.</param>
        /// <returns>0 if successful, -1 otherwise.</returns>
        private int ImportReadStrings(string filename)
        {
            string path = Data.Load(filename);
            if (string.IsNullOrEmpty(path))
                return -1;
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string keywords = sr.ReadToEnd();
            sr.Close();
            string[] AddedStrings;

            ReadGreetings = ReadData.Keywords(keywords, "Greetings");

            ReadGivenName = ReadData.Keywords(keywords, "Given Name");
            AddedStrings = ReadData.Keywords(keywords, "Force Given Name");
            ForceReadGivenName = ReadGivenName.Combine<string>(AddedStrings);

            ReadGivenPlace = ReadData.Keywords(keywords, "Given Place");
            AddedStrings = ReadData.Keywords(keywords, "Force Given Place");
            ForceReadGivenPlace = ReadGivenPlace.Combine<string>(AddedStrings);

            ReadPosition = ReadData.Keywords(keywords, "Position");
            ReadArticles = ReadData.Keywords(keywords, "Article");
            ReadOther = ReadData.Keywords(keywords, "Other");
            ReadPositive = ReadData.Keywords(keywords, "Positive");
            ReadNegative = ReadData.Keywords(keywords, "Negative");
            ReadNeutral = ReadData.Keywords(keywords, "Neutral");
            ReadEmphasis = ReadData.Keywords(keywords, "Emphasis");
            ReadAssertion = ReadData.Keywords(keywords, "Assertion");
            ReadPrivate = ReadData.Keywords(keywords, "Private");
            ReadThanks = ReadData.Keywords(keywords, "Thanks");
            return 0;
        }
        /// <summary>
        /// Import the bot's operation settings from a file.
        /// </summary>
        /// <param name="filename">The file to be opened.</param>
        /// <returns>0 if successful, -1 otherwise.</returns>
        private int ImportSettings(string filename)
        {
            string path = Data.Load(filename);
            if (string.IsNullOrEmpty(path))
                return -1;
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string settings = sr.ReadToEnd();
            sr.Close();
            opensChat = ReadData.BooleanSetting(settings, "BOT_OPENS_CHAT");
            if (ReadData.BooleanSetting(settings, "BLOCK_ALL"))
            {
                block_e1 = true; block_e2 = true;
            }
            else
            {
                block_e1 = ReadData.BooleanSetting(settings, "BLOCK_E1");
                block_e2 = ReadData.BooleanSetting(settings, "BLOCK_E2");
            }
            return 0;
        }

        /// <summary>
        /// Gets the bot's reply to a given message.
        /// </summary>
        /// <param name="s">The incoming message.</param>
        public override string GetReply(string msg)
        {
            string reply = String.Empty;
            dominantCategory = ReplyContent.None;
            RequestState prevState = this.requestState;
            RequestState storedState = RequestState.None;
            bool blockCore2 = false;
            // Split the message into its component clauses.
            string[] s = Util.Split(msg, EndMarks.ToCharArray(), false);
            lastGreet = usedGreet; // lastGreet is used to determine if a greeting in the user's message was a statement or a response.
            usedGreet = false;
            genericReply = false;
            // First, compute a reply to the entire message for consistency. Then look at each individual clause.
            for (int i = -1; i < s.Length; i++)
            {
                // Compute a reply to the current clause.
                string si;
                if (i == -1) 
                { 
                    si = GetReplyCore1(msg); 
                    mainContent = dominantCategory;
                    genericReply = true; // The bot will only reply generically if the complete message is indifferent.
                }
                else { si = GetReplyCore1(s[i], mainContent); }
                reply += si;
                if (!String.IsNullOrEmpty(reply))
                {
                    if (!Char.IsWhiteSpace(reply, reply.Length - 1))
                        reply += ' ';
                }
                // If a request has been made, store the request in memory, but do not use it when computing replies to remaining clauses.
                if (requestState != prevState && requestState != RequestState.None)
                {
                    storedState = requestState;
                    requestState = RequestState.None;
                    blockCore2 = true;
                }
            }
            reply = reply.Trim();
            if (openReply > 0 && !blockCore2)
            {
                bool runCore2 = Util.RandomBoolean(openReply);
                if (runCore2)
                    reply += ' ' + GetReplyCore2();
            }
            reply = reply.Trim();
            reply = Util.Capitalize(reply);
            reply = method1(reply);
            return reply;
        }
        /// <summary>Used when the bot has to initiate a conversation.</summary>
        public override string OpenChat()
        {
            return Util.Capitalize(Greet(true, false, 60, 75));
        }
        /// <summary>
        /// Core logic for computing replies to messages. This method scans the user's message and attempts to construct an appropriate response. If the dominant category of the user's message
        /// is the same as the specified ReplyContent, a reply will not be generated for this message. 
        /// </summary>
        /// <param name="s">The message to reply to.</param>
        /// <returns>A suitable reply, mostly in lowercase except for special cases.</returns>
        private string GetReplyCore1(string s, ReplyContent c)
        {
            dominantCategory = ReplyContent.None;
            // Using Reinterpret, compile the following structures: words, the list of individual words and separators; rwords, the same list but case-sensitive; message, the complete reinterpreted message.
            string[] words0 = Util.Split(s, Separators.ToCharArray(), true);
            string[] rwords = new string[words0.Length * 2];
            string[] words = new string[words0.Length * 2];
            int wi = 0;
            for (int i = 0; i < words0.Length; i++)
            {
                string w = Reinterpret(words0[i]);
                string rw = SensitiveReinterpret(words0[i]); // rwords[i] is defined as words[i] with the capitalization the user used when typing the word
                if (w.Contains(' '))
                {
                    string[] split = w.Split(' '); string[] rsplit = rw.Split(' ');
                    words[wi] = split[0]; rwords[wi] = rsplit[0]; wi++;
                    words[wi] = split[1]; rwords[wi] = rsplit[1]; wi++;
                }
                else { words[wi] = w; rwords[wi] = rw; wi++; }
            }
            rwords = Util.RefineLength(rwords);
            words = Util.RefineLength(words);
            string[] withspace = (string[])words.Clone();
            for (int i = 0; i < words.Length - 1; i++)
            {
                if (!EndMarks.Contains(words[i+1]))
                    withspace[i] += ' ';
            }
            string message = String.Concat(withspace);
            // Scan the user message for responses to the bot's most recent request, if there is one. If it returns a response, it must assign dominantCategory accordingly or it will default to None.
            switch (requestState)
            {
                case RequestState.Name:
                    if (c == ReplyContent.Name) { break; }
                    int r = SearchName(words, message);
                    if (r == 1) { openReply = 100; dominantCategory = ReplyContent.Name; return Greet(true, true, 0, 0); }
                    if (r == PRIVATE) { openReply = 100; return GenericReply(); }
                    break;
                case RequestState.Mood:
                    if (c == ReplyContent.Mood) { break; }
                    int moodResponse = SearchMood(words, message);
                    if (moodResponse != NO_MOOD) { openReply = 100; dominantCategory = ReplyContent.Mood; return ReplyMood(words, message); }
                    break;
                case RequestState.Place:
                    if (c == ReplyContent.Place) { break; }
                    int rplace = SearchPlace(words, rwords, message);
                    if (rplace == 1) { dominantCategory = ReplyContent.Place; return ReplyPlace(); }
                    if (rplace == PRIVATE) { openReply = 100; return GenericReply(); }
                    break;
            }
            // Scan the user message for recognised reply categories.
            for (int i = 0; i < contentCount; i++)
                replyContent[i] = 0;
            // Greeting
            if (Util.IndexOfAny(message, ReadGreetings) > -1 || message.IndexOf("good day") == 0) // "Good day" is recognised as a greeting only if it is at the start of a sentence.
                replyContent[(int)ReplyContent.Greeting] += 13;
            if (!knowsName)
                replyContent[(int)ReplyContent.Greeting] += 2;
            // Name
            if (Util.IndexOfAny(Refine(message), ReadGivenName) > -1)
            {
                if (SearchName(words, message) == 1)
                    replyContent[(int)ReplyContent.Name] += 37;
            }
            // Mood
            int moodMod = SearchMood(words, message);
            if (moodMod != NO_MOOD)
                replyContent[(int)ReplyContent.Mood] += (Math.Abs(moodMod) * 7) + 3;
            // Place
            if (Util.IndexOfAny(Refine(message), ReadGivenPlace) > -1)
            {
                if (SearchPlace(words, rwords, message) == 1)
                    replyContent[(int)ReplyContent.Place] += 31;
            }
            // Compute a dominant category set. Currently only supports one dominant category.
            int categoryIndex = 0;
            int maxValue = -1;
            replyContent[(int)ReplyContent.NA] = -2;
            for (int i = 0; i < contentCount; i++)
            {
                if (replyContent[i] > maxValue)
                {
                    maxValue = replyContent[i];
                    categoryIndex = i;
                }
            }
            dominantCategory = (ReplyContent)categoryIndex;
            // Check that we are not repeating a response to the main message. Note that if s is the main message itself, the reply content of c is "not applicable" (NA).
            if (dominantCategory == c) { return String.Empty; }
            // Compute a response based on the dominant category.
            openReply = 0;
            switch (dominantCategory)
            {
                case ReplyContent.Mood: // Express feelings on the user's mood.
                    { openReply = 100; return ReplyMood(words, message); }
                case ReplyContent.Name: // Greet the user by name, request the user's mood.
                    return Greet(true, true, 0, 100);
                case ReplyContent.Greeting: // Greet the user if it was an initial greeting; otherwise leave the reply open. Depends on lastGreet.
                    if (lastGreet) { openReply = 100; return String.Empty; }
                    else { return Greet(true, Util.RandomBoolean(60)); }
                case ReplyContent.Place:
                    return ReplyPlace();
                default:
                    openReply = 100; dominantCategory = ReplyContent.None; return GenericReply();
            }
        }
        private string GetReplyCore1(string s)
        {
            return GetReplyCore1(s, ReplyContent.NA);
        }
        /// <summary>
        /// Core logic for computing replies to messages. This method generates a random conversational topic. 
        /// <para>This method is occasionally run after Core1, but only if the openReply flag was set to true when running Core1.</para>
        /// </summary>
        /// <returns>A suitable reply in lowercase.</returns>
        private string GetReplyCore2()
        {
            if (!knowsName)
                return Greet(false, false, 100, 0);
            if (!knowsMood)
                return AskMood();
            if (!knowsPlace)
                return AskPlace();
            return "I don't know what else to say.";
        }

        /// <summary>
        /// Converts a word to lowercase, then reinterprets it using the current ReinterpretTable.
        /// </summary>
        /// <param name="s">The string to be converted.</param>
        /// <returns>The reinterpreted string.</returns>
        private string Reinterpret(string s)
        {
            string t = s;
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsUpper(s[i])) // Decapitalize all letters
                    t = Util.ToLowercase(t, i);
            }
            return rTable.GetResult(t);
        }
        /// <summary>
        /// Converts a word to lowercase, reinterprets it using the current ReinterpretTable, and then attempts to restore the upper/lowercase status of its letters to the original.
        /// </summary>
        /// <param name="s">The string to be converted.</param>
        private string SensitiveReinterpret(string s)
        {
            string t = Reinterpret(s);
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsUpper(s[i]) && i < t.Length)
                {
                    if (char.IsLower(t[i]))
                        t = Util.ToUppercase(t, i);
                }
            }
            return t;
        }

        /// <summary>Easter egg.</summary>
        private string method1(string s)
        {
            if (block_e1) { return s; }
            if (!knowsName) { return s; }
            if (userName.Contains("Hitler")) { return s + " HEIL HITLER!"; }
            if (userName.Contains("Kitler")) { return s + " HEIL KITLER!"; }
            return s;
        }
        /// <summary>Removes assertion words from a set of words, returning the resulting set of words.</summary>
        private string[] Refine(string[] words)
        {
            string[] newWords = words;
            for (int i = 0; i < newWords.Length; i++)
            {
                if (ReadAssertion.Contains(newWords[i]))
                {
                    for (int j = i; j < newWords.Length - 1; j++) { newWords[j] = newWords[j + 1]; }
                }
            }
            return Util.RefineLength(newWords);
        }
        /// <summary>Removes assertion words from a string, returning the resulting string.</summary>
        private string Refine(string message)
        {
            string newMessage = message;
            while (Util.IndexOfAny(newMessage, ReadAssertion) != -1)
            {
                int i = Util.IndexOfAny(newMessage, ReadAssertion);
                int j = newMessage.IndexOf(ReadAssertion[i]);
                newMessage = newMessage.Remove(j, ReadAssertion[i].Length);
                if (j > 0)
                    newMessage = newMessage.Remove(j - 1, 1);
            }
            return newMessage;
        }
        /// <summary>
        /// Greets the user, and possibly requests the user's name or mood depending on the values of its arguments. Sets usedGreet to true if applicable.
        /// Always makes a request if it does not use a greeting.
        /// </summary>
        /// <param name="greetUser">Whether to use a greeting, such as 'hi'.</param>
        /// <param name="useName">If set to true, the bot will always address the user by name if it is known.</param>
        /// <param name="askNameChance">The chance of asking for the user's name.</param>
        /// <param name="askMoodChance">The chance of asking for the user's mood, if it does not request the user's name.</param>
        /// <returns></returns>
        private string Greet(bool greetUser, bool useName, int askNameChance, int askMoodChance)
        {
            string greet = greetUser ? Util.RandomPick(Write.Greetings) : String.Empty;
            string intro;
            string end = Util.RandomPick(Write.EndPhrase);
            string reply;
            if (knowsName)
            {
                intro = Util.RandomPick(Write.Intros);
                string pComma = Util.RandomPick(Write.RandomComma);
                if (useName) // Use name, maybe greet, maybe ask mood
                {
                    if (this.Name == userName)
                        return "well, this is awkward.";
                    PString[] s = new PString[2];
                    if (greetUser)
                    {
                        s[0] = new PString(greet + pComma + userName + end, 100 - askMoodChance);
                        s[1] = new PString(greet + " " + userName + ", " + intro + '?', askMoodChance);
                    }
                    else
                    {
                        s[0] = new PString(userName + ", " + intro + '?', 50);
                        s[1] = new PString(intro + pComma + userName + '?', 50);
                    }
                    reply = Util.RandomPick(s);
                    if (reply.Contains(intro))
                        requestState = RequestState.Mood;
                    usedGreet = greetUser;
                    return reply;
                }
                else
                {
                    PString[] s;
                    if (greetUser) // Don't use name, greet, maybe ask mood
                    {
                        s = new PString[2];
                        s[0] = new PString(greet + end, 100 - askMoodChance);
                        s[1] = new PString(greet + ", " + intro + '?', askMoodChance);
                        usedGreet = true;
                    }
                    else // Don't use name, don't greet, ask mood
                    {
                        requestState = RequestState.Mood;
                        return intro + '?';
                    }
                    reply = Util.RandomPick(s);
                    if (reply.Contains(intro))
                        requestState = RequestState.Mood;
                    return reply;
                }
            }
            else
            {
                Boolean askName = Util.RandomBoolean(askNameChance);
                if (askName) // Greet if greetUser=true, ask name
                {
                    this.requestState = RequestState.Name;
                    intro = Util.RandomPick(Write.NameIntros);
                    if (greetUser) { usedGreet = true; return (greet + ", " + intro + '?'); }
                    else return intro + '?';
                }
                else // Greet if greetUser=true, maybe ask mood
                {
                    intro = Util.RandomPick(Write.Intros);
                    if (!greetUser) { requestState = RequestState.Mood; return (intro + '?'); }
                    PString[] s = new PString[2];
                    s[0] = new PString(greet + end, 100 - askMoodChance);
                    s[1] = new PString(greet + ", " + intro + '?', askMoodChance);
                    reply = Util.RandomPick(s);
                    if (reply.Contains(intro))
                        requestState = RequestState.Mood;
                    usedGreet = true;
                    return reply;
                }
            }
        }
        /// <summary>
        /// Greets the user, with a chance to request the user's name, and a chance to request the user's mood. The forceGreet flag is intended for use when the bot is initiating a chat,
        /// and only influences the result if the user's name is unknown.
        /// </summary>
        /// <param name="useName">If set to true, the bot will always address the user by name if it is known.</param>
        private string Greet(Boolean greetUser, Boolean useName)
        {
            return Greet(greetUser, useName, 60, 75);
        }
        /// <summary>
        /// Greets the user, with a chance to request the user's name, and a chance to request the user's mood.
        /// </summary>
        /// <param name="useName">If set to true, the bot will always address the user by name if it is known.</param>
        private string Greet(bool useName)
        {
            return Greet(false, useName);
        }
        /// <summary>
        /// Greets the user, with a chance to request the user's name, and a chance to request the user's mood.
        /// </summary>
        private string Greet()
        {
            return Greet(false);
        }

        /// <summary>Requests the user's mood.</summary>
        private string AskMood()
        {
            return Util.RandomPick(Write.Intros) + '?';
        }
        /// <summary>Expresses joy at the user's response.</summary>
        private string ReplyHappy()
        {
            return Util.RandomPick(Write.PositiveReplies) + Util.RandomPick(Write.EndPhrase);
        }
        /// <summary>Expresses sadness at the user's response.</summary>
        private string ReplyUnhappy()
        {
            return Util.RandomPick(Write.NegativeReplies) + Util.RandomPick(Write.EndPhrase);
        }
        /// <summary>Expresses indifference at the user's response, then sets genericReply to true. Returns the empty string if genericReply is already true.</summary>
        private string GenericReply()
        {
            if (genericReply)
                return "";
            genericReply = true;
            return Util.RandomPick(Write.GenericReplies) + Util.RandomPick(Write.EndPhrase);
        }
        /// <summary>
        /// Expresses joy, sadness or indifference depending on the average mood of the given message.
        /// </summary>
        /// <param name="words">The individual words in the message.</param> <param name="message">The complete message.</param>
        private string ReplyMood(string[] words, string message)
        {
            int mood = SearchMood(words, message);
            if (mood != NO_MOOD) { requestState = RequestState.None; }
            if (mood == 1) { return ReplyHappy(); }
            else if (mood == -1) { return ReplyUnhappy(); }
            else { return GenericReply(); }
        }

        /// <summary>Easter egg.</summary>
        private string method2(string s)
        {
            if (block_e2) { return s; }
            if (s.Contains("Scott") || s.Contains("Alastair")) { return "Special Scott"; }
            else { return s; }
        }
        /// <summary>
        /// Requests the user's location. Has a high chance of addressing the user by name.
        /// </summary>
        private string AskPlace()
        {
            string place = Util.RandomPick(Write.Place);
            requestState = RequestState.Place;
            return WriteQuestion(place, 80);
        }
        /// <summary>
        /// Makes further comments or questions after acquiring the user's location.
        /// </summary>
        private string ReplyPlace()
        {
            return "So you live " + placePosition + ' ' + userPlace + ". Is that correct?";
        }

        /// <summary>
        /// Makes a statement, with a chance to address the user by name. Defaults to 0% if the user's name is unknown.
        /// </summary>
        /// <param name="message">The bot's statement.</param>
        /// <param name="useNameChance">The percentage chance to address the user by name. Does not apply if the user's name is unknown.</param>
        private string WriteStatement(string message, int useNameChance)
        {
            string endMark = Util.RandomPick(Write.EndPhrase);
            if (!knowsName)
                return (message + endMark);
            string pComma = Util.RandomPick(Write.RandomComma);
            PString[] s = new PString[2];
            s[0] = new PString(message + endMark, 100 - useNameChance);
            s[1] = new PString(message + pComma + userName + endMark, useNameChance);
            return Util.RandomPick(s);
        }
        /// <summary>
        /// Asks the user a question, with a chance to address the user by name. Defaults to 0% if the user's name is unknown.
        /// </summary>
        /// <param name="question">The question to be asked.</param>
        /// <param name="useNameChance">The percentage chance to address the user by name. Does not apply if the user's name is unknown.</param>
        private string WriteQuestion(string message, int useNameChance)
        {
            if (!knowsName)
                return (message + '?');
            string pComma = Util.RandomPick(Write.RandomComma);
            PString[] s = new PString[3];
            s[0] = new PString(message + '?', 100 - useNameChance);
            s[1] = new PString(userName + ", " + message + '?', useNameChance / 2);
            s[2] = new PString(message + pComma + userName + '?', useNameChance / 2);
            return Util.RandomPick(s);
        }

        /// <summary>
        /// Searches the user's message for a name, returns 1 if a name is found, 0 if not, PRIVATE if the user appears to be keeping it private.
        /// </summary>
        /// <param name="words">The individual words in the message.</param> <param name="message">The complete message.</param>
        private int SearchName(string[] words, string message)
        {
            words = Refine(words);
            message = Refine(message);
            int index = Util.IndexOfAny(message, ForceReadGivenName);
            if (index == -1)
            {
                if (words.Length > 3)
                {
                    if (!EndMarks.Contains(words[1]) && !EndMarks.Contains(words[2]) && !EndMarks.Contains(words[3]))
                        return 0;
                }
                string s = "";
                for (int i = 0; i < message.Length; i++)
                {
                    if (EndMarks.Contains(message[i]))
                        break;
                    else
                    {
                        s += message[i];
                    }
                }
                if (s == "")
                    return 0;
                if (Util.IndexOfAny(s, ReadPrivate) != -1)
                    return PRIVATE;
                userName = Util.CapitalizeWords(s);
            }
            else
            {
                int startSearch = message.IndexOf(ForceReadGivenName[index]) + ForceReadGivenName[index].Length + 1;
                string s = "";
                if (Util.IndexOfAny(message, ReadPrivate) != -1)
                    return PRIVATE;
                for (int i = startSearch; i < message.Length; i++)
                {
                    if (EndMarks.Contains(message[i]))
                        break;
                    else
                    {
                        s += message[i];
                    }
                }
                if (s == "")
                    return 0;
                userName = Util.CapitalizeWords(s);
            }
            knowsName = true;
            requestState = RequestState.None;
            userName = method2(userName);
            return 1;
        }

        /// <summary>
        /// Searches the message for any expression of mood.
        /// Returns 1 if there is a positive mood, -1 if there is a negative one, 0 if there is an average one, NO_MOOD if no mood is expressed.
        /// </summary>
        /// <param name="words">The individual words in the message.</param> <param name="message">The complete message.</param>
        private int SearchMood(string[] words, string message)
        {
            int mood = 0;
            int modifier = 1; bool moodFound = false;
            for (int i = 0; i < words.Length; i++)
            {
                if (ReadNegate.Contains(words[i]))
                    modifier = -modifier;
                else if (ReadEmphasis.Contains(words[i]))
                    modifier *= 2;
                else if (ReadPositive.Contains(words[i]))
                {
                    if (words[i] == "well" && modifier == 1) { continue; } // If the 'positive' word is "well", but it's not preceded by "very", "not", etc., chances are it's just an interjection.
                    mood += modifier;
                    moodFound = true;
                    modifier = 1;
                }
                else if (ReadNegative.Contains(words[i]))
                {
                    mood -= modifier;
                    moodFound = true;
                    modifier = 1;
                }
                else if (ReadNeutral.Contains(words[i]))
                {
                    moodFound = true;
                    modifier = 1;
                }
            }
            if (!moodFound) { return NO_MOOD; }
            knowsMood = true;
            if (mood > 0) { return 1; }
            else if (mood == 0) { return 0; }
            else { return -1; }
        }

        /// <summary>
        /// Searches the user's message for their location, returns 1 if a place was found, 0 if not, PRIVATE if the user seems to be keeping it private.
        /// </summary>
        /// <param name="words">The individual words in the message.</param>
        /// <param name="rwords">The individual words in the message, capitalized where the user used capitals.</param>
        /// <param name="message">The complete message.</param>
        private int SearchPlace(string[] words, string[] rwords, string message)
        {
            // Remove words of assertion (e.g. "I'm actually living in" becomes "I'm living in")
            words = Refine(words);
            rwords = Refine(rwords); // Note: this won't catch capitalized assertion words, but unless the user's grammar is terrible that doesn't make a difference anyway.
            message = Refine(message);
            // Determine the starting position of the search: the first word after a declaration of the user's location being somewhere, or just the first word.
            if (Util.IndexOfAny(message, ReadPrivate) != -1) { return PRIVATE; }
            int index = Util.IndexOfAny(message, ForceReadGivenPlace);
            int startSearch;
            if (index == -1) { startSearch = 0; }
            else
            {
                int startWord = message.IndexOf(ForceReadGivenPlace[index]) + ForceReadGivenPlace[index].Length + 1;
                string w = "";
                for (int i = startWord; i < message.Length; i++)
                {
                    if (Separators.Contains(message[i]))
                        break;
                    else
                        w += message[i];
                }
                int j = 0;
                for (j = 0; j < words.Length; j++)
                {
                    if (words[j] == w)
                        break;
                }
                startSearch = j;
            }
            // Determine the appropriate preposition/article for the user's location
            if (ReadPosition.Contains(words[startSearch]))
            {
                if (rwords.Length < 2) { return 0; }
                placePosition = words[startSearch];
                startSearch++;
                if (ReadArticles.Contains(words[startSearch]))
                {
                    if (rwords.Length < 3) { return 0; }
                    placePosition += ' ' + words[startSearch];
                    startSearch++;
                }
            }
            else { placePosition = "in"; }
            // Determine the name of the user's location
            string s = "";
            for (int i = startSearch; i < rwords.Length; i++)
            {
                if (EndMarks.Contains(rwords[i]))
                    break;
                else
                {
                    s += rwords[i] + ' ';
                }
            }
            if (s == "")
                return 0;
            // The user's location is now known. Trim whitespaces, set knowsPlace and reset requestState. Return that the location was found.
            userPlace = s.Trim();
            knowsPlace = true;
            requestState = RequestState.None;
            return 1;
        }
    }
}
