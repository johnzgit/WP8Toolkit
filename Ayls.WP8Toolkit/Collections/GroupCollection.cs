using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Ayls.WP8Toolkit.Collections
{
    public class GroupCollection<T> : ObservableCollection<GroupItem<T>> where T : IGroupable
    {
        public static GroupCollection<T> CreateGroups(IEnumerable<T> items, CultureInfo ci)
        {
            var list = new GroupCollection<T>();

            foreach (T item in items)
            {
                AddGroupedItem(list, item);
            }

            return list;
        }

        private static void AddGroupedItem(GroupCollection<T> list, T item)
        {
            var itemKey = item.Group;
            var group = list.FirstOrDefault(x => x.Key == itemKey);

            if (group == null)
            {
                group = new GroupItem<T>(itemKey);
                list.Add(group);
            }

            group.Add(item);
        }

        public ObservableCollection<string> Groups
        {
            get { return new ObservableCollection<string>(this.Select(x => x.Key).Distinct()); }
        }

        public bool HasItems
        {
            get { return AllItems.Any(); }
        }

        public IEnumerable<T> AllItems
        {
            get 
            {
                return this.SelectMany(x => x);
            }
        }

        public void AddGroupedItem(T item)
        {
            AddGroupedItem(this, item);
            NotifyPropertyChanged("HasItems");
        }

        public void RemoveGroupedItem(T item)
        {
            var itemKey = item.Group;
            var group = this.FirstOrDefault(x => x.Key == itemKey);

            if (group != null)
            {
                var existingItem = group.FirstOrDefault(x => x.Id == item.Id);
                if (existingItem != null)
                {
                    group.Remove(existingItem);
                }

                NotifyPropertyChanged("HasItems");
            }
            CleanupGroups();
        }

        private void CleanupGroups()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                var group = this[i];
                if (!group.Any())
                {
                    this.Remove(group);
                }
            }
        }

        public void MoveGroupedItem(T oldItem, T newItem)
        {
            RemoveGroupedItem(oldItem);
            AddGroupedItem(newItem);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
