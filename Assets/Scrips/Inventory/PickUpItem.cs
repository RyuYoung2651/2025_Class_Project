using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : InteractableObject
{
    public ItemData ItemData;
    public int amount = 1;


    public override void Interact()
    {
        base.Interact();

        if(InventoryManager.Instance != null)
        {
            bool added = InventoryManager.Instance.AddItem(ItemData, amount);

            if(added)
            {
                Destroy(gameObject);
            }
        }
    }


}
