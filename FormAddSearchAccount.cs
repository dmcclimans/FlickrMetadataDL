using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlickrMetadataDL
{
    public partial class FormAddSearchAccount : Form
    {
        private Settings Settings { get; set; }
        private FlickrManager FlickrManager { get; set; }

        // If a new user is added, this is set to the new user.
        public User NewUser = null;

        private void FormAddSearchAccount_Load(object sender, EventArgs e)
        {
            if (Settings.FormAddSearchLocation.X != 0 ||
                  Settings.FormAddSearchLocation.Y != 0)
            {
                this.Location = Settings.FormAddSearchLocation;
            }
        }

        private void FormAddSearchAccount_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.FormAddSearchLocation = this.Location;
        }

        public FormAddSearchAccount(Settings settings, FlickrManager flickrManager)
        {
            Settings = settings;
            FlickrManager = flickrManager;
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Add is the accept button, because I want it to be clicked when the user hits enter.
            // That means the DialogResult is set to OK by the framework.
            // Set it to none, so that if errors occur, we don't exit this dialog.
            // If we successfully add, we will set it back to OK.
            this.DialogResult = DialogResult.None;

            string newUserName = txtUserName.Text;
            newUserName = newUserName.Trim();
            if (string.IsNullOrWhiteSpace(newUserName))
            {
                MessageBox.Show("Please enter a Flickr account name or email address.");
                return;
            }
            int index = Settings.FlickrSearchAccountList.ToList<User>().FindIndex(x => x.UserName == newUserName);
            if (index >= 0)
            {
                MessageBox.Show("That account is already in the search account list.");
                return;
            }

            FlickrNet.Flickr f = null;
            FlickrNet.FoundUser userInfo = null;
            try
            {
                f = FlickrManager.GetFlickrAuthInstance();
                if (f == null)
                {
                    MessageBox.Show("Could not connect with Flickr.");
                    return;
                }

                // Look for the user by name. 
                Cursor.Current = Cursors.WaitCursor;
                userInfo = f?.PeopleFindByUserName(newUserName);
            }
            catch (FlickrNet.FlickrException)
            {
                try
                {
                    userInfo = f?.PeopleFindByEmail(newUserName);
                }
                catch (FlickrNet.FlickrException)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Could not find user " + newUserName + ".");
                    return;
                }
                catch (Exception exc)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(exc.Message);
                    return;
                }
            }
            catch (Exception exc)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(exc.Message);
                return;
            }

            if (userInfo == null)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Could not find user " + newUserName + ".");
                return;
            }

            // Check if this user is already in the account list. We checked before, but this
            // could still happen if they searched by email.
            index = Settings.FlickrSearchAccountList.ToList<User>().FindIndex(x => x.UserName == userInfo.UserName);
            if (index >= 0)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("That account is already in the account list.");
                return;
            }

            // Create a new user.
            NewUser = new User(userInfo.UserName);
            NewUser.UserId = userInfo.UserId;

            // Get the users real name.
            FlickrNet.Person person = null;
            try
            {
                person = f.PeopleGetInfo(NewUser.UserId);
            }
            catch (Exception exc)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(exc.Message);
                return;
            }

            if (!string.IsNullOrWhiteSpace(person.RealName))
            {
                NewUser.RealName = person.RealName;
            }

            // Add the user to the search account list.
            Settings.AddReplaceFlickrSearchAccountName(NewUser);

            Cursor.Current = Cursors.Default;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
