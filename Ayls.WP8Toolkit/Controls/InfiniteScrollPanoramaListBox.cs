using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Ayls.WP8Toolkit.Linq;

namespace Ayls.WP8Toolkit.Controls
{
    public class InfiniteScrollPanoramaListBox : ListBox
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

        public InfiniteScrollPanoramaListBox()
        {
            DefaultStyleKey = typeof(InfiniteScrollPanoramaListBox);
            Loaded += ListBox_Loaded;
        }

        public static readonly DependencyProperty ScrollAreaWidthProperty =
            DependencyProperty.Register("ScrollAreaWidth", typeof(int), typeof(InfiniteScrollPanoramaListBox), new PropertyMetadata(0));

        public int ScrollAreaWidth
        {
            get { return (int)GetValue(ScrollAreaWidthProperty); }
            set { SetValue(ScrollAreaWidthProperty, value); }
        }

        public static readonly DependencyProperty TitleContentProperty =
            DependencyProperty.Register("TitleContent", typeof(object), typeof(InfiniteScrollPanoramaListBox), null);

        public object TitleContent
        {
            get { return GetValue(TitleContentProperty); }
            set { SetValue(TitleContentProperty, value); }
        }

        public static readonly DependencyProperty LoadNextIndicatorContentProperty =
            DependencyProperty.Register("LoadNextIndicatorContent", typeof(object), typeof(InfiniteScrollPanoramaListBox), null);

        public object LoadNextIndicatorContent 
        {
            get { return GetValue(LoadNextIndicatorContentProperty); }
            set { SetValue(LoadNextIndicatorContentProperty, value); }
        }

        public static readonly DependencyProperty EmptyContentProperty =
            DependencyProperty.Register("EmptyContent", typeof(object), typeof(InfiniteScrollPanoramaListBox), null);

        public object EmptyContent
        {
            get { return GetValue(EmptyContentProperty); }
            set { SetValue(EmptyContentProperty, value); }
        }

        public static readonly DependencyProperty LoadNextCommandProperty =
            DependencyProperty.Register("LoadNextCommand", typeof(ICommand), typeof(InfiniteScrollPanoramaListBox), null);

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
                HookupScrollCompressionEvent(_scrollViewer);
                SetLoadNextIndicatorContent(_scrollViewer);
            }
        }

        private void HookupScrollCompressionEvent(ScrollViewer scrollViewer)
        {
            var element = VisualTreeHelper.GetChild(scrollViewer, 0) as FrameworkElement;
            if (element != null)
            {
                VisualStateGroup horizontalGroup = FindVisualState(element, HorizontalCompressionGroupName);
                if (horizontalGroup != null)
                {
                    horizontalGroup.CurrentStateChanging += HorizontalGroupCurrentStateChanging;
                }
            }
        }

        private void SetLoadNextIndicatorContent(ScrollViewer scrollViewer)
        {
            var loadNextIndicator = scrollViewer.Descendants()
                                                .OfType<ContentControl>()
                                                .SingleOrDefault(s => s.Name == "LoadNextIndicator");

            if (loadNextIndicator != null)
            {
                loadNextIndicator.Content = LoadNextIndicatorContent;
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
