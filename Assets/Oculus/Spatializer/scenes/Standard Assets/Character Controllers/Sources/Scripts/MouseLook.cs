using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;
	float translationalSpeed = .01F;
    
	CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

	public float speed = .01f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;


	void Update ()
	{
		        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes
 			moveDirection = (Input.GetKey(KeyCode.W)?1:Input.GetKey(KeyCode.S)?-1:0)* speed* new Vector3(this.transform.forward.x,0,this.transform.forward.z) + (Input.GetKey(KeyCode.A)?-1:Input.GetKey(KeyCode.D)?1:0)* speed* new Vector3(this.transform.right.x,0,this.transform.right.z);
            //moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            //moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

		/*
		this.transform.position += (Input.GetKey(KeyCode.W)?1:Input.GetKey(KeyCode.S)?-1:0)* translationalSpeed* new Vector3(this.transform.forward.x,0,this.transform.forward.z) + (Input.GetKey(KeyCode.A)?-1:Input.GetKey(KeyCode.D)?1:0)* translationalSpeed* new Vector3(this.transform.right.x,0,this.transform.right.z);
        //only yaw should change for aerial view
        //only using mouse x b/c left/right makes more sense for aerial yaw than up/down
*/

		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
			
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
	}
	
	void Start ()
	{
		characterController = GetComponent<CharacterController>();

		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	Cursor.lockState=CursorLockMode.Locked;

	}
}
