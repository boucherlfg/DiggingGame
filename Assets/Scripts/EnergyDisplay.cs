using UnityEngine;
using UnityEngine.UI;

public class EnergyDisplay : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private void Start()
    {
        Events.OnEnergyChanged.AddListener(OnValueChanged);
    }

    private void Update()
    {
        slider.gameObject.SetActive(slider.value < 0.99f);
    }

    private void OnValueChanged(float energy)
    {
        slider.value = energy;
    }
}