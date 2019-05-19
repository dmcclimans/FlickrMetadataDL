using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FlickrMetadataDL
{
    public class User : IComparable<User>
    {
        public User()
        {
        }
        public User(string name)
        {
            UserName = name;
        }

        public string UserName { get; set; } = "";

        public string RealName { get; set; } = "";

        [XmlIgnore]
        public string CombinedName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(UserName))
                {
                    return "";
                }
                else if (String.IsNullOrWhiteSpace(RealName))
                {
                    return UserName;
                }
                else
                {
                    return UserName + " (" + RealName + ")";
                }
            }
        }

        public string OAuthToken { get; set; }

        public string OAuthTokenSecret { get; set; }

        public string UserId { get; set; }

        public int CompareTo(User that)
        {
            return this.UserName.CompareTo(that.UserName);
        }

    }
}
