using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float attackInterval = 0.25f;
    public float attackDamage = 1;
    public float attackDistance = 2;
    public float attackRadius = 0.5f;

    [SerializeField] private int selectedConsumable;
    [SerializeField] private Transform hotkeys;
    private readonly List<Transform> _hotkeysList = new();
    
    private Inventory _inventory;
    private void Start()
    {
        _inventory = ServiceManager.Instance.Get<Inventory>();
        _inventory.Add(ResourceEnum.Pickaxe);
        
        foreach(Transform child in hotkeys) _hotkeysList.Add(child);
    }

    private void Update()
    {
        HandleConsumableSelection();
        
        HandlePassive();
        HandleUse();
        HandleConsume();
    }

    void HandlePassive()
    {
        var prefab = _hotkeysList[selectedConsumable];
        
        if (!prefab) return;
        if (!prefab.TryGetComponent<ResourceBased>(out var resource)) return;
        if (!_inventory.Has(resource.resource)) return;
        if (!prefab.TryGetComponent<Passive>(out var passive)) return;
        
        passive.Effect(this);
    }
    void HandleConsume()
    {
        if (!Input.GetButtonUp("Fire1")) return;

        var prefab = _hotkeysList[selectedConsumable];
        
        if (!prefab) return;
        if (!prefab.TryGetComponent<ResourceBased>(out var resource)) return;
        {
            if (!_inventory.Has(resource.resource)) return;
        }

        if (!prefab.TryGetComponent<ConsumableScript>(out var consumable)) return;
        
        if(resource) _inventory.Remove(resource.resource);
        consumable.Consume(this);
    }
    
    void HandleUse()
    {
        var prefab = _hotkeysList[selectedConsumable];
        
        if (!prefab) return;
        if (!prefab.TryGetComponent(out Usable usable)) return;
        if (!prefab.TryGetComponent(out ResourceBased resource)) return;
        if (!_inventory.Has(resource.resource)) return;
        
        if (Input.GetButton("Fire1"))
        {
            usable.Use(this);
        }
        else
        {
            usable.StopUsing(this);
        }
    }

    private void HandleConsumableSelection()
    {
        bool consumableChanged = false;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            consumableChanged = true;
            selectedConsumable = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            consumableChanged = true;
            selectedConsumable = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            consumableChanged = true;
            selectedConsumable = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            consumableChanged = true;
            selectedConsumable = 3;
        }
        
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            consumableChanged = true;
            selectedConsumable = 4;
        }

        if (consumableChanged)
        {
            Events.OnConsumableChanged.Invoke(selectedConsumable);
        }
    }
}