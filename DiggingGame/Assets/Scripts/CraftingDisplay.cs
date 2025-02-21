using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingDisplay : MonoBehaviour
{
    private Inventory _inventory;
    [SerializeField] private GameObject textMessage;
    [SerializeField] private GameObject craftingSlot;
    [SerializeField] private Transform container;
    [SerializeField] private List<Recipe> recipes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inventory = ServiceManager.Instance.Get<Inventory>();
        Events.OnInventoryChanged.AddListener(OnInventoryChanged);
    }

    private void OnInventoryChanged(Inventory inventory)
    {
        foreach(Transform child in container) Destroy(child.gameObject);
        
        foreach (var recipe in recipes)
        {
            if (!inventory.Has(recipe.Input)) return;
            
            var shopButtonObj = Instantiate(craftingSlot, container);
            var craftSlot = shopButtonObj.GetComponentInChildren<CraftSlot>();
            craftSlot.Sprite = recipe.Sprite;
            craftSlot.OnClick += () => Craft(recipe);
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
                var text = Instantiate(textMessage, Vector3.zero, Quaternion.identity);
                text.GetComponent<TMP_Text>().text = resource.ToString();
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
