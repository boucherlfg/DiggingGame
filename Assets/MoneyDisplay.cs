using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    private TMPro.TMP_Text _label;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _label = GetComponent<TMPro.TMP_Text>();
        Events.OnInventoryChanged.AddListener(OnInventoryChanged);
        _label.text = "0 $";
    }

    private void OnInventoryChanged(Inventory arg0)
    {
        var money = arg0.Money;
        _label.text = money + " $";
    }
}
