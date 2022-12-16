﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace RunAsManager.Converters
{
    public class BooleanNegativeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value
            is bool boolean ? !boolean : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolean ? !boolean : null;
        }
    }
}
