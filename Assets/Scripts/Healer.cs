using System;
using UnityEngine;

public class Healer : MonoBehaviour
{
    [SerializeField] private float healAmount = 50;
    private void Heal()
    {
        var life = FindAnyObjectByType<LifeScript>();
        life.CurrentLife = Mathf.Min(life.CurrentLife + healAmount, life.Life);
    }

    protected void Start()
    {
        Heal();
    }
}
