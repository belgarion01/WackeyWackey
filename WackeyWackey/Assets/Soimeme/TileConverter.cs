using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileConverter : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private TileBase obstacleTile;

    [SerializeField] private GameObject obstablePrefab;

    private Vector2 offsetToCenter = new Vector2(0.5f, 0.5f);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ConvertTiles(true);
        }
    }

    public void ConvertTiles(bool debug)
    {
        Transform root = new GameObject("Obstacles").transform;
        foreach(Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if(tile == obstacleTile)
            {
                GameObject obstacle = Instantiate(obstablePrefab, (Vector2Int)pos + offsetToCenter, Quaternion.identity, root);
                obstacle.GetComponent<MeshRenderer>().enabled = debug;
                tilemap.SetTile(pos, null);
            }
        }
    }
}
