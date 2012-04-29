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
using System.Windows.Navigation;
using Dobberman.Phone;

namespace TAUP2C.Dobberman.Phone.PivotContent
{
    public partial class ReportsList : UserControl
    {
        //ObservableCollection<Stats> reportList = new ObservableCollection<Stats>();
        DobbermanServiceClient client = new DobbermanServiceClient();

        public ReportsList()
        {
            InitializeComponent();
            loading.Visibility = Visibility.Visible;
            client.GetReportsByUserIdCompleted += new EventHandler<GetReportsByUserIdCompletedEventArgs>(client_FindReportCompleted);
            client.GetReportsByUserIdAsync(States.userId);
        }

        void client_FindReportCompleted(object sender, GetReportsByUserIdCompletedEventArgs e)
        {
            loading.Visibility = Visibility.Collapsed;
            ReportList.ItemsSource = e.Result;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Report ButtonReport = (sender as Button).DataContext as Report;
            States.CurReport = ButtonReport;
            var frame = App.Current.RootVisual as PhoneApplicationFrame;
            frame.Navigate(new Uri("/Pages/ReportDetails.xaml", UriKind.Relative));
        }
    }
}