using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject player;
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        bulletPrefab = Resources.Load<GameObject>("BulletPrefab");
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootBullet();
        }
    }

    void HandleMovement()
    {
        // Basic 2D movement using the arrow keys
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);
        player.transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    void ShootBullet()
    {
        // Instantiate the bullet prefab at the player's position
        GameObject bullet = Instantiate(bulletPrefab, player.transform.position, Quaternion.identity);

        // Assuming the bullet should move forward along the x-axis
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0f);
    }
}

