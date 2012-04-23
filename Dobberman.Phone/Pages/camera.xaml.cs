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
using Microsoft.Samples.WindowsPhoneCloud.StorageClient;
using Microsoft.Samples.WindowsPhoneCloud.StorageClient.Credentials;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Phone;

namespace TAUP2C.Dobberman.Phone.Pages
{
    public partial class camera : PhoneApplicationPage
    {
        string storageAccount = "dobberman";
        string storageKey = "9Uure7kO3RV71DRY+5rbMWurEuNfP1oCKkGvRi+/TlJq9nQbqU39FUYbPpqt+ml8qHfZNCGlxf4WBuhyO6gkdQ==";
        string blobServiceUri = "http://dobberman.blob.core.windows.net";
        
        CloudBlobClient blobClient;

        public camera()
        {
            InitializeComponent();
            var credentials = new StorageCredentialsAccountAndKey(storageAccount,storageKey);
            blobClient = new CloudBlobClient(blobServiceUri, credentials);
        }




        private void button1_Click(object sender, RoutedEventArgs e)
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
                Camera.Visibility = Visibility.Visible;
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
                        var blobName = "my report name here";
                        var blob = container.GetBlobReference(blobName);
                        blob.Metadata["ReportId"] = ""; //TODO put something here
                        blob.UploadFromStream(stream, r2 =>
                            Dispatcher.BeginInvoke(() =>
                                {
                                    // Report.Photo = Container.Uri + "/" + blobName;
                                }));
                    }));

        }
        

    }
}