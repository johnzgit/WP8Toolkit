using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Ayls.WP8Toolkit.Linq
{
    /// <summary>
    /// Adapts a DependencyObject to provide methods required for generate
    /// a Linq To Tree API
    /// Code sourced from http://www.scottlogic.co.uk/blog/colin/2010/03/linq-to-visual-tree/
    /// </summary>
    public class VisualTreeAdapter : ILinqTree<DependencyObject>
    {
        private readonly DependencyObject _item;

        public VisualTreeAdapter(DependencyObject item)
        {
            _item = item;
        }

        public IEnumerable<DependencyObject> Children()
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(_item);
            for (int i = 0; i < childrenCount; i++)
            {
                yield return VisualTreeHelper.GetChild(_item, i);
            }
        }

        public DependencyObject Parent
        {
            get
            {
                return VisualTreeHelper.GetParent(_item);
            }
        }
    }
}