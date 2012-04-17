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

namespace TAUP2C.Dobberman.Phone.Converters
{
    public class BorderBrushConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var mood = (string) value;
            switch (mood)
            {
                case "positive":
                    return "DodgerBlue";
                case "negative":
                    return "IndianRed";
                case "concerned":
                    return "Gold";
                case null:
                    return "Gold";
            }
            return "DodgerBlue";
        }


        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
