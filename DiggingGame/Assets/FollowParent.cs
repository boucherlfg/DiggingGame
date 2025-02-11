using UnityEngine;

public class FollowParent : MonoBehaviour
{
    [SerializeField]private float followSpeed = 10;
    private Transform _target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _target = transform.parent;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, _target.position, followSpeed * Time.deltaTime);
    }
}
