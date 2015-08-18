using BigEgg.Framework;
using System;
using System.Collections.Generic;

namespace Bugger.PlugIns.TrackingSystems.Fake.Utils
{
    public static class RandomNumberHelper
    {
        private static readonly Random random = new Random(DateTime.Now.Millisecond);

        public static bool IsLucky(int percentage)
        {
            Preconditions.Check(() => percentage < 0);
            Preconditions.Check(() => percentage > 100);

            return random.Next(100 / percentage) == 0;
        }

        public static T GetLuckyEnum<T>(Type enumType)
        {
            var values = Enum.GetValues(enumType);
            return (T)values.GetValue(random.Next(100 / values.Length));
        }

        public static T GetLuckyItem<T>(IList<T> items)
        {
            return items[random.Next(items.Count)];
        }
    }
}
