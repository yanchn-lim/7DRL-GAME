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
            //destroy door
            foreach (var item in doorPos)
            {
                map.SetTile(item, null);
            }
        }
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
