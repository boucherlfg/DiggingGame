using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : Targettable
{
    [SerializeField] private GameObject textIndicator;
    [SerializeField] private ParticleSystem destroyParticle;
    private float _attackTimer;
    public List<ResourceEnum> resourceName = new();
    public float currentDurability;
    public float durability;

    private void Start()
    {
        currentDurability = durability;
    }

    public override void Performed(PlayerInteraction player)
    {
        if (currentDurability <= 0) return;
        var inventory = ServiceManager.Instance.Get<Inventory>();
        var position = transform.position;
        position.z = -1;
        
        _attackTimer += Time.deltaTime;
        if (_attackTimer > player.attackInterval)
        {
            // destroy particle
            Instantiate(destroyParticle, position, Quaternion.identity);
            _attackTimer = 0;
        } 
        
        currentDurability -= Time.deltaTime * player.attackDamage;
        if (currentDurability > 0) return;
        
        var res = resourceName;
        inventory.AddRange(res);
        StartCoroutine(SpawnWords());
        return;
        
        IEnumerator SpawnWords()
        {
            // text label
            foreach (var r in res)
            {
                var text = Instantiate(textIndicator, position, Quaternion.identity);
                if (text.TryGetComponent<TMPro.TMP_Text>(out var label))
                {
                    label.text = r.ToString();
                }
                yield return new WaitForSeconds(0.25f);
            }
            Destroy(gameObject);
        } 
    }
    
    public override void Cancelled(PlayerInteraction player)
    {
        
    }
}