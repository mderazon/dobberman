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
using TAUP2C.Dobberman.Phone.Helpers;

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
        List<Report> reportList = new List<Report>();
        List<Stats> ss = new List<Stats>();
        public MainPage()
        {
            InitializeComponent();
           
            DobbermanServiceClient client = new DobbermanServiceClient();
            client.GetReportsByUserIdCompleted += new EventHandler<GetReportsByUserIdCompletedEventArgs>(client_FindReportCompleted);
            client.GetReportsByUserIdAsync(1);
            //Loaded += new RoutedEventHandler(PivotPage1_Loaded);

        }

        void client_FindReportCompleted(object sender, GetReportsByUserIdCompletedEventArgs e)
        {
            
            foreach (Report r in e.Result.Cast<Report>())
            {
                reportList.Add(r);
            }


            ss.Add(new Stats("Cellcom", "Pelefon", "Orange", "Yes", "Hot", "Netvision", "Communucation"));
            ss.Add(new Stats("Tel-Aviv University", "Technion", "HIT", "MIT", "Yell", "Columbia", "Studies"));
            ss.Add(new Stats("Cellcom", "Pelefon", "Orange", "Yes", "Hot", "Netvision", "Resturants"));
            ss.Add(new Stats("Tel-Aviv University", "Technion", "HIT", "MIT", "Yell", "Columbia", "Hotels"));
            ss.Add(new Stats("Cellcom", "Pelefon", "Orange", "Yes", "Hot", "Netvision", "Authorities"));
            ss.Add(new Stats("Tel-Aviv University", "Technion", "HIT", "MIT", "Yell", "Columbia", "Studies"));
            StatisticsList.ItemsSource = ss;

            ReportList.ItemsSource = reportList;
            
        }

        void PivotPage1_Loaded(object sender, RoutedEventArgs e)
        {

       

        }



        // Handle selection changed on ListBox

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Report ButtonReport = ( sender as Button ).DataContext as Report;
           States.CurReport = ButtonReport;
            //ReportDetails P = new ReportDetails(ButtonReport);


            NavigationService.Navigate(new Uri("/Pages/ReportDetails.xaml", UriKind.Relative));
            //object data = (sender as Button).DataContext as object;
            //ListBoxItem pressedItem = this.ReportList.ItemContainerGenerator.ContainerFromItem(data) as ListBoxItem;
            //if (pressedItem != null)
            //{
            //    NavigationService.Navigate(new Uri("/Page1.xaml", UriKind.Relative));

            //}
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {

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