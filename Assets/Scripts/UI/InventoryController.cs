using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;


public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab; 
    public int slotCount;
    public GameObject[] itemPrefabs; //test

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PopulateInventory()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //center item within slot
                slot.currentItem = item;
            }
        }
    }

}
