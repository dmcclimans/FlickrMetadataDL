using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace FlickrMetadataDL
{
    /// <summary>
    /// Settings values.
    /// Implement INotifyPropertyChanged.
    /// Persist settings to xml file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class will persist the settings data to an XML file. It will attempt to
    /// save the settings in the file "FlickrMetadataDLSettings.xml"
    /// in the folder of the executable. However, if that folder is not writable by the
    /// program (which will usually be the case if the user has installed it to a folder in
    /// Program Files), then it will save the data to
    /// Documents\FlickrMetadataDL\FlickrMetadataDLSettings.xml.
    /// </para>
    /// <para>
    /// You should create only once instance of this class. Pass this instance
    /// to any class or method that needs to access the settings.
    /// Conceptually this class could be a static class without any instantiation. But the .net
    /// XMLSerializer will not serialize a static class, it will only work with instances.
    /// </para>
    /// </remarks>
    public class Settings : INotifyPropertyChanged, IChangeTracking
    {
        /// <summary>
        /// Event raised when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private const string SettingsFilename = "FlickrMetadataDLSettings.xml";
        private const string DefaultSettingsFolder = "FlickrMetadataDL";

        // Properties that are serialized, and trigger the PropertyChanged events.

        private bool filterByDateValue = false;
        public bool FilterByDate
        {
            get { return filterByDateValue; }
            set { SetProperty(ref filterByDateValue, value, true); }
        }

        private bool findAllAlbums = false;
        public bool FindAllAlbums
        {
            get { return findAllAlbums; }
            set { SetProperty(ref findAllAlbums, value, true); }
        }

        // It would be convenient to make this a Dictionary<string, User>. But
        // Dictionary does not automatically serialize to XML, while List does.
        // Also it would be convenient to have a default value for the list that is
        // new User("Public"). You can't do this because the XMLSerilalizer, when it
        // deserializes a <List>, does not *replace* the list but rather *adds* to the
        // list. So you end up with multiple "Public" users in the list. Instead
        // I handle this in the Load() method which adds the Public user when it
        // does not find a file to deserialize.
        private BindingList<User> flickrLoginAccountListValue = new BindingList<User>();
        public BindingList<User> FlickrLoginAccountList
        {
            get { return flickrLoginAccountListValue; }
            set { SetProperty(ref flickrLoginAccountListValue, value, true); }
        }

        private string flickrLoginAccountNameValue = "";
        public string FlickrLoginAccountName
        {
            get { return flickrLoginAccountNameValue; }
            set { SetProperty(ref flickrLoginAccountNameValue, value, true); }
        }

        private BindingList<User> flickrSearchAccountListValue = new BindingList<User>();
        public BindingList<User> FlickrSearchAccountList
        {
            get { return flickrSearchAccountListValue; }
            set { SetProperty(ref flickrSearchAccountListValue, value, true); }
        }

        private string flickrSearchAccountNameValue = "";
        public string FlickrSearchAccountName
        {
            get { return flickrSearchAccountNameValue; }
            set { SetProperty(ref flickrSearchAccountNameValue, value, true); }
        }

        private Point formAboutLocationValue;
        public Point FormAboutLocation
        {
            get { return formAboutLocationValue; }
            set { SetProperty(ref formAboutLocationValue, value, true); }
        }

        private Point formAddLoginAccountLocationValue;
        public Point FormAddLoginAccountLocation
        {
            get { return formAddLoginAccountLocationValue; }
            set { SetProperty(ref formAddLoginAccountLocationValue, value, true); }
        }

        private Point formAddSearchLocationValue;
        public Point FormAddSearchLocation
        {
            get { return formAddSearchLocationValue; }
            set { SetProperty(ref formAddSearchLocationValue, value, true); }
        }

        private Point formMainLocationValue;
        public Point FormMainLocation
        {
            get { return formMainLocationValue; }
            set { SetProperty(ref formMainLocationValue, value, true); }
        }

        private Size formMainSizeValue;
        public Size FormMainSize
        {
            get { return formMainSizeValue; }
            set { SetProperty(ref formMainSizeValue, value, true); }
        }

        private string outputFilenameValue = "";
        public string OutputFilename
        {
            get { return outputFilenameValue; }
            set { SetProperty(ref outputFilenameValue, value, true); }
        }

        private bool searchAllPhotosValue = false;
        public bool SearchAllPhotos
        {
            get { return searchAllPhotosValue; }
            set { SetProperty(ref searchAllPhotosValue, value, true); }
        }

        private DateTime startDateValue = DateTime.MinValue;
        public DateTime StartDate
        {
            get { return startDateValue; }
            set { SetProperty(ref startDateValue, value, true); }
        }

        private DateTime stopDateValue = DateTime.MinValue;
        public DateTime StopDate
        {
            get { return stopDateValue; }
            set { SetProperty(ref stopDateValue, value, true); }
        }

        // Properties which are not persisted, but trigger property changed.
        private bool filterDateEnabledValue = true;
        [XmlIgnore]
        public bool FilterDateEnabled
        {
            get { return filterDateEnabledValue; }
            set { SetProperty(ref filterDateEnabledValue, value, true); }
        }

        private bool getAlbumsEnabledValue = true;
        [XmlIgnore]
        public bool GetAlbumsEnabled
        {
            get { return getAlbumsEnabledValue; }
            set { SetProperty(ref getAlbumsEnabledValue, value, true);  }
        }

        // Class static properties.
        // These properties are not persisted nor trigger property changed.
        private static string _ExeFolder = "";
        [XmlIgnore]
        public static string ExeFolder
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_ExeFolder))
                {
                    _ExeFolder = Path.GetDirectoryName(Application.ExecutablePath);
                }
                return _ExeFolder;
            }
        }

        private static string _SettingsFolder = "";
        [XmlIgnore]
        public static string SettingsFolder
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_SettingsFolder))
                {
                    _SettingsFolder = Path.GetDirectoryName(Application.ExecutablePath);
                    if (!IsFolderWritable(_SettingsFolder))
                    {
                        _SettingsFolder =
                            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        _SettingsFolder = Path.Combine(_SettingsFolder, DefaultSettingsFolder);
                    }

                }
                return _SettingsFolder;
            }
        }

        // Methods
        public static Settings Load()
        {
            string path = SettingsFolder;
            if (path.Length > 0 && Directory.Exists(path))
            {
                path = Path.Combine(path, SettingsFilename);
                if (path.Length > 0 && File.Exists(path))
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                        TextReader reader = new StreamReader(path);

                        Settings mySettings = (Settings)serializer.Deserialize(reader);
                        reader.Close();

                        // Success
                        return mySettings;
                    }
                    catch (Exception e1)
                    {
                        // We failed to deserialize a Settings object
                        string msg = "Error loading settings from " + path;
                        msg += " -- " + e1.Message;
                        MessageBox.Show(msg);
                    }
                }
            }

            // Return a default settings object, with only public login access.
            Settings defaultSettings = new Settings();
            defaultSettings.FlickrLoginAccountList.Add(new User("Public"));

            // Set the Filter start and stop dates to 1 month ago through today
            DateTime today = DateTime.Now.Date;
            defaultSettings.StartDate = today.AddMonths(-1);
            defaultSettings.StopDate = today;

            return defaultSettings;
         }

        public void SaveIfChanged()
        {
            if (IsChanged)
            {
                if (Save())
                {
                    AcceptChanges();
                }
            }
        }

        public bool Save()
        {
            try
            {
                string path = SettingsFolder;
                // Ensure destination folder exists
                System.IO.Directory.CreateDirectory(path);
                path = Path.Combine(path, SettingsFilename);
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                TextWriter writer = new StreamWriter(path);
                serializer.Serialize(writer, this);
                writer.Close();
                return true;
            }
            catch (Exception e)
            {
                string msg = "Error saving settings - " + e.ToString();
                MessageBox.Show(msg);
                return false;
            }
        }

        public static bool IsFolderWritable(string folderpath)
        {
            try
            {
                string filename = Guid.NewGuid().ToString() + ".txt";
                System.IO.File.Create(folderpath + filename).Close();
                System.IO.File.Delete(folderpath + filename);
            }
            catch (System.UnauthorizedAccessException)
            {
                return false;
            }

            return true;
        }

        public void UpdateEnabledProperties()
        {
            FilterDateEnabled = FilterByDate;
            GetAlbumsEnabled = !SearchAllPhotos &&
                               FlickrSearchAccountList.Count > 0 &&
                               !String.IsNullOrWhiteSpace(FlickrSearchAccountName);
        }

        // Implementation of IChangeTracking
        // This property is not persisted and does not trigger property changed.
        /// <summary>
        /// Gets the object's changed status
        /// </summary>
        /// <value>True if the object's content has changed since the last call to AcceptChanges().</value>
        [XmlIgnore]
        public bool IsChanged { get; private set; }

        /// <summary>
        /// Resets the object's state to unchanged by accepting the modifications.
        /// </summary>
        public void AcceptChanges()
        {
            IsChanged = false;
        }

        /// <summary>
        /// Helper method to implement a property setter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldValue">The backing field for the property</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="markChanged">If true, and the property has changed, will mark the object as changed</param>
        /// <returns>True if the property changed</returns>
        /// <param name="propertyName">The name of the property</param>
        /// <remarks>
        /// If the property value has changed raises the PropertyChanged event.
        /// Marks the property as changed only if <paramref name="markChanged"/> is true and the property value has changed.
        /// The <paramref name="markChanged"/> parameter is normally the inverse of the XmlIgore attribute.
        /// </remarks>
        protected bool SetProperty<T>(ref T oldValue, T newValue, bool markChanged, [CallerMemberName] string propertyName = "")
        {
            //if (oldValue != null && oldValue.Equals(newValue))
            if (oldValue != null && EqualityComparer<T>.Default.Equals(oldValue, newValue))
                return false;
            oldValue = newValue;
            OnPropertyChanged(propertyName);
            if (markChanged)
                IsChanged = true;
            UpdateEnabledProperties();
            return true;
        }

        // Add a new user or replace an existing user to the FlickrLoginAccountName list.
        // By using this method, we trigger the property changed event and set IsChanged.
        // If the user replaces an element in the list directly, these don't occur.
        public void AddReplaceFlickrLoginAccountName(User newUser)
        {
            // Find where to insert in the list.
            // It's a small list, don't bother with binary search
            int index = 0;
            while (index < FlickrLoginAccountList.Count)
            {
                int compare = String.Compare(FlickrLoginAccountList[index].UserName, newUser.UserName, StringComparison.OrdinalIgnoreCase);
                if (compare == 0)
                {
                    // Found matching name, replace it
                    FlickrLoginAccountList[index] = newUser;
                    break;
                }
                else if (compare > 0)
                {
                    // Found name beyond the new user name, so insert before here.
                    FlickrLoginAccountList.Insert(index, newUser);
                    break;
                }
                index++;
            }
            if (index >= FlickrLoginAccountList.Count)
            {
                // NewUser is beyond end of list. Add to the end.
                FlickrLoginAccountList.Add(newUser);
            }

            OnPropertyChanged("FlickrLoginAccountList");
            IsChanged = true;
            FlickrLoginAccountList.ResetBindings();
            FlickrLoginAccountName = newUser.UserName;
            UpdateEnabledProperties();
        }

        // Remove a user in the FlickrLoginAccountName list.
        // By using this method, we trigger the property changed event and set IsChanged.
        public bool RemoveFlickrLoginAccountName(User user)
        {
            int index = FlickrLoginAccountList.ToList<User>().FindIndex(x => x.UserName == user.UserName);
            if (index < 0)
            {
                MessageBox.Show("Unexpected error. The user '" + user.UserName + "' is not found.");
                return false;
            }
            else
            {
                FlickrLoginAccountList.RemoveAt(index);
                OnPropertyChanged("FlickrLoginAccountList");
                IsChanged = true;
                FlickrLoginAccountList.ResetBindings();
                if (FlickrLoginAccountList.Count == 0)
                {
                    FlickrLoginAccountName = "";
                }
                else if (index < FlickrLoginAccountList.Count)
                {
                    FlickrLoginAccountName = FlickrLoginAccountList[index].UserName;
                }
                else
                {
                    FlickrLoginAccountName = FlickrLoginAccountList[index - 1].UserName;
                }
                UpdateEnabledProperties();
                return true;
            }
        }

        // Add a new user or replace an existing user to the FlickrLoginAccountName list.
        // Keep the list in sorted order by name.
        public void AddReplaceFlickrSearchAccountName(User newUser)
        {
            // Find where to insert in the list.
            // It's a small list, don't bother with binary search
            int index = 0;
            while (index < FlickrSearchAccountList.Count)
            {
                int compare = String.Compare(FlickrSearchAccountList[index].UserName, newUser.UserName, StringComparison.OrdinalIgnoreCase);
                if (compare == 0)
                {
                    // Found matching name, replace it
                    FlickrSearchAccountList[index] = newUser;
                    break;
                }
                else if (compare > 0)
                {
                    // Found name beyond the new user name, so insert before here.
                    FlickrSearchAccountList.Insert(index, newUser);
                    break;
                }
                index++;
            }
            if (index >= FlickrSearchAccountList.Count)
            {
                // NewUser is beyond end of list. Add to the end.
                FlickrSearchAccountList.Add(newUser);
            }

            OnPropertyChanged("FlickrSearchAccountList");
            IsChanged = true;
            FlickrSearchAccountList.ResetBindings();
            FlickrSearchAccountName = newUser.UserName;
            UpdateEnabledProperties();
        }

        // Remove a user in the FlickrLoginAccountName list.
        // By using this method, we trigger the property changed event and set IsChanged.
        public bool RemoveFlickrSearchAccountName(User user)
        {
            int index = FlickrSearchAccountList.ToList<User>().FindIndex(x => x.UserName == user.UserName);
            if (index < 0)
            {
                MessageBox.Show("Unexpected error. The user '" + user.UserName + "' is not found.");
                return false;
            }
            else
            {
                FlickrSearchAccountList.RemoveAt(index);
                OnPropertyChanged("FlickrSearchAccountList");
                IsChanged = true;
                FlickrSearchAccountList.ResetBindings();
                if (FlickrSearchAccountList.Count == 0)
                {
                    FlickrSearchAccountName = "";
                }
                else if (index < FlickrSearchAccountList.Count)
                {
                    FlickrSearchAccountName = FlickrSearchAccountList[index].UserName;
                }
                else
                {
                    FlickrSearchAccountName = FlickrSearchAccountList[index-1].UserName;
                }
                UpdateEnabledProperties();
    	        return true;
            }
        }

        /// <summary>
        /// Raise the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property that was changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
