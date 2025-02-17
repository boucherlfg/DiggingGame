
using UnityEngine.Events;

public abstract class Events
{
    public static readonly UnityEvent<float> OnEnergyChanged = new();
    public static readonly UnityEvent<float> OnLifeChanged = new();
    public static readonly UnityEvent<float> OnDepthChanged = new();
    public static readonly UnityEvent<int> OnConsumableChanged = new();
}