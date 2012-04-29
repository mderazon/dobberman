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
using System.IO.IsolatedStorage;
using System.IO;
using ExifLib;

namespace TAUP2C.Dobberman.Phone.Pages
{
    public partial class AddReport : PhoneApplicationPage
    {
        // blobs stuff
        string storageAccount = "dobberman";
        string storageKey = "9Uure7kO3RV71DRY+5rbMWurEuNfP1oCKkGvRi+/TlJq9nQbqU39FUYbPpqt+ml8qHfZNCGlxf4WBuhyO6gkdQ==";
        string blobServiceUri = "http://dobberman.blob.core.windows.net";
        CloudBlobClient blobClient;
        private string location;
        public static string LastMoodCheck;
        private Report newReport;
        private  List<Authority> AuthorityList = new List<Authority>();
        private System.IO.Stream ChosenPhoto = null;
        private bool isUploading = false;
        public bool IsUploading
        {
            get
            {
                return this.isUploading;
            }

            set
            {
                if (this.isUploading != value)
                {
                    this.isUploading = value;
                    //this.NotifyPropertyChanged("IsUploading");
                }
            }
        }

        public AddReport()
        {
            InitializeComponent();
            newReport = new Report();
            DobbermanServiceClient client = new DobbermanServiceClient();
            client.GetAllAuthoritiesCompleted += new EventHandler<GetAllAuthoritiesCompletedEventArgs>(client_GetAllAuthoritiesCompleted);
            client.GetAllAuthoritiesAsync();
            

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            
            
            

            


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
            if (autoCompleteBox1.Text == "Start Typing...")
            {
                autoCompleteBox1.Text = "";
            }
        }
        #region mood buttons
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
        #endregion

        private void SendReportClick(object sender, EventArgs e)
        {
            this.UploadingBar.Visibility = Visibility.Visible;
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
            if (this.checkbox.IsChecked == true)
                newReport.Location = location;
            else
                newReport.Location = "";
            if (LastMoodCheck == "")
            {
                MessageBox.Show("Please select report mood");
                goto reportEnd;
            }
            newReport.Date = System.DateTime.Now;
            newReport.Mood = LastMoodCheck;
            newReport.UserId = States.userId;


            if (ChosenPhoto == null)
            {                
                PostToServer();
            }
            else
            {
                UploadToBlobContainer(ChosenPhoto);
            }
        reportEnd:
            return;
        }

        private void PostToWall()
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
                        this.NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.RelativeOrAbsolute));
                       
                    });
                }
            };

            var parameters = new Dictionary<string, object>
               {
                    { "message", "I have just posted a " + newReport.Mood + " report about " + newReport.Authority.Name +"!" },
                    { "link", newReport.Photo },
                    { "picture", newReport.Photo },
                    { "name", "This is what I have to say:" },
                    { "caption", "Posted with Dobberman App" },
                    { "description", newReport.Description },            
                };
            fb.PostAsync(newReport.Authority.FacebookPage + "/feed", parameters);
            
        }

        private void PostToServer()
        {

            DobbermanServiceClient client = new DobbermanServiceClient();
            client.CreateNewReportCompleted += new EventHandler<CreateNewReportCompletedEventArgs>(client_CreateNewReportCompletedE);
            client.CreateNewReportAsync(newReport);


        }

        void client_CreateNewReportCompletedE(object sender, CreateNewReportCompletedEventArgs e)
        {
            
            if (checkBox2.IsChecked == true)
            {
                PostToWall();
                MessageBox.Show("Report Sent Succesfully");
            }
        }

       
        private void GPScheckbox_Checked(object sender, RoutedEventArgs e)
        {
            location = System.String.Empty;
#if REAL_WP7
            IGeoPositionWatcher<GeoCoordinate> watcher;
            watcher = new System.Device.Location.GeoCoordinateWatcher();

            // Do not suppress prompt, and wait 1000 milliseconds to start.
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
            GeoCoordinate coord = watcher.Position.Location;

            if (coord.IsUnknown != true)
            {
                location = coord.ToString(); 
                watcher.Stop();
              
            }
                
            else
            {
                location = null;
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
                location = coord.ToString();
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

        void ctask_Completed(object sender, PhotoResult e)
        {

            if (e.TaskResult == TaskResult.OK && e.ChosenPhoto != null)
            {
                int angle = GetAngleFromExif(e.ChosenPhoto);
                WriteableBitmap CapturedImage = DecodeImage(e.ChosenPhoto, angle);
                // Create a file name for the JPEG file in isolated storage.
                String tempJPEG = "TempJPEG";

                // Create a virtual store and file stream. Check for duplicate tempJPEG files.
                var myStore = IsolatedStorageFile.GetUserStoreForApplication();
                if (myStore.FileExists(tempJPEG))
                {
                    myStore.DeleteFile(tempJPEG);
                }
                
                IsolatedStorageFileStream myFileStream = myStore.CreateFile(tempJPEG);

                //WriteableBitmap CapturedImage = PictureDecoder.DecodeJpeg(e.ChosenPhoto,400,400);

                CapturedImage.SaveJpeg(myFileStream, CapturedImage.PixelWidth, CapturedImage.PixelHeight,0,85);
                ChosenPhoto = new MemoryStream();
                myFileStream.Position = 0;
                myFileStream.CopyTo(ChosenPhoto);
                myFileStream.Close();
                //UploadToBlobContainer(ChosenPhoto);

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
            this.IsUploading = true;
            var credentials = new StorageCredentialsAccountAndKey(storageAccount, storageKey);
            blobClient = new CloudBlobClient(blobServiceUri, credentials);
            string containerName = "reportsphotos";
            var container = blobClient.GetContainerReference(containerName);

            container.CreateIfNotExist(true, r =>
                Dispatcher.BeginInvoke(() =>
                {
                    if (r.Exception == null)
                    {
                        // generate guid for the resource
                        var uniqueIdentifier = Guid.NewGuid();
                        var blobName = "report" + uniqueIdentifier.ToString() + ".jpg";
                        var blob = container.GetBlobReference(blobName);
                        blob.Metadata["ImageType"] = "image/jpeg";
                        //blob.Metadata["Date"] = newReport.Date.ToString();
                        blob.Metadata["FileExtension"] = "jpg";
                        blob.SetMetadata(r2 => { });

                        blob.UploadFromStream(
                            stream,
                            response => this.Dispatcher.BeginInvoke(
                            () =>
                            {
                                stream.Close();
                                this.IsUploading = false;

                                if (response.Exception == null)
                                {
                                    newReport.Photo = blob.Uri.ToString();
                                    PostToServer();
                                }
                                else
                                {
                                    MessageBox.Show(response.Exception.Message);
                                }
                            }));
                    }
                    else
                    {
                        MessageBox.Show(r.Exception.Message);
                    }
                }));

        }

        #region handle camera rotation
        int GetAngleFromExif(Stream imageStream)
        {
            var position = imageStream.Position;
            imageStream.Position = 0;
            ExifOrientation orientation = ExifReader.ReadJpeg(imageStream).Orientation;        
            imageStream.Position = position;
 
            switch (orientation)
            {
                case ExifOrientation.TopRight:
                    return 90;
                case ExifOrientation.BottomRight:
                    return 180;
                case ExifOrientation.BottomLeft:
                    return 270;
                case ExifOrientation.TopLeft:
                case ExifOrientation.Undefined:
                default:
                    return 0;
            }
        }
        private WriteableBitmap RotateBitmap(WriteableBitmap source, int width, int height, int angle)
        {
            var target = new WriteableBitmap(width, height); 
            int sourceIndex = 0;
            int targetIndex = 0;
            for (int x = 0; x < source.PixelWidth; x++)
            {
                for (int y = 0; y < source.PixelHeight; y++)
                {
                    sourceIndex = x + y * source.PixelWidth;
                    switch (angle) 
                    {
                        case 90:
                            targetIndex = (source.PixelHeight - y - 1) 
                                + x * target.PixelWidth;
                            break;
                        case 180:  
                            targetIndex = (source.PixelWidth - x - 1) 
                                + (source.PixelHeight - y - 1) * source.PixelWidth;
                            break;
                        case 270:  
                            targetIndex = y + (source.PixelWidth - x - 1) 
                                * target.PixelWidth;
                            break;
                    }
                    target.Pixels[targetIndex] = source.Pixels[sourceIndex]; 
                }
            }
            return target;
        }
        private WriteableBitmap DecodeImage(Stream imageStream, int angle)
        {
            WriteableBitmap source = PictureDecoder.DecodeJpeg(imageStream, 400, 400); 
 
            switch(angle)
            {
                case 90: 
                case 270: 
                    return RotateBitmap(source, source.PixelHeight, 
                        source.PixelWidth, angle);                 
                case 180: 
                    return RotateBitmap(source, source.PixelHeight, 
                        source.PixelWidth, angle);
                default:
                    return source;  
            }
        }
        #endregion
    }
}
