using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Ayls.WP8Toolkit.Collections
{
    public class GroupCollection<T> : ObservableCollection<GroupItem<T>> where T : IGroupable
    {
        public static GroupCollection<T> CreateGroups(IEnumerable<T> items, IComparer<string> comparer)
        {
            var list = new GroupCollection<T>() {GroupComparer = comparer};

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

                var groupIndex = GetIndexToInsertGroupAt(list, group);
                if (groupIndex < list.Count)
                {
                    list.Insert(groupIndex, group);  
                }
                else
                {
                    list.Add(group);
                }
            }

            var itemIndex = GetIndexToInsertGroupItemAt(group, item);
            if (itemIndex < group.Count)
            {
                group.Insert(itemIndex, item);
            }
            else
            {
                group.Add(item);   
            }
        }

        private static int GetIndexToInsertGroupAt(GroupCollection<T> list, GroupItem<T> newGroup)
        {
            var tempList = new GroupCollection<T>();
            foreach (var g in list)
            {
                tempList.Add(g);
            }
            tempList.Add(newGroup);

            var tempSortedList = tempList.OrderBy(x => x.Key, list.GroupComparer).ToList();

            return tempSortedList.IndexOf(newGroup);
        }

        private static int GetIndexToInsertGroupItemAt(GroupItem<T> group, T newItem)
        {
            var tempGroup = new GroupItem<T>();
            foreach (var item in group)
            {
                tempGroup.Add(item);
            }
            tempGroup.Add(newItem);

            var tempSortedList = tempGroup.OrderBy(x => x.Title).ToList();

            return tempSortedList.IndexOf(newItem);
        }

        public IComparer<string> GroupComparer { get; protected set; }

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
