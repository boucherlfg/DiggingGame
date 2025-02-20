using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        var player = FindAnyObjectByType<PlayerMove>();
        player.transform.position = new Vector2(Random.Range(-20, 20), 1);
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        Destroy(gameObject);
    }
}
