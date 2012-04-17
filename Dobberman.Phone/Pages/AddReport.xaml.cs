#define VIRTUAL_WP7
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
using TAUP2C.Dobberman.Phone.DobbermanService;
using Facebook;
using Microsoft.Phone.Controls.Maps;
using TAUP2C.Dobberman.Phone.Google_Maps;
using System.Device.Location;

namespace TAUP2C.Dobberman.Phone.Pages
{
    public partial class AddReport : PhoneApplicationPage
    {
        public static string LastMoodCheck;
        private  List<Authority> AuthorityList = new List<Authority>();
        public AddReport()
        {
            InitializeComponent();


        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //this.ContentPanel.DataContext = States.CurReport;
            GetAuthorities();


        }


        public void GetAuthorities()
        {


            DobbermanServiceClient client = new DobbermanServiceClient();
            client.GetAllAuthoritiesCompleted += new EventHandler<GetAllAuthoritiesCompletedEventArgs>(client_GetAllAuthoritiesCompleted);
            client.GetAllAuthoritiesAsync();


        }

        void client_GetAllAuthoritiesCompleted(object sender, GetAllAuthoritiesCompletedEventArgs e)
        {
            List<string> AutList = new List<string>();
            foreach (Authority a in e.Result.Cast<Authority>())
            {
                AutList.Add(a.Name);
                AuthorityList.Add(a);
            }
            autoCompleteBox1.ItemsSource = AutList;
        }

        private void AutoCompleteSelected(object sender, RoutedEventArgs e)
        {
            if (autoCompleteBox1.Text == "Select Authority")
            {
                autoCompleteBox1.Text = "";
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (this.button2.Opacity == 1)
            {
                this.button2.Opacity = 0.5;
            }

            if (this.button3.Opacity == 1)
            {
                this.button3.Opacity = 0.5;
            }
            (sender as Button).Opacity = 1;
            LastMoodCheck = "positive";
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (this.button1.Opacity == 1)
            {
                this.button1.Opacity = 0.5;
            }

            if (this.button3.Opacity == 1)
            {
                this.button3.Opacity = 0.5;
            }
            (sender as Button).Opacity = 1;
            LastMoodCheck = "negative";
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (this.button1.Opacity == 1)
            {
                this.button1.Opacity = 0.5;
            }

            if (this.button2.Opacity == 1)
            {
                this.button2.Opacity = 0.5;
            }
            (sender as Button).Opacity = 1;
            LastMoodCheck = "concerned";
        }

        private void PostToWall(Report r)
        {
            var fb = new FacebookClient(States.accessToken);

            // make sure to add event handler for PostCompleted.
            fb.PostCompleted += (o, args) =>
            {
                // incase you support cancellation, make sure to check
                // e.Cancelled property first even before checking (e.Error!=null).
                if (args.Cancelled)
                {
                    // for this example, we can ignore as we don't allow this
                    // example to be cancelled.

                    // you can check e.Error for reasons behind the cancellation.
                    var cancellationError = args.Error;
                }
                else if (args.Error != null)
                {
                    // error occurred
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show(args.Error.Message);
                    });
                }
                else
                {
                    // the request was completed successfully

                    // now we can either cast it to IDictionary<string, object> or IList<object>
                    var result = (IDictionary<string, object>)args.GetResultData();
                    //_lastMessageId = (string)result["id"];

                    // make sure to be on the right thread when working with ui.
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Report posted to facebook succesfully");
                    });
                }
            };

            var parameters = new Dictionary<string, object>
               {
                    { "message", "I have just posted a " + r.Mood + " report about " + r.Authority.Name +"!" },
                    { "link", "http://download.codeplex.com/Project/Download/FileDownload.aspx?ProjectName=facebooksdk&DownloadId=170794&Build=17672" },
                    { "picture", "http://download.codeplex.com/Project/Download/FileDownload.aspx?ProjectName=facebooksdk&DownloadId=170794&Build=17672" },
                    { "name", "This is what I have to say:" },
                    { "caption", "Posted with Dobberman App" },
                    { "description", r.Description },            
                };
            fb.PostAsync(r.Authority.FacebookPage + "/feed", parameters);
            
        }

        private void PostReport_Click(object sender, RoutedEventArgs e)
        {
            Report NewReport = new Report();
            bool match = false;

            foreach (Authority a in AuthorityList)
            {
                if (autoCompleteBox1.Text == a.Name)
                {
                    match = true;
                    NewReport.Authority = a;
                    NewReport.AuthorityId = a.AuthorityId;
                }
            }

            if (match == false)
            {
                MessageBox.Show("Authority Not Recognized");
                goto reportEnd;

            }

            NewReport.Description = this.Desc.Text;
            NewReport.Location = "";
            if (LastMoodCheck == "")
            {
                MessageBox.Show("Please select report mood");
                goto reportEnd;
            }
            NewReport.Date = System.DateTime.Now;
            NewReport.Mood = LastMoodCheck;
            NewReport.UserId = States.userId;
            

            if (checkBox2.IsChecked == true)
            {
                PostToWall(NewReport);
               
            }

            PostToServer(NewReport);

        reportEnd:
            return;
       }

        private void PostToServer(Report r)
        {

            DobbermanServiceClient client = new DobbermanServiceClient();
            client.CreateNewReportCompleted += new EventHandler<CreateNewReportCompletedEventArgs>(client_CreateNewReportCompletedE);
            client.CreateNewReportAsync(r);


        }

        void client_CreateNewReportCompletedE(object sender, CreateNewReportCompletedEventArgs e)
        {
            MessageBox.Show("Report Sent Succesfulluy");
        }

       
             private void GPScheckbox_Checked(object sender, RoutedEventArgs e)
        {
            string coordinate = System.String.Empty;
#if REAL_WP7
            IGeoPositionWatcher<GeoCoordinate> watcher;
            watcher = new System.Device.Location.GeoCoordinateWatcher();

            // Do not suppress prompt, and wait 1000 milliseconds to start.
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
            GeoCoordinate coord = watcher.Position.Location;

            if (coord.IsUnknown != true)
            {
                coordinate = coord.ToString(); 
                watcher.Stop();
              
            }
                
            else
            {
                coordinate = null;
            }
        }
#endif
#if VIRTUAL_WP7
            GeoCoordinate coord = new GeoCoordinate();
            List<GeoCoordinate> myList = new List<GeoCoordinate>();
              
                myList.Add(new GeoCoordinate(31.101,35.145));
                myList.Add(new GeoCoordinate(31.838, 34.878));
                myList.Add(new GeoCoordinate(32.549, 35.234));
                coord = myList[RandomNumber(0, 2)];
                coordinate = coord.ToString();
#endif
 
}
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
 
        

        
       
       
  }
}
