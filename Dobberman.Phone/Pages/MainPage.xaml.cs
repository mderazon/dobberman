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

namespace TAUP2C.Dobberman.Phone.Pages
{
    



    public partial class MainPage : PhoneApplicationPage
    {
        List<Report> reportList = new List<Report>();
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
    }
}