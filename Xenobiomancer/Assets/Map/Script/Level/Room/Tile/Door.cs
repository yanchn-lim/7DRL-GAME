using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UpgradeStation;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{

    [SerializeField] private float doorRadius;
    [SerializeField] private TextMeshProUGUI information;
    private Player player;
    private bool playerIsNearby;
    public List<Vector3Int> doorPos;
    public Tilemap map;

    Vector3Int[] surrounding =
        { new (-1,-1,0),new (-1,0,0),new(-1,1,0),
          new(0,-1, 0 ),             new(0 ,1,0),
          new(1, -1,0 ),new(1,0,0),new(1,1,0)
        };
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        SensePlayer();

        if (playerIsNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("destroy door");

            Cascade();

            //destroy door
            foreach (var item in doorPos)
            {
                //map.SetTile(item, null);
                //map.SetColor(item, Color.red);
            }
        }
    }

    public void Cascade()
    {
        foreach (var item in surrounding)
        {
            TileData td = new();
            Vector3Int pos = Vector3Int.RoundToInt(transform.position) + item;
            TileBase tile = map.GetTile(pos);
            if(tile != null)
            {
                if (tile.name.Contains("Door"))
                {
                    map.GetTile(pos).GetTileData(pos, map, ref td);
                    map.SetTile(pos, null);
                    td.gameObject.GetComponent<Door>().Cascade();
                }
            }
        }
        map.SetTile(Vector3Int.RoundToInt(transform.position), null);

    }

    private void SensePlayer()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance <= doorRadius)
        {
            playerIsNearby = true;
            information.gameObject.SetActive(true);
        }
        else
        {
            playerIsNearby = false;
            information.gameObject.SetActive(false);
        }
    }
}
