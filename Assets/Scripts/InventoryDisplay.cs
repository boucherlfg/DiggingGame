using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private GameObject inventorySlot;
    [SerializeField] private Transform container;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Events.OnInventoryChanged.AddListener(Refresh);
    }

    private void Refresh(Inventory inventory)
    {
        foreach(Transform child in container) Destroy(child.gameObject);
        foreach (var item in inventory.Distinct())
        {
            var count = inventory.Count(item);
            var instance = Instantiate(inventorySlot, container);
            instance.GetComponent<TMPro.TMP_Text>().text = $"{count} x {item}";
        }
    }

}
