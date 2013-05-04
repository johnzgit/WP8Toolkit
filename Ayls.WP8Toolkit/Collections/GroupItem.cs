using System.Collections.ObjectModel;

namespace Ayls.WP8Toolkit.Collections
{
    public class GroupItem<T> : ObservableCollection<T>
    {
        public string Key { get; set; }

        public GroupItem()
        {
        }

        public GroupItem(string key) : this ()
        {
            Key = key;
        }
    }
}

