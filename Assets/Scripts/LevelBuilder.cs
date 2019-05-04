using System.Collections;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelBuilder : MonoBehaviour
{
    public int width = 100;
    public int height = 100;
    public float perlinScale = 10f;
    public float wallThreshold = 0.5f;
    public int minPathLength = 10;
    public Tile wall;
    public float duration = 1f;
    public bool findPathOpenCell = true;

    public LineRenderer lineRenderer;
    public Transform startCircle;
    public Transform endCircle;

    Tilemap tilemap;
    List<Vector3Int> wallTiles;
    List<Vector3Int> groundTiles;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        wallTiles = new List<Vector3Int>();
        groundTiles = new List<Vector3Int>();

        PerlinNoiseGrid noise = new PerlinNoiseGrid(width, height, perlinScale);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3Int pos = new Vector3Int(i, j, 0);

                if (noise[i, j] < wallThreshold)
                {
                    tilemap.SetTile(pos, wall);
                    wallTiles.Add(pos);
                }
                else
                {
                    groundTiles.Add(pos);
                }
            }
        }

        StartCoroutine(NextPath());
    }

    IEnumerator NextPath()
    {
        Vector3 start;
        Vector3 end;
        List<Vector3> path;

        do
        {
            start = tilemap.GetCellCenterWorld(RandomElement(groundTiles));

            List<Vector3Int> list = findPathOpenCell ? groundTiles : wallTiles;
            end = tilemap.GetCellCenterWorld(RandomElement(list));

            path = AStar.FindPathClosest(tilemap, start, end);
        } while (start == end || path == null || path.Count < minPathLength);
        path.Add(end);

        lineRenderer.positionCount = path.Count;
        lineRenderer.SetPositions(path.ToArray());
        startCircle.position = start;
        endCircle.position = end;

        yield return new WaitForSeconds(duration);

        StartCoroutine(NextPath());
    }

    Vector3Int RandomElement(List<Vector3Int> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}
