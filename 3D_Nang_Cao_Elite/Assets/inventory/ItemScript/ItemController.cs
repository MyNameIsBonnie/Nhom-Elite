using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public Item item;

    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);
        Destroy(gameObject);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }
    public void UseItem()
    {
        /*switch (item.itemType)
        {
            case Item.ItemType.HpPotion:
                PlayerMovementVerTwo.Instance.InstantHealPotion();
                break;
            case Item.ItemType.RegenPotion:
                PlayerMovementVerTwo.Instance.RegenPotion();
                break;
        }
        RemoveItem();*/
        if (item == null)
        {
            Debug.LogError("UseItem() - item is NULL!");
            return;
        }

        switch (item.itemType)
        {
            case Item.ItemType.HpPotion:
                PlayerMovementVerTwo.Instance?.InstantHealPotion();
                break;
            case Item.ItemType.RegenPotion:
                PlayerMovementVerTwo.Instance?.RegenPotion();
                break;
        }

        RemoveItem();
    }
}
