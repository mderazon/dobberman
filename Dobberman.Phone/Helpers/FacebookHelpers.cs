using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TAUP2C.Dobberman.Phone.Helpers
{
    public class FacebookHelpers
    {
        public static readonly string appId = "221692321249175";
        public static Uri GetLogoutUri()
        {
            var sessionkey = ExtractSessionKeyFromAccessToken(States.accessToken);
            var url = String.Format("http://facebook.com/logout.php?app_key={0}&session_key={1}&next={2}",appId , sessionkey, "http://www.facebook.com");
            return new Uri(url);
        }

        private static string ExtractSessionKeyFromAccessToken(string accessToken)
        {
            if (!String.IsNullOrEmpty(accessToken))
            {
                var parts = accessToken.Split('|');
                if (parts.Length > 2)
                {
                    return parts[1];
                }
            }
            return String.Empty;
        }

    }
}
