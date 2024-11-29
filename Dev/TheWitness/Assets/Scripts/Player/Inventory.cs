using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    #region Properties

    private List<Item> items = new List<Item>();

    #endregion

    #region Methods

    public void AddItem(Item item)
    {
        if (items.Contains(item)) return;

        items.Add(item);
        GameManager.instance.InventoryUI.AddItem(item);
        Debug.Log("Item of name : " + item.name + " has been stored!");
    }

    public Item GetItem(int id)
    {
        if (id > 0 && id < items.Count)
        {
            return items[id];
        }
        return null;
    }

    #endregion
}
