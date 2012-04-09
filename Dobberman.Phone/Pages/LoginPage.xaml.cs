using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Facebook;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;
using System.Text;


namespace TAUP2C.Dobberman.Phone.Pages
{
    public partial class LoginPage : PhoneApplicationPage
   {
        private string _accessToken;
        private WebBrowser _webBrowser;
        public LoginPage()
        {
            InitializeComponent();
            //Initialize _webBrowser
            _webBrowser = new WebBrowser();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Get this from the facebook
            string appId = "221692321249175";
            //List of all the permissions you want to have access to
            //You can get the list of possiblities from here http://developers.facebook.com/tools/explorer/
            string[] extendedPermissions = new[] { "publish_stream", "offline_access", "user_groups" };
            
            var oauth = new FacebookOAuthClient { AppId = appId };
            //Telling the Facebook that we want token as response
            //and we are using touch enabled device
            var parameters = new Dictionary<string, object>
                    {
                        { "response_type", "token" },
                        { "display", "touch" }
                    };
            //If there's extended permissions build the string and set it up
            if (extendedPermissions != null && extendedPermissions.Length > 0)
            {
                var scope = new StringBuilder();
                scope.Append(string.Join(",", extendedPermissions));
                parameters["scope"] = scope.ToString();
            }
            //Create the login url
            var loginUrl = oauth.GetLoginUrl(parameters);
            //Add webBrowser to the contentPanel
            ContentPanel.Children.Add(_webBrowser);
            _webBrowser.Navigated += webBrowser_Navigated;
            //Open the facebook login page into the browser
            _webBrowser.Navigate(loginUrl);
        }

        void webBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            FacebookOAuthResult result;
            //Try because there might be cases when user input wrong password
            if (FacebookOAuthResult.TryParse(e.Uri.AbsoluteUri, out result))
            {
                if (result.IsSuccess)
                {
                    _accessToken = result.AccessToken;
                    //AccessToken is used when you want to use API as a user
                    //This example is not using it at all just showing it in a messagebox
                    NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.RelativeOrAbsolute));
                    //Hide the browser controller
                    _webBrowser.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    var errorDescription = result.ErrorDescription;
                    var errorReason = result.ErrorReason;
                    MessageBox.Show(errorReason + " " + errorDescription);
                }
            }
        }
    } 

   
}