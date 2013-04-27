using System.Windows;
using System.Windows.Controls;

namespace Ayls.WP8Toolkit.Controls
{
    public class IndeterminateProgressBar : ProgressBar
    {
        public IndeterminateProgressBar()
        {
            IsIndeterminate = true;
        }

        public static readonly DependencyProperty IndicatorEnabledProperty =
            DependencyProperty.Register("IndicatorEnabled", typeof(bool), typeof(IndeterminateProgressBar), new PropertyMetadata(true, IndicatorEnabledPropertyChanged));

        private static void IndicatorEnabledPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var progressBar = sender as ProgressBar;
            if (progressBar != null)
            {
                if ((bool)e.NewValue)
                {
                    progressBar.Visibility = Visibility.Visible;
                    progressBar.Value = 0;
                }
                else
                {
                    progressBar.Visibility = Visibility.Collapsed;
                }
            }
        }

        public bool IndicatorEnabled
        {
            get { return (bool)GetValue(IndicatorEnabledProperty); }
            set { SetValue(IndicatorEnabledProperty, value); }
        }

    }
}
