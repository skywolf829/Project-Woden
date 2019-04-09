using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    public int inventorySlots;

    [SerializeField]
    public List<Item> items;
    [SerializeField]
    public List<int> stackSize;

    public void AddItem(Item item)
    {
        bool added = false;
        for(int i = 0; i < items.Count && !added; i++)
        {
            if (items[i].GetType().Equals(item.GetType()))
            {
                if (items[i].maxStack > stackSize[i])
                {
                    stackSize[i]++;
                    added = true;
                }
            }
        }
        if(items.Count < inventorySlots)
        {
            items.Add(item);
        }
    }

    public void UseItem(Item item)
    {
        item.Use();
        RemoveItem(item);
    }
    public void UseItem(int spot)
    {
        items[spot].Use();
        RemoveItem(spot);
    }
    public void RemoveItem(Item item)
    {
        int spot = items.IndexOf(item);
        RemoveItem(spot);
    }

    public void RemoveItem(int spot)
    {
        if (stackSize[spot] > 1)
        {
            stackSize[spot]--;
        }
        else
        {
            items.RemoveAt(spot);
        }
    }
}
