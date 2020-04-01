using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSocialBot.sbot_util
{
    /// <summary>
    /// Represents a probabilistic string, which includes a string and a number from 0-100 representing the probability of that string being used by a bot.
    /// </summary>
    class PString
    {
        public string String { get; set; }
        public int Chance { get; set; }

        /// <summary>
        /// Constructs a new instance of PString with the given string and percentage chance.
        /// <para>Please note: it is important that the integral percentage chances in PString arrays sum to 100. They will not function properly otherwise.</para>
        /// </summary>
        /// <param name="str">The string to be used.</param>
        /// <param name="percentChance">An integer representing the percentage chance of using this string.</param>
        public PString(string str, int percentChance)
        {
            this.String = str;
            this.Chance = percentChance;
        }
    }
}
