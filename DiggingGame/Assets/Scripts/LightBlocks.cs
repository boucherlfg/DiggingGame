using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightBlocks : MonoBehaviour
{
    public bool doOnUpdate = true;
    public int rayCount = 50;
    public int floodIterations = 2;
    public Material lightMaterial;
    public Material semiDarkMaterial;
    public Material darkMaterial;
    private readonly List<Transform> _oldList = new();

    private void OnDestroy()
    {
        _oldList.RemoveAll(x => !x);
        _oldList.ForEach(x => x.GetComponent<SpriteRenderer>().material = semiDarkMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        Extensions.LightAlgorithm(_oldList, transform, darkMaterial, semiDarkMaterial, lightMaterial, rayCount, floodIterations);
    }
}
