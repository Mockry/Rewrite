using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectControlMethod : MonoBehaviour
{
    public PlayerController thePlayer;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Allows the game to switch between Mouse and joypad controls

        //Detect Mouse input
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
            thePlayer.useController = false;
        if (Input.GetAxisRaw("Mouse X") != 0.0f || Input.GetAxisRaw("Mouse Y") != 0.0f)
        thePlayer.useController = false;

        //Detect Controller Input
        if (Input.GetAxisRaw("RightHorizontal") != 0 || Input.GetAxisRaw("RightVertical") != 0.0f)
            thePlayer.useController = true;

    }
}
