using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{

    void PickUp();
    void Drop();
    void Purchase();
    void Sell();
    void Use();
    GameObject Create();
}
