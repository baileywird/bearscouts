using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab; //FIXME -- NEED SLOT PREFAB
    public int slotCount;
    public GameObject[] itemPrefabs; //test


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<slot>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //center item within slot
                slot.currentItem = item;
            }
        }
    }

}
