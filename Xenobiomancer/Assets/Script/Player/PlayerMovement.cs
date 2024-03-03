using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private float horizontal_Input;
    private float vertical_Input;
    HandleInputs handleInputs = new HandleInputs();


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleInputs()
    {
        horizontal_Input = handleInputs.GetHorizontalInput();
        vertical_Input = handleInputs.GetVerticalInput();
    
    }


    public void Move()
    {
       
    }

}
