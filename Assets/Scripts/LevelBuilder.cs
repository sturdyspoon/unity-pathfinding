using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelBuilder : MonoBehaviour
{
    public int width = 100;
    public int height = 100;
    public float perlinScale = 10f;

    public Tile ground;
    public Tile wall;

    Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        PerlinNoiseGrid noise = new PerlinNoiseGrid(width, height, perlinScale);

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                Tile t = (noise[i, j] < 0.5) ? ground : wall;
                tilemap.SetTile(new Vector3Int(i, j, 0), t);
            }
        }

    }
}
