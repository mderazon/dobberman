using System;
using System.Collections.Generic;
using System.Device.Location;
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
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;
using TAUP2C.Dobberman.Phone.ReverseCoor;


namespace TAUP2C.Dobberman.Phone.Pages
{
    public partial class ReportDetails : PhoneApplicationPage
    {
        public ReportDetails()
        {
            InitializeComponent();
            

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.ContentPanel.DataContext = States.CurReport;
            switch (States.CurReport.Mood)
            {
                case ("positive"):
                    MoodText.Text = "Happy";
                    break;
                case "negative":
                    MoodText.Text = "Unhappy";
                    break;
                case "concerned":
                    MoodText.Text = "Unhappy";
                    break;
            }



            if (States.CurReport.Location == "")
            {
                this.LocationBox.Text = "No Location Provided";
            }
            else
            {
                GeoCoordinate location = new GeoCoordinate();
                string[] coordinate = States.CurReport.Location.Split((','));
                location.Latitude = Convert.ToDouble(coordinate[0]);
                location.Longitude = Convert.ToDouble(coordinate[1]);


                ReverseGeocodeRequest reverseGeocodeRequest = new ReverseGeocodeRequest();

                // Set the credentials using a valid Bing Maps key
                reverseGeocodeRequest.Credentials = new Credentials();
                reverseGeocodeRequest.Credentials.ApplicationId =
                    "ApIZ31eaMUXWc4YVuYaZZSi7LdLjbtdfHWcv7n-ax0z1-ytR8z9GbRxAVyLCJFC5";

                // Set the point to use to find a matching address
                //BingMaps.Location point = new BingMaps.Location();
                //point.Latitude = location.Latitude;
                //point.Longitude = location.Longitude;
                reverseGeocodeRequest.Location = new Location();
                reverseGeocodeRequest.Location.Latitude = location.Latitude;
                reverseGeocodeRequest.Location.Longitude = location.Longitude;


                // Make the reverse geocode request
                GeocodeServiceClient geocodeService = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
                geocodeService.ReverseGeocodeCompleted +=
                    new EventHandler<ReverseGeocodeCompletedEventArgs>(geocodeService_ReverseGeocodeCompleted);
                geocodeService.ReverseGeocodeAsync(reverseGeocodeRequest);
            }
        }


        void geocodeService_ReverseGeocodeCompleted(object sender, ReverseGeocodeCompletedEventArgs e)
        {
            string s = string.Format("{0}{2}", e.Result.Results[0].Address.AddressLine,
                                                       System.Environment.NewLine, e.Result.Results[0].Address.Locality);
            if (s == "")
            {
                this.LocationBox.Text = "Cannot Resolve Address";
            }
            else
            {


                this.LocationBox.Text = string.Format("{0}{1}{2}", e.Result.Results[0].Address.AddressLine,
                                                      System.Environment.NewLine, e.Result.Results[0].Address.Locality);
            }

        }
        
    }
}
