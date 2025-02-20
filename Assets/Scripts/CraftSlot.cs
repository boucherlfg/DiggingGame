using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CraftSlot : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    public Sprite Sprite
    {
        get => image.sprite;
        set => image.sprite = value;
    }

    public event UnityAction OnClick
    {
        add => button.onClick.AddListener(value);
        remove => button.onClick.RemoveListener(value);
    }
}
