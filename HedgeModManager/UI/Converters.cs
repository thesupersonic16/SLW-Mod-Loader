﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Markdig;

namespace HedgeModManager
{
    [ValueConversion(typeof(string), typeof(string))]
    public class ModUpdateStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var server = (string)value;
            if (string.IsNullOrEmpty(server))
                return Lang.Localise("CommonUINo");

            return Singleton.GetInstance<NetworkConfig>().IsServerBlocked(server)
                ? Lang.Localise("CommonUIBlocked")
                : Lang.Localise("CommonUIYes");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(string), typeof(Visibility))]
    public class EmptyStringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;

            return string.IsNullOrWhiteSpace(str) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(string), typeof(Visibility))]
    public class InverseEmptyStringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;

            return !string.IsNullOrWhiteSpace(str) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(string), typeof(string))]
    public class MarkdownToHtmlConverter : IValueConverter
    {
        private static MarkdownPipeline Pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string md = value?.ToString();
            if (string.IsNullOrEmpty(md))
                return string.Empty;

            return $@"
            <html>
                <body>
                    <style>
                        {HedgeMessageBox.GetHtmlStyleSheet()}
                    </style>
                        {Markdown.ToHtml(md, Pipeline)}
                </body>
            </html>";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool val))
                return Visibility.Collapsed;

            return val ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility vis))
                throw new ArgumentException("Invalid argument");

            return vis == Visibility.Visible;
        }
    }

    [ValueConversion(typeof(object), typeof(bool))]
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
