using System;
using System.Collections.Specialized;
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
        private const int PullInterval = 1;

        private ScrollViewer _scrollViewer = null;
        private ContentPresenter _emptyContentPresenter;
        private bool _alreadyHookedScrollEvents = false;
        private DispatcherTimer _addToHeadTimer;
        private bool _refresh = false;

        public HorizontalEndlessScrollListBox()
        {
            DefaultStyleKey = typeof(HorizontalEndlessScrollListBox);
            Loaded += ListBox_Loaded;
        }

        public static readonly DependencyProperty ScrollAreaWidthProperty =
            DependencyProperty.Register("ScrollAreaWidth", typeof(int), typeof(HorizontalEndlessScrollListBox), new PropertyMetadata(0));

        public int ScrollAreaWidth
        {
            get { return (int)GetValue(ScrollAreaWidthProperty); }
            set { SetValue(ScrollAreaWidthProperty, value); }
        }

        public static readonly DependencyProperty EmptyContentProperty =
            DependencyProperty.Register("EmptyContent", typeof(object), typeof(HorizontalEndlessScrollListBox), null);
  
        public object EmptyContent 
        {
            get { return GetValue(EmptyContentProperty); }
            set { SetValue(EmptyContentProperty, value); }
        }
 
        public static readonly DependencyProperty EmptyContentTemplateProperty =
            DependencyProperty.Register("EmptyContentTemplate", typeof(DataTemplate), typeof(HorizontalEndlessScrollListBox), null);

        public DataTemplate EmptyContentTemplate
        {
            get { return (DataTemplate)GetValue(EmptyContentTemplateProperty); }
            set { SetValue(EmptyContentTemplateProperty, value); }
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

            _scrollViewer = (ScrollViewer)GetTemplateChild("ScrollViewer");
            _emptyContentPresenter = (ContentPresenter)GetTemplateChild("EmptyContentPresenter");
            if (_scrollViewer != null)
            {
                // Visual States are always on the first child of the control template 
                var element = VisualTreeHelper.GetChild(_scrollViewer, 0) as FrameworkElement;
                if (element != null)
                {
                    VisualStateGroup horizontalGroup = FindVisualState(element, HorizontalCompressionGroupName);
                    if (horizontalGroup != null)
                    {
                        horizontalGroup.CurrentStateChanging += HorizontalGroupCurrentStateChanging;
                    }
                }
            }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            ApplyTemplate();

            if (_scrollViewer != null)
            {
                var hasItems = Items.Count > 0;

                _emptyContentPresenter.Visibility = hasItems ? Visibility.Collapsed : Visibility.Visible;
                _scrollViewer.Visibility = hasItems ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void HorizontalGroupCurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState.Name == CompressionRightState)
            {
                _addToHeadTimer = new DispatcherTimer();
                _refresh = false;
                _addToHeadTimer.Interval = TimeSpan.FromSeconds(PullInterval);
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
