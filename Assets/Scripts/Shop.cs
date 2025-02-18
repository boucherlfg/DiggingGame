using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject textMessage;
    [SerializeField] private RectTransform shop;
    [SerializeField] private GameObject shopButton;
    [SerializeField] private Recipe[] recipes;
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
        Events.OnInventoryChanged.AddListener(Refresh);
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

    private void Refresh(Inventory inventory)
    {
        foreach (Transform child in shop)
        {
            Destroy(child.gameObject);
        }

        foreach (var recipe in recipes)
        {
            if (!inventory.Has(recipe.Input)) return;
            
            var shopButtonObj = Instantiate(shopButton, shop);
            var button = shopButtonObj.GetComponent<Button>();
            button.onClick.AddListener(() => Craft(recipe));
            
            var image = shopButtonObj.GetComponent<Image>();
            image.sprite = recipe.Sprite;
        }
    }

    private void Craft(Recipe recipe)
    {
        _inventory.RemoveAll(recipe.Input);
        _inventory.AddRange(recipe.Output);
        StartCoroutine(ShowText(recipe.Output));
        return;

        IEnumerator ShowText(List<ResourceEnum> resources)
        {
            foreach (var resource in resources)
            {
                var text = Instantiate(textMessage, shop.transform.position, Quaternion.identity);
                text.GetComponent<TMP_Text>().text = resource.ToString();
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
