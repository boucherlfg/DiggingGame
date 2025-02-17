using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    public GameObject explosionParticle;
    public float delayBeforeExplosion = 3;

    public float explosionRange = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(delayBeforeExplosion);
        Explode();
    }

    private void Explode()
    {
        var results = new Collider2D[300];
        var size = Physics2D.OverlapCircle(transform.position, explosionRange, new ContactFilter2D().NoFilter(), results);
        var breakables = results.Take(size).Where(x => x && x.GetComponent<Breakable>());
        foreach(var breakable in breakables) Destroy(breakable.gameObject);
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
        results = Array.FindAll(results, x => x);
        var col = results.FirstOrDefault(x => x.GetComponent<LifeScript>());
        if (!col) return;
        
        col.TryGetComponent(out LifeScript player);
        player.ExplosionDamage(transform.position, explosionRange);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
