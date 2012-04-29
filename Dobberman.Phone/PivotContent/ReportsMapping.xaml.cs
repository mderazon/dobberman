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
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using Dobberman.Phone;

namespace TAUP2C.Dobberman.Phone.PivotContent
{
    public partial class ReportsMapping : UserControl
    {
        DobbermanServiceClient client = new DobbermanServiceClient();
        List<Report> GeoReportList = new List<Report>();
        MapLayer PushpinLayer = new MapLayer();
        private List<Pushpin> pins = new List<Pushpin>();
        
        public ReportsMapping()
        {
            InitializeComponent();
            googlemap.SetView(GetMyLocation(), 16);
            client.GetAllReportsWithLocationCompleted += new EventHandler<GetAllReportsWithLocationCompletedEventArgs>(client_GetAllReportsWithLocation);
            client.GetAllReportsWithLocationAsync(1);
        }

        void client_GetAllReportsWithLocation(object sender, GetAllReportsWithLocationCompletedEventArgs e)
        {
            foreach (Report r in e.Result.Cast<Report>())
            {
                GeoReportList.Add(r);
            }
            map.ItemsSource = GeoReportList;
        }

        private GeoCoordinate GetMyLocation()
        {

        #if REAL_WP7
                    IGeoPositionWatcher<GeoCoordinate> watcher;
                    watcher = new System.Device.Location.GeoCoordinateWatcher();

                    // Do not suppress prompt, and wait 1000 milliseconds to start.
                    watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
                    GeoCoordinate geoCenter = watcher.Position.Location;
                    watcher.Stop();
                    return geoCenter;
        #endif
        #if VIRTUAL_WP7
                GeoCoordinate geoCenter = new GeoCoordinate(32.114022, 34.803593);
                return geoCenter;
        #endif

        }

        private void Pushpin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            Report PushpinReport = (sender as Pushpin).DataContext as Report;
            States.CurReport = PushpinReport;
            
            var frame = App.Current.RootVisual as PhoneApplicationFrame;
            frame.Navigate(new Uri("/Pages/ReportDetails.xaml", UriKind.Relative));
            

        } 
    }
}