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
using TAUP2C.Dobberman.Phone.DobbermanService;
using TAUP2C.Dobberman.Phone.Helpers;


namespace TAUP2C.Dobberman.Phone.Pages
{
    public partial class LoginPage : PhoneApplicationPage
   {
        private string _accessToken;
        private IDictionary<string, object> _me;
        private WebBrowser _webBrowser;
        private bool isLoading = false;
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }

            set
            {
                if (this.isLoading != value)
                {
                    this.isLoading = value;
                    //this.NotifyPropertyChanged("IsUploading");
                }
            }
        }
        public LoginPage()
        {
            InitializeComponent();
            //Initialize _webBrowser
            _webBrowser = new WebBrowser();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {            
            //List of all the permissions you want to have access to
            //You can get the list of possiblities from here http://developers.facebook.com/tools/explorer/
            string[] extendedPermissions = new[] { "email", "publish_stream", "offline_access", "user_groups" };
            
            var oauth = new FacebookOAuthClient { AppId = FacebookHelpers.appId };
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
                    States.accessToken = result.AccessToken;
                    //AccessToken is used when you want to use API as a user
                    //This example is not using it at all just showing it in a messagebox
                    //NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.RelativeOrAbsolute));
                    //Hide the browser controller
                    _webBrowser.Visibility = System.Windows.Visibility.Collapsed;
                    Register_User();
                    

                }
                else
                {
                    var errorDescription = result.ErrorDescription;
                    var errorReason = result.ErrorReason;
                    MessageBox.Show(errorReason + " " + errorDescription);
                }
            }
        }

        // authenticates and registers the user at the dobberman service
        void Register_User()
        {
            this.loading.Visibility=Visibility.Visible;
            var fb = new FacebookClient(_accessToken);

            fb.GetCompleted += (o, args) =>
            {
                if (args.Error == null)
                {
                    _me = (IDictionary<string, object>)args.GetResultData();

                    Dispatcher.BeginInvoke(
                        () =>
                        {
                            //Welcome.Text = "Welcome " + _me["first_name"] + "!";
                            
                            DobbermanServiceClient client = new DobbermanServiceClient();
                            User user = new User()
                            {
                                Name = (string)_me["name"],
                                Email = (string)_me["email"],
                            };
                            client.CreateNewUserCompleted += new EventHandler<CreateNewUserCompletedEventArgs>(client_CreateNewUserCompleted);
                            client.CreateNewUserAsync(user);

                        });
                }
                else
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(args.Error.Message));
                }
            };

            // do a GetAsync me in order to get basic details of the user.
            fb.GetAsync("me");
            
        }

        private void client_CreateNewUserCompleted(object sender, CreateNewUserCompletedEventArgs e)
        {
            // this is the user id from the dobberman db
            States.userId = e.Result;
            PhoneHelpers.SetIsolatedStorageSetting("UserIsRegistered", true);
            //this.loading.Visibility = Visibility.Collapsed;
            this.NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    if (PhoneHelpers.GetIsolatedStorageSetting<bool>("UserIsRegistered") && !PhoneHelpers.GetApplicationState<bool>("UserBackPress"))
        //    {
        //        this.NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));                
        //    }
        //    else
        //    {
        //        //this.StartTransition();

        //        PhoneHelpers.SetApplicationState("UserBackPress", false);

        //        //this.SignInControl.GetSecurityToken();
        //    }
        //}
    } 
}