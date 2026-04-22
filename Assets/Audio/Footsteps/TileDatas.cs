using UnityEngine;
using UnityEngine.Tilemaps;

public enum FloorType
{
    Grass,
    Dirt,
    Wood,

}

[CreateAssetMenu(fileName = "TileDatas", menuName = "Scriptable Objects/TileDatas")]
public class TileDatas : ScriptableObject
{
    public TileBase[] tiles;
    public AudioClip[] clip;
    public FloorType floorType;

}
