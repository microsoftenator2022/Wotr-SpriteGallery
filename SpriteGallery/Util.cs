using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteGallery.Util
{
    public static class Enumerable
    {
        public static IEnumerable<(int, T)> Indexed<T>(this IEnumerable<T> enumerable)
        {
            var i = 0;

            var enumerator = enumerable.GetEnumerator();

            while (enumerator.MoveNext())
                yield return (i++, enumerator.Current);
        }
    }
}
