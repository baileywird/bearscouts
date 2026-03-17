
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;
    // Start is called once before the first frame update
    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger hit: " + collision.gameObject.name);
        if (collision.CompareTag("Item"))
        {
            Debug.Log("Item tag confirmed");
            Item item = collision.GetComponentInParent<Item>();
            if (item != null)
            {
                Debug.Log("Item component found");
                bool itemAdded = inventoryController.AddItem(collision.gameObject);
                Debug.Log("Item added: " + itemAdded);
                if (itemAdded)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
