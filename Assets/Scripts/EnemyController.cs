using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody myRB;
    public float moveSpeed;

    //the enemy only needs to target the player and only the player has a PlayerController Script
    public PlayerController thePlayer;

    private float storedSpeed;
    private float freezeTimer;
    public float freezeLength;
    private bool timeFreeze;




    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        //searches the level for an object with PlayerController
        thePlayer = FindObjectOfType<PlayerController>();
        storedSpeed = moveSpeed;
    }

    void FixedUpdate()
    {
        myRB.velocity = (transform.forward * moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {   //finds the player position and makes the enemy face it
        //unlike the mouse pointer the enemy and player will be at the same height 
        transform.LookAt(thePlayer.transform.position);

        if (Input.GetMouseButtonDown(1) && timeFreeze == false)
        {
            timeFreeze = true;
            freezeTimer = freezeLength;
            moveSpeed = 0.0f;
        }
        if (freezeTimer > 0)
            freezeTimer -= Time.deltaTime;

        if (freezeTimer <= 0)
        {
            timeFreeze = false;
            moveSpeed = storedSpeed;
        }

    }   

    
}
