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
    [SerializeField] private Grid grid;
    private Player player;
    private bool playerIsNearby;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        SensePlayer();

        if (playerIsNearby && Input.GetKeyUp(KeyCode.E))
        {
            
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
