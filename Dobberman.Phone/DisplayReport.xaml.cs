﻿using System;
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

namespace TAUP2C.Dobberman.Phone
{
    public partial class DisplayReport : PhoneApplicationPage
    {
        public DisplayReport(DobbermanService.Report R)
        {
            InitializeComponent();
            this.xxx.Text = R.Mood;
        }
    }
}