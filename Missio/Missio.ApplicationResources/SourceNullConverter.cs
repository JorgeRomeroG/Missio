﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace Missio.ApplicationResources
{
    public class SourceNullConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null) return true;
            return false;
        }
    

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
