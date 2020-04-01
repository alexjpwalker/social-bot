using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSocialBot.sbot_main
{
    /// <summary>
    /// Represents a social bot, which accepts chat from an external source and must be able to open a chat and produce a reply to every message.
    /// The methods invoked by a social bot may change over its lifetime.
    /// </summary>
    class SocialBot
    {
        public string Name { get; set; }
        public bool LoadSuccess { get; protected set; }
        public bool opensChat { get; protected set; }

        public virtual string OpenChat() { return String.Empty; }
        public virtual string GetReply(string s) { return String.Empty; }
    }
}
