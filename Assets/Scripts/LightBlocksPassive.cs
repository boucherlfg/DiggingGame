using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightBlocksPassive : Passive
{   
    public int rayCount = 50;
    public int floodIterations = 2;
    public Material lightMaterial;
    public Material semiDarkMaterial;
    public Material darkMaterial;
    private readonly List<Transform> _oldList = new ();
    public override void Effect(PlayerInteraction playerInteraction)
    {
        Extensions.LightAlgorithm(_oldList, playerInteraction.transform, darkMaterial, semiDarkMaterial, lightMaterial, rayCount, floodIterations);
    }
    
    
}