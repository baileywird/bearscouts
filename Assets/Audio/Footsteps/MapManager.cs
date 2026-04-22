using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Tilemap map;

    [SerializeField] private List<TileDatas> tileDatas;

    private Dictionary<TileBase, TileDatas> dataFromTiles;

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileDatas>();
        foreach (var tileData in tileDatas)
        {
            foreach(var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    public AudioClip GetCurrentFloorClip(Vector2 worldPosition)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);
        TileBase tile = map.GetTile(gridPosition);

        int index = Random.Range(0, dataFromTiles[tile].clip.Length);
        AudioClip currentFloorClip = dataFromTiles[tile].clip[index];

        return currentFloorClip;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
