using UnityEngine;

public class Usable : MonoBehaviour
{
    public virtual void Use(PlayerInteraction player)
    {
        // empty
    }

    public virtual void StopUsing(PlayerInteraction player)
    {
        // empty
    }
}