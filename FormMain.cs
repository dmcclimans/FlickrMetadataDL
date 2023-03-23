using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlickrNet;
using System.Data.SQLite;


namespace FlickrMetadataDL
{
    public partial class FormMain : Form
    {
        private Settings Settings { get; set; }
        private FlickrManager FlickrManager { get; set; }

        FlickrNet.PhotoSearchExtras SearchExtras =
#if true
            FlickrNet.PhotoSearchExtras.All;
#else
            // Used for testing. The app is faster if you don't download all attributes of the
            // photos. My testing showed about half the time to execute a search.
            FlickrNet.PhotoSearchExtras.SmallUrl |
            FlickrNet.PhotoSearchExtras.LargeUrl |
            FlickrNet.PhotoSearchExtras.OriginalUrl |
            FlickrNet.PhotoSearchExtras.Description |
            FlickrNet.PhotoSearchExtras.OwnerName |
            FlickrNet.PhotoSearchExtras.Tags |
            FlickrNet.PhotoSearchExtras.DateTaken;
#endif
        // Error message returned from BG search methods. Empty if no error.
        private string BGErrorMessage { get; set; }

        // User corresponding to Settings.FlickrSearchAccountName. This is the user
        // that is being searched.
        private User SearchAccountUser;

        // Number of photos found by search
        private int SearchPhotoCount = 0;

        // The list of photosets (albums) returned by GetAlbums, and used during a search.
        private SortableBindingList<Photoset> PhotosetList { get; set; }

        private bool FormIsLoaded { get; set; } = false;

        // The number of times we will try some Flickr commands before giving up. This only applies to
        // commands that can take a long time.
        private const int FlickrMaxTries = 3;

        // Checkbox that is put in the header of the dgvPhotosets.
        private CheckBox cbHeader;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Settings = Settings.Load();
            if (Settings.FormMainLocation.X != 0 ||
                  Settings.FormMainLocation.Y != 0)
            {
                this.Location = Settings.FormMainLocation;
            }
            if (Settings.FormMainSize.Height != 0 ||
                  Settings.FormMainSize.Width != 0)
            {
                this.Size = Settings.FormMainSize;
            }

            FlickrManager = new FlickrManager(Settings);

            // Bind the login account list.
            cbLoginAccount.DataSource = Settings.FlickrLoginAccountList;
            cbLoginAccount.DisplayMember = "CombinedName";
            if (Settings.FlickrLoginAccountName.Length > 0)
            {
                int index = cbLoginAccount.FindString(Settings.FlickrLoginAccountName);
                if (index >= 0)
                {
                    cbLoginAccount.SelectedIndex = index;
                }
            }

            cbSearchAccount.DataSource = Settings.FlickrSearchAccountList;
            cbSearchAccount.DisplayMember = "CombinedName";
            if (Settings.FlickrSearchAccountName.Length > 0)
            {
                int index = cbSearchAccount.FindString(Settings.FlickrSearchAccountName);
                if (index >= 0)
                {
                    cbSearchAccount.SelectedIndex = index;
                }
            }

            // For the Album (photoset) DataGridView, add a "select all" checkbox to the header row
            // Set checkbox header to center of header cell. This is kluge code from the internet,
            // modified slightly so it looks right on my system.
            Rectangle rect = dgvPhotosets.GetCellDisplayRectangle(0, -1, true);
            rect.X = rect.X + rect.Width / 4;
            rect.Y = rect.Y + 2;

            cbHeader = new CheckBox
            {
                Name = "cbHeader",
                Size = new System.Drawing.Size(18, 18),
                Location = rect.Location
            };
            cbHeader.CheckedChanged += new EventHandler(cbHeader_CheckedChanged);
            dgvPhotosets.Controls.Add(cbHeader);

            // set up bindings
            chkSearchAllPhotos.DataBindings.Add("Checked", Settings, "SearchAllPhotos", true, DataSourceUpdateMode.OnPropertyChanged);
            btnGetAlbums.DataBindings.Add("Enabled", Settings, "GetAlbumsButtonEnabled");
            btnSearch.DataBindings.Add("Enabled", Settings, "SearchButtonEnabled");
            chkFilterDate.DataBindings.Add("Checked", Settings, "FilterByDate", true, DataSourceUpdateMode.OnPropertyChanged);
            dateTimePickerStart.DataBindings.Add("Value", Settings, "StartDate", true, DataSourceUpdateMode.OnPropertyChanged);
            dateTimePickerStart.DataBindings.Add("Enabled", Settings, "FilterDateEnabled");
            dateTimePickerStop.DataBindings.Add("Value", Settings, "StopDate", true, DataSourceUpdateMode.OnPropertyChanged);
            dateTimePickerStop.DataBindings.Add("Enabled", Settings, "FilterDateEnabled");
            chkFindAllAlbums.DataBindings.Add("Checked", Settings, "FindAllAlbums", true, DataSourceUpdateMode.OnPropertyChanged);
            txtOutputFile.DataBindings.Add("Text", Settings, "OutputFilename");

            FormIsLoaded = true;
        }

        private void cbHeader_CheckedChanged(object sender, EventArgs e)
        {
            bool enable = ((CheckBox)dgvPhotosets.Controls.Find("cbHeader", true)[0]).Checked;
            for (int i=0; i<dgvPhotosets.RowCount; i++)
            {
                dgvPhotosets[0, i].Value = enable;
            }
            dgvPhotosets.EndEdit();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save the form location
            Settings.FormMainLocation = this.Location;
            Settings.FormMainSize = this.Size;

            Settings.Save();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BGErrorMessage = "";

            SearchAccountUser = (User)cbSearchAccount.SelectedItem;
            if (SearchAccountUser == null)
            {
                MessageBox.Show("No search account selected");
                return;
            }

            Stopwatch RunTimer = Stopwatch.StartNew();

            bool searchSuccessful = false;
            if (Settings.SearchAllPhotos)
            {
                searchSuccessful = SearchAllPhotos();
            }
            else
            {
                searchSuccessful = SearchPhotosets();
            }

            RunTimer.Stop();
            if (searchSuccessful)
            {
                if (String.IsNullOrWhiteSpace(BGErrorMessage))
                {
                    MessageBox.Show(String.Format("Search found {0} photos in {1}:{2:mm}:{2:ss}.",
                        SearchPhotoCount.ToString(),
                        (int)RunTimer.Elapsed.TotalHours, RunTimer.Elapsed, RunTimer.Elapsed));
                }
                else if (BGErrorMessage.Contains("Too many photos"))
                {
                    int index = BGErrorMessage.IndexOf(":");
                    int count = 0;
                    if (index >= 0)
                    {
                        int.TryParse(BGErrorMessage.Substring(index + 1), out count);
                    }
                    if (Settings.SearchAllPhotos)
                    {
                        MessageBox.Show("Too many photos found.\r\n\r\n" +
                            "Flickr limits the number of photos returned from a search to about 4000. " +
                            $"This search found {count} photos and the resulting photo list is not accurate.\r\n\r\n" +
                            "Reduce the size of the search by either searching by album or limiting the search by date.");
                    }
                    else
                    {
                        // It is not clear from the FlickrApi documentation whether the 4000 photo limit applies
                        // when searching albums (Photosets.GetPhotos).
                        // At present I assume it does not. This seems consistent with the fact that you cannot
                        // filter a Photoset.GetPhotos call by date.
                        // This error message is not currently returned by my code when searching albums, so
                        // you will never see it. But there is some disabled (ifdef) code in BGSearchPhotosets
                        // that could be enabled to return this error.
                        MessageBox.Show("Too many photos found.\r\n\r\n" +
                            "Flickr limits the number of photos returned from a search to about 4000. " +
                            $"One of the album searches found {count} photos and the resulting photo list is not accurate.\r\n\r\n" +
                            "Reduce the size of the search by reducing the number of photos in the albums.");
                    }
                }
                else
                {
                    MessageBox.Show(BGErrorMessage);
                }
            }
        }

        private bool OverwriteFile()
        {
            string filename = Settings.OutputFilename;
            try
            {
                if (String.IsNullOrWhiteSpace(filename))
                {
                    MessageBox.Show("No output filename specified");
                    return false;
                }
                string folder = Path.GetDirectoryName(filename);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                if (File.Exists(filename))
                {
                    DialogResult result = MessageBox.Show(
                        "The file \"" + Path.GetFileName(filename) + "\" exists and will be overwritten.",
                        "FlickrMetatdataDL", MessageBoxButtons.OKCancel);
                    if (result == DialogResult.Cancel)
                        return false;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return false;
            }
            return true;
        }

        private bool SearchAllPhotos()
        {
            if (!OverwriteFile())
                return false;

            FormProgress dlg = new FormProgress("Search all photos", BGSearchPhotos);

            // Show dialog with Synchronous/blocking call.
            // BGSearchPhotos() is called by dialog.
            DialogResult result = dlg.ShowDialog();
            return result == DialogResult.OK;
        }

        private bool SearchPhotosets()
        {
            // Check for no photosets enabled
            int count = 0;
            if (PhotosetList != null)
            {
                foreach (Photoset ps in PhotosetList)
                {
                    if (ps.EnableSearch)
                        count++;
                }
            }
            if (count == 0)
            {
                MessageBox.Show("No albums enabled to search");
                return false;
            }
            if (!OverwriteFile())
                return false;

            FormProgress dlg = new FormProgress("Search albums", BGSearchPhotos);

            // Show dialog with Synchronous/blocking call.
            // BGSearchPhotos() is called by dialog.
            DialogResult result = dlg.ShowDialog();
            return result == DialogResult.OK;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout dlg = new FormAbout(Settings);
            dlg.ShowDialog(this);
        }

        private void btnGetAlbums_Click(object sender, EventArgs e)
        {
            BGErrorMessage = "";

            dgvPhotosets.Rows.Clear();

            SearchAccountUser = (User)cbSearchAccount.SelectedItem;
            if (SearchAccountUser == null)
            {
                MessageBox.Show("No search account selected");
                return;
            }
            PhotosetList = new SortableBindingList<Photoset>();

            FormProgress dlg = new FormProgress("Find albums", BGFindPhotosets);

            // Show dialog with Synchronous/blocking call.
            // BGFindPhotosets() is called by dialog.
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (!String.IsNullOrWhiteSpace(BGErrorMessage))
                {
                    MessageBox.Show(BGErrorMessage);
                }
                else
                {
                    bindingSourcePhotosets.DataSource = PhotosetList;
                    if (dgvPhotosets.Rows.Count > 0 &&
                        dgvPhotosets.Rows[0].Cells.Count > 0)
                    {
                        dgvPhotosets.CurrentCell = dgvPhotosets.Rows[0].Cells[0];
                        dgvPhotosets.Rows[0].Selected = true;
                    }
                }
            }
            else
            {
                // Search was canceled. Do nothing.
            }
        }


        private void BGFindPhotosets(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            FlickrNet.Flickr f = FlickrManager.GetFlickrAuthInstance();
            if (f == null)
            {
                BGErrorMessage = "You must authenticate before you can download data from Flickr.";
                return;
            }

            try
            {
                int page = 1;
                int perPage = 500;
                FlickrNet.PhotosetCollection photoSets = new FlickrNet.PhotosetCollection();
                FlickrNet.PhotoSearchExtras PhotosetExtras = 0;
                do
                {
                    bool success = false;
                    for (int attempt = 0; attempt < FlickrMaxTries && !success; attempt++)
                    {
                        try
                        {
                            photoSets = f.PhotosetsGetList(SearchAccountUser.UserId, page, perPage, PhotosetExtras);
                            success = true;
                        }
                        catch (FlickrNet.FlickrException ex)
                        {
                            // Save the *first* error message for display, not subsequent ones.
                            if (attempt == 0)
                                BGErrorMessage = "Album search failed. Flickr error: " + ex.Message;
                        }
                        catch (Exception ex)
                        {
                            if (attempt == 0)
                                BGErrorMessage = "Album search failed. Unexpected Flickr error: " + ex.Message;
                        }
                    }
                    if (!success)
                    {
                        return;
                    }
                    BGErrorMessage = "";

                    foreach (FlickrNet.Photoset ps in photoSets)
                    {
                        PhotosetList.Add(new Photoset(ps));
                        int index = PhotosetList.Count - 1;
                        PhotosetList[index].OriginalSortOrder = index;
                    }
                    page = photoSets.Page + 1;
                }
                while (page <= photoSets.Pages);
            }
            catch (FlickrNet.FlickrException ex)
            {
                BGErrorMessage = "Album search failed. Flickr error: " + ex.Message;
                return;
            }
            catch (Exception ex)
            {
                BGErrorMessage = "Album search failed. Unexpected error: " + ex.Message;
                return;
            }
        }

        // Background thread to search for photos
        private void BGSearchPhotos(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            worker.ReportProgress(0, "Connecting");

            // Create the database
            string DBFilename = Settings.OutputFilename;
            try
            {
                // Create new file.
                // Will overwrite any existing file.
                // If we don't do this we will open the existing file.
                SQLiteConnection.CreateFile(DBFilename);

            }
            catch (Exception exc)
            {
                BGErrorMessage = "Failed to create database.\r\nError: " + exc.Message;
                return;
            }

            SearchPhotoCount = 0;

            // Open the connection and search for photos.
            try
            {
                using (SQLiteConnection connection =
                    new SQLiteConnection("Data Source=" + DBFilename + ";Version=3;"))
                {
                    try
                    {
                        connection.Open();

                        CreateDBTables(connection);

                        using (SQLiteTransaction transaction = connection.BeginTransaction())
                        {
                            if (Settings.SearchAllPhotos)
                            {
                                BGSearchAllPhotos(worker, e, connection);
                            }
                            else
                            {
                                BGSearchPhotosets(worker, e, connection);
                            }
                            transaction.Commit();
                        }
                    }
                    catch (Exception exc)
                    {
                        BGErrorMessage = exc.Message;
                        return;
                    }
                }
            }
            catch (Exception exc)
            {
                BGErrorMessage = exc.Message;
                return;
            }
        }

        private void CreateDBTables(SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE 'Photos' (" +
                    "[PhotoID] TEXT NOT NULL PRIMARY KEY" +
                    ",[CountComments] INTEGER" +
                    ",[CountFaves] INTEGER" +
                    ",[DateTaken] TEXT" +
                    ",[DateUploaded] TEXT" +
                    ",[Description] TEXT" +
                    ",[Large1600Height] INTEGER" +
                    ",[Large1600Url] TEXT" +
                    ",[Large1600Width] INTEGER" +
                    ",[Large2048Height] INTEGER" +
                    ",[Large2048Url] TEXT" +
                    ",[Large2048Width] INTEGER" +
                    ",[LargeHeight] INTEGER" +
                    ",[LargeUrl] TEXT" +
                    ",[LargeWidth] INTEGER" +
                    ",[LargeSquareThumbnailHeight] INTEGER" +
                    ",[LargeSquareThumbnailUrl] TEXT" +
                    ",[LargeSquareThumbnailWidth] INTEGER" +
                    ",[Latitude] REAL" +
                    ",[License] TEXT" +
                    ",[Longitude] REAL" +
                    ",[Medium640Height] INTEGER" +
                    ",[Medium640Url] TEXT" +
                    ",[Medium640Width] INTEGER" +
                    ",[Medium800Height] INTEGER" +
                    ",[Medium800Url] TEXT" +
                    ",[Medium800Width] INTEGER" +
                    ",[MediumHeight] INTEGER" +
                    ",[MediumUrl] TEXT" +
                    ",[MediumWidth] INTEGER" +
                    ",[OriginalFormat] TEXT" +
                    ",[OriginalHeight] INTEGER" +
                    ",[OriginalUrl] TEXT" +
                    ",[OriginalWidth] INTEGER" +
                    ",[OwnerName] TEXT" +
                    ",[PlaceId] TEXT" +
                    ",[Rotation] INTEGER" +
                    ",[Small320Height] INTEGER" +
                    ",[Small320Url] TEXT" +
                    ",[Small320Width] INTEGER" +
                    ",[SmallHeight] INTEGER" +
                    ",[SmallUrl] TEXT" +
                    ",[SmallWidth] INTEGER" +
                    ",[SquareThumbnailHeight] INTEGER" +
                    ",[SquareThumbnailUrl] TEXT" +
                    ",[SquareThumbnailWidth] INTEGER" +
                    ",[ThumbnailHeight] INTEGER" +
                    ",[ThumbnailUrl] TEXT" +
                    ",[ThumbnailWidth] INTEGER" +
                    ",[Title] TEXT" +
                    ",[UserId] TEXT" +
                    ",[Url] TEXT" +
                    ",[Views] INTEGER" +
                    ",[WoeId] TEXT" +
                    ");";
                command.ExecuteNonQuery();

                command.CommandText = "CREATE TABLE 'PhotoTags' (" +
                    "[PhotoID] TEXT NOT NULL" +
                    ",[Tag] TEXT" +
                    ",FOREIGN KEY(PhotoID) REFERENCES Photos(PhotoID) ON DELETE CASCADE" +
                    ");";
                command.ExecuteNonQuery();

                if (Settings.FindAllAlbums || !Settings.SearchAllPhotos)
                {
                    command.CommandText = "CREATE TABLE 'Albums' (" +
                    "[AlbumID] TEXT NOT NULL PRIMARY KEY" +
                    ",[CountComments] INTEGER" +
                    ",[CountPhotos] INTEGER" +
                    ",[CountVideos] INTEGER" +
                    ",[DateCreated] TEXT" +
                    ",[DateUpdated] TEXT" +
                    ",[Description] TEXT" +
                    ",[OwnerName] TEXT" +
                    ",[SmallUrl] TEXT" +
                    ",[SquareThumbnailUrl] TEXT" +
                    ",[ThumbnailUrl] TEXT" +
                    ",[Title] TEXT" +
                    ",[Url] TEXT" +
                    ",[Views] INTEGER" +
                    ");";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE 'PhotoAlbums' (" +
                         "[PhotoID] TEXT NOT NULL" +
                         ",[AlbumID] TEXT" +
                         ",FOREIGN KEY(PhotoID) REFERENCES Photos(PhotoID) ON DELETE CASCADE ON UPDATE CASCADE" +
                         ",FOREIGN KEY(AlbumID) REFERENCES Albums(AlbumID) ON DELETE CASCADE ON UPDATE CASCADE" +
                         ");";
                    command.ExecuteNonQuery();
                }
            }
        }

        private void BGSearchAllPhotos(BackgroundWorker worker, DoWorkEventArgs e, SQLiteConnection connection)
        {
            List<FlickrNet.Photo> photoList = new List<FlickrNet.Photo>();

            try
            {
                FlickrNet.Flickr f = FlickrManager.GetFlickrAuthInstance();

                FlickrNet.PhotoSearchOptions options = new FlickrNet.PhotoSearchOptions();
                options.Extras = SearchExtras;
                options.SortOrder = FlickrNet.PhotoSearchSortOrder.DateTakenAscending;
                if (Settings.FilterByDate)
                {
                    options.MinTakenDate = Settings.StartDate.Date;
                    options.MaxTakenDate = Settings.StopDate.Date + new TimeSpan(23, 59, 59);
                }
                options.UserId = SearchAccountUser.UserId;
                options.Page = 1;
                options.PerPage = 500;

                FlickrNet.PhotoCollection photoCollection = null;
                do
                {
                    if (worker.CancellationPending) // See if cancel button was pressed.
                    {
                        return;
                    }

                    // Try searching Flickr up to FlickrMaxTries times
                    bool success = false;
                    for (int attempt = 0; attempt < FlickrMaxTries && !success; attempt++)
                    {
                        try
                        {
                            photoCollection = f.PhotosSearch(options);
                            success = true;
                        }
                        catch (FlickrNet.FlickrException ex)
                        {
                            // Save the *first* error message for display, not subsequent ones.
                            if (attempt == 0)
                                BGErrorMessage = "Search failed. Flickr error: " + ex.Message;
                        }
                        catch (Exception ex)
                        {
                            if (attempt == 0)
                                BGErrorMessage = "Search failed. Unexpected Flickr error: " + ex.Message;
                        }
                    }
                    if (!success)
                    {
                        return;
                    }
                    BGErrorMessage = "";

                    if (photoCollection != null && photoCollection.Total > 3999)
                    {
                        BGErrorMessage = $"Too many photos: {photoCollection.Total}";
                        return;
                    }

                    foreach (FlickrNet.Photo flickrPhoto in photoCollection)
                    {
                        // The list of photos from flickr should contain each photo once only.
                        // No need to check for duplicates

                        AddPhotoToDB(connection, f, flickrPhoto, null);
                    }
                    // Calculate percent complete based on how many pages we have completed.
                    int percent = (options.Page * 100 / photoCollection.Pages);
                    worker.ReportProgress(percent, "Searching all photos");

                    options.Page = photoCollection.Page + 1;
                }
                while (options.Page <= photoCollection.Pages);
            }
            catch (FlickrNet.FlickrException ex)
            {
                BGErrorMessage = "Search failed. Flickr error: " + ex.Message;
                return;
            }
            catch (Exception ex)
            {
                BGErrorMessage = "Search failed. Unexpected Flickr error: " + ex.Message;
                return;
            }
        }

        // Background worker task to search selected photosets.
        private void BGSearchPhotosets(BackgroundWorker worker, DoWorkEventArgs e, SQLiteConnection connection)
        {
            // Count the number of enabled photosets, so we can do an estimate of percent complete;
            int enabledPhotosets = 0;
            foreach (Photoset photoset in PhotosetList)
            {
                if (photoset.EnableSearch)
                {
                    enabledPhotosets++;
                }
            }

            if (enabledPhotosets == 0)
            {
                // No photosets are enabled. We are done.
                return;
            }

            int indexPhotoset = 0;

            FlickrNet.Flickr f = FlickrManager.GetFlickrAuthInstance();

            // Iterate over the photosets and get the photos from each set.
            FlickrNet.PhotosetPhotoCollection photoCollection = new FlickrNet.PhotosetPhotoCollection();

            foreach (Photoset photoset in PhotosetList)
            {
                if (worker.CancellationPending) // See if cancel button was clicked.
                {
                    return;
                }

                if (photoset.EnableSearch)
                {
                    int percent = indexPhotoset * 100 / enabledPhotosets;
                    string userState = "Searching Album " + photoset.Title;
                    worker.ReportProgress(percent, userState); // Report percent and user-status info to dialog

                    int page = 1;
                    int perpage = 500;
                    do
                    {
                        if (worker.CancellationPending) // See if cancel button was pressed.
                        {
                            return;
                        }

                        try
                        {
                            // Try searching Flickr up to FlickrMaxTries times
                            bool success = false;
                            for (int attempt = 0; attempt < FlickrMaxTries && !success; attempt++)
                            {
                                try
                                {
                                    photoCollection = f.PhotosetsGetPhotos(photoset.PhotosetId, SearchExtras, page, perpage);
                                    success = true;
                                }
                                catch (FlickrNet.FlickrException ex)
                                {
                                    // Save the *first* error message for display, not subsequent ones.
                                    if (attempt == 0)
                                        BGErrorMessage = "Search failed. Flickr error: " + ex.Message;
                                }
                                catch (Exception ex)
                                {
                                    // Save the *first* error message for display, not subsequent ones.
                                    if (attempt == 0)
                                        BGErrorMessage = "Search failed. Unexpected Flickr error: " + ex.Message;
                                }
                            }
                            if (!success)
                            {
                                return;
                            }
                            BGErrorMessage = "";

#if false
                            // It is not clear from the documentation whether the limit of 4000 photos per search applies
                            // to album searches. If an album has more than 4000 photos, is the result of GetPhotos
                            // accurate? I'm going to assume for now that it is. If not, you can enable this code.
                            if (photoCollection.Total > 3999)
                            {
                                SearchErrorMessage = $"Too many photos: {photoCollection.Total}";
                                return;
                            }
#endif
                            foreach (FlickrNet.Photo flickrPhoto in photoCollection)
                            {
                                // Filter by date, if filter option enabled and date taken is known.
                                if (!Settings.FilterByDate ||
                                    flickrPhoto.DateTakenUnknown ||
                                    (flickrPhoto.DateTaken.Date >= Settings.StartDate && flickrPhoto.DateTaken.Date <= Settings.StopDate))
                                {
                                    AddPhotoToDB(connection, f, flickrPhoto, photoset);
                                }
                            }
                            // Calculate percent complete based on both how many photo sets we have completed,
                            // plus how many pages we have read
                            percent = (indexPhotoset * 100 + page * 100 / photoCollection.Pages) / enabledPhotosets;
                            worker.ReportProgress(percent, userState);
                            page = photoCollection.Page + 1;
                        }
                        catch (FlickrNet.FlickrException ex)
                        {
                            BGErrorMessage = "Search failed. Flickr error: " + ex.Message;
                            return;
                        }
                        catch (Exception ex)
                        {
                            BGErrorMessage = "Search failed. Unexpected error: " + ex.Message;
                            return;
                        }
                    }
                    while (page <= photoCollection.Pages);

                    indexPhotoset++;
                }
            }
        }

        private void AddPhotoToDB(SQLiteConnection connection, FlickrNet.Flickr f, FlickrNet.Photo flickrPhoto,
                        Photoset photoset)
        {
            // If Find All Albums is checked, get the list of all albums this photo belongs to from Flickr.
            FlickrNet.AllContexts contexts = null;
            if (Settings.FindAllAlbums)
            {
                contexts = f.PhotosGetAllContexts(flickrPhoto.PhotoId);
            }

#if false
            // Sometimes Flickr fails to return the list of tags. Calling PhotosetsGetPhotos or
            // PhotosSearch returns a list of tags that is one element long, and that tag is the
            // empty string (""). This appears to be a problem with the FlickrAPI. In my testing,
            // it occurs repeatedly on specific photos -- but only 7 out of 2000 photos.
            //
            // To recover from this error, call TagsGetListPhoto. This will correctly return
            // the tags for the photo. We don't do this for every photo because that would be
            // slow.
            System.Collections.ObjectModel.Collection<PhotoInfoTag> tags = null;
            if (flickrPhoto.Tags.Count > 0 && String.IsNullOrWhiteSpace(flickrPhoto.Tags[0]))
            {
                tags = f.TagsGetListPhoto(flickrPhoto.PhotoId);
            }
#endif

            using (SQLiteCommand command = connection.CreateCommand())
            {
                bool photoExists = false;
                command.CommandText = "SELECT EXISTS (SELECT 1 FROM Photos WHERE [PhotoID]=@PhotoID LIMIT 1)";
                command.Parameters.AddWithValue("@PhotoID", flickrPhoto.PhotoId);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    // The reader will always have 1 row, with one column.
                    // That column will have type long, and value 0 or 1.
                    reader.Read();
                    photoExists = (reader.GetInt32(0) != 0);
                    reader.Close();
                }

                if (!photoExists)
                {
                    // Add a row to the photo table if that row doesn't exist already.
                    command.CommandText = "INSERT Into Photos VALUES(" +
                        "@PhotoID" +
                        ",@CountComments" +
                        ",@CountFaves" +
                        ",@DateTaken" +
                        ",@DateUploaded" +
                        ",@Description" +
                        ",@Large1600Height" +
                        ",@Large1600Url" +
                        ",@Large1600Width" +
                        ",@Large2048Height" +
                        ",@Large2048Url" +
                        ",@Large2048Width" +
                        ",@LargeHeight" +
                        ",@LargeUrl" +
                        ",@LargeWidth" +
                        ",@LargeSquareThumbnailHeight" +
                        ",@LargeSquareThumbnailUrl" +
                        ",@LargeSquareThumbnailWidth" +
                        ",@Latitude" +
                        ",@License" +
                        ",@Longitude" +
                        ",@Medium640Height" +
                        ",@Medium640Url" +
                        ",@Medium640Width" +
                        ",@Medium800Height" +
                        ",@Medium800Url" +
                        ",@Medium800Width" +
                        ",@MediumHeight" +
                        ",@MediumUrl" +
                        ",@MediumWidth" +
                        ",@OriginalFormat" +
                        ",@OriginalHeight" +
                        ",@OriginalUrl" +
                        ",@OriginalWidth" +
                        ",@OwnerName" +
                        ",@PlaceId" +
                        ",@Rotation" +
                        ",@Small320Height" +
                        ",@Small320Url" +
                        ",@Small320Width" +
                        ",@SmallHeight" +
                        ",@SmallUrl" +
                        ",@SmallWidth" +
                        ",@SquareThumbnailHeight" +
                        ",@SquareThumbnailUrl" +
                        ",@SquareThumbnailWidth" +
                        ",@ThumbnailHeight" +
                        ",@ThumbnailUrl" +
                        ",@ThumbnailWidth" +
                        ",@Title" +
                        ",@UserId" +
                        ",@Url" +
                        ",@Views" +
                        ",@WoeId" +
                        ");"
                        ;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@PhotoID", flickrPhoto.PhotoId);
                    command.Parameters.AddWithValue("@CountComments", flickrPhoto.CountComments);
                    command.Parameters.AddWithValue("@CountFaves", flickrPhoto.CountFaves);
                    command.Parameters.AddWithValue("@DateTaken", flickrPhoto.DateTaken.ToString(@"yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@DateUploaded", flickrPhoto.DateUploaded.ToString(@"yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@Description", flickrPhoto.Description);
                    command.Parameters.AddWithValue("@Large1600Height", flickrPhoto.Large1600Height);
                    command.Parameters.AddWithValue("@Large1600Url", flickrPhoto.Large1600Url);
                    command.Parameters.AddWithValue("@Large1600Width", flickrPhoto.Large1600Width);
                    command.Parameters.AddWithValue("@Large2048Height", flickrPhoto.Large2048Height);
                    command.Parameters.AddWithValue("@Large2048Url", flickrPhoto.Large2048Url);
                    command.Parameters.AddWithValue("@Large2048Width", flickrPhoto.Large2048Width);
                    command.Parameters.AddWithValue("@LargeHeight", flickrPhoto.LargeHeight);
                    command.Parameters.AddWithValue("@LargeUrl", flickrPhoto.LargeUrl);
                    command.Parameters.AddWithValue("@LargeWidth", flickrPhoto.LargeWidth);
                    command.Parameters.AddWithValue("@LargeSquareThumbnailHeight", flickrPhoto.LargeSquareThumbnailHeight);
                    command.Parameters.AddWithValue("@LargeSquareThumbnailUrl", flickrPhoto.LargeSquareThumbnailUrl);
                    command.Parameters.AddWithValue("@LargeSquareThumbnailWidth", flickrPhoto.LargeSquareThumbnailWidth);
                    command.Parameters.AddWithValue("@Latitude", flickrPhoto.Latitude);
                    command.Parameters.AddWithValue("@License", flickrPhoto.License);
                    command.Parameters.AddWithValue("@Longitude", flickrPhoto.Longitude);
                    command.Parameters.AddWithValue("@Medium640Height", flickrPhoto.Medium640Height);
                    command.Parameters.AddWithValue("@Medium640Url", flickrPhoto.Medium640Url);
                    command.Parameters.AddWithValue("@Medium640Width", flickrPhoto.Medium640Width);
                    command.Parameters.AddWithValue("@Medium800Height", flickrPhoto.Medium800Height);
                    command.Parameters.AddWithValue("@Medium800Url", flickrPhoto.Medium800Url);
                    command.Parameters.AddWithValue("@Medium800Width", flickrPhoto.Medium800Width);
                    command.Parameters.AddWithValue("@MediumHeight", flickrPhoto.MediumHeight);
                    command.Parameters.AddWithValue("@MediumUrl", flickrPhoto.MediumUrl);
                    command.Parameters.AddWithValue("@MediumWidth", flickrPhoto.MediumWidth);
                    command.Parameters.AddWithValue("@OriginalFormat", flickrPhoto.OriginalFormat);
                    command.Parameters.AddWithValue("@OriginalHeight", flickrPhoto.OriginalHeight);
                    command.Parameters.AddWithValue("@OriginalUrl", flickrPhoto.OriginalUrl);
                    command.Parameters.AddWithValue("@OriginalWidth", flickrPhoto.OriginalWidth);
                    command.Parameters.AddWithValue("@OwnerName", flickrPhoto.OwnerName);
                    command.Parameters.AddWithValue("@PlaceId", flickrPhoto.PlaceId);
                    command.Parameters.AddWithValue("@Rotation", flickrPhoto.Rotation);
                    command.Parameters.AddWithValue("@Small320Height", flickrPhoto.Small320Height);
                    command.Parameters.AddWithValue("@Small320Url", flickrPhoto.Small320Url);
                    command.Parameters.AddWithValue("@Small320Width", flickrPhoto.Small320Width);
                    command.Parameters.AddWithValue("@SmallHeight", flickrPhoto.SmallHeight);
                    command.Parameters.AddWithValue("@SmallUrl", flickrPhoto.SmallUrl);
                    command.Parameters.AddWithValue("@SmallWidth", flickrPhoto.SmallWidth);
                    command.Parameters.AddWithValue("@SquareThumbnailHeight", flickrPhoto.SquareThumbnailHeight);
                    command.Parameters.AddWithValue("@SquareThumbnailUrl", flickrPhoto.SquareThumbnailUrl);
                    command.Parameters.AddWithValue("@SquareThumbnailWidth", flickrPhoto.SquareThumbnailWidth);
                    command.Parameters.AddWithValue("@ThumbnailHeight", flickrPhoto.ThumbnailHeight);
                    command.Parameters.AddWithValue("@ThumbnailUrl", flickrPhoto.ThumbnailUrl);
                    command.Parameters.AddWithValue("@ThumbnailWidth", flickrPhoto.ThumbnailWidth);
                    command.Parameters.AddWithValue("@Title", flickrPhoto.Title);
                    command.Parameters.AddWithValue("@UserId", flickrPhoto.UserId);
                    command.Parameters.AddWithValue("@Url", flickrPhoto.WebUrl);
                    command.Parameters.AddWithValue("@Views", flickrPhoto.Views);
                    command.Parameters.AddWithValue("@WoeId", flickrPhoto.WoeId);
                    command.ExecuteNonQuery();

#if false
                    if (tags != null)
                    {
                        foreach (PhotoInfoTag tag in tags)
                        {
                            AddTagToDB(connection, flickrPhoto.PhotoId, tag.TagText);
                        }

                    }
                    else
#endif
                    {
                        foreach (string tag in flickrPhoto.Tags)
                        {
                            AddTagToDB(connection, flickrPhoto.PhotoId, tag);
                        }
                    }

                    SearchPhotoCount++;
                }
            }

            // Add this album to the Albums table, and this album/photo to the PhotoAlbums table.
            AddPhotoAlbumsToDB(connection, flickrPhoto, photoset, contexts);
        }

        private void AddTagToDB(SQLiteConnection connection, string PhotoId, string tag)
        {
            string trimmedTag = tag.Trim();
            if (String.IsNullOrWhiteSpace(trimmedTag))
            {
                trimmedTag = "invalidtag";
            }
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO PhotoTags VALUES(@PhotoID,@Tag);";
                command.Parameters.Add("@PhotoID", DbType.String);
                command.Parameters.Add("@Tag", DbType.String);
                command.Parameters[0].Value = PhotoId;
                command.Parameters[1].Value = trimmedTag;
                command.ExecuteNonQuery();
            }
        }

        private void AddPhotoAlbumsToDB(SQLiteConnection connection, FlickrNet.Photo flickrPhoto,
                        Photoset photoset, FlickrNet.AllContexts contexts)
        {
            if (contexts == null)
            {
                if (photoset != null)
                {
                    AddAlbumToDB(connection, photoset.PhotosetId);
                    AddPhotoToPhotoAlbums(connection, flickrPhoto.PhotoId, photoset.PhotosetId);
                }
            }
            else
            {
                System.Collections.ObjectModel.Collection<ContextSet> sets = contexts.Sets;
                foreach (ContextSet set in sets)
                {
                    AddAlbumToDB(connection, set.PhotosetId);
                    AddPhotoToPhotoAlbums(connection, flickrPhoto.PhotoId, set.PhotosetId);
                }
            }
        }

        // If the photoset is not present in the Albums table, add it.
        private void AddAlbumToDB(SQLiteConnection connection, string photosetId)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                // Look to see if it exists.
                command.CommandText = "SELECT EXISTS (SELECT 1 FROM Albums WHERE [AlbumID]=@AlbumID LIMIT 1)";
                command.Parameters.AddWithValue("@AlbumID", photosetId);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    // The reader will always have 1 row, with one column.
                    // That column will have type long, and value 0 or 1.
                    reader.Read();
                    int exists = reader.GetInt32(0);
                    reader.Close();

                    if (exists != 0)
                    {
                        // Row for this album already exists, do nothing.
                        return;
                    }
                }

                // Get information about this album.
                FlickrNet.Flickr f = FlickrManager.GetFlickrAuthInstance();
                if (f == null)
                {
                    BGErrorMessage = "You must authenticate before you can download data from Flickr.";
                    return;
                }

                try
                {
                    FlickrNet.Photoset photoset = f.PhotosetsGetInfo(photosetId);
                    command.CommandText = "INSERT Into Albums VALUES(" +
                    "@AlbumID" +
                    ",@CountComments" +
                    ",@CountPhotos" +
                    ",@CountVideos" +
                    ",@DateCreated" +
                    ",@DateUpdated" +
                    ",@Description" +
                    ",@OwnerName" +
                    ",@SmallUrl" +
                    ",@SquareThumbnailUrl" +
                    ",@ThumbnailUrl" +
                    ",@Title" +
                    ",@Url" +
                    ",@Views" +
                    ");"
                    ;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@AlbumID", photoset.PhotosetId);
                    command.Parameters.AddWithValue("@CountComments", photoset.CommentCount);
                    command.Parameters.AddWithValue("@CountPhotos", photoset.NumberOfPhotos);
                    command.Parameters.AddWithValue("@CountVideos", photoset.NumberOfVideos);
                    command.Parameters.AddWithValue("@DateCreated", photoset.DateCreated.ToString(@"yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@DateUpdated", photoset.DateUpdated.ToString(@"yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@Description", photoset.Description);
                    command.Parameters.AddWithValue("@OwnerName", photoset.OwnerName);
                    command.Parameters.AddWithValue("@SmallUrl", photoset.PhotosetSmallUrl);
                    command.Parameters.AddWithValue("@SquareThumbnailUrl", photoset.PhotosetSquareThumbnailUrl);
                    command.Parameters.AddWithValue("@ThumbnailUrl", photoset.PhotosetThumbnailUrl);
                    command.Parameters.AddWithValue("@Title", photoset.Title);
                    command.Parameters.AddWithValue("@Url", photoset.Url);
                    command.Parameters.AddWithValue("@Views", photoset.ViewCount);
                    command.ExecuteNonQuery();

                }
                catch (FlickrNet.FlickrException ex)
                {
                    BGErrorMessage = "Album info request failed. Error: " + ex.Message;
                    return;
                }
                catch (Exception ex)
                {
                    BGErrorMessage = "Album info request failed. Unexpected error: " + ex.Message;
                    return;
                }
            }
        }

        private void AddPhotoToPhotoAlbums(SQLiteConnection connection, string photoId, string photosetId)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                // Add this photo to the PhotoAlbums table
                command.CommandText = "INSERT INTO PhotoAlbums VALUES(@PhotoID,@AlbumID);";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@PhotoID", photoId);
                command.Parameters.AddWithValue("@AlbumID", photosetId);
                command.ExecuteNonQuery();
            }
        }

            private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(BGErrorMessage))
            {
                MessageBox.Show(BGErrorMessage);
            }
        }

        private void btnRemoveLoginAccount_Click(object sender, EventArgs e)
        {
            var user = (User)cbLoginAccount.SelectedItem;
            if (user == null)
            {
                MessageBox.Show("No user selected.");
            }
            else if (user.UserName == "Public")
            {
                MessageBox.Show("Cannot remove access to public photos.");
            }
            else
            {
                DialogResult result = MessageBox.Show("Remove the login account '" + user.CombinedName + "'?",
                    "FlickrMetadataDL", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    Settings.RemoveFlickrLoginAccountName(user);
                }
            }
        }

        private void btnAddLoginAccount_Click(object sender, EventArgs e)
        {
            FormAddLoginAccount dlg = new FormAddLoginAccount(Settings, FlickrManager);
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                User NewUser = dlg.NewUser;
                if (NewUser != null)
                {
                    try
                    {
                        cbLoginAccount.SelectedItem = NewUser;
                        Settings.FlickrLoginAccountName = NewUser.UserName;
                        cbSearchAccount.SelectedItem = NewUser;
                        Settings.FlickrSearchAccountName = NewUser.UserName;
                    }
                    catch (Exception)
                    {
                        // Ignore error.
                    }
                }
            }
        }

        private void cbLoginAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormIsLoaded)
            {
                User user = (User)cbLoginAccount.SelectedItem;
                if (user != null)
                {
                    Settings.FlickrLoginAccountName = user.UserName;
                }
            }
        }

        private void btnAddSearchAccount_Click(object sender, EventArgs e)
        {
            FormAddSearchAccount dlg = new FormAddSearchAccount(Settings, FlickrManager);
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                User NewUser = dlg.NewUser;
                if (NewUser != null)
                {
                    try
                    {
                        cbSearchAccount.SelectedItem = NewUser;
                        Settings.FlickrSearchAccountName = NewUser.UserName;
                    }
                    catch (Exception)
                    {
                        // Ignore error.
                    }
                }
            }
        }

        private void btnRemoveSearchAccount_Click(object sender, EventArgs e)
        {
            User user = (User)cbSearchAccount.SelectedItem;
            if (user == null)
            {
                MessageBox.Show("No user selected.");
            }
            else
            {
                DialogResult result = MessageBox.Show("Remove the search account '" + user.CombinedName + "'?",
                "FlickrMetadataDL", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    Settings.RemoveFlickrSearchAccountName(user);
                }
            }
        }

        private void cbSearchAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormIsLoaded)
            {
                User user = (User)cbSearchAccount.SelectedItem;
                if (user != null)
                {
                    Settings.FlickrSearchAccountName = user.UserName;
                }
            }
        }

        private void chkSearchAllPhotos_CheckedChanged(object sender, EventArgs e)
        {
            if (FormIsLoaded)
            {
                if (chkSearchAllPhotos.Checked)
                {
                    dgvPhotosets.Rows.Clear();
                    cbHeader.Checked = false;
                }
            }
        }

        private void btnBrowseOutputFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = "sqlite";
            dlg.Filter = "SQLite database files (*.sqlite)|*.sqlite|All files (*.*)|*.*";

            DialogResult result = dlg.ShowDialog();

            // If user did not cancel, save the results
            if (result == DialogResult.OK) // Test result.
            {
                Settings.OutputFilename = dlg.FileName;
            }
        }

    }
}
