using System;
using System.Drawing;
using System.Windows.Forms;

namespace SteamAccountSwitcher
{
    public partial class Account : UserControl
    {
        public new AccountSwitcher Parent { get; set; }

        bool selected = false;
        int position = 0;

        public Account()
        {
            InitializeComponent();
        }

        public Account(Int64 uID) : this()
        {
            this.UniqueID = uID;
        }

        public Account(Int64 uID, string userName, string description, int pos) : this(uID)
        {
            this.Username = userName;
            this.Description = description;
            if (String.IsNullOrWhiteSpace(CustomDisplayName)) CustomDisplayName = this.Username;
            this.position = pos;
        }

        public Account(Int64 uID, string userName, string description, string customDisplayName, int pos) : this(uID, userName, description, pos)
        {
            this.CustomDisplayName = customDisplayName;
        }

        public Int64 UniqueID { get; }

        public string Username
        { 
            get
            {
                return toolTip.GetToolTip(userNameLabel); 
            }
            
            set
            {
                toolTip.SetToolTip(userNameLabel, value);
            }
        }

        public string CustomDisplayName
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
                toolTip.SetToolTip(descriptionLabel, value);
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

        public int Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public void UpdateUI()
        {
            upPosButton.Enabled = !Parent.IsAccountTop(this);
            downPosButton.Enabled = !Parent.IsAccountBottom(this);
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

        private void accountInterface_DoubleClick(object sender, EventArgs e)
        {
            Parent.Login(this);
        }

        private void deleteAccount_Click(object sender, EventArgs e)
        {
            Parent.RemoveAccount(this);
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            Parent.OpenAccountEditor(this);
        }

        private void upPosButton_Click(object sender, EventArgs e)
        {
            Parent.MoveAccountUp(this);
        }

        private void downPosButton_Click(object sender, EventArgs e)
        {
            Parent.MoveAccountDown(this);
        }
    }
}
