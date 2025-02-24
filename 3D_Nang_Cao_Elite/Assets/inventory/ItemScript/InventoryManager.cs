using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<Item> Items = new List<Item>();

    public Transform ItemContent;

    public GameObject InventoryItem;

    public ItemController[] ItemsController;

    private void Awake()
    {
        Instance = this;
    }

    public void Add(Item item)
    {
        Items.Add(item);
        ListItems();
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
    }
    public void ListItems()
    {
        // Xoá tất cả các item con một cách an toàn
        for (int i = ItemContent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(ItemContent.GetChild(i).gameObject);
        }

        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
        SetInventoryItems();
    }

    /*
        public void ListItems()
        {
            //clean content before open
            foreach (Transform item in ItemContent)
            {
                Destroy(item.gameObject);
            }

            foreach (var item in Items)
            {
                GameObject obj = Instantiate(InventoryItem, ItemContent);
                var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
                var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

                itemName.text = item.itemName;
                itemIcon.sprite = item.icon;
            }
            SetInventoryItems();
        }*/
    /*public void SetInventoryItems()
    {
        ItemsController = ItemContent.GetComponentsInChildren<ItemController>();

        for (int i = 0; i < Items.Count; i++)
        {
            ItemsController[i].AddItem(Items[i]);
        }

    }*/
    /*public void SetInventoryItems()
    {
        ItemsController = ItemContent.GetComponentsInChildren<ItemController>();

        if (ItemsController.Length != Items.Count)
        {
            Debug.LogError($"Mismatch: ItemsController.Length = {ItemsController.Length}, Items.Count = {Items.Count}");
        }

        for (int i = 0; i < ItemsController.Length; i++)
        {
            if (ItemsController[i] == null)
            {
                Debug.LogError($"ItemsController[{i}] is NULL!");
                continue;
            }

            if (i < Items.Count)
            {
                ItemsController[i].AddItem(Items[i]);
            }
            else
            {
                Debug.LogError($"Index {i} out of range for Items.Count = {Items.Count}");
            }
        }
    }*/
    public void SetInventoryItems()
    {
        ItemsController = ItemContent.GetComponentsInChildren<ItemController>();

        if (ItemsController.Length != Items.Count)
        {
            Debug.LogError($"Mismatch: ItemsController.Length = {ItemsController.Length}, Items.Count = {Items.Count}. Check ItemContent cleanup.");
            return;
        }

        for (int i = 0; i < ItemsController.Length; i++)
        {
            ItemsController[i].AddItem(Items[i]);
        }
    }

}
