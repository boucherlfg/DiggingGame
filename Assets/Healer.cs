using UnityEngine;

public class Healer : MonoBehaviour
{
    [SerializeField] private float healAmount = 50;
    public void Heal()
    {
        var life = FindAnyObjectByType<LifeScript>();
        life.CurrentLife = Mathf.Min(life.CurrentLife + healAmount, life.Life);
    }
}
