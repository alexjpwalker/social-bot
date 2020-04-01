using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSocialBot.app_chat
{
    class BasicChatHandler : ChatHandler
    {
        public string SubmitChat(string chat)
        {
            return SubmitChat(chat, "You");
        }
        public string SubmitChat(string chat, string name)
        {
            return String.Concat(name, " said...\n   ", chat, "\n");
        }
    }
}
