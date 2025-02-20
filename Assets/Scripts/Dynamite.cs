using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dynamite : MonoBehaviour
{
    public GameObject explosionParticle;
    public float delayBeforeExplosion = 3;

    public float explosionRange = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        GetComponent<Rigidbody2D>().angularVelocity += Random.Range(-50, 50);
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(delayBeforeExplosion);
        
        var results = new Collider2D[300];
        var size = Physics2D.OverlapCircle(transform.position, explosionRange, new ContactFilter2D().NoFilter(), results);
        var breakables = results.Take(size).Where(x => x && x.TryGetComponent<Breakable>(out _));
        foreach(var breakable in breakables) Destroy(breakable.gameObject);
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
        results = Array.FindAll(results, x => x);
        var col = results.FirstOrDefault(x => x.TryGetComponent<LifeScript>(out _));
        if (!col) yield break;
        
        col.TryGetComponent(out LifeScript player);
        player.ExplosionDamage(transform.position, explosionRange);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
