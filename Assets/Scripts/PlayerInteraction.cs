using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private GameObject textIndicator;
    [SerializeField] private ParticleSystem destroyParticle;
    
    [SerializeField] public float attackInterval = 0.25f;
    [SerializeField] private float attackDamage = 1;
    [SerializeField] public float attackDistance = 2;
    [SerializeField] private float attackRadius = 0.5f;

    [SerializeField] private int selectedConsumable;
    [SerializeField]
    private List<GameObject> consumablePrefab = new();
    
    private float _attackTimer;
    private GameObject _targetted;
    private Inventory _inventory;

    private Camera _mainCamera;
    private void Start()
    {
        _mainCamera = Camera.main;
        _inventory = ServiceManager.Instance.Get<Inventory>();
        _inventory.Add(ResourceEnum.Pickaxe);
    }

    private void Update()
    {
        HandleConsumableSelection();

        if (selectedConsumable < 1)
        {
            HandleInteract();
        }
        else
        {
            HandleConsumableUse();
        }
    }

    void HandleConsumableUse()
    {
        if (!Input.GetButtonUp("Fire1")) return;

        Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Physics2D.OverlapPoint(mousePos)) return;
        
        var consumable = consumablePrefab[selectedConsumable - 1];
        if (!consumable) return;
        
        var resource = consumable.GetComponent<ResourceScript>();
        if (!_inventory.Has(resource.resourceName)) return;
        foreach(var res in resource.resourceName) _inventory.Remove(res);
        
        Instantiate(consumable, mousePos, Quaternion.identity);
    }
    
    void HandleInteract()
    {
        var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var direction = mousePos - transform.position;
        var walls = new RaycastHit2D[5];
        var size = Physics2D.Raycast(transform.position, 
            direction.normalized, 
            new ContactFilter2D().NoFilter(), 
            walls, 
            attackDistance);
        
        if (size <= 0) return;
        
        var closestWall = walls.Where(x => x && x.transform.CompareTag("Wall"))
            .OrderBy(x => Vector2.Distance(transform.position, x.transform.position))
            .FirstOrDefault();
        
        if (closestWall && closestWall.transform.TryGetComponent(out ResourceScript resourceScript))
        {
            Targetted = closestWall.transform.gameObject;
        }
        else if (closestWall)
        {
            var point = closestWall.point;
            var components = closestWall.transform.GetComponentsInChildren<ResourceScript>();
            if (components.Length <= 0) return;

            resourceScript = components.OrderBy(x => Vector2.Distance(x.transform.position, point)).FirstOrDefault();
            if (!resourceScript) return;

            Targetted = resourceScript.gameObject;
        }
        else return;
        
        if (!Input.GetButton("Fire1")) return;
        
        var position = Targetted.transform.position;
        position.z = -1;
        
        _attackTimer += Time.deltaTime;
        if (_attackTimer > attackInterval)
        {
            // destroy particle
            Instantiate(destroyParticle, position, Quaternion.identity);
            _attackTimer = 0;
        } 
        
        resourceScript.currentDurability -= Time.deltaTime * attackDamage;
        if (resourceScript.currentDurability > 0) return;
        
        
        var res = resourceScript.resourceName;
        _inventory.AddRange(res);
        Destroy(Targetted);

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
        } 
    }

    private void HandleConsumableSelection()
    {
        bool consumableChanged = false;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            consumableChanged = true;
            selectedConsumable = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            consumableChanged = true;
            selectedConsumable = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            consumableChanged = true;
            selectedConsumable = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            consumableChanged = true;
            selectedConsumable = 3;
        }
        
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            consumableChanged = true;
            selectedConsumable = 4;
        }

        if (consumableChanged)
        {
            Events.OnConsumableChanged.Invoke(selectedConsumable);
        }
    }
    
    private GameObject Targetted
    {
        get => _targetted;
        set
        {
            if (_targetted == value) return;
            if (_targetted && _targetted.transform.childCount > 0)
            {
                if (!_targetted.TryGetComponent<ResourceScript>(out var resourceScript)) return;
                if (!resourceScript.selectable) return;
                _targetted.transform.GetChild(0).gameObject.SetActive(false);
            }
            
            _targetted = value;
            
            if (!_targetted) return;
            
            if (_targetted.transform.childCount > 0)
            {
                if (!_targetted.TryGetComponent<ResourceScript>(out var resourceScript)) return;
                if (!resourceScript.selectable) return;
                _targetted.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}