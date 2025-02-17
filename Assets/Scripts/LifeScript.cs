using System;
using UnityEngine;

public class LifeScript : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    
    [SerializeField] private float life;
    private float _currentCurrentLife;
    
    public float CurrentLife
    {
        get => _currentCurrentLife;
        set
        {
            _currentCurrentLife = value;
            Events.OnLifeChanged.Invoke(_currentCurrentLife/life);
        }
    }
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _currentCurrentLife = life;
    }

    private void Update()
    {
        if (_currentCurrentLife < 0)
        {
            Events.OnDeath.Invoke();
            Destroy(gameObject);
        }
    }

    public void ExplosionDamage(Vector2 explosionPosition, float explosionRange)
    {
        var explosionVector = explosionPosition - (Vector2)transform.position;
        var explosionDistance = Vector2.Distance(transform.position, explosionPosition);
        var damage = Extensions.ExplosionDamage(explosionDistance, life, explosionRange, 1.5f);
        
        _currentCurrentLife -= damage;
        Events.OnLifeChanged.Invoke(_currentCurrentLife/life);
        if (damage <= 0.01f) return;
        _rigidbody.linearVelocity -= explosionVector * 10;
    }
}