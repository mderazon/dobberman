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
//using Microsoft.Maps.MapControl.WPF;
using TAUP2C.Dobberman.Phone.DobbermanService;
using System.Globalization;



namespace TAUP2C.Dobberman.Phone.Pages
{
    public partial class ReportMappingPage : PhoneApplicationPage
    {
        List<GeoCoordinate> GeoLocationList = new List<GeoCoordinate>();
        List<string> GeoLocationListStrings = new List<string>();
        List<Report> reportList = new List<Report>();
        MapLayer PushpinLayer = new MapLayer();
        private List<Pushpin> pins = new List<Pushpin>();
        GeoCoordinate coordinate = new GeoCoordinate();
        GeoCoordinate geotry = new GeoCoordinate(32.067, 34.47);
        //string geotrystring = geotry.ToString();
      //  string geotrystring = System.String.Empty;
       // geotrystring = geotry.ToString();
        double lat = 0, lon = 0;
      
        
        public ReportMappingPage()
        {
           
            InitializeComponent();
            googlemap.SetView(geotry, 9);
            DobbermanServiceClient client = new DobbermanServiceClient();
            client.GetAllReportsWithLocationCompleted += new EventHandler<GetAllReportsWithLocationCompletedEventArgs>(client_FindReportCompleted);
            client.GetAllReportsWithLocationAsync(1);
            //Loaded += new RoutedEventHandler(PivotPage1_Loaded);
           /* Locations = new ObservableCollection<GeoCoordinate>() {
                new GeoCoordinate(-56, 73),
                new GeoCoordinate(-14, 120),
                new GeoCoordinate(48, -133),
                new GeoCoordinate(-2, 11),
                new GeoCoordinate(0, 40),
                new GeoCoordinate(-78, -85),
            };*/
           //CurrentLocation = Locations[0];
            GeoCoordinate g1 = new GeoCoordinate(31.201, 35.145);
            GeoCoordinate g2 = new GeoCoordinate(31.101, 35.145);
         //   GeoLocationList.Add(g1);
         //   GeoLocationList.Add(g2);
          
         // pusha.DataContext = g1;
     //    fuckit.ItemsSource = GeoLocationList;
            //create a new array wcih contains all the fields but the geo field is real and not string
             
        //    DataContext = this;

            //fuckit.ItemsSource = GeoLocationList;
        }
        /*from DB*/
        void client_FindReportCompleted(object sender, GetAllReportsWithLocationCompletedEventArgs e)
        {
            
            foreach (Report r in e.Result.Cast<Report>())
            {
                reportList.Add(r);
            }

            
            /*for (int i = 0; i < reportList.Count; i++)
            {   // Loop through List with for
                GeoLocationList.Add((ParseGeoCoordinate(reportList[i].Location)));      
            }*/

            GeoCoordinate g1 = new GeoCoordinate(31.201, 35.145);
            GeoCoordinate g2 = new GeoCoordinate(31.101, 35.145);
            GeoLocationList.Add(g1);
            GeoLocationList.Add(g2);
            GeoLocationListStrings.Add("31.201, 35.145");
            GeoLocationListStrings.Add("31.507, 35.607");
            fuckit.ItemsSource = reportList;
        }


        private void Pushpin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
{
       
            Report PushpinReport = (sender as Pushpin ).DataContext as Report;
            States.CurReport = PushpinReport;
            //ReportDetails P = new ReportDetails(ButtonReport);


            NavigationService.Navigate(new Uri("/Pages/ReportDetails.xaml", UriKind.Relative));
            //object data = (sender as Button).DataContext as object;
            //ListBoxItem pressedItem = this.ReportList.ItemContainerGenerator.ContainerFromItem(data) as ListBoxItem;
            //if (pressedItem != null)
            //{
            //    NavigationService.Navigate(new Uri("/Page1.xaml", UriKind.Relative));

            //}
        }        

        /*coordunate from string to Geocordinate*/
        public GeoCoordinate ParseGeoCoordinate(string input)
        {
            // GeoCoordinate.ToString() uses InvariantCulture, so the doubles will use '.'
            // for decimal placement, even in european environments
            string[] parts = input.Split(',');

            
                if (parts.Length != 2)
                {
                    // throw new ArgumentException("Invalid format");
                    return new GeoCoordinate(32.567, 35.267);
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

        private void GestureListener_Tap(object sender, GestureEventArgs e)
        {
            //  int success = 7;
            this.NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
          //  (Pushpin) e.OriginalSource.
            //Point pos = e.GetPosition(img);
            // string text = "I'm a popup!";
            // Popup popup = new Popup();
            // popup.IsOpen = true;
        }

       /*display all reports*/
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            
                GeoCoordinate lcn = new GeoCoordinate();
            /*buid the geolocationList*/
            for (int i = 0; i < reportList.Count; i++)
            {   // Loop through List with for
                
            
                GeoLocationList.Add((ParseGeoCoordinate(reportList[i].Location)));
                  //fuckit.ItemsSource = GeoLocationList;
            }

            /*display pushpins*/

            for (int i = 0; i < GeoLocationList.Count; i++) // Loop through List with for
            {

                GeoCoordinate coord1 = new GeoCoordinate();
                 coord1 = GeoLocationList[i];

                Pushpin pin = new Pushpin();
                pin.Location = coord1;
                pin.Template = null;
                pin.Content = "hell";
              
               // Pushpin p = new Pushpin(
               // Create the infobox for the pushpin

                //Template="{StaticResource PinTemplate}"

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
                //what to do on pushpin tapping
                var gl = GestureService.GetGestureListener(pin);
                gl.Tap += new EventHandler<GestureEventArgs>(GestureListener_Tap); 
            }
        }

     
    }
    }

    
