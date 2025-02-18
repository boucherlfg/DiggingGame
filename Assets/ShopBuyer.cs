using UnityEngine;

public class ShopBuyer : MonoBehaviour
{
    public Shop shop;
    public ResourceEnum resource;

    public void Buy()
    {
        shop.BuyStuff(resource);
    }
}
