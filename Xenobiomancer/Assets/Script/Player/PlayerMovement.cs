using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public bool MoveCheck;

    public InputHandler InputHandler;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2D;
    private Vector2 targetPosition;

    public float MoveSpeed = 5f;
    


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateMovement();
    }

    public void MovementCheck()
    {
        //check if the "W" key has been pressed
        MoveCheck = InputHandler.GetMovementKeyDown();
    }


    public void Move(Vector2 targetPos)
    {

        IsMovingForward(targetPos);
        
        targetPosition = targetPos;
        
    }


    private void IsMovingForward(Vector2 targetPos)
    {
        //check if the player is moving forward or backward

        float dist = targetPos.x - transform.position.x;

        if (dist < 0 )
        {
            spriteRenderer.flipX = true;
        }
        else if (dist > 0)
        {
            spriteRenderer.flipX = false;
        }


    }

    private void UpdateMovement()
    {
        //updating the movement of the character so that the character will move smooothly
        if (Vector2.Distance(rigidbody2D.position, targetPosition) > 0.1f)
        {
            
            Vector2 positionToMoveTo = Vector2.MoveTowards(rigidbody2D.position, targetPosition, MoveSpeed * Time.deltaTime);

            
            rigidbody2D.MovePosition(positionToMoveTo);
        }
    }

}
