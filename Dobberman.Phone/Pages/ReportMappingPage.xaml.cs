#define GPS_EMULATOR // defining a compiler GPS symbol.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
//using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using TAUP2C.Dobberman.Phone.Google_Maps;
using System.Device.Location;
using System.Xml.Serialization;
using System.Xml.Linq;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;
using System.Text;

using TAUP2C.Dobberman.Phone.DobbermanService;
using System.Globalization;


namespace TAUP2C.Dobberman.Phone.Pages
{
    public partial class ReportMappingPage : PhoneApplicationPage
    {
        List<GeoCoordinate> GeoLocationList = new List<GeoCoordinate>();
        List<Report> reportList = new List<Report>();
        MapLayer PushpinLayer = new MapLayer();
        private List<Pushpin> pins = new List<Pushpin>();
        GeoCoordinate coordinate = new GeoCoordinate();
        GeoCoordinate geotry = new GeoCoordinate(32.549, 35.234);
        //string geotrystring = geotry.ToString();
      //  string geotrystring = System.String.Empty;
       // geotrystring = geotry.ToString();
        double lat = 0, lon = 0;
      
        
        public ReportMappingPage()
        {
           
            InitializeComponent();
            googlemap.SetView(geotry, 10);
            DobbermanServiceClient client = new DobbermanServiceClient();
            client.GetAllReportsWithLocationCompleted += new EventHandler<GetAllReportsWithLocationCompletedEventArgs>(client_FindReportCompleted);
            client.GetAllReportsWithLocationAsync(1);
            //Loaded += new RoutedEventHandler(PivotPage1_Loaded);

        }
        /*from DB*/
        void client_FindReportCompleted(object sender, GetAllReportsWithLocationCompletedEventArgs e)
        {
            
            foreach (Report r in e.Result.Cast<Report>())
            {
                reportList.Add(r);
            }
          
        }

        /*coordunate from string to Geocordinate*/
        public GeoCoordinate ParseGeoCoordinate(string input)
        {
            // GeoCoordinate.ToString() uses InvariantCulture, so the doubles will use '.'
            // for decimal placement, even in european environments
            string[] parts = input.Split(',');

            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid format");
            }

            double latitude = Double.Parse(parts[0], CultureInfo.InvariantCulture);
            double longitude = Double.Parse(parts[1], CultureInfo.InvariantCulture);

            return new GeoCoordinate(latitude, longitude);
        }

        /*create pushpin*/
        private Pushpin CreatePin(string description, GeoCoordinate location)
        {
            Pushpin pin = new Pushpin();
            pin.Location = location;
            pin.Opacity = 0.4;
            pin.Content = description;
            return pin;
        }
  
       /*display all reports*/
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            
            /*buid the geolocationList*/
            for (int i = 0; i < reportList.Count; i++) // Loop through List with for
            {
                GeoLocationList.Add((ParseGeoCoordinate(reportList[i].Location)));
            }

            /*display pushpins*/

            for (int i = 0; i < GeoLocationList.Count; i++) // Loop through List with for
            {

                GeoCoordinate coord1 = new GeoCoordinate();
                 coord1 = GeoLocationList[i];

                Pushpin pin = new Pushpin();
                pin.Location = coord1;
                pin.Template = null; 
                pin.Content = new Ellipse()
                {
                    Fill = new SolidColorBrush(Colors.Red),
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 4,
                    Opacity = .8,
                    Height = 25,
                    Width = 25
                };
                googlemap.Children.Add(pin);
                
                googlemap.SetView(coord1, 7); //at the navigation     
            }
        }
    }
    }

    
