﻿using PlexDL.Common.Logging;
using PlexDL.PlexAPI;
using PlexDL.WaitWindow;
using System;
using System.Windows.Forms;

namespace PlexDL.UI
{
    public partial class PlexLogin : Form
    {
        public bool Success { get; set; } = false;
        public string AccountToken { get; set; } = "";

        public PlexLogin()
        {
            InitializeComponent();
        }

        private void PlexLogin_Load(object sender, EventArgs e)
        {
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                btnLogin_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LoginWorker(object sender, WaitWindowEventArgs e)
        {
            try
            {
                var plex = new MyPlex();
                var user = plex.Authenticate(txtUsername.Text, txtPassword.Text);
                string token = user.authenticationToken;
                e.Result = token;
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "PlexLoginWorkerError");
                e.Result = null;
            }
        }

        private void btnShowHidePwd_Click(object sender, EventArgs e)
        {
            if (txtPassword.UseSystemPasswordChar)
            {
                txtPassword.UseSystemPasswordChar = false;
                btnShowHidePwd.Text = "Hide";
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
                btnShowHidePwd.Text = "Show";
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtUsername.Text) && !string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    string token = (string)WaitWindow.WaitWindow.Show(LoginWorker, "Logging you in");
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        // valid account tokens are always 20 characters in length
                        if (token.Length == 20)
                        {
                            MessageBox.Show("Successfully authenticated your Plex.tv account. You can now load servers and relays from Plex.tv", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AccountToken = token;
                            Success = true;
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Received an invalid token from the server", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Incorrect username/password", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter your Plex.tv username and password correctly", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to login to Plex.tv; an error occurred.\n\n" + ex, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggingHelpers.RecordException(ex.Message, "PlexLoginError");
                Success = false;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}