using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

public class ItemData : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public int maxStack = 99;


    
}
