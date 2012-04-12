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
    public class Reports
    {
        public string authority { get; set; }
        public string date { get; set; }
        public string mood { get; set; }
        public string description { get; set; }
        public string moodImage { get; set; }

        public Reports(string authority, string date, string mood, string description)
        {
            this.authority = authority;
            this.date = date;
            this.mood = mood;
            this.description = description;
            this.moodImage = "Images/appbar.add.rest.png";

            switch (mood)
            {
                case "positive":
                    this.moodImage = "Images/appbar.add.rest.png";
                    break;
                case "negative":
                    this.moodImage = "Images/appbar.delete.rest.png";
                    break;

            }
        }


    }





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

            String DefaultDate = "25/07/2010 21:17:00";
            String DefaultAuthority = "";
            String DefaultMood = "positive";
            String RandomType = "";
            List<Reports> reportList = new List<Reports>();


            for (int i = 0; i < 20; i++)
            {

                DefaultAuthority = "ab" + i.ToString();
                switch (DefaultMood)
                {
                    case "positive":
                        DefaultMood = "negative";
                        break;
                    case "negative":
                        DefaultMood = "positive";
                        break;

                }
                reportList.Add(new Reports(DefaultAuthority, DefaultDate, DefaultMood, ""));
            }


            ReportList.ItemsSource = reportList;

        }



        // Handle selection changed on ListBox

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page1.xaml", UriKind.Relative));
            object data = (sender as Button).DataContext as object;
            ListBoxItem pressedItem = this.ReportList.ItemContainerGenerator.ContainerFromItem(data) as ListBoxItem;
            if (pressedItem != null)
            {
                NavigationService.Navigate(new Uri("/Page1.xaml", UriKind.Relative));

            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }
}