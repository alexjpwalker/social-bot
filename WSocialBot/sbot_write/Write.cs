using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSocialBot.sbot_main;
using WSocialBot.sbot_util;

namespace WSocialBot.sbot_write
{
    /// <summary>
    /// The class of write-strings for a social bot in PString array format. All properties are read-only.
    /// <para>Certain write-strings depend on specified parameters. In these cases the capitalization of string parameters is preserved.</para>
    /// </summary>
    sealed class Write
    {
        /* Template for properties
        /// <summary></summary>
        public static PString[] newPString
        {
            get
            {
                PString[] s = new PString[10];
                s[0] = new PString("", 0);
                s[1] = new PString("", 0);
                s[2] = new PString("", 0);
                s[3] = new PString("", 0);
                s[4] = new PString("", 0);
                s[5] = new PString("", 0);
                s[6] = new PString("", 0);
                s[7] = new PString("", 0);
                s[8] = new PString("", 0);
                s[9] = new PString("", 0);
                return s;
            }
        }

         * Template for methods
        /// <summary></summary>
        public static PString[] newPString(string name)
        {
            PString[] s = new PString[10];
            s[0] = new PString("", 0);
            s[1] = new PString("", 0);
            s[2] = new PString("", 0);
            s[3] = new PString("", 0);
            s[4] = new PString("", 0);
            s[5] = new PString("", 0);
            s[6] = new PString("", 0);
            s[7] = new PString("", 0);
            s[8] = new PString("", 0);
            s[9] = new PString("", 0);
            return s;
        }
        */
        /// <summary>Greets the user. Typical string: "hi"</summary>
        public static PString[] Greetings
        {
            get
            {
                PString[] s = new PString[4];
                s[0] = new PString("hello", 30);
                s[1] = new PString("hi", 35);
                s[2] = new PString("hey", 30);
                s[3] = new PString("good day", 5);
                return s;
            }
        }
        /// <summary>Requests the user's name. Typical string: "what's your name"</summary>
        public static PString[] NameIntros
        {
            get
            {
                PString[] s = new PString[2];
                s[0] = new PString("what's your name", 70);
                s[1] = new PString("what is your name", 30);
                return s;
            }
        }
        /// <summary>Requests the user's location. Typical string: "where are you from"</summary>
        public static PString[] Place
        {
            get
            {
                PString[] s = new PString[6];
                s[0] = new PString("where do you live", 15);
                s[1] = new PString("where are you located", 10);
                s[2] = new PString("where are you from", 30);
                s[3] = new PString("where do you normally live", 10);
                s[4] = new PString("where are you living at the moment", 20);
                s[5] = new PString("where are you living right now", 15);
                return s;
            }
        }
        /// <summary>Requests the user's mood. Typical string: "how are you"</summary>
        public static PString[] Intros
        {
            get
            {
                PString[] s = new PString[6];
                s[0] = new PString("how are you", 30);
                s[1] = new PString("how are you doing", 10);
                s[2] = new PString("how's it going", 20);
                s[3] = new PString("what's up", 20);
                s[4] = new PString("what's new", 10);
                s[5] = new PString("how are you today", 10);
                return s;
            }
        }
        /// <summary>Inserts a separator between two phrases or sentences. Typical string: ". "</summary>
        public static PString[] RandomSeparators
        {
            get
            {
                PString[] s = new PString[5];
                s[0] = new PString(". ", 35);
                s[1] = new PString(", ", 25);
                s[2] = new PString(" - ", 15);
                s[3] = new PString("; ", 20);
                s[4] = new PString("... ", 5);
                return s;
            }
        }
        /// <summary>Inserts a blank space, with a high chance to insert a comma immediately before it. Typical string: ", "</summary>
        public static PString[] RandomComma
        {
            get
            {
                PString[] s = new PString[2];
                s[0] = new PString(", ", 78);
                s[1] = new PString(" ", 22);
                return s;
            }
        }
        /// <summary>Ends a sentence with a punctuation mark. Typical string: ". "</summary>
        public static PString[] EndPhrase
        {
            get
            {
                PString[] s = new PString[3];
                s[0] = new PString("! ", 25);
                s[1] = new PString(". ", 70);
                s[2] = new PString("... ", 5);
                return s;
            }
        }
        /// <summary>Ends a sentence with a high chance to use an exclamation mark. Typical string: "! "</summary>
        public static PString[] EndExclaim
        {
            get
            {
                PString[] s = new PString[2];
                s[0] = new PString("! ", 65);
                s[1] = new PString(". ", 35);
                return s;
            }
        }
        /// <summary>Expresses joy at a user's response. Typical string: "good to hear"</summary>
        public static PString[] PositiveReplies
        {
            get
            {
                PString[] s = new PString[10];
                s[0] = new PString("that's nice", 16);
                s[1] = new PString("excellent", 10);
                s[2] = new PString("OK, good", 16);
                s[3] = new PString("good to hear", 16);
                s[4] = new PString("glad to hear it", 5);
                s[5] = new PString("that's good", 10);
                s[6] = new PString("cool", 5);
                s[7] = new PString("OK, great", 12);
                s[8] = new PString("well, that's good", 5);
                s[9] = new PString("awesome", 5);
                return s;
            }
        }
        /// <summary>Expresses sadness at a user's response. Typical string: "that's a shame"</summary>
        public static PString[] NegativeReplies
        {
            get
            {
                PString[] s = new PString[7];
                s[0] = new PString("that's a shame", 30);
                s[1] = new PString("that's not good", 10);
                s[2] = new PString("ah, sorry to hear", 20);
                s[3] = new PString("sorry to hear that", 15);
                s[4] = new PString("that's unfortunate", 5);
                s[5] = new PString("well, never mind", 12);
                s[6] = new PString("hope you're all right", 8);
                return s;
            }
        }
        /// <summary>Expresses indifference at a user's response. Typical string: "alright"</summary>
        public static PString[] GenericReplies
        {
            get
            {
                PString[] s = new PString[10];
                s[0] = new PString("OK", 5);
                s[1] = new PString("alright", 15);
                s[2] = new PString("OK then", 10);
                s[3] = new PString("alright then", 10);
                s[4] = new PString("fair enough", 10);
                s[5] = new PString("well, that's interesting", 15);
                s[6] = new PString("interesting", 5);
                s[7] = new PString("fine then", 5);
                s[8] = new PString("I see", 15);
                s[9] = new PString("okay", 10);
                return s;
            }
        }
        /// <summary>Asks for the user's opinion on a place after the place has been stated. Typical string: "what's it like in England"
        /// <remarks>Depends on Article keywords.</remarks></summary>
        /// <param name="position">Preposition/article describing the place.</param><param name="place">The name of the place.</param>
        public static PString[] AskPlaceOpinion(string position, string place)
        {
            string article = "";
            if (position.Contains(" a")) { position = position.Replace(" a", ""); article = "your"; }
            else if (position.Contains(" an")) { position = position.Replace(" an", ""); article = "your"; }
            else if (position.Contains(" some")) { position = position.Replace(" some", ""); article = "your"; }
            else if (position.Contains(" the")) { position = position.Replace(" the", ""); article = "the"; }

            string aPlace = (article == "" ? place : article + ' ' + place);
            string inPlace = position + ' ' + aPlace;

            PString[] s = new PString[5];
            s[0] = new PString("do you enjoy being " + inPlace, 20);
            s[1] = new PString("what's it like " + inPlace, 30);
            s[2] = new PString("do you like " + aPlace, 30);
            s[3] = new PString("what do you think of " + aPlace, 12);
            s[4] = new PString("what's your opinion of" + aPlace, 8);
            return s;
        }
        /// <summary>Asks for the user's opinion on a place after the place has been stated. Typical string: "what's it like in England"
        /// <remarks>Depends on Article keywords.</remarks></summary>
        /// <param name="position">Preposition/article describing the place.</param><param name="place">The name of the place.</param>
        public static PString[] AskObjectOpinion(string name)
        {
            return null;
        }
    }
}
