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


namespace TAUP2C.Dobberman.Phone.Pages
{
    public partial class ReportDetails : PhoneApplicationPage
    {
        public ReportDetails()
        {
            InitializeComponent();
            

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.ContentPanel.DataContext = States.CurReport;
            switch (States.CurReport.Mood)
            {
                case ("positive"):
                    MoodText.Text = "Happy";    
                    break;
                case "negative":
                    MoodText.Text = "Unhappy";
                    break;
                case "concerned":
                    MoodText.Text = "Unhappy";
                    break;
            }

        }
        
    }
}
