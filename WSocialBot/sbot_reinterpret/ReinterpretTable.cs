using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSocialBot.sbot_reinterpret
{
    /// <summary>
    /// A mapping that converts words typed by a user into phrases that can be interpreted by a social bot.
    /// </summary>
    interface ReinterpretTable
    {
        /// <summary>
        /// Reinterprets a string according to the reinterpret table.
        /// </summary>
        /// <param name="original">The string to be reinterpreted.</param>
        /// <returns>A reinterpretation of the original string.</returns>
        string GetResult(string original);
    }
}
