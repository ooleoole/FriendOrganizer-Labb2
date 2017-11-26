using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FriendOrganizer.UI.Extensions
{
    public static class IEnumerableExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> data)
        {
            var temp = new ObservableCollection<T>();
            foreach (var value in data)
                temp.Add(value);
            
            return temp;
        }
    }
}
