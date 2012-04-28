using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls.Maps;
using TAUP2C.Dobberman.Phone.Google_Maps;
using System.Device.Location;
using System.Globalization;

namespace TAUP2C.Dobberman.Phone.Converters
{
    public class StringToGeoConverter : IValueConverter
    {
        //olga
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var location = (string) value;
            string[] parts = location.Split(',');
            if (parts.Length != 2)
            {
                // throw new ArgumentException("Invalid format");
                return new GeoCoordinate(32.567, 35.267);
            }
            double latitude = Double.Parse(parts[0], CultureInfo.InvariantCulture);
            double longitude = Double.Parse(parts[1], CultureInfo.InvariantCulture);

            return new GeoCoordinate(latitude, longitude);
           // return new GeoCoordinate(32.567, 35.267);
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
             throw new NotImplementedException();
        }
    }
}






  

