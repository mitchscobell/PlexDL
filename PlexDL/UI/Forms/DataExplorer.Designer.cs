﻿using PlexDL.Common.Components.Controls;

namespace PlexDL.UI.Forms
{
    partial class DataExplorer
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataExplorer));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.lstTables = new System.Windows.Forms.ListBox();
            this.dgvMain = new PlexDL.Common.Components.Controls.FlatDataGridView();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.lblTable = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTableValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblViewing = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblViewingValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.cxtMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itmRawXml = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.statusMain.SuspendLayout();
            this.cxtMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.lstTables, 0, 0);
            this.tlpMain.Controls.Add(this.dgvMain, 1, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(800, 428);
            this.tlpMain.TabIndex = 0;
            // 
            // lstTables
            // 
            this.lstTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTables.FormattingEnabled = true;
            this.lstTables.Location = new System.Drawing.Point(3, 3);
            this.lstTables.Name = "lstTables";
            this.lstTables.Size = new System.Drawing.Size(205, 422);
            this.lstTables.TabIndex = 0;
            this.lstTables.SelectedIndexChanged += new System.EventHandler(this.LstTables_SelectedIndexChanged);
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            this.dgvMain.AllowUserToOrderColumns = true;
            this.dgvMain.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMain.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMain.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMain.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMain.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.dgvMain.IsContentTable = false;
            this.dgvMain.Location = new System.Drawing.Point(214, 3);
            this.dgvMain.MultiSelect = false;
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.RowHeadersVisible = false;
            this.dgvMain.RowsEmptyText = "No Data Found";
            this.dgvMain.RowsEmptyTextForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(134)))), ((int)(((byte)(134)))));
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.Size = new System.Drawing.Size(583, 422);
            this.dgvMain.TabIndex = 1;
            // 
            // statusMain
            // 
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblTable,
            this.lblTableValue,
            this.lblViewing,
            this.lblViewingValue});
            this.statusMain.Location = new System.Drawing.Point(0, 428);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(800, 22);
            this.statusMain.TabIndex = 1;
            this.statusMain.Text = "Status";
            // 
            // lblTable
            // 
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(37, 17);
            this.lblTable.Text = "Table:";
            // 
            // lblTableValue
            // 
            this.lblTableValue.ForeColor = System.Drawing.Color.DarkRed;
            this.lblTableValue.Name = "lblTableValue";
            this.lblTableValue.Size = new System.Drawing.Size(69, 17);
            this.lblTableValue.Text = "Not Loaded";
            // 
            // lblViewing
            // 
            this.lblViewing.Name = "lblViewing";
            this.lblViewing.Size = new System.Drawing.Size(52, 17);
            this.lblViewing.Text = "Viewing:";
            // 
            // lblViewingValue
            // 
            this.lblViewingValue.Name = "lblViewingValue";
            this.lblViewingValue.Size = new System.Drawing.Size(24, 17);
            this.lblViewingValue.Text = "0/0";
            // 
            // cxtMain
            // 
            this.cxtMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmRawXml});
            this.cxtMain.Name = "cxtMain";
            this.cxtMain.Size = new System.Drawing.Size(124, 26);
            // 
            // itmRawXml
            // 
            this.itmRawXml.Name = "itmRawXml";
            this.itmRawXml.Size = new System.Drawing.Size(123, 22);
            this.itmRawXml.Text = "Raw XML";
            this.itmRawXml.Click += new System.EventHandler(this.ItmRawXml_Click);
            // 
            // DataExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ContextMenuStrip = this.cxtMain;
            this.Controls.Add(this.tlpMain);
            this.Controls.Add(this.statusMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "DataExplorer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Explorer";
            this.Load += new System.EventHandler(this.ApiExplorer_Load);
            this.tlpMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.cxtMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.ListBox lstTables;
        private FlatDataGridView dgvMain;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel lblViewing;
        private System.Windows.Forms.ToolStripStatusLabel lblViewingValue;
        private System.Windows.Forms.ToolStripStatusLabel lblTable;
        private System.Windows.Forms.ToolStripStatusLabel lblTableValue;
        private System.Windows.Forms.ContextMenuStrip cxtMain;
        private System.Windows.Forms.ToolStripMenuItem itmRawXml;
    }
}