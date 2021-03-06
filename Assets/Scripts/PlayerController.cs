﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody myRigidbody;

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    // sets Camera to the scene's main camera
    private Camera mainCamera;
    // lets you set which gun is being controlled
    //not actually used in this version
    public GunController theGun;
    //controls what input the update looks for
    public bool useController;

    //Putting these on the player as killing the enemies also destroys their audioSource
    //so they can't play the sound
    [SerializeField] private AudioSource enemyDeath;
    [SerializeField] private AudioSource catDeath;
    [SerializeField] private AudioSource batDeath;
    [SerializeField] private AudioSource sheepDeath;

    // Start is called before the first frame update
    void Start()
    {
        // sets myRigidBody to the rigid body attached to
        // whatever object the script is attached to
        myRigidbody = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType <Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // detects input from the keyboard and multiplies it by 
        // the movement speed to get velocity
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput * moveSpeed;

        //Rotate with mouse
        if (!useController)
        {
            // Creates a line from the camera to the mouse pointer
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Creates a plane facing up at the default position
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            //If the ray hits the groundplane it sets the rayLength to be the distance
            // from the camera to that point (where the mouse is in this case)
            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                //gets the point where the ray is intersecting the plane
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);

                //displays a line from the camera to the plane to show how it works
                //Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

                //makes the object face the mousepointer but adjusted
                //to have the same vertical as the object (player)
                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }
            //Shoots if the shoot button is held down
            if (Input.GetMouseButtonDown(0))
                theGun.isFiring = true;
            //doesn't shoot if it isnt
            if (Input.GetMouseButtonUp(0))
                theGun.isFiring = false;
          
        } // end of if(!usecontroller)

        //rotate and fire with controller
        if(useController)
        {   //checks for rotation from the right stick using the input axes I set up
            Vector3 playerDirection = Vector3.right * Input.GetAxisRaw("RightHorizontal") + Vector3.forward * -Input.GetAxisRaw("RightVertical");
            // rotates the player if there is any input from the right stick
            if(playerDirection.sqrMagnitude > 0.0f)
            {
                transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
            }
            // button 5 is the right bumper on an xbox controller
            if (Input.GetKeyDown(KeyCode.Joystick1Button5))
                theGun.isFiring = true;
            if (Input.GetKeyUp(KeyCode.Joystick1Button5))
                theGun.isFiring = false;
        }

    }

    // Updates at a fixed interval that isnt dependant on framerate (.02 seconds by default)
    private void FixedUpdate()
    {
        // updates the position with move velocity
        myRigidbody.velocity = moveVelocity;
    } 
    // This should probably be some sort of switch statement
    // but its the last thing I did and I didn't want to complicate it

     //When an enemy dies they call this and pass in their enemyType
    public void playSound(string enemyType)
    {
        if (enemyType == "basic")
            enemyDeath.Play();
        if (enemyType == "cat")
            catDeath.Play();
        if (enemyType == "bat")
            batDeath.Play();
        if (enemyType == "sheep")
            sheepDeath.Play();
    }
     
}
