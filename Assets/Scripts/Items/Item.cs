using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IItem
{
    public string itemName;
    public string itemDescription;
    public int maxStack;
    public int cost;
    public int worth;
    public bool destroyOnUse;

    public Sprite shopSprite;
    public Sprite groundSprite;
    public Sprite inventorySprite;

    public Animation groundAnimation;
    public Animation shopAnimation;
    public Animation inventoryAnimation;
    
    public abstract void Drop();
    public abstract void PickUp();
    public abstract void Purchase();
    public abstract void Sell();
    public abstract void Use();
    public abstract GameObject Create();
}
