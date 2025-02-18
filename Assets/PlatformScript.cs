using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    private Collider2D _collider2D;

    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
    }
    
    // Update is called once per frame
    private void Update()
    {
        _collider2D.enabled = !(Input.GetAxisRaw("Vertical") < -1e-5);
    }
}
