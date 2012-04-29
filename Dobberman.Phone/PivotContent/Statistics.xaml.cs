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
using System.Collections.ObjectModel;
using TAUP2C.Dobberman.Phone.DobbermanService;

namespace TAUP2C.Dobberman.Phone.PivotContent
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
        public Stats(string a, string b, string c, string d, string e, string f, string g)
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
    public partial class Statistics : UserControl
    {
        ObservableCollection<Stats> reportList = new ObservableCollection<Stats>();
        ObservableCollection<Stats> ss = new ObservableCollection<Stats>();
        DobbermanServiceClient client = new DobbermanServiceClient();
        public Statistics()
        {
            InitializeComponent();
            loading.Visibility = Visibility.Visible;
            client.GetAllCategoriesCompleted += new EventHandler<GetAllCategoriesCompletedEventArgs>(client_GetAllCategoriesCompleted);
            client.GetAllCategoriesAsync();
        }

        void client_GetAllCategoriesCompleted(object sender, GetAllCategoriesCompletedEventArgs e)
        {
            client.GetAuthoritiesByCategoryIdCompleted += new EventHandler<GetAuthoritiesByCategoryIdCompletedEventArgs>(client_GetAuthoritiesByCategoryIdCompleted);
            ss.Clear();
            ss = new ObservableCollection<Stats>();
            int categoryCount = e.Result.Count;
            int i = 1;
            foreach (Category c in e.Result.Cast<Category>())
            {
                if (i == categoryCount)
                {
                    loading.Visibility = Visibility.Collapsed;
                }
                client.GetAuthoritiesByCategoryIdAsync(c.CategoryId);
                i++;
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
                    s.TopThird = e.Result.ElementAt(2).Name;
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
    }
}