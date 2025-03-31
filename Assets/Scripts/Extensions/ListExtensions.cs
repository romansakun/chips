using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class ListExtensions
    {
        public static T GetAndRemove<T>(this List<T> list, int index)
        {
            if (list == null) 
                throw new ArgumentNullException(nameof(list));
            if (index < 0 || index >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var result = list[index];
            list.RemoveAt(index);
            return result;
        }

        public static void AddManyTimes<T>(this List<T> list, T value, int count)
        {
            if (list == null) 
                throw new ArgumentNullException(nameof(list));
            if (count < 1)
                throw new ArgumentException(nameof(count));

            for (var i = 0; i < count; i++)
            {
                list.Add(value);
            }
        }
    }
}