using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            var itemKey = item.GetGroup();
            var group = list.FirstOrDefault(x => x.Key == itemKey);

            if (group == null)
            {
                group = new GroupItem<T>(itemKey);
                list.Add(group);
            }

            group.Add(item);
        }

        public void AddGroupedItem(T item)
        {
            AddGroupedItem(this, item);
        }

        public void RemoveGroupedItem(T item)
        {
            var itemKey = item.GetGroup();
            var group = this.FirstOrDefault(x => x.Key == itemKey);

            if (group != null)
            {
                var feed = group.FirstOrDefault(x => x.GetId() == item.GetId());
                if (feed != null)
                {
                    group.Remove(feed);
                }
            }
        }

        public void MoveGroupedItem(T oldItem, T newItem)
        {
            RemoveGroupedItem(oldItem);
            AddGroupedItem(newItem);
        }

        public IEnumerable<T> AllItems
        {
            get 
            {
                return this.SelectMany(x => x);
            }
        }
    }
}
