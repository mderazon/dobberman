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


namespace TAUP2C.Dobberman.Phone.Pages
{
    public partial class ReportMappingPage : PhoneApplicationPage
    {
        MapLayer PushpinLayer = new MapLayer();
        private List<Pushpin> pins = new List<Pushpin>();
        GeoCoordinate coordinate = new GeoCoordinate();
        double lat = 0, lon = 0;
        public ReportMappingPage()
        {
            InitializeComponent();
        }
        private Pushpin CreatePin(string description, GeoCoordinate location)
        {
            Pushpin pin = new Pushpin();
            pin.Location = location;
            pin.Opacity = 0.4;
            pin.Content = description;
            // if (isHoldEvent)
            //{
            //  pin.Hold += new EventHandler<System.Windows.Input.GestureEventArgs>(pinRemove_Hold);
            //}
            return pin;
        }

        /*   private void googlemap_Hold(object sender, System.Windows.Input.GestureEventArgs e)
           {
               Point p = e.GetPosition(this.googlemap);
               GeoCoordinate geo = new GeoCoordinate();
               geo = googlemap.ViewportPointToLocation(p);
               GetPinPoint(geo, true);
           }

           private void GetPinPoint(GeoCoordinate geo, bool isHoldEvent)
           {
               Pushpin pin = CreatePin("Loading...", geo, isHoldEvent);
               googlemap.Children.Add(pin);
               string searchLocation = geo.Latitude + "," + geo.Longitude;
               Uri requestAddress = new Uri("https://maps.googleapis.com/maps/api/geocode/xml?latlng=" + searchLocation + "&sensor=true");
               WebClient downloader = new WebClient();
               downloader.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Downloaded_Revese);
               downloader.DownloadStringAsync(requestAddress, pin);
           }
           */
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        private void GestureListener_Tap(object sender, GestureEventArgs e)
        {
            int success = 7;
            this.NavigationService.Navigate(new Uri("/Pages/Page1.xaml", UriKind.Relative));
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            IGeoPositionWatcher<GeoCoordinate> watcher;

            watcher = new System.Device.Location.GeoCoordinateWatcher();

            // Do not suppress prompt, and wait 1000 milliseconds to start.
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
            GeoCoordinate coord = watcher.Position.Location;

            if (coord.IsUnknown != true)
            {

                //  Console.WriteLine("Lat: {0}, Long: {1}",
                //      coord.Latitude,
                //    coord.Longitude);
               // coord.Latitude = 31.802;
                //coord.Longitude = 34.848;

                 GeoCoordinate coord1 = new GeoCoordinate();
            List<GeoCoordinate> myList = new List<GeoCoordinate>();

            myList.Add(new GeoCoordinate(31.101, 35.145));
            myList.Add(new GeoCoordinate(31.838, 34.878));
            myList.Add(new GeoCoordinate(32.549, 35.234));
                coord1 = myList[RandomNumber(0, 2)];
 

                Pushpin pin = new Pushpin();
                pin.Location = coord1;

                // googlemap.Children.Add(PushpinLayer);
                /*    Image image = new Image(); 
                    BitmapImage bmi = new BitmapImage(new Uri("L_tshirt_front.png", UriKind.Relative));
                    //img.Source = bmi;
               
                    textBlock.Text = "My Pushpin";
                    Grid grid = new Grid();
                    grid.Children.Add(image);
                    grid.Children.Add(textBlock); 
                
                   // image.Source = ("Images\pushpin.png");
                    image.Width = 40;
                    image.Height = 40;
                //    PushpinLayer.AddChild(image,coord);
                  
                    //new Microsoft.Maps.MapControl.Location(42.658, -71.137),  
            //PositionOrigin.Center);*/
                watcher.Stop();
                googlemap.Children.Add(pin);
                //   pin.DoubleTap = new EventHandler<System.Windows.Input.GestureEventArgs>(pin1_MouseLeftButtonUp);

                var gl = GestureService.GetGestureListener(pin);
                gl.Tap += new EventHandler<GestureEventArgs>(GestureListener_Tap); 

            }
                
            else
            {
                print.Text = ("Unknown latitude and longitude");
                //Console.WriteLine("Unknown latitude and longitude.");
            }
        }


    }
    }

    


/*
MapLayer m_PushpinLayer = new MapLayer();
Your_Map.Children.Add(m_PushpinLayer);
Image image = new Image();
image.Source = ResourceFile.GetBitmap("Images/Me.png", From.This);
image.Width = 40;
image.Height = 40;
m_PushpinLayer.AddChild(image,
    new Microsoft.Maps.MapControl.Location(42.658, -71.137),  
        PositionOrigin.Center);



 private void AddPinToPointsOfIntrests(Pushpin pin)
        {
            WayPoint point = new WayPoint();
            point.Description = pin.Content.ToString();
            point.LocationLat = pin.Location.Latitude;
            point.LocationLng = pin.Location.Longitude;
            this.view.Favorits.PointOfInterst.Add(point);
        }

        private Pushpin CreatePin(string description, GeoCoordinate location, bool isHoldEvent)
        {
            Pushpin pin = new Pushpin();
            pin.Location = location;
            pin.Opacity = 0.4;
            pin.Content = description;
            if (isHoldEvent)
            {
                pin.Hold += new EventHandler<System.Windows.Input.GestureEventArgs>(pinRemove_Hold);
            }
            return pin;*/