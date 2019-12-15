using System;
using System.Drawing;
using System.Windows.Forms;

namespace SteamAccountSwitcher
{
    public partial class Account : UserControl
    {
        public new AccountSwitcher Parent { get; set; }

        bool selected = false;

        public Account()
        {
            InitializeComponent();
        }

        public Account(Int64 uID, string userName, string description) : this()
        {
            this.UniqueID = uID;
            this.Username = userName;
            this.Description = description;
        }

        public Int64 UniqueID { get; }

        public string Username
        { 
            get
            {
                return userNameLabel.Text; 
            }
            
            set
            {
                userNameLabel.Text = value;
            }
        }

        public string Description
        {
            get
            {
                return descriptionLabel.Text;
            }

            set
            {
                descriptionLabel.Text = value;
            }
        }

        public bool Selected
        {
            get
            {
                return selected;
            }

            set
            {
                if (value) SetColour(SystemColors.ActiveBorder);
                else SetColour(SystemColors.Control);
                selected = value;
            }
        }

        private void SetColour(Color color)
        {
            userNameLabel.BackColor = color;
            userNamePanel.BackColor = color;

            descriptionLabel.BackColor = color;
            descriptionPanel.BackColor = color;

            utilsPanel.BackColor = color;
        }

        private void accountInterface_Click(object sender, EventArgs e)
        {
            Parent.SelectAccount(this);
        }

        private void deleteAccount_Click(object sender, EventArgs e)
        {
            Parent.RemoveAccount(this);
        }
    }
}
