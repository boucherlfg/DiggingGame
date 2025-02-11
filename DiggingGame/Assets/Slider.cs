using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnergyDisplay : MonoBehaviour
{
    private void Start()
    {
        Events.OnEnergyChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(float energy)
    {
        GetComponent<Slider>().value = energy;
    }
}