using System.Collections;
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
    public GunController theGun;

    public bool useController;

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

            //If the ray hits the groundplane it sets the rayLenght to be the distance
            // from the camera to that point (where the mouse is in this case)
            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                //gets the point where the ray is intersecting the plane
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);

                // displays a line from the camera to the plane to show how it works
                //Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

                //makes the object face the mousepointer but adjusted to have the same vertical as the object (player)
                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }

            if (Input.GetMouseButtonDown(0))
                theGun.isFiring = true;

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
}
