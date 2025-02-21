using UnityEngine;

public class DeepDimmer : MonoBehaviour
{
    [SerializeField] int maxDeepness = 50;
    [SerializeField] SpriteRenderer spriteRenderer;
    // Update is called once per frame
    void Update()
    {
        var color = spriteRenderer.color;
        color.a = Mathf.Clamp01((maxDeepness + transform.position.y) / maxDeepness);
        spriteRenderer.color = color;
    }
}
