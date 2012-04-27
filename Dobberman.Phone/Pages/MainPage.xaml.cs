﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        List<Report> reportList = new List<Report>();
        ObservableCollection<Stats> ss = new ObservableCollection<Stats>();
        DobbermanServiceClient client = new DobbermanServiceClient();
        public MainPage()
        {
            InitializeComponent();
            
           
            //Loaded += new RoutedEventHandler(PivotPage1_Loaded);

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            
            client.GetReportsByUserIdCompleted += new EventHandler<GetReportsByUserIdCompletedEventArgs>(client_FindReportCompleted);
            client.GetReportsByUserIdAsync(1);
            client.GetAllCategoriesCompleted += new EventHandler<GetAllCategoriesCompletedEventArgs>(client_GetAllCategoriesCompleted);
            client.GetAllCategoriesAsync();
            



        }

        void client_GetAllCategoriesCompleted(object sender, GetAllCategoriesCompletedEventArgs e)
        {
            client.GetAuthoritiesByCategoryIdCompleted += new EventHandler<GetAuthoritiesByCategoryIdCompletedEventArgs>(client_GetAuthoritiesByCategoryIdCompleted);
            ss.Clear();
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
                StatisticsList.ItemsSource = ss;
               
            }
            
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