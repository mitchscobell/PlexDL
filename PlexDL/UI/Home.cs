﻿using PlexDL.AltoHTTP.Classes;
using PlexDL.Common;
using PlexDL.Common.API;
using PlexDL.Common.Caching;
using PlexDL.Common.Globals;
using PlexDL.Common.Logging;
using PlexDL.Common.PlayerLaunchers;
using PlexDL.Common.Renderers;
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
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Directory = System.IO.Directory;

//using System.Threading.Tasks;

namespace PlexDL.UI
{
    public partial class Home : Form
    {
        private void LblViewFullLog_LinkClicked(object sender, EventArgs e)
        {
            ShowLogViewer();
        }

        private void BtnSetDlDir_Click(object sender, EventArgs e)
        {
            SetDownloadDirectory();
        }

        private void ManualSectionLoad()
        {
            if (dgvLibrary.Rows.Count > 0)
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
        }

        private void ItmDownloadThisEpisode_Click(object sender, EventArgs e)
        {
            cxtEpisodes.Close();
            DoDownloadSelected();
        }

        private void ItmDownloadAllEpisodes_Click(object sender, EventArgs e)
        {
            cxtEpisodes.Close();
            DoDownloadAllEpisodes();
        }

        private void ItmManuallyLoadSection_Click(object sender, EventArgs e)
        {
            cxtLibrarySections.Close();
            ManualSectionLoad();
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
            DoDownloadAllEpisodes();
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
            PVSLauncher.LaunchPVS(GlobalStaticVars.CurrentStream, ReturnCorrectTable());
        }

        private void ItmStreamInVLC_Click(object sender, EventArgs e)
        {
            cxtStreamOptions.Close();
            VLCLauncher.LaunchVLC(GlobalStaticVars.CurrentStream);
        }

        private void ItmStreamInBrowser_Click(object sender, EventArgs e)
        {
            cxtStreamOptions.Close();
            BrowserLauncher.LaunchBrowser(GlobalStaticVars.CurrentStream);
        }

        #region GlobalStringVariables

        public string Uri = "";
        public string FilterQuery = "";

        #endregion GlobalStringVariables

        #region GlobalIntVariables

        public int DownloadIndex;
        public int DownloadTotal;
        public int logIncrementer;
        public int downloadsSoFar;

        #endregion GlobalIntVariables

        #region GlobalXmlDocumentVariables

        private XmlDocument _contentXmlDoc;

        #endregion GlobalXmlDocumentVariables

        #region Form

        #region FormInitialiser

        public Home()
        {
            InitializeComponent();
            tabMain.SelectedIndex = 0;
        }

        #endregion FormInitialiser

        #endregion Form

        #region XML/Metadata

        #region PlexMovieBuilders

        public int GetTableIndexFromDGV(DataGridView dgv, DataTable table = null, string key = "title")
        {
            var sel = dgv.SelectedRows[0];
            if (table == null)
                table = ReturnCorrectTable();
            foreach (DataRow r in table.Rows)
                if (dgv.Columns.Contains(key))
                {
                    var titleValue = sel.Cells[key].Value.ToString();
                    if (r[key].ToString() == titleValue) return table.Rows.IndexOf(r);
                }

            return 0;
        }

        private PlexMovie GetMovieObjectFromSelection()
        {
            var obj = new PlexMovie();
            if (dgvContent.SelectedRows.Count == 1 || dgvEpisodes.SelectedRows.Count == 1)
            {
                var index = GetTableIndexFromDGV(dgvContent);
                obj = GetMovieObjectFromIndex(index);
            }

            return obj;
        }

        private PlexTVShow GetTVObjectFromSelection()
        {
            var obj = new PlexTVShow();
            if (dgvTVShows.SelectedRows.Count == 1 && dgvEpisodes.SelectedRows.Count == 1)
            {
                var index = GetTableIndexFromDGV(dgvEpisodes, GlobalTables.EpisodesTable);
                obj = GetTVObjectFromIndex(index);
            }

            return obj;
        }

        private PlexTVShow GetTVObjectFromIndex(int index)
        {
            var obj = new PlexTVShow();
            XmlDocument metadata;
            if (dgvTVShows.SelectedRows.Count == 1)
            {
                AddToLog("Content Parse Started");
                AddToLog("Grabbing Titles");

                metadata = GetEpisodeMetadata(index);

                AddToLog("Checking XML validity");
                if (Methods.PlexXmlValid(metadata))
                {
                    AddToLog("XML Valid");

                    var dlInfo = GetContentDownloadInfo_Xml(metadata);

                    if (dlInfo != null)
                    {
                        AddToLog("Assembling Object");

                        obj.ContentGenre = GetContentGenre(metadata);
                        obj.StreamInformation = dlInfo;
                        obj.Season = GetTVShowSeason(metadata);
                        obj.EpisodesInSeason = GlobalTables.EpisodesTable.Rows.Count;
                        obj.TVShowName = GetTVShowTitle(metadata);
                        obj.StreamResolution = GetContentResolution(metadata);
                        obj.Actors = GetActorsFromMetadata(metadata);
                        obj.StreamIndex = index;
                        obj.Synopsis = GetContentSynopsis(metadata);
                    }
                    else
                    {
                        MessageBox.Show(
                            "Failed to get contextual information; an unknown error occurred. Check the exception log for more information.",
                            "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LoggingHelpers.RecordException(
                            "DownloadInfo invalid. This may be an internal error; please report this issue on GitHub.",
                            "ContextDownloadInfoNull");
                        AddToLog("DownloadInfo is invalid (no stream contexual information)");
                    }
                }
                else
                {
                    AddToLog("XML Invalid");
                }
            }

            return obj;
        }

        private PlexMovie GetMovieObjectFromIndex(int index)
        {
            var obj = new PlexMovie();
            XmlDocument metadata;
            if (dgvContent.SelectedRows.Count == 1)
            {
                AddToLog("Content Parse Started");
                AddToLog("Grabbing Titles");
                metadata = GetContentMetadata(index);

                AddToLog("Checking XML validity");
                if (Methods.PlexXmlValid(metadata))
                {
                    AddToLog("XML Valid");

                    var dlInfo = GetContentDownloadInfo_Xml(metadata);

                    if (dlInfo != null)
                    {
                        AddToLog("Assembling Object");

                        obj.ContentGenre = GetContentGenre(metadata);
                        obj.StreamInformation = dlInfo;
                        obj.StreamResolution = GetContentResolution(metadata);
                        obj.Actors = GetActorsFromMetadata(metadata);
                        obj.StreamIndex = index;
                        obj.Synopsis = GetContentSynopsis(metadata);
                    }
                    else
                    {
                        MessageBox.Show(
                            "Failed to get contextual information; an unknown error occurred. Check the exception log for more information.",
                            "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LoggingHelpers.RecordException(
                            "DownloadInfo invalid. This may be an internal error; please report this issue on GitHub.",
                            "ContextDownloadInfoNull");
                        AddToLog("DownloadInfo is invalid (no stream contexual information)");
                    }
                }
                else
                {
                    AddToLog("XML Invalid");
                }
            }

            return obj;
        }

        #endregion PlexMovieBuilders

        #region MetadataGatherers

        public XmlDocument GetTVShowMetadata(int index)
        {
            AddToLog("Sorting existing data");

            DataRow result;

            result = GetDataRowSeries(index);

            var key = result["key"].ToString();
            var baseUri = GetBaseUri(false);
            key = key.TrimStart('/');
            var uri = baseUri + key + "/?X-Plex-Token=";

            AddToLog("Contacting server");
            var reply = XmlGet.GetXMLTransaction(uri);
            return reply;
        }

        public DataRow GetDataRowContent(int index, bool directTable)
        {
            if (!directTable) return GetDataRowTbl(ReturnCorrectTable(), index);

            if (Flags.IsFiltered)
                return GetDataRowTbl(GlobalTables.FilteredTable, index);
            return GetDataRowTbl(GlobalTables.TitlesTable, index);
        }

        public DataRow GetDataRowSeries(int index)
        {
            return GetDataRowTbl(GlobalTables.SeriesTable, index);
        }

        public DataRow GetDataRowEpisodes(int index)
        {
            return GetDataRowTbl(GlobalTables.EpisodesTable, index);
        }

        public DataRow GetDataRowLibrary(int index)
        {
            return GetDataRowTbl(GlobalTables.SectionsTable, index);
        }

        public XmlDocument GetContentMetadata(int index)
        {
            AddToLog("Sorting existing data");

            DataRow result;

            result = GetDataRowContent(index, false);

            var key = result["key"].ToString();
            var baseUri = GetBaseUri(false);
            key = key.TrimStart('/');
            var uri = baseUri + key + "/?X-Plex-Token=";

            AddToLog("Contacting server");
            var reply = XmlGet.GetXMLTransaction(uri);
            return reply;
        }

        public XmlDocument GetEpisodeMetadata(int index)
        {
            AddToLog("Sorting existing data");

            DataRow result;

            result = GetDataRowEpisodes(index);

            var key = result["key"].ToString();
            var baseUri = GetBaseUri(false);
            key = key.TrimStart('/');
            var uri = baseUri + key + "/?X-Plex-Token=";

            AddToLog("Contacting server");
            var reply = XmlGet.GetXMLTransaction(uri);
            return reply;
        }

        #endregion MetadataGatherers

        #region XMLGatherers

        public XmlDocument GetSeriesXml(int index)
        {
            AddToLog("Sorting existing data");

            DataRow result;

            result = GetDataRowContent(index, true);

            var key = result["key"].ToString();
            var baseUri = GetBaseUri(false);
            key = key.TrimStart('/');
            var uri = baseUri + key + "/?X-Plex-Token=";

            //MessageBox.Show(uri);

            AddToLog("Contacting server");
            var reply = XmlGet.GetXMLTransaction(uri);
            return reply;
        }

        public XmlDocument GetEpisodeXml(int index)
        {
            AddToLog("Sorting existing data");

            DataRow result;
            result = GetDataRowSeries(index);

            var key = result["key"].ToString();
            var baseUri = GetBaseUri(false);
            key = key.TrimStart('/');
            var uri = baseUri + key + "/?X-Plex-Token=";

            //MessageBox.Show(uri);

            AddToLog("Contacting server");
            var reply = XmlGet.GetXMLTransaction(uri);
            return reply;
        }

        #endregion XMLGatherers

        #region MetadataParsers

        private List<PlexActor> GetActorsFromMetadata(XmlDocument metadata)
        {
            var actors = new List<PlexActor>();

            var sections = new DataSet();
            sections.ReadXml(new XmlNodeReader(metadata));
            var dtActors = sections.Tables["Role"];

            if (dtActors != null)
                foreach (DataRow r in dtActors.Rows)
                {
                    var a = new PlexActor();
                    var thumb = "";
                    var role = "Unknown";
                    var name = "Unknown";
                    if (dtActors.Columns.Contains("thumb"))
                        if (r["thumb"] != null)
                            thumb = r["thumb"].ToString();
                    if (dtActors.Columns.Contains("role"))
                        if (r["role"] != null)
                            role = r["role"].ToString();
                    if (dtActors.Columns.Contains("tag"))
                        if (r["tag"] != null)
                            name = r["tag"].ToString();
                    a.ThumbnailUri = thumb;
                    a.ActorRole = role;
                    a.ActorName = name;
                    actors.Add(a);
                }

            return actors;
        }

        private Resolution GetContentResolution(XmlDocument metadata)
        {
            var sections = new DataSet();
            sections.ReadXml(new XmlNodeReader(metadata));
            var video = sections.Tables["Media"];
            var row = video.Rows[0];
            var width = 0;
            if (video.Columns.Contains("width"))
                if (row["width"] != null)
                    width = Convert.ToInt32(row["width"]);
            var height = 0;
            if (video.Columns.Contains("height"))
                if (row["height"] != null)
                    height = Convert.ToInt32(row["height"]);
            var result = new Resolution
            {
                Width = width,
                Height = height
            };
            return result;
        }

        private string GetContentGenre(XmlDocument metadata)
        {
            var sections = new DataSet();
            sections.ReadXml(new XmlNodeReader(metadata));
            var video = sections.Tables["Genre"];
            var genre = "Unknown";
            if (video != null)
            {
                var row = video.Rows[0];
                if (!string.IsNullOrEmpty((string)row["tag"]))
                    genre = row["tag"].ToString();
            }

            return genre;
        }

        private string GetContentSynopsis(XmlDocument metadata)
        {
            var sections = new DataSet();
            sections.ReadXml(new XmlNodeReader(metadata));
            var video = sections.Tables["Video"];
            var synopsis = "Plot synopsis not provided";
            if (video != null)
            {
                var row = video.Rows[0];
                if (!string.IsNullOrEmpty((string)row["summary"]))
                    synopsis = row["summary"].ToString();
            }

            return synopsis;
        }

        private string GetTVShowSeason(XmlDocument metadata)
        {
            var sections = new DataSet();
            sections.ReadXml(new XmlNodeReader(metadata));
            var video = sections.Tables["Video"];
            var season = "Unknown Season";
            if (video != null)
            {
                var row = video.Rows[0];
                if (!string.IsNullOrEmpty((string)row["parentTitle"]))
                    season = row["parentTitle"].ToString();
            }

            return season;
        }

        private string GetTVShowTitle(XmlDocument metadata)
        {
            var sections = new DataSet();
            sections.ReadXml(new XmlNodeReader(metadata));
            var video = sections.Tables["Video"];
            var title = "Unknown Title";
            if (video != null)
            {
                var row = video.Rows[0];
                if (!string.IsNullOrEmpty((string)row["grandparentTitle"]))
                    title = row["grandparentTitle"].ToString();
            }

            return title;
        }

        private DataTable ReturnCorrectTable(bool directTable = false)
        {
            if (Flags.IsTVShow && !directTable) return GlobalTables.EpisodesTable;

            if (Flags.IsFiltered)
                return GlobalTables.FilteredTable;
            return GlobalTables.TitlesTable;
        }

        #endregion MetadataParsers

        #endregion XML/Metadata

        #region Helpers

        #region StringIntHelpers

        private string GetBaseUri(bool incToken)
        {
            if (incToken)
                return "http://" + GlobalStaticVars.Settings.ConnectionInfo.PlexAddress + ":" + GlobalStaticVars.Settings.ConnectionInfo.PlexPort +
                       "/?X-Plex-Token=";
            return "http://" + GlobalStaticVars.Settings.ConnectionInfo.PlexAddress + ":" + GlobalStaticVars.Settings.ConnectionInfo.PlexPort + "/";
        }

        #endregion StringIntHelpers

        #region ProfileHelpers

        public void LoadProfile()
        {
            if (!Flags.IsConnected)
            {
                if (ofdLoadProfile.ShowDialog() == DialogResult.OK) DoLoadProfile(ofdLoadProfile.FileName);
            }
            else
            {
                MessageBox.Show("You can't load profiles while you're connected; please disconnect first.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void SaveProfile()
        {
            if (string.IsNullOrEmpty(GlobalStaticVars.Settings.ConnectionInfo.PlexAccountToken))
            {
                MessageBox.Show("You need to specify an account token before saving a profile", "Validation Error",
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
                var subReq = GlobalStaticVars.Settings;
                var xsSubmit = new XmlSerializer(typeof(ApplicationOptions));
                var xmlWriterSettings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    OmitXmlDeclaration = false
                };
                var xmlSettings = xmlWriterSettings;
                using (var sww = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sww, xmlSettings))
                    {
                        xsSubmit.Serialize(sww, GlobalStaticVars.Settings);

                        //delete the existing file; the user was asked if they wanted to replace it.
                        if (File.Exists(fileName))
                            File.Delete(fileName);

                        File.WriteAllText(fileName, sww.ToString());
                    }
                }

                if (!silent)
                    MessageBox.Show("Successfully saved profile!", "Message", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                AddToLog("Saved profile " + fileName);
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "SaveProfileError");
                if (!silent)
                    MessageBox.Show(ex.ToString(), "Error in saving XML Profile", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }

        public void DoLoadProfile(string fileName, bool silent = false)
        {
            try
            {
                ApplicationOptions subReq = null;

                var serializer = new XmlSerializer(typeof(ApplicationOptions));

                var reader = new StreamReader(fileName);
                subReq = (ApplicationOptions)serializer.Deserialize(reader);
                reader.Close();
                Version vStoredVersion = new Version(subReq.Generic.StoredAppVersion);
                Version vThisVersion = new Version(Application.ProductVersion);
                int vCompare = vThisVersion.CompareTo((vStoredVersion));
                if (vCompare < 0)
                {
                    if (!silent)
                    {
                        var result = MessageBox.Show(
                            "You're trying to load a profile made in a newer version of PlexDL. This could have several implications:\n" +
                            "- Possible data loss of your current profile if PlexDL overwrites it\n" +
                            "- Features may not work as intended\n" +
                            "- Increased risk of errors\n\n" +
                            "Press 'OK' to continue loading, or 'Cancel' to stop loading.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                        if (result == DialogResult.Cancel)
                            return;
                    }

                    AddToLog("Tried to load a profile made in a newer version: " + vStoredVersion.ToString() + " > " + vThisVersion.ToString());
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
                            "Press 'OK' to continue loading, or 'Cancel' to stop loading.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                        if (result == DialogResult.Cancel)
                            return;
                    }

                    AddToLog("Tried to load a profile made in an earlier version: " + vStoredVersion.ToString() + " < " + vThisVersion.ToString());
                }
                GlobalStaticVars.Settings = subReq;

                if (!silent)
                    MessageBox.Show("Successfully loaded profile!", "Message", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                AddToLog("Loaded profile " + fileName);
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "LoadProfileError");
                if (!silent)
                    MessageBox.Show(ex.ToString(), "Error in loading XML Profile", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
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

        private void DoStreamExport()
        {
            try
            {
                if (dgvContent.SelectedRows.Count == 1 || dgvEpisodes.SelectedRows.Count == 1)
                {
                    PlexObject content = null;
                    if (Flags.IsTVShow)
                        content = GetTVObjectFromSelection();
                    else
                        content = GetMovieObjectFromSelection();

                    if (sfdExport.ShowDialog() == DialogResult.OK)
                        ImportExport.MetadataToFile(sfdExport.FileName, content);
                }
            }
            catch (Exception ex)
            {
                //log and ignore
                AddToLog("Export error: " + ex.Message);
                LoggingHelpers.RecordException(ex.Message, "StreamExportError");
            }
        }

        #endregion ConnectionHelpers

        #endregion Helpers

        #region Workers

        #region UpdateWorkers

        private DataRow GetDataRowTbl(DataTable table, int index)
        {
            return table.Rows[index];
        }

        private void PopulateLibraryWorker(XmlDocument doc)
        {
            if (doc != null)
                try
                {
                    AddToLog("Library population requested");
                    var libraryDir = KeyGatherers.GetLibraryKey(doc).TrimEnd('/');
                    var baseUri = GetBaseUri(false);
                    var uriSectionKey = baseUri + libraryDir + "/?X-Plex-Token=";
                    var xmlSectionKey = XmlGet.GetXMLTransaction(uriSectionKey);

                    var sectionDir = KeyGatherers.GetSectionKey(xmlSectionKey).TrimEnd('/');
                    var uriSections = baseUri + libraryDir + "/" + sectionDir + "/?X-Plex-Token=";
                    var xmlSections = XmlGet.GetXMLTransaction(uriSections);

                    AddToLog("Creating new datasets");
                    var sections = new DataSet();
                    sections.ReadXml(new XmlNodeReader(xmlSections));

                    DataTable sectionsTable;
                    sectionsTable = sections.Tables["Directory"];
                    GlobalTables.SectionsTable = sectionsTable;

                    AddToLog("Binding to grid");
                    RenderLibraryView(sectionsTable);
                    Flags.IsLibraryFilled = true;
                    Uri = baseUri + libraryDir + "/" + sectionDir + "/";
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
                    LoggingHelpers.RecordException(ex.Message, "LibPopError");
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private void GetTitlesTable(XmlDocument doc, bool isTVShow)
        {
            var sections = new DataSet();
            sections.ReadXml(new XmlNodeReader(doc));

            if (isTVShow)
                GlobalTables.TitlesTable = sections.Tables["Directory"];
            else
                GlobalTables.TitlesTable = sections.Tables["Video"];
        }

        private void UpdateContentViewWorker(XmlDocument doc, bool isTVShow)
        {
            DGVLibraryEnabled(false);

            AddToLog("Updating library contents");

            GetTitlesTable(doc, isTVShow);

            Flags.IsTVShow = isTVShow;

            if (Flags.IsTVShow)
            {
                AddToLog("Rendering TV Shows");
                RenderTVView(GlobalTables.TitlesTable);
            }
            else
            {
                AddToLog("Rendering Movies");
                RenderContentView(GlobalTables.TitlesTable);
            }

            _contentXmlDoc = doc;

            DGVLibraryEnabled(true);

            //MessageBox.Show("ContentTable: " + contentTable.Rows.Count.ToString() + "\nTitlesTable: " + GlobalTables.TitlesTable.Rows.Count.ToString());
        }

        private void UpdateEpisodeViewWorker(XmlDocument doc)
        {
            DGVSeasonsEnabled(false);
            AddToLog("Updating episode contents");

            AddToLog("Creating datasets");
            var sections = new DataSet();
            sections.ReadXml(new XmlNodeReader(doc));

            GlobalTables.EpisodesTable = sections.Tables["Video"];

            AddToLog("Cleaning unwanted data");

            AddToLog("Binding to grid");
            RenderEpisodesView(GlobalTables.EpisodesTable);

            _contentXmlDoc = doc;

            DGVSeasonsEnabled(true);

            //MessageBox.Show("ContentTable: " + contentTable.Rows.Count.ToString() + "\nTitlesTable: " + GlobalTables.TitlesTable.Rows.Count.ToString());
        }

        private void UpdateSeriesViewWorker(XmlDocument doc)
        {
            DGVContentEnabled(false);
            AddToLog("Updating series contents");

            AddToLog("Creating datasets");
            var sections = new DataSet();
            sections.ReadXml(new XmlNodeReader(doc));

            GlobalTables.SeriesTable = sections.Tables["Directory"];

            AddToLog("Cleaning unwanted data");

            AddToLog("Binding to grid");
            RenderSeriesView(GlobalTables.SeriesTable);

            _contentXmlDoc = doc;

            DGVContentEnabled(true);

            //MessageBox.Show("ContentTable: " + contentTable.Rows.Count.ToString() + "\nTitlesTable: " + GlobalTables.TitlesTable.Rows.Count.ToString());
        }

        #endregion UpdateWorkers

        #region BackgroundWorkers

        private void WkrGetMetadata_DoWork(object sender, DoWorkEventArgs e)
        {
            AddToLog(@"Metadata worker started");
            AddToLog(@"Doing directory checks");
            if (string.IsNullOrEmpty(GlobalStaticVars.Settings.Generic.DownloadDirectory) ||
                string.IsNullOrWhiteSpace(GlobalStaticVars.Settings.Generic.DownloadDirectory)) ResetDownloadDirectory();
            var tv = GlobalStaticVars.Settings.Generic.DownloadDirectory + @"\TV";
            var movies = GlobalStaticVars.Settings.Generic.DownloadDirectory + @"\Movies";
            if (!Directory.Exists(tv))
            {
                Directory.CreateDirectory(tv);
                AddToLog("Created " + tv);
            }

            if (!Directory.Exists(movies))
            {
                Directory.CreateDirectory(movies);
                AddToLog(movies);
            }

            AddToLog(@"Grabbing metadata");
            if (Flags.IsTVShow)
            {
                AddToLog(@"Worker is to grab TV Show metadata");
                if (Flags.IsDownloadAllEpisodes)
                {
                    AddToLog(@"Worker is to grab metadata for All Episodes");
                    var index = 0;
                    foreach (DataRow r in GlobalTables.EpisodesTable.Rows)
                    {
                        var percent = Math.Round(((decimal)index + 1) / GlobalTables.EpisodesTable.Rows.Count) * 100;
                        BeginInvoke((MethodInvoker)delegate
                        {
                            lblProgress.Text = @"Getting Metadata " + (index + 1) + @"/" + GlobalTables.EpisodesTable.Rows.Count;
                        });
                        var show = GetTVObjectFromIndex(index);
                        var dlInfo = show.StreamInformation;
                        var dir = DownloadLayout.CreateDownloadLayoutTVShow(show, GlobalStaticVars.Settings,
                            DownloadLayout.PlexStandardLayout);
                        dlInfo.DownloadPath = dir.SeasonPath;
                        GlobalStaticVars.Queue.Add(dlInfo);
                        index += 1;
                    }
                }
                else
                {
                    AddToLog(@"Worker is to grab Single Episode metadata");
                    BeginInvoke((MethodInvoker)delegate { lblProgress.Text = @"Getting Metadata"; });
                    var show = GetTVObjectFromSelection();
                    var dlInfo = show.StreamInformation;
                    var dir = DownloadLayout.CreateDownloadLayoutTVShow(show, GlobalStaticVars.Settings,
                        DownloadLayout.PlexStandardLayout);
                    dlInfo.DownloadPath = dir.SeasonPath;
                    GlobalStaticVars.Queue.Add(dlInfo);
                }
            }
            else
            {
                AddToLog(@"Worker is to grab Movie metadata");
                BeginInvoke((MethodInvoker)delegate { lblProgress.Text = @"Getting Metadata"; });
                var movie = GetMovieObjectFromSelection();
                var dlInfo = movie.StreamInformation;
                dlInfo.DownloadPath = GlobalStaticVars.Settings.Generic.DownloadDirectory + @"\Movies";
                GlobalStaticVars.Queue.Add(dlInfo);
            }

            AddToLog("Worker is to invoke downloader thread");
            BeginInvoke((MethodInvoker)delegate
            {
                StartDownload(GlobalStaticVars.Queue, GlobalStaticVars.Settings.Generic.DownloadDirectory);
                AddToLog("Worker has started the download process");
            });
        }

        #endregion BackgroundWorkers

        #region UpdateCallbackWorkers

        private void WorkerUpdateContentView(object sender, WaitWindowEventArgs e)
        {
            var doc = (XmlDocument)e.Arguments[0];
            UpdateContentViewWorker(doc, (bool)e.Arguments[1]);
        }

        private void WorkerRenderContentView(object sender, WaitWindowEventArgs e)
        {
            var t = (DataTable)e.Arguments[0];
            RenderContentView(t);
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

        #endregion UpdateCallbackWorkers

        #region ContentRenderers

        private void RenderContentView(DataTable content)
        {
            if (!(content == null))
            {
                ClearTVViews();
                ClearContentView();

                var wantedColumns = GlobalStaticVars.Settings.DataDisplay.ContentView.DisplayColumns;
                var wantedCaption = GlobalStaticVars.Settings.DataDisplay.ContentView.DisplayCaptions;

                var info = new RenderStruct
                {
                    Data = content,
                    WantedColumns = wantedColumns,
                    WantedCaption = wantedCaption
                };

                GlobalTables.ContentViewTable = GenericRenderer.RenderView(info, dgvContent);

                SelectMoviesTab();
            }
        }

        private void SelectMoviesTab(bool checkSelected = true)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    if (checkSelected)
                    {
                        if (tabMain.SelectedTab != tabMovies) tabMain.SelectedTab = tabMovies;
                    }
                    else
                    {
                        tabMain.SelectedTab = tabMovies;
                    }
                });
            }
            else
            {
                if (checkSelected)
                {
                    if (tabMain.SelectedTab != tabMovies) tabMain.SelectedTab = tabMovies;
                }
                else
                {
                    tabMain.SelectedTab = tabMovies;
                }
            }
        }

        private void SelectTVTab(bool checkSelected = true)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    if (checkSelected)
                    {
                        if (tabMain.SelectedTab != tabTV) tabMain.SelectedTab = tabTV;
                    }
                    else
                    {
                        tabMain.SelectedTab = tabTV;
                    }
                });
            }
            else
            {
                if (checkSelected)
                {
                    if (tabMain.SelectedTab != tabTV) tabMain.SelectedTab = tabTV;
                }
                else
                {
                    tabMain.SelectedTab = tabTV;
                }
            }
        }

        private void ClearContentView()
        {
            dgvContent.DataSource = null;
        }

        private void ClearTVViews()
        {
            dgvSeasons.DataSource = null;
            dgvEpisodes.DataSource = null;
            dgvTVShows.DataSource = null;
        }

        private void ClearLibraryViews()
        {
            dgvLibrary.DataSource = null;
        }

        private void RenderTVView(DataTable content)
        {
            if (content == null)
            {
            }
            else
            {
                ClearTVViews();
                ClearContentView();

                var wantedColumns = GlobalStaticVars.Settings.DataDisplay.TVView.DisplayColumns;
                var wantedCaption = GlobalStaticVars.Settings.DataDisplay.TVView.DisplayCaptions;

                var info = new RenderStruct
                {
                    Data = content,
                    WantedColumns = wantedColumns,
                    WantedCaption = wantedCaption
                };

                GlobalTables.TvViewTable = GenericRenderer.RenderView(info, dgvTVShows);

                SelectTVTab();
            }
        }

        private void RenderSeriesView(DataTable content)
        {
            if (content == null)
            {
            }
            else
            {
                var wantedColumns = GlobalStaticVars.Settings.DataDisplay.SeriesView.DisplayColumns;
                var wantedCaption = GlobalStaticVars.Settings.DataDisplay.SeriesView.DisplayCaptions;

                var info = new RenderStruct
                {
                    Data = content,
                    WantedColumns = wantedColumns,
                    WantedCaption = wantedCaption
                };

                GenericRenderer.RenderView(info, dgvSeasons);
            }
        }

        private void RenderEpisodesView(DataTable content)
        {
            if (content == null)
            {
            }
            else
            {
                var wantedColumns = GlobalStaticVars.Settings.DataDisplay.EpisodesView.DisplayColumns;
                var wantedCaption = GlobalStaticVars.Settings.DataDisplay.EpisodesView.DisplayCaptions;

                var info = new RenderStruct
                {
                    Data = content,
                    WantedColumns = wantedColumns,
                    WantedCaption = wantedCaption
                };

                GenericRenderer.RenderView(info, dgvEpisodes);
            }
        }

        private void DgvLibrary_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvLibrary.SortOrder.ToString() == "Descending") // Check if sorting is Descending
                GlobalTables.SectionsTable.DefaultView.Sort =
                dgvLibrary.SortedColumn.Name + " DESC"; // Get Sorted Column name and sort it in Descending order
            else
                GlobalTables.SectionsTable.DefaultView.Sort =
                dgvLibrary.SortedColumn.Name + " ASC"; // Otherwise sort it in Ascending order
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

            GenericRenderer.RenderView(info, dgvLibrary);
        }

        #endregion ContentRenderers

        #region UpdateWaitWorkers

        private void UpdateContentView(XmlDocument content, bool isTVShow)
        {
            WaitWindow.WaitWindow.Show(WorkerUpdateContentView, "Updating Content", content, isTVShow);
        }

        private void UpdateSeriesView(XmlDocument content)
        {
            WaitWindow.WaitWindow.Show(WorkerUpdateSeriesView, "Updating Series", content);
        }

        private void UpdateEpisodeView(XmlDocument content)
        {
            WaitWindow.WaitWindow.Show(WorkerUpdateEpisodesView, "Updating Episodes", content);
        }

        private void PopulateLibrary(XmlDocument content)
        {
            WaitWindow.WaitWindow.Show(WorkerUpdateLibraryView, "Updating Library", content);
        }

        #endregion UpdateWaitWorkers

        #region PlexAPIWorkers

        private void GetServerListWorker(object sender, WaitWindowEventArgs e)
        {
            Helpers.CacheStructureBuilder();
            if (!GlobalStaticVars.Settings.ConnectionInfo.RelaysOnly)
            {
                if (ServerCaching.ServerInCache(GlobalStaticVars.User.authenticationToken))
                {
                    try
                    {
                        e.Result = ServerCaching.ServerFromCache(GlobalStaticVars.User.authenticationToken);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        var result = GlobalStaticVars.Plex.GetServers(GlobalStaticVars.User);
                        ServerCaching.ServerToCache(result, GlobalStaticVars.User.authenticationToken);
                        e.Result = result;
                        LoggingHelpers.RecordException(ex.Message, "ServerIOAccessError");
                    }
                }
                else
                {
                    var result = GlobalStaticVars.Plex.GetServers(GlobalStaticVars.User);
                    ServerCaching.ServerToCache(result, GlobalStaticVars.User.authenticationToken);
                    e.Result = result;
                }
            }
            else
            {
                var result = Relays.GetServerRelays(GlobalStaticVars.User.authenticationToken);
                e.Result = result;
            }
        }

        private void GetMovieObjectFromSelectionWorker(object sender, WaitWindowEventArgs e)
        {
            e.Result = GetMovieObjectFromSelection();
        }

        private void GetTVObjectFromSelectionWorker(object sender, WaitWindowEventArgs e)
        {
            e.Result = GetTVObjectFromSelection();
        }

        #endregion PlexAPIWorkers

        #endregion Workers

        #region Logging

        private void AddToLog(string logEntry)
        {
            logIncrementer += 1;
            string[] headers =
            {
                "ID", "DateTime", "Entry"
            };
            string[] logEntryToAdd =
            {
                logIncrementer.ToString(), DateTime.Now.ToString(), logEntry
            };
            var logLine = ">>" + logEntry;
            if (lstLog.InvokeRequired)
                lstLog.BeginInvoke((MethodInvoker)delegate { lstLog.Items.Add(logLine); });
            else
                lstLog.Items.Add(logLine);
            if (GlobalStaticVars.Settings.Logging.EnableGenericLogDel)
                LoggingHelpers.LogDelWriter("PlexDL.logdel", headers, logEntryToAdd);
        }

        private void DGVDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            var parent = (DataGridView)sender;
            //don't show the event to the user; but log it.
            AddToLog("Experienced data error in " + parent.Name);
            e.Cancel = true;
        }

        #endregion Logging

        #region Download

        #region DownloadInfoGatherers

        private DownloadInfo GetContentDownloadInfo_Xml(XmlDocument xml)
        {
            try
            {
                if (Methods.PlexXmlValid(xml))
                {
                    var obj = new DownloadInfo();

                    AddToLog("Creating new datasets");
                    var sections = new DataSet();
                    sections.ReadXml(new XmlNodeReader(xml));

                    var part = sections.Tables["Part"];
                    var video = sections.Tables["Video"].Rows[0];
                    var title = video["title"].ToString();
                    if (title.Length > GlobalStaticVars.Settings.Generic.DefaultStringLength)
                        title = title.Substring(0, GlobalStaticVars.Settings.Generic.DefaultStringLength);
                    var thumb = video["thumb"].ToString();
                    var thumbnailFullUri = "";
                    if (string.IsNullOrEmpty(thumb))
                    {
                    }
                    else
                    {
                        var baseUri = GetBaseUri(false).TrimEnd('/');
                        thumbnailFullUri = baseUri + thumb + "?X-Plex-Token=" + GetToken();
                    }

                    var partRow = part.Rows[0];

                    var filePart = "";
                    var container = "";
                    if (partRow["key"] != null)
                        filePart = partRow["key"].ToString();
                    if (partRow["container"] != null)
                        container = partRow["container"].ToString();
                    var byteLength = Convert.ToInt64(partRow["size"]);
                    var contentDuration = Convert.ToInt32(partRow["duration"]);
                    var fileName = Methods.RemoveIllegalCharacters(title + "." + container);

                    var link = GetBaseUri(false).TrimEnd('/') + filePart + "/?X-Plex-Token=" + GetToken();
                    obj.Link = link;
                    obj.Container = container;
                    obj.ByteLength = byteLength;
                    obj.ContentDuration = contentDuration;
                    obj.FileName = fileName;
                    obj.ContentTitle = title;
                    obj.ContentThumbnailUri = thumbnailFullUri;

                    return obj;
                }

                return new DownloadInfo();
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "DownloadXmlError");
                return null;
            }
        }

        #endregion DownloadInfoGatherers

        #region DownloadMethods

        private void CancelDownload()
        {
            if (wkrGetMetadata.IsBusy) wkrGetMetadata.Abort();
            if (Flags.IsEngineRunning)
            {
                GlobalStaticVars.Engine.Cancel();
                GlobalStaticVars.Engine.Clear();
            }

            if (Flags.IsDownloadRunning)
            {
                SetProgressLabel("Download Cancelled");
                SetDownloadStart();
                SetResume();
                pbMain.Value = pbMain.Maximum;
                btnPause.Enabled = false;
                AddToLog("Download Cancelled");
                Flags.IsDownloadRunning = false;
                Flags.IsDownloadPaused = false;
                Flags.IsEngineRunning = false;
                Flags.IsDownloadQueueCancelled = true;
                downloadsSoFar = 0;
                DownloadTotal = 0;
                DownloadIndex = 0;
                MessageBox.Show("Download cancelled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartDownloadEngine()
        {
            GlobalStaticVars.Engine.QueueProgressChanged += Engine_DownloadProgressChanged;
            GlobalStaticVars.Engine.QueueCompleted += Engine_DownloadCompleted;

            GlobalStaticVars.Engine.StartAsync();
            //MessageBox.Show("Started!");
            AddToLog("Download is Progressing");
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
                AddToLog("Download directory updated to " + GlobalStaticVars.Settings.Generic.DownloadDirectory);
            }
        }

        private void SetDownloadCompleted()
        {
            pbMain.Value = pbMain.Maximum;
            btnPause.Enabled = false;
            SetResume();
            SetDownloadStart();
            SetProgressLabel("Download Completed");
            AddToLog("Download completed");
            GlobalStaticVars.Engine.Clear();
            Flags.IsDownloadRunning = false;
            Flags.IsDownloadPaused = false;
            Flags.IsEngineRunning = false;
        }

        private void StartDownload(List<DownloadInfo> queue, string location)
        {
            AddToLog("Download Process Started");
            pbMain.Value = 0;

            AddToLog("Starting HTTP Engine");
            GlobalStaticVars.Engine = new DownloadQueue();
            if (queue.Count > 1)
            {
                foreach (var dl in queue)
                {
                    var fqPath = dl.DownloadPath + @"\" + dl.FileName;
                    if (File.Exists(fqPath))
                        AddToLog(dl.FileName + " already exists; will not download.");
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
                    AddToLog(dl.FileName + " already exists; get user confirmation.");
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
            var CurrentProgress = Math.Round(GlobalStaticVars.Engine.CurrentProgress);
            var speed = Methods.FormatBytes(GlobalStaticVars.Engine.CurrentDownloadSpeed) + "/s";
            var order = "(" + (GlobalStaticVars.Engine.CurrentIndex + 1) + "/" + GlobalStaticVars.Engine.QueueLength + ")";

            SetProgressLabel(CurrentProgress + "% " + order + " @ " + speed);

            pbMain.Value = (int)CurrentProgress;

            //MessageBox.Show("Started!");
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
                    if (Flags.IsTVShow)
                        RenderTVView(GlobalTables.TitlesTable);
                    else
                        RenderContentView(GlobalTables.TitlesTable);
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
                AddToLog("Title search requested");
                if (dgvContent.Rows.Count > 0 || dgvTVShows.Rows.Count > 0)
                {
                    RenderStruct info;
                    DataGridView dgv;
                    if (Flags.IsTVShow)
                    {
                        dgv = dgvTVShows;
                        info = new RenderStruct()
                        {
                            Data = GlobalTables.TvViewTable,
                            WantedCaption = GlobalStaticVars.Settings.DataDisplay.TVView.DisplayCaptions,
                            WantedColumns = GlobalStaticVars.Settings.DataDisplay.TVView.DisplayColumns
                        };
                    }
                    else
                    {
                        dgv = dgvContent;
                        info = new RenderStruct()
                        {
                            Data = GlobalTables.ContentViewTable,
                            WantedCaption = GlobalStaticVars.Settings.DataDisplay.ContentView.DisplayCaptions,
                            WantedColumns = GlobalStaticVars.Settings.DataDisplay.ContentView.DisplayColumns
                        };
                    }
                    if (Search.RunTitleSearch(dgv, info, true))
                    {
                        Flags.IsFiltered = true;
                        SetCancelSearch();
                    }
                    else
                    {
                        Flags.IsFiltered = false;
                        GlobalTables.FilteredTable = null;
                        SetStartSearch();
                    }
                }
                else
                {
                    AddToLog("No data to search");
                }
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "SearchError");
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Search

        #region UIMethods

        private void DGVContentEnabled(bool enabled)
        {
            if (dgvContent.InvokeRequired)
                dgvContent.BeginInvoke((MethodInvoker)delegate { dgvContent.Enabled = enabled; });
            else
                dgvContent.Enabled = enabled;
        }

        private void DGVLibraryEnabled(bool enabled)
        {
            if (dgvLibrary.InvokeRequired)
                dgvLibrary.BeginInvoke((MethodInvoker)delegate { dgvLibrary.Enabled = enabled; });
            else
                dgvLibrary.Enabled = enabled;
        }

        private void DGVSeasonsEnabled(bool enabled)
        {
            if (dgvSeasons.InvokeRequired)
                dgvSeasons.BeginInvoke((MethodInvoker)delegate { dgvSeasons.Enabled = enabled; });
            else
                dgvSeasons.Enabled = enabled;
        }

        /// <summary>
        ///    Thread-safe way of changing the progress label
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
            itmSearchStatus.Text = @"Not Filtered";
        }

        private void SetCancelSearch()
        {
            if (GlobalTables.FilteredTable != null && GlobalTables.TitlesTable != null)
                itmSearchStatus.Text = @"Filtered " + GlobalTables.FilteredTable.Rows.Count.ToString() + "/" + GlobalTables.TitlesTable.Rows.Count.ToString();
            itmStartSearch.Text = @"Clear Search";
        }

        private void SetConnect()
        {
            itmDisconnect.Enabled = false;
        }

        private void SetDisconnect()
        {
            itmDisconnect.Enabled = true;
        }

        #endregion UIMethods

        private string GetToken()
        {
            return GlobalStaticVars.Svr.accessToken;
        }

        private void doConnectFromServer(Server s)
        {
            string address = s.address;
            int port = s.port;

            ConnectionInfo connectInfo;

            connectInfo = new ConnectionInfo()
            {
                PlexAccountToken = GetToken(),
                PlexAddress = address,
                PlexPort = port,
                RelaysOnly = GlobalStaticVars.Settings.ConnectionInfo.RelaysOnly
            };

            GlobalStaticVars.Settings.ConnectionInfo = connectInfo;

            string uri = GetBaseUri(true);
            //MessageBox.Show(uri);
            XmlDocument reply = (XmlDocument)PlexDL.WaitWindow.WaitWindow.Show(XmlGet.GetXMLTransactionWorker, "Connecting", new object[] { uri });
            Flags.IsConnected = true;
            if (GlobalStaticVars.Settings.Generic.ShowConnectionSuccess)
            {
                MessageBox.Show("Connection successful!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            SetProgressLabel("Connected");
            SetDisconnect();
            if (reply.ChildNodes.Count != 0)
            {
                PopulateLibrary(reply);
            }
            Flags.IsConnectAgainDisabled = true;
        }

        #region FormEvents

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
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
                        AddToLog("PlexDL Exited");
                        e.Cancel = false;
                    }
                }
            }
            else
            {
                AddToLog("PlexDL Exited");
            }
        }

        private void ResetDownloadDirectory()
        {
            var curUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            GlobalStaticVars.Settings.Generic.DownloadDirectory = curUser + "\\Videos\\PlexDL";
            if (!Directory.Exists(GlobalStaticVars.Settings.Generic.DownloadDirectory))
                Directory.CreateDirectory(GlobalStaticVars.Settings.Generic.DownloadDirectory);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                ResetDownloadDirectory();
                AddToLog("PlexDL Started");
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "StartupError");
                MessageBox.Show("Startup Error:\n\n" + ex, "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateFromLibraryKey(string key, bool isTVShow)
        {
            object[] args =
            {
                key, isTVShow
            };
            WaitWindow.WaitWindow.Show(UpdateFromLibraryKey_Worker, "Getting Metadata", args);
        }

        private void UpdateFromLibraryKey_Worker(object sender, WaitWindowEventArgs e)
        {
            var isTVShow = (bool)e.Arguments[1];
            var key = (string)e.Arguments[0];
            try
            {
                AddToLog("Requesting ibrary contents");
                var contentUri = Uri + key + "/all/?X-Plex-Token=";
                var contentXml = XmlGet.GetXMLTransaction(contentUri);

                _contentXmlDoc = contentXml;
                UpdateContentView(contentXml, isTVShow);
            }
            catch (WebException ex)
            {
                LoggingHelpers.RecordException(ex.Message, "UpdateLibraryError");
                if (ex.Status == WebExceptionStatus.ProtocolError)
                    if (ex.Response is HttpWebResponse response)
                        switch ((int)response.StatusCode)
                        {
                            case 401:
                                MessageBox.Show(
                                    "The web server denied access to the resource. Check your token and try again. (401)");
                                break;

                            case 404:
                                MessageBox.Show(
                                    "The web server couldn't serve the request because it couldn't find the resource specified. (404)");
                                break;

                            case 400:
                                MessageBox.Show(
                                    "The web server couldn't serve the request because the request was bad. (400)");
                                break;
                        }
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "UpdateLibraryError");
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CxtLibrarySections_Opening(object sender, CancelEventArgs e)
        {
            if (dgvLibrary.Rows.Count == 0) e.Cancel = true;
        }

        #endregion FormEvents

        #region DGVRowChanges

        private void DgvLibrary_OnRowChange(object sender, EventArgs e)
        {
            if (dgvLibrary.SelectedRows.Count == 1 && Flags.IsLibraryFilled)
            {
                AddToLog("Selection Changed");
                //don't re-render the grids when clearing the search; this would end badly for performance reasons.
                ClearSearch(false);
                AddToLog("Cleared possible searches");
                var index = GetTableIndexFromDGV(dgvLibrary, GlobalTables.SectionsTable);
                var r = GetDataRowLibrary(index);

                var key = "";
                var type = "";
                if (r != null)
                {
                    if (r["key"] != null)
                        key = r["key"].ToString();
                    if (r["type"] != null)
                        type = r["type"].ToString();
                }

                if (type == "show")
                    UpdateFromLibraryKey(key, true);
                else if (type == "movie") UpdateFromLibraryKey(key, false);
            }
        }

        private void DgvSeasons_OnRowChange(object sender, EventArgs e)
        {
            if (dgvSeasons.SelectedRows.Count == 1)
            {
                var index = GetTableIndexFromDGV(dgvSeasons, GlobalTables.SeriesTable);
                var episodes = GetEpisodeXml(index);
                UpdateEpisodeView(episodes);
            }
        }

        private void DgvContent_OnRowChange(object sender, EventArgs e)
        {
            //nothing, more or less.
        }

        //when the user double-clicks a cell in dgvContent (Movies), show a messagebox with the cell content
        private void DgvContent_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvContent.Rows.Count > 0)
                {
                    var value = dgvContent.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    MessageBox.Show(value, "Cell Content", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                //ignore
            }
        }

        //when the user double-clicks a cell in dgvSeasons (TV), show a messagebox with the cell content
        private void DgvSeasons_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvSeasons.Rows.Count > 0)
                {
                    var value = dgvSeasons.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    MessageBox.Show(value, "Cell Content", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                //ignore
            }
        }

        //when the user double-clicks a cell in dgvEpisodes (TV), show a messagebox with the cell content
        private void DgvEpisodes_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvEpisodes.Rows.Count > 0)
                {
                    var value = dgvEpisodes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    MessageBox.Show(value, "Cell Content", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                //ignore
            }
        }

        //when the user double-clicks a cell in dgvTVShows (TV), show a messagebox with the cell content
        private void DgvTVShows_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvTVShows.Rows.Count > 0)
                {
                    var value = dgvTVShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    MessageBox.Show(value, "Cell Content", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                //ignore
            }
        }

        private void DgvTVShows_OnRowChange(object sender, EventArgs e)
        {
            if (dgvTVShows.SelectedRows.Count == 1)
            {
                int index;
                if (Flags.IsFiltered)
                    index = GetTableIndexFromDGV(dgvTVShows, GlobalTables.FilteredTable);
                else
                    index = GetTableIndexFromDGV(dgvTVShows, GlobalTables.TitlesTable);

                if (Flags.IsTVShow)
                {
                    var series = GetSeriesXml(index);
                    UpdateSeriesView(series);
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
                var uri = GetBaseUri(true);
                var reply = (XmlDocument)WaitWindow.WaitWindow.Show(XmlGet.GetXMLTransactionWorker, "Connecting", uri);
                MessageBox.Show("Connection successful!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (WebException ex)
            {
                LoggingHelpers.RecordException(ex.Message, "TestConnectionError");
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    if (ex.Response is HttpWebResponse response)
                        switch ((int)response.StatusCode)
                        {
                            case 401:
                                MessageBox.Show(
                                    "The web server denied access to the resource. Check your token and try again. (401)");
                                break;

                            case 404:
                                MessageBox.Show(
                                    "The web server couldn't serve the request because it couldn't find the resource specified. (404)");
                                break;

                            case 400:
                                MessageBox.Show(
                                    "The web server couldn't serve the request because the request was bad. (400)");
                                break;
                        }
                    else
                        MessageBox.Show("Unknown status code; the server failed to serve the request. (?)");
                }
                else
                {
                    MessageBox.Show("An unknown error occurred:\n\n" + ex, "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void DoDownloadAllEpisodes()
        {
            AddToLog("Awaiting download safety checks");
            if (!Flags.IsDownloadRunning && !Flags.IsEngineRunning)
            {
                AddToLog("Download process is starting");
                SetProgressLabel("Waiting");
                Flags.IsDownloadAllEpisodes = true;
                DownloadTotal = GlobalTables.EpisodesTable.Rows.Count;
                Flags.IsDownloadRunning = true;
                if (wkrGetMetadata.IsBusy) wkrGetMetadata.Abort();
                wkrGetMetadata.RunWorkerAsync();
                AddToLog("Worker invoke process started");
                SetDownloadCancel();
            }
            else
            {
                AddToLog("Download process failed; download is already running.");
            }
        }

        private void DoDownloadSelected()
        {
            AddToLog("Awaiting download safety checks");
            if (!Flags.IsDownloadRunning && !Flags.IsEngineRunning)
            {
                AddToLog("Download process is starting");
                SetProgressLabel("Waiting");
                Flags.IsDownloadAllEpisodes = false;
                DownloadTotal = 1;
                Flags.IsDownloadRunning = true;
                if (wkrGetMetadata.IsBusy) wkrGetMetadata.Abort();
                wkrGetMetadata.RunWorkerAsync();
                AddToLog("Worker invoke process started");
                SetDownloadCancel();
            }
            else
            {
                AddToLog("Download process failed; download is already running.");
            }
        }

        private void ResetContentGridViews()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    dgvContent.DataSource = null;
                    dgvSeasons.DataSource = null;
                    dgvEpisodes.DataSource = null;
                });
            }
            else
            {
                dgvContent.DataSource = null;
                dgvSeasons.DataSource = null;
                dgvEpisodes.DataSource = null;
            }
        }

        private void StartStreaming(PlexObject stream)
        {
            //so that cxtStreamOptions can access the current stream's information, a global object has to be used.
            GlobalStaticVars.CurrentStream = stream;
            if (GlobalStaticVars.Settings.Player.PlaybackEngine == PlaybackMode.PVSPlayer)
            {
                PVSLauncher.LaunchPVS(stream, ReturnCorrectTable());
            }
            else if (GlobalStaticVars.Settings.Player.PlaybackEngine == PlaybackMode.VLCPlayer)
            {
                VLCLauncher.LaunchVLC(stream);
            }
            else if (GlobalStaticVars.Settings.Player.PlaybackEngine == PlaybackMode.Browser)
            {
                BrowserLauncher.LaunchBrowser(stream);
            }
            else if (GlobalStaticVars.Settings.Player.PlaybackEngine == PlaybackMode.MenuSelector)
            {
                //display the options menu where the
                var loc = new Point(Location.X + btnHTTPPlay.Location.X, Location.Y + btnHTTPPlay.Location.Y);
                cxtStreamOptions.Show(loc);
            }
            else
            {
                MessageBox.Show("Unrecognised Playback Mode (\"" + GlobalStaticVars.Settings.Player.PlaybackEngine + "\")",
                    "Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                AddToLog("Invalid Playback Mode: " + GlobalStaticVars.Settings.Player.PlaybackEngine);
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
            if (dgvContent.SelectedRows.Count == 0) e.Cancel = true;
        }

        private void Metadata(PlexObject result = null)
        {
            if (dgvContent.SelectedRows.Count == 1 || dgvEpisodes.SelectedRows.Count == 1)
            {
                if (!Flags.IsDownloadRunning && !Flags.IsEngineRunning)
                {
                    if (result == null)
                    {
                        if (!Flags.IsTVShow)
                            result = (PlexObject)WaitWindow.WaitWindow.Show(GetMovieObjectFromSelectionWorker,
                                "Getting Metadata");
                        else
                            result = (PlexObject)WaitWindow.WaitWindow.Show(GetTVObjectFromSelectionWorker,
                                "Getting Metadata");
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
            else if (dgvContent.Rows.Count == 0 && dgvTVShows.Rows.Count == 0)
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

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (Settings frm = new Settings())
                frm.ShowDialog();
        }

        private void itmServerManager_Click(object sender, EventArgs e)
        {
            if (wininet.CheckForInternetConnection())
            {
                using (ServerManager frm = new ServerManager())
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        GlobalStaticVars.Settings.ConnectionInfo.PlexAccountToken = frm.SelectedServer.accessToken;
                        GlobalStaticVars.Settings.ConnectionInfo.PlexAddress = frm.SelectedServer.address;
                        GlobalStaticVars.Settings.ConnectionInfo.PlexPort = frm.SelectedServer.port;
                        GlobalStaticVars.Svr = frm.SelectedServer;
                        doConnectFromServer(frm.SelectedServer);
                    }
                }
            }
            else
            {
                MessageBox.Show(@"No internet connection. Please connect to a network before attempting to start a Plex server connection.", @"Network Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
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
            if (dgvContent.SelectedRows.Count == 1 || dgvEpisodes.SelectedRows.Count == 1)
            {
                if (!Flags.IsDownloadRunning && !Flags.IsEngineRunning)
                {
                    GlobalStaticVars.Queue = new List<DownloadInfo>();
                    if (Flags.IsTVShow)
                    {
                        if (dgvEpisodes.SelectedRows.Count == 1) cxtEpisodes.Show(MousePosition);
                    }
                    else
                    {
                        DoDownloadSelected();
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
            if (dgvContent.SelectedRows.Count == 1 || dgvEpisodes.SelectedRows.Count == 1)
            {
                PlexObject result;
                if (!Flags.IsTVShow)
                {
                    result = (PlexMovie)WaitWindow.WaitWindow.Show(GetMovieObjectFromSelectionWorker,
                        "Getting Metadata");
                }
                else
                {
                    if (dgvEpisodes.SelectedRows.Count == 1)
                    {
                        result = (PlexTVShow)WaitWindow.WaitWindow.Show(GetTVObjectFromSelectionWorker,
                            "Getting Metadata");
                    }
                    else
                    {
                        MessageBox.Show(@"No episode is selected", @"Validation Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }

                StartStreaming(result);
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
                frm.ShowDialog();
        }

        private void itmCacheMetrics_Click(object sender, EventArgs e)
        {
            using (var frm = new CachingMetricsUI())
            {
                frm.Metrics = CachingMetrics.FromLatest();
                frm.ShowDialog();
            }
        }

        private void itmSettings_Click(object sender, EventArgs e)
        {
            using (Settings frm = new Settings())
                frm.ShowDialog();
        }
    }
}