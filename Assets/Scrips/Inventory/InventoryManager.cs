using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Inventory Setting")]
    public int inventorySize = 20;
    public GameObject inventoryUI;
    public Transform itemSlotParent;
    public GameObject itemSlotPrefab;

    [Header("Input")]
    public KeyCode inventoryKey = KeyCode.I;
    private List<InventorySlot> slots = new List<InventorySlot>();
    private bool isInventoryOpen = false;


    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        CreatInventorySlots();
        inventoryUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(inventoryKey))
        {
            Toggleinventory();
        }
    }

    //인ㅂㅔㄴ토리슬ㄹㅗㅅ
    void CreatInventorySlots()
    {
        for(int i = 0; i < inventorySize; i++)
        {
            //프리팹ㅇㅡㄹㅗ 슬ㄹㅗㅅ생성
            GameObject slotObj = Instantiate(itemSlotPrefab, itemSlotParent);
            InventorySlot slot = slotObj.GetComponent<InventorySlot>();
        }
    }

    //인ㅂㅔㄴ토리 UI룰 열거나 닫ㄴㅡㄴ 함ㅅㅜ
    public void Toggleinventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    //아이템을 인ㅂㅔㄴ토르에 추가
    public bool AddItem(ItemData item, int amount = 1)
    {
        foreach(InventorySlot slot in slots)
        {
            if(slot.item == item && slot.amount < item.maxStack)
            {
                int spaceLeft = item.maxStack - slot.amount;
                int amountToAdd = Mathf.Min(spaceLeft, amount);
                slot.AddAmount(amountToAdd);

                if(amountToAdd < 0)
                {
                    return true;
                }
            }
        }
        foreach(InventorySlot slot in slots)
        {
            if(slot.item == null)
            {
                slot.SetItem(item, amount);
                return true;
            }
        }

        Debug.Log("인ㅂㅔ토리가 가드ㄱ 참");
        return false;
    }

    public void RemoveItem(ItemData item, int amount = 1)
    {
        foreach(InventorySlot slot in slots)
        {
            if(slot.item == item)
            {
                slot.RemoveAmount(amount);
                return;
            }
        }
    } 


    public int GetItemCount(ItemData item)
    {
        int count = 0;
        foreach(InventorySlot slot in slots)
        {
            if(slot.item == item)
            {
                count += slot.amount;
            }
        }
        return count;
    }
}
