using UnityEngine;

public class Targettable : MonoBehaviour
{
    public bool Selectable => transform.childCount > 0 && transform.GetChild(0).TryGetComponent<SpriteRenderer>(out _);

    public virtual void Performed(PlayerInteraction player)
    {
        // empty
    }

    public virtual void Cancelled(PlayerInteraction player)
    {
        // empty
    }
}