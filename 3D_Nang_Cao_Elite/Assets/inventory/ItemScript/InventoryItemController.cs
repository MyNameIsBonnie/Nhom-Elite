using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
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
        switch (item.itemType)
        {
            case Item.ItemType.HpPotion:
                PlayerMovementVerTwo.Instance.InstantHealPotion();
                break;
            case Item.ItemType.RegenPotion:
                PlayerMovementVerTwo.Instance.RegenPotion();
                break;
        }
        RemoveItem();
    }
}
