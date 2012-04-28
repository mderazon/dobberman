

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
      
        List<Report> GeoReportList = new List<Report>();
        MapLayer PushpinLayer = new MapLayer();
        private List<Pushpin> pins = new List<Pushpin>();
       
        GeoCoordinate geocenter = new GeoCoordinate(32.067, 34.47);
        
        public ReportMappingPage()
        {
           
            InitializeComponent();
            googlemap.SetView(geocenter, 9);
            DobbermanServiceClient client = new DobbermanServiceClient();
            client.GetAllReportsWithLocationCompleted += new EventHandler<GetAllReportsWithLocationCompletedEventArgs>(client_FindReportCompleted);
            client.GetAllReportsWithLocationAsync(1);
          
       
        }
        /*from DB*/
        void client_FindReportCompleted(object sender, GetAllReportsWithLocationCompletedEventArgs e)
        {
            
            foreach (Report r in e.Result.Cast<Report>())
            {
                GeoReportList.Add(r);
            }

            fuckit.ItemsSource = GeoReportList;
        }


        private void Pushpin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
{
       
            Report PushpinReport = (sender as Pushpin ).DataContext as Report;
            States.CurReport = PushpinReport;
        
            NavigationService.Navigate(new Uri("/Pages/ReportDetails.xaml", UriKind.Relative));
           
        }        

      
       
        }

     
    }
    

    
