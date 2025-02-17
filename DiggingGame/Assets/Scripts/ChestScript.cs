using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestScript : ResourceScript
{
    [SerializeField] private int minNumber = 2;
    [SerializeField] private int maxNumber = 5;
    private void Awake()
    {
        var values = Enum.GetValues(typeof(ResourceEnum));
        var rand = Random.Range(2, 5);
        while(resourceName.Count < rand)
        {
            var random = Random.Range(0, values.Length);
            var en = (ResourceEnum)values.GetValue(random);
            resourceName.Add(en);
        }
    }
}