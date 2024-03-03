using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Camera mainCamera;

    public bool MoveCheck;

    public float ClampValue;

    HandleInputs handleInputs = new HandleInputs();



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //Move();
    }

    public void MovementCheck()
    {
        
        MoveCheck = handleInputs.GetMovementKeyDown();
    }


    public void Move()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPos = transform.position;

        float minDistx = playerPos.x - ClampValue;
        float maxDistx = playerPos.x + ClampValue;

        float minDisty = playerPos.y - ClampValue;
        float maxDisty = playerPos.y + ClampValue;

        float clampedx = Mathf.Clamp(mouseWorldPos.x, minDistx, maxDistx);
        float clampedy = Mathf.Clamp(mouseWorldPos.y, minDisty, maxDisty);


        Vector3 targetPos = new(clampedx,clampedy, 0);
        
        
        
        transform.position = targetPos;
    }

}
