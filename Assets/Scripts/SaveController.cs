using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //define save location
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindObjectOfType<InventoryController>();

        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        { 
           playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
           inventorySaveData = inventoryController.GetInventoryItems()
            //, MAP BOUNDARY WHEN CREATED
        };
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if(File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;
            //, MAP BOUNDARY WHEN CREATED

            inventoryController.SetInventoryItems(saveData.inventorySaveData);
        }
        else
        {
            SaveGame(); //create new file
        }
    }
}
