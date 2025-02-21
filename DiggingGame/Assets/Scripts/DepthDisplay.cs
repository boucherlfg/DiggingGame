using System.Globalization;
using UnityEngine;

public class DepthDisplay : MonoBehaviour
{
    private TMPro.TMP_Text _label;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _label = GetComponent<TMPro.TMP_Text>();
        Events.OnDepthChanged.AddListener(OnDepthChanged);
    }

    private void OnDepthChanged(float arg0)
    {
        _label.text = ((int)arg0).ToString() + " m";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
