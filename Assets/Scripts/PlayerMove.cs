using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 8;
    private Rigidbody2D _rigidbody;


    private Camera _mainCamera;
 
    private void Start()
    {
        _mainCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        var move = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, _rigidbody.linearVelocity.y);
        _rigidbody.linearVelocity = move;
        Events.OnDepthChanged.Invoke(transform.position.y);
    }
}