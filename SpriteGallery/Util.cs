﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static IEnumerable<T> OrderedIntersect<T>(this IEnumerable<T> sourceA, IEnumerable<T> sourceB)
        {
            sourceA = sourceA.Where(a => a != null);
            sourceB = sourceB.Where(b => b != null);

            return sourceA
                .Zip(sourceB)
                .SkipWhile(ab => !ab.First!.Equals(ab.Second))
                .TakeWhile(ab => ab.First!.Equals(ab.Second))
                .Select(ab => ab.First);
        }
    }

    public static class Utils
    {
        public static T Retry<T>(Func<T> func, int numRetries = 1, int delayMs = 0)
        {
            var retries = 0;
            
            T? result;

            while (true)
            {
                try
                {
                    result = func();
                    break;
                }
                catch (Exception e)
                {
                    if (retries > numRetries)
                    {
                        throw new Exception($"Failed after {retries} retries: {e}", e);
                    }

                    if(delayMs >= 0) Thread.Sleep(delayMs);
                }
                finally { retries++; }
            }

            if (retries > 0) Debug.Print($"Retries: {retries}");

            return result;
        }

        public static void Retry(Action action, int numRetries = 1, int delayMs = 0) =>
            Retry<object?>(() => { action(); return null; }, numRetries, delayMs);
    }
}
