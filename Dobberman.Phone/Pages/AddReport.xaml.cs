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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Devices;
using Microsoft.Phone;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using TAUP2C.Dobberman.Phone.DobbermanService;
using Facebook;
using Microsoft.Phone.Controls.Maps;
using TAUP2C.Dobberman.Phone.Google_Maps;
using System.Device.Location;
using Microsoft.Samples.WindowsPhoneCloud.StorageClient;
using Microsoft.Samples.WindowsPhoneCloud.StorageClient.Credentials;

namespace TAUP2C.Dobberman.Phone.Pages
{
    public partial class AddReport : PhoneApplicationPage
    {
        // blobs stuff
        string storageAccount = "dobberman";
        string storageKey = "9Uure7kO3RV71DRY+5rbMWurEuNfP1oCKkGvRi+/TlJq9nQbqU39FUYbPpqt+ml8qHfZNCGlxf4WBuhyO6gkdQ==";
        string blobServiceUri = "http://dobberman.blob.core.windows.net";
        CloudBlobClient blobClient;

        public static string LastMoodCheck;
        private Report newReport;
        private  List<Authority> AuthorityList = new List<Authority>();
        public AddReport()
        {
            InitializeComponent();
            

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            newReport = new Report();
            var credentials = new StorageCredentialsAccountAndKey(storageAccount, storageKey);
            blobClient = new CloudBlobClient(blobServiceUri, credentials);

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

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Decs.Text == "Please Decsribe.")
            {
                Decs.Text = "";
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

        private void SendReportClick(object sender, EventArgs e)
        {
            //Report NewReport = new Report();
            bool match = false;

            foreach (Authority a in AuthorityList)
            {
                if (autoCompleteBox1.Text == a.Name)
                {
                    match = true;
                    newReport.Authority = a;
                    newReport.AuthorityId = a.AuthorityId;
                }
            }

            if (match == false)
            {
                MessageBox.Show("Authority Not Recognized");
                goto reportEnd;

            }

            newReport.Description = this.Decs.Text;
            newReport.Location = "";
            if (LastMoodCheck == "")
            {
                MessageBox.Show("Please select report mood");
                goto reportEnd;
            }
            newReport.Date = System.DateTime.Now;
            newReport.Mood = LastMoodCheck;
            newReport.UserId = States.userId;
            

            if (checkBox2.IsChecked == true)
            {
                PostToWall(newReport);               
            }
            PostToServer(newReport);

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




        private void TakePhotoClick(object sender, EventArgs eventArgs)
        {
            //The camera chooser used to capture a picture.
            CameraCaptureTask ctask;

            //Create new instance of CameraCaptureClass
            ctask = new CameraCaptureTask();

            //Create new event handler for capturing a photo
            ctask.Completed += new EventHandler<PhotoResult>(ctask_Completed);

            //Show the camera.
            ctask.Show();

        }

        /// <summary>
        /// Event handler for retrieving the JPEG photo stream.
        /// Also to for decoding JPEG stream into a writeable bitmap and displaying.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ctask_Completed(object sender, PhotoResult e)
        {

            if (e.TaskResult == TaskResult.OK && e.ChosenPhoto != null)
            {

                WriteableBitmap CapturedImage = PictureDecoder.DecodeJpeg(e.ChosenPhoto);
                UploadToBlobContainer(e.ChosenPhoto);

                ReportImage.Source = CapturedImage;
                ReportImage.Visibility = Visibility.Visible;
            }
            else
            {
                //user decided not to take a picture
            }
        }
        private void UploadToBlobContainer(System.IO.Stream stream)
        {
            string containerName = "reportPictures";
            var container = blobClient.GetContainerReference(containerName);

            container.CreateIfNotExist(true, r =>
                Dispatcher.BeginInvoke(() =>
                {
                    // TODO reportid not yet created
                    var blobName = "report" + newReport.ReportId.ToString();
                    var blob = container.GetBlobReference(blobName);
                    blob.Metadata["ReportId"] = newReport.ReportId.ToString();
                    blob.Metadata["Date"] = newReport.Date.ToString();
                    blob.UploadFromStream(stream, r2 =>
                        Dispatcher.BeginInvoke(() =>
                        {
                             newReport.Photo = container.Uri + "/" + blobName;
                        }));
                }));

        }

     

       
  }
}
