using System;
using UnityEngine;

public class FollowVelocity : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    private float _scaleX;
    private float _offset = 1;
    [SerializeField] private float maxOffset = 0.05f;
    [SerializeField] private float offsetSpeed = 2;
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var velocityX = rb.linearVelocityX;
        if (Mathf.Abs(velocityX) < 0.1f)
        {
            transform.localPosition = Vector3.zero;
            _offset = 1;
            return;
        }
        
        var scale = transform.localScale;
        scale.x = Mathf.Abs(velocityX)/velocityX * Mathf.Abs(scale.x);
        transform.localScale = scale;

        DoAnimation();
    }

    void DoAnimation()
    {
        _offset += Time.deltaTime;
        var pos = transform.localPosition;
        pos.y = Mathf.Sin(_offset * Mathf.PI * 2 * offsetSpeed) * maxOffset;
        transform.localPosition = pos;
    }
}
