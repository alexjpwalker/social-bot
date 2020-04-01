using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSocialBot
{
    public partial class RenameDialog : Form
    {
        int randomizeCount;

        public RenameDialog(string currentName)
        {
            InitializeComponent();
            randomizeCount = 0;
            textField.Text = currentName;
        }

        public string GetNewName()
        {
            return textField.Text;
        }

        private void RenameDialog_Load(object sender, EventArgs e)
        {
            textField.SelectAll();
        }

        private void Randomize_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textField.Text = sbot_util.Util.RandomID();
            randomizeCount++;
            if (randomizeCount >= 64)
            {
                app_view.Info.Display("You have been spending a long time clicking Randomize. Are you sure you don't have too much free time on your hands?");
                randomizeCount = 56;
            }
        }
    }
}
