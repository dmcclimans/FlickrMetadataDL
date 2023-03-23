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
    /// You should create only once instance of this class. Pass this instance
    /// to any class or method that needs to access the settings.
    /// </para>
    /// </remarks>
    public class Settings : SimpleSettings.SettingsBase
    {
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
        // I handle this in the SetDefaults() method which adds the Public user when it
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
        private bool filterDateEnabledValue = false;
        [XmlIgnore]
        public bool FilterDateEnabled
        {
            get { return filterDateEnabledValue; }
            set { SetProperty(ref filterDateEnabledValue, value, true); }
        }

        private bool getAlbumsButtonEnabledValue = false;
        [XmlIgnore]
        public bool GetAlbumsButtonEnabled
        {
            get { return getAlbumsButtonEnabledValue; }
            set { SetProperty(ref getAlbumsButtonEnabledValue, value, true);  }
        }

        private bool searchButtonEnabledValue = false;
        [XmlIgnore]
        public bool SearchButtonEnabled
        {
            get { return searchButtonEnabledValue; }
            set { SetProperty(ref searchButtonEnabledValue, value, true); }
        }

        // Methods
        // Load, Save, and SaveIfChanged methods must be defined.
        public static Settings Load()
        {
            return SimpleSettings.SettingsBase.Load<Settings>();
        }
        public bool Save()
        {
            return base.Save(typeof(Settings));
        }
        public void SaveIfChanged()
        {
            base.SaveIfChanged(typeof(Settings));
        }
        protected override void SetDefaults()
        {
            // Add a public option to login accounts.
            FlickrLoginAccountList.Add(new User("Public"));

            // Set the Filter start and stop dates to 1 month ago through today
            DateTime today = DateTime.Now.Date;
            StartDate = today.AddMonths(-1);
            StopDate = today;
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

            OnPropertyChanged(nameof(FlickrLoginAccountList));
            IsChanged = true;
            FlickrLoginAccountList.ResetBindings();
            FlickrLoginAccountName = newUser.UserName;
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
                OnPropertyChanged(nameof(FlickrLoginAccountList));
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

            OnPropertyChanged(nameof(FlickrSearchAccountList));
            IsChanged = true;
            FlickrSearchAccountList.ResetBindings();
            FlickrSearchAccountName = newUser.UserName;
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
                OnPropertyChanged(nameof(FlickrSearchAccountList));
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
    	        return true;
            }
        }

        /// <summary>
        /// Raise the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property that was changed.</param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(FilterByDate))
            {
                FilterDateEnabled = FilterByDate;
            }
            if (propertyName == nameof(SearchAllPhotos) ||
                propertyName == nameof(FlickrSearchAccountList) ||
                propertyName == nameof(FlickrSearchAccountName))
            {
                GetAlbumsButtonEnabled = !SearchAllPhotos &&
                                   FlickrSearchAccountList.Count > 0 &&
                                   !String.IsNullOrWhiteSpace(FlickrSearchAccountName);
            }
            if (propertyName == nameof(FlickrSearchAccountList) ||
                propertyName == nameof(FlickrSearchAccountName))
            {
                SearchButtonEnabled = (FlickrSearchAccountList.Count > 0 &&
                                      !String.IsNullOrWhiteSpace(FlickrSearchAccountName));

            }
        }
    }
}
