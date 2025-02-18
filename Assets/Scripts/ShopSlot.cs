using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Slot", menuName = "Shop Slot")]
public class ShopSlot : ScriptableObject
{
    [SerializeField] private List<ResourceEnum> input;
    [SerializeField] private List<ResourceEnum> output;
}