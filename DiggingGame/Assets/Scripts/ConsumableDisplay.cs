using UnityEngine;

public class ConsumableDisplay : MonoBehaviour
{
    [SerializeField]
    private HotkeySlot[] consumables;
    private int _currentConsumable;

    [SerializeField]
    private Color selectedColor;
    [SerializeField]
    private Color unselectedColor;
    
    private void Start()
    {
        foreach(var image in consumables) image.Color = unselectedColor;
        
        Events.OnConsumableChanged.AddListener(HandleConsumableChanged);
        Events.OnInventoryChanged.AddListener(HandleInventoryChanged);
        HandleConsumableChanged(0);
    }

    private void HandleInventoryChanged(Inventory inventory)
    {
        foreach (var resource in consumables)
        {
            var count = inventory.Count(resource.Resource);
            resource.Count = count;
        }
    }

    private void HandleConsumableChanged(int arg0)
    {
        if (_currentConsumable >= 0 && _currentConsumable < consumables.Length)
        {
            consumables[_currentConsumable].Color = unselectedColor;
        }

        _currentConsumable = arg0;

        if (_currentConsumable >= 0 && _currentConsumable < consumables.Length)
        {
            consumables[_currentConsumable].Color = selectedColor;
        }
    }
}