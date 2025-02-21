using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    public Sprite Sprite => sprite;
    public List<ResourceEnum> Input => input;
    public List<ResourceEnum> Output => output;
    
    [SerializeField] private Sprite sprite;
    [SerializeField] private List<ResourceEnum> input;
    [SerializeField] private List<ResourceEnum> output;
}