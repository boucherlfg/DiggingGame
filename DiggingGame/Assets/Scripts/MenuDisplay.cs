using System;
using UnityEngine;

public class MenuDisplay : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject crafting;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        UIEvents.MenuRequested.AddListener(OnMenuRequested);
    }

    private void OnMenuRequested(MenuType menuType)
    {
        inventory.SetActive(false);
        crafting.SetActive(false);

        switch (menuType)
        {
            case MenuType.Crafting:
                crafting.SetActive(true);
                break;
            case MenuType.Inventory:
                inventory.SetActive(true);
                break;
            case MenuType.Pause:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(menuType), menuType, null);
        }
    }

    public void CloseMenu()
    {
        inventory.SetActive(false);
        crafting.SetActive(false);
    }
}
