﻿
#define VIRTUAL_WP7
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TAUP2C.Dobberman.Phone.DobbermanService;
using TAUP2C.Dobberman.Phone.Helpers;
using TAUP2C.Dobberman.Phone.PivotContent;

namespace TAUP2C.Dobberman.Phone.Pages
{
    public class Stats
    {
        public string TopFirst { get; set; }
        public string TopSecond { get; set; }
        public string TopThird { get; set; }
        public string BottomFirst { get; set; }
        public string BottomSecond { get; set; }
        public string BottomThird { get; set; }
        public string Type { get; set; }
       

        public Stats(string c)
        {
            TopFirst = "";
            TopSecond = "";
            TopThird = "";
            BottomFirst = "";
            BottomSecond = "";
            BottomThird = "";
            Type = c;
          
        }
        public Stats (string a, string b, string c, string d, string e, string f, string g)
        {
            TopFirst = a;
            TopSecond = b;
            TopThird = c;
            BottomFirst = d;
            BottomSecond = e;
            BottomThird = f;
            Type = g;
        }

    }
    public partial class MainPage : PhoneApplicationPage
    {

        ObservableCollection<Stats> reportList = new ObservableCollection<Stats>();
        ObservableCollection<Stats> ss = new ObservableCollection<Stats>();
        DobbermanServiceClient client = new DobbermanServiceClient();
        List<Report> GeoReportList = new List<Report>();
        MapLayer PushpinLayer = new MapLayer();
        private List<Pushpin> pins = new List<Pushpin>();

        private void OnLoadingPivotItem(object sender, PivotItemEventArgs e)
        {
            if (e.Item.Content != null)
            {
                // Content loaded already
                return;
            }

            Pivot pivot = (Pivot)sender;

            if (e.Item == pivot.Items[0])
            {
                e.Item.Content = new ReportsList();
            }
            else if (e.Item == pivot.Items[1])
            {
                e.Item.Content = new Statistics();
            }
            else if (e.Item == pivot.Items[2])
            {
                e.Item.Content = new ReportsMapping();
            }
        }

        public MainPage()
        {
            InitializeComponent();
            //googlemap.SetView(GetMyLocation(), 16);
            //client.GetAllCategoriesCompleted += new EventHandler<GetAllCategoriesCompletedEventArgs>(client_GetAllCategoriesCompleted);
            //client.GetAllCategoriesAsync();
            //client.GetAllReportsWithLocationCompleted += new EventHandler<GetAllReportsWithLocationCompletedEventArgs>(client_GetAllUsers);
            //client.GetAllReportsWithLocationAsync(1);
        }

        void client_GetAllUsers(object sender, GetAllReportsWithLocationCompletedEventArgs e)
        {
            foreach (Report r in e.Result.Cast<Report>())
            {
                GeoReportList.Add(r);
            }
            // TODO fuckit.ItemsSource = GeoReportList;
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

        //protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);

            
        //    client.GetReportsByUserIdCompleted += new EventHandler<GetReportsByUserIdCompletedEventArgs>(client_FindReportCompleted);
        //    client.GetReportsByUserIdAsync(States.userId);
            
            



        //}
        //olga - itemsource or datacontext for pushpin - tells what to bind. binding is needed to be at scope.
        void client_GetAllCategoriesCompleted(object sender, GetAllCategoriesCompletedEventArgs e)
        {
            client.GetAuthoritiesByCategoryIdCompleted += new EventHandler<GetAuthoritiesByCategoryIdCompletedEventArgs>(client_GetAuthoritiesByCategoryIdCompleted);
            ss.Clear();
            ss = new ObservableCollection<Stats>();
            foreach (Category c in e.Result.Cast<Category>())
            {
                
                client.GetAuthoritiesByCategoryIdAsync(c.CategoryId);
            }

            

        }

        void client_GetAuthoritiesByCategoryIdCompleted(object sender, GetAuthoritiesByCategoryIdCompletedEventArgs e)
        {

            int i = e.Result.Cast<Authority>().Count();
            
            if (i > 0)
            {
                Stats s = new Stats(e.Result.ElementAt(0).Category.Name);

                    s.TopFirst = e.Result.ElementAt(0).Name;
                if (i > 1)
                    s.TopSecond = e.Result.ElementAt(1).Name;
                if (i > 2)
                    s.TopThird= e.Result.ElementAt(2).Name;
                if (i > 3)
                    s.BottomThird = e.Result.ElementAt(i - 3).Name;
                if (i > 4)
                    s.BottomSecond = e.Result.ElementAt(i - 2).Name;
                if (i > 5)
                    s.BottomFirst = e.Result.ElementAt(i - 1).Name;
               
                ss.Add(s);

              
                //TODO StatisticsList.ItemsSource = ss;

            }
            
        }


        private void Pushpin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            Report PushpinReport = (sender as Pushpin).DataContext as Report;
            States.CurReport = PushpinReport;

            NavigationService.Navigate(new Uri("/Pages/ReportDetails.xaml", UriKind.Relative));

        } 

        void client_FindReportCompleted(object sender, GetReportsByUserIdCompletedEventArgs e)
        {

           

            // TODO ReportList.ItemsSource = e.Result;
            
        }

       




        // Handle selection changed on ListBox

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Report ButtonReport = ( sender as Button /*as pushpin*/).DataContext as Report;
           States.CurReport = ButtonReport;
           


            NavigationService.Navigate(new Uri("/Pages/ReportDetails.xaml", UriKind.Relative));
            
        }

       

        private void AddReport_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/AddReport.xaml", UriKind.Relative));
        }

        private void OnLogout(object sender, EventArgs e)
        {
            this.CleanUp();
        }

        private void CleanUp()
        {
            // Clean the current authentication token, flags and view models.
            PhoneHelpers.SetApplicationState("UserBackPress", false);
            PhoneHelpers.RemoveIsolatedStorageSetting("UserIsRegistered");



            // Navigate to the log in page.
            this.NavigationService.GoBack();
        }
    }
}