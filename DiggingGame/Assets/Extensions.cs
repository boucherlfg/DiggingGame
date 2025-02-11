using UnityEngine;
using System.Collections.Generic;
using Enumerable = System.Linq.Enumerable;

public static class Extensions
{
    public static T Random<T>(this IEnumerable<T> list)
    {
        var enumerable = list as T[] ?? Enumerable.ToArray(list);
        return enumerable[UnityEngine.Random.Range(0, enumerable.Length)];
    }
    public static T Random<T>(this T[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }

    public static int[] GetNeighbours(this int value, int width, int height)
    {
        var x = value / height;
        var y = value % height;
        var neighbours = new List<int>();
        for (var i = -1; i <= 1; i++)
        {
            for (var j = -1; j <= 1; j++)
            {
                if (i * i + j * j != 1) continue;
                var u = x + i;
                var v = y + j;
                if (u < 0 || u >= width || v < 0 || v >= height) continue;
                
                neighbours.Add(u * height + v);
            }
        }
        return neighbours.ToArray();
    }
}