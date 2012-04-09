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

 
 

   
    public partial class PivotPage1 : PhoneApplicationPage
    {
        List<Report> reportList = new List<Report>();
        public PivotPage1()
        {
            InitializeComponent();
            DobbermanServiceClient client = new DobbermanServiceClient();
            client.GetReportByIdCompleted += new EventHandler<GetReportByIdCompletedEventArgs>(client_FindReportCompleted);
            client.GetReportByIdAsync("1");
            //Loaded += new RoutedEventHandler(PivotPage1_Loaded);
            
        }

        void client_FindReportCompleted(object sender, GetReportByIdCompletedEventArgs e)
        {
            Page f = new DisplayReport(e.Result);
            f.Name = "1.1";
            reportList.Add (e.Result);
            ReportList.ItemsSource = reportList;
        }

        void PivotPage1_Loaded(object sender, RoutedEventArgs e)
        {
            
            //String DefaultDate = "25/07/2010 21:17:00";
            //String DefaultAuthority = "";
            //String DefaultMood = "positive";
            //String RandomType = "";

        

            //for (int i = 0; i < 20; i++)
            //{

            //    defaultauthority = "ab" + i.tostring();
            //    switch (defaultmood)
            //    {
            //        case "positive":
            //            defaultmood = "negative";
            //            break;
            //        case "negative":
            //            defaultmood = "positive";
            //            break;

            //    }
            //reportlist.add(new reports(defaultauthority, defaultdate, defaultmood, ""));
            //}
            //Report rrr = new Report();
            //rrr.Mood = "Happy";
            //reportList.Add(rrr);
            
            ReportList.ItemsSource = reportList;

        }
       


// Handle selection changed on ListBox
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    // If selected index is -1 (no selection) do nothing
    if (ReportList.SelectedIndex == -1)
        return;
    // Navigate to the new page
    NavigationService.Navigate(new Uri("/1.1", UriKind.Relative));

    // Reset selected index to -1 (no selection)
    ReportList.SelectedIndex = -1;
}
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }
}