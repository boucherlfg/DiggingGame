using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightBlocks : MonoBehaviour
{
    public int rayCount = 50;
    public Material lightMaterial;
    public Material semiDarkMaterial;
    public Material darkMaterial;
    private List<GameObject> oldList = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        oldList.RemoveAll(x => !x);
        oldList.ForEach(x => x.GetComponent<SpriteRenderer>().material = semiDarkMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        oldList.RemoveAll(x => !x);
        var newList = new List<GameObject>();
        var semiList = new List<GameObject>();
        for (int i = 0; i < rayCount; i++)
        {
            var angle = i * Mathf.PI * 2 / rayCount;
            var direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            var hits = new RaycastHit2D[5];
            if(Physics2D.Raycast(transform.position, direction, new ContactFilter2D().NoFilter(), hits) <= 1) continue;
            
            var first = hits.Where(x => x &&x.transform != transform && x.collider.CompareTag("Wall")).OrderBy(x => Vector2.Distance(transform.position, x.transform.position)).FirstOrDefault();
            
            if (!first) continue;
            if (newList.Contains(first.collider.gameObject)) continue;
            
            newList.Add(first.collider.gameObject);
            for (int j = 0; j < 4; j++)
            {
                var angle2 = j * Mathf.PI / 2;
                var direction2 = new Vector3(Mathf.Cos(angle2), Mathf.Sin(angle2), 0f);
                var col = Physics2D.OverlapPoint(first.transform.position + direction2);
                if (!col || !col.CompareTag("Wall")) continue;
                if (col.TryGetComponent<LightBlocks>(out _)) continue;
                if(newList.Contains(col.gameObject)) continue;
                
                semiList.Add(col.gameObject);
            }
        }

        oldList.ForEach(x => x.GetComponent<SpriteRenderer>().material = semiDarkMaterial);
        semiList.ForEach(x => x.GetComponent<SpriteRenderer>().material = semiDarkMaterial);
        newList.ForEach(x => x.GetComponent<SpriteRenderer>().material = lightMaterial);
        oldList = newList;
        oldList.AddRange(semiList);
    }
}
