using System;
using System.Linq;
using UnityEngine;

public class PlayerHurtScript : MonoBehaviour
{
    [SerializeField] private float hurtDistance = 2;
    [SerializeField] private LifeScript lifeScript;

    private void Update()
    {
        var results = new Collider2D[50];
        if (Physics2D.OverlapCircle(transform.position, hurtDistance, new ContactFilter2D().NoFilter(), results) <= 0) return;
        
        var closest = results.Where(x => x && x.TryGetComponent(out Hurtful _)).OrderBy(x => Vector2.Distance(transform.position, x.transform.position)).FirstOrDefault();
        if (!closest) return;
        
        closest.TryGetComponent<Hurtful>(out var hurtful);
        
        lifeScript.CurrentLife -= hurtful.HurtQuantity * Time.deltaTime;
    }
}