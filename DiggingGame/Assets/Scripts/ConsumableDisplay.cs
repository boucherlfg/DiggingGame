using System;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableDisplay : MonoBehaviour
{
    [SerializeField]
    private Image[] consumables;
    private int _currentConsumable = 0;

    [SerializeField]
    private Color selectedColor;
    [SerializeField]
    private Color unselectedColor;
    private void Start()
    {
        foreach(var image in consumables) image.color = unselectedColor;
        
        Events.OnConsumableChanged.AddListener(HandleConsumableChanged);
        HandleConsumableChanged(0);
    }

    private void HandleConsumableChanged(int arg0)
    {
        consumables[_currentConsumable].color = unselectedColor;
        _currentConsumable = arg0;
        consumables[_currentConsumable].color = selectedColor;

    }
}