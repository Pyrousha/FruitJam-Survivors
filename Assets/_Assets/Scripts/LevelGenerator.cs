using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap grid;
    [SerializeField] private RuleTile tile;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cutoff;
    [SerializeField] private float scale;

    public void GenerateTerrain()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float val = Mathf.PerlinNoise((float)i / width * scale, (float)j / height * scale);
                if (val >= cutoff)
                    grid.SetTile(new Vector3Int(i, j, 0), tile);
            }
        }
    }

    public void ClearTerrain()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grid.SetTile(new Vector3Int(i, j, 0), null);
            }
        }
    }
}
