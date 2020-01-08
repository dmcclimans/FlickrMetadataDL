using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using FlickrNet;

namespace FlickrMetadataDL
{
    public class FlickrManager
    {
        private Settings Settings { get; set; }

        public FlickrManager(Settings settings)
        {
            Settings = settings;
        }

        public Flickr GetFlickrInstance()
        {
            return new Flickr(FlickrKey.Key, FlickrKey.Secret);
        }

        // Returns an authenticated instance based on the login user.
        public Flickr GetFlickrAuthInstance()
        {
            string name = Settings.FlickrLoginAccountName;
            int index = Settings.FlickrLoginAccountList.ToList<User>().FindIndex(
                x => x.UserName == Settings.FlickrLoginAccountName);
            if (index < 0)
            {
                return GetFlickrAuthInstance(null);
            }
            else
            {
                return GetFlickrAuthInstance(Settings.FlickrLoginAccountList[index]);
            }
        }

        // Returns an authenticated instance based on user. If the user is not
        // authenticated (Public), returns a non-authenticated instance.
        public Flickr GetFlickrAuthInstance(User user)
        {
            Flickr f = GetFlickrInstance();
            f.OAuthAccessToken = user?.OAuthToken;
            f.OAuthAccessTokenSecret = user?.OAuthTokenSecret;
            return f;
        }
    }
}
