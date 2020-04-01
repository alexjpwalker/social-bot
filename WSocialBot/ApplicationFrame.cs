using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSocialBot.app_view;
using WSocialBot.app_chat;
using WSocialBot.sbot_main;
using WSocialBot.sbot_util;

namespace WSocialBot
{
    public partial class ApplicationFrame : Form
    {
        private SocialBot socialBot;
        private ChatHandler chatHandler;
        private ChatOptions chatOptions;

        public ApplicationFrame()
        {
            socialBot = new DefaultBot(Util.RandomID(8));
            chatHandler = new BasicChatHandler();
            chatOptions = new ChatOptions();
            InitializeComponent();
        }

        private bool isBlank(String s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != ' ')
                    return false;
            }
            return true;
        }

        private void updateFont(Font newFont, Color newColor)
        {
            selectFont.Font = newFont;
            selectFont.Color = newColor;
            ChatBox.Font = newFont;
            ChatBox.ForeColor = newColor;
            ChatEntry.Font = ChatBox.Font;
            ChatEntry.ForeColor = ChatBox.ForeColor;
        }

        private void updateChatOptions()
        {
            chatOptions.AutoCaps = autoCaps.Checked;
        }

        private void updateChatOptions(object sender, EventArgs e)
        {
            updateChatOptions();
        }

        /// <summary>
        /// Appends some text to the chat log.
        /// </summary>
        /// <param name="text">The text to be added.</param>
        private void appendText(string text)
        {
            ChatBox.AppendText(text);
            ChatBox.ScrollToCaret();
            ChatBox.Refresh();
        }

        private void ApplicationFrame_Load(object sender, EventArgs e)
        {
            if (!socialBot.LoadSuccess)
            {
                Error.Fatal();
                this.Close();
            }
            updateFont(ChatBox.Font, ChatBox.ForeColor);
            updateChatOptions();
            if (socialBot.opensChat)
            {
                string chat = socialBot.OpenChat();
                chat = chatHandler.SubmitChat(chat, socialBot.Name);
                appendText(chat);
            }
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            string entry = ChatEntry.Text;
            if (isBlank(entry))
            {
                ChatEntry.Clear();
                return;
            }
            string chat = chatHandler.SubmitChat(entry);
            appendText(chat);
            ChatEntry.Clear();
            ChatEntry.Refresh();
            string reply = socialBot.GetReply(entry);
            chat = chatHandler.SubmitChat(reply, socialBot.Name);
            appendText(chat);
        }

        private void renameBot_Click(object sender, EventArgs e)
        {
            RenameDialog renameDialog = new RenameDialog(socialBot.Name);
            if (renameDialog.ShowDialog() == DialogResult.OK)
                socialBot.Name = renameDialog.GetNewName();
        }

        private void changeFont_Click(object sender, EventArgs e)
        {
            if (selectFont.ShowDialog() == DialogResult.OK)
                updateFont(selectFont.Font, selectFont.Color);
        }

        private void ChatEntry_TextChanged(object sender, EventArgs e)
        {
            if (chatOptions.AutoCaps)
            {
                int caretLocation = ChatEntry.SelectionStart;
                ChatEntry.Text = Util.Capitalize(ChatEntry.Text);
                ChatEntry.SelectionStart = caretLocation;
            }
        }
    }
}
