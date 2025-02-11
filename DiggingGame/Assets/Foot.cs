using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Foot : MonoBehaviour
{
    public UnityEvent onFloorDetected = new ();
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag($"Wall"))
        {
            onFloorDetected.Invoke();
        }
    }
}