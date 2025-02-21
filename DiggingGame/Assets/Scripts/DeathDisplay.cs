using UnityEngine;

public class DeathDisplay : MonoBehaviour
{
    [SerializeField] private GameObject deathDisplay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Events.OnDeath.AddListener(DisplayDeath);
    }

    private void DisplayDeath()
    {
        Events.OnDeath.RemoveListener(DisplayDeath);
        deathDisplay.SetActive(true);
    }
}
