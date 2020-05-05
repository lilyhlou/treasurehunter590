using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class HeadVRSimulator : MonoBehaviour
{
    public float translationalSpeed=0.1f;
    public float rotationalSpeed=2.0f;
    public Camera aerialViewCam;
    public bool shouldLockMouse=true;
    // Start is called before the first frame update
    void Start()
    {
        GameObject ovrCamRigGO=this.transform.parent.transform.parent.gameObject;
        ovrCamRigGO.GetComponent<OVRCameraRig>().enabled=false;
        ovrCamRigGO.GetComponent<OVRManager>().enabled=false;
        ovrCamRigGO.GetComponent<OVRHeadsetEmulator>().enabled=false;
        Transform possibleOVRPlayCont=ovrCamRigGO.transform.parent;
        if (possibleOVRPlayCont != null){
            possibleOVRPlayCont.gameObject.GetComponent<CharacterController>().enabled=false;
            possibleOVRPlayCont.gameObject.GetComponent<OVRPlayerController>().enabled=false;
            possibleOVRPlayCont.gameObject.GetComponent<OVRSceneSampleController>().enabled=false;
            possibleOVRPlayCont.gameObject.GetComponent<OVRDebugInfo>().enabled=false;
        } else{
            print("not using player controller... probably just rig");
        }
        this.gameObject.tag="Untagged";
        this.GetComponent<Camera>().enabled=false;
        aerialViewCam.gameObject.tag="MainCamera";
        aerialViewCam.transform.parent=this.transform;
        aerialViewCam.transform.localPosition=new Vector3(0, aerialViewCam.transform.localPosition.y, 0);
        aerialViewCam.transform.eulerAngles=new Vector3(aerialViewCam.transform.eulerAngles.x, this.transform.eulerAngles.y, aerialViewCam.transform.eulerAngles.z);
        //hit escape to unlock
        Cursor.lockState=shouldLockMouse?CursorLockMode.Locked:CursorLockMode.None;
    }
 
    // Update is called once per frame
    void Update()
    {
        this.transform.position += (Input.GetKey(KeyCode.W)?1:Input.GetKey(KeyCode.S)?-1:0)* translationalSpeed* new Vector3(this.transform.forward.x,0,this.transform.forward.z) + 
        (Input.GetKey(KeyCode.A)?-1:Input.GetKey(KeyCode.D)?1:0)* translationalSpeed* new Vector3(this.transform.right.x,0,this.transform.right.z);
        //only yaw should change for aerial view
        //only using mouse x b/c left/right makes more sense for aerial yaw than up/down
        this.transform.eulerAngles=new Vector3(0, this.transform.eulerAngles.y+Input.GetAxis("Mouse X")*rotationalSpeed, 0);
    }
}
 
