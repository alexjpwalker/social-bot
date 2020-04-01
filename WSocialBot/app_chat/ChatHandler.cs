using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSocialBot.app_chat
{
    /// <summary>
    /// Represents a chat handler, which converts user/AI chat into a readable log of the conversation.
    /// </summary>
    interface ChatHandler
    {
        /// <summary>
        /// Reports that the user has submitted the specified message.
        /// </summary>
        /// <param name="chat">The user's chat message.</param>
        string SubmitChat(string chat);
        /// <summary>
        /// Reports that a user with the specified name has submitted the specified message.
        /// </summary>
        /// <param name="chat">The user's chat message.</param>
        string SubmitChat(string chat, string name);
    }
}
