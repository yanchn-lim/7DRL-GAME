using Patterns;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UpgradeStation
{
    public class UpgradeStationBehaviour : MonoBehaviour
    {
        [SerializeField] private UpgradeUI upgradeUI;
        [SerializeField] private UpgradeStationData data;
        [SerializeField] private TextMeshProUGUI information;
        private Player player;
        private bool playerIsNearby;

        public UpgradeStationData Data { get => data; }

        private void Awake()
        {
            //might need to change this later since this a bad way to find player
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            
        }

        private void Update()
        {
            SensePlayer();

            if (playerIsNearby && Input.GetKeyUp(KeyCode.E))
            {
                upgradeUI.ShowUpgradeUI();
            }
        }

        private void SensePlayer()
        {
            float distance = Vector2.Distance(player.transform.position, transform.position);
            if (distance <= data.ToggleRadius)
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
}