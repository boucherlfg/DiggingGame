using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject textMessage;
    [Serializable]
    public struct Price
    {
        public ResourceEnum resource;
        public int price;
    }
    [SerializeField] private Price[] prices;
    [SerializeField] private Price[] offers;
    [SerializeField] private RectTransform shop;

    [SerializeField]
    private Vector2 shopPosition;

    private Camera _mainCamera;
    private Inventory _inventory;
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere((Vector2)transform.position + shopPosition, 0.5f);
    }

    private void Start()
    {
        _inventory = ServiceManager.Instance.Get<Inventory>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        var screenPos = _mainCamera.WorldToScreenPoint((Vector2)transform.position + shopPosition);
        shop.position = screenPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        shop.gameObject.SetActive(true);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        shop.gameObject.SetActive(false);
    }

    public void SellStuff()
    {
        var total = 0;
        foreach (var item in prices)
        {
            var count = _inventory.Count(item.resource);
            total += count;
            _inventory.Money += item.price * count;
            _inventory.RemoveAll(item.resource);
        }
        
        Instantiate(textMessage, shopPosition, Quaternion.identity).GetComponent<TMP_Text>().text = $"{total} $";
        
    }

    public void BuyStuff(ResourceEnum resource)
    {
        if (offers.All(x => x.resource != resource)) return;
        var offer = offers.FirstOrDefault(x => x.resource == resource);

        if (_inventory.Money < offer.price)
        {
            Instantiate(textMessage, shopPosition, Quaternion.identity).GetComponent<TMP_Text>().text = "Not enough money";
            return;
        }
        
        _inventory.Money -= offer.price;
        _inventory.Add(offer.resource);
    }
}
