using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemPickUp : MonoBehaviour
{
    public Item Item;

    /*void Pickup()
    {
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
    }*/
    void Pickup()
    {
        if (Item == null)
        {
            Debug.LogError("ItemPickUp: Item is NULL!");
            return;
        }

        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        Pickup();
    }

    
}
