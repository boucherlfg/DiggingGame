using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Enumerable = System.Linq.Enumerable;

public static class Extensions
{
    public static void LightAlgorithm(List<Transform> oldList, Transform center, Material dark, Material semi, Material light, int rayCount = 50, int floodIterations = 2)
    {
        oldList.RemoveAll(x => !x);
        var newList = new List<Transform>();
        var semiList = new List<Transform>();
        for (var i = 0; i < rayCount; i++)
        {
            var angle = i * Mathf.PI * 2 / rayCount;
            var direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            var hits = new RaycastHit2D[5];
            if(Physics2D.Raycast(center.position, direction, new ContactFilter2D().NoFilter(), hits) <= 1) continue;
            
            var first = hits.Where(x => x &&x.transform != center && x.collider.CompareTag("Wall"))
                .OrderBy(x => Vector2.Distance(center.position, x.transform.position)).FirstOrDefault();
            
            if (!first) continue;
            if (newList.Contains(first.transform)) continue;
            
            newList.Add(first.transform);
            Flood(floodIterations, first.transform);
        }

        foreach (var oldItem in oldList)
        {
            if (!oldItem.TryGetComponent(out SpriteRenderer rend)) return;
            rend.material = semi;
        }
        foreach (var semiItem in semiList)
        {
            if (!semiItem.TryGetComponent(out SpriteRenderer rend)) return;
            rend.material = semi;
        }
        foreach (var newItem in newList)
        {
            if (!newItem.TryGetComponent(out SpriteRenderer rend)) return;
            rend.material = light;
        }
        
        oldList = newList;
        oldList.AddRange(semiList);
        return;
        
        void Flood(int iterationsLeft, Transform first)
        {
            if (iterationsLeft <= 0) return;
            for (var j = 0; j < 4; j++)
            {
                var angle2 = j * Mathf.PI / 2;
                var direction2 = new Vector3(Mathf.Cos(angle2), Mathf.Sin(angle2), 0f);
                var col = Physics2D.OverlapPoint(first.transform.position + direction2);
                if (!col || !col.CompareTag("Wall")) continue;
                if (col.TryGetComponent<LightBlocks>(out _)) continue;
                if(newList.Contains(col.transform)) continue;
            
                semiList.Add(col.transform);
                Flood(iterationsLeft - 1, col.transform);
            }
        }
    }
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