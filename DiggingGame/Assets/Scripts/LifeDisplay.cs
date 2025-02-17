using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class LifeDisplay : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        private void Start()
        {
            Events.OnLifeChanged.AddListener(OnValueChanged);
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
}