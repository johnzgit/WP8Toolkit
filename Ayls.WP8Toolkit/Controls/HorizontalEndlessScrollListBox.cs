using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Ayls.WP8Toolkit.Controls
{
    public class HorizontalEndlessScrollListBox : ListBox
    {
        private const string CompressionRightState = "CompressionRight";
        private const string NoHorizonatalCompressionState = "NoHorizontalCompression";
        private const string HorizontalCompressionGroupName = "HorizontalCompression";
        private const int PullDownInterval = 1;

        private ScrollViewer _scrollViewer = null;
        private bool _alreadyHookedScrollEvents = false;
        private DispatcherTimer _addToHeadTimer;
        private bool _refresh = false;

        public HorizontalEndlessScrollListBox()
        {
            Loaded += ListBox_Loaded;
        }

        public static readonly DependencyProperty LoadNextCommandProperty =
            DependencyProperty.Register("LoadNextCommand", typeof(ICommand), typeof(HorizontalEndlessScrollListBox), new PropertyMetadata(null));

        public ICommand LoadNextCommand
        {
            get { return (ICommand)GetValue(LoadNextCommandProperty); }
            set { SetValue(LoadNextCommandProperty, value); }
        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (_alreadyHookedScrollEvents)
                return;

            _alreadyHookedScrollEvents = true;

            _scrollViewer = (ScrollViewer)FindElementRecursive(this, typeof(ScrollViewer));

            if (_scrollViewer != null)
            {
                // Visual States are always on the first child of the control template 
                var element = VisualTreeHelper.GetChild(_scrollViewer, 0) as FrameworkElement;
                if (element != null)
                {
                    VisualStateGroup horizontalGroup = FindVisualState(element, HorizontalCompressionGroupName);

                    if (horizontalGroup != null)
                        horizontalGroup.CurrentStateChanging += HorizontalGroupCurrentStateChanging;
                }
            }
        }

        private void HorizontalGroupCurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState.Name == CompressionRightState)
            {
                _addToHeadTimer = new DispatcherTimer();
                _refresh = false;
                _addToHeadTimer.Interval = TimeSpan.FromSeconds(PullDownInterval);
                _addToHeadTimer.Tick += (s, ea) => { _refresh = true; };
                _addToHeadTimer.Start();
            }

            if (e.NewState.Name == NoHorizonatalCompressionState)
            {
                if (_addToHeadTimer != null &&
                    _addToHeadTimer.IsEnabled)
                {
                    _addToHeadTimer.Stop();

                    if (_refresh && LoadNextCommand != null && LoadNextCommand.CanExecute(ItemsSource))
                    {
                        _refresh = false;
                        LoadNextCommand.Execute(ItemsSource);
                    }
                }
            }
        }

        private static UIElement FindElementRecursive(FrameworkElement parent, Type targetType)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            UIElement returnElement = null;

            if (childCount > 0)
                for (int i = 0; i < childCount; i++)
                {
                    Object element = VisualTreeHelper.GetChild(parent, i);
                    if (element.GetType() == targetType)
                    {
                        return element as UIElement;
                    }
                    else
                    {
                        returnElement = FindElementRecursive(VisualTreeHelper.GetChild(parent, i) as FrameworkElement, targetType);
                    }
                }

            return returnElement;
        }

        private static VisualStateGroup FindVisualState(FrameworkElement element, string name)
        {
            if (element == null)
                return null;

            var groups = VisualStateManager.GetVisualStateGroups(element);

            foreach (VisualStateGroup group in groups)
            {
                if (group.Name == name)
                {
                    return group;
                }
            }

            return null;
        }
    }
}
