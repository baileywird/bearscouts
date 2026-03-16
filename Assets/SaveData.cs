using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SaveData
{
    public Vector3 playerPosition;
    //ADD MAP BOUNDARIES STRING WHEN WE HAVE THEM
    public List<InventorySaveData> inventorySaveData;
}
