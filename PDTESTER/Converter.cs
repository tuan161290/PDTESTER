using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using static GPIOCommunication.GPIOHelper;

namespace PDTESTER
{
    public class StateToColorConverter : IValueConverter
    {
        private const PinValue ON = PinValue.ON;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PinValue)
            {
                if ((PinValue)value == PinValue.ON)
                    return new SolidColorBrush(Colors.LimeGreen);
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PinValueToGreenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PinValue)
            {
                if ((PinValue)value == PinValue.ON)
                    return new SolidColorBrush(Colors.LimeGreen);
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    //#FF7a96ea
    public class PinValueToRedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PinValue)
            {
                if ((PinValue)value == PinValue.ON)
                    return new SolidColorBrush(Colors.Red);
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PinValueToPurpleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PinValue)
            {
                if ((PinValue)value == PinValue.ON)
                    return new SolidColorBrush(Colors.LimeGreen);
            }
            return new SolidColorBrush(Color.FromArgb(0xFF, 0x7A, 0x96, 0xEA));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PinValueToOrangeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PinValue)
            {
                if ((PinValue)value == PinValue.ON)
                    return new SolidColorBrush(Colors.Orange);
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PackingPinStateToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PinValue)
            {
                if ((PinValue)value == PinValue.ON)
                    return new SolidColorBrush(Colors.DarkOrange);
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StateToEffectColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PinValue)
            {
                if ((PinValue)value == PinValue.ON)
                    return Colors.LimeGreen;
            }
            return Colors.DarkGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TextToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan)
            {
                TimeSpan Time = (TimeSpan)value;
                int seconds = (int)Time.TotalSeconds;
                return seconds.ToString();
            }
            else return "Error";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int temp;
            if (int.TryParse((string)value, out temp))
            {
                return TimeSpan.FromSeconds(temp);
            }
            else return TimeSpan.FromSeconds(120);
        }
    }

    public class PinStateToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PinValue)
            {
                if ((PinValue)value == PinValue.OFF)
                    return Visibility.Visible;
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsSetInJigConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value == true)
                    return Visibility.Visible;
            }
            return Visibility.Hidden;
            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsJigEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value == true)
                    return "USED";
            }
            return "NOT USED";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class boolToRoyalBlueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value == true)
                    return new SolidColorBrush(Colors.RoyalBlue);
            }
            return new SolidColorBrush(Colors.Gray);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TestItemToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is TestItemStatus)
            {
                switch (value)
                {
                    case TestItemStatus.FAIL:
                        return Red;
                    case TestItemStatus.PASS:
                        return LimeGreen;
                    case TestItemStatus.TEST:
                        return Orange;
                    default:
                        return DarkGray;
                }
            }
            return new SolidColorBrush(Colors.Gray);
            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        SolidColorBrush Red = new SolidColorBrush(Colors.Red);
        SolidColorBrush LimeGreen = new SolidColorBrush(Colors.LimeGreen);
        SolidColorBrush Yellow = new SolidColorBrush(Colors.Yellow);
        SolidColorBrush Orange = new SolidColorBrush(Colors.Orange);
        SolidColorBrush DarkGray = new SolidColorBrush(Colors.Gray);
    }

    public class TestResultToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is TestResult)
            {
                switch (value)
                {
                    case TestResult.FAIL:
                        return LightRed;
                    case TestResult.PASS:
                        return LimeGreen;
                    case TestResult.TEST:
                        return Yellow;
                    case TestResult.RETEST:
                        return LightOrange;
                    case TestResult.NOT_USE:
                        return Gray;
                    case TestResult.BLOCKED:
                        return Black;
                    case TestResult.ABORTED:
                        return new SolidColorBrush(Colors.Purple);
                }
            }
            //FF7a96ea
            return LightBlue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        //SolidColorBrush LightRed = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x66, 0x88));
        SolidColorBrush LightRed = new SolidColorBrush(Colors.Red);
        SolidColorBrush LimeGreen = new SolidColorBrush(Colors.LimeGreen);
        SolidColorBrush Yellow = new SolidColorBrush(Colors.Yellow);
        SolidColorBrush LightOrange = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC9, 0x88));
        SolidColorBrush LightBlue = new SolidColorBrush(Color.FromArgb(0xFF, 0x7A, 0x96, 0xEA));
        SolidColorBrush Gray = new SolidColorBrush(Colors.DarkGray);
        SolidColorBrush Black = new SolidColorBrush(Colors.Black);
    }

    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return value.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                int temp;
                if (int.TryParse(value.ToString(), out temp))
                    return temp;
                else return 0;
            }
            return 0;
        }
    }
}
