using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;
    // Start is called once before the first frame update
    public AudioClip pickupSound;

    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);

            Item item = collision.GetComponent<Item>();
            if(item != null)
            {
                //Add item inventory        
                bool itemAdded = inventoryController.AddItem(collision.gameObject);

                if (itemAdded)
                {
                    item.Pickup();
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
