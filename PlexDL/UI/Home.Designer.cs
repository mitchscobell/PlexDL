﻿namespace PlexDL.UI
{
    partial class Home
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
            MetroSet_UI.Extensions.ImageSet imageSet1 = new MetroSet_UI.Extensions.ImageSet();
            MetroSet_UI.Extensions.ImageSet imageSet2 = new MetroSet_UI.Extensions.ImageSet();
            MetroSet_UI.Extensions.ImageSet imageSet3 = new MetroSet_UI.Extensions.ImageSet();
            MetroSet_UI.Extensions.ImageSet imageSet4 = new MetroSet_UI.Extensions.ImageSet();
            MetroSet_UI.Extensions.ImageSet imageSet5 = new MetroSet_UI.Extensions.ImageSet();
            MetroSet_UI.Extensions.ImageSet imageSet6 = new MetroSet_UI.Extensions.ImageSet();
            MetroSet_UI.Extensions.ImageSet imageSet7 = new MetroSet_UI.Extensions.ImageSet();
            MetroSet_UI.Extensions.ImageSet imageSet8 = new MetroSet_UI.Extensions.ImageSet();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            this.wkrUpdateContentView = new System.ComponentModel.BackgroundWorker();
            this.sfdSaveProfile = new System.Windows.Forms.SaveFileDialog();
            this.fbdSave = new System.Windows.Forms.FolderBrowserDialog();
            this.ofdLoadProfile = new System.Windows.Forms.OpenFileDialog();
            this.wkrDownloadAsync = new System.ComponentModel.BackgroundWorker();
            this.lstLog = new MetroSet_UI.Controls.MetroSetListBox();
            this.lblLibraryContent = new MetroSet_UI.Controls.MetroSetLabel();
            this.tipMain = new System.Windows.Forms.ToolTip(this.components);
            this.lblLibraryContentSections = new MetroSet_UI.Controls.MetroSetLabel();
            this.wkrGetMetadata = new libbrhscgui.Components.AbortableBackgroundWorker();
            this.cxtEpisodes = new MetroSet_UI.Controls.MetroSetContextMenuStrip();
            this.itmDownloadThisEpisode = new System.Windows.Forms.ToolStripMenuItem();
            this.itmDownloadAllEpisodes = new System.Windows.Forms.ToolStripMenuItem();
            this.cxtEpisodeOptions = new MetroSet_UI.Controls.MetroSetContextMenuStrip();
            this.itmEpisodeMetadata = new System.Windows.Forms.ToolStripMenuItem();
            this.itmEpisodeDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.thisEpisodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thisSeasonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cxtLibrarySections = new MetroSet_UI.Controls.MetroSetContextMenuStrip();
            this.itmManuallyLoadSection = new System.Windows.Forms.ToolStripMenuItem();
            this.cxtContentOptions = new MetroSet_UI.Controls.MetroSetContextMenuStrip();
            this.itmContentMetadata = new System.Windows.Forms.ToolStripMenuItem();
            this.itmContentDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdMetadata = new System.Windows.Forms.OpenFileDialog();
            this.cxtProfile = new MetroSet_UI.Controls.MetroSetContextMenuStrip();
            this.loadProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pbMain = new MetroSet_UI.Controls.MetroSetProgressBar();
            this.lblProgress = new MetroSet_UI.Controls.MetroSetLabel();
            this.lnkLogViewer = new MetroSet_UI.Controls.MetroSetLink();
            this.lblLog = new MetroSet_UI.Controls.MetroSetLabel();
            this.dgvEpisodes = new PlexDL.Common.Components.FlatDataGridView();
            this.dgvSeasons = new PlexDL.Common.Components.FlatDataGridView();
            this.dgvContent = new PlexDL.Common.Components.FlatDataGridView();
            this.dgvLibrary = new PlexDL.Common.Components.FlatDataGridView();
            this.dgvServers = new PlexDL.Common.Components.FlatDataGridView();
            this.lblLibrarySections = new MetroSet_UI.Controls.MetroSetLabel();
            this.btnMetadata = new MetroSet_UI.Controls.MetroSetEllipse();
            this.btnSetDlDir = new MetroSet_UI.Controls.MetroSetEllipse();
            this.btnSearch = new MetroSet_UI.Controls.MetroSetEllipse();
            this.btnLoadProfile = new MetroSet_UI.Controls.MetroSetEllipse();
            this.btnPause = new MetroSet_UI.Controls.MetroSetEllipse();
            this.btnHTTPPlay = new MetroSet_UI.Controls.MetroSetEllipse();
            this.btnConnect = new MetroSet_UI.Controls.MetroSetEllipse();
            this.btnDownload = new MetroSet_UI.Controls.MetroSetEllipse();
            this.cxtEpisodes.SuspendLayout();
            this.cxtEpisodeOptions.SuspendLayout();
            this.cxtLibrarySections.SuspendLayout();
            this.cxtContentOptions.SuspendLayout();
            this.cxtProfile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEpisodes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSeasons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLibrary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServers)).BeginInit();
            this.SuspendLayout();
            // 
            // sfdSaveProfile
            // 
            this.sfdSaveProfile.DefaultExt = "prof";
            this.sfdSaveProfile.Filter = "XML Profile|*.prof";
            this.sfdSaveProfile.Title = "Save XML Profile";
            // 
            // fbdSave
            // 
            this.fbdSave.Description = "Select a path to save downloaded items";
            // 
            // ofdLoadProfile
            // 
            this.ofdLoadProfile.Filter = "XML Profile|*.prof";
            this.ofdLoadProfile.Title = "Load XML Profile";
            // 
            // lstLog
            // 
            this.lstLog.BorderColor = System.Drawing.Color.LightGray;
            this.lstLog.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.lstLog.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.lstLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lstLog.HoveredItemBackColor = System.Drawing.Color.LightGray;
            this.lstLog.HoveredItemColor = System.Drawing.Color.DimGray;
            this.lstLog.ItemHeight = 20;
            this.lstLog.Location = new System.Drawing.Point(26, 266);
            this.lstLog.MultiSelect = false;
            this.lstLog.Name = "lstLog";
            this.lstLog.SelectedIndex = -1;
            this.lstLog.SelectedItem = null;
            this.lstLog.SelectedItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.lstLog.SelectedItemColor = System.Drawing.Color.White;
            this.lstLog.SelectedValue = null;
            this.lstLog.ShowBorder = true;
            this.lstLog.ShowScrollBar = true;
            this.lstLog.Size = new System.Drawing.Size(252, 220);
            this.lstLog.Style = MetroSet_UI.Design.Style.Light;
            this.lstLog.StyleManager = null;
            this.lstLog.TabIndex = 13;
            this.lstLog.ThemeAuthor = null;
            this.lstLog.ThemeName = null;
            this.tipMain.SetToolTip(this.lstLog, "PlexDL Log");
            // 
            // lblLibraryContent
            // 
            this.lblLibraryContent.AutoSize = true;
            this.lblLibraryContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblLibraryContent.Location = new System.Drawing.Point(731, 72);
            this.lblLibraryContent.Name = "lblLibraryContent";
            this.lblLibraryContent.Size = new System.Drawing.Size(105, 17);
            this.lblLibraryContent.Style = MetroSet_UI.Design.Style.Light;
            this.lblLibraryContent.StyleManager = null;
            this.lblLibraryContent.TabIndex = 17;
            this.lblLibraryContent.Text = "Library Content";
            this.lblLibraryContent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLibraryContent.ThemeAuthor = "Narwin";
            this.lblLibraryContent.ThemeName = "MetroLite";
            // 
            // lblLibraryContentSections
            // 
            this.lblLibraryContentSections.AutoSize = true;
            this.lblLibraryContentSections.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblLibraryContentSections.Location = new System.Drawing.Point(1025, 72);
            this.lblLibraryContentSections.Name = "lblLibraryContentSections";
            this.lblLibraryContentSections.Size = new System.Drawing.Size(163, 17);
            this.lblLibraryContentSections.Style = MetroSet_UI.Design.Style.Light;
            this.lblLibraryContentSections.StyleManager = null;
            this.lblLibraryContentSections.TabIndex = 19;
            this.lblLibraryContentSections.Text = "Library Content Sections";
            this.lblLibraryContentSections.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLibraryContentSections.ThemeAuthor = "Narwin";
            this.lblLibraryContentSections.ThemeName = "MetroLite";
            // 
            // wkrGetMetadata
            // 
            this.wkrGetMetadata.DoWork += new System.ComponentModel.DoWorkEventHandler(this.wkrGetMetadata_DoWork);
            // 
            // cxtEpisodes
            // 
            this.cxtEpisodes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cxtEpisodes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmDownloadThisEpisode,
            this.itmDownloadAllEpisodes});
            this.cxtEpisodes.Name = "cxtEpisodes";
            this.cxtEpisodes.Size = new System.Drawing.Size(197, 48);
            this.cxtEpisodes.Style = MetroSet_UI.Design.Style.Light;
            this.cxtEpisodes.StyleManager = null;
            this.cxtEpisodes.ThemeAuthor = "Narwin";
            this.cxtEpisodes.ThemeName = "MetroLite";
            // 
            // itmDownloadThisEpisode
            // 
            this.itmDownloadThisEpisode.Name = "itmDownloadThisEpisode";
            this.itmDownloadThisEpisode.Size = new System.Drawing.Size(196, 22);
            this.itmDownloadThisEpisode.Text = "Download This Episode";
            this.itmDownloadThisEpisode.Click += new System.EventHandler(this.itmDownloadThisEpisode_Click);
            // 
            // itmDownloadAllEpisodes
            // 
            this.itmDownloadAllEpisodes.Name = "itmDownloadAllEpisodes";
            this.itmDownloadAllEpisodes.Size = new System.Drawing.Size(196, 22);
            this.itmDownloadAllEpisodes.Text = "Download This Season";
            this.itmDownloadAllEpisodes.Click += new System.EventHandler(this.itmDownloadAllEpisodes_Click);
            // 
            // cxtEpisodeOptions
            // 
            this.cxtEpisodeOptions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cxtEpisodeOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmEpisodeMetadata,
            this.itmEpisodeDownload});
            this.cxtEpisodeOptions.Name = "cxtEpisodeOptions";
            this.cxtEpisodeOptions.Size = new System.Drawing.Size(129, 48);
            this.cxtEpisodeOptions.Style = MetroSet_UI.Design.Style.Light;
            this.cxtEpisodeOptions.StyleManager = null;
            this.cxtEpisodeOptions.ThemeAuthor = "Narwin";
            this.cxtEpisodeOptions.ThemeName = "MetroLite";
            this.cxtEpisodeOptions.Opening += new System.ComponentModel.CancelEventHandler(this.cxtEpisodeOptions_Opening);
            // 
            // itmEpisodeMetadata
            // 
            this.itmEpisodeMetadata.Name = "itmEpisodeMetadata";
            this.itmEpisodeMetadata.Size = new System.Drawing.Size(128, 22);
            this.itmEpisodeMetadata.Text = "Metadata";
            this.itmEpisodeMetadata.Click += new System.EventHandler(this.metadataToolStripMenuItem_Click);
            // 
            // itmEpisodeDownload
            // 
            this.itmEpisodeDownload.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.thisEpisodeToolStripMenuItem,
            this.thisSeasonToolStripMenuItem});
            this.itmEpisodeDownload.Name = "itmEpisodeDownload";
            this.itmEpisodeDownload.Size = new System.Drawing.Size(128, 22);
            this.itmEpisodeDownload.Text = "Download";
            // 
            // thisEpisodeToolStripMenuItem
            // 
            this.thisEpisodeToolStripMenuItem.Name = "thisEpisodeToolStripMenuItem";
            this.thisEpisodeToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.thisEpisodeToolStripMenuItem.Text = "This Episode";
            this.thisEpisodeToolStripMenuItem.Click += new System.EventHandler(this.thisEpisodeToolStripMenuItem_Click);
            // 
            // thisSeasonToolStripMenuItem
            // 
            this.thisSeasonToolStripMenuItem.Name = "thisSeasonToolStripMenuItem";
            this.thisSeasonToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.thisSeasonToolStripMenuItem.Text = "This Season";
            this.thisSeasonToolStripMenuItem.Click += new System.EventHandler(this.thisSeasonToolStripMenuItem_Click);
            // 
            // cxtLibrarySections
            // 
            this.cxtLibrarySections.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cxtLibrarySections.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmManuallyLoadSection});
            this.cxtLibrarySections.Name = "cxtLibrarySections";
            this.cxtLibrarySections.Size = new System.Drawing.Size(195, 26);
            this.cxtLibrarySections.Style = MetroSet_UI.Design.Style.Light;
            this.cxtLibrarySections.StyleManager = null;
            this.cxtLibrarySections.ThemeAuthor = "Narwin";
            this.cxtLibrarySections.ThemeName = "MetroLite";
            this.cxtLibrarySections.Opening += new System.ComponentModel.CancelEventHandler(this.cxtLibrarySections_Opening);
            // 
            // itmManuallyLoadSection
            // 
            this.itmManuallyLoadSection.Name = "itmManuallyLoadSection";
            this.itmManuallyLoadSection.Size = new System.Drawing.Size(194, 22);
            this.itmManuallyLoadSection.Text = "Manually Load Section";
            this.itmManuallyLoadSection.Click += new System.EventHandler(this.itmManuallyLoadSection_Click);
            // 
            // cxtContentOptions
            // 
            this.cxtContentOptions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cxtContentOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmContentMetadata,
            this.itmContentDownload});
            this.cxtContentOptions.Name = "cxtEpisodeOptions";
            this.cxtContentOptions.Size = new System.Drawing.Size(129, 48);
            this.cxtContentOptions.Style = MetroSet_UI.Design.Style.Light;
            this.cxtContentOptions.StyleManager = null;
            this.cxtContentOptions.ThemeAuthor = "Narwin";
            this.cxtContentOptions.ThemeName = "MetroLite";
            this.cxtContentOptions.Opening += new System.ComponentModel.CancelEventHandler(this.cxtContentOptions_Opening);
            // 
            // itmContentMetadata
            // 
            this.itmContentMetadata.Name = "itmContentMetadata";
            this.itmContentMetadata.Size = new System.Drawing.Size(128, 22);
            this.itmContentMetadata.Text = "Metadata";
            this.itmContentMetadata.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // itmContentDownload
            // 
            this.itmContentDownload.Name = "itmContentDownload";
            this.itmContentDownload.Size = new System.Drawing.Size(128, 22);
            this.itmContentDownload.Text = "Download";
            this.itmContentDownload.Click += new System.EventHandler(this.itmContentDownload_Click);
            // 
            // ofdMetadata
            // 
            this.ofdMetadata.Filter = "PlexMovie XML|*.pmxml";
            this.ofdMetadata.Title = "Load Metadata File";
            // 
            // cxtProfile
            // 
            this.cxtProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cxtProfile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadProfileToolStripMenuItem,
            this.saveProfileToolStripMenuItem});
            this.cxtProfile.Name = "cxtProfile";
            this.cxtProfile.Size = new System.Drawing.Size(138, 48);
            this.cxtProfile.Style = MetroSet_UI.Design.Style.Light;
            this.cxtProfile.StyleManager = null;
            this.cxtProfile.ThemeAuthor = "Narwin";
            this.cxtProfile.ThemeName = "MetroLite";
            // 
            // loadProfileToolStripMenuItem
            // 
            this.loadProfileToolStripMenuItem.Name = "loadProfileToolStripMenuItem";
            this.loadProfileToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.loadProfileToolStripMenuItem.Text = "Load Profile";
            this.loadProfileToolStripMenuItem.Click += new System.EventHandler(this.loadProfileToolStripMenuItem_Click);
            // 
            // saveProfileToolStripMenuItem
            // 
            this.saveProfileToolStripMenuItem.Name = "saveProfileToolStripMenuItem";
            this.saveProfileToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.saveProfileToolStripMenuItem.Text = "Save Profile";
            this.saveProfileToolStripMenuItem.Click += new System.EventHandler(this.saveProfileToolStripMenuItem_Click);
            // 
            // pbMain
            // 
            this.pbMain.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.pbMain.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.pbMain.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.pbMain.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.pbMain.DisabledProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.pbMain.Location = new System.Drawing.Point(26, 129);
            this.pbMain.Maximum = 100;
            this.pbMain.Minimum = 0;
            this.pbMain.Name = "pbMain";
            this.pbMain.Orientation = MetroSet_UI.Enums.ProgressOrientation.Horizontal;
            this.pbMain.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.pbMain.Size = new System.Drawing.Size(252, 10);
            this.pbMain.Style = MetroSet_UI.Design.Style.Light;
            this.pbMain.StyleManager = null;
            this.pbMain.TabIndex = 22;
            this.pbMain.Text = "metroSetProgressBar1";
            this.pbMain.ThemeAuthor = null;
            this.pbMain.ThemeName = null;
            this.pbMain.Value = 0;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblProgress.Location = new System.Drawing.Point(26, 142);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(30, 17);
            this.lblProgress.Style = MetroSet_UI.Design.Style.Light;
            this.lblProgress.StyleManager = null;
            this.lblProgress.TabIndex = 23;
            this.lblProgress.Text = "Idle";
            this.lblProgress.ThemeAuthor = null;
            this.lblProgress.ThemeName = null;
            // 
            // lnkLogViewer
            // 
            this.lnkLogViewer.AutoSize = true;
            this.lnkLogViewer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkLogViewer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lnkLogViewer.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkLogViewer.Location = new System.Drawing.Point(200, 246);
            this.lnkLogViewer.Name = "lnkLogViewer";
            this.lnkLogViewer.Size = new System.Drawing.Size(78, 17);
            this.lnkLogViewer.Style = MetroSet_UI.Design.Style.Light;
            this.lnkLogViewer.StyleManager = null;
            this.lnkLogViewer.TabIndex = 24;
            this.lnkLogViewer.TabStop = true;
            this.lnkLogViewer.Text = "Log Viewer";
            this.lnkLogViewer.ThemeAuthor = null;
            this.lnkLogViewer.ThemeName = null;
            this.lnkLogViewer.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(157)))), ((int)(((byte)(205)))));
            this.lnkLogViewer.Click += new System.EventHandler(this.lblViewFullLog_LinkClicked);
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblLog.Location = new System.Drawing.Point(26, 246);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(80, 17);
            this.lblLog.Style = MetroSet_UI.Design.Style.Light;
            this.lblLog.StyleManager = null;
            this.lblLog.TabIndex = 25;
            this.lblLog.Text = "PlexDL Log";
            this.lblLog.ThemeAuthor = null;
            this.lblLog.ThemeName = null;
            // 
            // dgvEpisodes
            // 
            this.dgvEpisodes.Location = new System.Drawing.Point(952, 301);
            this.dgvEpisodes.Name = "dgvEpisodes";
            this.dgvEpisodes.RowsEmptyText = "No TV Episodes Found";
            this.dgvEpisodes.Size = new System.Drawing.Size(320, 201);
            this.dgvEpisodes.TabIndex = 21;
            this.dgvEpisodes.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvEpisodes_Paint);
            // 
            // dgvSeasons
            // 
            this.dgvSeasons.Location = new System.Drawing.Point(952, 94);
            this.dgvSeasons.MultiSelect = false;
            this.dgvSeasons.Name = "dgvSeasons";
            this.dgvSeasons.RowsEmptyText = "No TV Seasons Found";
            this.dgvSeasons.Size = new System.Drawing.Size(320, 201);
            this.dgvSeasons.TabIndex = 20;
            this.dgvSeasons.SelectionChanged += new System.EventHandler(this.dgvSeasons_OnRowChange);
            this.dgvSeasons.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvSeasons_Paint);
            // 
            // dgvContent
            // 
            this.dgvContent.Location = new System.Drawing.Point(626, 94);
            this.dgvContent.MultiSelect = false;
            this.dgvContent.Name = "dgvContent";
            this.dgvContent.RowsEmptyText = "No Library Content Found";
            this.dgvContent.Size = new System.Drawing.Size(320, 408);
            this.dgvContent.TabIndex = 18;
            this.dgvContent.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvContent_ColumnHeaderMouseClick);
            this.dgvContent.SelectionChanged += new System.EventHandler(this.dgvContent_OnRowChange);
            // 
            // dgvLibrary
            // 
            this.dgvLibrary.Location = new System.Drawing.Point(300, 301);
            this.dgvLibrary.MultiSelect = false;
            this.dgvLibrary.Name = "dgvLibrary";
            this.dgvLibrary.RowsEmptyText = "No Library Sections Found";
            this.dgvLibrary.Size = new System.Drawing.Size(320, 201);
            this.dgvLibrary.TabIndex = 16;
            this.dgvLibrary.SelectionChanged += new System.EventHandler(this.dgvLibrary_OnRowChange);
            this.dgvLibrary.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvLibrary_Paint);
            // 
            // dgvServers
            // 
            this.dgvServers.Location = new System.Drawing.Point(300, 94);
            this.dgvServers.MultiSelect = false;
            this.dgvServers.Name = "dgvServers";
            this.dgvServers.RowsEmptyText = "No Servers Found";
            this.dgvServers.Size = new System.Drawing.Size(320, 201);
            this.dgvServers.TabIndex = 15;
            this.dgvServers.SelectionChanged += new System.EventHandler(this.dgvServers_OnRowChange);
            // 
            // lblLibrarySections
            // 
            this.lblLibrarySections.AutoSize = true;
            this.lblLibrarySections.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblLibrarySections.Location = new System.Drawing.Point(394, 72);
            this.lblLibrarySections.Name = "lblLibrarySections";
            this.lblLibrarySections.Size = new System.Drawing.Size(124, 17);
            this.lblLibrarySections.Style = MetroSet_UI.Design.Style.Light;
            this.lblLibrarySections.StyleManager = null;
            this.lblLibrarySections.TabIndex = 14;
            this.lblLibrarySections.Text = "Server Information";
            this.lblLibrarySections.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLibrarySections.ThemeAuthor = "Narwin";
            this.lblLibrarySections.ThemeName = "MetroLite";
            // 
            // btnMetadata
            // 
            this.btnMetadata.BackColor = System.Drawing.Color.Transparent;
            this.btnMetadata.BorderThickness = 0;
            this.btnMetadata.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnMetadata.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.btnMetadata.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.btnMetadata.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnMetadata.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnMetadata.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnMetadata.HoverTextColor = System.Drawing.Color.White;
            this.btnMetadata.Image = global::PlexDL.Properties.Resources.baseline_book_black_18dp;
            imageSet1.Focus = global::PlexDL.Properties.Resources.baseline_book_black_18dp_white;
            imageSet1.Idle = global::PlexDL.Properties.Resources.baseline_book_black_18dp;
            this.btnMetadata.ImageSet = imageSet1;
            this.btnMetadata.ImageSize = new System.Drawing.Size(28, 28);
            this.btnMetadata.Location = new System.Drawing.Point(130, 196);
            this.btnMetadata.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnMetadata.Name = "btnMetadata";
            this.btnMetadata.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnMetadata.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnMetadata.NormalTextColor = System.Drawing.Color.Black;
            this.btnMetadata.PressBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnMetadata.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnMetadata.PressTextColor = System.Drawing.Color.White;
            this.btnMetadata.Size = new System.Drawing.Size(44, 36);
            this.btnMetadata.Style = MetroSet_UI.Design.Style.Light;
            this.btnMetadata.StyleManager = null;
            this.btnMetadata.TabIndex = 10;
            this.btnMetadata.ThemeAuthor = null;
            this.btnMetadata.ThemeName = null;
            this.tipMain.SetToolTip(this.btnMetadata, "Set Download Directory");
            this.btnMetadata.Click += new System.EventHandler(this.btnMetadata_Click);
            // 
            // btnSetDlDir
            // 
            this.btnSetDlDir.BackColor = System.Drawing.Color.Transparent;
            this.btnSetDlDir.BorderThickness = 0;
            this.btnSetDlDir.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnSetDlDir.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.btnSetDlDir.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.btnSetDlDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSetDlDir.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnSetDlDir.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnSetDlDir.HoverTextColor = System.Drawing.Color.White;
            this.btnSetDlDir.Image = global::PlexDL.Properties.Resources.baseline_video_library_black_18dp;
            imageSet2.Focus = global::PlexDL.Properties.Resources.baseline_video_library_black_18dp_white;
            imageSet2.Idle = global::PlexDL.Properties.Resources.baseline_video_library_black_18dp;
            this.btnSetDlDir.ImageSet = imageSet2;
            this.btnSetDlDir.ImageSize = new System.Drawing.Size(28, 28);
            this.btnSetDlDir.Location = new System.Drawing.Point(78, 196);
            this.btnSetDlDir.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSetDlDir.Name = "btnSetDlDir";
            this.btnSetDlDir.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnSetDlDir.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnSetDlDir.NormalTextColor = System.Drawing.Color.Black;
            this.btnSetDlDir.PressBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnSetDlDir.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnSetDlDir.PressTextColor = System.Drawing.Color.White;
            this.btnSetDlDir.Size = new System.Drawing.Size(44, 36);
            this.btnSetDlDir.Style = MetroSet_UI.Design.Style.Light;
            this.btnSetDlDir.StyleManager = null;
            this.btnSetDlDir.TabIndex = 9;
            this.btnSetDlDir.ThemeAuthor = null;
            this.btnSetDlDir.ThemeName = null;
            this.tipMain.SetToolTip(this.btnSetDlDir, "Set Download Directory");
            this.btnSetDlDir.Click += new System.EventHandler(this.btnSetDlDir_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BorderThickness = 0;
            this.btnSearch.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnSearch.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.btnSearch.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSearch.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnSearch.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnSearch.HoverTextColor = System.Drawing.Color.White;
            this.btnSearch.Image = global::PlexDL.Properties.Resources.baseline_search_black_18dp;
            imageSet3.Focus = global::PlexDL.Properties.Resources.baseline_search_black_18dp_white;
            imageSet3.Idle = global::PlexDL.Properties.Resources.baseline_search_black_18dp;
            this.btnSearch.ImageSet = imageSet3;
            this.btnSearch.ImageSize = new System.Drawing.Size(28, 28);
            this.btnSearch.Location = new System.Drawing.Point(182, 84);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnSearch.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnSearch.NormalTextColor = System.Drawing.Color.Black;
            this.btnSearch.PressBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnSearch.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnSearch.PressTextColor = System.Drawing.Color.White;
            this.btnSearch.Size = new System.Drawing.Size(44, 36);
            this.btnSearch.Style = MetroSet_UI.Design.Style.Light;
            this.btnSearch.StyleManager = null;
            this.btnSearch.TabIndex = 3;
            this.btnSearch.ThemeAuthor = null;
            this.btnSearch.ThemeName = null;
            this.tipMain.SetToolTip(this.btnSearch, "Search");
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnLoadProfile
            // 
            this.btnLoadProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadProfile.BorderThickness = 0;
            this.btnLoadProfile.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnLoadProfile.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.btnLoadProfile.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.btnLoadProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnLoadProfile.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnLoadProfile.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnLoadProfile.HoverTextColor = System.Drawing.Color.White;
            this.btnLoadProfile.Image = global::PlexDL.Properties.Resources.baseline_folder_open_black_18dp;
            imageSet4.Focus = global::PlexDL.Properties.Resources.baseline_folder_open_black_18dp_white;
            imageSet4.Idle = global::PlexDL.Properties.Resources.baseline_folder_open_black_18dp;
            this.btnLoadProfile.ImageSet = imageSet4;
            this.btnLoadProfile.ImageSize = new System.Drawing.Size(28, 28);
            this.btnLoadProfile.Location = new System.Drawing.Point(26, 196);
            this.btnLoadProfile.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnLoadProfile.Name = "btnLoadProfile";
            this.btnLoadProfile.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnLoadProfile.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnLoadProfile.NormalTextColor = System.Drawing.Color.Black;
            this.btnLoadProfile.PressBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnLoadProfile.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnLoadProfile.PressTextColor = System.Drawing.Color.White;
            this.btnLoadProfile.Size = new System.Drawing.Size(44, 36);
            this.btnLoadProfile.Style = MetroSet_UI.Design.Style.Light;
            this.btnLoadProfile.StyleManager = null;
            this.btnLoadProfile.TabIndex = 8;
            this.btnLoadProfile.ThemeAuthor = null;
            this.btnLoadProfile.ThemeName = null;
            this.tipMain.SetToolTip(this.btnLoadProfile, "Save/Load Profile");
            this.btnLoadProfile.Click += new System.EventHandler(this.btnProfile_Click);
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.Transparent;
            this.btnPause.BorderThickness = 0;
            this.btnPause.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnPause.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.btnPause.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.btnPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnPause.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnPause.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnPause.HoverTextColor = System.Drawing.Color.White;
            this.btnPause.Image = global::PlexDL.Properties.Resources.baseline_play_arrow_black_18dp;
            imageSet5.Focus = global::PlexDL.Properties.Resources.baseline_play_arrow_black_18dp_white;
            imageSet5.Idle = global::PlexDL.Properties.Resources.baseline_play_arrow_black_18dp;
            this.btnPause.ImageSet = imageSet5;
            this.btnPause.ImageSize = new System.Drawing.Size(28, 28);
            this.btnPause.Location = new System.Drawing.Point(78, 84);
            this.btnPause.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnPause.Name = "btnPause";
            this.btnPause.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnPause.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnPause.NormalTextColor = System.Drawing.Color.Black;
            this.btnPause.PressBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnPause.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnPause.PressTextColor = System.Drawing.Color.White;
            this.btnPause.Size = new System.Drawing.Size(44, 36);
            this.btnPause.Style = MetroSet_UI.Design.Style.Light;
            this.btnPause.StyleManager = null;
            this.btnPause.TabIndex = 1;
            this.btnPause.ThemeAuthor = null;
            this.btnPause.ThemeName = null;
            this.tipMain.SetToolTip(this.btnPause, "Pause/Resume Download");
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnHTTPPlay
            // 
            this.btnHTTPPlay.BorderThickness = 0;
            this.btnHTTPPlay.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnHTTPPlay.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.btnHTTPPlay.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.btnHTTPPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnHTTPPlay.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnHTTPPlay.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnHTTPPlay.HoverTextColor = System.Drawing.Color.White;
            this.btnHTTPPlay.Image = global::PlexDL.Properties.Resources.baseline_rss_feed_black_18dp;
            imageSet6.Focus = global::PlexDL.Properties.Resources.baseline_rss_feed_black_18dp_white;
            imageSet6.Idle = global::PlexDL.Properties.Resources.baseline_rss_feed_black_18dp;
            this.btnHTTPPlay.ImageSet = imageSet6;
            this.btnHTTPPlay.ImageSize = new System.Drawing.Size(28, 28);
            this.btnHTTPPlay.Location = new System.Drawing.Point(130, 84);
            this.btnHTTPPlay.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnHTTPPlay.Name = "btnHTTPPlay";
            this.btnHTTPPlay.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnHTTPPlay.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnHTTPPlay.NormalTextColor = System.Drawing.Color.Black;
            this.btnHTTPPlay.PressBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnHTTPPlay.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnHTTPPlay.PressTextColor = System.Drawing.Color.White;
            this.btnHTTPPlay.Size = new System.Drawing.Size(44, 36);
            this.btnHTTPPlay.Style = MetroSet_UI.Design.Style.Light;
            this.btnHTTPPlay.StyleManager = null;
            this.btnHTTPPlay.TabIndex = 2;
            this.btnHTTPPlay.ThemeAuthor = null;
            this.btnHTTPPlay.ThemeName = null;
            this.tipMain.SetToolTip(this.btnHTTPPlay, "Stream Selected Title");
            this.btnHTTPPlay.Click += new System.EventHandler(this.btnHTTPPlay_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.BorderThickness = 0;
            this.btnConnect.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnConnect.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.btnConnect.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnConnect.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnConnect.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnConnect.HoverTextColor = System.Drawing.Color.White;
            this.btnConnect.Image = global::PlexDL.Properties.Resources.baseline_power_black_18dp;
            imageSet7.Focus = global::PlexDL.Properties.Resources.baseline_power_black_18dp_white;
            imageSet7.Idle = global::PlexDL.Properties.Resources.baseline_power_black_18dp;
            this.btnConnect.ImageSet = imageSet7;
            this.btnConnect.ImageSize = new System.Drawing.Size(28, 28);
            this.btnConnect.Location = new System.Drawing.Point(234, 84);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnConnect.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnConnect.NormalTextColor = System.Drawing.Color.Black;
            this.btnConnect.PressBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnConnect.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnConnect.PressTextColor = System.Drawing.Color.White;
            this.btnConnect.Size = new System.Drawing.Size(44, 36);
            this.btnConnect.Style = MetroSet_UI.Design.Style.Light;
            this.btnConnect.StyleManager = null;
            this.btnConnect.TabIndex = 4;
            this.btnConnect.ThemeAuthor = null;
            this.btnConnect.ThemeName = null;
            this.tipMain.SetToolTip(this.btnConnect, "Connect");
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.BorderThickness = 0;
            this.btnDownload.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnDownload.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.btnDownload.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.btnDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnDownload.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnDownload.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnDownload.HoverTextColor = System.Drawing.Color.White;
            this.btnDownload.Image = global::PlexDL.Properties.Resources.baseline_cloud_download_black_18dp;
            imageSet8.Focus = global::PlexDL.Properties.Resources.baseline_cloud_download_black_18dp_white;
            imageSet8.Idle = global::PlexDL.Properties.Resources.baseline_cloud_download_black_18dp;
            this.btnDownload.ImageSet = imageSet8;
            this.btnDownload.ImageSize = new System.Drawing.Size(28, 28);
            this.btnDownload.Location = new System.Drawing.Point(26, 84);
            this.btnDownload.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnDownload.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnDownload.NormalTextColor = System.Drawing.Color.Black;
            this.btnDownload.PressBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDownload.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDownload.PressTextColor = System.Drawing.Color.White;
            this.btnDownload.Size = new System.Drawing.Size(44, 36);
            this.btnDownload.Style = MetroSet_UI.Design.Style.Light;
            this.btnDownload.StyleManager = null;
            this.btnDownload.TabIndex = 0;
            this.btnDownload.ThemeAuthor = null;
            this.btnDownload.ThemeName = null;
            this.tipMain.SetToolTip(this.btnDownload, "Download Selected Title");
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // Home
            // 
            this.AllowResize = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 514);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.lnkLogViewer);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.pbMain);
            this.Controls.Add(this.btnMetadata);
            this.Controls.Add(this.btnSetDlDir);
            this.Controls.Add(this.dgvEpisodes);
            this.Controls.Add(this.dgvSeasons);
            this.Controls.Add(this.dgvContent);
            this.Controls.Add(this.dgvLibrary);
            this.Controls.Add(this.dgvServers);
            this.Controls.Add(this.lblLibraryContentSections);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.btnLoadProfile);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnHTTPPlay);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.lblLibraryContent);
            this.Controls.Add(this.lblLibrarySections);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PlexDL by BRH Media";
            this.ThemeAuthor = null;
            this.ThemeName = null;
            this.UseSlideAnimation = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.cxtEpisodes.ResumeLayout(false);
            this.cxtEpisodeOptions.ResumeLayout(false);
            this.cxtLibrarySections.ResumeLayout(false);
            this.cxtContentOptions.ResumeLayout(false);
            this.cxtProfile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEpisodes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSeasons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLibrary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.ComponentModel.BackgroundWorker wkrUpdateContentView;
        private System.Windows.Forms.SaveFileDialog sfdSaveProfile;
        public System.Windows.Forms.FolderBrowserDialog fbdSave;
        private System.Windows.Forms.OpenFileDialog ofdLoadProfile;
        private System.ComponentModel.BackgroundWorker wkrDownloadAsync;
        public MetroSet_UI.Controls.MetroSetEllipse btnDownload;
        private MetroSet_UI.Controls.MetroSetEllipse btnConnect;
        private MetroSet_UI.Controls.MetroSetEllipse btnHTTPPlay;
        private MetroSet_UI.Controls.MetroSetEllipse btnLoadProfile;
        private MetroSet_UI.Controls.MetroSetListBox lstLog;
        private MetroSet_UI.Controls.MetroSetEllipse btnSearch;
        public MetroSet_UI.Controls.MetroSetLabel lblLibraryContent;
        private System.Windows.Forms.ToolTip tipMain;
        public MetroSet_UI.Controls.MetroSetLabel lblLibraryContentSections;
        private MetroSet_UI.Controls.MetroSetEllipse btnPause;
        private libbrhscgui.Components.AbortableBackgroundWorker wkrGetMetadata;
        private PlexDL.Common.Components.FlatDataGridView dgvContent;
        private PlexDL.Common.Components.FlatDataGridView dgvSeasons;
        private PlexDL.Common.Components.FlatDataGridView dgvEpisodes;
        private MetroSet_UI.Controls.MetroSetEllipse btnSetDlDir;
        private MetroSet_UI.Controls.MetroSetContextMenuStrip cxtEpisodes;
        private System.Windows.Forms.ToolStripMenuItem itmDownloadThisEpisode;
        private System.Windows.Forms.ToolStripMenuItem itmDownloadAllEpisodes;
        private MetroSet_UI.Controls.MetroSetContextMenuStrip cxtLibrarySections;
        private System.Windows.Forms.ToolStripMenuItem itmManuallyLoadSection;
        private MetroSet_UI.Controls.MetroSetContextMenuStrip cxtEpisodeOptions;
        private System.Windows.Forms.ToolStripMenuItem itmEpisodeMetadata;
        private System.Windows.Forms.ToolStripMenuItem itmEpisodeDownload;
        private System.Windows.Forms.ToolStripMenuItem thisEpisodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thisSeasonToolStripMenuItem;
        private MetroSet_UI.Controls.MetroSetContextMenuStrip cxtContentOptions;
        private System.Windows.Forms.ToolStripMenuItem itmContentMetadata;
        private System.Windows.Forms.ToolStripMenuItem itmContentDownload;
        private MetroSet_UI.Controls.MetroSetEllipse btnMetadata;
        private System.Windows.Forms.OpenFileDialog ofdMetadata;
        private MetroSet_UI.Controls.MetroSetContextMenuStrip cxtProfile;
        private System.Windows.Forms.ToolStripMenuItem loadProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProfileToolStripMenuItem;
        private PlexDL.Common.Components.FlatDataGridView dgvLibrary;
        private PlexDL.Common.Components.FlatDataGridView dgvServers;
        private MetroSet_UI.Controls.MetroSetProgressBar pbMain;
        private MetroSet_UI.Controls.MetroSetLabel lblProgress;
        private MetroSet_UI.Controls.MetroSetLink lnkLogViewer;
        private MetroSet_UI.Controls.MetroSetLabel lblLog;
        public MetroSet_UI.Controls.MetroSetLabel lblLibrarySections;
    }
}

