using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class TreasureHunter : MonoBehaviour
{
    // Start is called before the first frame update
    //public Collectible[] collectibles;
    public TreasureHunterInventory inventory;
    //public Target[] collectiblesInScene;
    public List<Target> collectiblesInScene;

    public StringIntDictionary strDict;

    public TargetIntDictionary collDict;

    int totalPoints = 0;
    int totalItems = 0;
    int distance = 3;
    int angle = 60;
    float d;
    bool isSpawned=false; // change from false to true in control version of game without distractors
    Vector3 prevForwardVector;
    Vector3 prevLocation;
    Vector3 playerPosNoY;
    Vector3 PEPosNoY;
    Vector3 posTarget;
    Vector3 targetOffset;
    Vector3 pointA;
    Vector3 pointB;
    Vector3 AngleA;
    Vector3 AngleB;

    float prevYawRelativeToCenter;
    public Camera player;
    public GameObject PE;
    public GameObject Cursor;
    public LayerMask collectiblesMask;
    public TextMesh userScore;
    public TextMesh positionDebugPE;
    public TextMesh positionDebugPlayer;
    public GameObject boundingCube;
    Quaternion rotation;

    private Vector3 rotateAxis = new Vector3(0, 1, 0);
    public Target targetObject;
    public GameObject corner1;
    public GameObject corner2;
    public GameObject corner3;
    public GameObject corner4;
    Renderer rend;

    CharacterController controller;


    //public GameObject collectible1;
    void Start()
    {
        d = D(player.transform.position + prevForwardVector, player.transform.position, player.transform.position + player.transform.forward);
        prevForwardVector = player.transform.forward;
        prevYawRelativeToCenter = angleBetweenVectors(player.transform.forward, PE.transform.position - player.transform.position);
        prevLocation=player.transform.position;
        rotation = PE.transform.rotation;
        controller = GetComponent<CharacterController>();


    } //end of start

    // Update is called once per frame
    void Update()
    {
        PE.transform.rotation = rotation;
        var howMuchUserRotated = angleBetweenVectors(prevForwardVector, player.transform.forward);
        var directionUserRotated = (D(player.transform.position + prevForwardVector, player.transform.position, player.transform.position + player.transform.forward) < 0) ? -1 : 1;
        
        var deltaYawRelativeToCenter = prevYawRelativeToCenter - angleBetweenVectors(player.transform.forward, PE.transform.position - player.transform.position);
        playerPosNoY.Set(player.transform.position.x, 0, player.transform.position.z);
        PEPosNoY.Set(PE.transform.position.x,0,PE.transform.position.z);
        var distanceFromCenter = (playerPosNoY - PEPosNoY).magnitude;
        var longestDimensionOfPE = 2.0f;
        //var howMuchToAccelerate=((deltaYawRelativeToCenter<0)? -decelerateThreshold [.13]: accelerateThreshold[.30]) * howMuchUserRotated * directionUserRotated * clamp(distanceFromCenter/longestDimensionOfPE/2,0,1);
        var howMuchToAccelerate = ((deltaYawRelativeToCenter < 0) ? -.13f : .30f) * howMuchUserRotated * directionUserRotated * Mathf.Clamp(distanceFromCenter / longestDimensionOfPE / 2, 0, 1);
        if (Mathf.Abs(howMuchToAccelerate) > 0)
        {
            PE.transform.RotateAround(player.transform.position, rotateAxis, howMuchToAccelerate);
        }

        var trajectoryVector = player.transform.position-prevLocation;
        trajectoryVector.Set(trajectoryVector.x,0,trajectoryVector.z);
        //var howMuchToTranslate = Vector3.Normalize(trajectoryVector)*.004f;
        var howMuchToTranslate = Vector3.Normalize(trajectoryVector)*.004f;
        PE.transform.position-=howMuchToTranslate;

        if (Input.GetKeyDown("1")){
            Collider[] overlappingThings = Physics.OverlapSphere(Cursor.transform.position, .5f, collectiblesMask);
            if (overlappingThings.Length > 0)
            {
                updateScore(getClosestHitObject(overlappingThings));
                Destroy(getClosestHitObject(overlappingThings).gameObject);
            }
        }

        if (Input.GetKeyDown("2"))
        {
            //Transform cameraTransform = Camera.main.transform;
            RaycastHit HitInfo;
            if (Physics.Raycast(player.transform.position, player.transform.forward, out HitInfo, 5000.0f, collectiblesMask))
            {
                HitInfo.collider.gameObject.GetComponentInParent<Target>().Health -= 1;
                if(HitInfo.collider.gameObject.GetComponentInParent<Target>().Health < 1){
                    Destroy(HitInfo.collider.gameObject);

                    isSpawned=false;
                }
                //updateScore(HitInfo.collider.gameObject.GetComponentInParent<Target>()); 
            }
            print("Pressed 2");
        }

        /*if (Input.GetKeyDown("2")){
            forceGrab(false);
        }*/


        rend = PE.GetComponent<Renderer>();
        Vector3 minBound = rend.bounds.min;//*.75f;
        Vector3 maxBound= rend.bounds.max;//*.75f;
        string output = "Rend bounds max: ";
        output += maxBound.ToString("F3");
        output += "\nRend bounds min "; 
        output += minBound.ToString("F3");
        output += "\nPlayer position: "; 
        output += player.transform.position.ToString("F3");
        output += "\n"; 
        //userScore.text = output;
        //Debug.Log(rend.bounds.max);
        //Debug.Log(rend.bounds.min);

        string output2 = corner1.transform.position.ToString("F3");
        output2 += "\n"; 
        output2 += corner2.transform.position.ToString("F3");
        output2 += "\n"; 
        output2 += corner3.transform.position.ToString("F3");
        output2 += "\n"; 
        output2 += corner4.transform.position.ToString("F3");

        

        prevForwardVector = player.transform.forward;
        prevYawRelativeToCenter = angleBetweenVectors(player.transform.forward, PE.transform.position - player.transform.position);
        prevLocation=player.transform.position;
        rotation = PE.transform.rotation;
        PE.transform.position = new Vector3(PE.transform.position.x, .5f,PE.transform.position.z);

        output += player.transform.position.x > maxBound.x ? "Player X > maxbound X" : "Player X < maxbound X";
        output += "\n"; 
        output += player.transform.position.z > maxBound.z ? "Player Z > maxbound Z" : "Player Z < maxbound Z";
        output += "\n"; 
        output += player.transform.position.x < minBound.x ? "Player X < minbound X" : "Player X > minbound X";
        output += "\n"; 
        output += player.transform.position.z < minBound.z ? "Player Z < minbound Z" : "Player Z < minbound Z";
        output += "\n"; 




        //if(player.transform.position.x > maxBound.x || player.transform.position.z > maxBound.z || player.transform.position.x < minBound.x || player.transform.position.z < minBound.z){
        if(!boundingCube.GetComponent<Collider>().bounds.Contains(player.transform.position)){   
            output += "\n Outside"; 
            controller.enabled = false; // Turn off movement

            if(!isSpawned){
                AngleA = Quaternion.AngleAxis(angle, player.transform.up) * player.transform.forward;
                AngleB = Quaternion.AngleAxis(-angle, player.transform.up) * player.transform.forward;
                Vector3[] points = new Vector3[2];
                RaycastHit HitInfo;
                if (Physics.Raycast(player.transform.position, AngleA, out HitInfo, (float)distance))
                {
                    pointA = HitInfo.point;
                }else{
                    pointA = player.transform.position + AngleA * distance;
                }
                if (Physics.Raycast(player.transform.position, AngleB, out HitInfo, (float)distance))
                {
                    pointB = HitInfo.point;
                }else{
                    pointB = player.transform.position + AngleB * distance;
                }
                pointA.y = 1f;
                pointB.y = 1f;
                if(directionUserRotated < 0){
                    Target spawnTarget = Instantiate(targetObject, pointA, Quaternion.identity);
                    points[0] = pointA;
                    points[1] = pointB;
                    spawnTarget.SendMessage("setPoints", points);
                }
                else{
                    Target spawnTarget = Instantiate(targetObject, pointB, Quaternion.identity);
                    points[0] = pointB;
                    points[1] = pointA;
                    spawnTarget.SendMessage("setPoints", points);
                }
                //Instantiate(targetObject, new Vector3(0,1,0), Quaternion.identity);
                isSpawned=true;
            }
        }
        else{
            output += "\n Inside";  
            controller.enabled = true; // Turn on movement again
         
        }
        string debugPos = player.transform.position.ToString("F3");
        debugPos += "\n";
        debugPos += PE.transform.position.ToString("F3");
        positionDebugPE.text = output;
        //positionDebugPlayer.text = debugPos;
        userScore.text = output2;
        /*if(boundingCube.GetComponent<Collider>().bounds.Contains(player.transform.position)){
            positionDebugPlayer.text = "true";
        }else{
            positionDebugPlayer.text = "false";
        }*/
        //print(boundingCube.GetComponent<Collider>().bounds.Contains(player.transform.position));
        
    } //end of update

    public float D(Vector3 A, Vector3 B, Vector3 C)
    { //right < 0 = true, left > 0 = false
        return (A.x - B.x) * (C.z - B.z) - (A.z - B.z) * (C.x - B.x);
    }

    public float angleBetweenVectors(Vector3 A, Vector3 B)
    {
        A.Set(A.x, 0, A.z);
        B.Set(B.x, 0, B.z);
        return 180.0f * Mathf.Acos(Vector3.Dot(Vector3.Normalize(A), Vector3.Normalize(B))) / Mathf.PI;
    }

    public void updateScore(Target treasure)
    {
        totalPoints += treasure.Value;
        totalItems++;
        string name = treasure.Name;
        var tres2 = Resources.Load<Target>(name);
        if (!collDict.ContainsKey(tres2))
        {
            collDict.Add(tres2, 1);
        }
        else
        {
            collDict[tres2] = collDict[tres2] + 1;
        }
        string message = "Item | Count | Value\n";
        foreach (Target key in collDict.Keys)
        {
            message += key.Name + " | " + collDict[key] + " | " + key.Value + "\n";
        }
        message += "You have " + totalItems + " items worth " + totalPoints + " points! -Evan & Lily";
        userScore.text = message;
    }
    Target getClosestHitObject(Collider[] hits)
    {
        float closestDistance = 10000.0f;
        Target closestObjectSoFar = null;
        foreach (Collider hit in hits)
        {
            Target c = hit.gameObject.GetComponentInParent<Target>();
            if (c)
            {
                float distanceBetweenHandAndObject = (c.gameObject.transform.position - Cursor.transform.position).magnitude;
                if (distanceBetweenHandAndObject < closestDistance)
                {
                    closestDistance = distanceBetweenHandAndObject;
                    closestObjectSoFar = c;
                }
            }
        }
        return closestObjectSoFar;
    }

    /*public int directionUserRotated(int d){ //right < 0 = true, left > 0 = false
        return d<0?-1:1;
    }*/
    /*public void attachGameObjectToAChildGameObject(GameObject GOToAttach, GameObject newParent, AttachmentRule locationRule, AttachmentRule rotationRule, AttachmentRule scaleRule, bool weld){
        GOToAttach.transform.parent=newParent.transform;
        handleAttachmentRules(GOToAttach,locationRule,rotationRule,scaleRule);
        if (weld){
            simulatePhysics(GOToAttach,Vector3.zero,false);
        }
    }
    void letGo(){
        if (thingIGrabbed){
            Collider[] overlappingThingsWithLeftHand=Physics.OverlapSphere(leftPointerObject.transform.position,0.01f,collectiblesMask);
            detachGameObject(thingIGrabbed.gameObject,AttachmentRule.KeepWorld,AttachmentRule.KeepWorld,AttachmentRule.KeepWorld);
            simulatePhysics(thingIGrabbed.gameObject,(leftPointerObject.gameObject.transform.position-previousPointerPos)/Time.deltaTime,true);
            thingIGrabbed=null;
        }
        }

      public static void detachGameObject(GameObject GOToDetach, AttachmentRule locationRule, AttachmentRule rotationRule, AttachmentRule scaleRule){
        //making the parent null sets its parent to the world origin (meaning relative & global transforms become the same)
        GOToDetach.transform.parent=null;
        handleAttachmentRules(GOToDetach,locationRule,rotationRule,scaleRule);
    }

    public static void handleAttachmentRules(GameObject GOToHandle, AttachmentRule locationRule, AttachmentRule rotationRule, AttachmentRule scaleRule){
        GOToHandle.transform.localPosition=
        (locationRule==AttachmentRule.KeepRelative)?GOToHandle.transform.position:
        //technically don't need to change anything but I wanted to compress into ternary
        (locationRule==AttachmentRule.KeepWorld)?GOToHandle.transform.localPosition:
        new Vector3(0,0,0);

        //localRotation in Unity is actually a Quaternion, so we need to specifically ask for Euler angles
        GOToHandle.transform.localEulerAngles=
        (rotationRule==AttachmentRule.KeepRelative)?GOToHandle.transform.eulerAngles:
        //technically don't need to change anything but I wanted to compress into ternary
        (rotationRule==AttachmentRule.KeepWorld)?GOToHandle.transform.localEulerAngles:
        new Vector3(0,0,0);

        GOToHandle.transform.localScale=
        (scaleRule==AttachmentRule.KeepRelative)?GOToHandle.transform.lossyScale:
        //technically don't need to change anything but I wanted to compress into ternary
        (scaleRule==AttachmentRule.KeepWorld)?GOToHandle.transform.localScale:
        new Vector3(1,1,1);
    }

        public void simulatePhysics(GameObject target,Vector3 oldParentVelocity,bool simulate){
        Rigidbody rb=target.GetComponent<Rigidbody>();
        if (rb){
            if (!simulate){
                Object.Destroy(rb);
            } 
        } else{
            if (simulate){
                //there's actually a problem here relative to the UE4 version since Unity doesn't have this simple "simulate physics" option
                //The object will NOT preserve momentum when you throw it like in UE4.
                //need to set its velocity itself.... even if you switch the kinematic/gravity settings around instead of deleting/adding rb
                Rigidbody newRB=target.AddComponent<Rigidbody>();
                newRB.velocity=oldParentVelocity;
            }
        }
    }
        void forceGrab(bool pressedA){
        RaycastHit outHit;
        //notice I'm using the layer mask again
        if (Physics.Raycast(leftPointerObject.transform.position, leftPointerObject.transform.forward, out outHit, 100.0f,collectiblesMask))
        {  
            AttachmentRule howToAttach=pressedA?AttachmentRule.KeepWorld:AttachmentRule.SnapToTarget;
            attachGameObjectToAChildGameObject(outHit.collider.gameObject,leftPointerObject.gameObject,howToAttach,howToAttach,AttachmentRule.KeepWorld,true);
            thingIGrabbed=outHit.collider.gameObject.GetComponent<Target>();
        }
    }*/

}
