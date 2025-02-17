using System;
using UnityEngine;
using UnityEngine.Serialization;

public class FallingDamage : MonoBehaviour
{
        
    [SerializeField] private float minSpeed = 10;
    [SerializeField] private float maxSpeed = 100;
    [SerializeField] private float maxFallingDamage = 400;
    [SerializeField] private Foot foot;
    [SerializeField] private LifeScript lifeScript;
    
    private float _fallSpeed;
    private Rigidbody2D _rigidbody;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        foot.onFloorDetected.AddListener(OnFallingDamage);
    }

    
    private void OnFallingDamage()
    {
        var damage = Extensions.FallingDamage(_fallSpeed, maxFallingDamage, minSpeed, maxSpeed, 1.5f);

        lifeScript.CurrentLife -= damage;
        _rigidbody.linearVelocity += Vector2.up * _fallSpeed * 0.25f;
    }

    private void Update()
    {
        _fallSpeed = Mathf.Abs(_rigidbody.linearVelocity.y);
        if (_fallSpeed < minSpeed) _fallSpeed = 0;
    }
}