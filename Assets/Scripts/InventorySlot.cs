using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int Count
    {
        get => int.Parse(text.text);
        set => text.text = value.ToString();
    }

    public Color Color
    {
        get => image.color;
        set => image.color = value;
    }

    public ResourceEnum Resource => resource;
    
    [SerializeField] private ResourceEnum resource;
    [SerializeField] private Image image;
    [SerializeField] private TMPro.TMP_Text text;
}