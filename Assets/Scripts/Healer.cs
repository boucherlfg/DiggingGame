using System;
using UnityEngine;

public class Healer : MonoBehaviour
{
    [SerializeField] private bool healOnStart = false;
    [SerializeField] private float healAmount = 50;
    public void Heal()
    {
        var life = FindAnyObjectByType<LifeScript>();
        life.CurrentLife = Mathf.Min(life.CurrentLife + healAmount, life.Life);
    }

    private void Start()
    {
        if (!healOnStart) return;
        Heal();
        Destroy(gameObject);
    }
}
