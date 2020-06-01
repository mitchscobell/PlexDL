﻿using LogDel;
using PlexDL.AltoHTTP.Classes;
using PlexDL.Common;
using PlexDL.Common.API;
using PlexDL.Common.API.Metadata;
using PlexDL.Common.API.Objects;
using PlexDL.Common.Caching;
using PlexDL.Common.Components;
using PlexDL.Common.Enums;
using PlexDL.Common.Globals;
using PlexDL.Common.Logging;
using PlexDL.Common.PlayerLaunchers;
using PlexDL.Common.Renderers;
using PlexDL.Common.Renderers.DGVRenderers;
using PlexDL.Common.SearchFramework;
using PlexDL.Common.Structures;
using PlexDL.Common.Structures.AppOptions;
using PlexDL.Common.Structures.AppOptions.Player;
using PlexDL.Common.Structures.Plex;
using PlexDL.PlexAPI;
using PlexDL.WaitWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Directory = System.IO.Directory;

//using System.Threading.Tasks;

namespace PlexDL.UI
{
    public partial class Home : Form
    {
        #region Form

        #region FormInitialiser

        public Home()
        {
            InitializeComponent();
            tabMain.SelectedIndex = 0;
        }

        #endregion FormInitialiser

        #endregion Form

        /*private void ManualSectionLoad()
        {
            if (dgvSections.Rows.Count > 0)
            {
                var ipt = GlobalStaticVars.LibUI.showInputForm("Enter section key", "Manual Section Lookup", true, "TV Library");
                if (ipt.txt == "!cancel=user")
                    return;
                if (!int.TryParse(ipt.txt, out _))
                    MessageBox.Show(@"Section key ust be numeric", @"Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                else
                    UpdateFromLibraryKey(ipt.txt, ipt.chkd);
            }
        }*/

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((dgvMovies.SelectedRows.Count == 1 || dgvEpisodes.SelectedRows.Count == 1) && GlobalStaticVars.Settings.Generic.DoubleClickLaunch)
            {
                if (keyData == Keys.Enter)
                {
                    DoubleClickLaunch();
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ItmDownloadThisEpisode_Click(object sender, EventArgs e)
        {
            cxtEpisodes.Close();
            DoDownloadSelected();
        }

        private void ItmDownloadAllEpisodes_Click(object sender, EventArgs e)
        {
            cxtEpisodes.Close();
            DoDownloadAll();
        }

        private void ItmManuallyLoadSection_Click(object sender, EventArgs e)
        {
            cxtLibrarySections.Close();
            //ManualSectionLoad();
        }

        private void ItmEpisodeMetadata_Click(object sender, EventArgs e)
        {
            cxtEpisodeOptions.Close();
            Metadata();
        }

        private void ItmDGVDownloadThisEpisode_Click(object sender, EventArgs e)
        {
            cxtEpisodeOptions.Close();
            DoDownloadSelected();
        }

        private void ItmDGVDownloadThisSeason_Click(object sender, EventArgs e)
        {
            cxtEpisodeOptions.Close();
            DoDownloadAll();
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            cxtContentOptions.Close();
            Metadata();
        }

        private void ItmContentDownload_Click(object sender, EventArgs e)
        {
            cxtContentOptions.Close();
            DoDownloadSelected();
        }

        private void NfyMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //send this form to the front of the screen
            TopMost = true;
            TopMost = false;
        }

        private void ItmStreamInPVS_Click(object sender, EventArgs e)
        {
            cxtStreamOptions.Close();
            PvsLauncher.LaunchPvs(GlobalStaticVars.CurrentStream, GlobalTables.ReturnCorrectTable());
        }

        private void ItmStreamInVLC_Click(object sender, EventArgs e)
        {
            cxtStreamOptions.Close();
            VlcLauncher.LaunchVlc(GlobalStaticVars.CurrentStream);
        }

        private void ItmStreamInBrowser_Click(object sender, EventArgs e)
        {
            cxtStreamOptions.Close();
            BrowserLauncher.LaunchBrowser(GlobalStaticVars.CurrentStream);
        }

        private void DoConnectFromServer(Server s)
        {
            var address = s.address;
            var port = s.port;

            var connectInfo = new ConnectionInfo
            {
                PlexAccountToken = GlobalStaticVars.GetToken(),
                PlexAddress = address,
                PlexPort = port,
                RelaysOnly = GlobalStaticVars.Settings.ConnectionInfo.RelaysOnly
            };

            GlobalStaticVars.Settings.ConnectionInfo = connectInfo;

            var uri = GlobalStaticVars.GetBaseUri(true);
            //MessageBox.Show(uri);
            var reply = (XmlDocument)WaitWindow.WaitWindow.Show(XmlGet.GetXMLTransactionWorker, "Connecting", uri);
            Flags.IsConnected = true;

            if (GlobalStaticVars.Settings.Generic.ShowConnectionSuccess)
                MessageBox.Show(@"Connection successful!", @"Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            SetProgressLabel("Connected");
            SetDisconnect();

            if (reply.ChildNodes.Count != 0)
                PopulateLibrary(reply);
        }

        private void itmServerManager_Click(object sender, EventArgs e)
        {
            if (wininet.CheckForInternetConnection())
                using (var frm = new ServerManager())
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        GlobalStaticVars.Settings.ConnectionInfo.PlexAccountToken = frm.SelectedServer.accessToken;
                        GlobalStaticVars.Settings.ConnectionInfo.PlexAddress = frm.SelectedServer.address;
                        GlobalStaticVars.Settings.ConnectionInfo.PlexPort = frm.SelectedServer.port;
                        GlobalStaticVars.Svr = frm.SelectedServer;
                        DoConnectFromServer(frm.SelectedServer);
                    }
                }
            else
                MessageBox.Show(@"No internet connection. Please connect to a network before attempting to start a Plex server connection.",
                    @"Network Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void itmStartSearch_Click(object sender, EventArgs e)
        {
            SearchProcedure();
        }

        private void itmLoadProfile_Click(object sender, EventArgs e)
        {
            LoadProfile();
        }

        private void itmSaveProfile_Click(object sender, EventArgs e)
        {
            SaveProfile();
        }

        private void itmExportObj_Click(object sender, EventArgs e)
        {
            if (Flags.IsConnected)
                DoStreamExport();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if ((dgvMovies.SelectedRows.Count == 1) || (dgvEpisodes.SelectedRows.Count == 1) || (dgvTracks.SelectedRows.Count == 1))
            {
                if (!Flags.IsDownloadRunning && !Flags.IsEngineRunning)
                {
                    GlobalStaticVars.Queue = new List<DownloadInfo>();
                    switch (GlobalStaticVars.CurrentContentType)
                    {
                        case ContentType.TvShows:
                            if (dgvEpisodes.SelectedRows.Count == 1) cxtEpisodes.Show(MousePosition);
                            break;

                        case ContentType.Movies:
                            DoDownloadSelected();
                            break;

                        case ContentType.Music:
                            if (dgvTracks.SelectedRows.Count == 1) cxtTracks.Show(MousePosition);
                            break;
                    }
                }
                else
                {
                    CancelDownload();
                }
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (Flags.IsDownloadRunning && Flags.IsEngineRunning)
            {
                if (!Flags.IsDownloadPaused)
                {
                    GlobalStaticVars.Engine.Pause();
                    SetResume();
                    SetProgressLabel(lblProgress.Text + " (Paused)");
                    Flags.IsDownloadPaused = true;
                }
                else
                {
                    GlobalStaticVars.Engine.ResumeAsync();
                    SetPause();
                    Flags.IsDownloadPaused = false;
                }
            }
        }

        private void itmSetDlDirectory_Click(object sender, EventArgs e)
        {
            SetDownloadDirectory();
        }

        private void btnHTTPPlay_Click(object sender, EventArgs e)
        {
            try
            {
                if (Flags.IsConnected && Flags.IsLibraryFilled)
                {
                    PlexObject result = null;
                    switch (GlobalStaticVars.CurrentContentType)
                    {
                        case ContentType.Movies:
                            if (dgvMovies.SelectedRows.Count == 1)
                                result = (PlexMovie)WaitWindow.WaitWindow.Show(GetMovieObjectFromSelectionWorker,
                                "Getting Metadata", new object[] { false });
                            else
                            {
                                ShowError(@"No movie is selected", @"Validation Error");
                                return;
                            }
                            break;

                        case ContentType.TvShows:
                            if (dgvEpisodes.SelectedRows.Count == 1)
                                result = (PlexTvShow)WaitWindow.WaitWindow.Show(GetTVObjectFromSelectionWorker,
                                    "Getting Metadata", new object[] { false });
                            else
                            {
                                ShowError(@"No episode is selected", @"Validation Error");
                                return;
                            }
                            break;

                        case ContentType.Music:
                            if (dgvTracks.SelectedRows.Count == 1)
                            {
                                result = (PlexMusic)WaitWindow.WaitWindow.Show(GetMusicObjectFromSelectionWorker,
                                    "Getting Metadata", new object[] { false });
                            }
                            else
                            {
                                ShowError(@"No track is selected", @"Validation Error");
                            }
                            break;
                    }

                    if (result != null)
                        StartStreaming(result);
                    else
                    {
                        LoggingHelpers.RecordGenericEntry("Couldn't start streaming; object was null.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "StartStreamError");
                LoggingHelpers.RecordGenericEntry("Couldn't start streaming; an error occured.");
                ShowError("Streaming preparation error occurred:\n\n" + ex, "Start Stream Error");
            }
        }

        private void tlpMain_Paint(object sender, PaintEventArgs e)
        {
        }

        private void itmMetadata_Click(object sender, EventArgs e)
        {
            Metadata();
        }

        private void itmLogViewer_Click(object sender, EventArgs e)
        {
            ShowLogViewer();
        }

        private void itmDisconnect_Click(object sender, EventArgs e)
        {
            if (Flags.IsConnected)
                Disconnect();
        }

        private void itmAbout_Click(object sender, EventArgs e)
        {
            using (var frm = new About())
            {
                frm.ShowDialog();
            }
        }

        private void itmCacheMetrics_Click(object sender, EventArgs e)
        {
            using (var frm = new CachingMetricsUi())
            {
                frm.Metrics = CachingMetrics.FromLatest();
                frm.ShowDialog();
            }
        }

        private void itmSettings_Click(object sender, EventArgs e)
        {
            using (var frm = new Settings())
            {
                frm.ShowDialog();
            }
        }

        private void itmClearCache_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(CachingFileDir.RootCacheDirectory))
                {
                    var result = MessageBox.Show(@"Are you sure you want to clear the cache?", @"Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result != DialogResult.Yes) return;
                    Directory.Delete(CachingFileDir.RootCacheDirectory, true);
                    MessageBox.Show(@"Successfully deleted cached data", @"Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ShowError(@"There's no cached data to clear", @"Validation Error");
                }
            }
            catch (Exception ex)
            {
                // ReSharper disable once LocalizableElement
                MessageBox.Show("Error whilst trying to delete cached data:\n\n" + ex.Message);
                LoggingHelpers.RecordException(ex.Message, "ClearCacheError");
            }
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabMain.SelectedTab == tabLog)
            {
                if (File.Exists(@"Logs\PlexDL.logdel"))
                    dgvLog.DataSource = LogReader.TableFromFile(@"Logs\PlexDL.logdel", false);
                else
                    dgvLog.DataSource = null;
            }
            else
            {
                dgvLog.DataSource = null;
            }
        }

        private void tmrWorkerTimeout_Tick(object sender, EventArgs e)
        {
            try
            {
                //we only need this timer to run once - so stop it once the
                //tick interval is reached.
                tmrWorkerTimeout.Stop();

                //check if we're still waiting for the worker to start doing
                //something
                if (string.Equals(lblProgress.Text.ToLower(), "waiting"))
                {
                    //it's still waiting; kill the worker thread.
                    if (wkrGetMetadata.IsBusy)
                        wkrGetMetadata.Abort();
                    //tell the user that the worker timed out
                    ShowError(@"Failed to get metadata; the worker timed out.", @"Data Error");

                    //cancel the download silently and with a custom log
                    //and label input
                    CancelDownload(true, "Worker Timeout");
                }
            }
            catch (ThreadAbortException)
            {
                //nothing; triggering AbortableBackgroundWorker.Abort() might
                //trigger this exception, so just ignore it - it's not a serious
                //issue.
            }
            catch (Exception ex)
            {
                //log and then ignore
                LoggingHelpers.RecordException(ex.Message, "WkrMetadataTimerError");
                CancelDownload(true, "Worker Timeout");
            }
        }

        #region GlobalIntVariables

        public int DownloadIndex;
        public int DownloadTotal;
        public int DownloadsSoFar;

        #endregion GlobalIntVariables

        #region XML/Metadata

        #region PlexMovieBuilders

        private PlexMovie GetMovieObjectFromSelection(bool formatLinkDownload)
        {
            var obj = new PlexMovie();
            if (dgvMovies.SelectedRows.Count == 1)
            {
                var index = GlobalTables.GetTableIndexFromDgv(dgvMovies);
                obj = ObjectBuilders.GetMovieObjectFromIndex(index, formatLinkDownload);
            }

            return obj;
        }

        private PlexTvShow GetTvObjectFromSelection(bool formatLinkDownload)
        {
            var obj = new PlexTvShow();
            if (dgvTVShows.SelectedRows.Count == 1 && dgvEpisodes.SelectedRows.Count == 1)
            {
                var index = GlobalTables.GetTableIndexFromDgv(dgvEpisodes, GlobalTables.EpisodesTable);
                obj = ObjectBuilders.GetTvObjectFromIndex(index, formatLinkDownload);
            }

            return obj;
        }

        private PlexMusic GetMusicObjectFromSelection(bool formatLinkDownload)
        {
            var obj = new PlexMusic();
            if (dgvArtists.SelectedRows.Count == 1 && dgvTracks.SelectedRows.Count == 1)
            {
                var index = GlobalTables.GetTableIndexFromDgv(dgvTracks, GlobalTables.TracksTable);
                obj = ObjectBuilders.GetMusicObjectFromIndex(index, formatLinkDownload);
            }

            return obj;
        }

        #endregion PlexMovieBuilders

        #endregion XML/Metadata

        #region ProfileHelpers

        public void LoadProfile()
        {
            if (!Flags.IsConnected)
            {
                if (ofdLoadProfile.ShowDialog() == DialogResult.OK) DoLoadProfile(ofdLoadProfile.FileName);
            }
            else
            {
                MessageBox.Show(@"You can't load profiles while you're connected; please disconnect first.",
                    @"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void SaveProfile()
        {
            if (string.IsNullOrEmpty(GlobalStaticVars.Settings.ConnectionInfo.PlexAccountToken))
            {
                MessageBox.Show(@"You need to authenticate before saving a profile", @"Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (sfdSaveProfile.ShowDialog() == DialogResult.OK) DoSaveProfile(sfdSaveProfile.FileName);
            }
        }

        public void DoSaveProfile(string fileName, bool silent = false)
        {
            try
            {
                ProfileImportExport.ProfileToFile(fileName, GlobalStaticVars.Settings, silent);

                if (!silent)
                    MessageBox.Show(@"Successfully saved profile!", @"Message", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                LoggingHelpers.RecordGenericEntry(@"Saved profile " + fileName);
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "@SaveProfileError");
                if (!silent)
                    ShowError(ex.ToString(), @"Error in saving XML Profile");
            }
        }

        public void DoLoadProfile(string fileName, bool silent = false)
        {
            try
            {
                ApplicationOptions subReq = ProfileImportExport.ProfileFromFile(fileName, silent);

                try
                {
                    var vStoredVersion = new Version(subReq.Generic.StoredAppVersion);
                    var vThisVersion = new Version(Application.ProductVersion);
                    var vCompare = vThisVersion.CompareTo(vStoredVersion);
                    if (vCompare < 0)
                    {
                        if (!silent)
                        {
                            var result = MessageBox.Show(
                                "You're trying to load a profile made in a newer version of PlexDL. This could have several implications:\n" +
                                "- Possible data loss of your current profile if PlexDL overwrites it\n" +
                                "- Features may not work as intended\n" +
                                "- Increased risk of errors\n\n" +
                                "Press 'OK' to continue loading, or 'Cancel' to stop loading.", "Warning", MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Exclamation);
                            if (result == DialogResult.Cancel)
                                return;
                        }

                        LoggingHelpers.RecordGenericEntry("Tried to load a profile made in a newer version: " + vStoredVersion + " > " + vThisVersion);
                    }
                    else if (vCompare > 0)
                    {
                        if (!silent)
                        {
                            var result = MessageBox.Show(
                                "You're trying to load a profile made in an earlier version of PlexDL. This could have several implications:\n" +
                                "- Possible data loss of your current profile if PlexDL overwrites it\n" +
                                "- Features may not work as intended\n" +
                                "- Increased risk of errors\n\n" +
                                "Press 'OK' to continue loading, or 'Cancel' to stop loading.", "Warning", MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Exclamation);
                            if (result == DialogResult.Cancel)
                                return;
                        }

                        LoggingHelpers.RecordGenericEntry("Tried to load a profile made in an earlier version: " + vStoredVersion + " < " + vThisVersion);
                    }
                }
                catch (Exception ex)
                {
                    LoggingHelpers.RecordGenericEntry("Version information load error: " + ex.Message);
                    LoggingHelpers.RecordException(ex.Message, "VersionLoadError");
                }

                if (subReq != null)
                {
                    GlobalStaticVars.Settings = subReq;

                    if (!silent)
                        MessageBox.Show("Successfully loaded profile!", "Message", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    LoggingHelpers.RecordGenericEntry("Loaded profile " + fileName);
                }
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "LoadProfileError");
                if (!silent)
                    ShowError(ex.ToString(), "Error in loading XML Profile");
                return;
            }
        }

        #endregion ProfileHelpers

        #region ConnectionHelpers

        private void Disconnect(bool silent = false)
        {
            if (Flags.IsConnected)
            {
                if (GlobalStaticVars.Engine != null) CancelDownload();
                ClearContentView();
                ClearTVViews();
                ClearMusicViews();
                ClearLibraryViews();
                SetProgressLabel(@"Disconnected from Plex");
                SetConnect();
                SelectMoviesTab();
                Flags.IsConnected = false;

                if (!silent)
                    MessageBox.Show(@"Disconnected from Plex", @"Message", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
            }
        }

        private void DoStreamExport(bool formatLinkDownload = true)
        {
            try
            {
                if ((dgvMovies.SelectedRows.Count == 1) || (dgvEpisodes.SelectedRows.Count == 1) || (dgvTracks.SelectedRows.Count == 1))
                {
                    PlexObject content = null;
                    switch (GlobalStaticVars.CurrentContentType)
                    {
                        case ContentType.Movies:
                            content = GetMovieObjectFromSelection(formatLinkDownload);
                            break;

                        case ContentType.TvShows:
                            content = GetTvObjectFromSelection(formatLinkDownload);
                            break;

                        case ContentType.Music:
                            content = GetMusicObjectFromSelection(formatLinkDownload);
                            break;
                    }

                    if (sfdExport.ShowDialog() == DialogResult.OK)
                        ImportExport.MetadataToFile(sfdExport.FileName, content);
                }
            }
            catch (Exception ex)
            {
                //log and ignore
                LoggingHelpers.RecordGenericEntry("Export error: " + ex.Message);
                LoggingHelpers.RecordException(ex.Message, "StreamExportError");
            }
        }

        #endregion ConnectionHelpers

        #region Workers

        #region UpdateWorkers

        private void PopulateLibraryWorker(XmlDocument doc)
        {
            if (doc != null)
                try
                {
                    LoggingHelpers.RecordGenericEntry("Library population requested");
                    var libraryDir = KeyGatherers.GetLibraryKey(doc).TrimEnd('/');
                    var baseUri = GlobalStaticVars.GetBaseUri(false);
                    var uriSectionKey = baseUri + libraryDir + "/?X-Plex-Token=";
                    var xmlSectionKey = XmlGet.GetXmlTransaction(uriSectionKey);

                    var sectionDir = KeyGatherers.GetSectionKey(xmlSectionKey).TrimEnd('/');
                    var uriSections = baseUri + libraryDir + "/" + sectionDir + "/?X-Plex-Token=";
                    var xmlSections = XmlGet.GetXmlTransaction(uriSections);

                    LoggingHelpers.RecordGenericEntry("Creating new datasets");
                    var sections = new DataSet();
                    sections.ReadXml(new XmlNodeReader(xmlSections));

                    DataTable sectionsTable;
                    sectionsTable = sections.Tables["Directory"];
                    GlobalTables.SectionsTable = sectionsTable;

                    LoggingHelpers.RecordGenericEntry("Binding to grid");
                    RenderLibraryView(sectionsTable);
                    Flags.IsLibraryFilled = true;
                    GlobalStaticVars.CurrentApiUri = baseUri + libraryDir + "/" + sectionDir + "/";
                    //we can render the content view if a row is already selected
                }
                catch (WebException ex)
                {
                    LoggingHelpers.RecordException(ex.Message, "LibPopError");
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                        if (ex.Response is HttpWebResponse response)
                            switch ((int)response.StatusCode)
                            {
                                case 401:
                                    ShowError(
                                        @"The web server denied access to the resource. Check your token and try again. (401)");
                                    break;

                                case 404:
                                    ShowError(
                                        @"The web server couldn't serve the request because it couldn't find the resource specified. (404)");
                                    break;

                                case 400:
                                    ShowError(
                                        @"The web server couldn't serve the request because the request was bad. (400)");
                                    break;
                            }
                }
                catch (Exception ex)
                {
                    LoggingHelpers.RecordException(ex.Message, "LibPopError");
                    ShowError(ex.ToString());
                }
        }

        private void GetTitlesTable(XmlDocument doc, ContentType type)
        {
            var sections = new DataSet();
            sections.ReadXml(new XmlNodeReader(doc));

            switch (type)
            {
                case ContentType.TvShows:
                    GlobalTables.TitlesTable = sections.Tables["Directory"];
                    break;

                case ContentType.Music:
                    GlobalTables.TitlesTable = sections.Tables["Directory"];
                    break;

                case ContentType.Movies:
                    GlobalTables.TitlesTable = sections.Tables["Video"];
                    break;
            }
        }

        private void UpdateContentViewWorker(XmlDocument doc, ContentType type)
        {
            LoggingHelpers.RecordGenericEntry("Updating library contents");

            GetTitlesTable(doc, type);

            if (GlobalTables.TitlesTable != null)
            {
                //set this in the toolstrip so the user can see how many items are loaded
                lblViewingValue.Text = GlobalTables.TitlesTable.Rows.Count + "/" + GlobalTables.TitlesTable.Rows.Count;

                GlobalStaticVars.CurrentContentType = type;

                //MessageBox.Show(GlobalStaticVars.CurrentContentType.ToString());

                switch (type)
                {
                    case ContentType.Movies:
                        LoggingHelpers.RecordGenericEntry("Rendering Movies");
                        RenderMoviesView(GlobalTables.TitlesTable);
                        break;

                    case ContentType.TvShows:
                        LoggingHelpers.RecordGenericEntry("Rendering TV Shows");
                        RenderTVView(GlobalTables.TitlesTable);
                        break;

                    case ContentType.Music:
                        LoggingHelpers.RecordGenericEntry("Rendering Artists");
                        RenderArtistsView(GlobalTables.TitlesTable);
                        break;
                }

                //MessageBox.Show("ContentTable: " + contentTable.Rows.Count.ToString() + "\nTitlesTable: " + GlobalTables.TitlesTable.Rows.Count.ToString());
            }
            else
            {
                LoggingHelpers.RecordGenericEntry("Library contents were null; rendering did not occur");
            }
        }

        private void UpdateEpisodeViewWorker(XmlDocument doc)
        {
            LoggingHelpers.RecordGenericEntry("Updating episode contents");

            LoggingHelpers.RecordGenericEntry("Creating datasets");
            var sections = new DataSet();
            sections.ReadXml(new XmlNodeReader(doc));

            GlobalTables.EpisodesTable = sections.Tables["Video"];

            LoggingHelpers.RecordGenericEntry("Cleaning unwanted data");

            LoggingHelpers.RecordGenericEntry("Binding to grid");
            RenderEpisodesView(GlobalTables.EpisodesTable);

            //MessageBox.Show("ContentTable: " + contentTable.Rows.Count.ToString() + "\nTitlesTable: " + GlobalTables.TitlesTable.Rows.Count.ToString());
        }

        private void UpdateTracksViewWorker(XmlDocument doc)
        {
            LoggingHelpers.RecordGenericEntry("Updating track contents");

            LoggingHelpers.RecordGenericEntry("Creating datasets");
            var sections = new DataSet();
            sections.ReadXml(new XmlNodeReader(doc));

            GlobalTables.TracksTable = sections.Tables["Track"];

            LoggingHelpers.RecordGenericEntry("Cleaning unwanted data");

            LoggingHelpers.RecordGenericEntry("Binding to grid");
            RenderTracksView(GlobalTables.TracksTable);

            //MessageBox.Show("ContentTable: " + contentTable.Rows.Count.ToString() + "\nTitlesTable: " + GlobalTables.TitlesTable.Rows.Count.ToString());
        }

        private void UpdateSeriesViewWorker(XmlDocument doc)
        {
            LoggingHelpers.RecordGenericEntry("Updating series contents");

            LoggingHelpers.RecordGenericEntry("Creating datasets");
            var sections = new DataSet();
            sections.ReadXml(new XmlNodeReader(doc));

            GlobalTables.SeasonsTable = sections.Tables["Directory"];

            LoggingHelpers.RecordGenericEntry("Cleaning unwanted data");

            LoggingHelpers.RecordGenericEntry("Binding to grid");
            RenderSeasonsView(GlobalTables.SeasonsTable);

            //MessageBox.Show("ContentTable: " + contentTable.Rows.Count.ToString() + "\nTitlesTable: " + GlobalTables.TitlesTable.Rows.Count.ToString());
        }

        private void UpdateAlbumsViewWorker(XmlDocument doc)
        {
            LoggingHelpers.RecordGenericEntry("Updating album contents");

            LoggingHelpers.RecordGenericEntry("Creating datasets");
            var sections = new DataSet();
            sections.ReadXml(new XmlNodeReader(doc));

            GlobalTables.AlbumsTable = sections.Tables["Directory"];

            LoggingHelpers.RecordGenericEntry("Cleaning unwanted data");

            LoggingHelpers.RecordGenericEntry("Binding to grid");
            RenderAlbumsView(GlobalTables.AlbumsTable);

            //MessageBox.Show("ContentTable: " + contentTable.Rows.Count.ToString() + "\nTitlesTable: " + GlobalTables.TitlesTable.Rows.Count.ToString());
        }

        #endregion UpdateWorkers

        #region BackgroundWorkers

        private void WkrGrabTV()
        {
            if (Flags.IsDownloadAll)
            {
                LoggingHelpers.RecordGenericEntry(@"Worker is to grab metadata for All Episodes");

                var rowCount = GlobalTables.EpisodesTable.Rows.Count;

                for (int i = 0; i < rowCount; i++)
                {
                    SetProgressLabel(@"Getting Metadata " + (i + 1) + @"/" + rowCount);

                    var show = ObjectBuilders.GetTvObjectFromIndex(i, true);
                    var dlInfo = show.StreamInformation;
                    var dir = DownloadLayout.CreateDownloadLayoutTVShow(show, GlobalStaticVars.Settings,
                        DownloadLayout.PlexStandardLayout);
                    dlInfo.DownloadPath = dir.SeasonPath;
                    GlobalStaticVars.Queue.Add(dlInfo);
                }
            }
            else
            {
                LoggingHelpers.RecordGenericEntry(@"Worker is to grab Single Episode metadata");

                SetProgressLabel(@"Getting Metadata 1/1");

                var show = GetTvObjectFromSelection(true);
                var dlInfo = show.StreamInformation;
                var dir = DownloadLayout.CreateDownloadLayoutTVShow(show, GlobalStaticVars.Settings,
                    DownloadLayout.PlexStandardLayout);
                dlInfo.DownloadPath = dir.SeasonPath;
                GlobalStaticVars.Queue.Add(dlInfo);
            }
        }

        private void WkrGrabMusic()
        {
            if (Flags.IsDownloadAll)
            {
                LoggingHelpers.RecordGenericEntry(@"Worker is to grab metadata for All Tracks");

                var rowCount = GlobalTables.TracksTable.Rows.Count;

                for (int i = 0; i < rowCount; i++)
                {
                    SetProgressLabel(@"Getting Metadata " + (i + 1) + @"/" + rowCount);

                    var track = ObjectBuilders.GetMusicObjectFromIndex(i, true);
                    var dlInfo = track.StreamInformation;
                    var dir = DownloadLayout.CreateDownloadLayoutMusic(track, GlobalStaticVars.Settings,
                        DownloadLayout.PlexStandardLayout);
                    dlInfo.DownloadPath = dir.AlbumPath;
                    GlobalStaticVars.Queue.Add(dlInfo);
                }
            }
            else
            {
                LoggingHelpers.RecordGenericEntry(@"Worker is to grab Single Track metadata");

                SetProgressLabel(@"Getting Metadata 1/1");

                var track = GetMusicObjectFromSelection(true);
                var dlInfo = track.StreamInformation;
                var dir = DownloadLayout.CreateDownloadLayoutMusic(track, GlobalStaticVars.Settings,
                    DownloadLayout.PlexStandardLayout);
                dlInfo.DownloadPath = dir.AlbumPath;
                GlobalStaticVars.Queue.Add(dlInfo);
            }
        }

        private void WkrGrabMovie()
        {
            SetProgressLabel(@"Getting Metadata 1/1");

            var movie = GetMovieObjectFromSelection(true);
            var dlInfo = movie.StreamInformation;
            dlInfo.DownloadPath = GlobalStaticVars.Settings.Generic.DownloadDirectory + @"\Movies";
            GlobalStaticVars.Queue.Add(dlInfo);
        }

        private void WkrGetMetadata_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //set needed globals
                GlobalStaticVars.Engine = new DownloadQueue();
                GlobalStaticVars.Queue = new List<DownloadInfo>();

                LoggingHelpers.RecordGenericEntry(@"Metadata worker started");
                LoggingHelpers.RecordGenericEntry(@"Doing directory checks");

                if (string.IsNullOrEmpty(GlobalStaticVars.Settings.Generic.DownloadDirectory) ||
                    string.IsNullOrWhiteSpace(GlobalStaticVars.Settings.Generic.DownloadDirectory)) ResetDownloadDirectory();

                LoggingHelpers.RecordGenericEntry(@"Grabbing metadata");

                switch (GlobalStaticVars.CurrentContentType)
                {
                    case ContentType.TvShows:
                        LoggingHelpers.RecordGenericEntry(@"Worker is to grab TV Show metadata");
                        WkrGrabTV();
                        break;

                    case ContentType.Movies:
                        LoggingHelpers.RecordGenericEntry(@"Worker is to grab Movie metadata");
                        WkrGrabMovie();
                        break;

                    case ContentType.Music:
                        LoggingHelpers.RecordGenericEntry(@"Worker is to grab Music metadata");
                        WkrGrabMusic();
                        break;
                }

                LoggingHelpers.RecordGenericEntry("Worker is to invoke downloader thread");

                BeginInvoke((MethodInvoker)delegate
                {
                    StartDownload(GlobalStaticVars.Queue, GlobalStaticVars.Settings.Generic.DownloadDirectory);
                    LoggingHelpers.RecordGenericEntry("Worker has started the download process");
                });
            }
            catch (Exception ex)
            {
                SetProgressLabel(@"Errored - Check Log");
                ShowError("Error occurred whilst getting needed metadata:\n\n" + ex);
                LoggingHelpers.RecordException(ex.Message, "MetadataWkrError");
                return;
            }
        }

        #endregion BackgroundWorkers

        #region UpdateCallbackWorkers

        private void WorkerUpdateContentView(object sender, WaitWindowEventArgs e)
        {
            var doc = (XmlDocument)e.Arguments[0];
            UpdateContentViewWorker(doc, (ContentType)e.Arguments[1]);
        }

        private void WorkerRenderMoviesView(object sender, WaitWindowEventArgs e)
        {
            var t = (DataTable)e.Arguments[0];
            RenderMoviesView(t);
        }

        private void WorkerRenderTVView(object sender, WaitWindowEventArgs e)
        {
            var t = (DataTable)e.Arguments[0];
            RenderTVView(t);
        }

        private void WorkerUpdateLibraryView(object sender, WaitWindowEventArgs e)
        {
            var doc = (XmlDocument)e.Arguments[0];
            PopulateLibraryWorker(doc);
        }

        private void WorkerUpdateSeriesView(object sender, WaitWindowEventArgs e)
        {
            var doc = (XmlDocument)e.Arguments[0];
            UpdateSeriesViewWorker(doc);
        }

        private void WorkerUpdateEpisodesView(object sender, WaitWindowEventArgs e)
        {
            var doc = (XmlDocument)e.Arguments[0];
            UpdateEpisodeViewWorker(doc);
        }

        private void WorkerUpdateTracksView(object sender, WaitWindowEventArgs e)
        {
            var doc = (XmlDocument)e.Arguments[0];
            UpdateTracksViewWorker(doc);
        }

        private void WorkerUpdateAlbumsView(object sender, WaitWindowEventArgs e)
        {
            var doc = (XmlDocument)e.Arguments[0];
            UpdateAlbumsViewWorker(doc);
        }

        #endregion UpdateCallbackWorkers

        #region ContentRenderers

        private void RenderMoviesView(DataTable content)
        {
            if (!(content == null))
            {
                ClearTVViews();
                ClearContentView();
                ClearMusicViews();

                var wantedColumns = GlobalStaticVars.Settings.DataDisplay.MoviesView.DisplayColumns;
                var wantedCaption = GlobalStaticVars.Settings.DataDisplay.MoviesView.DisplayCaptions;

                var info = new RenderStruct
                {
                    Data = content,
                    WantedColumns = wantedColumns,
                    WantedCaption = wantedCaption
                };

                GlobalViews.MoviesViewTable = GenericRenderer.RenderView(info, dgvMovies);

                SelectMoviesTab();
            }
        }

        private void SelectMoviesTab()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    tabMain.SelectedTab = tabMovies;
                });
            }
            else
            {
                tabMain.SelectedTab = tabMovies;
            }
        }

        private void SelectTVTab()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    tabMain.SelectedTab = tabTV;
                });
            }
            else
            {
                tabMain.SelectedTab = tabTV;
            }
        }

        private void SelectMusicTab()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    tabMain.SelectedTab = tabMusic;
                });
            }
            else
            {
                tabMain.SelectedTab = tabMusic;
            }
        }

        private void ClearContentView()
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate
                {
                    dgvMovies.DataSource = null;
                });
            else
                dgvMovies.DataSource = null;
        }

        private void ClearTVViews()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    dgvSeasons.DataSource = null;
                    dgvEpisodes.DataSource = null;
                    dgvTVShows.DataSource = null;
                });
            }
            else
            {
                dgvSeasons.DataSource = null;
                dgvEpisodes.DataSource = null;
                dgvTVShows.DataSource = null;
            }
        }

        private void ClearLibraryViews()
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate
                {
                    dgvSections.DataSource = null;
                });
            else
                dgvSections.DataSource = null;
        }

        private void ClearMusicViews()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    dgvArtists.DataSource = null;
                    dgvAlbums.DataSource = null;
                    dgvTracks.DataSource = null;
                });
            }
            else
            {
                dgvArtists.DataSource = null;
                dgvAlbums.DataSource = null;
                dgvTracks.DataSource = null;
            }
        }

        private void RenderTVView(DataTable content)
        {
            if (content != null)
            {
                ClearTVViews();
                ClearContentView();
                ClearMusicViews();

                var wantedColumns = GlobalStaticVars.Settings.DataDisplay.TvView.DisplayColumns;
                var wantedCaption = GlobalStaticVars.Settings.DataDisplay.TvView.DisplayCaptions;

                var info = new RenderStruct
                {
                    Data = content,
                    WantedColumns = wantedColumns,
                    WantedCaption = wantedCaption
                };

                GlobalViews.TvViewTable = GenericRenderer.RenderView(info, dgvTVShows);

                SelectTVTab();
            }
        }

        private void RenderArtistsView(DataTable content)
        {
            if (content != null)
            {
                ClearTVViews();
                ClearContentView();
                ClearMusicViews();

                var wantedColumns = GlobalStaticVars.Settings.DataDisplay.ArtistsView.DisplayColumns;
                var wantedCaption = GlobalStaticVars.Settings.DataDisplay.ArtistsView.DisplayCaptions;

                var info = new RenderStruct
                {
                    Data = content,
                    WantedColumns = wantedColumns,
                    WantedCaption = wantedCaption
                };

                GlobalViews.ArtistViewTable = GenericRenderer.RenderView(info, dgvArtists);

                SelectMusicTab();
            }
        }

        private void RenderSeasonsView(DataTable content)
        {
            if (content != null)
            {
                var wantedColumns = GlobalStaticVars.Settings.DataDisplay.SeriesView.DisplayColumns;
                var wantedCaption = GlobalStaticVars.Settings.DataDisplay.SeriesView.DisplayCaptions;

                var info = new RenderStruct
                {
                    Data = content,
                    WantedColumns = wantedColumns,
                    WantedCaption = wantedCaption
                };

                GlobalViews.SeasonsViewTable = GenericRenderer.RenderView(info, dgvSeasons);
            }
        }

        private void RenderEpisodesView(DataTable content)
        {
            if (content != null)
            {
                var wantedColumns = GlobalStaticVars.Settings.DataDisplay.EpisodesView.DisplayColumns;
                var wantedCaption = GlobalStaticVars.Settings.DataDisplay.EpisodesView.DisplayCaptions;

                var info = new RenderStruct
                {
                    Data = content,
                    WantedColumns = wantedColumns,
                    WantedCaption = wantedCaption
                };

                GlobalViews.EpisodesViewTable = GenericRenderer.RenderView(info, dgvEpisodes);
            }
        }

        private void RenderAlbumsView(DataTable content)
        {
            if (content != null)
            {
                var wantedColumns = GlobalStaticVars.Settings.DataDisplay.AlbumsView.DisplayColumns;
                var wantedCaption = GlobalStaticVars.Settings.DataDisplay.AlbumsView.DisplayCaptions;

                var info = new RenderStruct
                {
                    Data = content,
                    WantedColumns = wantedColumns,
                    WantedCaption = wantedCaption
                };

                GlobalViews.AlbumViewTable = GenericRenderer.RenderView(info, dgvAlbums);
            }
        }

        private void RenderTracksView(DataTable content)
        {
            if (content != null)
            {
                var wantedColumns = GlobalStaticVars.Settings.DataDisplay.TracksView.DisplayColumns;
                var wantedCaption = GlobalStaticVars.Settings.DataDisplay.TracksView.DisplayCaptions;

                var info = new RenderStruct
                {
                    Data = content,
                    WantedColumns = wantedColumns,
                    WantedCaption = wantedCaption
                };

                GlobalViews.TracksViewTable = GenericRenderer.RenderView(info, dgvTracks);
            }
        }

        private void DgvLibrary_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvSections.SortOrder.ToString() == "Descending") // Check if sorting is Descending
                GlobalTables.SectionsTable.DefaultView.Sort =
                dgvSections.SortedColumn.Name + " DESC"; // Get Sorted Column name and sort it in Descending order
            else
                GlobalTables.SectionsTable.DefaultView.Sort =
                dgvSections.SortedColumn.Name + " ASC"; // Otherwise sort it in Ascending order
            GlobalTables.SectionsTable =
            GlobalTables.SectionsTable.DefaultView
            .ToTable(); // The Sorted View converted to DataTable and then assigned to table object.
        }

        private void RenderLibraryView(DataTable content)
        {
            if (content == null) return;
            var wantedColumns = GlobalStaticVars.Settings.DataDisplay.LibraryView.DisplayColumns;
            var wantedCaption = GlobalStaticVars.Settings.DataDisplay.LibraryView.DisplayCaptions;

            var info = new RenderStruct
            {
                Data = content,
                WantedColumns = wantedColumns,
                WantedCaption = wantedCaption
            };

            GenericRenderer.RenderView(info, dgvSections);
        }

        #endregion ContentRenderers

        #region UpdateWaitWorkers

        private void UpdateContentView(XmlDocument content, ContentType type)
        {
            WaitWindow.WaitWindow.Show(WorkerUpdateContentView, "Updating Content", new object[] { content, type });
        }

        private void UpdateSeriesView(XmlDocument content)
        {
            WaitWindow.WaitWindow.Show(WorkerUpdateSeriesView, "Updating Series", content);
        }

        private void UpdateEpisodeView(XmlDocument content)
        {
            WaitWindow.WaitWindow.Show(WorkerUpdateEpisodesView, "Updating Episodes", content);
        }

        private void UpdateTracksView(XmlDocument content)
        {
            WaitWindow.WaitWindow.Show(WorkerUpdateTracksView, "Updating Tracks", content);
        }

        private void UpdateAlbumsView(XmlDocument content)
        {
            WaitWindow.WaitWindow.Show(WorkerUpdateAlbumsView, "Updating Albums", content);
        }

        private void PopulateLibrary(XmlDocument content)
        {
            WaitWindow.WaitWindow.Show(WorkerUpdateLibraryView, "Updating Library", content);
        }

        #endregion UpdateWaitWorkers

        #region PlexAPIWorkers

        private void GetMovieObjectFromSelectionWorker(object sender, WaitWindowEventArgs e)
        {
            bool formatLinkDownload = false;
            if (e.Arguments.Count > 0)
                formatLinkDownload = (bool)e.Arguments[0];
            e.Result = GetMovieObjectFromSelection(formatLinkDownload);
        }

        private void GetTVObjectFromSelectionWorker(object sender, WaitWindowEventArgs e)
        {
            bool formatLinkDownload = false;
            if (e.Arguments.Count > 0)
                formatLinkDownload = (bool)e.Arguments[0];
            e.Result = GetTvObjectFromSelection(formatLinkDownload);
        }

        private void GetMusicObjectFromSelectionWorker(object sender, WaitWindowEventArgs e)
        {
            bool formatLinkDownload = false;
            if (e.Arguments.Count > 0)
                formatLinkDownload = (bool)e.Arguments[0];
            e.Result = GetMusicObjectFromSelection(formatLinkDownload);
        }

        #endregion PlexAPIWorkers

        #endregion Workers

        #region Download

        #region DownloadMethods

        private void CancelDownload(bool silent = false, string msg = "Download Cancelled")
        {
            //try and kill the worker if it's still trying to do something
            if (wkrGetMetadata.IsBusy) wkrGetMetadata.Abort();

            //check if the Engine's still running; if it is, we can then cancel and clear the download queue.
            if (Flags.IsEngineRunning)
            {
                GlobalStaticVars.Engine.Cancel();
                GlobalStaticVars.Engine.Clear();
            }

            //only run the rest if a download is actually running; we've killed the engine, now we need to set the appropriate
            //flags and values.
            if (Flags.IsDownloadRunning)
            {
                //gui settings functions
                SetProgressLabel(msg);
                SetDlOrderLabel(@"~");
                SetSpeedLabel(@"~");
                SetEtaLabel(@"~");
                SetDownloadStart();
                SetResume();

                //misc. gui settings
                pbMain.Value = pbMain.Minimum;
                btnPause.Enabled = false;

                //log download cancelled message
                LoggingHelpers.RecordGenericEntry(msg);

                //set project global flags
                Flags.IsDownloadRunning = false;
                Flags.IsDownloadPaused = false;
                Flags.IsEngineRunning = false;
                Flags.IsDownloadQueueCancelled = true;

                //set form global indices
                DownloadsSoFar = 0;
                DownloadTotal = 0;
                DownloadIndex = 0;

                if (!silent)
                    ShowError(msg);
            }
        }

        private void StartDownloadEngine()
        {
            GlobalStaticVars.Engine.QueueProgressChanged += Engine_DownloadProgressChanged;
            GlobalStaticVars.Engine.QueueCompleted += Engine_DownloadCompleted;

            GlobalStaticVars.Engine.StartAsync();
            //MessageBox.Show("Started!");
            LoggingHelpers.RecordGenericEntry("Download is Progressing");
            Flags.IsDownloadRunning = true;
            Flags.IsEngineRunning = true;
            Flags.IsDownloadPaused = false;
            SetPause();
        }

        private void SetDownloadDirectory()
        {
            if (fbdSave.ShowDialog() == DialogResult.OK)
            {
                GlobalStaticVars.Settings.Generic.DownloadDirectory = fbdSave.SelectedPath;
                MessageBox.Show("Download directory updated to " + GlobalStaticVars.Settings.Generic.DownloadDirectory, "Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoggingHelpers.RecordGenericEntry("Download directory updated to " + GlobalStaticVars.Settings.Generic.DownloadDirectory);
            }
        }

        private void SetDownloadCompleted(string msg = "Download Completed")
        {
            //gui settings functions
            SetProgressLabel(msg);
            SetDlOrderLabel(@"~");
            SetSpeedLabel(@"~");
            SetEtaLabel(@"~");
            SetDownloadStart();
            SetResume();

            //misc. gui settings
            pbMain.Value = pbMain.Maximum;
            btnPause.Enabled = false;

            //log download completed
            LoggingHelpers.RecordGenericEntry(msg);

            //clear the download queue (just in case)
            GlobalStaticVars.Engine.Clear();

            //set the global project flags
            Flags.IsDownloadRunning = false;
            Flags.IsDownloadPaused = false;
            Flags.IsEngineRunning = false;
        }

        private void StartDownload(List<DownloadInfo> queue, string location)
        {
            LoggingHelpers.RecordGenericEntry("Download Process Started");
            pbMain.Value = 0;

            LoggingHelpers.RecordGenericEntry("Starting HTTP Engine");
            GlobalStaticVars.Engine = new DownloadQueue();
            if (queue.Count > 1)
            {
                foreach (var dl in queue)
                {
                    var fqPath = dl.DownloadPath + @"\" + dl.FileName;
                    if (File.Exists(fqPath))
                        LoggingHelpers.RecordGenericEntry(dl.FileName + " already exists; will not download.");
                    else
                        GlobalStaticVars.Engine.Add(dl.Link, fqPath);
                }
            }
            else
            {
                var dl = queue[0];
                var fqPath = dl.DownloadPath + @"\" + dl.FileName;
                if (File.Exists(fqPath))
                {
                    LoggingHelpers.RecordGenericEntry(dl.FileName + " already exists; get user confirmation.");
                    var msg = MessageBox.Show(dl.FileName + " already exists. Skip this title?", "Message",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (msg == DialogResult.Yes)
                    {
                        SetDownloadCompleted();
                        return;
                    }
                }

                GlobalStaticVars.Engine.Add(dl.Link, fqPath);
            }

            btnPause.Enabled = true;
            StartDownloadEngine();
        }

        #endregion DownloadMethods

        #region DownloadEngineMethods

        private void ShowBalloon(string msg, string title, bool error = false, int timeout = 2000)
        {
            if (!InvokeRequired)
            {
                if (error)
                    nfyMain.BalloonTipIcon = ToolTipIcon.Error;
                else
                    nfyMain.BalloonTipIcon = ToolTipIcon.Info;

                nfyMain.BalloonTipText = msg;
                nfyMain.BalloonTipTitle = title;
                nfyMain.ShowBalloonTip(timeout);
            }
            else
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    if (error)
                        nfyMain.BalloonTipIcon = ToolTipIcon.Error;
                    else
                        nfyMain.BalloonTipIcon = ToolTipIcon.Info;

                    nfyMain.BalloonTipText = msg;
                    nfyMain.BalloonTipTitle = title;
                    nfyMain.ShowBalloonTip(timeout);
                });
            }
        }

        private void Engine_DownloadCompleted(object sender, EventArgs e)
        {
            ShowBalloon("Download completed!", "Message");
            SetDownloadCompleted();
        }

        private void Engine_DownloadProgressChanged(object sender, EventArgs e)
        {
            try
            {
                //engine values - very important information.
                var engineProgress = GlobalStaticVars.Engine.CurrentProgress;
                var bytesGet = GlobalStaticVars.Engine.BytesReceived;
                var engineSpeed = GlobalStaticVars.Engine.CurrentDownloadSpeed;
                var contentSize = GlobalStaticVars.Engine.CurrentContentLength;

                //proper formatting of engine data for display
                var progress = Math.Round(engineProgress);
                var speed = Methods.FormatBytes(engineSpeed) + "/s";
                var total = Methods.FormatBytes((long)contentSize);
                var order = (GlobalStaticVars.Engine.CurrentIndex + 1) + "/" + GlobalStaticVars.Engine.QueueLength;
                var eta = @"~";

                //it'd be really bad if we tried to divide by 0 and 0
                if (bytesGet > 0 && contentSize > 0)
                {
                    //subtract the byte count we already have from the total we need
                    var diff = contentSize - bytesGet;

                    //~needs to be in milisecond format; so * seconds by 1000~
                    var val = (diff / engineSpeed) * 1000;

                    //this converts the raw "ETA" data into human-readable information, then sets it up for display.
                    eta = Methods.CalculateTime(val);
                }

                //gui settings functions
                SetProgressLabel(progress + "% of " + total);
                SetDlOrderLabel(order);
                SetSpeedLabel(speed);
                SetEtaLabel(eta);

                //misc. gui settings
                pbMain.Value = (int)progress;

                //MessageBox.Show("Started!");
            }
            catch (Exception ex)
            {
                //gui settings functions
                SetDlOrderLabel(@"~");
                SetSpeedLabel(@"~");
                SetEtaLabel(@"~");
                SetProgressLabel("Download Status Error(s) Occurred - Check Log");

                //log the error
                LoggingHelpers.RecordException(ex.Message, "DLProgressError");
                return;
            }
        }

        #endregion DownloadEngineMethods

        #endregion Download

        #region Search

        private void ClearSearch(bool renderTables = true)
        {
            if (Flags.IsFiltered)
            {
                if (renderTables)
                {
                    switch (GlobalStaticVars.CurrentContentType)
                    {
                        case ContentType.TvShows:
                            RenderTVView(GlobalTables.TitlesTable);
                            break;

                        case ContentType.Movies:
                            RenderMoviesView(GlobalTables.TitlesTable);
                            break;

                        case ContentType.Music:
                            RenderArtistsView(GlobalTables.TitlesTable);
                            break;
                    }
                }

                GlobalTables.FilteredTable = null;
                Flags.IsFiltered = false;
                SetStartSearch();
            }
        }

        private void RunTitleSearch()
        {
            try
            {
                LoggingHelpers.RecordGenericEntry("Title search requested");
                if ((dgvMovies.Rows.Count > 0) || (dgvTVShows.Rows.Count > 0) || (dgvArtists.Rows.Count > 0))
                {
                    RenderStruct info = null;
                    DataGridView dgv = null;

                    switch (GlobalStaticVars.CurrentContentType)
                    {
                        case ContentType.TvShows:
                            dgv = dgvTVShows;
                            info = new RenderStruct
                            {
                                Data = GlobalTables.TitlesTable,
                                WantedCaption = GlobalStaticVars.Settings.DataDisplay.TvView.DisplayCaptions,
                                WantedColumns = GlobalStaticVars.Settings.DataDisplay.TvView.DisplayColumns
                            };
                            break;

                        case ContentType.Movies:
                            dgv = dgvMovies;
                            info = new RenderStruct
                            {
                                Data = GlobalTables.TitlesTable,
                                WantedCaption = GlobalStaticVars.Settings.DataDisplay.MoviesView.DisplayCaptions,
                                WantedColumns = GlobalStaticVars.Settings.DataDisplay.MoviesView.DisplayColumns
                            };
                            break;

                        case ContentType.Music:
                            dgv = dgvArtists;
                            info = new RenderStruct
                            {
                                Data = GlobalTables.TitlesTable,
                                WantedCaption = GlobalStaticVars.Settings.DataDisplay.ArtistsView.DisplayCaptions,
                                WantedColumns = GlobalStaticVars.Settings.DataDisplay.ArtistsView.DisplayColumns
                            };
                            break;
                    }

                    //MessageBox.Show(info.Data.Rows.Count.ToString());

                    if (Search.RunTitleSearch(dgv, info, true))
                    {
                        Flags.IsFiltered = true;
                        SetCancelSearch();
                    }
                    else
                    {
                        Flags.IsFiltered = false;
                        GlobalViews.FilteredViewTable = null;
                        GlobalTables.FilteredTable = null;
                        SetStartSearch();
                    }
                }
                else
                {
                    LoggingHelpers.RecordGenericEntry("No data to search");
                }
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "SearchError");
                ShowError(ex.ToString());
            }
        }

        #endregion Search

        #region UIMethods

        /// <summary>
        ///     Thread-safe way of changing the progress label
        /// </summary>
        /// <param name="status">
        /// </param>
        private void SetProgressLabel(string status)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { lblProgress.Text = status; });
            else
                lblProgress.Text = status;
        }

        private void SetSpeedLabel(string speed)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { lblSpeedValue.Text = speed; });
            else
                lblSpeedValue.Text = speed;
        }

        private void SetDlOrderLabel(string dlOrder)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { lblDownloadingValue.Text = dlOrder; });
            else
                lblDownloadingValue.Text = dlOrder;
        }

        private void SetEtaLabel(string eta)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { lblEtaValue.Text = eta; });
            else
                lblEtaValue.Text = eta;
        }

        private void SetDownloadCancel()
        {
            btnDownload.Text = @"Cancel";
        }

        private void SetDownloadStart()
        {
            btnDownload.Text = @"Download";
        }

        private void SetPause()
        {
            btnPause.Text = @"Pause";
        }

        private void SetResume()
        {
            btnPause.Text = @"Resume";
        }

        private void SetStartSearch()
        {
            itmStartSearch.Text = @"Start Search";
            if (GlobalTables.TitlesTable != null)
                if (GlobalTables.TitlesTable.Rows != null)
                    lblViewingValue.Text = GlobalTables.TitlesTable.Rows.Count + "/" + GlobalTables.TitlesTable.Rows.Count;
        }

        private void SetCancelSearch()
        {
            if (GlobalTables.FilteredTable != null && GlobalTables.TitlesTable != null)
                if (GlobalTables.FilteredTable.Rows != null && GlobalTables.TitlesTable.Rows != null)
                    lblViewingValue.Text = GlobalTables.FilteredTable.Rows.Count + "/" + GlobalTables.TitlesTable.Rows.Count;
            itmStartSearch.Text = @"Clear Search";
        }

        private void SetConnect()
        {
            itmDisconnect.Enabled = false;
            lblViewingValue.Text = @"0/0";
        }

        private void SetDisconnect()
        {
            itmDisconnect.Enabled = true;
        }

        private void ShowError(string msg, string caption = "Error")
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error); //threadsafe
                });
            }
            else
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion UIMethods

        #region FormEvents

        private void LoadDevStatus()
        {
            var choc = Color.Chocolate;
            var red = Color.DarkRed;
            var green = Color.DarkGreen;
            switch (BuildState.State)
            {
                case DevStatus.InDevelopment:
                    lblBeta.ForeColor = choc;
                    lblBeta.Text = "Developer Build";
                    break;

                case DevStatus.InBeta:
                    lblBeta.ForeColor = red;
                    lblBeta.Text = "Beta Testing Build";
                    break;

                case DevStatus.ProductionReady:
                    lblBeta.ForeColor = green;
                    lblBeta.Text = "Production Build";
                    break;
            }
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Flags.IsDownloadRunning)
            {
                if (!Flags.IsMsgAlreadyShown)
                {
                    var msg = MessageBox.Show("Are you sure you want to exit PlexDL? A download is still running.",
                        "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (msg == DialogResult.Yes)
                    {
                        Flags.IsMsgAlreadyShown = true;
                        LoggingHelpers.RecordGenericEntry("PlexDL Exited");
                        e.Cancel = false;
                    }
                    else if (msg == DialogResult.No)
                        e.Cancel = true;
                }
            }
            else
            {
                LoggingHelpers.RecordGenericEntry("PlexDL Exited");
            }
        }

        private void ResetDownloadDirectory()
        {
            var curUser = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            GlobalStaticVars.Settings.Generic.DownloadDirectory = curUser + @"\PlexDL";
            if (!Directory.Exists(GlobalStaticVars.Settings.Generic.DownloadDirectory))
                Directory.CreateDirectory(GlobalStaticVars.Settings.Generic.DownloadDirectory);
        }

        private void SetDebugLocation()
        {
            if (GlobalStaticVars.DebugForm != null && Flags.IsDebug)
            {
                Point thisp = Location;
                int x = thisp.X + Width;
                int y = thisp.Y;
                GlobalStaticVars.DebugForm.Location = new Point(x, y);
            }
        }

        private void SetSessionId()
        {
            lblSidValue.Text = GlobalStaticVars.CurrentSessionId;
        }

        private void Home_Move(object sender, EventArgs e)
        {
            SetDebugLocation();
        }

        private void Home_Focus(object sender, EventArgs e)
        {
        }

        private void Home_Load(object sender, EventArgs e)
        {
            try
            {
                if (Flags.IsDebug)
                {
                    GlobalStaticVars.DebugForm = new Debug();
                    SetDebugLocation();
                    GlobalStaticVars.DebugForm.Show();
                }

                //MessageBox.Show(GlobalStaticVars.PlexDlAppData);
                //CachingFileDir.RootCacheDirectory = $"{GlobalStaticVars.PlexDlAppData}\\caching";

                SetSessionId();
                LoadDevStatus();
                ResetDownloadDirectory();
                LoggingHelpers.RecordGenericEntry("PlexDL Started");
                LoggingHelpers.RecordGenericEntry($"Data location: {GlobalStaticVars.PlexDlAppData}");
                LoggingHelpers.RecordCacheEvent($"Using cache directory: {CachingFileDir.RootCacheDirectory}", "N/A");
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "StartupError");
                ShowError("Startup Error:\n\n" + ex, "Startup Error");
            }
        }

        private void UpdateFromLibraryKey(string key, ContentType type)
        {
            object[] args =
            {
                key, type
            };
            WaitWindow.WaitWindow.Show(UpdateFromLibraryKey_Worker, @"Getting Metadata", args);
        }

        private void UpdateFromLibraryKey_Worker(object sender, WaitWindowEventArgs e)
        {
            var type = (ContentType)e.Arguments[1];
            var key = (string)e.Arguments[0];
            try
            {
                LoggingHelpers.RecordGenericEntry(@"Requesting ibrary contents");
                var contentUri = GlobalStaticVars.CurrentApiUri + key + @"/all/?X-Plex-Token=";
                var contentXml = XmlGet.GetXmlTransaction(contentUri);

                UpdateContentView(contentXml, type);
            }
            catch (WebException ex)
            {
                LoggingHelpers.RecordException(ex.Message, @"UpdateLibraryError");
                if (ex.Status == WebExceptionStatus.ProtocolError)
                    if (ex.Response is HttpWebResponse response)
                        switch ((int)response.StatusCode)
                        {
                            case 401:
                                MessageBox.Show(
                                    @"The web server denied access to the resource. Check your token and try again. (401)");
                                break;

                            case 404:
                                MessageBox.Show(
                                    @"The web server couldn't serve the request because it couldn't find the resource specified. (404)");
                                break;

                            case 400:
                                MessageBox.Show(
                                    @"The web server couldn't serve the request because the request was bad. (400)");
                                break;
                        }
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "UpdateLibraryError");
                ShowError(ex.ToString(), "Error");
            }
        }

        private void CxtLibrarySections_Opening(object sender, CancelEventArgs e)
        {
            if (dgvSections.Rows.Count == 0) e.Cancel = true;
        }

        #endregion FormEvents

        #region DGVRowChanges

        private void DgvLibrary_OnRowChange(object sender, EventArgs e)
        {
            if (dgvSections.SelectedRows.Count == 1 && Flags.IsLibraryFilled)
            {
                LoggingHelpers.RecordGenericEntry("Selection Changed");
                //don't re-render the grids when clearing the search; this would end badly for performance reasons.
                ClearSearch(false);
                LoggingHelpers.RecordGenericEntry("Cleared possible searches");
                var index = GlobalTables.GetTableIndexFromDgv(dgvSections, GlobalTables.SectionsTable);
                var r = RowGet.GetDataRowLibrary(index);

                var key = "";
                var type = "";
                if (r != null)
                {
                    if (r["key"] != null)
                        key = r["key"].ToString();
                    if (r["type"] != null)
                        type = r["type"].ToString();
                }

                switch (type)
                {
                    case "show":
                        UpdateFromLibraryKey(key, ContentType.TvShows);
                        break;

                    case "movie":
                        UpdateFromLibraryKey(key, ContentType.Movies);
                        break;

                    case "artist":
                        UpdateFromLibraryKey(key, ContentType.Music);
                        break;
                }
            }
        }

        private void DgvSeasons_OnRowChange(object sender, EventArgs e)
        {
            if (dgvSeasons.SelectedRows.Count == 1)
            {
                var index = GlobalTables.GetTableIndexFromDgv(dgvSeasons, GlobalTables.SeasonsTable);
                var episodes = XmlMetadataGatherers.GetEpisodeXml(index);
                UpdateEpisodeView(episodes);
            }
        }

        private void DgvAlbums_OnRowChange(object sender, EventArgs e)
        {
            if (dgvAlbums.SelectedRows.Count == 1)
            {
                var index = GlobalTables.GetTableIndexFromDgv(dgvAlbums, GlobalTables.AlbumsTable);
                var tracks = XmlMetadataGatherers.GetTracksXml(index);
                UpdateTracksView(tracks);
            }
        }

        private void dgvMovies_OnRowChange(object sender, EventArgs e)
        {
            //nothing, more or less.
        }

        private void DoubleClickLaunch(bool formatLinkDownload = false)
        {
            PlexObject stream = null;

            switch (GlobalStaticVars.CurrentContentType)
            {
                case ContentType.Movies:
                    if (dgvMovies.SelectedRows.Count == 1)
                    {
                        var obj = GetMovieObjectFromSelection(formatLinkDownload);
                        if (obj != null)
                        {
                            stream = obj;
                        }
                        else
                            LoggingHelpers.RecordGenericEntry("Doubleclick stream failed; null object.");
                    }
                    break;

                case ContentType.TvShows:
                    if (dgvEpisodes.SelectedRows.Count == 1 && dgvTVShows.SelectedRows.Count == 1)
                    {
                        var obj = GetTvObjectFromSelection(formatLinkDownload);
                        if (obj != null)
                        {
                            stream = obj;
                        }
                        else
                            LoggingHelpers.RecordGenericEntry("Doubleclick stream failed; null object.");
                    }
                    break;

                case ContentType.Music:
                    if (dgvTracks.SelectedRows.Count == 1 && dgvArtists.SelectedRows.Count == 1)
                    {
                        var obj = GetMusicObjectFromSelection(formatLinkDownload);
                        if (obj != null)
                        {
                            stream = obj;
                        }
                        else
                            LoggingHelpers.RecordGenericEntry("Doubleclick stream failed; null object.");
                    }
                    break;
            }

            if (stream != null)
            {
                if (GlobalStaticVars.Settings.Player.PlaybackEngine == PlaybackMode.MenuSelector)
                {
                    if (VlcLauncher.VlcInstalled())
                    {
                        StartStreaming(stream, PlaybackMode.VLCPlayer);
                    }
                    else
                    {
                        StartStreaming(stream, PlaybackMode.PVSPlayer);
                    }
                }
                else
                {
                    StartStreaming(stream);
                }
            }
        }

        private void DoubleClickProcessor(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var senderType = sender.GetType();
                var gridType = typeof(FlatDataGridView);
                if (senderType == gridType)
                {
                    var gridView = (FlatDataGridView)sender;
                    if (GlobalStaticVars.Settings.Generic.DoubleClickLaunch && gridView.IsContentTable)
                    {
                        if (gridView.SelectedRows.Count == 1)
                            DoubleClickLaunch();
                    }
                    else
                    {
                        if (gridView.Rows.Count <= 0) return;
                        var value = gridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        MessageBox.Show(value, @"Cell Content", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    LoggingHelpers.RecordGenericEntry("Double-click launch failed; incorrect object type. Expecting object of type FlatDataGridView.");
                }
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "DoubleClickError");
                LoggingHelpers.RecordGenericEntry("Double-click launch failed; an error occurred. Check exception log for more information.");
            }
        }

        //debugging stuff
        private void XmlMessageBox(XmlDocument doc)
        {
            if (doc != null)
            {
                using (var sww = new StringWriter())
                using (var writer = XmlWriter.Create(sww))
                {
                    doc.WriteTo(writer);
                    writer.Flush();
                    MessageBox.Show(sww.GetStringBuilder().ToString());
                }
            }
            else
                MessageBox.Show("XML Document was null");
        }

        private void DgvTVShows_OnRowChange(object sender, EventArgs e)
        {
            if (dgvTVShows.SelectedRows.Count == 1)
            {
                var index = GlobalTables.GetTableIndexFromDgv(dgvTVShows, GlobalTables.ReturnCorrectTable());

                //debugging
                //MessageBox.Show(index.ToString());

                if (GlobalStaticVars.CurrentContentType == ContentType.TvShows)
                {
                    var series = XmlMetadataGatherers.GetSeriesXml(index);
                    //debugging
                    //XmlMessageBox(series);
                    UpdateSeriesView(series);
                }
            }
        }

        private void DgvArtists_OnRowChange(object sender, EventArgs e)
        {
            if (dgvArtists.SelectedRows.Count == 1)
            {
                var index = GlobalTables.GetTableIndexFromDgv(dgvArtists, GlobalTables.ReturnCorrectTable());

                //debugging
                //MessageBox.Show(index.ToString());

                if (GlobalStaticVars.CurrentContentType == ContentType.Music)
                {
                    var albums = XmlMetadataGatherers.GetAlbumsXml(index);
                    //debugging
                    //XmlMessageBox(series);
                    UpdateAlbumsView(albums);
                }
            }
        }

        #endregion DGVRowChanges

        #region ButtonClicks

        private void BtnTest_Click(object sender, EventArgs e)
        {
            try
            {
                //deprecated (planned reintroduction)
                var uri = GlobalStaticVars.GetBaseUri(true);
                var reply = (XmlDocument)WaitWindow.WaitWindow.Show(XmlGet.GetXMLTransactionWorker, @"Connecting", uri);
                MessageBox.Show(@"Connection successful!", @"Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (WebException ex)
            {
                LoggingHelpers.RecordException(ex.Message, @"TestConnectionError");
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    if (ex.Response is HttpWebResponse response)
                        switch ((int)response.StatusCode)
                        {
                            case 401:
                                ShowError(
                                    @"The web server denied access to the resource. Check your token and try again. (401)");
                                break;

                            case 404:
                                ShowError(
                                    @"The web server couldn't serve the request because it couldn't find the resource specified. (404)");
                                break;

                            case 400:
                                ShowError(
                                    @"The web server couldn't serve the request because the request was bad. (400)");
                                break;
                        }
                    else
                        ShowError(@"Unknown status code; the server failed to serve the request. (?)");
                }
                else
                {
                    ShowError("An unknown error occurred:\n\n" + ex, "Error");
                }
            }
        }

        private void DoDownloadAll()
        {
            LoggingHelpers.RecordGenericEntry("Awaiting download safety checks");
            if (!Flags.IsDownloadRunning && !Flags.IsEngineRunning)
            {
                LoggingHelpers.RecordGenericEntry("Download process is starting");
                SetProgressLabel("Waiting");
                Flags.IsDownloadAll = true;
                DownloadTotal = GlobalTables.ReturnCorrectTable(true).Rows.Count;
                Flags.IsDownloadRunning = true;
                if (wkrGetMetadata.IsBusy) wkrGetMetadata.Abort();
                wkrGetMetadata.RunWorkerAsync();
                tmrWorkerTimeout.Start();
                LoggingHelpers.RecordGenericEntry("Worker invoke process started");
                SetDownloadCancel();
            }
            else
                LoggingHelpers.RecordGenericEntry("Download process failed; download is already running.");
        }

        private void DoDownloadSelected()
        {
            LoggingHelpers.RecordGenericEntry("Awaiting download safety checks");
            //MessageBox.Show(GlobalStaticVars.CurrentContentType.ToString());
            if (!Flags.IsDownloadRunning && !Flags.IsEngineRunning)
            {
                LoggingHelpers.RecordGenericEntry("Download process is starting");
                SetProgressLabel("Waiting");
                Flags.IsDownloadAll = false;
                DownloadTotal = 1;
                Flags.IsDownloadRunning = true;
                if (wkrGetMetadata.IsBusy) wkrGetMetadata.Abort();
                wkrGetMetadata.RunWorkerAsync();
                tmrWorkerTimeout.Start();
                LoggingHelpers.RecordGenericEntry("Worker invoke process started");
                SetDownloadCancel();
            }
            else
                LoggingHelpers.RecordGenericEntry("Download process failed; download is already running.");
        }

        private void StartStreaming(PlexObject stream)
        {
            int def = GlobalStaticVars.Settings.Player.PlaybackEngine;
            StartStreaming(stream, def);
        }

        private void StartStreaming(PlexObject stream, int PlaybackEngine)
        {
            //so that cxtStreamOptions can access the current stream's information, a global object has to be used.
            GlobalStaticVars.CurrentStream = stream;
            if (PlaybackEngine != -1)
            {
                if (PlaybackEngine == PlaybackMode.PVSPlayer)
                {
                    PvsLauncher.LaunchPvs(stream, GlobalTables.ReturnCorrectTable());
                }
                else if (PlaybackEngine == PlaybackMode.VLCPlayer)
                {
                    VlcLauncher.LaunchVlc(stream);
                }
                else if (PlaybackEngine == PlaybackMode.Browser)
                {
                    BrowserLauncher.LaunchBrowser(stream);
                }
                else if (PlaybackEngine == PlaybackMode.MenuSelector)
                {
                    //display the options menu where the
                    var loc = new Point(Location.X + btnHTTPPlay.Location.X, Location.Y + btnHTTPPlay.Location.Y);
                    cxtStreamOptions.Show(loc);
                }
                else
                {
                    MessageBox.Show("Unrecognised Playback Mode (\"" + PlaybackEngine + "\")",
                        "Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    LoggingHelpers.RecordGenericEntry("Invalid Playback Mode: " + PlaybackEngine);
                }
            }
        }

        private void SearchProcedure()
        {
            if (Flags.IsFiltered)
                ClearSearch();
            else
                RunTitleSearch();
        }

        private void CxtEpisodeOptions_Opening(object sender, CancelEventArgs e)
        {
            if (dgvEpisodes.SelectedRows.Count == 0) e.Cancel = true;
        }

        private void CxtContentOptions_Opening(object sender, CancelEventArgs e)
        {
            if (dgvMovies.SelectedRows.Count == 0) e.Cancel = true;
        }

        private void Metadata(PlexObject result = null)
        {
            if ((dgvMovies.SelectedRows.Count == 1) || (dgvEpisodes.SelectedRows.Count == 1) || (dgvTracks.SelectedRows.Count == 1))
            {
                if (!Flags.IsDownloadRunning && !Flags.IsEngineRunning)
                {
                    if (result == null)
                    {
                        switch (GlobalStaticVars.CurrentContentType)
                        {
                            case ContentType.Movies:
                                result = (PlexObject)WaitWindow.WaitWindow.Show(GetMovieObjectFromSelectionWorker,
                                "Getting Metadata", new object[] { false });
                                break;

                            case ContentType.TvShows:
                                result = (PlexObject)WaitWindow.WaitWindow.Show(GetTVObjectFromSelectionWorker,
                                "Getting Metadata", new object[] { false });
                                break;

                            case ContentType.Music:
                                result = (PlexMusic)WaitWindow.WaitWindow.Show(GetMusicObjectFromSelectionWorker,
                                "Getting Metadata", new object[] { false });
                                break;
                        }
                    }

                    using (var frm = new Metadata())
                    {
                        frm.StreamingContent = result;
                        frm.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("You cannot view metadata while an internal download is running",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else if (dgvMovies.Rows.Count == 0 && dgvTVShows.Rows.Count == 0)
            {
                using (var frm = new Metadata())
                {
                    frm.StationaryMode = true;
                    frm.ShowDialog();
                }
            }
        }

        private void ShowLogViewer()
        {
            using (var frm = new LogViewer())
            {
                frm.ShowDialog();
            }
        }

        #endregion ButtonClicks

        private void itmDownloadThisTrack_Click(object sender, EventArgs e)
        {
            cxtTracks.Close();
            DoDownloadSelected();
        }

        private void itmDownloadThisAlbum_Click(object sender, EventArgs e)
        {
            cxtTracks.Close();
            DoDownloadAll();
        }

        private void cxtTrackOptions_Opening(object sender, CancelEventArgs e)
        {
            if (dgvTracks.SelectedRows.Count == 0) e.Cancel = true;
        }

        private void itmDGVDownloadThisTrack_Click(object sender, EventArgs e)
        {
            cxtTrackOptions.Close();
            DoDownloadSelected();
        }

        private void itmDGVDownloadThisAlbum_Click(object sender, EventArgs e)
        {
            cxtTrackOptions.Close();
            DoDownloadAll();
        }

        private void itmTrackMetadata_Click(object sender, EventArgs e)
        {
            cxtTrackOptions.Close();
            Metadata();
        }
    }
}