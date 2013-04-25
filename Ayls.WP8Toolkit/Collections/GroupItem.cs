using System.Collections.ObjectModel;

namespace Ayls.WP8Toolkit.Collections
{
    public class GroupItem<T> : ObservableCollection<T>
    {
        public delegate string GetKeyDelegate(T item);

        public string Key { get; private set; }

        public GroupItem(string key)
        {
            Key = key;
        }
    }
}

