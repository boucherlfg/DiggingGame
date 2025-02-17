using System;
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

    public static float ExplosionDamage(float actualDistance, float maxDamage, float maxDistance, float coefficient = 2)
    {
        if (actualDistance > maxDistance) return 0;
        return maxDamage * Mathf.Pow(maxDistance - actualDistance, coefficient)/Mathf.Pow(maxDistance, coefficient);
    }

    public static float FallingDamage(float actualSpeed, float maxDamage, float minSpeed, float maxSpeed, float coefficient = 2)
    {
        if (actualSpeed < minSpeed) return 0;
        if(actualSpeed > maxSpeed) return maxDamage;
        return maxDamage * Mathf.Pow(actualSpeed - minSpeed, coefficient)/Mathf.Pow(maxSpeed, coefficient);
    }
}