using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour
{
    public bool selectable = true;
    public List<ResourceEnum> resourceName = new();
    public float currentDurability;
    public float durability;

    private void Start()
    {
        currentDurability = durability;
    }
}