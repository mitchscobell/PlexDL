﻿namespace PlexDL.UI
{
    partial class CacheSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.settingsGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // settingsGrid
            // 
            this.settingsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsGrid.Location = new System.Drawing.Point(0, 0);
            this.settingsGrid.Name = "settingsGrid";
            this.settingsGrid.Size = new System.Drawing.Size(319, 285);
            this.settingsGrid.TabIndex = 0;
            // 
            // CacheSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 285);
            this.Controls.Add(this.settingsGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CacheSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Caching Options";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid settingsGrid;
    }
}